using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

// Configuration
var csvFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
var outputPath = Path.Combine(csvFolder, "import-data.sql");

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine($"CSV folder: {csvFolder}");
Console.WriteLine($"Output: {outputPath}");

// --- Known Vietnamese saint names (for splitting "GIUSE NGUYỄN VĂN A" → ChristName="Giuse", rest=family name) ---
var knownSaintNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{
    "MARIA", "GIUSE", "PHERO", "PHÊRÔ", "PHAOLO", "PHAOLÔ", "ANNA", "TERESA", "TÊRÊSA",
    "DOMINICO", "ĐÔMINICÔ", "ĐA MINH", "ĐAMINH", "GIOAN", "GIOAN BAOTIXITA", "GIOAN KIM",
    "AUGUSTINO", "AUGUSTINÔ", "ANTÔN", "ANTON", "VINCENTE", "VICENTÊ", "VINCENTÊ", "VINH SƠN",
    "MARCO", "MÁCCÔ", "MATCO", "MATTA", "MATTHIAS", "MICAEL", "MICAE",
    "PHANXICO", "PHANXICÔ", "PHANXICO XAVIE", "PHANXICÔ XAVIÊ",
    "LUCIA", "LOUIS", "INHAXIO", "ISAVE", "ISAVEL",
    "ANEDE", "ANÊĐÊ", "ANÊ", "CECILIA", "CÉCILIA", "CATHARINA", "CATARINA",
    "BENEDICTO", "GIERADO", "GIACOBE", "GIUSE MARIA",
    "MARIA CLARA", "MARIA GORETTY", "MARIA GORETY", "MARIA ANGELA",
    "MARIA - TERESA", "MARIA TÊRÊSA", "MARIA-TERESA",
    "ĐA MINH NGUYÊN", "PHAOLO TỊNH", "ĐÔMINICÔ SAVIÔ",
    "ANPHONGSO", "ANPHONGSÔ",
    "TÔMA", "SIMON", "PHILIPPHÊ",
    "ĐA-MINH", "ĐAMINH",
    "GIOAN.B", "JB.", "JB",
    "TERESA CALCUTTA",
    "AGATHA", "ROSA",
    "INHAXIO", "PHILIPPHÊ",
};

// Saint name abbreviation → full form mapping
var saintNameNormalization = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    ["GIOAN.B"] = "Gioan Baotixita",
    ["JB."] = "Gioan Baotixita",
    ["JB"] = "Gioan Baotixita",
};

// Female saint names for gender inference
var femaleSaintNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{
    "MARIA", "ANNA", "TERESA", "TÊRÊSA", "LUCIA", "ISAVE", "ISAVEL",
    "ANEDE", "ANÊĐÊ", "ANÊ", "CECILIA", "CÉCILIA", "CATHARINA", "CATARINA",
    "MARIA CLARA", "MARIA GORETTY", "MARIA GORETY", "MARIA ANGELA",
    "MARIA - TERESA", "MARIA TÊRÊSA", "MARIA-TERESA", "MATTA",
    "MARIA GORETTY", "ROSA", "ISAVE", "AGATHA", "TERESA CALCUTTA"
};

// Class file mapping: filename suffix → (ClassCode, ClassName)
var classMapping = new Dictionary<string, (string Code, string Name)>
{
    ["KTDB"] = ("KTDB", "Khai Tâm Dự Bị"),
    ["KT1"] = ("KT1", "Khai Tâm 1"),
    ["KT2A"] = ("KT2A", "Khai Tâm 2A"),
    ["KT2B"] = ("KT2B", "Khai Tâm 2B"),
    ["RL1A"] = ("RL1A", "Rước Lễ 1A"),
    ["RL1B"] = ("RL1B", "Rước Lễ 1B"),
    ["RL2A"] = ("RL2A", "Rước Lễ 2A"),
    ["RL2B"] = ("RL2B", "Rước Lễ 2B"),
    ["BD1A"] = ("BD1A", "Bao Đồng 1A"),
    ["BD1B"] = ("BD1B", "Bao Đồng 1B"),
    ["BD2A"] = ("BD2A", "Bao Đồng 2A"),
    ["BD2B"] = ("BD2B", "Bao Đồng 2B"),
    ["BD3"] = ("BD3", "Bao Đồng 3"),
    ["BD4"] = ("BD4", "Bao Đồng 4"),
    ["TS1A"] = ("TS1A", "Thêm Sức 1A"),
    ["TS1B"] = ("TS1B", "Thêm Sức 1B"),
    ["TS2A"] = ("TS2A", "Thêm Sức 2A"),
    ["TS2B"] = ("TS2B", "Thêm Sức 2B"),
    ["TS2C"] = ("TS2C", "Thêm Sức 2C"),
    ["VD1"] = ("VD1", "Vào Đời 1"),
    ["VD2"] = ("VD2", "Vào Đời 2"),
};

// --- Storage ---
var catechists = new List<CatechistRecord>();
var catechistByName = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
var youths = new List<YouthRecord>();
var milestones = new List<MilestoneRecord>();
var contacts = new List<ContactRecord>();
var youthContactLinks = new List<YouthContactLink>();
var classCatechistAssignments = new List<ClassCatechistAssignment>();
var classYouthEnrollments = new List<ClassYouthEnrollment>();

var yearId = Guid.NewGuid();
var classIds = new Dictionary<string, Guid>();
foreach (var kv in classMapping)
{
    classIds[kv.Key] = Guid.NewGuid();
}

// --- Helper functions ---
string Esc(string? s) => s?.Replace("'", "''") ?? "";

string NullOrStr(string? s) => string.IsNullOrWhiteSpace(s) ? "NULL" : $"'{Esc(s)}'";

string NullOrDate(DateOnly? d) => d.HasValue ? $"'{d.Value:yyyy-MM-dd}'" : "NULL";

DateOnly? ParseDate(string? s)
{
    if (string.IsNullOrWhiteSpace(s) || s.Trim() == "0") return null;
    // Try DD/MM/YYYY
    if (DateTime.TryParseExact(s.Trim(), new[] { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy" },
        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
    {
        return DateOnly.FromDateTime(dt);
    }
    return null;
}

(string? ChristName, string FamilyName, string? MiddleName, string GivenName) SplitFullNameWithSaint(string fullText)
{
    fullText = fullText.Trim();
    if (string.IsNullOrWhiteSpace(fullText)) return (null, "", null, "");

    // Try to match known multi-word saint names first (longest match)
    string? christName = null;
    string remaining = fullText;

    var sortedSaints = knownSaintNames.OrderByDescending(s => s.Length).ToList();
    foreach (var saint in sortedSaints)
    {
        if (fullText.StartsWith(saint + " ", StringComparison.OrdinalIgnoreCase) ||
            fullText.Equals(saint, StringComparison.OrdinalIgnoreCase))
        {
            christName = saintNameNormalization.TryGetValue(saint, out var normalized) ? normalized : TitleCase(saint);
            remaining = fullText.Length > saint.Length ? fullText[(saint.Length + 1)..].Trim() : "";
            break;
        }
        // Handle "SAINT - " format (e.g., "ĐAMINH - NGUYỄN TRỌNG NGHĨA")
        if (fullText.StartsWith(saint + " - ", StringComparison.OrdinalIgnoreCase))
        {
            christName = saintNameNormalization.TryGetValue(saint, out var normalized) ? normalized : TitleCase(saint);
            remaining = fullText[(saint.Length + 3)..].Trim();
            break;
        }
    }

    // If no known saint matched but there's a dash separator
    if (christName == null && fullText.Contains(" - "))
    {
        var parts = fullText.Split(" - ", 2);
        christName = TitleCase(parts[0].Trim());
        remaining = parts[1].Trim();
    }

    return SplitVietnameseName(christName, remaining);
}

(string? ChristName, string FamilyName, string? MiddleName, string GivenName) SplitVietnameseName(string? christName, string nameStr)
{
    if (string.IsNullOrWhiteSpace(nameStr)) return (christName, "", null, "");

    var words = nameStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (words.Length == 1) return (christName, "", null, TitleCase(words[0]));
    if (words.Length == 2) return (christName, TitleCase(words[0]), null, TitleCase(words[1]));

    var family = TitleCase(words[0]);
    var given = TitleCase(words[^1]);
    var middle = string.Join(" ", words[1..^1].Select(TitleCase));
    return (christName, family, middle, given);
}

(string FamilyName, string? MiddleName, string GivenName) SplitName(string fullName)
{
    var words = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (words.Length == 0) return ("", null, "");
    if (words.Length == 1) return ("", null, TitleCase(words[0]));
    if (words.Length == 2) return (TitleCase(words[0]), null, TitleCase(words[1]));

    return (TitleCase(words[0]), string.Join(" ", words[1..^1].Select(TitleCase)), TitleCase(words[^1]));
}

string TitleCase(string s)
{
    if (string.IsNullOrWhiteSpace(s)) return s;
    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
}

bool IsFemale(string? christName)
{
    if (string.IsNullOrWhiteSpace(christName)) return false;
    return femaleSaintNames.Contains(christName.Trim());
}

string? CleanPhone(string? phone)
{
    if (string.IsNullOrWhiteSpace(phone)) return null;
    phone = phone.Trim().Replace(" ", "");
    if (phone == "0000000000" || phone == "0") return null;
    return phone;
}

string? CleanChristName(string? name)
{
    if (string.IsNullOrWhiteSpace(name)) return null;
    var upper = name.Trim().ToUpper();
    if (upper == "CHƯA RỬA TỘI" || upper == "CHUA RUA TOI") return null;
    return TitleCase(name.Trim());
}

// --- PHASE 1: Parse Catechist List ---
Console.WriteLine("\n--- Parsing catechist list ---");
var glvFile = Path.Combine(csvFolder, "THÔNG TIN THIẾU NHI  - DANH SÁCH GLV.csv");
var glvLines = File.ReadAllLines(glvFile, Encoding.UTF8);

// Skip header rows (first 7 lines are title/header)
for (int i = 7; i < glvLines.Length; i++)
{
    var cols = ParseCsvLine(glvLines[i]);
    if (cols.Length < 3) continue;

    var fullName = cols.Length > 2 ? cols[2]?.Trim() : null;
    if (string.IsNullOrWhiteSpace(fullName)) continue;

    var christName = cols.Length > 1 ? cols[1]?.Trim() : null;
    if (string.IsNullOrWhiteSpace(christName)) christName = null;
    else christName = TitleCase(christName);

    var isFemaleMarker = cols.Length > 3 && !string.IsNullOrWhiteSpace(cols[3]) &&
                         cols[3].Trim().Equals("X", StringComparison.OrdinalIgnoreCase);

    // Feast day DD/MM
    int? patronDay = null, patronMonth = null;
    string? patronSaintName = christName;
    if (cols.Length > 4 && !string.IsNullOrWhiteSpace(cols[4]))
    {
        var feastParts = cols[4].Trim().Split('/');
        if (feastParts.Length == 2 &&
            int.TryParse(feastParts[0], out var d) &&
            int.TryParse(feastParts[1], out var m))
        {
            patronDay = d;
            patronMonth = m;
        }
    }

    // Date of birth
    DateOnly? dob = cols.Length > 5 ? ParseDate(cols[5]) : null;

    // Phone
    string? phone = cols.Length > 6 ? CleanPhone(cols[6]) : null;

    // Split name
    var (family, middle, given) = SplitName(fullName);

    bool isMale = !isFemaleMarker;
    // Also infer from saint name if no marker
    if (!isFemaleMarker && christName != null && IsFemale(christName))
    {
        isMale = false;
    }

    var id = Guid.NewGuid();
    catechists.Add(new CatechistRecord(id, christName, family, middle, given, dob, isMale, phone,
        patronDay, patronMonth, patronSaintName));

    // Register by normalized key for matching
    var matchKey = fullName.ToUpper().Trim();
    catechistByName.TryAdd(matchKey, id);
    // Also register with saint name prefix for class header matching
    if (christName != null)
    {
        catechistByName.TryAdd($"{christName} {fullName}".ToUpper().Trim(), id);
    }
}

Console.WriteLine($"  Parsed {catechists.Count} catechists");

// --- PHASE 2: Parse Class Sheets ---
Console.WriteLine("\n--- Parsing class sheets ---");
int youthCode = 1;

foreach (var (suffix, (classCode, className)) in classMapping)
{
    var fileName = $"THÔNG TIN THIẾU NHI  - {suffix}.csv";
    var filePath = Path.Combine(csvFolder, fileName);
    if (!File.Exists(filePath))
    {
        Console.WriteLine($"  WARNING: {fileName} not found, skipping");
        continue;
    }

    var lines = File.ReadAllLines(filePath, Encoding.UTF8);
    var classId = classIds[suffix];

    // Parse header rows for catechist assignments
    ParseClassCatechists(lines, classId, suffix);

    // Find the data header row (starts with "STT,")
    int dataStartRow = -1;
    for (int i = 0; i < Math.Min(lines.Length, 10); i++)
    {
        if (lines[i].TrimStart().StartsWith("STT,") || lines[i].TrimStart().StartsWith("STT\t"))
        {
            dataStartRow = i + 1;
            break;
        }
    }

    if (dataStartRow < 0)
    {
        Console.WriteLine($"  WARNING: Could not find data header in {fileName}");
        continue;
    }

    int classYouthCount = 0;
    for (int i = dataStartRow; i < lines.Length; i++)
    {
        var cols = ParseCsvLine(lines[i]);
        if (cols.Length < 4) continue;

        // Skip empty rows (STT is empty or not a number, and name is empty)
        var stt = cols[0]?.Trim();
        var givenName = cols.Length > 3 ? cols[3]?.Trim() : null;
        if (string.IsNullOrWhiteSpace(givenName)) continue;

        var christNameRaw = cols.Length > 1 ? cols[1]?.Trim() : null;
        var christName = CleanChristName(christNameRaw);
        var hoVaTenDem = cols.Length > 2 ? cols[2]?.Trim() : null; // Family + Middle name
        givenName = TitleCase(givenName);

        // Split HỌ VÀ TÊN ĐỆM into family + middle
        string familyName = "";
        string? middleName = null;
        if (!string.IsNullOrWhiteSpace(hoVaTenDem))
        {
            var parts = hoVaTenDem.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                familyName = TitleCase(parts[0]);
            }
            else
            {
                familyName = TitleCase(parts[0]);
                middleName = string.Join(" ", parts[1..].Select(TitleCase));
            }
        }

        // Date of birth
        DateOnly? dob = cols.Length > 4 ? ParseDate(cols[4]) : null;

        // Baptism date
        DateOnly? baptismDate = cols.Length > 5 ? ParseDate(cols[5]) : null;

        // Phone (SĐT-TN) - col 6
        string? phone = cols.Length > 6 ? CleanPhone(cols[6]) : null;

        // Address - col 7
        string? address = cols.Length > 7 ? cols[7]?.Trim() : null;
        if (string.IsNullOrWhiteSpace(address)) address = null;

        // Gender from saint name
        bool isMale = !IsFemale(christName);

        var youthId = Guid.NewGuid();
        var code = $"2025{youthCode:D3}";
        youthCode++;

        youths.Add(new YouthRecord(youthId, code, christName, familyName, middleName, givenName,
            dob, isMale, address, phone, classCode));

        // Enrollment
        classYouthEnrollments.Add(new ClassYouthEnrollment(Guid.NewGuid(), youthId, classId));

        // Milestones
        if (baptismDate.HasValue)
        {
            milestones.Add(new MilestoneRecord(Guid.NewGuid(), youthId, 0, baptismDate.Value, null)); // Baptism=0
        }

        // FirstCommunion - col 16 (0-indexed), place col 17
        if (cols.Length > 16)
        {
            var fcDate = ParseDate(cols[16]);
            var fcPlace = cols.Length > 17 ? cols[17]?.Trim() : null;
            if (fcDate.HasValue)
            {
                milestones.Add(new MilestoneRecord(Guid.NewGuid(), youthId, 2, fcDate.Value,
                    string.IsNullOrWhiteSpace(fcPlace) ? null : fcPlace)); // FirstCommunion=2
            }
        }

        // Confirmation - col 18, place col 19 (or col 20 for TS2C which has extra col)
        if (cols.Length > 18)
        {
            var confDate = ParseDate(cols[18]);
            string? confPlace = null;
            // TS2C has NGƯỜI ĐỠ ĐẦU at col 19 and GIÁO XỨ at col 20
            if (suffix == "TS2C" && cols.Length > 20)
            {
                confPlace = cols[20]?.Trim();
            }
            else if (cols.Length > 19)
            {
                confPlace = cols[19]?.Trim();
            }

            if (confDate.HasValue)
            {
                milestones.Add(new MilestoneRecord(Guid.NewGuid(), youthId, 3, confDate.Value,
                    string.IsNullOrWhiteSpace(confPlace) ? null : confPlace)); // Confirmation=3
            }
        }

        // Contacts: Father (col 8-9), Mother (col 10-11), Guardian (col 12-13)
        ParseContact(cols, 8, 9, youthId, 1); // Father
        ParseContact(cols, 10, 11, youthId, 2); // Mother
        ParseContact(cols, 12, 13, youthId, 3); // Guardian

        classYouthCount++;
    }

    Console.WriteLine($"  {suffix} ({className}): {classYouthCount} youths");
}

Console.WriteLine($"\nTotals: {youths.Count} youths, {milestones.Count} milestones, {contacts.Count} contacts, {classCatechistAssignments.Count} class-catechist assignments");

// --- PHASE 3: Generate SQL ---
Console.WriteLine("\n--- Generating SQL ---");
var sb = new StringBuilder();
sb.AppendLine("-- TNTTBD Data Import Script");
sb.AppendLine("-- Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
sb.AppendLine("-- Source: Google Sheets export (CSV files)");
sb.AppendLine();
sb.AppendLine("BEGIN;");
sb.AppendLine();

// CatechismYear
sb.AppendLine("-- === CATECHISM YEAR ===");
sb.AppendLine($"""INSERT INTO "CatechismYearEntity" ("Id", "StartDate", "EndDate", "Theme") VALUES ('{yearId}', '2025-09-01', '2026-05-31', NULL);""");
sb.AppendLine();

// CatechismClasses
sb.AppendLine("-- === CATECHISM CLASSES ===");
foreach (var (suffix, (code, name)) in classMapping)
{
    var cid = classIds[suffix];
    sb.AppendLine($"""INSERT INTO "CatechismClassEntity" ("Id", "ClassName", "ClassCode", "CatechismYearId") VALUES ('{cid}', '{Esc(name)}', '{Esc(code)}', '{yearId}');""");
}
sb.AppendLine();

// Catechists
sb.AppendLine("-- === CATECHISTS ===");
foreach (var c in catechists)
{
    sb.AppendLine($"""INSERT INTO "CatechistEntity" ("Id", "ChristName", "FamilyName", "MiddleName", "GivenName", "DateOfBirth", "IsMale", "Address", "PhoneNumber") VALUES ('{c.Id}', {NullOrStr(c.ChristName)}, '{Esc(c.FamilyName)}', {NullOrStr(c.MiddleName)}, '{Esc(c.GivenName)}', {NullOrDate(c.DateOfBirth)}, {c.IsMale.ToString().ToLower()}, NULL, {NullOrStr(c.PhoneNumber)});""");
}
sb.AppendLine();

// Catechist Patron Saints
sb.AppendLine("-- === CATECHIST PATRON SAINTS ===");
foreach (var c in catechists.Where(c => c.PatronDay.HasValue && c.PatronMonth.HasValue && c.PatronSaintName != null))
{
    sb.AppendLine($"""INSERT INTO "CatechistPatronSaints" ("Id", "CatechistId", "SaintName", "Day", "Month") VALUES ('{Guid.NewGuid()}', '{c.Id}', '{Esc(c.PatronSaintName)}', {c.PatronDay}, {c.PatronMonth});""");
}
sb.AppendLine();

// EucharisticYouth Running Number
sb.AppendLine("-- === RUNNING NUMBER ===");
sb.AppendLine($"""INSERT INTO "EucharisticYouthRunningNumberEntity" ("Id", "Year", "CurrentNumber") VALUES ('{Guid.NewGuid()}', 2025, {youthCode - 1});""");
sb.AppendLine();

// Youths
sb.AppendLine("-- === EUCHARISTIC YOUTHS ===");
foreach (var y in youths)
{
    sb.AppendLine($"""INSERT INTO "EucharisticYouthEntity" ("Id", "Code", "ChristName", "FamilyName", "MiddleName", "GivenName", "DateOfBirth", "IsMale", "Address", "PhoneNumber") VALUES ('{y.Id}', '{Esc(y.Code)}', {NullOrStr(y.ChristName)}, '{Esc(y.FamilyName)}', {NullOrStr(y.MiddleName)}, '{Esc(y.GivenName)}', {NullOrDate(y.DateOfBirth)}, {y.IsMale.ToString().ToLower()}, {NullOrStr(y.Address)}, {NullOrStr(y.PhoneNumber)});""");
}
sb.AppendLine();

// Youth Patron Saints
sb.AppendLine("-- === YOUTH PATRON SAINTS ===");
foreach (var y in youths.Where(y => y.ChristName != null))
{
    sb.AppendLine($"""INSERT INTO "EucharisticYouthPatronSaints" ("Id", "EucharisticYouthId", "SaintName", "Day", "Month") VALUES ('{Guid.NewGuid()}', '{y.Id}', '{Esc(y.ChristName)}', 0, 0);""");
}
sb.AppendLine();

// Milestones
sb.AppendLine("-- === SPIRITUAL MILESTONES ===");
foreach (var m in milestones)
{
    sb.AppendLine($"""INSERT INTO "SpiritualMilestoneEntity" ("Id", "EucharisticYouthId", "Type", "Date", "DocumentCode", "Place", "Note") VALUES ('{m.Id}', '{m.YouthId}', {m.Type}, '{m.Date:yyyy-MM-dd}', NULL, {NullOrStr(m.Place)}, NULL);""");
}
sb.AppendLine();

// Contacts
sb.AppendLine("-- === CONTACTS ===");
foreach (var c in contacts)
{
    sb.AppendLine($"""INSERT INTO "ContactEntity" ("Id", "ChristName", "FamilyName", "MiddleName", "GivenName", "PhoneNumber", "Email") VALUES ('{c.Id}', {NullOrStr(c.ChristName)}, {NullOrStr(c.FamilyName)}, {NullOrStr(c.MiddleName)}, '{Esc(c.GivenName)}', {NullOrStr(c.PhoneNumber)}, NULL);""");
}
sb.AppendLine();

// Youth-Contact Links
sb.AppendLine("-- === YOUTH-CONTACT LINKS ===");
foreach (var l in youthContactLinks)
{
    sb.AppendLine($"""INSERT INTO "EucharisticYouthContactEntity" ("Id", "EucharisticYouthId", "ContactId", "ContactRelationType") VALUES ('{l.Id}', '{l.YouthId}', '{l.ContactId}', {l.RelationType});""");
}
sb.AppendLine();

// Class-Catechist Assignments
sb.AppendLine("-- === CLASS-CATECHIST ASSIGNMENTS ===");
foreach (var a in classCatechistAssignments)
{
    sb.AppendLine($"""INSERT INTO "CatechismClassCatechistEntity" ("Id", "CatechistId", "CatechismClassId", "Role", "EnrolledAt", "LeftAt", "Status", "Note") VALUES ('{a.Id}', '{a.CatechistId}', '{a.ClassId}', {a.Role}, '2025-09-01T00:00:00+07:00', NULL, 1, NULL);""");
}
sb.AppendLine();

// Class-Youth Enrollments
sb.AppendLine("-- === CLASS-YOUTH ENROLLMENTS ===");
foreach (var e in classYouthEnrollments)
{
    sb.AppendLine($"""INSERT INTO "CatechismClassEucharisticYouthEntity" ("Id", "EucharisticYouthId", "CatechismClassId", "EnrolledAt", "LeftAt", "Status", "Note") VALUES ('{e.Id}', '{e.YouthId}', '{e.ClassId}', '2025-09-01T00:00:00+07:00', NULL, 1, NULL);""");
}
sb.AppendLine();

sb.AppendLine("COMMIT;");

File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
Console.WriteLine($"\nDone! Generated {outputPath}");
Console.WriteLine($"  Total lines: {sb.ToString().Split('\n').Length}");

// ======================== LOCAL FUNCTIONS ========================

void ParseClassCatechists(string[] lines, Guid classId, string suffix)
{
    // Headers typically look like:
    // Row 0: "DANH SÁCH LỚP ... 2025-2026"
    // Row 1: "GLV:,Chị Maria Vũ Minh Trân,,DT: Têrêsa Bùi Thảo My"
    // Row 2: ",Chị Maria Phạm Hoàng Yến Nhi,,DT: Martin Nguyễn Minh Hải"
    // Row 3: ",Chị Maria Vũ Ngọc Hương"
    // Row 4: "Vị trí lớp học: ..."

    bool isFirst = true;
    for (int i = 1; i < Math.Min(lines.Length, 6); i++)
    {
        var line = lines[i];
        if (line.StartsWith("Vị trí", StringComparison.OrdinalIgnoreCase) ||
            line.StartsWith("STT,", StringComparison.OrdinalIgnoreCase))
            break;

        var cols = ParseCsvLine(line);

        // Main GLV in col[1]
        if (cols.Length > 1 && !string.IsNullOrWhiteSpace(cols[1]))
        {
            var name = cols[1].Trim();
            var role = isFirst ? 0 : 1; // Main=0, Member=1
            isFirst = false;
            var catechistId = FindOrCreateCatechist(name);
            if (catechistId.HasValue)
            {
                classCatechistAssignments.Add(new ClassCatechistAssignment(Guid.NewGuid(), catechistId.Value, classId, role));
            }
        }

        // DT/TĐ in col[3] (or col[4] depending on format)
        for (int c = 2; c < cols.Length; c++)
        {
            var cell = cols[c]?.Trim();
            if (string.IsNullOrWhiteSpace(cell)) continue;

            int dtRole = -1;
            string? dtName = null;

            if (cell.StartsWith("DT:", StringComparison.OrdinalIgnoreCase) ||
                cell.StartsWith("DT :", StringComparison.OrdinalIgnoreCase))
            {
                dtName = cell[3..].Trim();
                dtRole = 2; // Trainee
            }
            else if (cell.StartsWith("TĐ:", StringComparison.OrdinalIgnoreCase) ||
                     cell.StartsWith("TĐ :", StringComparison.OrdinalIgnoreCase))
            {
                dtName = cell[3..].Trim();
                dtRole = 3; // Auxiliary
            }

            if (dtName != null && dtRole >= 0)
            {
                var catechistId = FindOrCreateCatechist(dtName);
                if (catechistId.HasValue)
                {
                    classCatechistAssignments.Add(new ClassCatechistAssignment(Guid.NewGuid(), catechistId.Value, classId, dtRole));
                }
            }
        }
    }
}

Guid? FindOrCreateCatechist(string rawName)
{
    // Strip prefixes: "Anh ", "Chị ", "Sơ "
    var name = rawName.Trim();
    foreach (var prefix in new[] { "Anh ", "Chị ", "Sơ " })
    {
        if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            name = name[prefix.Length..].Trim();
            break;
        }
    }

    if (string.IsNullOrWhiteSpace(name)) return null;

    // Try exact match
    var key = name.ToUpper().Trim();
    if (catechistByName.TryGetValue(key, out var id)) return id;

    // Try matching without saint name — extract name portion
    var parsed = SplitFullNameWithSaint(name);
    var nameOnly = $"{parsed.FamilyName} {parsed.MiddleName} {parsed.GivenName}".Replace("  ", " ").Trim().ToUpper();
    if (catechistByName.TryGetValue(nameOnly, out id)) return id;

    // Try with saint in front
    if (parsed.ChristName != null)
    {
        var withSaint = $"{parsed.ChristName} {parsed.FamilyName} {parsed.MiddleName} {parsed.GivenName}".Replace("  ", " ").Trim().ToUpper();
        if (catechistByName.TryGetValue(withSaint, out id)) return id;
    }

    // Not found — create minimal catechist
    Console.WriteLine($"    Creating missing catechist: {name}");
    var newId = Guid.NewGuid();
    bool isMale = !IsFemale(parsed.ChristName);

    catechists.Add(new CatechistRecord(newId, parsed.ChristName, parsed.FamilyName, parsed.MiddleName, parsed.GivenName,
        null, isMale, null, null, null, null));
    catechistByName[key] = newId;
    catechistByName[nameOnly] = newId;
    if (parsed.ChristName != null)
    {
        var withSaint = $"{parsed.ChristName} {parsed.FamilyName} {parsed.MiddleName} {parsed.GivenName}".Replace("  ", " ").Trim().ToUpper();
        catechistByName[withSaint] = newId;
    }

    return newId;
}

void ParseContact(string[] cols, int nameCol, int phoneCol, Guid youthId, int relationType)
{
    if (cols.Length <= nameCol) return;
    var nameRaw = cols[nameCol]?.Trim();
    if (string.IsNullOrWhiteSpace(nameRaw) || nameRaw == "0000000000") return;

    var phone = cols.Length > phoneCol ? CleanPhone(cols[phoneCol]) : null;
    var parsed = SplitFullNameWithSaint(nameRaw);

    // Skip if we couldn't extract a given name
    if (string.IsNullOrWhiteSpace(parsed.GivenName) && string.IsNullOrWhiteSpace(parsed.FamilyName)) return;

    var contactId = Guid.NewGuid();
    contacts.Add(new ContactRecord(contactId, parsed.ChristName, parsed.FamilyName, parsed.MiddleName, parsed.GivenName, phone));
    youthContactLinks.Add(new YouthContactLink(Guid.NewGuid(), youthId, contactId, relationType));
}

string[] ParseCsvLine(string line)
{
    var result = new List<string>();
    bool inQuotes = false;
    var current = new StringBuilder();

    for (int i = 0; i < line.Length; i++)
    {
        char c = line[i];
        if (c == '"')
        {
            if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
            {
                current.Append('"');
                i++;
            }
            else
            {
                inQuotes = !inQuotes;
            }
        }
        else if (c == ',' && !inQuotes)
        {
            result.Add(current.ToString());
            current.Clear();
        }
        else
        {
            current.Append(c);
        }
    }
    result.Add(current.ToString());
    return result.ToArray();
}

// --- Data models (must be after all top-level statements) ---
record CatechistRecord(Guid Id, string? ChristName, string FamilyName, string? MiddleName, string GivenName,
    DateOnly? DateOfBirth, bool IsMale, string? PhoneNumber, int? PatronDay, int? PatronMonth, string? PatronSaintName);

record YouthRecord(Guid Id, string Code, string? ChristName, string FamilyName, string? MiddleName, string GivenName,
    DateOnly? DateOfBirth, bool IsMale, string? Address, string? PhoneNumber, string ClassCode);

record MilestoneRecord(Guid Id, Guid YouthId, int Type, DateOnly Date, string? Place);

record ContactRecord(Guid Id, string? ChristName, string? FamilyName, string? MiddleName, string GivenName,
    string? PhoneNumber);

record YouthContactLink(Guid Id, Guid YouthId, Guid ContactId, int RelationType);

record ClassCatechistAssignment(Guid Id, Guid CatechistId, Guid ClassId, int Role);

record ClassYouthEnrollment(Guid Id, Guid YouthId, Guid ClassId);
