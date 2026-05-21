import{$ as ur,A as lr,b as ir,c as ce,ga as dr,ha as mr,ja as hr,ka as pr,p as sr,y as cr}from"./chunk-UTAGKRPA.js";import{$ as Fe,Aa as qe,Ba as jn,C as et,Ca as Et,Cc as tr,E as Zt,Ec as v,F as Jt,Fc as ie,Gc as er,H as ut,Hc as nr,Jb as Gn,Jc as rr,Kb as Wn,L as Tn,Lb as Qn,M as W,Mb as mt,Mc as se,N as $e,Pc as H,Qc as ar,S as Nn,T as U,Tc as or,U as Xt,Vb as Yn,W as P,Wa as zn,X as R,Y as kn,Yb as ae,Zb as ht,_ as x,_b as Kn,a as d,aa as On,ac as Zn,b as A,ba as _,bc as Jn,ca as te,d as _n,da as u,db as Dt,e as In,ea as Un,eb as z,f as An,ga as Q,gb as $n,h as tt,ha as k,hc as oe,i as T,ib as Fn,ic as Xn,jb as Ve,kb as dt,lb as Ge,m as N,ma as Pn,mb as rt,n as M,o as f,oa as Be,p as Mn,pa as Ln,pb as ne,q as Le,qa as nt,r as En,ra as He,sb as Bn,t as S,ta as ee,u as je,ub as Hn,v as B,va as y,w as ze,wb as qn,x as Dn,xb as Vn,y as Kt,za as Mt,zb as re}from"./chunk-WJUDORRU.js";var h="primary",Vt=Symbol("RouteTitle"),Ze=class{params;constructor(r){this.params=r||{}}has(r){return Object.prototype.hasOwnProperty.call(this.params,r)}get(r){if(this.has(r)){let t=this.params[r];return Array.isArray(t)?t[0]:t}return null}getAll(r){if(this.has(r)){let t=this.params[r];return Array.isArray(t)?t:[t]}return[]}get keys(){return Object.keys(this.params)}};function ot(e){return new Ze(e)}function We(e,r,t){for(let n=0;n<e.length;n++){let a=e[n],o=r[n];if(a[0]===":")t[a.substring(1)]=o;else if(a!==o.path)return!1}return!0}function Rr(e,r,t){let n=t.path.split("/"),a=n.indexOf("**");if(a===-1){if(n.length>e.length||t.pathMatch==="full"&&(r.hasChildren()||n.length<e.length))return null;let c={},l=e.slice(0,n.length);return We(n,l,c)?{consumed:l,posParams:c}:null}if(a!==n.lastIndexOf("**"))return null;let o=n.slice(0,a),s=n.slice(a+1);if(o.length+s.length>e.length||t.pathMatch==="full"&&r.hasChildren()&&t.path!=="**")return null;let i={};return!We(o,e.slice(0,o.length),i)||!We(s,e.slice(e.length-s.length),i)?null:{consumed:e,posParams:i}}function pe(e){return new Promise((r,t)=>{e.pipe(W()).subscribe({next:n=>r(n),error:n=>t(n)})})}function ma(e,r){if(e.length!==r.length)return!1;for(let t=0;t<e.length;++t)if(!$(e[t],r[t]))return!1;return!0}function $(e,r){let t=e?Je(e):void 0,n=r?Je(r):void 0;if(!t||!n||t.length!=n.length)return!1;let a;for(let o=0;o<t.length;o++)if(a=t[o],!xr(e[a],r[a]))return!1;return!0}function Je(e){return[...Object.keys(e),...Object.getOwnPropertySymbols(e)]}function xr(e,r){if(Array.isArray(e)&&Array.isArray(r)){if(e.length!==r.length)return!1;let t=[...e].sort(),n=[...r].sort();return t.every((a,o)=>n[o]===a)}else return e===r}function ha(e){return e.length>0?e[e.length-1]:null}function ct(e){return Le(e)?e:Hn(e)?M(Promise.resolve(e)):f(e)}function Cr(e){return Le(e)?pe(e):Promise.resolve(e)}var pa={exact:_r,subset:Ir},Sr={exact:fa,subset:ba,ignored:()=>!0},pn={paths:"exact",fragment:"ignored",matrixParams:"ignored",queryParams:"exact"},Pt={paths:"subset",fragment:"ignored",matrixParams:"ignored",queryParams:"subset"};function fn(e,r,t){let n=e instanceof I?e:r.parseUrl(e);return ie(()=>Xe(r.lastSuccessfulNavigation()?.finalUrl??new I,n,d(d({},Pt),t)))}function Xe(e,r,t){return pa[t.paths](e.root,r.root,t.matrixParams)&&Sr[t.queryParams](e.queryParams,r.queryParams)&&!(t.fragment==="exact"&&e.fragment!==r.fragment)}function fa(e,r){return $(e,r)}function _r(e,r,t){if(!at(e.segments,r.segments)||!de(e.segments,r.segments,t)||e.numberOfChildren!==r.numberOfChildren)return!1;for(let n in r.children)if(!e.children[n]||!_r(e.children[n],r.children[n],t))return!1;return!0}function ba(e,r){return Object.keys(r).length<=Object.keys(e).length&&Object.keys(r).every(t=>xr(e[t],r[t]))}function Ir(e,r,t){return Ar(e,r,r.segments,t)}function Ar(e,r,t,n){if(e.segments.length>t.length){let a=e.segments.slice(0,t.length);return!(!at(a,t)||r.hasChildren()||!de(a,t,n))}else if(e.segments.length===t.length){if(!at(e.segments,t)||!de(e.segments,t,n))return!1;for(let a in r.children)if(!e.children[a]||!Ir(e.children[a],r.children[a],n))return!1;return!0}else{let a=t.slice(0,e.segments.length),o=t.slice(e.segments.length);return!at(e.segments,a)||!de(e.segments,a,n)||!e.children[h]?!1:Ar(e.children[h],r,o,n)}}function de(e,r,t){return r.every((n,a)=>Sr[t](e[a].parameters,n.parameters))}var I=class{root;queryParams;fragment;_queryParamMap;constructor(r=new b([],{}),t={},n=null){this.root=r,this.queryParams=t,this.fragment=n}get queryParamMap(){return this._queryParamMap??=ot(this.queryParams),this._queryParamMap}toString(){return ya.serialize(this)}},b=class{segments;children;parent=null;constructor(r,t){this.segments=r,this.children=t,Object.values(t).forEach(n=>n.parent=this)}hasChildren(){return this.numberOfChildren>0}get numberOfChildren(){return Object.keys(this.children).length}toString(){return me(this)}},Y=class{path;parameters;_parameterMap;constructor(r,t){this.path=r,this.parameters=t}get parameterMap(){return this._parameterMap??=ot(this.parameters),this._parameterMap}toString(){return Er(this)}};function ga(e,r){return at(e,r)&&e.every((t,n)=>$(t.parameters,r[n].parameters))}function at(e,r){return e.length!==r.length?!1:e.every((t,n)=>t.path===r[n].path)}function va(e,r){let t=[];return Object.entries(e.children).forEach(([n,a])=>{n===h&&(t=t.concat(r(a,n)))}),Object.entries(e.children).forEach(([n,a])=>{n!==h&&(t=t.concat(r(a,n)))}),t}var xt=(()=>{class e{static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:()=>new K,providedIn:"root"})}return e})(),K=class{parse(r){let t=new en(r);return new I(t.parseRootSegment(),t.parseQueryParams(),t.parseFragment())}serialize(r){let t=`/${Tt(r.root,!0)}`,n=xa(r.queryParams),a=typeof r.fragment=="string"?`#${wa(r.fragment)}`:"";return`${t}${n}${a}`}},ya=new K;function me(e){return e.segments.map(r=>Er(r)).join("/")}function Tt(e,r){if(!e.hasChildren())return me(e);if(r){let t=e.children[h]?Tt(e.children[h],!1):"",n=[];return Object.entries(e.children).forEach(([a,o])=>{a!==h&&n.push(`${a}:${Tt(o,!1)}`)}),n.length>0?`${t}(${n.join("//")})`:t}else{let t=va(e,(n,a)=>a===h?[Tt(e.children[h],!1)]:[`${a}:${Tt(n,!1)}`]);return Object.keys(e.children).length===1&&e.children[h]!=null?`${me(e)}/${t[0]}`:`${me(e)}/(${t.join("//")})`}}function Mr(e){return encodeURIComponent(e).replace(/%40/g,"@").replace(/%3A/gi,":").replace(/%24/g,"$").replace(/%2C/gi,",")}function le(e){return Mr(e).replace(/%3B/gi,";")}function wa(e){return encodeURI(e)}function tn(e){return Mr(e).replace(/\(/g,"%28").replace(/\)/g,"%29").replace(/%26/gi,"&")}function he(e){return decodeURIComponent(e)}function fr(e){return he(e.replace(/\+/g,"%20"))}function Er(e){return`${tn(e.path)}${Ra(e.parameters)}`}function Ra(e){return Object.entries(e).map(([r,t])=>`;${tn(r)}=${tn(t)}`).join("")}function xa(e){let r=Object.entries(e).map(([t,n])=>Array.isArray(n)?n.map(a=>`${le(t)}=${le(a)}`).join("&"):`${le(t)}=${le(n)}`).filter(t=>t);return r.length?`?${r.join("&")}`:""}var Ca=/^[^\/()?;#]+/;function Qe(e){let r=e.match(Ca);return r?r[0]:""}var Sa=/^[^\/()?;=#]+/;function _a(e){let r=e.match(Sa);return r?r[0]:""}var Ia=/^[^=?&#]+/;function Aa(e){let r=e.match(Ia);return r?r[0]:""}var Ma=/^[^&#]+/;function Ea(e){let r=e.match(Ma);return r?r[0]:""}var en=class{url;remaining;constructor(r){this.url=r,this.remaining=r}parseRootSegment(){for(;this.consumeOptional("/"););return this.remaining===""||this.peekStartsWith("?")||this.peekStartsWith("#")?new b([],{}):new b([],this.parseChildren())}parseQueryParams(){let r={};if(this.consumeOptional("?"))do this.parseQueryParam(r);while(this.consumeOptional("&"));return r}parseFragment(){return this.consumeOptional("#")?decodeURIComponent(this.remaining):null}parseChildren(r=0){if(r>50)throw new R(4010,!1);if(this.remaining==="")return{};this.consumeOptional("/");let t=[];for(this.peekStartsWith("(")||t.push(this.parseSegment());this.peekStartsWith("/")&&!this.peekStartsWith("//")&&!this.peekStartsWith("/(");)this.capture("/"),t.push(this.parseSegment());let n={};this.peekStartsWith("/(")&&(this.capture("/"),n=this.parseParens(!0,r));let a={};return this.peekStartsWith("(")&&(a=this.parseParens(!1,r)),(t.length>0||Object.keys(n).length>0)&&(a[h]=new b(t,n)),a}parseSegment(){let r=Qe(this.remaining);if(r===""&&this.peekStartsWith(";"))throw new R(4009,!1);return this.capture(r),new Y(he(r),this.parseMatrixParams())}parseMatrixParams(){let r={};for(;this.consumeOptional(";");)this.parseParam(r);return r}parseParam(r){let t=_a(this.remaining);if(!t)return;this.capture(t);let n="";if(this.consumeOptional("=")){let a=Qe(this.remaining);a&&(n=a,this.capture(n))}r[he(t)]=he(n)}parseQueryParam(r){let t=Aa(this.remaining);if(!t)return;this.capture(t);let n="";if(this.consumeOptional("=")){let s=Ea(this.remaining);s&&(n=s,this.capture(n))}let a=fr(t),o=fr(n);if(r.hasOwnProperty(a)){let s=r[a];Array.isArray(s)||(s=[s],r[a]=s),s.push(o)}else r[a]=o}parseParens(r,t){let n={};for(this.capture("(");!this.consumeOptional(")")&&this.remaining.length>0;){let a=Qe(this.remaining),o=this.remaining[a.length];if(o!=="/"&&o!==")"&&o!==";")throw new R(4010,!1);let s;a.indexOf(":")>-1?(s=a.slice(0,a.indexOf(":")),this.capture(s),this.capture(":")):r&&(s=h);let i=this.parseChildren(t+1);n[s??h]=Object.keys(i).length===1&&i[h]?i[h]:new b([],i),this.consumeOptional("//")}return n}peekStartsWith(r){return this.remaining.startsWith(r)}consumeOptional(r){return this.peekStartsWith(r)?(this.remaining=this.remaining.substring(r.length),!0):!1}capture(r){if(!this.consumeOptional(r))throw new R(4011,!1)}};function Dr(e){return e.segments.length>0?new b([],{[h]:e}):e}function Tr(e){let r={};for(let[n,a]of Object.entries(e.children)){let o=Tr(a);if(n===h&&o.segments.length===0&&o.hasChildren())for(let[s,i]of Object.entries(o.children))r[s]=i;else(o.segments.length>0||o.hasChildren())&&(r[n]=o)}let t=new b(e.segments,r);return Da(t)}function Da(e){if(e.numberOfChildren===1&&e.children[h]){let r=e.children[h];return new b(e.segments.concat(r.segments),r.children)}return e}function Z(e){return e instanceof I}function Nr(e,r,t=null,n=null,a=new K){let o=kr(e);return Or(o,r,t,n,a)}function kr(e){let r;function t(o){let s={};for(let c of o.children){let l=t(c);s[c.outlet]=l}let i=new b(o.url,s);return o===e&&(r=i),i}let n=t(e.root),a=Dr(n);return r??a}function Or(e,r,t,n,a){let o=e;for(;o.parent;)o=o.parent;if(r.length===0)return Ye(o,o,o,t,n,a);let s=Ta(r);if(s.toRoot())return Ye(o,o,new b([],{}),t,n,a);let i=Na(s,o,e),c=i.processChildren?kt(i.segmentGroup,i.index,s.commands):Pr(i.segmentGroup,i.index,s.commands);return Ye(o,i.segmentGroup,c,t,n,a)}function fe(e){return typeof e=="object"&&e!=null&&!e.outlets&&!e.segmentPath}function Lt(e){return typeof e=="object"&&e!=null&&e.outlets}function br(e,r,t){e||="\u0275";let n=new I;return n.queryParams={[e]:r},t.parse(t.serialize(n)).queryParams[e]}function Ye(e,r,t,n,a,o){let s={};for(let[l,m]of Object.entries(n??{}))s[l]=Array.isArray(m)?m.map(p=>br(l,p,o)):br(l,m,o);let i;e===r?i=t:i=Ur(e,r,t);let c=Dr(Tr(i));return new I(c,s,a)}function Ur(e,r,t){let n={};return Object.entries(e.children).forEach(([a,o])=>{o===r?n[a]=t:n[a]=Ur(o,r,t)}),new b(e.segments,n)}var be=class{isAbsolute;numberOfDoubleDots;commands;constructor(r,t,n){if(this.isAbsolute=r,this.numberOfDoubleDots=t,this.commands=n,r&&n.length>0&&fe(n[0]))throw new R(4003,!1);let a=n.find(Lt);if(a&&a!==ha(n))throw new R(4004,!1)}toRoot(){return this.isAbsolute&&this.commands.length===1&&this.commands[0]=="/"}};function Ta(e){if(typeof e[0]=="string"&&e.length===1&&e[0]==="/")return new be(!0,0,e);let r=0,t=!1,n=e.reduce((a,o,s)=>{if(typeof o=="object"&&o!=null){if(o.outlets){let i={};return Object.entries(o.outlets).forEach(([c,l])=>{i[c]=typeof l=="string"?l.split("/"):l}),[...a,{outlets:i}]}if(o.segmentPath)return[...a,o.segmentPath]}return typeof o!="string"?[...a,o]:s===0?(o.split("/").forEach((i,c)=>{c==0&&i==="."||(c==0&&i===""?t=!0:i===".."?r++:i!=""&&a.push(i))}),a):[...a,o]},[]);return new be(t,r,n)}var ft=class{segmentGroup;processChildren;index;constructor(r,t,n){this.segmentGroup=r,this.processChildren=t,this.index=n}};function Na(e,r,t){if(e.isAbsolute)return new ft(r,!0,0);if(!t)return new ft(r,!1,NaN);if(t.parent===null)return new ft(t,!0,0);let n=fe(e.commands[0])?0:1,a=t.segments.length-1+n;return ka(t,a,e.numberOfDoubleDots)}function ka(e,r,t){let n=e,a=r,o=t;for(;o>a;){if(o-=a,n=n.parent,!n)throw new R(4005,!1);a=n.segments.length}return new ft(n,!1,a-o)}function Oa(e){return Lt(e[0])?e[0].outlets:{[h]:e}}function Pr(e,r,t){if(e??=new b([],{}),e.segments.length===0&&e.hasChildren())return kt(e,r,t);let n=Ua(e,r,t),a=t.slice(n.commandIndex);if(n.match&&n.pathIndex<e.segments.length){let o=new b(e.segments.slice(0,n.pathIndex),{});return o.children[h]=new b(e.segments.slice(n.pathIndex),e.children),kt(o,0,a)}else return n.match&&a.length===0?new b(e.segments,{}):n.match&&!e.hasChildren()?nn(e,r,t):n.match?kt(e,0,a):nn(e,r,t)}function kt(e,r,t){if(t.length===0)return new b(e.segments,{});{let n=Oa(t),a={};if(Object.keys(n).some(o=>o!==h)&&e.children[h]&&e.numberOfChildren===1&&e.children[h].segments.length===0){let o=kt(e.children[h],r,t);return new b(e.segments,o.children)}return Object.entries(n).forEach(([o,s])=>{typeof s=="string"&&(s=[s]),s!==null&&(a[o]=Pr(e.children[o],r,s))}),Object.entries(e.children).forEach(([o,s])=>{n[o]===void 0&&(a[o]=s)}),new b(e.segments,a)}}function Ua(e,r,t){let n=0,a=r,o={match:!1,pathIndex:0,commandIndex:0};for(;a<e.segments.length;){if(n>=t.length)return o;let s=e.segments[a],i=t[n];if(Lt(i))break;let c=`${i}`,l=n<t.length-1?t[n+1]:null;if(a>0&&c===void 0)break;if(c&&l&&typeof l=="object"&&l.outlets===void 0){if(!vr(c,l,s))return o;n+=2}else{if(!vr(c,{},s))return o;n++}a++}return{match:!0,pathIndex:a,commandIndex:n}}function nn(e,r,t){let n=e.segments.slice(0,r),a=0;for(;a<t.length;){let o=t[a];if(Lt(o)){let c=Pa(o.outlets);return new b(n,c)}if(a===0&&fe(t[0])){let c=e.segments[r];n.push(new Y(c.path,gr(t[0]))),a++;continue}let s=Lt(o)?o.outlets[h]:`${o}`,i=a<t.length-1?t[a+1]:null;s&&i&&fe(i)?(n.push(new Y(s,gr(i))),a+=2):(n.push(new Y(s,{})),a++)}return new b(n,{})}function Pa(e){let r={};return Object.entries(e).forEach(([t,n])=>{typeof n=="string"&&(n=[n]),n!==null&&(r[t]=nn(new b([],{}),0,n))}),r}function gr(e){let r={};return Object.entries(e).forEach(([t,n])=>r[t]=`${n}`),r}function vr(e,r,t){return e==t.path&&$(r,t.parameters)}var Ot="imperative",w=(function(e){return e[e.NavigationStart=0]="NavigationStart",e[e.NavigationEnd=1]="NavigationEnd",e[e.NavigationCancel=2]="NavigationCancel",e[e.NavigationError=3]="NavigationError",e[e.RoutesRecognized=4]="RoutesRecognized",e[e.ResolveStart=5]="ResolveStart",e[e.ResolveEnd=6]="ResolveEnd",e[e.GuardsCheckStart=7]="GuardsCheckStart",e[e.GuardsCheckEnd=8]="GuardsCheckEnd",e[e.RouteConfigLoadStart=9]="RouteConfigLoadStart",e[e.RouteConfigLoadEnd=10]="RouteConfigLoadEnd",e[e.ChildActivationStart=11]="ChildActivationStart",e[e.ChildActivationEnd=12]="ChildActivationEnd",e[e.ActivationStart=13]="ActivationStart",e[e.ActivationEnd=14]="ActivationEnd",e[e.Scroll=15]="Scroll",e[e.NavigationSkipped=16]="NavigationSkipped",e})(w||{}),D=class{id;url;constructor(r,t){this.id=r,this.url=t}},it=class extends D{type=w.NavigationStart;navigationTrigger;restoredState;constructor(r,t,n="imperative",a=null){super(r,t),this.navigationTrigger=n,this.restoredState=a}toString(){return`NavigationStart(id: ${this.id}, url: '${this.url}')`}},j=class extends D{urlAfterRedirects;type=w.NavigationEnd;constructor(r,t,n){super(r,t),this.urlAfterRedirects=n}toString(){return`NavigationEnd(id: ${this.id}, url: '${this.url}', urlAfterRedirects: '${this.urlAfterRedirects}')`}},C=(function(e){return e[e.Redirect=0]="Redirect",e[e.SupersededByNewNavigation=1]="SupersededByNewNavigation",e[e.NoDataFromResolver=2]="NoDataFromResolver",e[e.GuardRejected=3]="GuardRejected",e[e.Aborted=4]="Aborted",e})(C||{}),jt=(function(e){return e[e.IgnoredSameUrlNavigation=0]="IgnoredSameUrlNavigation",e[e.IgnoredByUrlHandlingStrategy=1]="IgnoredByUrlHandlingStrategy",e})(jt||{}),O=class extends D{reason;code;type=w.NavigationCancel;constructor(r,t,n,a){super(r,t),this.reason=n,this.code=a}toString(){return`NavigationCancel(id: ${this.id}, url: '${this.url}')`}};function Lr(e){return e instanceof O&&(e.code===C.Redirect||e.code===C.SupersededByNewNavigation)}var V=class extends D{reason;code;type=w.NavigationSkipped;constructor(r,t,n,a){super(r,t),this.reason=n,this.code=a}},st=class extends D{error;target;type=w.NavigationError;constructor(r,t,n,a){super(r,t),this.error=n,this.target=a}toString(){return`NavigationError(id: ${this.id}, url: '${this.url}', error: ${this.error})`}},zt=class extends D{urlAfterRedirects;state;type=w.RoutesRecognized;constructor(r,t,n,a){super(r,t),this.urlAfterRedirects=n,this.state=a}toString(){return`RoutesRecognized(id: ${this.id}, url: '${this.url}', urlAfterRedirects: '${this.urlAfterRedirects}', state: ${this.state})`}},ge=class extends D{urlAfterRedirects;state;type=w.GuardsCheckStart;constructor(r,t,n,a){super(r,t),this.urlAfterRedirects=n,this.state=a}toString(){return`GuardsCheckStart(id: ${this.id}, url: '${this.url}', urlAfterRedirects: '${this.urlAfterRedirects}', state: ${this.state})`}},ve=class extends D{urlAfterRedirects;state;shouldActivate;type=w.GuardsCheckEnd;constructor(r,t,n,a,o){super(r,t),this.urlAfterRedirects=n,this.state=a,this.shouldActivate=o}toString(){return`GuardsCheckEnd(id: ${this.id}, url: '${this.url}', urlAfterRedirects: '${this.urlAfterRedirects}', state: ${this.state}, shouldActivate: ${this.shouldActivate})`}},ye=class extends D{urlAfterRedirects;state;type=w.ResolveStart;constructor(r,t,n,a){super(r,t),this.urlAfterRedirects=n,this.state=a}toString(){return`ResolveStart(id: ${this.id}, url: '${this.url}', urlAfterRedirects: '${this.urlAfterRedirects}', state: ${this.state})`}},we=class extends D{urlAfterRedirects;state;type=w.ResolveEnd;constructor(r,t,n,a){super(r,t),this.urlAfterRedirects=n,this.state=a}toString(){return`ResolveEnd(id: ${this.id}, url: '${this.url}', urlAfterRedirects: '${this.urlAfterRedirects}', state: ${this.state})`}},Re=class{route;type=w.RouteConfigLoadStart;constructor(r){this.route=r}toString(){return`RouteConfigLoadStart(path: ${this.route.path})`}},xe=class{route;type=w.RouteConfigLoadEnd;constructor(r){this.route=r}toString(){return`RouteConfigLoadEnd(path: ${this.route.path})`}},Ce=class{snapshot;type=w.ChildActivationStart;constructor(r){this.snapshot=r}toString(){return`ChildActivationStart(path: '${this.snapshot.routeConfig&&this.snapshot.routeConfig.path||""}')`}},Se=class{snapshot;type=w.ChildActivationEnd;constructor(r){this.snapshot=r}toString(){return`ChildActivationEnd(path: '${this.snapshot.routeConfig&&this.snapshot.routeConfig.path||""}')`}},_e=class{snapshot;type=w.ActivationStart;constructor(r){this.snapshot=r}toString(){return`ActivationStart(path: '${this.snapshot.routeConfig&&this.snapshot.routeConfig.path||""}')`}},Ie=class{snapshot;type=w.ActivationEnd;constructor(r){this.snapshot=r}toString(){return`ActivationEnd(path: '${this.snapshot.routeConfig&&this.snapshot.routeConfig.path||""}')`}};var gt=class{},$t=class{},vt=class{url;navigationBehaviorOptions;constructor(r,t){this.url=r,this.navigationBehaviorOptions=t}};function La(e){return!(e instanceof gt)&&!(e instanceof vt)&&!(e instanceof $t)}var Ae=class{rootInjector;outlet=null;route=null;children;attachRef=null;get injector(){return this.route?.snapshot._environmentInjector??this.rootInjector}constructor(r){this.rootInjector=r,this.children=new Ct(this.rootInjector)}},Ct=(()=>{class e{rootInjector;contexts=new Map;constructor(t){this.rootInjector=t}onChildOutletCreated(t,n){let a=this.getOrCreateContext(t);a.outlet=n,this.contexts.set(t,a)}onChildOutletDestroyed(t){let n=this.getContext(t);n&&(n.outlet=null,n.attachRef=null)}onOutletDeactivated(){let t=this.contexts;return this.contexts=new Map,t}onOutletReAttached(t){this.contexts=t}getOrCreateContext(t){let n=this.getContext(t);return n||(n=new Ae(this.rootInjector),this.contexts.set(t,n)),n}getContext(t){return this.contexts.get(t)||null}static \u0275fac=function(n){return new(n||e)(te(Q))};static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})(),Me=class{_root;constructor(r){this._root=r}get root(){return this._root.value}parent(r){let t=this.pathFromRoot(r);return t.length>1?t[t.length-2]:null}children(r){let t=rn(r,this._root);return t?t.children.map(n=>n.value):[]}firstChild(r){let t=rn(r,this._root);return t&&t.children.length>0?t.children[0].value:null}siblings(r){let t=an(r,this._root);return t.length<2?[]:t[t.length-2].children.map(a=>a.value).filter(a=>a!==r)}pathFromRoot(r){return an(r,this._root).map(t=>t.value)}};function rn(e,r){if(e===r.value)return r;for(let t of r.children){let n=rn(e,t);if(n)return n}return null}function an(e,r){if(e===r.value)return[r];for(let t of r.children){let n=an(e,t);if(n.length)return n.unshift(r),n}return[]}var E=class{value;children;constructor(r,t){this.value=r,this.children=t}toString(){return`TreeNode(${this.value})`}};function pt(e){let r={};return e&&e.children.forEach(t=>r[t.value.outlet]=t),r}var Ft=class extends Me{snapshot;constructor(r,t){super(r),this.snapshot=t,gn(this,r)}toString(){return this.snapshot.toString()}};function jr(e,r){let t=ja(e,r),n=new T([new Y("",{})]),a=new T({}),o=new T({}),s=new T({}),i=new T(""),c=new G(n,a,s,i,o,h,e,t.root);return c.snapshot=t.root,new Ft(new E(c,[]),t)}function ja(e,r){let t={},n={},a={},s=new yt([],t,a,"",n,h,e,null,{},r);return new Bt("",new E(s,[]))}var G=class{urlSubject;paramsSubject;queryParamsSubject;fragmentSubject;dataSubject;outlet;component;snapshot;_futureSnapshot;_routerState;_paramMap;_queryParamMap;title;url;params;queryParams;fragment;data;constructor(r,t,n,a,o,s,i,c){this.urlSubject=r,this.paramsSubject=t,this.queryParamsSubject=n,this.fragmentSubject=a,this.dataSubject=o,this.outlet=s,this.component=i,this._futureSnapshot=c,this.title=this.dataSubject?.pipe(S(l=>l[Vt]))??f(void 0),this.url=r,this.params=t,this.queryParams=n,this.fragment=a,this.data=o}get routeConfig(){return this._futureSnapshot.routeConfig}get root(){return this._routerState.root}get parent(){return this._routerState.parent(this)}get firstChild(){return this._routerState.firstChild(this)}get children(){return this._routerState.children(this)}get pathFromRoot(){return this._routerState.pathFromRoot(this)}get paramMap(){return this._paramMap??=this.params.pipe(S(r=>ot(r))),this._paramMap}get queryParamMap(){return this._queryParamMap??=this.queryParams.pipe(S(r=>ot(r))),this._queryParamMap}toString(){return this.snapshot?this.snapshot.toString():`Future(${this._futureSnapshot})`}};function bn(e,r,t="emptyOnly"){let n,{routeConfig:a}=e;return r!==null&&(t==="always"||a?.path===""||!r.component&&!r.routeConfig?.loadComponent)?n={params:d(d({},r.params),e.params),data:d(d({},r.data),e.data),resolve:d(d(d(d({},e.data),r.data),a?.data),e._resolvedData)}:n={params:d({},e.params),data:d({},e.data),resolve:d(d({},e.data),e._resolvedData??{})},a&&$r(a)&&(n.resolve[Vt]=a.title),n}var yt=class{url;params;queryParams;fragment;data;outlet;component;routeConfig;_resolve;_resolvedData;_routerState;_paramMap;_queryParamMap;_environmentInjector;get title(){return this.data?.[Vt]}constructor(r,t,n,a,o,s,i,c,l,m){this.url=r,this.params=t,this.queryParams=n,this.fragment=a,this.data=o,this.outlet=s,this.component=i,this.routeConfig=c,this._resolve=l,this._environmentInjector=m}get root(){return this._routerState.root}get parent(){return this._routerState.parent(this)}get firstChild(){return this._routerState.firstChild(this)}get children(){return this._routerState.children(this)}get pathFromRoot(){return this._routerState.pathFromRoot(this)}get paramMap(){return this._paramMap??=ot(this.params),this._paramMap}get queryParamMap(){return this._queryParamMap??=ot(this.queryParams),this._queryParamMap}toString(){let r=this.url.map(n=>n.toString()).join("/"),t=this.routeConfig?this.routeConfig.path:"";return`Route(url:'${r}', path:'${t}')`}},Bt=class extends Me{url;constructor(r,t){super(t),this.url=r,gn(this,t)}toString(){return zr(this._root)}};function gn(e,r){r.value._routerState=e,r.children.forEach(t=>gn(e,t))}function zr(e){let r=e.children.length>0?` { ${e.children.map(zr).join(", ")} } `:"";return`${e.value}${r}`}function Ke(e){if(e.snapshot){let r=e.snapshot,t=e._futureSnapshot;e.snapshot=t,$(r.queryParams,t.queryParams)||e.queryParamsSubject.next(t.queryParams),r.fragment!==t.fragment&&e.fragmentSubject.next(t.fragment),$(r.params,t.params)||e.paramsSubject.next(t.params),ma(r.url,t.url)||e.urlSubject.next(t.url),$(r.data,t.data)||e.dataSubject.next(t.data)}else e.snapshot=e._futureSnapshot,e.dataSubject.next(e._futureSnapshot.data)}function on(e,r){let t=$(e.params,r.params)&&ga(e.url,r.url),n=!e.parent!=!r.parent;return t&&!n&&(!e.parent||on(e.parent,r.parent))}function $r(e){return typeof e.title=="string"||e.title===null}var Fr=new _(""),vn=(()=>{class e{activated=null;get activatedComponentRef(){return this.activated}_activatedRoute=null;name=h;activateEvents=new nt;deactivateEvents=new nt;attachEvents=new nt;detachEvents=new nt;routerOutletData=rr();parentContexts=u(Ct);location=u($n);changeDetector=u(se);inputBinder=u(Gt,{optional:!0});supportsBindingToComponentInputs=!0;ngOnChanges(t){if(t.name){let{firstChange:n,previousValue:a}=t.name;if(n)return;this.isTrackedInParentContexts(a)&&(this.deactivate(),this.parentContexts.onChildOutletDestroyed(a)),this.initializeOutletWithName()}}ngOnDestroy(){this.isTrackedInParentContexts(this.name)&&this.parentContexts.onChildOutletDestroyed(this.name),this.inputBinder?.unsubscribeFromRouteData(this)}isTrackedInParentContexts(t){return this.parentContexts.getContext(t)?.outlet===this}ngOnInit(){this.initializeOutletWithName()}initializeOutletWithName(){if(this.parentContexts.onChildOutletCreated(this.name,this),this.activated)return;let t=this.parentContexts.getContext(this.name);t?.route&&(t.attachRef?this.attach(t.attachRef,t.route):this.activateWith(t.route,t.injector))}get isActivated(){return!!this.activated}get component(){if(!this.activated)throw new R(4012,!1);return this.activated.instance}get activatedRoute(){if(!this.activated)throw new R(4012,!1);return this._activatedRoute}get activatedRouteData(){return this._activatedRoute?this._activatedRoute.snapshot.data:{}}detach(){if(!this.activated)throw new R(4012,!1);this.location.detach();let t=this.activated;return this.activated=null,this._activatedRoute=null,this.detachEvents.emit(t.instance),t}attach(t,n){this.activated=t,this._activatedRoute=n,this.location.insert(t.hostView),this.inputBinder?.bindActivatedRouteToOutletComponent(this),this.attachEvents.emit(t.instance)}deactivate(){if(this.activated){let t=this.component;this.activated.destroy(),this.activated=null,this._activatedRoute=null,this.deactivateEvents.emit(t)}}activateWith(t,n){if(this.isActivated)throw new R(4013,!1);this._activatedRoute=t;let a=this.location,s=t.snapshot.component,i=this.parentContexts.getOrCreateContext(this.name).children,c=new sn(t,i,a.injector,this.routerOutletData);this.activated=a.createComponent(s,{index:a.length,injector:c,environmentInjector:n}),this.changeDetector.markForCheck(),this.inputBinder?.bindActivatedRouteToOutletComponent(this),this.activateEvents.emit(this.activated.instance)}static \u0275fac=function(n){return new(n||e)};static \u0275dir=rt({type:e,selectors:[["router-outlet"]],inputs:{name:"name",routerOutletData:[1,"routerOutletData"]},outputs:{activateEvents:"activate",deactivateEvents:"deactivate",attachEvents:"attach",detachEvents:"detach"},exportAs:["outlet"],features:[Mt]})}return e})(),sn=class{route;childContexts;parent;outletData;constructor(r,t,n,a){this.route=r,this.childContexts=t,this.parent=n,this.outletData=a}get(r,t){return r===G?this.route:r===Ct?this.childContexts:r===Fr?this.outletData:this.parent.get(r,t)}},Gt=new _(""),yn=(()=>{class e{outletDataSubscriptions=new Map;bindActivatedRouteToOutletComponent(t){this.unsubscribeFromRouteData(t),this.subscribeToRouteData(t)}unsubscribeFromRouteData(t){this.outletDataSubscriptions.get(t)?.unsubscribe(),this.outletDataSubscriptions.delete(t)}subscribeToRouteData(t){let{activatedRoute:n}=t,a=je([n.queryParams,n.params,n.data]).pipe(U(([o,s,i],c)=>(i=d(d(d({},o),s),i),c===0?f(i):Promise.resolve(i)))).subscribe(o=>{if(!t.isActivated||!t.activatedComponentRef||t.activatedRoute!==n||n.component===null){this.unsubscribeFromRouteData(t);return}let s=or(n.component);if(!s){this.unsubscribeFromRouteData(t);return}for(let{templateName:i}of s.inputs)t.activatedComponentRef.setInput(i,o[i])});this.outletDataSubscriptions.set(t,a)}static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:e.\u0275fac})}return e})(),wn=(()=>{class e{static \u0275fac=function(n){return new(n||e)};static \u0275cmp=dt({type:e,selectors:[["ng-component"]],exportAs:["emptyRouterOutlet"],decls:1,vars:0,template:function(n,a){n&1&&Gn(0,"router-outlet")},dependencies:[vn],encapsulation:2})}return e})();function Rn(e){let r=e.children&&e.children.map(Rn),t=r?A(d({},e),{children:r}):d({},e);return!t.component&&!t.loadComponent&&(r||t.loadChildren)&&t.outlet&&t.outlet!==h&&(t.component=wn),t}function za(e,r,t){let n=Ht(e,r._root,t?t._root:void 0);return new Ft(n,r)}function Ht(e,r,t){if(t&&e.shouldReuseRoute(r.value,t.value.snapshot)){let n=t.value;n._futureSnapshot=r.value;let a=$a(e,r,t);return new E(n,a)}else{if(e.shouldAttach(r.value)){let o=e.retrieve(r.value);if(o!==null){let s=o.route;return s.value._futureSnapshot=r.value,s.children=r.children.map(i=>Ht(e,i)),s}}let n=Fa(r.value),a=r.children.map(o=>Ht(e,o));return new E(n,a)}}function $a(e,r,t){return r.children.map(n=>{for(let a of t.children)if(e.shouldReuseRoute(n.value,a.value.snapshot))return Ht(e,n,a);return Ht(e,n)})}function Fa(e){return new G(new T(e.url),new T(e.params),new T(e.queryParams),new T(e.fragment),new T(e.data),e.outlet,e.component,e)}var wt=class{redirectTo;navigationBehaviorOptions;constructor(r,t){this.redirectTo=r,this.navigationBehaviorOptions=t}},Br="ngNavigationCancelingError";function Ee(e,r){let{redirectTo:t,navigationBehaviorOptions:n}=Z(r)?{redirectTo:r,navigationBehaviorOptions:void 0}:r,a=Hr(!1,C.Redirect);return a.url=t,a.navigationBehaviorOptions=n,a}function Hr(e,r){let t=new Error(`NavigationCancelingError: ${e||""}`);return t[Br]=!0,t.cancellationCode=r,t}function Ba(e){return qr(e)&&Z(e.url)}function qr(e){return!!e&&e[Br]}var cn=class{routeReuseStrategy;futureState;currState;forwardEvent;inputBindingEnabled;constructor(r,t,n,a,o){this.routeReuseStrategy=r,this.futureState=t,this.currState=n,this.forwardEvent=a,this.inputBindingEnabled=o}activate(r){let t=this.futureState._root,n=this.currState?this.currState._root:null;this.deactivateChildRoutes(t,n,r),Ke(this.futureState.root),this.activateChildRoutes(t,n,r)}deactivateChildRoutes(r,t,n){let a=pt(t);r.children.forEach(o=>{let s=o.value.outlet;this.deactivateRoutes(o,a[s],n),delete a[s]}),Object.values(a).forEach(o=>{this.deactivateRouteAndItsChildren(o,n)})}deactivateRoutes(r,t,n){let a=r.value,o=t?t.value:null;if(a===o)if(a.component){let s=n.getContext(a.outlet);s&&this.deactivateChildRoutes(r,t,s.children)}else this.deactivateChildRoutes(r,t,n);else o&&this.deactivateRouteAndItsChildren(t,n)}deactivateRouteAndItsChildren(r,t){r.value.component&&this.routeReuseStrategy.shouldDetach(r.value.snapshot)?this.detachAndStoreRouteSubtree(r,t):this.deactivateRouteAndOutlet(r,t)}detachAndStoreRouteSubtree(r,t){let n=t.getContext(r.value.outlet),a=n&&r.value.component?n.children:t,o=pt(r);for(let s of Object.values(o))this.deactivateRouteAndItsChildren(s,a);if(n&&n.outlet){let s=n.outlet.detach(),i=n.children.onOutletDeactivated();this.routeReuseStrategy.store(r.value.snapshot,{componentRef:s,route:r,contexts:i})}}deactivateRouteAndOutlet(r,t){let n=t.getContext(r.value.outlet),a=n&&r.value.component?n.children:t,o=pt(r);for(let s of Object.values(o))this.deactivateRouteAndItsChildren(s,a);n&&(n.outlet&&(n.outlet.deactivate(),n.children.onOutletDeactivated()),n.attachRef=null,n.route=null)}activateChildRoutes(r,t,n){let a=pt(t);r.children.forEach(o=>{this.activateRoutes(o,a[o.value.outlet],n),this.forwardEvent(new Ie(o.value.snapshot))}),r.children.length&&this.forwardEvent(new Se(r.value.snapshot))}activateRoutes(r,t,n){let a=r.value,o=t?t.value:null;if(Ke(a),a===o)if(a.component){let s=n.getOrCreateContext(a.outlet);this.activateChildRoutes(r,t,s.children)}else this.activateChildRoutes(r,t,n);else if(a.component){let s=n.getOrCreateContext(a.outlet);if(this.routeReuseStrategy.shouldAttach(a.snapshot)){let i=this.routeReuseStrategy.retrieve(a.snapshot);this.routeReuseStrategy.store(a.snapshot,null),s.children.onOutletReAttached(i.contexts),s.attachRef=i.componentRef,s.route=i.route.value,s.outlet&&s.outlet.attach(i.componentRef,i.route.value),Ke(i.route.value),this.activateChildRoutes(r,null,s.children)}else s.attachRef=null,s.route=a,s.outlet&&s.outlet.activateWith(a,s.injector),this.activateChildRoutes(r,null,s.children)}else this.activateChildRoutes(r,null,n)}},De=class{path;route;constructor(r){this.path=r,this.route=this.path[this.path.length-1]}},bt=class{component;route;constructor(r,t){this.component=r,this.route=t}};function Ha(e,r,t){let n=e._root,a=r?r._root:null;return Nt(n,a,t,[n.value])}function qa(e){let r=e.routeConfig?e.routeConfig.canActivateChild:null;return!r||r.length===0?null:{node:e,guards:r}}function St(e,r){let t=Symbol(),n=r.get(e,t);return n===t?typeof e=="function"&&!On(e)?e:r.get(e):n}function Nt(e,r,t,n,a={canDeactivateChecks:[],canActivateChecks:[]}){let o=pt(r);return e.children.forEach(s=>{Va(s,o[s.value.outlet],t,n.concat([s.value]),a),delete o[s.value.outlet]}),Object.entries(o).forEach(([s,i])=>Ut(i,t.getContext(s),a)),a}function Va(e,r,t,n,a={canDeactivateChecks:[],canActivateChecks:[]}){let o=e.value,s=r?r.value:null,i=t?t.getContext(e.value.outlet):null;if(s&&o.routeConfig===s.routeConfig){let c=Ga(s,o,o.routeConfig.runGuardsAndResolvers);c?a.canActivateChecks.push(new De(n)):(o.data=s.data,o._resolvedData=s._resolvedData),o.component?Nt(e,r,i?i.children:null,n,a):Nt(e,r,t,n,a),c&&i&&i.outlet&&i.outlet.isActivated&&a.canDeactivateChecks.push(new bt(i.outlet.component,s))}else s&&Ut(r,i,a),a.canActivateChecks.push(new De(n)),o.component?Nt(e,null,i?i.children:null,n,a):Nt(e,null,t,n,a);return a}function Ga(e,r,t){if(typeof t=="function")return k(r._environmentInjector,()=>t(e,r));switch(t){case"pathParamsChange":return!at(e.url,r.url);case"pathParamsOrQueryParamsChange":return!at(e.url,r.url)||!$(e.queryParams,r.queryParams);case"always":return!0;case"paramsOrQueryParamsChange":return!on(e,r)||!$(e.queryParams,r.queryParams);default:return!on(e,r)}}function Ut(e,r,t){let n=pt(e),a=e.value;Object.entries(n).forEach(([o,s])=>{a.component?r?Ut(s,r.children.getContext(o),t):Ut(s,null,t):Ut(s,r,t)}),a.component?r&&r.outlet&&r.outlet.isActivated?t.canDeactivateChecks.push(new bt(r.outlet.component,a)):t.canDeactivateChecks.push(new bt(null,a)):t.canDeactivateChecks.push(new bt(null,a))}function Wt(e){return typeof e=="function"}function Wa(e){return typeof e=="boolean"}function Qa(e){return e&&Wt(e.canLoad)}function Ya(e){return e&&Wt(e.canActivate)}function Ka(e){return e&&Wt(e.canActivateChild)}function Za(e){return e&&Wt(e.canDeactivate)}function Ja(e){return e&&Wt(e.canMatch)}function Vr(e){return e instanceof En||e?.name==="EmptyError"}var ue=Symbol("INITIAL_VALUE");function Rt(){return U(e=>je(e.map(r=>r.pipe(ut(1),Nn(ue)))).pipe(S(r=>{for(let t of r)if(t!==!0){if(t===ue)return ue;if(t===!1||Xa(t))return t}return!0}),et(r=>r!==ue),ut(1)))}function Xa(e){return Z(e)||e instanceof wt}function Gr(e){return e.aborted?f(void 0).pipe(ut(1)):new An(r=>{let t=()=>{r.next(),r.complete()};return e.addEventListener("abort",t),()=>e.removeEventListener("abort",t)})}function Wr(e){return Xt(Gr(e))}function to(e){return B(r=>{let{targetSnapshot:t,currentSnapshot:n,guards:{canActivateChecks:a,canDeactivateChecks:o}}=r;return o.length===0&&a.length===0?f(A(d({},r),{guardsResult:!0})):eo(o,t,n).pipe(B(s=>s&&Wa(s)?no(t,a,e):f(s)),S(s=>A(d({},r),{guardsResult:s})))})}function eo(e,r,t){return M(e).pipe(B(n=>so(n.component,n.route,t,r)),W(n=>n!==!0,!0))}function no(e,r,t){return M(r).pipe(Jt(n=>Dn(ao(n.route.parent,t),ro(n.route,t),io(e,n.path),oo(e,n.route))),W(n=>n!==!0,!0))}function ro(e,r){return e!==null&&r&&r(new _e(e)),f(!0)}function ao(e,r){return e!==null&&r&&r(new Ce(e)),f(!0)}function oo(e,r){let t=r.routeConfig?r.routeConfig.canActivate:null;if(!t||t.length===0)return f(!0);let n=t.map(a=>Kt(()=>{let o=r._environmentInjector,s=St(a,o),i=Ya(s)?s.canActivate(r,e):k(o,()=>s(r,e));return ct(i).pipe(W())}));return f(n).pipe(Rt())}function io(e,r){let t=r[r.length-1],a=r.slice(0,r.length-1).reverse().map(o=>qa(o)).filter(o=>o!==null).map(o=>Kt(()=>{let s=o.guards.map(i=>{let c=o.node._environmentInjector,l=St(i,c),m=Ka(l)?l.canActivateChild(t,e):k(c,()=>l(t,e));return ct(m).pipe(W())});return f(s).pipe(Rt())}));return f(a).pipe(Rt())}function so(e,r,t,n){let a=r&&r.routeConfig?r.routeConfig.canDeactivate:null;if(!a||a.length===0)return f(!0);let o=a.map(s=>{let i=r._environmentInjector,c=St(s,i),l=Za(c)?c.canDeactivate(e,r,t,n):k(i,()=>c(e,r,t,n));return ct(l).pipe(W())});return f(o).pipe(Rt())}function co(e,r,t,n,a){let o=r.canLoad;if(o===void 0||o.length===0)return f(!0);let s=o.map(i=>{let c=St(i,e),l=Qa(c)?c.canLoad(r,t):k(e,()=>c(r,t)),m=ct(l);return a?m.pipe(Wr(a)):m});return f(s).pipe(Rt(),Qr(n))}function Qr(e){return In(P(r=>{if(typeof r!="boolean")throw Ee(e,r)}),S(r=>r===!0))}function lo(e,r,t,n,a,o){let s=r.canMatch;if(!s||s.length===0)return f(!0);let i=s.map(c=>{let l=St(c,e),m=Ja(l)?l.canMatch(r,t,a):k(e,()=>l(r,t,a));return ct(m).pipe(Wr(o))});return f(i).pipe(Rt(),Qr(n))}var q=class e extends Error{segmentGroup;constructor(r){super(),this.segmentGroup=r||null,Object.setPrototypeOf(this,e.prototype)}},qt=class e extends Error{urlTree;constructor(r){super(),this.urlTree=r,Object.setPrototypeOf(this,e.prototype)}};function uo(e){throw new R(4e3,!1)}function mo(e){throw Hr(!1,C.GuardRejected)}var ln=class{urlSerializer;urlTree;constructor(r,t){this.urlSerializer=r,this.urlTree=t}async lineralizeSegments(r,t){let n=[],a=t.root;for(;;){if(n=n.concat(a.segments),a.numberOfChildren===0)return n;if(a.numberOfChildren>1||!a.children[h])throw uo(`${r.redirectTo}`);a=a.children[h]}}async applyRedirectCommands(r,t,n,a,o){let s=await ho(t,a,o);if(s instanceof I)throw new qt(s);let i=this.applyRedirectCreateUrlTree(s,this.urlSerializer.parse(s),r,n);if(s[0]==="/")throw new qt(i);return i}applyRedirectCreateUrlTree(r,t,n,a){let o=this.createSegmentGroup(r,t.root,n,a);return new I(o,this.createQueryParams(t.queryParams,this.urlTree.queryParams),t.fragment)}createQueryParams(r,t){let n={};return Object.entries(r).forEach(([a,o])=>{if(typeof o=="string"&&o[0]===":"){let i=o.substring(1);n[a]=t[i]}else n[a]=o}),n}createSegmentGroup(r,t,n,a){let o=this.createSegments(r,t.segments,n,a),s={};return Object.entries(t.children).forEach(([i,c])=>{s[i]=this.createSegmentGroup(r,c,n,a)}),new b(o,s)}createSegments(r,t,n,a){return t.map(o=>o.path[0]===":"?this.findPosParam(r,o,a):this.findOrReturn(o,n))}findPosParam(r,t,n){let a=n[t.path.substring(1)];if(!a)throw new R(4001,!1);return a}findOrReturn(r,t){let n=0;for(let a of t){if(a.path===r.path)return t.splice(n),a;n++}return r}};function ho(e,r,t){if(typeof e=="string")return Promise.resolve(e);let n=e;return pe(ct(k(t,()=>n(r))))}function po(e,r){return e.providers&&!e._injector&&(e._injector=Ve(e.providers,r,`Route: ${e.path}`)),e._injector??r}function L(e){return e.outlet||h}function fo(e,r){let t=e.filter(n=>L(n)===r);return t.push(...e.filter(n=>L(n)!==r)),t}var un={matched:!1,consumedSegments:[],remainingSegments:[],parameters:{},positionalParamSegments:{}};function Yr(e){return{routeConfig:e.routeConfig,url:e.url,params:e.params,queryParams:e.queryParams,fragment:e.fragment,data:e.data,outlet:e.outlet,title:e.title,paramMap:e.paramMap,queryParamMap:e.queryParamMap}}function bo(e,r,t,n,a,o,s){let i=Kr(e,r,t);if(!i.matched)return f(i);let c=Yr(o(i));return n=po(r,n),lo(n,r,t,a,c,s).pipe(S(l=>l===!0?i:d({},un)))}function Kr(e,r,t){if(r.path==="")return r.pathMatch==="full"&&(e.hasChildren()||t.length>0)?d({},un):{matched:!0,consumedSegments:[],remainingSegments:t,parameters:{},positionalParamSegments:{}};let a=(r.matcher||Rr)(t,e,r);if(!a)return d({},un);let o={};Object.entries(a.posParams??{}).forEach(([i,c])=>{o[i]=c.path});let s=a.consumed.length>0?d(d({},o),a.consumed[a.consumed.length-1].parameters):o;return{matched:!0,consumedSegments:a.consumed,remainingSegments:t.slice(a.consumed.length),parameters:s,positionalParamSegments:a.posParams??{}}}function yr(e,r,t,n,a){return t.length>0&&yo(e,t,n,a)?{segmentGroup:new b(r,vo(n,new b(t,e.children))),slicedSegments:[]}:t.length===0&&wo(e,t,n)?{segmentGroup:new b(e.segments,go(e,t,n,e.children)),slicedSegments:t}:{segmentGroup:new b(e.segments,e.children),slicedSegments:t}}function go(e,r,t,n){let a={};for(let o of t)if(Ne(e,r,o)&&!n[L(o)]){let s=new b([],{});a[L(o)]=s}return d(d({},n),a)}function vo(e,r){let t={};t[h]=r;for(let n of e)if(n.path===""&&L(n)!==h){let a=new b([],{});t[L(n)]=a}return t}function yo(e,r,t,n){return t.some(a=>!Ne(e,r,a)||!(L(a)!==h)?!1:!(n!==void 0&&L(a)===n))}function wo(e,r,t){return t.some(n=>Ne(e,r,n))}function Ne(e,r,t){return(e.hasChildren()||r.length>0)&&t.pathMatch==="full"?!1:t.path===""}function Ro(e,r,t){return r.length===0&&!e.children[t]}var dn=class{};async function xo(e,r,t,n,a,o,s="emptyOnly",i){return new mn(e,r,t,n,a,s,o,i).recognize()}var Co=31,mn=class{injector;configLoader;rootComponentType;config;urlTree;paramsInheritanceStrategy;urlSerializer;abortSignal;applyRedirects;absoluteRedirectCount=0;allowRedirects=!0;constructor(r,t,n,a,o,s,i,c){this.injector=r,this.configLoader=t,this.rootComponentType=n,this.config=a,this.urlTree=o,this.paramsInheritanceStrategy=s,this.urlSerializer=i,this.abortSignal=c,this.applyRedirects=new ln(this.urlSerializer,this.urlTree)}noMatchError(r){return new R(4002,`'${r.segmentGroup}'`)}async recognize(){let r=yr(this.urlTree.root,[],[],this.config).segmentGroup,{children:t,rootSnapshot:n}=await this.match(r),a=new E(n,t),o=new Bt("",a),s=Nr(n,[],this.urlTree.queryParams,this.urlTree.fragment);return s.queryParams=this.urlTree.queryParams,o.url=this.urlSerializer.serialize(s),{state:o,tree:s}}async match(r){let t=new yt([],Object.freeze({}),Object.freeze(d({},this.urlTree.queryParams)),this.urlTree.fragment,Object.freeze({}),h,this.rootComponentType,null,{},this.injector);try{return{children:await this.processSegmentGroup(this.injector,this.config,r,h,t),rootSnapshot:t}}catch(n){if(n instanceof qt)return this.urlTree=n.urlTree,this.match(n.urlTree.root);throw n instanceof q?this.noMatchError(n):n}}async processSegmentGroup(r,t,n,a,o){if(n.segments.length===0&&n.hasChildren())return this.processChildren(r,t,n,o);let s=await this.processSegment(r,t,n,n.segments,a,!0,o);return s instanceof E?[s]:[]}async processChildren(r,t,n,a){let o=[];for(let c of Object.keys(n.children))c==="primary"?o.unshift(c):o.push(c);let s=[];for(let c of o){let l=n.children[c],m=fo(t,c),p=await this.processSegmentGroup(r,m,l,c,a);s.push(...p)}let i=Zr(s);return So(i),i}async processSegment(r,t,n,a,o,s,i){for(let c of t)try{return await this.processSegmentAgainstRoute(c._injector??r,t,c,n,a,o,s,i)}catch(l){if(l instanceof q||Vr(l))continue;throw l}if(Ro(n,a,o))return new dn;throw new q(n)}async processSegmentAgainstRoute(r,t,n,a,o,s,i,c){if(L(n)!==s&&(s===h||!Ne(a,o,n)))throw new q(a);if(n.redirectTo===void 0)return this.matchSegmentAgainstRoute(r,a,n,o,s,c);if(this.allowRedirects&&i)return this.expandSegmentAgainstRouteUsingRedirect(r,a,t,n,o,s,c);throw new q(a)}async expandSegmentAgainstRouteUsingRedirect(r,t,n,a,o,s,i){let{matched:c,parameters:l,consumedSegments:m,positionalParamSegments:p,remainingSegments:g}=Kr(t,a,o);if(!c)throw new q(t);typeof a.redirectTo=="string"&&a.redirectTo[0]==="/"&&(this.absoluteRedirectCount++,this.absoluteRedirectCount>Co&&(this.allowRedirects=!1));let J=this.createSnapshot(r,a,o,l,i);if(this.abortSignal.aborted)throw new Error(this.abortSignal.reason);let F=await this.applyRedirects.applyRedirectCommands(m,a.redirectTo,p,Yr(J),r),X=await this.applyRedirects.lineralizeSegments(a,F);return this.processSegment(r,n,t,X.concat(g),s,!1,i)}createSnapshot(r,t,n,a,o){let s=new yt(n,a,Object.freeze(d({},this.urlTree.queryParams)),this.urlTree.fragment,Io(t),L(t),t.component??t._loadedComponent??null,t,Ao(t),r),i=bn(s,o,this.paramsInheritanceStrategy);return s.params=Object.freeze(i.params),s.data=Object.freeze(i.data),s}async matchSegmentAgainstRoute(r,t,n,a,o,s){if(this.abortSignal.aborted)throw new Error(this.abortSignal.reason);let i=Yt=>this.createSnapshot(r,n,Yt.consumedSegments,Yt.parameters,s),c=await pe(bo(t,n,a,r,this.urlSerializer,i,this.abortSignal));if(n.path==="**"&&(t.children={}),!c?.matched)throw new q(t);r=n._injector??r;let{routes:l}=await this.getChildConfig(r,n,a),m=n._loadedInjector??r,{parameters:p,consumedSegments:g,remainingSegments:J}=c,F=this.createSnapshot(r,n,g,p,s),{segmentGroup:X,slicedSegments:It}=yr(t,g,J,l,o);if(It.length===0&&X.hasChildren()){let Yt=await this.processChildren(m,l,X,F);return new E(F,Yt)}if(l.length===0&&It.length===0)return new E(F,[]);let Pe=L(n)===o,At=await this.processSegment(m,l,X,It,Pe?h:o,!0,F);return new E(F,At instanceof E?[At]:[])}async getChildConfig(r,t,n){if(t.children)return{routes:t.children,injector:r};if(t.loadChildren){if(t._loadedRoutes!==void 0){let o=t._loadedNgModuleFactory;return o&&!t._loadedInjector&&(t._loadedInjector=o.create(r).injector),{routes:t._loadedRoutes,injector:t._loadedInjector}}if(this.abortSignal.aborted)throw new Error(this.abortSignal.reason);if(await pe(co(r,t,n,this.urlSerializer,this.abortSignal))){let o=await this.configLoader.loadChildren(r,t);return t._loadedRoutes=o.routes,t._loadedInjector=o.injector,t._loadedNgModuleFactory=o.factory,o}throw mo(t)}return{routes:[],injector:r}}};function So(e){e.sort((r,t)=>r.value.outlet===h?-1:t.value.outlet===h?1:r.value.outlet.localeCompare(t.value.outlet))}function _o(e){let r=e.value.routeConfig;return r&&r.path===""}function Zr(e){let r=[],t=new Set;for(let n of e){if(!_o(n)){r.push(n);continue}let a=r.find(o=>n.value.routeConfig===o.value.routeConfig);a!==void 0?(a.children.push(...n.children),t.add(a)):r.push(n)}for(let n of t){let a=Zr(n.children);r.push(new E(n.value,a))}return r.filter(n=>!t.has(n))}function Io(e){return e.data||{}}function Ao(e){return e.resolve||{}}function Mo(e,r,t,n,a,o,s){return B(async i=>{let{state:c,tree:l}=await xo(e,r,t,n,i.extractedUrl,a,o,s);return A(d({},i),{targetSnapshot:c,urlAfterRedirects:l})})}function Eo(e){return B(r=>{let{targetSnapshot:t,guards:{canActivateChecks:n}}=r;if(!n.length)return f(r);let a=new Set(n.map(i=>i.route)),o=new Set;for(let i of a)if(!o.has(i))for(let c of Jr(i))o.add(c);let s=0;return M(o).pipe(Jt(i=>a.has(i)?Do(i,t,e):(i.data=bn(i,i.parent,e).resolve,f(void 0))),P(()=>s++),$e(1),B(i=>s===o.size?f(r):N))})}function Jr(e){let r=e.children.map(t=>Jr(t)).flat();return[e,...r]}function Do(e,r,t){let n=e.routeConfig,a=e._resolve;return n?.title!==void 0&&!$r(n)&&(a[Vt]=n.title),Kt(()=>(e.data=bn(e,e.parent,t).resolve,To(a,e,r).pipe(S(o=>(e._resolvedData=o,e.data=d(d({},e.data),o),null)))))}function To(e,r,t){let n=Je(e);if(n.length===0)return f({});let a={};return M(n).pipe(B(o=>No(e[o],r,t).pipe(W(),P(s=>{if(s instanceof wt)throw Ee(new K,s);a[o]=s}))),$e(1),S(()=>a),Zt(o=>Vr(o)?N:Mn(o)))}function No(e,r,t){let n=r._environmentInjector,a=St(e,n),o=a.resolve?a.resolve(r,t):k(n,()=>a(r,t));return ct(o)}function wr(e){return U(r=>{let t=e(r);return t?M(t).pipe(S(()=>r)):f(r)})}var xn=(()=>{class e{buildTitle(t){let n,a=t.root;for(;a!==void 0;)n=this.getResolvedTitleForRoute(a)??n,a=a.children.find(o=>o.outlet===h);return n}getResolvedTitleForRoute(t){return t.data[Vt]}static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:()=>u(Xr),providedIn:"root"})}return e})(),Xr=(()=>{class e extends xn{title;constructor(t){super(),this.title=t}updateTitle(t){let n=this.buildTitle(t);n!==void 0&&this.title.setTitle(n)}static \u0275fac=function(n){return new(n||e)(te(sr))};static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})(),_t=new _("",{factory:()=>({})}),Qt=new _(""),ta=(()=>{class e{componentLoaders=new WeakMap;childrenLoaders=new WeakMap;onLoadStartListener;onLoadEndListener;compiler=u(tr);async loadComponent(t,n){if(this.componentLoaders.get(n))return this.componentLoaders.get(n);if(n._loadedComponent)return Promise.resolve(n._loadedComponent);this.onLoadStartListener&&this.onLoadStartListener(n);let a=(async()=>{try{let o=await Cr(k(t,()=>n.loadComponent())),s=await ra(na(o));return this.onLoadEndListener&&this.onLoadEndListener(n),n._loadedComponent=s,s}finally{this.componentLoaders.delete(n)}})();return this.componentLoaders.set(n,a),a}loadChildren(t,n){if(this.childrenLoaders.get(n))return this.childrenLoaders.get(n);if(n._loadedRoutes)return Promise.resolve({routes:n._loadedRoutes,injector:n._loadedInjector});this.onLoadStartListener&&this.onLoadStartListener(n);let a=(async()=>{try{let o=await ea(n,this.compiler,t,this.onLoadEndListener);return n._loadedRoutes=o.routes,n._loadedInjector=o.injector,n._loadedNgModuleFactory=o.factory,o}finally{this.childrenLoaders.delete(n)}})();return this.childrenLoaders.set(n,a),a}static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})();async function ea(e,r,t,n){let a=await Cr(k(t,()=>e.loadChildren())),o=await ra(na(a)),s;o instanceof Fn||Array.isArray(o)?s=o:s=await r.compileModuleAsync(o),n&&n(e);let i,c,l=!1,m;return Array.isArray(s)?(c=s,l=!0):(i=s.create(t).injector,m=s,c=i.get(Qt,[],{optional:!0,self:!0}).flat()),{routes:c.map(Rn),injector:i,factory:m}}function ko(e){return e&&typeof e=="object"&&"default"in e}function na(e){return ko(e)?e.default:e}async function ra(e){return e}var ke=(()=>{class e{static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:()=>u(Oo),providedIn:"root"})}return e})(),Oo=(()=>{class e{shouldProcessUrl(t){return!0}extract(t){return t}merge(t,n){return t}static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})(),aa=new _("");var Uo=()=>{},oa=new _(""),ia=(()=>{class e{currentNavigation=y(null,{equal:()=>!1});currentTransition=null;lastSuccessfulNavigation=y(null);events=new tt;transitionAbortWithErrorSubject=new tt;configLoader=u(ta);environmentInjector=u(Q);destroyRef=u(Be);urlSerializer=u(xt);rootContexts=u(Ct);location=u(ce);inputBindingEnabled=u(Gt,{optional:!0})!==null;titleStrategy=u(xn);options=u(_t,{optional:!0})||{};paramsInheritanceStrategy=this.options.paramsInheritanceStrategy||"emptyOnly";urlHandlingStrategy=u(ke);createViewTransition=u(aa,{optional:!0});navigationErrorHandler=u(oa,{optional:!0});navigationId=0;get hasRequestedNavigation(){return this.navigationId!==0}transitions;afterPreactivation=()=>f(void 0);rootComponentType=null;destroyed=!1;constructor(){let t=a=>this.events.next(new Re(a)),n=a=>this.events.next(new xe(a));this.configLoader.onLoadEndListener=n,this.configLoader.onLoadStartListener=t,this.destroyRef.onDestroy(()=>{this.destroyed=!0})}complete(){this.transitions?.complete()}handleNavigationRequest(t){let n=++this.navigationId;v(()=>{this.transitions?.next(A(d({},t),{extractedUrl:this.urlHandlingStrategy.extract(t.rawUrl),targetSnapshot:null,targetRouterState:null,guards:{canActivateChecks:[],canDeactivateChecks:[]},guardsResult:null,id:n,routesRecognizeHandler:{},beforeActivateHandler:{}}))})}setupNavigations(t){return this.transitions=new T(null),this.transitions.pipe(et(n=>n!==null),U(n=>{let a=!1,o=new AbortController,s=()=>!a&&this.currentTransition?.id===n.id;return f(n).pipe(U(i=>{if(this.navigationId>n.id)return this.cancelNavigationTransition(n,"",C.SupersededByNewNavigation),N;this.currentTransition=n;let c=this.lastSuccessfulNavigation();this.currentNavigation.set({id:i.id,initialUrl:i.rawUrl,extractedUrl:i.extractedUrl,targetBrowserUrl:typeof i.extras.browserUrl=="string"?this.urlSerializer.parse(i.extras.browserUrl):i.extras.browserUrl,trigger:i.source,extras:i.extras,previousNavigation:c?A(d({},c),{previousNavigation:null}):null,abort:()=>o.abort(),routesRecognizeHandler:i.routesRecognizeHandler,beforeActivateHandler:i.beforeActivateHandler});let l=!t.navigated||this.isUpdatingInternalState()||this.isUpdatedBrowserUrl(),m=i.extras.onSameUrlNavigation??t.onSameUrlNavigation;if(!l&&m!=="reload")return this.events.next(new V(i.id,this.urlSerializer.serialize(i.rawUrl),"",jt.IgnoredSameUrlNavigation)),i.resolve(!1),N;if(this.urlHandlingStrategy.shouldProcessUrl(i.rawUrl))return f(i).pipe(U(p=>(this.events.next(new it(p.id,this.urlSerializer.serialize(p.extractedUrl),p.source,p.restoredState)),p.id!==this.navigationId?N:Promise.resolve(p))),Mo(this.environmentInjector,this.configLoader,this.rootComponentType,t.config,this.urlSerializer,this.paramsInheritanceStrategy,o.signal),P(p=>{n.targetSnapshot=p.targetSnapshot,n.urlAfterRedirects=p.urlAfterRedirects,this.currentNavigation.update(g=>(g.finalUrl=p.urlAfterRedirects,g)),this.events.next(new $t)}),U(p=>M(n.routesRecognizeHandler.deferredHandle??f(void 0)).pipe(S(()=>p))),P(()=>{let p=new zt(i.id,this.urlSerializer.serialize(i.extractedUrl),this.urlSerializer.serialize(i.urlAfterRedirects),i.targetSnapshot);this.events.next(p)}));if(l&&this.urlHandlingStrategy.shouldProcessUrl(i.currentRawUrl)){let{id:p,extractedUrl:g,source:J,restoredState:F,extras:X}=i,It=new it(p,this.urlSerializer.serialize(g),J,F);this.events.next(It);let Pe=jr(this.rootComponentType,this.environmentInjector).snapshot;return this.currentTransition=n=A(d({},i),{targetSnapshot:Pe,urlAfterRedirects:g,extras:A(d({},X),{skipLocationChange:!1,replaceUrl:!1})}),this.currentNavigation.update(At=>(At.finalUrl=g,At)),f(n)}else return this.events.next(new V(i.id,this.urlSerializer.serialize(i.extractedUrl),"",jt.IgnoredByUrlHandlingStrategy)),i.resolve(!1),N}),S(i=>{let c=new ge(i.id,this.urlSerializer.serialize(i.extractedUrl),this.urlSerializer.serialize(i.urlAfterRedirects),i.targetSnapshot);return this.events.next(c),this.currentTransition=n=A(d({},i),{guards:Ha(i.targetSnapshot,i.currentSnapshot,this.rootContexts)}),n}),to(i=>this.events.next(i)),U(i=>{if(n.guardsResult=i.guardsResult,i.guardsResult&&typeof i.guardsResult!="boolean")throw Ee(this.urlSerializer,i.guardsResult);let c=new ve(i.id,this.urlSerializer.serialize(i.extractedUrl),this.urlSerializer.serialize(i.urlAfterRedirects),i.targetSnapshot,!!i.guardsResult);if(this.events.next(c),!s())return N;if(!i.guardsResult)return this.cancelNavigationTransition(i,"",C.GuardRejected),N;if(i.guards.canActivateChecks.length===0)return f(i);let l=new ye(i.id,this.urlSerializer.serialize(i.extractedUrl),this.urlSerializer.serialize(i.urlAfterRedirects),i.targetSnapshot);if(this.events.next(l),!s())return N;let m=!1;return f(i).pipe(Eo(this.paramsInheritanceStrategy),P({next:()=>{m=!0;let p=new we(i.id,this.urlSerializer.serialize(i.extractedUrl),this.urlSerializer.serialize(i.urlAfterRedirects),i.targetSnapshot);this.events.next(p)},complete:()=>{m||this.cancelNavigationTransition(i,"",C.NoDataFromResolver)}}))}),wr(i=>{let c=m=>{let p=[];if(m.routeConfig?._loadedComponent)m.component=m.routeConfig?._loadedComponent;else if(m.routeConfig?.loadComponent){let g=m._environmentInjector;p.push(this.configLoader.loadComponent(g,m.routeConfig).then(J=>{m.component=J}))}for(let g of m.children)p.push(...c(g));return p},l=c(i.targetSnapshot.root);return l.length===0?f(i):M(Promise.all(l).then(()=>i))}),wr(()=>this.afterPreactivation()),U(()=>{let{currentSnapshot:i,targetSnapshot:c}=n,l=this.createViewTransition?.(this.environmentInjector,i.root,c.root);return l?M(l).pipe(S(()=>n)):f(n)}),ut(1),U(i=>{let c=za(t.routeReuseStrategy,i.targetSnapshot,i.currentRouterState);this.currentTransition=n=i=A(d({},i),{targetRouterState:c}),this.currentNavigation.update(m=>(m.targetRouterState=c,m)),this.events.next(new gt);let l=n.beforeActivateHandler.deferredHandle;return l?M(l.then(()=>i)):f(i)}),P(i=>{new cn(t.routeReuseStrategy,n.targetRouterState,n.currentRouterState,c=>this.events.next(c),this.inputBindingEnabled).activate(this.rootContexts),s()&&(a=!0,this.currentNavigation.update(c=>(c.abort=Uo,c)),this.lastSuccessfulNavigation.set(v(this.currentNavigation)),this.events.next(new j(i.id,this.urlSerializer.serialize(i.extractedUrl),this.urlSerializer.serialize(i.urlAfterRedirects))),this.titleStrategy?.updateTitle(i.targetRouterState.snapshot),i.resolve(!0))}),Xt(Gr(o.signal).pipe(et(()=>!a&&!n.targetRouterState),P(()=>{this.cancelNavigationTransition(n,o.signal.reason+"",C.Aborted)}))),P({complete:()=>{a=!0}}),Xt(this.transitionAbortWithErrorSubject.pipe(P(i=>{throw i}))),Tn(()=>{o.abort(),a||this.cancelNavigationTransition(n,"",C.SupersededByNewNavigation),this.currentTransition?.id===n.id&&(this.currentNavigation.set(null),this.currentTransition=null)}),Zt(i=>{if(a=!0,this.destroyed)return n.resolve(!1),N;if(qr(i))this.events.next(new O(n.id,this.urlSerializer.serialize(n.extractedUrl),i.message,i.cancellationCode)),Ba(i)?this.events.next(new vt(i.url,i.navigationBehaviorOptions)):n.resolve(!1);else{let c=new st(n.id,this.urlSerializer.serialize(n.extractedUrl),i,n.targetSnapshot??void 0);try{let l=k(this.environmentInjector,()=>this.navigationErrorHandler?.(c));if(l instanceof wt){let{message:m,cancellationCode:p}=Ee(this.urlSerializer,l);this.events.next(new O(n.id,this.urlSerializer.serialize(n.extractedUrl),m,p)),this.events.next(new vt(l.redirectTo,l.navigationBehaviorOptions))}else throw this.events.next(c),i}catch(l){this.options.resolveNavigationPromiseOnError?n.resolve(!1):n.reject(l)}}return N}))}))}cancelNavigationTransition(t,n,a){let o=new O(t.id,this.urlSerializer.serialize(t.extractedUrl),n,a);this.events.next(o),t.resolve(!1)}isUpdatingInternalState(){return this.currentTransition?.extractedUrl.toString()!==this.currentTransition?.currentUrlTree.toString()}isUpdatedBrowserUrl(){let t=this.urlHandlingStrategy.extract(this.urlSerializer.parse(this.location.path(!0))),n=v(this.currentNavigation),a=n?.targetBrowserUrl??n?.extractedUrl;return t.toString()!==a?.toString()&&!n?.extras.skipLocationChange}static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})();function Po(e){return e!==Ot}var sa=new _("");var ca=(()=>{class e{static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:()=>u(Lo),providedIn:"root"})}return e})(),Te=class{shouldDetach(r){return!1}store(r,t){}shouldAttach(r){return!1}retrieve(r){return null}shouldReuseRoute(r,t){return r.routeConfig===t.routeConfig}shouldDestroyInjector(r){return!0}},Lo=(()=>{class e extends Te{static \u0275fac=(()=>{let t;return function(a){return(t||(t=qe(e)))(a||e)}})();static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})(),Oe=(()=>{class e{urlSerializer=u(xt);options=u(_t,{optional:!0})||{};canceledNavigationResolution=this.options.canceledNavigationResolution||"replace";location=u(ce);urlHandlingStrategy=u(ke);urlUpdateStrategy=this.options.urlUpdateStrategy||"deferred";currentUrlTree=new I;getCurrentUrlTree(){return this.currentUrlTree}rawUrlTree=this.currentUrlTree;getRawUrlTree(){return this.rawUrlTree}createBrowserPath({finalUrl:t,initialUrl:n,targetBrowserUrl:a}){let o=t!==void 0?this.urlHandlingStrategy.merge(t,n):n,s=a??o;return s instanceof I?this.urlSerializer.serialize(s):s}routerUrlState(t){return t?.targetBrowserUrl===void 0||t?.finalUrl===void 0?{}:{\u0275routerUrl:this.urlSerializer.serialize(t.finalUrl)}}commitTransition({targetRouterState:t,finalUrl:n,initialUrl:a}){n&&t?(this.currentUrlTree=n,this.rawUrlTree=this.urlHandlingStrategy.merge(n,a),this.routerState=t):this.rawUrlTree=a}routerState=jr(null,u(Q));getRouterState(){return this.routerState}_stateMemento=this.createStateMemento();get stateMemento(){return this._stateMemento}updateStateMemento(){this._stateMemento=this.createStateMemento()}createStateMemento(){return{rawUrlTree:this.rawUrlTree,currentUrlTree:this.currentUrlTree,routerState:this.routerState}}restoredState(){return this.location.getState()}static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:()=>u(jo),providedIn:"root"})}return e})(),jo=(()=>{class e extends Oe{currentPageId=0;lastSuccessfulId=-1;get browserPageId(){return this.canceledNavigationResolution!=="computed"?this.currentPageId:this.restoredState()?.\u0275routerPageId??this.currentPageId}registerNonRouterCurrentEntryChangeListener(t){return this.location.subscribe(n=>{n.type==="popstate"&&setTimeout(()=>{t(n.url,n.state,"popstate",{replaceUrl:!0})})})}handleRouterEvent(t,n){t instanceof it?this.updateStateMemento():t instanceof V?this.commitTransition(n):t instanceof zt?this.urlUpdateStrategy==="eager"&&(n.extras.skipLocationChange||this.setBrowserUrl(this.createBrowserPath(n),n)):t instanceof gt?(this.commitTransition(n),this.urlUpdateStrategy==="deferred"&&!n.extras.skipLocationChange&&this.setBrowserUrl(this.createBrowserPath(n),n)):t instanceof O&&!Lr(t)?this.restoreHistory(n):t instanceof st?this.restoreHistory(n,!0):t instanceof j&&(this.lastSuccessfulId=t.id,this.currentPageId=this.browserPageId)}setBrowserUrl(t,n){let{extras:a,id:o}=n,{replaceUrl:s,state:i}=a;if(this.location.isCurrentPathEqualTo(t)||s){let c=this.browserPageId,l=d(d({},i),this.generateNgRouterState(o,c,n));this.location.replaceState(t,"",l)}else{let c=d(d({},i),this.generateNgRouterState(o,this.browserPageId+1,n));this.location.go(t,"",c)}}restoreHistory(t,n=!1){if(this.canceledNavigationResolution==="computed"){let a=this.browserPageId,o=this.currentPageId-a;o!==0?this.location.historyGo(o):this.getCurrentUrlTree()===t.finalUrl&&o===0&&(this.resetInternalState(t),this.resetUrlToCurrentUrlTree())}else this.canceledNavigationResolution==="replace"&&(n&&this.resetInternalState(t),this.resetUrlToCurrentUrlTree())}resetInternalState({finalUrl:t}){this.routerState=this.stateMemento.routerState,this.currentUrlTree=this.stateMemento.currentUrlTree,this.rawUrlTree=this.urlHandlingStrategy.merge(this.currentUrlTree,t??this.rawUrlTree)}resetUrlToCurrentUrlTree(){this.location.replaceState(this.urlSerializer.serialize(this.getRawUrlTree()),"",this.generateNgRouterState(this.lastSuccessfulId,this.currentPageId))}generateNgRouterState(t,n,a){return this.canceledNavigationResolution==="computed"?d({navigationId:t,\u0275routerPageId:n},this.routerUrlState(a)):d({navigationId:t},this.routerUrlState(a))}static \u0275fac=(()=>{let t;return function(a){return(t||(t=qe(e)))(a||e)}})();static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})();function Cn(e,r){e.events.pipe(et(t=>t instanceof j||t instanceof O||t instanceof st||t instanceof V),S(t=>t instanceof j||t instanceof V?0:(t instanceof O?t.code===C.Redirect||t.code===C.SupersededByNewNavigation:!1)?2:1),et(t=>t!==2),ut(1)).subscribe(()=>{r()})}var lt=(()=>{class e{get currentUrlTree(){return this.stateManager.getCurrentUrlTree()}get rawUrlTree(){return this.stateManager.getRawUrlTree()}disposed=!1;nonRouterCurrentEntryChangeSubscription;console=u(Bn);stateManager=u(Oe);options=u(_t,{optional:!0})||{};pendingTasks=u(Ln);urlUpdateStrategy=this.options.urlUpdateStrategy||"deferred";navigationTransitions=u(ia);urlSerializer=u(xt);location=u(ce);urlHandlingStrategy=u(ke);injector=u(Q);_events=new tt;get events(){return this._events}get routerState(){return this.stateManager.getRouterState()}navigated=!1;routeReuseStrategy=u(ca);injectorCleanup=u(sa,{optional:!0});onSameUrlNavigation=this.options.onSameUrlNavigation||"ignore";config=u(Qt,{optional:!0})?.flat()??[];componentInputBindingEnabled=!!u(Gt,{optional:!0});currentNavigation=this.navigationTransitions.currentNavigation.asReadonly();constructor(){this.resetConfig(this.config),this.navigationTransitions.setupNavigations(this).subscribe({error:t=>{}}),this.subscribeToNavigationEvents()}eventsSubscription=new _n;subscribeToNavigationEvents(){let t=this.navigationTransitions.events.subscribe(n=>{try{let a=this.navigationTransitions.currentTransition,o=v(this.navigationTransitions.currentNavigation);if(a!==null&&o!==null){if(this.stateManager.handleRouterEvent(n,o),n instanceof O&&n.code!==C.Redirect&&n.code!==C.SupersededByNewNavigation)this.navigated=!0;else if(n instanceof j)this.navigated=!0,this.injectorCleanup?.(this.routeReuseStrategy,this.routerState,this.config);else if(n instanceof vt){let s=n.navigationBehaviorOptions,i=this.urlHandlingStrategy.merge(n.url,a.currentRawUrl),c=d({scroll:a.extras.scroll,browserUrl:a.extras.browserUrl,info:a.extras.info,skipLocationChange:a.extras.skipLocationChange,replaceUrl:a.extras.replaceUrl||this.urlUpdateStrategy==="eager"||Po(a.source)},s);this.scheduleNavigation(i,Ot,null,c,{resolve:a.resolve,reject:a.reject,promise:a.promise})}}La(n)&&this._events.next(n)}catch(a){this.navigationTransitions.transitionAbortWithErrorSubject.next(a)}});this.eventsSubscription.add(t)}resetRootComponentType(t){this.routerState.root.component=t,this.navigationTransitions.rootComponentType=t}initialNavigation(){this.setUpLocationChangeListener(),this.navigationTransitions.hasRequestedNavigation||this.navigateToSyncWithBrowser(this.location.path(!0),Ot,this.stateManager.restoredState(),{replaceUrl:!0})}setUpLocationChangeListener(){this.nonRouterCurrentEntryChangeSubscription??=this.stateManager.registerNonRouterCurrentEntryChangeListener((t,n,a,o)=>{this.navigateToSyncWithBrowser(t,a,n,o)})}navigateToSyncWithBrowser(t,n,a,o){let s=a?.navigationId?a:null,i=a?.\u0275routerUrl??t;if(a?.\u0275routerUrl&&(o=A(d({},o),{browserUrl:t})),a){let l=d({},a);delete l.navigationId,delete l.\u0275routerPageId,delete l.\u0275routerUrl,Object.keys(l).length!==0&&(o.state=l)}let c=this.parseUrl(i);this.scheduleNavigation(c,n,s,o).catch(l=>{this.disposed||this.injector.get(ee)(l)})}get url(){return this.serializeUrl(this.currentUrlTree)}getCurrentNavigation(){return v(this.navigationTransitions.currentNavigation)}get lastSuccessfulNavigation(){return this.navigationTransitions.lastSuccessfulNavigation}resetConfig(t){this.config=t.map(Rn),this.navigated=!1}ngOnDestroy(){this.dispose()}dispose(){this._events.unsubscribe(),this.navigationTransitions.complete(),this.nonRouterCurrentEntryChangeSubscription?.unsubscribe(),this.nonRouterCurrentEntryChangeSubscription=void 0,this.disposed=!0,this.eventsSubscription.unsubscribe()}createUrlTree(t,n={}){let{relativeTo:a,queryParams:o,fragment:s,queryParamsHandling:i,preserveFragment:c}=n,l=c?this.currentUrlTree.fragment:s,m=null;switch(i??this.options.defaultQueryParamsHandling){case"merge":m=d(d({},this.currentUrlTree.queryParams),o);break;case"preserve":m=this.currentUrlTree.queryParams;break;default:m=o||null}m!==null&&(m=this.removeEmptyProps(m));let p;try{let g=a?a.snapshot:this.routerState.snapshot.root;p=kr(g)}catch{(typeof t[0]!="string"||t[0][0]!=="/")&&(t=[]),p=this.currentUrlTree.root}return Or(p,t,m,l??null,this.urlSerializer)}navigateByUrl(t,n={skipLocationChange:!1}){let a=Z(t)?t:this.parseUrl(t),o=this.urlHandlingStrategy.merge(a,this.rawUrlTree);return this.scheduleNavigation(o,Ot,null,n)}navigate(t,n={skipLocationChange:!1}){return zo(t),this.navigateByUrl(this.createUrlTree(t,n),n)}serializeUrl(t){return this.urlSerializer.serialize(t)}parseUrl(t){try{return this.urlSerializer.parse(t)}catch{return this.console.warn(kn(4018,!1)),this.urlSerializer.parse("/")}}isActive(t,n){let a;if(n===!0?a=d({},pn):n===!1?a=d({},Pt):a=d(d({},Pt),n),Z(t))return Xe(this.currentUrlTree,t,a);let o=this.parseUrl(t);return Xe(this.currentUrlTree,o,a)}removeEmptyProps(t){return Object.entries(t).reduce((n,[a,o])=>(o!=null&&(n[a]=o),n),{})}scheduleNavigation(t,n,a,o,s){if(this.disposed)return Promise.resolve(!1);let i,c,l;s?(i=s.resolve,c=s.reject,l=s.promise):l=new Promise((p,g)=>{i=p,c=g});let m=this.pendingTasks.add();return Cn(this,()=>{queueMicrotask(()=>this.pendingTasks.remove(m))}),this.navigationTransitions.handleNavigationRequest({source:n,restoredState:a,currentUrlTree:this.currentUrlTree,currentRawUrl:this.currentUrlTree,rawUrl:t,extras:o,resolve:i,reject:c,promise:l,currentSnapshot:this.routerState.snapshot,currentRouterState:this.routerState}),l.catch(Promise.reject.bind(Promise))}static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})();function zo(e){for(let r=0;r<e.length;r++)if(e[r]==null)throw new R(4008,!1)}var Fo=(()=>{class e{router=u(lt);stateManager=u(Oe);fragment=y("");queryParams=y({});path=y("");serializer=u(xt);constructor(){this.updateState(),this.router.events?.subscribe(t=>{t instanceof j&&this.updateState()})}updateState(){let{fragment:t,root:n,queryParams:a}=this.stateManager.getCurrentUrlTree();this.fragment.set(t),this.queryParams.set(a),this.path.set(this.serializer.serialize(new I(n)))}static \u0275fac=function(n){return new(n||e)};static \u0275prov=x({token:e,factory:e.\u0275fac,providedIn:"root"})}return e})(),Ue=(()=>{class e{router;route;tabIndexAttribute;renderer;el;locationStrategy;hrefAttributeValue=u(new nr("href"),{optional:!0});reactiveHref=er(()=>this.isAnchorElement?this.computeHref(this._urlTree()):this.hrefAttributeValue);get href(){return v(this.reactiveHref)}set href(t){this.reactiveHref.set(t)}set target(t){this._target.set(t)}get target(){return v(this._target)}_target=y(void 0);set queryParams(t){this._queryParams.set(t)}get queryParams(){return v(this._queryParams)}_queryParams=y(void 0,{equal:()=>!1});set fragment(t){this._fragment.set(t)}get fragment(){return v(this._fragment)}_fragment=y(void 0);set queryParamsHandling(t){this._queryParamsHandling.set(t)}get queryParamsHandling(){return v(this._queryParamsHandling)}_queryParamsHandling=y(void 0);set state(t){this._state.set(t)}get state(){return v(this._state)}_state=y(void 0,{equal:()=>!1});set info(t){this._info.set(t)}get info(){return v(this._info)}_info=y(void 0,{equal:()=>!1});set relativeTo(t){this._relativeTo.set(t)}get relativeTo(){return v(this._relativeTo)}_relativeTo=y(void 0);set preserveFragment(t){this._preserveFragment.set(t)}get preserveFragment(){return v(this._preserveFragment)}_preserveFragment=y(!1);set skipLocationChange(t){this._skipLocationChange.set(t)}get skipLocationChange(){return v(this._skipLocationChange)}_skipLocationChange=y(!1);set replaceUrl(t){this._replaceUrl.set(t)}get replaceUrl(){return v(this._replaceUrl)}_replaceUrl=y(!1);isAnchorElement;onChanges=new tt;applicationErrorHandler=u(ee);options=u(_t,{optional:!0});reactiveRouterState=u(Fo);constructor(t,n,a,o,s,i){this.router=t,this.route=n,this.tabIndexAttribute=a,this.renderer=o,this.el=s,this.locationStrategy=i;let c=s.nativeElement.tagName?.toLowerCase();this.isAnchorElement=c==="a"||c==="area"||!!(typeof customElements=="object"&&customElements.get(c)?.observedAttributes?.includes?.("href"))}setTabIndexIfNotOnNativeEl(t){this.tabIndexAttribute!=null||this.isAnchorElement||this.applyAttributeValue("tabindex",t)}ngOnChanges(t){this.onChanges.next(this)}routerLinkInput=y(null);set routerLink(t){t==null?(this.routerLinkInput.set(null),this.setTabIndexIfNotOnNativeEl(null)):(Z(t)?this.routerLinkInput.set(t):this.routerLinkInput.set(Array.isArray(t)?t:[t]),this.setTabIndexIfNotOnNativeEl("0"))}onClick(t,n,a,o,s){let i=this._urlTree();if(i===null||this.isAnchorElement&&(t!==0||n||a||o||s||typeof this.target=="string"&&this.target!="_self"))return!0;let c={skipLocationChange:this.skipLocationChange,replaceUrl:this.replaceUrl,state:this.state,info:this.info};return this.router.navigateByUrl(i,c)?.catch(l=>{this.applicationErrorHandler(l)}),!this.isAnchorElement}ngOnDestroy(){}applyAttributeValue(t,n){let a=this.renderer,o=this.el.nativeElement;n!==null?a.setAttribute(o,t,n):a.removeAttribute(o,t)}_urlTree=ie(()=>{this.reactiveRouterState.path(),this._preserveFragment()&&this.reactiveRouterState.fragment();let t=a=>a==="preserve"||a==="merge";(t(this._queryParamsHandling())||t(this.options?.defaultQueryParamsHandling))&&this.reactiveRouterState.queryParams();let n=this.routerLinkInput();return n===null||!this.router.createUrlTree?null:Z(n)?n:this.router.createUrlTree(n,{relativeTo:this._relativeTo()!==void 0?this._relativeTo():this.route,queryParams:this._queryParams(),fragment:this._fragment(),queryParamsHandling:this._queryParamsHandling(),preserveFragment:this._preserveFragment()})},{equal:(t,n)=>this.computeHref(t)===this.computeHref(n)});get urlTree(){return v(this._urlTree)}computeHref(t){return t!==null&&this.locationStrategy?this.locationStrategy?.prepareExternalUrl(this.router.serializeUrl(t))??"":null}static \u0275fac=function(n){return new(n||e)(z(lt),z(G),jn("tabindex"),z(Dt),z(Et),z(ir))};static \u0275dir=rt({type:e,selectors:[["","routerLink",""]],hostVars:2,hostBindings:function(n,a){n&1&&Yn("click",function(s){return a.onClick(s.button,s.ctrlKey,s.shiftKey,s.altKey,s.metaKey)}),n&2&&re("href",a.reactiveHref(),zn)("target",a._target())},inputs:{target:"target",queryParams:"queryParams",fragment:"fragment",queryParamsHandling:"queryParamsHandling",state:"state",info:"info",relativeTo:"relativeTo",preserveFragment:[2,"preserveFragment","preserveFragment",H],skipLocationChange:[2,"skipLocationChange","skipLocationChange",H],replaceUrl:[2,"replaceUrl","replaceUrl",H],routerLink:"routerLink"},features:[Mt]})}return e})(),Bo=(()=>{class e{router;element;renderer;cdr;links;classes=[];routerEventsSubscription;linkInputChangesSubscription;_isActive=!1;get isActive(){return this._isActive}routerLinkActiveOptions={exact:!1};ariaCurrentWhenActive;isActiveChange=new nt;link=u(Ue,{optional:!0});constructor(t,n,a,o){this.router=t,this.element=n,this.renderer=a,this.cdr=o,this.routerEventsSubscription=t.events.subscribe(s=>{s instanceof j&&this.update()})}ngAfterContentInit(){f(this.links.changes,f(null)).pipe(ze()).subscribe(t=>{this.update(),this.subscribeToEachLinkOnChanges()})}subscribeToEachLinkOnChanges(){this.linkInputChangesSubscription?.unsubscribe();let t=[...this.links.toArray(),this.link].filter(n=>!!n).map(n=>n.onChanges);this.linkInputChangesSubscription=M(t).pipe(ze()).subscribe(n=>{this._isActive!==this.isLinkActive(this.router)(n)&&this.update()})}set routerLinkActive(t){let n=Array.isArray(t)?t:t.split(" ");this.classes=n.filter(a=>!!a)}ngOnChanges(t){this.update()}ngOnDestroy(){this.routerEventsSubscription.unsubscribe(),this.linkInputChangesSubscription?.unsubscribe()}update(){!this.links||!this.router.navigated||queueMicrotask(()=>{let t=this.hasActiveLinks();this.classes.forEach(n=>{t?this.renderer.addClass(this.element.nativeElement,n):this.renderer.removeClass(this.element.nativeElement,n)}),t&&this.ariaCurrentWhenActive!==void 0?this.renderer.setAttribute(this.element.nativeElement,"aria-current",this.ariaCurrentWhenActive.toString()):this.renderer.removeAttribute(this.element.nativeElement,"aria-current"),this._isActive!==t&&(this._isActive=t,this.cdr.markForCheck(),this.isActiveChange.emit(t))})}isLinkActive(t){let n=Ho(this.routerLinkActiveOptions)?this.routerLinkActiveOptions:this.routerLinkActiveOptions.exact??!1?d({},pn):d({},Pt);return a=>{let o=a.urlTree;return o?v(fn(o,t,n)):!1}}hasActiveLinks(){let t=this.isLinkActive(this.router);return this.link&&t(this.link)||this.links.some(t)}static \u0275fac=function(n){return new(n||e)(z(lt),z(Et),z(Dt),z(se))};static \u0275dir=rt({type:e,selectors:[["","routerLinkActive",""]],contentQueries:function(n,a,o){if(n&1&&Kn(o,Ue,5),n&2){let s;Zn(s=Jn())&&(a.links=s)}},inputs:{routerLinkActiveOptions:"routerLinkActiveOptions",ariaCurrentWhenActive:"ariaCurrentWhenActive",routerLinkActive:"routerLinkActive"},outputs:{isActiveChange:"isActiveChange"},exportAs:["routerLinkActive"],features:[Mt]})}return e})();function Ho(e){let r=e;return!!(r.paths||r.matrixParams||r.queryParams||r.fragment)}var qo=new _("");function Vo(e,...r){return Un([{provide:Qt,multi:!0,useValue:e},[],{provide:G,useFactory:Go},{provide:qn,multi:!0,useFactory:Qo},r.map(t=>t.\u0275providers)])}function Go(){return u(lt).routerState.root}function Wo(e,r){return{\u0275kind:e,\u0275providers:r}}function Qo(){let e=u(Pn);return r=>{let t=e.get(Vn);if(r!==t.components[0])return;let n=e.get(lt),a=e.get(Yo);e.get(Ko)===1&&n.initialNavigation(),e.get(Zo,null,{optional:!0})?.setUpPreloading(),e.get(qo,null,{optional:!0})?.init(),n.resetRootComponentType(t.componentTypes[0]),a.closed||(a.next(),a.complete(),a.unsubscribe())}}var Yo=new _("",{factory:()=>new tt}),Ko=new _("",{factory:()=>1});var Zo=new _("");function Jo(){return Wo(8,[yn,{provide:Gt,useExisting:yn}])}var Xo=["mat-icon-button",""],ti=["*"],ei=new _("MAT_BUTTON_CONFIG");function la(e){return e==null?void 0:ar(e)}var Sn=(()=>{class e{_elementRef=u(Et);_ngZone=u(He);_animationsDisabled=ur();_config=u(ei,{optional:!0});_focusMonitor=u(cr);_cleanupClick;_renderer=u(Dt);_rippleLoader=u(dr);_isAnchor;_isFab=!1;color;get disableRipple(){return this._disableRipple}set disableRipple(t){this._disableRipple=t,this._updateRippleDisabled()}_disableRipple=!1;get disabled(){return this._disabled}set disabled(t){this._disabled=t,this._updateRippleDisabled()}_disabled=!1;ariaDisabled;disabledInteractive;tabIndex;set _tabindex(t){this.tabIndex=t}constructor(){u(lr).load(mr);let t=this._elementRef.nativeElement;this._isAnchor=t.tagName==="A",this.disabledInteractive=this._config?.disabledInteractive??!1,this.color=this._config?.color??null,this._rippleLoader?.configureRipple(t,{className:"mat-mdc-button-ripple"})}ngAfterViewInit(){this._focusMonitor.monitor(this._elementRef,!0),this._isAnchor&&this._setupAsAnchor()}ngOnDestroy(){this._cleanupClick?.(),this._focusMonitor.stopMonitoring(this._elementRef),this._rippleLoader?.destroyRipple(this._elementRef.nativeElement)}focus(t="program",n){t?this._focusMonitor.focusVia(this._elementRef.nativeElement,t,n):this._elementRef.nativeElement.focus(n)}_getAriaDisabled(){return this.ariaDisabled!=null?this.ariaDisabled:this._isAnchor?this.disabled||null:this.disabled&&this.disabledInteractive?!0:null}_getDisabledAttribute(){return this.disabledInteractive||!this.disabled?null:!0}_updateRippleDisabled(){this._rippleLoader?.setDisabled(this._elementRef.nativeElement,this.disableRipple||this.disabled)}_getTabIndex(){return this._isAnchor?this.disabled&&!this.disabledInteractive?-1:this.tabIndex:this.tabIndex}_setupAsAnchor(){this._cleanupClick=this._ngZone.runOutsideAngular(()=>this._renderer.listen(this._elementRef.nativeElement,"click",t=>{this.disabled&&(t.preventDefault(),t.stopImmediatePropagation())}))}static \u0275fac=function(n){return new(n||e)};static \u0275dir=rt({type:e,hostAttrs:[1,"mat-mdc-button-base"],hostVars:13,hostBindings:function(n,a){n&2&&(re("disabled",a._getDisabledAttribute())("aria-disabled",a._getAriaDisabled())("tabindex",a._getTabIndex()),Xn(a.color?"mat-"+a.color:""),oe("mat-mdc-button-disabled",a.disabled)("mat-mdc-button-disabled-interactive",a.disabledInteractive)("mat-unthemed",!a.color)("_mat-animation-noopable",a._animationsDisabled))},inputs:{color:"color",disableRipple:[2,"disableRipple","disableRipple",H],disabled:[2,"disabled","disabled",H],ariaDisabled:[2,"aria-disabled","ariaDisabled",H],disabledInteractive:[2,"disabledInteractive","disabledInteractive",H],tabIndex:[2,"tabIndex","tabIndex",la],_tabindex:[2,"tabindex","_tabindex",la]}})}return e})(),ni=(()=>{class e extends Sn{constructor(){super(),this._rippleLoader.configureRipple(this._elementRef.nativeElement,{centered:!0})}static \u0275fac=function(n){return new(n||e)};static \u0275cmp=dt({type:e,selectors:[["button","mat-icon-button",""],["a","mat-icon-button",""],["button","matIconButton",""],["a","matIconButton",""]],hostAttrs:[1,"mdc-icon-button","mat-mdc-icon-button"],exportAs:["matButton","matAnchor"],features:[ne],attrs:Xo,ngContentSelectors:ti,decls:4,vars:0,consts:[[1,"mat-mdc-button-persistent-ripple","mdc-icon-button__ripple"],[1,"mat-focus-indicator"],[1,"mat-mdc-button-touch-target"]],template:function(n,a){n&1&&(ae(),mt(0,"span",0),ht(1),mt(2,"span",1)(3,"span",2))},styles:[`.mat-mdc-icon-button {
  -webkit-user-select: none;
  user-select: none;
  display: inline-block;
  position: relative;
  box-sizing: border-box;
  border: none;
  outline: none;
  background-color: transparent;
  fill: currentColor;
  text-decoration: none;
  cursor: pointer;
  z-index: 0;
  overflow: visible;
  border-radius: var(--mat-icon-button-container-shape, var(--mat-sys-corner-full, 50%));
  flex-shrink: 0;
  text-align: center;
  width: var(--mat-icon-button-state-layer-size, 40px);
  height: var(--mat-icon-button-state-layer-size, 40px);
  padding: calc(calc(var(--mat-icon-button-state-layer-size, 40px) - var(--mat-icon-button-icon-size, 24px)) / 2);
  font-size: var(--mat-icon-button-icon-size, 24px);
  color: var(--mat-icon-button-icon-color, var(--mat-sys-on-surface-variant));
  -webkit-tap-highlight-color: transparent;
}
.mat-mdc-icon-button .mat-mdc-button-ripple,
.mat-mdc-icon-button .mat-mdc-button-persistent-ripple,
.mat-mdc-icon-button .mat-mdc-button-persistent-ripple::before {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  pointer-events: none;
  border-radius: inherit;
}
.mat-mdc-icon-button .mat-mdc-button-ripple {
  overflow: hidden;
}
.mat-mdc-icon-button .mat-mdc-button-persistent-ripple::before {
  content: "";
  opacity: 0;
}
.mat-mdc-icon-button .mdc-button__label,
.mat-mdc-icon-button .mat-icon {
  z-index: 1;
  position: relative;
}
.mat-mdc-icon-button .mat-focus-indicator {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  border-radius: inherit;
}
.mat-mdc-icon-button:focus-visible > .mat-focus-indicator::before {
  content: "";
  border-radius: inherit;
}
.mat-mdc-icon-button .mat-ripple-element {
  background-color: var(--mat-icon-button-ripple-color, color-mix(in srgb, var(--mat-sys-on-surface-variant) calc(var(--mat-sys-pressed-state-layer-opacity) * 100%), transparent));
}
.mat-mdc-icon-button .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-icon-button-state-layer-color, var(--mat-sys-on-surface-variant));
}
.mat-mdc-icon-button.mat-mdc-button-disabled .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-icon-button-disabled-state-layer-color, var(--mat-sys-on-surface-variant));
}
.mat-mdc-icon-button:hover > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-icon-button-hover-state-layer-opacity, var(--mat-sys-hover-state-layer-opacity));
}
.mat-mdc-icon-button.cdk-program-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-icon-button.cdk-keyboard-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-icon-button.mat-mdc-button-disabled-interactive:focus > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-icon-button-focus-state-layer-opacity, var(--mat-sys-focus-state-layer-opacity));
}
.mat-mdc-icon-button:active > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-icon-button-pressed-state-layer-opacity, var(--mat-sys-pressed-state-layer-opacity));
}
.mat-mdc-icon-button .mat-mdc-button-touch-target {
  position: absolute;
  top: 50%;
  height: var(--mat-icon-button-touch-target-size, 48px);
  display: var(--mat-icon-button-touch-target-display, block);
  left: 50%;
  width: var(--mat-icon-button-touch-target-size, 48px);
  transform: translate(-50%, -50%);
}
.mat-mdc-icon-button._mat-animation-noopable {
  transition: none !important;
  animation: none !important;
}
.mat-mdc-icon-button[disabled], .mat-mdc-icon-button.mat-mdc-button-disabled {
  cursor: default;
  pointer-events: none;
  color: var(--mat-icon-button-disabled-icon-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
}
.mat-mdc-icon-button.mat-mdc-button-disabled-interactive {
  pointer-events: auto;
}
.mat-mdc-icon-button img,
.mat-mdc-icon-button svg {
  width: var(--mat-icon-button-icon-size, 24px);
  height: var(--mat-icon-button-icon-size, 24px);
  vertical-align: baseline;
}
.mat-mdc-icon-button .mat-mdc-button-persistent-ripple {
  border-radius: var(--mat-icon-button-container-shape, var(--mat-sys-corner-full, 50%));
}
.mat-mdc-icon-button[hidden] {
  display: none;
}
.mat-mdc-icon-button.mat-unthemed:not(.mdc-ripple-upgraded):focus::before, .mat-mdc-icon-button.mat-primary:not(.mdc-ripple-upgraded):focus::before, .mat-mdc-icon-button.mat-accent:not(.mdc-ripple-upgraded):focus::before, .mat-mdc-icon-button.mat-warn:not(.mdc-ripple-upgraded):focus::before {
  background: transparent;
  opacity: 1;
}
`,`@media (forced-colors: active) {
  .mat-mdc-button:not(.mdc-button--outlined),
  .mat-mdc-unelevated-button:not(.mdc-button--outlined),
  .mat-mdc-raised-button:not(.mdc-button--outlined),
  .mat-mdc-outlined-button:not(.mdc-button--outlined),
  .mat-mdc-button-base.mat-tonal-button,
  .mat-mdc-icon-button.mat-mdc-icon-button,
  .mat-mdc-outlined-button .mdc-button__ripple {
    outline: solid 1px;
  }
}
`],encapsulation:2,changeDetection:0})}return e})();var ri=["matButton",""],ai=[[["",8,"material-icons",3,"iconPositionEnd",""],["mat-icon",3,"iconPositionEnd",""],["","matButtonIcon","",3,"iconPositionEnd",""]],"*",[["","iconPositionEnd","",8,"material-icons"],["mat-icon","iconPositionEnd",""],["","matButtonIcon","","iconPositionEnd",""]]],oi=[".material-icons:not([iconPositionEnd]), mat-icon:not([iconPositionEnd]), [matButtonIcon]:not([iconPositionEnd])","*",".material-icons[iconPositionEnd], mat-icon[iconPositionEnd], [matButtonIcon][iconPositionEnd]"];var ua=new Map([["text",["mat-mdc-button"]],["filled",["mdc-button--unelevated","mat-mdc-unelevated-button"]],["elevated",["mdc-button--raised","mat-mdc-raised-button"]],["outlined",["mdc-button--outlined","mat-mdc-outlined-button"]],["tonal",["mat-tonal-button"]]]),Ps=(()=>{class e extends Sn{get appearance(){return this._appearance}set appearance(t){this.setAppearance(t||this._config?.defaultAppearance||"text")}_appearance=null;constructor(){super();let t=ii(this._elementRef.nativeElement);t&&this.setAppearance(t)}setAppearance(t){if(t===this._appearance)return;let n=this._elementRef.nativeElement.classList,a=this._appearance?ua.get(this._appearance):null,o=ua.get(t);a&&n.remove(...a),n.add(...o),this._appearance=t}static \u0275fac=function(n){return new(n||e)};static \u0275cmp=dt({type:e,selectors:[["button","matButton",""],["a","matButton",""],["button","mat-button",""],["button","mat-raised-button",""],["button","mat-flat-button",""],["button","mat-stroked-button",""],["a","mat-button",""],["a","mat-raised-button",""],["a","mat-flat-button",""],["a","mat-stroked-button",""]],hostAttrs:[1,"mdc-button"],inputs:{appearance:[0,"matButton","appearance"]},exportAs:["matButton","matAnchor"],features:[ne],attrs:ri,ngContentSelectors:oi,decls:7,vars:4,consts:[[1,"mat-mdc-button-persistent-ripple"],[1,"mdc-button__label"],[1,"mat-focus-indicator"],[1,"mat-mdc-button-touch-target"]],template:function(n,a){n&1&&(ae(ai),mt(0,"span",0),ht(1),Wn(2,"span",1),ht(3,1),Qn(),ht(4,2),mt(5,"span",2)(6,"span",3)),n&2&&oe("mdc-button__ripple",!a._isFab)("mdc-fab__ripple",a._isFab)},styles:[`.mat-mdc-button-base {
  text-decoration: none;
}
.mat-mdc-button-base .mat-icon {
  min-height: fit-content;
  flex-shrink: 0;
}
@media (hover: none) {
  .mat-mdc-button-base:hover > span.mat-mdc-button-persistent-ripple::before {
    opacity: 0;
  }
}

.mdc-button {
  -webkit-user-select: none;
  user-select: none;
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  box-sizing: border-box;
  min-width: 64px;
  border: none;
  outline: none;
  line-height: inherit;
  -webkit-appearance: none;
  overflow: visible;
  vertical-align: middle;
  background: transparent;
  padding: 0 8px;
}
.mdc-button::-moz-focus-inner {
  padding: 0;
  border: 0;
}
.mdc-button:active {
  outline: none;
}
.mdc-button:hover {
  cursor: pointer;
}
.mdc-button:disabled {
  cursor: default;
  pointer-events: none;
}
.mdc-button[hidden] {
  display: none;
}
.mdc-button .mdc-button__label {
  position: relative;
}

.mat-mdc-button {
  padding: 0 var(--mat-button-text-horizontal-padding, 12px);
  height: var(--mat-button-text-container-height, 40px);
  font-family: var(--mat-button-text-label-text-font, var(--mat-sys-label-large-font));
  font-size: var(--mat-button-text-label-text-size, var(--mat-sys-label-large-size));
  letter-spacing: var(--mat-button-text-label-text-tracking, var(--mat-sys-label-large-tracking));
  text-transform: var(--mat-button-text-label-text-transform);
  font-weight: var(--mat-button-text-label-text-weight, var(--mat-sys-label-large-weight));
}
.mat-mdc-button, .mat-mdc-button .mdc-button__ripple {
  border-radius: var(--mat-button-text-container-shape, var(--mat-sys-corner-full));
}
.mat-mdc-button:not(:disabled) {
  color: var(--mat-button-text-label-text-color, var(--mat-sys-primary));
}
.mat-mdc-button[disabled], .mat-mdc-button.mat-mdc-button-disabled {
  cursor: default;
  pointer-events: none;
  color: var(--mat-button-text-disabled-label-text-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
}
.mat-mdc-button.mat-mdc-button-disabled-interactive {
  pointer-events: auto;
}
.mat-mdc-button:has(.material-icons, mat-icon, [matButtonIcon]) {
  padding: 0 var(--mat-button-text-with-icon-horizontal-padding, 16px);
}
.mat-mdc-button > .mat-icon {
  margin-right: var(--mat-button-text-icon-spacing, 8px);
  margin-left: var(--mat-button-text-icon-offset, -4px);
}
[dir=rtl] .mat-mdc-button > .mat-icon {
  margin-right: var(--mat-button-text-icon-offset, -4px);
  margin-left: var(--mat-button-text-icon-spacing, 8px);
}
.mat-mdc-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-text-icon-offset, -4px);
  margin-left: var(--mat-button-text-icon-spacing, 8px);
}
[dir=rtl] .mat-mdc-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-text-icon-spacing, 8px);
  margin-left: var(--mat-button-text-icon-offset, -4px);
}
.mat-mdc-button .mat-ripple-element {
  background-color: var(--mat-button-text-ripple-color, color-mix(in srgb, var(--mat-sys-primary) calc(var(--mat-sys-pressed-state-layer-opacity) * 100%), transparent));
}
.mat-mdc-button .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-text-state-layer-color, var(--mat-sys-primary));
}
.mat-mdc-button.mat-mdc-button-disabled .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-text-disabled-state-layer-color, var(--mat-sys-on-surface-variant));
}
.mat-mdc-button:hover > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-text-hover-state-layer-opacity, var(--mat-sys-hover-state-layer-opacity));
}
.mat-mdc-button.cdk-program-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-button.cdk-keyboard-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-button.mat-mdc-button-disabled-interactive:focus > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-text-focus-state-layer-opacity, var(--mat-sys-focus-state-layer-opacity));
}
.mat-mdc-button:active > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-text-pressed-state-layer-opacity, var(--mat-sys-pressed-state-layer-opacity));
}
.mat-mdc-button .mat-mdc-button-touch-target {
  position: absolute;
  top: 50%;
  height: var(--mat-button-text-touch-target-size, 48px);
  display: var(--mat-button-text-touch-target-display, block);
  left: 0;
  right: 0;
  transform: translateY(-50%);
}

.mat-mdc-unelevated-button {
  transition: box-shadow 280ms cubic-bezier(0.4, 0, 0.2, 1);
  height: var(--mat-button-filled-container-height, 40px);
  font-family: var(--mat-button-filled-label-text-font, var(--mat-sys-label-large-font));
  font-size: var(--mat-button-filled-label-text-size, var(--mat-sys-label-large-size));
  letter-spacing: var(--mat-button-filled-label-text-tracking, var(--mat-sys-label-large-tracking));
  text-transform: var(--mat-button-filled-label-text-transform);
  font-weight: var(--mat-button-filled-label-text-weight, var(--mat-sys-label-large-weight));
  padding: 0 var(--mat-button-filled-horizontal-padding, 24px);
}
.mat-mdc-unelevated-button > .mat-icon {
  margin-right: var(--mat-button-filled-icon-spacing, 8px);
  margin-left: var(--mat-button-filled-icon-offset, -8px);
}
[dir=rtl] .mat-mdc-unelevated-button > .mat-icon {
  margin-right: var(--mat-button-filled-icon-offset, -8px);
  margin-left: var(--mat-button-filled-icon-spacing, 8px);
}
.mat-mdc-unelevated-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-filled-icon-offset, -8px);
  margin-left: var(--mat-button-filled-icon-spacing, 8px);
}
[dir=rtl] .mat-mdc-unelevated-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-filled-icon-spacing, 8px);
  margin-left: var(--mat-button-filled-icon-offset, -8px);
}
.mat-mdc-unelevated-button .mat-ripple-element {
  background-color: var(--mat-button-filled-ripple-color, color-mix(in srgb, var(--mat-sys-on-primary) calc(var(--mat-sys-pressed-state-layer-opacity) * 100%), transparent));
}
.mat-mdc-unelevated-button .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-filled-state-layer-color, var(--mat-sys-on-primary));
}
.mat-mdc-unelevated-button.mat-mdc-button-disabled .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-filled-disabled-state-layer-color, var(--mat-sys-on-surface-variant));
}
.mat-mdc-unelevated-button:hover > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-filled-hover-state-layer-opacity, var(--mat-sys-hover-state-layer-opacity));
}
.mat-mdc-unelevated-button.cdk-program-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-unelevated-button.cdk-keyboard-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-unelevated-button.mat-mdc-button-disabled-interactive:focus > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-filled-focus-state-layer-opacity, var(--mat-sys-focus-state-layer-opacity));
}
.mat-mdc-unelevated-button:active > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-filled-pressed-state-layer-opacity, var(--mat-sys-pressed-state-layer-opacity));
}
.mat-mdc-unelevated-button .mat-mdc-button-touch-target {
  position: absolute;
  top: 50%;
  height: var(--mat-button-filled-touch-target-size, 48px);
  display: var(--mat-button-filled-touch-target-display, block);
  left: 0;
  right: 0;
  transform: translateY(-50%);
}
.mat-mdc-unelevated-button:not(:disabled) {
  color: var(--mat-button-filled-label-text-color, var(--mat-sys-on-primary));
  background-color: var(--mat-button-filled-container-color, var(--mat-sys-primary));
}
.mat-mdc-unelevated-button, .mat-mdc-unelevated-button .mdc-button__ripple {
  border-radius: var(--mat-button-filled-container-shape, var(--mat-sys-corner-full));
}
.mat-mdc-unelevated-button[disabled], .mat-mdc-unelevated-button.mat-mdc-button-disabled {
  cursor: default;
  pointer-events: none;
  color: var(--mat-button-filled-disabled-label-text-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
  background-color: var(--mat-button-filled-disabled-container-color, color-mix(in srgb, var(--mat-sys-on-surface) 12%, transparent));
}
.mat-mdc-unelevated-button.mat-mdc-button-disabled-interactive {
  pointer-events: auto;
}

.mat-mdc-raised-button {
  transition: box-shadow 280ms cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: var(--mat-button-protected-container-elevation-shadow, var(--mat-sys-level1));
  height: var(--mat-button-protected-container-height, 40px);
  font-family: var(--mat-button-protected-label-text-font, var(--mat-sys-label-large-font));
  font-size: var(--mat-button-protected-label-text-size, var(--mat-sys-label-large-size));
  letter-spacing: var(--mat-button-protected-label-text-tracking, var(--mat-sys-label-large-tracking));
  text-transform: var(--mat-button-protected-label-text-transform);
  font-weight: var(--mat-button-protected-label-text-weight, var(--mat-sys-label-large-weight));
  padding: 0 var(--mat-button-protected-horizontal-padding, 24px);
}
.mat-mdc-raised-button > .mat-icon {
  margin-right: var(--mat-button-protected-icon-spacing, 8px);
  margin-left: var(--mat-button-protected-icon-offset, -8px);
}
[dir=rtl] .mat-mdc-raised-button > .mat-icon {
  margin-right: var(--mat-button-protected-icon-offset, -8px);
  margin-left: var(--mat-button-protected-icon-spacing, 8px);
}
.mat-mdc-raised-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-protected-icon-offset, -8px);
  margin-left: var(--mat-button-protected-icon-spacing, 8px);
}
[dir=rtl] .mat-mdc-raised-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-protected-icon-spacing, 8px);
  margin-left: var(--mat-button-protected-icon-offset, -8px);
}
.mat-mdc-raised-button .mat-ripple-element {
  background-color: var(--mat-button-protected-ripple-color, color-mix(in srgb, var(--mat-sys-primary) calc(var(--mat-sys-pressed-state-layer-opacity) * 100%), transparent));
}
.mat-mdc-raised-button .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-protected-state-layer-color, var(--mat-sys-primary));
}
.mat-mdc-raised-button.mat-mdc-button-disabled .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-protected-disabled-state-layer-color, var(--mat-sys-on-surface-variant));
}
.mat-mdc-raised-button:hover > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-protected-hover-state-layer-opacity, var(--mat-sys-hover-state-layer-opacity));
}
.mat-mdc-raised-button.cdk-program-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-raised-button.cdk-keyboard-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-raised-button.mat-mdc-button-disabled-interactive:focus > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-protected-focus-state-layer-opacity, var(--mat-sys-focus-state-layer-opacity));
}
.mat-mdc-raised-button:active > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-protected-pressed-state-layer-opacity, var(--mat-sys-pressed-state-layer-opacity));
}
.mat-mdc-raised-button .mat-mdc-button-touch-target {
  position: absolute;
  top: 50%;
  height: var(--mat-button-protected-touch-target-size, 48px);
  display: var(--mat-button-protected-touch-target-display, block);
  left: 0;
  right: 0;
  transform: translateY(-50%);
}
.mat-mdc-raised-button:not(:disabled) {
  color: var(--mat-button-protected-label-text-color, var(--mat-sys-primary));
  background-color: var(--mat-button-protected-container-color, var(--mat-sys-surface));
}
.mat-mdc-raised-button, .mat-mdc-raised-button .mdc-button__ripple {
  border-radius: var(--mat-button-protected-container-shape, var(--mat-sys-corner-full));
}
@media (hover: hover) {
  .mat-mdc-raised-button:hover {
    box-shadow: var(--mat-button-protected-hover-container-elevation-shadow, var(--mat-sys-level2));
  }
}
.mat-mdc-raised-button:focus {
  box-shadow: var(--mat-button-protected-focus-container-elevation-shadow, var(--mat-sys-level1));
}
.mat-mdc-raised-button:active, .mat-mdc-raised-button:focus:active {
  box-shadow: var(--mat-button-protected-pressed-container-elevation-shadow, var(--mat-sys-level1));
}
.mat-mdc-raised-button[disabled], .mat-mdc-raised-button.mat-mdc-button-disabled {
  cursor: default;
  pointer-events: none;
  color: var(--mat-button-protected-disabled-label-text-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
  background-color: var(--mat-button-protected-disabled-container-color, color-mix(in srgb, var(--mat-sys-on-surface) 12%, transparent));
}
.mat-mdc-raised-button[disabled].mat-mdc-button-disabled, .mat-mdc-raised-button.mat-mdc-button-disabled.mat-mdc-button-disabled {
  box-shadow: var(--mat-button-protected-disabled-container-elevation-shadow, var(--mat-sys-level0));
}
.mat-mdc-raised-button.mat-mdc-button-disabled-interactive {
  pointer-events: auto;
}

.mat-mdc-outlined-button {
  border-style: solid;
  transition: border 280ms cubic-bezier(0.4, 0, 0.2, 1);
  height: var(--mat-button-outlined-container-height, 40px);
  font-family: var(--mat-button-outlined-label-text-font, var(--mat-sys-label-large-font));
  font-size: var(--mat-button-outlined-label-text-size, var(--mat-sys-label-large-size));
  letter-spacing: var(--mat-button-outlined-label-text-tracking, var(--mat-sys-label-large-tracking));
  text-transform: var(--mat-button-outlined-label-text-transform);
  font-weight: var(--mat-button-outlined-label-text-weight, var(--mat-sys-label-large-weight));
  border-radius: var(--mat-button-outlined-container-shape, var(--mat-sys-corner-full));
  border-width: var(--mat-button-outlined-outline-width, 1px);
  padding: 0 var(--mat-button-outlined-horizontal-padding, 24px);
}
.mat-mdc-outlined-button > .mat-icon {
  margin-right: var(--mat-button-outlined-icon-spacing, 8px);
  margin-left: var(--mat-button-outlined-icon-offset, -8px);
}
[dir=rtl] .mat-mdc-outlined-button > .mat-icon {
  margin-right: var(--mat-button-outlined-icon-offset, -8px);
  margin-left: var(--mat-button-outlined-icon-spacing, 8px);
}
.mat-mdc-outlined-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-outlined-icon-offset, -8px);
  margin-left: var(--mat-button-outlined-icon-spacing, 8px);
}
[dir=rtl] .mat-mdc-outlined-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-outlined-icon-spacing, 8px);
  margin-left: var(--mat-button-outlined-icon-offset, -8px);
}
.mat-mdc-outlined-button .mat-ripple-element {
  background-color: var(--mat-button-outlined-ripple-color, color-mix(in srgb, var(--mat-sys-primary) calc(var(--mat-sys-pressed-state-layer-opacity) * 100%), transparent));
}
.mat-mdc-outlined-button .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-outlined-state-layer-color, var(--mat-sys-primary));
}
.mat-mdc-outlined-button.mat-mdc-button-disabled .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-outlined-disabled-state-layer-color, var(--mat-sys-on-surface-variant));
}
.mat-mdc-outlined-button:hover > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-outlined-hover-state-layer-opacity, var(--mat-sys-hover-state-layer-opacity));
}
.mat-mdc-outlined-button.cdk-program-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-outlined-button.cdk-keyboard-focused > .mat-mdc-button-persistent-ripple::before, .mat-mdc-outlined-button.mat-mdc-button-disabled-interactive:focus > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-outlined-focus-state-layer-opacity, var(--mat-sys-focus-state-layer-opacity));
}
.mat-mdc-outlined-button:active > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-outlined-pressed-state-layer-opacity, var(--mat-sys-pressed-state-layer-opacity));
}
.mat-mdc-outlined-button .mat-mdc-button-touch-target {
  position: absolute;
  top: 50%;
  height: var(--mat-button-outlined-touch-target-size, 48px);
  display: var(--mat-button-outlined-touch-target-display, block);
  left: 0;
  right: 0;
  transform: translateY(-50%);
}
.mat-mdc-outlined-button:not(:disabled) {
  color: var(--mat-button-outlined-label-text-color, var(--mat-sys-primary));
  border-color: var(--mat-button-outlined-outline-color, var(--mat-sys-outline));
}
.mat-mdc-outlined-button[disabled], .mat-mdc-outlined-button.mat-mdc-button-disabled {
  cursor: default;
  pointer-events: none;
  color: var(--mat-button-outlined-disabled-label-text-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
  border-color: var(--mat-button-outlined-disabled-outline-color, color-mix(in srgb, var(--mat-sys-on-surface) 12%, transparent));
}
.mat-mdc-outlined-button.mat-mdc-button-disabled-interactive {
  pointer-events: auto;
}

.mat-tonal-button {
  transition: box-shadow 280ms cubic-bezier(0.4, 0, 0.2, 1);
  height: var(--mat-button-tonal-container-height, 40px);
  font-family: var(--mat-button-tonal-label-text-font, var(--mat-sys-label-large-font));
  font-size: var(--mat-button-tonal-label-text-size, var(--mat-sys-label-large-size));
  letter-spacing: var(--mat-button-tonal-label-text-tracking, var(--mat-sys-label-large-tracking));
  text-transform: var(--mat-button-tonal-label-text-transform);
  font-weight: var(--mat-button-tonal-label-text-weight, var(--mat-sys-label-large-weight));
  padding: 0 var(--mat-button-tonal-horizontal-padding, 24px);
}
.mat-tonal-button:not(:disabled) {
  color: var(--mat-button-tonal-label-text-color, var(--mat-sys-on-secondary-container));
  background-color: var(--mat-button-tonal-container-color, var(--mat-sys-secondary-container));
}
.mat-tonal-button, .mat-tonal-button .mdc-button__ripple {
  border-radius: var(--mat-button-tonal-container-shape, var(--mat-sys-corner-full));
}
.mat-tonal-button[disabled], .mat-tonal-button.mat-mdc-button-disabled {
  cursor: default;
  pointer-events: none;
  color: var(--mat-button-tonal-disabled-label-text-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
  background-color: var(--mat-button-tonal-disabled-container-color, color-mix(in srgb, var(--mat-sys-on-surface) 12%, transparent));
}
.mat-tonal-button.mat-mdc-button-disabled-interactive {
  pointer-events: auto;
}
.mat-tonal-button > .mat-icon {
  margin-right: var(--mat-button-tonal-icon-spacing, 8px);
  margin-left: var(--mat-button-tonal-icon-offset, -8px);
}
[dir=rtl] .mat-tonal-button > .mat-icon {
  margin-right: var(--mat-button-tonal-icon-offset, -8px);
  margin-left: var(--mat-button-tonal-icon-spacing, 8px);
}
.mat-tonal-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-tonal-icon-offset, -8px);
  margin-left: var(--mat-button-tonal-icon-spacing, 8px);
}
[dir=rtl] .mat-tonal-button .mdc-button__label + .mat-icon {
  margin-right: var(--mat-button-tonal-icon-spacing, 8px);
  margin-left: var(--mat-button-tonal-icon-offset, -8px);
}
.mat-tonal-button .mat-ripple-element {
  background-color: var(--mat-button-tonal-ripple-color, color-mix(in srgb, var(--mat-sys-on-secondary-container) calc(var(--mat-sys-pressed-state-layer-opacity) * 100%), transparent));
}
.mat-tonal-button .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-tonal-state-layer-color, var(--mat-sys-on-secondary-container));
}
.mat-tonal-button.mat-mdc-button-disabled .mat-mdc-button-persistent-ripple::before {
  background-color: var(--mat-button-tonal-disabled-state-layer-color, var(--mat-sys-on-surface-variant));
}
.mat-tonal-button:hover > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-tonal-hover-state-layer-opacity, var(--mat-sys-hover-state-layer-opacity));
}
.mat-tonal-button.cdk-program-focused > .mat-mdc-button-persistent-ripple::before, .mat-tonal-button.cdk-keyboard-focused > .mat-mdc-button-persistent-ripple::before, .mat-tonal-button.mat-mdc-button-disabled-interactive:focus > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-tonal-focus-state-layer-opacity, var(--mat-sys-focus-state-layer-opacity));
}
.mat-tonal-button:active > .mat-mdc-button-persistent-ripple::before {
  opacity: var(--mat-button-tonal-pressed-state-layer-opacity, var(--mat-sys-pressed-state-layer-opacity));
}
.mat-tonal-button .mat-mdc-button-touch-target {
  position: absolute;
  top: 50%;
  height: var(--mat-button-tonal-touch-target-size, 48px);
  display: var(--mat-button-tonal-touch-target-display, block);
  left: 0;
  right: 0;
  transform: translateY(-50%);
}

.mat-mdc-button,
.mat-mdc-unelevated-button,
.mat-mdc-raised-button,
.mat-mdc-outlined-button,
.mat-tonal-button {
  -webkit-tap-highlight-color: transparent;
}
.mat-mdc-button .mat-mdc-button-ripple,
.mat-mdc-button .mat-mdc-button-persistent-ripple,
.mat-mdc-button .mat-mdc-button-persistent-ripple::before,
.mat-mdc-unelevated-button .mat-mdc-button-ripple,
.mat-mdc-unelevated-button .mat-mdc-button-persistent-ripple,
.mat-mdc-unelevated-button .mat-mdc-button-persistent-ripple::before,
.mat-mdc-raised-button .mat-mdc-button-ripple,
.mat-mdc-raised-button .mat-mdc-button-persistent-ripple,
.mat-mdc-raised-button .mat-mdc-button-persistent-ripple::before,
.mat-mdc-outlined-button .mat-mdc-button-ripple,
.mat-mdc-outlined-button .mat-mdc-button-persistent-ripple,
.mat-mdc-outlined-button .mat-mdc-button-persistent-ripple::before,
.mat-tonal-button .mat-mdc-button-ripple,
.mat-tonal-button .mat-mdc-button-persistent-ripple,
.mat-tonal-button .mat-mdc-button-persistent-ripple::before {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  pointer-events: none;
  border-radius: inherit;
}
.mat-mdc-button .mat-mdc-button-ripple,
.mat-mdc-unelevated-button .mat-mdc-button-ripple,
.mat-mdc-raised-button .mat-mdc-button-ripple,
.mat-mdc-outlined-button .mat-mdc-button-ripple,
.mat-tonal-button .mat-mdc-button-ripple {
  overflow: hidden;
}
.mat-mdc-button .mat-mdc-button-persistent-ripple::before,
.mat-mdc-unelevated-button .mat-mdc-button-persistent-ripple::before,
.mat-mdc-raised-button .mat-mdc-button-persistent-ripple::before,
.mat-mdc-outlined-button .mat-mdc-button-persistent-ripple::before,
.mat-tonal-button .mat-mdc-button-persistent-ripple::before {
  content: "";
  opacity: 0;
}
.mat-mdc-button .mdc-button__label,
.mat-mdc-button .mat-icon,
.mat-mdc-unelevated-button .mdc-button__label,
.mat-mdc-unelevated-button .mat-icon,
.mat-mdc-raised-button .mdc-button__label,
.mat-mdc-raised-button .mat-icon,
.mat-mdc-outlined-button .mdc-button__label,
.mat-mdc-outlined-button .mat-icon,
.mat-tonal-button .mdc-button__label,
.mat-tonal-button .mat-icon {
  z-index: 1;
  position: relative;
}
.mat-mdc-button .mat-focus-indicator,
.mat-mdc-unelevated-button .mat-focus-indicator,
.mat-mdc-raised-button .mat-focus-indicator,
.mat-mdc-outlined-button .mat-focus-indicator,
.mat-tonal-button .mat-focus-indicator {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  border-radius: inherit;
}
.mat-mdc-button:focus-visible > .mat-focus-indicator::before,
.mat-mdc-unelevated-button:focus-visible > .mat-focus-indicator::before,
.mat-mdc-raised-button:focus-visible > .mat-focus-indicator::before,
.mat-mdc-outlined-button:focus-visible > .mat-focus-indicator::before,
.mat-tonal-button:focus-visible > .mat-focus-indicator::before {
  content: "";
  border-radius: inherit;
}
.mat-mdc-button._mat-animation-noopable,
.mat-mdc-unelevated-button._mat-animation-noopable,
.mat-mdc-raised-button._mat-animation-noopable,
.mat-mdc-outlined-button._mat-animation-noopable,
.mat-tonal-button._mat-animation-noopable {
  transition: none !important;
  animation: none !important;
}
.mat-mdc-button > .mat-icon,
.mat-mdc-unelevated-button > .mat-icon,
.mat-mdc-raised-button > .mat-icon,
.mat-mdc-outlined-button > .mat-icon,
.mat-tonal-button > .mat-icon {
  display: inline-block;
  position: relative;
  vertical-align: top;
  font-size: 1.125rem;
  height: 1.125rem;
  width: 1.125rem;
}

.mat-mdc-outlined-button .mat-mdc-button-ripple,
.mat-mdc-outlined-button .mdc-button__ripple {
  top: -1px;
  left: -1px;
  bottom: -1px;
  right: -1px;
}

.mat-mdc-unelevated-button .mat-focus-indicator::before,
.mat-tonal-button .mat-focus-indicator::before,
.mat-mdc-raised-button .mat-focus-indicator::before {
  margin: calc(calc(var(--mat-focus-indicator-border-width, 3px) + 2px) * -1);
}

.mat-mdc-outlined-button .mat-focus-indicator::before {
  margin: calc(calc(var(--mat-focus-indicator-border-width, 3px) + 3px) * -1);
}
`,`@media (forced-colors: active) {
  .mat-mdc-button:not(.mdc-button--outlined),
  .mat-mdc-unelevated-button:not(.mdc-button--outlined),
  .mat-mdc-raised-button:not(.mdc-button--outlined),
  .mat-mdc-outlined-button:not(.mdc-button--outlined),
  .mat-mdc-button-base.mat-tonal-button,
  .mat-mdc-icon-button.mat-mdc-icon-button,
  .mat-mdc-outlined-button .mdc-button__ripple {
    outline: solid 1px;
  }
}
`],encapsulation:2,changeDetection:0})}return e})();function ii(e){return e.hasAttribute("mat-raised-button")?"elevated":e.hasAttribute("mat-stroked-button")?"outlined":e.hasAttribute("mat-flat-button")?"filled":e.hasAttribute("mat-button")?"text":null}var Ls=(()=>{class e{static \u0275fac=function(n){return new(n||e)};static \u0275mod=Ge({type:e});static \u0275inj=Fe({imports:[pr,hr]})}return e})();export{G as a,vn as b,lt as c,Ue as d,Bo as e,Vo as f,Jo as g,ni as h,Ps as i,Ls as j};
