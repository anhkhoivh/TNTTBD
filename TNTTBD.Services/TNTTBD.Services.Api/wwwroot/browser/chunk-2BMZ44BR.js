import{a as Lt,b as zt}from"./chunk-LOCDUIU6.js";import{c as Nt}from"./chunk-54AP4VZ4.js";import{a as Ut}from"./chunk-EDWJVDPD.js";import"./chunk-L4BKUYED.js";import"./chunk-R3HFD3F4.js";import{a as Vt}from"./chunk-5AL2SDHA.js";import{b as gt}from"./chunk-WOPUPVC7.js";import{a as ae}from"./chunk-J5RP3A2G.js";import{a as Qt,b as Yt,c as Wt,d as qt,e as Jt,f as Kt,g as Zt,h as Xt,i as te,j as ee,k as ne}from"./chunk-3LSZMPIF.js";import{a as Ft}from"./chunk-3ODL2LXL.js";import{b as Gt,f as Pt}from"./chunk-RR5LTW6N.js";import"./chunk-4MZPLR3U.js";import{a as jt}from"./chunk-OXC2EYSK.js";import{a as Ht}from"./chunk-267P6CPO.js";import{a as Tt,b as Mt,c as wt,f as Bt,h as At}from"./chunk-EZCVIYWJ.js";import{a as $t}from"./chunk-HOV3TQSL.js";import{a as Ot}from"./chunk-QHBGS32Z.js";import"./chunk-JMZ5XG7L.js";import{a as Dt,u as Rt}from"./chunk-PTFPPLIF.js";import"./chunk-TBZDRLNV.js";import{a as Ct,b as St,c as Et}from"./chunk-FOIS2F75.js";import{a as ut,i as kt,j as It}from"./chunk-CSGIYDIV.js";import{$ as ht,A as pt,N as bt,Q as j,f as ct,fa as ft,ha as _t,ia as vt,ja as yt,ka as xt,y as mt}from"./chunk-UTAGKRPA.js";import{$ as K,$b as ot,Ab as T,Ac as st,Bb as M,Ca as Z,Eb as w,Fb as B,Gb as g,Hb as s,Hc as dt,Ib as r,Jb as x,Mc as $,Nb as A,Ob as E,Pc as h,Qb as V,Vb as _,Xa as l,Xb as y,Yb as et,Z as J,Zb as nt,_b as at,a as Y,ac as N,b as W,ba as P,bc as L,da as c,fc as it,hc as z,ia as F,ja as O,jc as u,kb as C,kc as m,lb as X,lc as H,mb as tt,mc as lt,qa as I,qb as k,tc as rt,uc as U,va as v,yc as p,z as q,zb as S,zc as b}from"./chunk-WJUDORRU.js";var _e=["button"],ve=["*"];function ye(a,o){if(a&1&&(s(0,"div",2),x(1,"mat-pseudo-checkbox",6),r()),a&2){let t=y();l(),g("disabled",t.disabled)}}var oe=new P("MAT_BUTTON_TOGGLE_DEFAULT_OPTIONS",{providedIn:"root",factory:()=>({hideSingleSelectionIndicator:!1,hideMultipleSelectionIndicator:!1,disabledInteractive:!1})}),ie=new P("MatButtonToggleGroup"),xe={provide:Dt,useExisting:J(()=>Q),multi:!0},D=class{source;value;constructor(o,t){this.source=o,this.value=t}},Q=(()=>{class a{_changeDetector=c($);_dir=c(vt,{optional:!0});_multiple=!1;_disabled=!1;_disabledInteractive=!1;_selectionModel;_rawValue;_controlValueAccessorChangeFn=()=>{};_onTouched=()=>{};_buttonToggles;appearance;get name(){return this._name}set name(t){this._name=t,this._markButtonsForCheck()}_name=c(j).getId("mat-button-toggle-group-");vertical=!1;get value(){let t=this._selectionModel?this._selectionModel.selected:[];return this.multiple?t.map(n=>n.value):t[0]?t[0].value:void 0}set value(t){this._setSelectionByValue(t),this.valueChange.emit(this.value)}valueChange=new I;get selected(){let t=this._selectionModel?this._selectionModel.selected:[];return this.multiple?t:t[0]||null}get multiple(){return this._multiple}set multiple(t){this._multiple=t,this._markButtonsForCheck()}get disabled(){return this._disabled}set disabled(t){this._disabled=t,this._markButtonsForCheck()}get disabledInteractive(){return this._disabledInteractive}set disabledInteractive(t){this._disabledInteractive=t,this._markButtonsForCheck()}get dir(){return this._dir&&this._dir.value==="rtl"?"rtl":"ltr"}change=new I;get hideSingleSelectionIndicator(){return this._hideSingleSelectionIndicator}set hideSingleSelectionIndicator(t){this._hideSingleSelectionIndicator=t,this._markButtonsForCheck()}_hideSingleSelectionIndicator;get hideMultipleSelectionIndicator(){return this._hideMultipleSelectionIndicator}set hideMultipleSelectionIndicator(t){this._hideMultipleSelectionIndicator=t,this._markButtonsForCheck()}_hideMultipleSelectionIndicator;constructor(){let t=c(oe,{optional:!0});this.appearance=t&&t.appearance?t.appearance:"standard",this._hideSingleSelectionIndicator=t?.hideSingleSelectionIndicator??!1,this._hideMultipleSelectionIndicator=t?.hideMultipleSelectionIndicator??!1}ngOnInit(){this._selectionModel=new Ot(this.multiple,void 0,!1)}ngAfterContentInit(){this._selectionModel.select(...this._buttonToggles.filter(t=>t.checked)),this.multiple||this._initializeTabIndex()}writeValue(t){this.value=t,this._changeDetector.markForCheck()}registerOnChange(t){this._controlValueAccessorChangeFn=t}registerOnTouched(t){this._onTouched=t}setDisabledState(t){this.disabled=t}_keydown(t){if(this.multiple||this.disabled||bt(t))return;let e=t.target.id,i=this._buttonToggles.toArray().findIndex(f=>f.buttonId===e),d=null;switch(t.keyCode){case 32:case 13:d=this._buttonToggles.get(i)||null;break;case 38:d=this._getNextButton(i,-1);break;case 37:d=this._getNextButton(i,this.dir==="ltr"?-1:1);break;case 40:d=this._getNextButton(i,1);break;case 39:d=this._getNextButton(i,this.dir==="ltr"?1:-1);break;default:return}d&&(t.preventDefault(),d._onButtonClick(),d.focus())}_emitChangeEvent(t){let n=new D(t,this.value);this._rawValue=n.value,this._controlValueAccessorChangeFn(n.value),this.change.emit(n)}_syncButtonToggle(t,n,e=!1,i=!1){!this.multiple&&this.selected&&!t.checked&&(this.selected.checked=!1),this._selectionModel?n?this._selectionModel.select(t):this._selectionModel.deselect(t):i=!0,i?Promise.resolve().then(()=>this._updateModelValue(t,e)):this._updateModelValue(t,e)}_isSelected(t){return this._selectionModel&&this._selectionModel.isSelected(t)}_isPrechecked(t){return typeof this._rawValue>"u"?!1:this.multiple&&Array.isArray(this._rawValue)?this._rawValue.some(n=>t.value!=null&&n===t.value):t.value===this._rawValue}_initializeTabIndex(){if(this._buttonToggles.forEach(t=>{t.tabIndex=-1}),this.selected)this.selected.tabIndex=0;else for(let t=0;t<this._buttonToggles.length;t++){let n=this._buttonToggles.get(t);if(!n.disabled){n.tabIndex=0;break}}}_getNextButton(t,n){let e=this._buttonToggles;for(let i=1;i<=e.length;i++){let d=(t+n*i+e.length)%e.length,f=e.get(d);if(f&&!f.disabled)return f}return null}_setSelectionByValue(t){if(this._rawValue=t,!this._buttonToggles)return;let n=this._buttonToggles.toArray();if(this.multiple&&t?(Array.isArray(t),this._clearSelection(),t.forEach(e=>this._selectValue(e,n))):(this._clearSelection(),this._selectValue(t,n)),!this.multiple&&n.every(e=>e.tabIndex===-1)){for(let e of n)if(!e.disabled){e.tabIndex=0;break}}}_clearSelection(){this._selectionModel.clear(),this._buttonToggles.forEach(t=>{t.checked=!1,this.multiple||(t.tabIndex=-1)})}_selectValue(t,n){for(let e of n)if(e.value===t){e.checked=!0,this._selectionModel.select(e),this.multiple||(e.tabIndex=0);break}}_updateModelValue(t,n){n&&this._emitChangeEvent(t),this.valueChange.emit(this.value)}_markButtonsForCheck(){this._buttonToggles?.forEach(t=>t._markForCheck())}static \u0275fac=function(n){return new(n||a)};static \u0275dir=tt({type:a,selectors:[["mat-button-toggle-group"]],contentQueries:function(n,e,i){if(n&1&&at(i,R,5),n&2){let d;N(d=L())&&(e._buttonToggles=d)}},hostAttrs:[1,"mat-button-toggle-group"],hostVars:6,hostBindings:function(n,e){n&1&&_("keydown",function(d){return e._keydown(d)}),n&2&&(S("role",e.multiple?"group":"radiogroup")("aria-disabled",e.disabled),z("mat-button-toggle-vertical",e.vertical)("mat-button-toggle-group-appearance-standard",e.appearance==="standard"))},inputs:{appearance:"appearance",name:"name",vertical:[2,"vertical","vertical",h],value:"value",multiple:[2,"multiple","multiple",h],disabled:[2,"disabled","disabled",h],disabledInteractive:[2,"disabledInteractive","disabledInteractive",h],hideSingleSelectionIndicator:[2,"hideSingleSelectionIndicator","hideSingleSelectionIndicator",h],hideMultipleSelectionIndicator:[2,"hideMultipleSelectionIndicator","hideMultipleSelectionIndicator",h]},outputs:{valueChange:"valueChange",change:"change"},exportAs:["matButtonToggleGroup"],features:[rt([xe,{provide:ie,useExisting:a}])]})}return a})(),R=(()=>{class a{_changeDetectorRef=c($);_elementRef=c(Z);_focusMonitor=c(mt);_idGenerator=c(j);_animationDisabled=ht();_checked=!1;ariaLabel;ariaLabelledby=null;_buttonElement;buttonToggleGroup;get buttonId(){return`${this.id}-button`}id;name;value;get tabIndex(){return this._tabIndex()}set tabIndex(t){this._tabIndex.set(t)}_tabIndex;disableRipple=!1;get appearance(){return this.buttonToggleGroup?this.buttonToggleGroup.appearance:this._appearance}set appearance(t){this._appearance=t}_appearance;get checked(){return this.buttonToggleGroup?this.buttonToggleGroup._isSelected(this):this._checked}set checked(t){t!==this._checked&&(this._checked=t,this.buttonToggleGroup&&this.buttonToggleGroup._syncButtonToggle(this,this._checked),this._changeDetectorRef.markForCheck())}get disabled(){return this._disabled||this.buttonToggleGroup&&this.buttonToggleGroup.disabled}set disabled(t){this._disabled=t}_disabled=!1;get disabledInteractive(){return this._disabledInteractive||this.buttonToggleGroup!==null&&this.buttonToggleGroup.disabledInteractive}set disabledInteractive(t){this._disabledInteractive=t}_disabledInteractive;change=new I;constructor(){c(pt).load(_t);let t=c(ie,{optional:!0}),n=c(new dt("tabindex"),{optional:!0})||"",e=c(oe,{optional:!0});this._tabIndex=v(parseInt(n)||0),this.buttonToggleGroup=t,this._appearance=e&&e.appearance?e.appearance:"standard",this._disabledInteractive=e?.disabledInteractive??!1}ngOnInit(){let t=this.buttonToggleGroup;this.id=this.id||this._idGenerator.getId("mat-button-toggle-"),t&&(t._isPrechecked(this)?this.checked=!0:t._isSelected(this)!==this._checked&&t._syncButtonToggle(this,this._checked))}ngAfterViewInit(){this._animationDisabled||this._elementRef.nativeElement.classList.add("mat-button-toggle-animations-enabled"),this._focusMonitor.monitor(this._elementRef,!0)}ngOnDestroy(){let t=this.buttonToggleGroup;this._focusMonitor.stopMonitoring(this._elementRef),t&&t._isSelected(this)&&t._syncButtonToggle(this,!1,!1,!0)}focus(t){this._buttonElement.nativeElement.focus(t)}_onButtonClick(){if(this.disabled)return;let t=this.isSingleSelector()?!0:!this._checked;if(t!==this._checked&&(this._checked=t,this.buttonToggleGroup&&(this.buttonToggleGroup._syncButtonToggle(this,this._checked,!0),this.buttonToggleGroup._onTouched())),this.isSingleSelector()){let n=this.buttonToggleGroup._buttonToggles.find(e=>e.tabIndex===0);n&&(n.tabIndex=-1),this.tabIndex=0}this.change.emit(new D(this,this.value))}_markForCheck(){this._changeDetectorRef.markForCheck()}_getButtonName(){return this.isSingleSelector()?this.buttonToggleGroup.name:this.name||null}isSingleSelector(){return this.buttonToggleGroup&&!this.buttonToggleGroup.multiple}static \u0275fac=function(n){return new(n||a)};static \u0275cmp=C({type:a,selectors:[["mat-button-toggle"]],viewQuery:function(n,e){if(n&1&&ot(_e,5),n&2){let i;N(i=L())&&(e._buttonElement=i.first)}},hostAttrs:["role","presentation",1,"mat-button-toggle"],hostVars:14,hostBindings:function(n,e){n&1&&_("focus",function(){return e.focus()}),n&2&&(S("aria-label",null)("aria-labelledby",null)("id",e.id)("name",null),z("mat-button-toggle-standalone",!e.buttonToggleGroup)("mat-button-toggle-checked",e.checked)("mat-button-toggle-disabled",e.disabled)("mat-button-toggle-disabled-interactive",e.disabledInteractive)("mat-button-toggle-appearance-standard",e.appearance==="standard"))},inputs:{ariaLabel:[0,"aria-label","ariaLabel"],ariaLabelledby:[0,"aria-labelledby","ariaLabelledby"],id:"id",name:"name",value:"value",tabIndex:"tabIndex",disableRipple:[2,"disableRipple","disableRipple",h],appearance:"appearance",checked:[2,"checked","checked",h],disabled:[2,"disabled","disabled",h],disabledInteractive:[2,"disabledInteractive","disabledInteractive",h]},outputs:{change:"change"},exportAs:["matButtonToggle"],ngContentSelectors:ve,decls:7,vars:13,consts:[["button",""],["type","button",1,"mat-button-toggle-button","mat-focus-indicator",3,"click","id","disabled"],[1,"mat-button-toggle-checkbox-wrapper"],[1,"mat-button-toggle-label-content"],[1,"mat-button-toggle-focus-overlay"],["matRipple","",1,"mat-button-toggle-ripple",3,"matRippleTrigger","matRippleDisabled"],["state","checked","aria-hidden","true","appearance","minimal",3,"disabled"]],template:function(n,e){if(n&1&&(et(),s(0,"button",1,0),_("click",function(){return e._onButtonClick()}),T(2,ye,2,1,"div",2),s(3,"span",3),nt(4),r()(),x(5,"span",4)(6,"span",5)),n&2){let i=it(1);g("id",e.buttonId)("disabled",e.disabled&&!e.disabledInteractive||null),S("role",e.isSingleSelector()?"radio":"button")("tabindex",e.disabled&&!e.disabledInteractive?-1:e.tabIndex)("aria-pressed",e.isSingleSelector()?null:e.checked)("aria-checked",e.isSingleSelector()?e.checked:null)("name",e._getButtonName())("aria-label",e.ariaLabel)("aria-labelledby",e.ariaLabelledby)("aria-disabled",e.disabled&&e.disabledInteractive?"true":null),l(2),M(e.buttonToggleGroup&&(!e.buttonToggleGroup.multiple&&!e.buttonToggleGroup.hideSingleSelectionIndicator||e.buttonToggleGroup.multiple&&!e.buttonToggleGroup.hideMultipleSelectionIndicator)?2:-1),l(4),g("matRippleTrigger",i)("matRippleDisabled",e.disableRipple||e.disabled)}},dependencies:[ft,Vt],styles:[`.mat-button-toggle-standalone,
.mat-button-toggle-group {
  position: relative;
  display: inline-flex;
  flex-direction: row;
  white-space: nowrap;
  overflow: hidden;
  -webkit-tap-highlight-color: transparent;
  border-radius: var(--mat-button-toggle-legacy-shape);
  transform: translateZ(0);
}
.mat-button-toggle-standalone:not([class*=mat-elevation-z]),
.mat-button-toggle-group:not([class*=mat-elevation-z]) {
  box-shadow: 0px 3px 1px -2px rgba(0, 0, 0, 0.2), 0px 2px 2px 0px rgba(0, 0, 0, 0.14), 0px 1px 5px 0px rgba(0, 0, 0, 0.12);
}
@media (forced-colors: active) {
  .mat-button-toggle-standalone,
  .mat-button-toggle-group {
    outline: solid 1px;
  }
}

.mat-button-toggle-standalone.mat-button-toggle-appearance-standard,
.mat-button-toggle-group-appearance-standard {
  border-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
  border: solid 1px var(--mat-button-toggle-divider-color, var(--mat-sys-outline));
}
.mat-button-toggle-standalone.mat-button-toggle-appearance-standard .mat-pseudo-checkbox,
.mat-button-toggle-group-appearance-standard .mat-pseudo-checkbox {
  --mat-pseudo-checkbox-minimal-selected-checkmark-color: var(--mat-button-toggle-selected-state-text-color, var(--mat-sys-on-secondary-container));
}
.mat-button-toggle-standalone.mat-button-toggle-appearance-standard:not([class*=mat-elevation-z]),
.mat-button-toggle-group-appearance-standard:not([class*=mat-elevation-z]) {
  box-shadow: none;
}
@media (forced-colors: active) {
  .mat-button-toggle-standalone.mat-button-toggle-appearance-standard,
  .mat-button-toggle-group-appearance-standard {
    outline: 0;
  }
}

.mat-button-toggle-vertical {
  flex-direction: column;
}
.mat-button-toggle-vertical .mat-button-toggle-label-content {
  display: block;
}

.mat-button-toggle {
  white-space: nowrap;
  position: relative;
  color: var(--mat-button-toggle-legacy-text-color);
  font-family: var(--mat-button-toggle-legacy-label-text-font);
  font-size: var(--mat-button-toggle-legacy-label-text-size);
  line-height: var(--mat-button-toggle-legacy-label-text-line-height);
  font-weight: var(--mat-button-toggle-legacy-label-text-weight);
  letter-spacing: var(--mat-button-toggle-legacy-label-text-tracking);
  --mat-pseudo-checkbox-minimal-selected-checkmark-color: var(--mat-button-toggle-legacy-selected-state-text-color);
}
.mat-button-toggle.cdk-keyboard-focused .mat-button-toggle-focus-overlay {
  opacity: var(--mat-button-toggle-legacy-focus-state-layer-opacity);
}
.mat-button-toggle .mat-icon svg {
  vertical-align: top;
}

.mat-button-toggle-checkbox-wrapper {
  display: inline-block;
  justify-content: flex-start;
  align-items: center;
  width: 0;
  height: 18px;
  line-height: 18px;
  overflow: hidden;
  box-sizing: border-box;
  position: absolute;
  top: 50%;
  left: 16px;
  transform: translate3d(0, -50%, 0);
}
[dir=rtl] .mat-button-toggle-checkbox-wrapper {
  left: auto;
  right: 16px;
}
.mat-button-toggle-appearance-standard .mat-button-toggle-checkbox-wrapper {
  left: 12px;
}
[dir=rtl] .mat-button-toggle-appearance-standard .mat-button-toggle-checkbox-wrapper {
  left: auto;
  right: 12px;
}
.mat-button-toggle-checked .mat-button-toggle-checkbox-wrapper {
  width: 18px;
}
.mat-button-toggle-animations-enabled .mat-button-toggle-checkbox-wrapper {
  transition: width 150ms 45ms cubic-bezier(0.4, 0, 0.2, 1);
}
.mat-button-toggle-vertical .mat-button-toggle-checkbox-wrapper {
  transition: none;
}

.mat-button-toggle-checked {
  color: var(--mat-button-toggle-legacy-selected-state-text-color);
  background-color: var(--mat-button-toggle-legacy-selected-state-background-color);
}

.mat-button-toggle-disabled {
  pointer-events: none;
  color: var(--mat-button-toggle-legacy-disabled-state-text-color);
  background-color: var(--mat-button-toggle-legacy-disabled-state-background-color);
  --mat-pseudo-checkbox-minimal-disabled-selected-checkmark-color: var(--mat-button-toggle-legacy-disabled-state-text-color);
}
.mat-button-toggle-disabled.mat-button-toggle-checked {
  background-color: var(--mat-button-toggle-legacy-disabled-selected-state-background-color);
}

.mat-button-toggle-disabled-interactive {
  pointer-events: auto;
}

.mat-button-toggle-appearance-standard {
  color: var(--mat-button-toggle-text-color, var(--mat-sys-on-surface));
  background-color: var(--mat-button-toggle-background-color, transparent);
  font-family: var(--mat-button-toggle-label-text-font, var(--mat-sys-label-large-font));
  font-size: var(--mat-button-toggle-label-text-size, var(--mat-sys-label-large-size));
  line-height: var(--mat-button-toggle-label-text-line-height, var(--mat-sys-label-large-line-height));
  font-weight: var(--mat-button-toggle-label-text-weight, var(--mat-sys-label-large-weight));
  letter-spacing: var(--mat-button-toggle-label-text-tracking, var(--mat-sys-label-large-tracking));
}
.mat-button-toggle-group-appearance-standard .mat-button-toggle-appearance-standard + .mat-button-toggle-appearance-standard {
  border-left: solid 1px var(--mat-button-toggle-divider-color, var(--mat-sys-outline));
}
[dir=rtl] .mat-button-toggle-group-appearance-standard .mat-button-toggle-appearance-standard + .mat-button-toggle-appearance-standard {
  border-left: none;
  border-right: solid 1px var(--mat-button-toggle-divider-color, var(--mat-sys-outline));
}
.mat-button-toggle-group-appearance-standard.mat-button-toggle-vertical .mat-button-toggle-appearance-standard + .mat-button-toggle-appearance-standard {
  border-left: none;
  border-right: none;
  border-top: solid 1px var(--mat-button-toggle-divider-color, var(--mat-sys-outline));
}
.mat-button-toggle-appearance-standard.mat-button-toggle-checked {
  color: var(--mat-button-toggle-selected-state-text-color, var(--mat-sys-on-secondary-container));
  background-color: var(--mat-button-toggle-selected-state-background-color, var(--mat-sys-secondary-container));
}
.mat-button-toggle-appearance-standard.mat-button-toggle-disabled {
  color: var(--mat-button-toggle-disabled-state-text-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
  background-color: var(--mat-button-toggle-disabled-state-background-color, transparent);
}
.mat-button-toggle-appearance-standard.mat-button-toggle-disabled .mat-pseudo-checkbox {
  --mat-pseudo-checkbox-minimal-disabled-selected-checkmark-color: var(--mat-button-toggle-disabled-selected-state-text-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
}
.mat-button-toggle-appearance-standard.mat-button-toggle-disabled.mat-button-toggle-checked {
  color: var(--mat-button-toggle-disabled-selected-state-text-color, color-mix(in srgb, var(--mat-sys-on-surface) 38%, transparent));
  background-color: var(--mat-button-toggle-disabled-selected-state-background-color, color-mix(in srgb, var(--mat-sys-on-surface) 12%, transparent));
}
.mat-button-toggle-appearance-standard .mat-button-toggle-focus-overlay {
  background-color: var(--mat-button-toggle-state-layer-color, var(--mat-sys-on-surface));
}
.mat-button-toggle-appearance-standard:hover .mat-button-toggle-focus-overlay {
  opacity: var(--mat-button-toggle-hover-state-layer-opacity, var(--mat-sys-hover-state-layer-opacity));
}
.mat-button-toggle-appearance-standard.cdk-keyboard-focused .mat-button-toggle-focus-overlay {
  opacity: var(--mat-button-toggle-focus-state-layer-opacity, var(--mat-sys-focus-state-layer-opacity));
}
@media (hover: none) {
  .mat-button-toggle-appearance-standard:hover .mat-button-toggle-focus-overlay {
    display: none;
  }
}

.mat-button-toggle-label-content {
  -webkit-user-select: none;
  user-select: none;
  display: inline-block;
  padding: 0 16px;
  line-height: var(--mat-button-toggle-legacy-height);
  position: relative;
}
.mat-button-toggle-appearance-standard .mat-button-toggle-label-content {
  padding: 0 12px;
  line-height: var(--mat-button-toggle-height, 40px);
}

.mat-button-toggle-label-content > * {
  vertical-align: middle;
}

.mat-button-toggle-focus-overlay {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  border-radius: inherit;
  pointer-events: none;
  opacity: 0;
  background-color: var(--mat-button-toggle-legacy-state-layer-color);
}

@media (forced-colors: active) {
  .mat-button-toggle-checked .mat-button-toggle-focus-overlay {
    border-bottom: solid 500px;
    opacity: 0.5;
    height: 0;
  }
  .mat-button-toggle-checked:hover .mat-button-toggle-focus-overlay {
    opacity: 0.6;
  }
  .mat-button-toggle-checked.mat-button-toggle-appearance-standard .mat-button-toggle-focus-overlay {
    border-bottom: solid 500px;
  }
}
.mat-button-toggle .mat-button-toggle-ripple {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  pointer-events: none;
}

.mat-button-toggle-button {
  border: 0;
  background: none;
  color: inherit;
  padding: 0;
  margin: 0;
  font: inherit;
  outline: none;
  width: 100%;
  cursor: pointer;
}
.mat-button-toggle-animations-enabled .mat-button-toggle-button {
  transition: padding 150ms 45ms cubic-bezier(0.4, 0, 0.2, 1);
}
.mat-button-toggle-vertical .mat-button-toggle-button {
  transition: none;
}
.mat-button-toggle-disabled .mat-button-toggle-button {
  cursor: default;
}
.mat-button-toggle-button::-moz-focus-inner {
  border: 0;
}
.mat-button-toggle-checked .mat-button-toggle-button:has(.mat-button-toggle-checkbox-wrapper) {
  padding-left: 30px;
}
[dir=rtl] .mat-button-toggle-checked .mat-button-toggle-button:has(.mat-button-toggle-checkbox-wrapper) {
  padding-left: 0;
  padding-right: 30px;
}

.mat-button-toggle-standalone.mat-button-toggle-appearance-standard {
  --mat-focus-indicator-border-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
}

.mat-button-toggle-group-appearance-standard:not(.mat-button-toggle-vertical) .mat-button-toggle:last-of-type .mat-button-toggle-button::before {
  border-top-right-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
  border-bottom-right-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
}
.mat-button-toggle-group-appearance-standard:not(.mat-button-toggle-vertical) .mat-button-toggle:first-of-type .mat-button-toggle-button::before {
  border-top-left-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
  border-bottom-left-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
}

.mat-button-toggle-group-appearance-standard.mat-button-toggle-vertical .mat-button-toggle:last-of-type .mat-button-toggle-button::before {
  border-bottom-right-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
  border-bottom-left-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
}
.mat-button-toggle-group-appearance-standard.mat-button-toggle-vertical .mat-button-toggle:first-of-type .mat-button-toggle-button::before {
  border-top-right-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
  border-top-left-radius: var(--mat-button-toggle-shape, var(--mat-sys-corner-extra-large));
}
`],encapsulation:2,changeDetection:0})}return a})(),le=(()=>{class a{static \u0275fac=function(n){return new(n||a)};static \u0275mod=X({type:a});static \u0275inj=K({imports:[xt,R,yt]})}return a})();var re=()=>["code","name","status"],se=(a,o)=>o.id,Ie=(a,o)=>o.v;function Ce(a,o){if(a&1&&(s(0,"mat-option",3),u(1),r()),a&2){let t=o.$implicit;g("value",t.id),l(),m(t.className)}}function Se(a,o){if(a&1&&(s(0,"mat-option",3),u(1),p(2,"dateLocale"),r()),a&2){let t=o.$implicit;g("value",t.id),l(),lt(" ",st(2,3,t.dateTime,"datetime")," \u2014 ",t.title," ")}}function Te(a,o){a&1&&(s(0,"th",16),u(1),p(2,"t"),r()),a&2&&(l(),m(b(2,1,"youth.code")))}function Me(a,o){if(a&1&&(s(0,"td",17),u(1),r()),a&2){let t=o.$implicit;l(),m(t.youth==null?null:t.youth.code)}}function we(a,o){a&1&&(s(0,"th",16),u(1),p(2,"t"),r()),a&2&&(l(),m(b(2,1,"youth.fullName")))}function Be(a,o){if(a&1&&(s(0,"td",17),u(1),p(2,"fullName"),r()),a&2){let t=o.$implicit;l(),m(b(2,1,t.youth==null?null:t.youth.fullName))}}function Ae(a,o){a&1&&(s(0,"th",16),u(1),p(2,"t"),r()),a&2&&(l(),m(b(2,1,"attendance.markFor")))}function Ee(a,o){if(a&1&&(s(0,"mat-button-toggle",3)(1,"mat-icon"),u(2),r(),u(3),p(4,"t"),r()),a&2){let t=o.$implicit;g("value",t.v),l(2),m(t.icon),l(),H(" ",b(4,3,t.k)," ")}}function De(a,o){if(a&1){let t=V();s(0,"td",17)(1,"mat-button-toggle-group",18),_("change",function(e){let i=F(t).$implicit,d=y(2);return O(d.setStatus(i.enrollmentId,e.value))}),w(2,Ee,5,5,"mat-button-toggle",3,Ie),r()()}if(a&2){let t=o.$implicit,n=y(2);l(),g("value",t.status),l(),B(n.statuses)}}function Re(a,o){a&1&&x(0,"tr",19)}function Ge(a,o){a&1&&x(0,"tr",20)}function Pe(a,o){if(a&1){let t=V();s(0,"table",6),A(1,7),k(2,Te,3,3,"th",8)(3,Me,2,1,"td",9),E(),A(4,10),k(5,we,3,3,"th",8)(6,Be,3,3,"td",9),E(),A(7,11),k(8,Ae,3,3,"th",8)(9,De,4,1,"td",9),E(),k(10,Re,1,0,"tr",12)(11,Ge,1,0,"tr",13),r(),s(12,"div",14)(13,"button",15),_("click",function(){F(t);let e=y();return O(e.saveAll())}),s(14,"mat-icon"),u(15,"save"),r(),u(16),p(17,"t"),r()()}if(a&2){let t=y();g("dataSource",t.rows()),l(10),g("matHeaderRowDef",U(6,re)),l(),g("matRowDefColumns",U(7,re)),l(5),H(" ",b(17,4,"attendance.saveAll")," ")}}function Fe(a,o){a&1&&(s(0,"p",5),u(1),p(2,"t"),r()),a&2&&(l(),m(b(2,1,"common.noData")))}var de=class a{route=c(ut);api=c(Ht);notify=c(Ut);AttendanceStatus=gt;statuses=[{v:"Present",k:"attendance.present",icon:"check_circle"},{v:"Absent",k:"attendance.absent",icon:"cancel"},{v:"Excused",k:"attendance.excused",icon:"event_busy"}];classes=$t(this.api.list("/classes"),{initialValue:{items:[],total:0,page:1,pageSize:50}});selectedClassId=v(null);selectedSessionId=v(null);sessions=v([]);rows=v([]);constructor(){this.route.queryParamMap.subscribe(o=>{let t=o.get("classId"),n=o.get("sessionId");t&&(this.selectedClassId.set(t),this.loadSessions(t,n))})}onClassChange(o){this.selectedClassId.set(o),this.selectedSessionId.set(null),this.rows.set([]),this.loadSessions(o,null)}onSessionChange(o){this.selectedSessionId.set(o),this.loadRows()}loadSessions(o,t){this.api.list("/sessions",{classId:o}).subscribe(n=>{this.sessions.set(n.items),t&&(this.selectedSessionId.set(t),this.loadRows())})}loadRows(){let o=this.selectedClassId(),t=this.selectedSessionId();!o||!t||q({enrollments:this.api.list("/class-youth",{classId:o}),attendance:this.api.list("/sessions/attendances",{sessionId:t}),youth:this.api.list("/youths")}).subscribe(({enrollments:n,attendance:e,youth:i})=>{let d=n.items.map(f=>{let ce=e.items.find(G=>G.catechismClassEucharisticYouthId===f.id);return{enrollmentId:f.id,youth:i.items.find(G=>G.id===f.eucharisticYouthId),status:ce?.attendanceStatus??"None"}});this.rows.set(d)})}setStatus(o,t){this.rows.update(n=>n.map(e=>e.enrollmentId===o?W(Y({},e),{status:t}):e))}saveAll(){let o=this.selectedSessionId();if(!o)return;let t={catechismSessionId:o,entries:this.rows().map(n=>({catechismClassEucharisticYouthId:n.enrollmentId,attendanceStatus:n.status}))};this.api.post("/sessions/attendances",t).subscribe({next:()=>this.notify.success("Saved"),error:()=>this.notify.error("Save failed")})}static \u0275fac=function(t){return new(t||a)};static \u0275cmp=C({type:a,selectors:[["app-attendance"]],decls:23,vars:13,consts:[[1,"picker-row"],["appearance","outline"],[3,"selectionChange","value"],[3,"value"],[3,"selectionChange","value","disabled"],[1,"text-center","mt-16"],["mat-table","",1,"full-width",3,"dataSource"],["matColumnDef","code"],["mat-header-cell","",4,"matHeaderCellDef"],["mat-cell","",4,"matCellDef"],["matColumnDef","name"],["matColumnDef","status"],["mat-header-row","",4,"matHeaderRowDef"],["mat-row","",4,"matRowDef","matRowDefColumns"],[1,"actions","mt-16"],["mat-flat-button","","color","primary",3,"click"],["mat-header-cell",""],["mat-cell",""],[3,"change","value"],["mat-header-row",""],["mat-row",""]],template:function(t,n){t&1&&(s(0,"mat-card")(1,"mat-card-header")(2,"mat-card-title"),u(3),p(4,"t"),r()(),s(5,"mat-card-content")(6,"div",0)(7,"mat-form-field",1)(8,"mat-label"),u(9),p(10,"t"),r(),s(11,"mat-select",2),_("selectionChange",function(i){return n.onClassChange(i.value)}),w(12,Ce,2,2,"mat-option",3,se),r()(),s(14,"mat-form-field",1)(15,"mat-label"),u(16),p(17,"t"),r(),s(18,"mat-select",4),_("selectionChange",function(i){return n.onSessionChange(i.value)}),w(19,Se,3,6,"mat-option",3,se),r()()(),T(21,Pe,18,8)(22,Fe,3,3,"p",5),r()()),t&2&&(l(3),m(b(4,7,"nav.attendance")),l(6),m(b(10,9,"nav.classes")),l(2),g("value",n.selectedClassId()),l(),B(n.classes().items),l(4),m(b(17,11,"class.sessions")),l(2),g("value",n.selectedSessionId())("disabled",!n.selectedClassId()),l(),B(n.sessions()),l(2),M(n.rows().length>0?21:n.selectedSessionId()?22:-1))},dependencies:[ct,Rt,At,Tt,wt,Bt,Mt,Ft,Pt,Gt,zt,Lt,Nt,ne,Qt,Wt,Zt,qt,Yt,Xt,Jt,Kt,te,ee,le,Q,R,It,kt,St,Ct,Et,ae,jt],styles:[".picker-row[_ngcontent-%COMP%]{display:flex;gap:12px;flex-wrap:wrap;margin-bottom:16px}.picker-row[_ngcontent-%COMP%]   mat-form-field[_ngcontent-%COMP%]{min-width:260px;flex:1}table.full-width[_ngcontent-%COMP%]{width:100%}.actions[_ngcontent-%COMP%]{display:flex;justify-content:flex-end}"]})};export{de as AttendancePage};
