import{a as he,b as ve}from"./chunk-ZBKGOEJE.js";import{a as H}from"./chunk-MOYO6C6C.js";import{b as _e,c as fe}from"./chunk-F2VEUWUN.js";import{a as ue}from"./chunk-3ODL2LXL.js";import{b as pe,f as ge}from"./chunk-RR5LTW6N.js";import{a as J,b as X,c as Y,d as ee,f as te,h as re}from"./chunk-EZCVIYWJ.js";import{b as ne,g as ae,h as oe,i as se,j as ce,k as me,s as le,u as de}from"./chunk-PTFPPLIF.js";import"./chunk-TBZDRLNV.js";import{a as U,b as Z,c as ie}from"./chunk-FOIS2F75.js";import{c as G,i as $,j as K}from"./chunk-CSGIYDIV.js";import{_ as Q,d as V,f as j,ja as q}from"./chunk-UTAGKRPA.js";import{$ as I,$b as W,Ab as k,Bb as E,Bc as z,Ca as B,Gb as d,Hb as n,Ib as o,Jb as g,Pb as M,Qc as C,Vb as h,Xa as a,Xb as y,ac as N,ba as T,bc as L,da as _,fc as R,gc as S,hc as A,ic as F,jc as s,ka as x,kb as b,kc as u,la as P,lb as O,lc as w,qb as D,va as v,yc as m,zb as f,zc as l}from"./chunk-WJUDORRU.js";var Ce=["determinateSpinner"];function xe(i,p){if(i&1&&(x(),n(0,"svg",11),g(1,"circle",12),o()),i&2){let e=y();f("viewBox",e._viewBox()),a(),S("stroke-dasharray",e._strokeCircumference(),"px")("stroke-dashoffset",e._strokeCircumference()/2,"px")("stroke-width",e._circleStrokeWidth(),"%"),f("r",e._circleRadius())}}var ke=new T("mat-progress-spinner-default-options",{providedIn:"root",factory:()=>({diameter:be})}),be=100,Ee=10,Me=(()=>{class i{_elementRef=_(B);_noopAnimations;get color(){return this._color||this._defaultColor}set color(e){this._color=e}_color;_defaultColor="primary";_determinateCircle;constructor(){let e=_(ke),r=Q(),t=this._elementRef.nativeElement;this._noopAnimations=r==="di-disabled"&&!!e&&!e._forceAnimations,this.mode=t.nodeName.toLowerCase()==="mat-spinner"?"indeterminate":"determinate",!this._noopAnimations&&r==="reduced-motion"&&t.classList.add("mat-progress-spinner-reduced-motion"),e&&(e.color&&(this.color=this._defaultColor=e.color),e.diameter&&(this.diameter=e.diameter),e.strokeWidth&&(this.strokeWidth=e.strokeWidth))}mode;get value(){return this.mode==="determinate"?this._value:0}set value(e){this._value=Math.max(0,Math.min(100,e||0))}_value=0;get diameter(){return this._diameter}set diameter(e){this._diameter=e||0}_diameter=be;get strokeWidth(){return this._strokeWidth??this.diameter/10}set strokeWidth(e){this._strokeWidth=e||0}_strokeWidth;_circleRadius(){return(this.diameter-Ee)/2}_viewBox(){let e=this._circleRadius()*2+this.strokeWidth;return`0 0 ${e} ${e}`}_strokeCircumference(){return 2*Math.PI*this._circleRadius()}_strokeDashOffset(){return this.mode==="determinate"?this._strokeCircumference()*(100-this._value)/100:null}_circleStrokeWidth(){return this.strokeWidth/this.diameter*100}static \u0275fac=function(r){return new(r||i)};static \u0275cmp=b({type:i,selectors:[["mat-progress-spinner"],["mat-spinner"]],viewQuery:function(r,t){if(r&1&&W(Ce,5),r&2){let c;N(c=L())&&(t._determinateCircle=c.first)}},hostAttrs:["role","progressbar","tabindex","-1",1,"mat-mdc-progress-spinner","mdc-circular-progress"],hostVars:18,hostBindings:function(r,t){r&2&&(f("aria-valuemin",0)("aria-valuemax",100)("aria-valuenow",t.mode==="determinate"?t.value:null)("mode",t.mode),F("mat-"+t.color),S("width",t.diameter,"px")("height",t.diameter,"px")("--mat-progress-spinner-size",t.diameter+"px")("--mat-progress-spinner-active-indicator-width",t.diameter+"px"),A("_mat-animation-noopable",t._noopAnimations)("mdc-circular-progress--indeterminate",t.mode==="indeterminate"))},inputs:{color:"color",mode:"mode",value:[2,"value","value",C],diameter:[2,"diameter","diameter",C],strokeWidth:[2,"strokeWidth","strokeWidth",C]},exportAs:["matProgressSpinner"],decls:14,vars:11,consts:[["circle",""],["determinateSpinner",""],["aria-hidden","true",1,"mdc-circular-progress__determinate-container"],["xmlns","http://www.w3.org/2000/svg","focusable","false",1,"mdc-circular-progress__determinate-circle-graphic"],["cx","50%","cy","50%",1,"mdc-circular-progress__determinate-circle"],["aria-hidden","true",1,"mdc-circular-progress__indeterminate-container"],[1,"mdc-circular-progress__spinner-layer"],[1,"mdc-circular-progress__circle-clipper","mdc-circular-progress__circle-left"],[3,"ngTemplateOutlet"],[1,"mdc-circular-progress__gap-patch"],[1,"mdc-circular-progress__circle-clipper","mdc-circular-progress__circle-right"],["xmlns","http://www.w3.org/2000/svg","focusable","false",1,"mdc-circular-progress__indeterminate-circle-graphic"],["cx","50%","cy","50%"]],template:function(r,t){if(r&1&&(D(0,xe,2,8,"ng-template",null,0,z),n(2,"div",2,1),x(),n(4,"svg",3),g(5,"circle",4),o()(),P(),n(6,"div",5)(7,"div",6)(8,"div",7),M(9,8),o(),n(10,"div",9),M(11,8),o(),n(12,"div",10),M(13,8),o()()()),r&2){let c=R(1);a(4),f("viewBox",t._viewBox()),a(),S("stroke-dasharray",t._strokeCircumference(),"px")("stroke-dashoffset",t._strokeDashOffset(),"px")("stroke-width",t._circleStrokeWidth(),"%"),f("r",t._circleRadius()),a(4),d("ngTemplateOutlet",c),a(2),d("ngTemplateOutlet",c),a(2),d("ngTemplateOutlet",c)}},dependencies:[V],styles:[`.mat-mdc-progress-spinner {
  --mat-progress-spinner-animation-multiplier: 1;
  display: block;
  overflow: hidden;
  line-height: 0;
  position: relative;
  direction: ltr;
  transition: opacity 250ms cubic-bezier(0.4, 0, 0.6, 1);
}
.mat-mdc-progress-spinner circle {
  stroke-width: var(--mat-progress-spinner-active-indicator-width, 4px);
}
.mat-mdc-progress-spinner._mat-animation-noopable, .mat-mdc-progress-spinner._mat-animation-noopable .mdc-circular-progress__determinate-circle {
  transition: none !important;
}
.mat-mdc-progress-spinner._mat-animation-noopable .mdc-circular-progress__indeterminate-circle-graphic,
.mat-mdc-progress-spinner._mat-animation-noopable .mdc-circular-progress__spinner-layer,
.mat-mdc-progress-spinner._mat-animation-noopable .mdc-circular-progress__indeterminate-container {
  animation: none !important;
}
.mat-mdc-progress-spinner._mat-animation-noopable .mdc-circular-progress__indeterminate-container circle {
  stroke-dasharray: 0 !important;
}
@media (forced-colors: active) {
  .mat-mdc-progress-spinner .mdc-circular-progress__indeterminate-circle-graphic,
  .mat-mdc-progress-spinner .mdc-circular-progress__determinate-circle {
    stroke: currentColor;
    stroke: CanvasText;
  }
}

.mat-progress-spinner-reduced-motion {
  --mat-progress-spinner-animation-multiplier: 1.25;
}

.mdc-circular-progress__determinate-container,
.mdc-circular-progress__indeterminate-circle-graphic,
.mdc-circular-progress__indeterminate-container,
.mdc-circular-progress__spinner-layer {
  position: absolute;
  width: 100%;
  height: 100%;
}

.mdc-circular-progress__determinate-container {
  transform: rotate(-90deg);
}
.mdc-circular-progress--indeterminate .mdc-circular-progress__determinate-container {
  opacity: 0;
}

.mdc-circular-progress__indeterminate-container {
  font-size: 0;
  letter-spacing: 0;
  white-space: nowrap;
  opacity: 0;
}
.mdc-circular-progress--indeterminate .mdc-circular-progress__indeterminate-container {
  opacity: 1;
  animation: mdc-circular-progress-container-rotate calc(1568.2352941176ms * var(--mat-progress-spinner-animation-multiplier)) linear infinite;
}

.mdc-circular-progress__determinate-circle-graphic,
.mdc-circular-progress__indeterminate-circle-graphic {
  fill: transparent;
}

.mat-mdc-progress-spinner .mdc-circular-progress__determinate-circle,
.mat-mdc-progress-spinner .mdc-circular-progress__indeterminate-circle-graphic {
  stroke: var(--mat-progress-spinner-active-indicator-color, var(--mat-sys-primary));
}
@media (forced-colors: active) {
  .mat-mdc-progress-spinner .mdc-circular-progress__determinate-circle,
  .mat-mdc-progress-spinner .mdc-circular-progress__indeterminate-circle-graphic {
    stroke: CanvasText;
  }
}

.mdc-circular-progress__determinate-circle {
  transition: stroke-dashoffset 500ms cubic-bezier(0, 0, 0.2, 1);
}

.mdc-circular-progress__gap-patch {
  position: absolute;
  top: 0;
  left: 47.5%;
  box-sizing: border-box;
  width: 5%;
  height: 100%;
  overflow: hidden;
}

.mdc-circular-progress__gap-patch .mdc-circular-progress__indeterminate-circle-graphic {
  left: -900%;
  width: 2000%;
  transform: rotate(180deg);
}
.mdc-circular-progress__circle-clipper .mdc-circular-progress__indeterminate-circle-graphic {
  width: 200%;
}
.mdc-circular-progress__circle-right .mdc-circular-progress__indeterminate-circle-graphic {
  left: -100%;
}
.mdc-circular-progress--indeterminate .mdc-circular-progress__circle-left .mdc-circular-progress__indeterminate-circle-graphic {
  animation: mdc-circular-progress-left-spin calc(1333ms * var(--mat-progress-spinner-animation-multiplier)) cubic-bezier(0.4, 0, 0.2, 1) infinite both;
}
.mdc-circular-progress--indeterminate .mdc-circular-progress__circle-right .mdc-circular-progress__indeterminate-circle-graphic {
  animation: mdc-circular-progress-right-spin calc(1333ms * var(--mat-progress-spinner-animation-multiplier)) cubic-bezier(0.4, 0, 0.2, 1) infinite both;
}

.mdc-circular-progress__circle-clipper {
  display: inline-flex;
  position: relative;
  width: 50%;
  height: 100%;
  overflow: hidden;
}

.mdc-circular-progress--indeterminate .mdc-circular-progress__spinner-layer {
  animation: mdc-circular-progress-spinner-layer-rotate calc(5332ms * var(--mat-progress-spinner-animation-multiplier)) cubic-bezier(0.4, 0, 0.2, 1) infinite both;
}

@keyframes mdc-circular-progress-container-rotate {
  to {
    transform: rotate(360deg);
  }
}
@keyframes mdc-circular-progress-spinner-layer-rotate {
  12.5% {
    transform: rotate(135deg);
  }
  25% {
    transform: rotate(270deg);
  }
  37.5% {
    transform: rotate(405deg);
  }
  50% {
    transform: rotate(540deg);
  }
  62.5% {
    transform: rotate(675deg);
  }
  75% {
    transform: rotate(810deg);
  }
  87.5% {
    transform: rotate(945deg);
  }
  100% {
    transform: rotate(1080deg);
  }
}
@keyframes mdc-circular-progress-left-spin {
  from {
    transform: rotate(265deg);
  }
  50% {
    transform: rotate(130deg);
  }
  to {
    transform: rotate(265deg);
  }
}
@keyframes mdc-circular-progress-right-spin {
  from {
    transform: rotate(-265deg);
  }
  50% {
    transform: rotate(-130deg);
  }
  to {
    transform: rotate(-265deg);
  }
}
`],encapsulation:2,changeDetection:0})}return i})();var ye=(()=>{class i{static \u0275fac=function(r){return new(r||i)};static \u0275mod=O({type:i});static \u0275inj=I({imports:[q]})}return i})();function Te(i,p){if(i&1&&(n(0,"p",7),s(1),m(2,"t"),o()),i&2){let e=y();a(),u(l(2,1,e.error()))}}function Pe(i,p){i&1&&g(0,"mat-spinner",9)}function Be(i,p){i&1&&(s(0),m(1,"t")),i&2&&w(" ",l(1,1,"auth.signIn")," ")}var Se=class i{auth=_(H);router=_(G);email=v("admin@tnttbd.local");password=v("");loading=v(!1);error=v(null);submit(){this.loading.set(!0),this.error.set(null),this.auth.login(this.email(),this.password()).subscribe({next:()=>{this.loading.set(!1),this.router.navigate(["/app/dashboard"])},error:()=>{this.loading.set(!1),this.error.set("auth.invalidCredentials")}})}loginSSO(p){this.loading.set(!0),this.auth.loginWithSSO(p).subscribe({next:()=>{this.loading.set(!1),this.router.navigate(["/app/dashboard"])},error:()=>{this.loading.set(!1),this.error.set("auth.invalidCredentials")}})}static \u0275fac=function(e){return new(e||i)};static \u0275cmp=b({type:i,selectors:[["app-login"]],decls:42,vars:26,consts:[["form","ngForm"],[1,"login-page"],[1,"login-card"],[3,"ngSubmit"],["appearance","outline",1,"full-width"],["matInput","","type","email","name","email","required","",3,"ngModelChange","ngModel"],["matInput","","type","password","name","password",3,"ngModelChange","ngModel"],[1,"error"],["mat-flat-button","","color","primary","type","submit",1,"full-width",3,"disabled"],["diameter","20"],[1,"divider-or"],["mat-stroked-button","",1,"full-width","sso",3,"click"]],template:function(e,r){e&1&&(n(0,"div",1)(1,"mat-card",2)(2,"mat-card-header")(3,"mat-card-title"),s(4),m(5,"t"),o(),n(6,"mat-card-subtitle"),s(7),m(8,"t"),o()(),n(9,"mat-card-content")(10,"form",3,0),h("ngSubmit",function(){return r.submit()}),n(12,"mat-form-field",4)(13,"mat-label"),s(14),m(15,"t"),o(),n(16,"input",5),h("ngModelChange",function(c){return r.email.set(c)}),o()(),n(17,"mat-form-field",4)(18,"mat-label"),s(19),m(20,"t"),o(),n(21,"input",6),h("ngModelChange",function(c){return r.password.set(c)}),o()(),k(22,Te,3,3,"p",7),n(23,"button",8),k(24,Pe,1,0,"mat-spinner",9)(25,Be,2,3),o()(),n(26,"div",10),g(27,"mat-divider"),n(28,"span"),s(29),m(30,"t"),o(),g(31,"mat-divider"),o(),n(32,"button",11),h("click",function(){return r.loginSSO("google")}),n(33,"mat-icon"),s(34,"login"),o(),s(35),m(36,"t"),o(),n(37,"button",11),h("click",function(){return r.loginSSO("microsoft")}),n(38,"mat-icon"),s(39,"login"),o(),s(40),m(41,"t"),o()()()()),e&2&&(a(4),u(l(5,12,"auth.signIn")),a(3),u(l(8,14,"app.title")),a(7),u(l(15,16,"auth.email")),a(2),d("ngModel",r.email()),a(3),u(l(20,18,"auth.password")),a(2),d("ngModel",r.password()),a(),E(r.error()?22:-1),a(),d("disabled",r.loading()),a(),E(r.loading()?24:25),a(5),u(l(30,20,"auth.or")),a(6),w(" ",l(36,22,"auth.signInWithGoogle")," "),a(5),w(" ",l(41,24,"auth.signInWithMicrosoft")," "))},dependencies:[j,de,me,ne,ae,oe,le,ce,se,re,J,Y,te,ee,X,ue,ge,pe,fe,_e,K,$,Z,U,ve,he,ye,Me,ie],styles:[".login-page[_ngcontent-%COMP%]{display:flex;align-items:center;justify-content:center;min-height:100vh;padding:16px;background:var(--mat-sys-surface-container)}.login-card[_ngcontent-%COMP%]{width:100%;max-width:420px}.full-width[_ngcontent-%COMP%]{width:100%}.error[_ngcontent-%COMP%]{color:var(--mat-sys-error);margin:0 0 12px}.divider-or[_ngcontent-%COMP%]{display:grid;grid-template-columns:1fr auto 1fr;align-items:center;gap:8px;margin:16px 0;color:var(--mat-sys-on-surface-variant);font-size:12px}.sso[_ngcontent-%COMP%]{margin-top:8px}"]})};export{Se as Login};
