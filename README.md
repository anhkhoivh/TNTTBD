# TNTTBD

## Local Docker Desktop

Start Docker Desktop with Linux containers, then run from the repository root:

```powershell
docker compose up --build -d
docker compose ps
curl.exe http://localhost:4200/health
```

- UI: http://localhost:4200
- API through the UI proxy: http://localhost:4200/api
- Direct API (for diagnostics): http://localhost:5000/api
- pgAdmin: http://localhost:5050

Compose builds the API and UI from this checkout. The UI build uses the
`production,docker` Angular configurations: production optimizations with the
local Docker environment. Existing development and production environment files
remain unchanged. Browser API calls and relative photo upload URLs go through
the UI's Nginx `/api/` proxy to `api:8080`.

The API runs in Production mode and waits for PostgreSQL to become healthy.
The local Compose credentials are for development only; do not deploy them
unchanged to the NAS or expose these ports publicly.

### Builds behind a TLS-inspecting proxy

If the API build fails during `dotnet restore` with `NU1301` and
`UntrustedRoot`, the build container may not trust your network's HTTPS
inspection certificate. Use only a root CA verified by your administrator;
do not disable TLS validation.

The API Dockerfile accepts an optional `build_ca` BuildKit secret. Export your
approved root CA as a PEM/Base-64 `.crt` file outside this repository. On Windows,
select the approved root certificate in Certificate Manager and export it using
"Base-64 encoded X.509 (.CER)", saving the result with a `.crt` extension.
Then run in PowerShell, adjusting the certificate path:

```powershell
$env:DOCKER_BUILD_CA_CERT = "$env:USERPROFILE\.docker\certs\company-root.crt"
docker compose -f .\docker-compose.yml -f .\docker-compose.ca.yml up -d --build
```

Use the same `-f` arguments for subsequent Compose commands on this network.
The certificate is mounted only during API dependency restore and is not copied
into the runtime image. Publishing reuses the restored packages. Do not commit
the certificate. On networks without HTTPS inspection, including the NAS if it
does not use this proxy, use the original command without the CA override.

### Photo storage and diagnostics

`FileStorage__BasePath=/app/uploads` is an absolute container path mounted to
the persistent `uploads` Docker volume. PostgreSQL uses the separate `pgdata`
volume. Both survive container replacement and `docker compose down`.
Do not use `docker compose down -v` unless you intend to delete both volumes.

```powershell
docker compose logs --tail 100 api ui
docker compose exec api ls -R /app/uploads
docker compose down
```

For uploads, inspect both requests in browser DevTools: the photo-reference POST
and the multipart POST to `/api/files/upload/...`. Both should use
`http://localhost:4200`. Test initially with an Admin account and a small valid
JPEG or PNG. The API currently limits the whole upload request to 5 MiB;
Nginx permits 6 MiB so it does not impose a smaller limit.

Photo-reference creation explicitly inserts new photo entities rather than
letting EF infer their state from preassigned GUIDs. Uploads append photos,
including profile photos, without removing earlier uploads or rewriting unrelated
member fields or patron saints. Only explicit photo deletion removes a reference.
If the reference request returns 500, check the API logs for the exception and
match its trace ID to the response; unexpected exceptions are logged server-side.

Photo tabs use the API's `photoId`/`url` fields. API images are loaded through
authenticated Angular HTTP requests and displayed using temporary blob URLs,
which are released when the image component changes or is destroyed. External
mock images use ordinary image requests without API credentials.
A failed byte upload can still leave metadata because the photo reference is
saved first; a missing image is shown as a localized loading error.

Manage photos from the Photos tab on catechist and youth detail pages, not from
their edit forms. The purpose dropdown (Profile, Gallery, Document) creates image
requests only for the selected category; photo metadata still comes from the
member detail response. The upload dialog selects a file, purpose and visibility
(Public, Restricted, Private), then refreshes the tab and selects the uploaded
purpose. Upload is available to Admin and the linked owner, matching the file
upload endpoint's current permission rules. Visibility is stored as metadata;
this UI does not change the API's existing file-read access policy.
Click a thumbnail to view the full image in a dialog.

### Later deployment to Synology

Replace the uploads volume mount with a NAS bind mount such as
`/volume1/docker/tnttbd/uploads:/app/uploads` and ensure the container can write
to it. Back up that folder and PostgreSQL.

For a separate API hostname, set `FileStorage__BaseUrl` to the public origin
(for example `https://api.tnttbd.local`, without `/api`) and configure the UI
production environment and API CORS origins for the real hostnames. An empty
storage base URL generates same-origin relative URLs for local Docker.
The UI Dockerfile defaults to a production build when built outside this
local Compose configuration.