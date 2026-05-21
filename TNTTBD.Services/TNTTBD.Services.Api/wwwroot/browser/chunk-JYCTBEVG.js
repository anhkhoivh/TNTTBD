import{a as Rt,b as Bt,c as At}from"./chunk-IXUQLJU2.js";import{a as Lt,b as Ht,c as Vt,d as Qt,e as $t}from"./chunk-HOEFEE77.js";import{a as qt,b as Gt}from"./chunk-ITYN3B67.js";import"./chunk-U7VS42BG.js";import{a as zt,b as Pt}from"./chunk-LOCDUIU6.js";import{c as Ft}from"./chunk-54AP4VZ4.js";import{a as Ut}from"./chunk-EDWJVDPD.js";import{a as ot,b as st}from"./chunk-2UQS5AU6.js";import"./chunk-L4BKUYED.js";import{b as ct,f as mt,g as ut}from"./chunk-R3HFD3F4.js";import"./chunk-5AL2SDHA.js";import"./chunk-WOPUPVC7.js";import{b as Nt,c as Ot}from"./chunk-F2VEUWUN.js";import{a as Tt}from"./chunk-3ODL2LXL.js";import{b as Et,c as kt,f as wt,g as ce}from"./chunk-RR5LTW6N.js";import"./chunk-4MZPLR3U.js";import{a as jt}from"./chunk-267P6CPO.js";import{a as lt,c as pt,h as dt}from"./chunk-EZCVIYWJ.js";import"./chunk-QHBGS32Z.js";import"./chunk-JMZ5XG7L.js";import{b as ft,d as P,e as _t,g as gt,h as vt,k as bt,m as yt,n as xt,o as Ct,p as St,s as Mt,t as It,v as Dt}from"./chunk-PTFPPLIF.js";import"./chunk-TBZDRLNV.js";import{a as pe,b as de,c as ht}from"./chunk-FOIS2F75.js";import{c as $e,h as tt,i as at,j as rt}from"./chunk-CSGIYDIV.js";import{$ as Je,A as Ue,B as Ke,N as We,P as Ze,Q as Ye,d as ge,f as Qe,fa as Xe,ha as et,ia as it,ja as le,ka as nt,t as qe,v as Ge,y as je}from"./chunk-UTAGKRPA.js";import{$ as X,$b as ae,Aa as j,Ab as v,Bb as b,Bc as _e,Ca as U,Cb as Pe,Da as K,Db as ie,Eb as N,Fb as O,Fc as oe,Gb as m,Hb as s,Ib as r,Jb as g,Jc as He,Mc as se,Pb as M,Pc as Q,Qb as ne,Qc as Ve,Rb as H,S as w,T as Ee,U as D,Vb as I,Xa as a,Xb as _,Yb as W,Zb as Z,_ as ke,_b as V,ac as x,ba as we,bb as A,bc as C,d as Me,da as f,db as Oe,fc as k,gb as Fe,gc as Re,h as J,hc as F,ia as q,ic as re,ja as G,jc as p,kb as E,kc as u,lb as ee,lc as z,mb as S,mc as he,o as Ie,oc as Be,pb as T,qa as B,qb as te,ra as Te,rb as ze,sc as Ae,t as De,tc as fe,va as y,vc as Y,xc as Le,yc as d,za as Ne,zb as L,zc as c}from"./chunk-WJUDORRU.js";var ri=["*"];function oi(t,o){t&1&&Z(0)}var ve=(()=>{class t{_elementRef=f(U);constructor(){}focus(){this._elementRef.nativeElement.focus()}static \u0275fac=function(i){return new(i||t)};static \u0275dir=S({type:t,selectors:[["","cdkStepHeader",""]],hostAttrs:["role","tab"]})}return t})(),be=(()=>{class t{template=f(A);constructor(){}static \u0275fac=function(i){return new(i||t)};static \u0275dir=S({type:t,selectors:[["","cdkStepLabel",""]]})}return t})();var R={NUMBER:"number",EDIT:"edit",DONE:"done",ERROR:"error"},si=new we("STEPPER_GLOBAL_OPTIONS"),me=(()=>{class t{_stepperOptions;_stepper=f($);_displayDefaultIndicatorType;stepLabel;_childForms;content;stepControl;get interacted(){return this._interacted()}set interacted(e){this._interacted.set(e)}_interacted=y(!1);interactedStream=new B;label;errorMessage;ariaLabel;ariaLabelledby;get state(){return this._state()}set state(e){this._state.set(e)}_state=y(void 0);get editable(){return this._editable()}set editable(e){this._editable.set(e)}_editable=y(!0);optional=!1;get completed(){let e=this._completedOverride(),i=this._interacted();return e??(i&&(!this.stepControl||this.stepControl.valid))}set completed(e){this._completedOverride.set(e)}_completedOverride=y(null);index=y(-1);isSelected=oe(()=>this._stepper.selectedIndex===this.index());indicatorType=oe(()=>{let e=this.isSelected(),i=this.completed,n=this._state()??R.NUMBER,l=this._editable();return this._showError()&&this.hasError&&!e?R.ERROR:this._displayDefaultIndicatorType?!i||e?R.NUMBER:l?R.EDIT:R.DONE:i&&!e?R.DONE:i&&e?n:l&&e?R.EDIT:n});isNavigable=oe(()=>{let e=this.isSelected();return this.completed||e||!this._stepper.linear});get hasError(){let e=this._customError();return e??this._getDefaultError()}set hasError(e){this._customError.set(e)}_customError=y(null);_getDefaultError(){return this.interacted&&!!this.stepControl?.invalid}constructor(){let e=f(si,{optional:!0});this._stepperOptions=e||{},this._displayDefaultIndicatorType=this._stepperOptions.displayDefaultIndicatorType!==!1}select(){this._stepper.selected=this}reset(){this._interacted.set(!1),this._completedOverride()!=null&&this._completedOverride.set(!1),this._customError()!=null&&this._customError.set(!1),this.stepControl&&(this._childForms?.forEach(e=>e.resetForm?.()),this.stepControl.reset())}ngOnChanges(){this._stepper._stateChanged()}_markAsInteracted(){this._interacted()||(this._interacted.set(!0),this.interactedStream.emit(this))}_showError(){return this._stepperOptions.showError??this._customError()!=null}static \u0275fac=function(i){return new(i||t)};static \u0275cmp=E({type:t,selectors:[["cdk-step"]],contentQueries:function(i,n,l){if(i&1&&V(l,be,5)(l,_t,5),i&2){let h;x(h=C())&&(n.stepLabel=h.first),x(h=C())&&(n._childForms=h)}},viewQuery:function(i,n){if(i&1&&ae(A,7),i&2){let l;x(l=C())&&(n.content=l.first)}},inputs:{stepControl:"stepControl",label:"label",errorMessage:"errorMessage",ariaLabel:[0,"aria-label","ariaLabel"],ariaLabelledby:[0,"aria-labelledby","ariaLabelledby"],state:"state",editable:[2,"editable","editable",Q],optional:[2,"optional","optional",Q],completed:[2,"completed","completed",Q],hasError:[2,"hasError","hasError",Q]},outputs:{interactedStream:"interacted"},exportAs:["cdkStep"],features:[Ne],ngContentSelectors:ri,decls:1,vars:0,template:function(i,n){i&1&&(W(),ze(0,oi,1,0,"ng-template"))},encapsulation:2,changeDetection:0})}return t})(),$=(()=>{class t{_dir=f(it,{optional:!0});_changeDetectorRef=f(se);_elementRef=f(U);_destroyed=new J;_keyManager;_steps;steps=new K;_stepHeader;_sortedHeaders=new K;get linear(){return this._linear()}set linear(e){this._linear.set(e)}_linear=y(!1);get selectedIndex(){return this._selectedIndex()}set selectedIndex(e){this._steps?(this._isValidIndex(e),this.selectedIndex!==e&&(this.selected?._markAsInteracted(),!this._anyControlsInvalidOrPending(e)&&(e>=this.selectedIndex||this.steps.toArray()[e].editable)&&this._updateSelectedItemIndex(e))):this._selectedIndex.set(e)}_selectedIndex=y(0);get selected(){return this.steps?this.steps.toArray()[this.selectedIndex]:void 0}set selected(e){this.selectedIndex=e&&this.steps?this.steps.toArray().indexOf(e):-1}selectionChange=new B;selectedIndexChange=new B;_groupId=f(Ye).getId("cdk-stepper-");get orientation(){return this._orientation}set orientation(e){this._orientation=e,this._keyManager&&this._keyManager.withVerticalOrientation(e==="vertical")}_orientation="horizontal";constructor(){}ngAfterContentInit(){this._steps.changes.pipe(w(this._steps),D(this._destroyed)).subscribe(e=>{this.steps.reset(e.filter(i=>i._stepper===this)),this.steps.forEach((i,n)=>i.index.set(n)),this.steps.notifyOnChanges()})}ngAfterViewInit(){if(this._stepHeader.changes.pipe(w(this._stepHeader),D(this._destroyed)).subscribe(e=>{this._sortedHeaders.reset(e.toArray().sort((i,n)=>i._elementRef.nativeElement.compareDocumentPosition(n._elementRef.nativeElement)&Node.DOCUMENT_POSITION_FOLLOWING?-1:1)),this._sortedHeaders.notifyOnChanges()}),this._keyManager=new Ze(this._sortedHeaders).withWrap().withHomeAndEnd().withVerticalOrientation(this._orientation==="vertical"),this._keyManager.updateActiveItem(this.selectedIndex),(this._dir?this._dir.change:Ie()).pipe(w(this._layoutDirection()),D(this._destroyed)).subscribe(e=>this._keyManager?.withHorizontalOrientation(e)),this._keyManager.updateActiveItem(this.selectedIndex),this.steps.changes.subscribe(()=>{this.selected||this._selectedIndex.set(Math.max(this.selectedIndex-1,0))}),this._isValidIndex(this.selectedIndex)||this._selectedIndex.set(0),this.linear&&this.selectedIndex>0){let e=this.steps.toArray().slice(0,this._selectedIndex());for(let i of e)i._markAsInteracted()}}ngOnDestroy(){this._keyManager?.destroy(),this.steps.destroy(),this._sortedHeaders.destroy(),this._destroyed.next(),this._destroyed.complete()}next(){this.selectedIndex=Math.min(this._selectedIndex()+1,this.steps.length-1)}previous(){this.selectedIndex=Math.max(this._selectedIndex()-1,0)}reset(){this._updateSelectedItemIndex(0),this.steps.forEach(e=>e.reset()),this._stateChanged()}_getStepLabelId(e){return`${this._groupId}-label-${e}`}_getStepContentId(e){return`${this._groupId}-content-${e}`}_stateChanged(){this._changeDetectorRef.markForCheck()}_getAnimationDirection(e){let i=e-this._selectedIndex();return i<0?this._layoutDirection()==="rtl"?"next":"previous":i>0?this._layoutDirection()==="rtl"?"previous":"next":"current"}_getFocusIndex(){return this._keyManager?this._keyManager.activeItemIndex:this._selectedIndex()}_updateSelectedItemIndex(e){let i=this.steps.toArray(),n=this._selectedIndex();this.selectionChange.emit({selectedIndex:e,previouslySelectedIndex:n,selectedStep:i[e],previouslySelectedStep:i[n]}),this._keyManager&&(this._containsFocus()?this._keyManager.setActiveItem(e):this._keyManager.updateActiveItem(e)),this._selectedIndex.set(e),this.selectedIndexChange.emit(e),this._stateChanged()}_onKeydown(e){let i=We(e),n=e.keyCode,l=this._keyManager;l?.activeItemIndex!=null&&!i&&(n===32||n===13)?(this.selectedIndex=l.activeItemIndex,e.preventDefault()):l?.setFocusOrigin("keyboard").onKeydown(e)}_anyControlsInvalidOrPending(e){return this.linear&&e>=0?this.steps.toArray().slice(0,e).some(i=>{let n=i.stepControl;return(n?n.invalid||n.pending||!i.interacted:!i.completed)&&!i.optional&&!i._completedOverride()}):!1}_layoutDirection(){return this._dir&&this._dir.value==="rtl"?"rtl":"ltr"}_containsFocus(){let e=this._elementRef.nativeElement,i=qe();return e===i||e.contains(i)}_isValidIndex(e){return e>-1&&(!this.steps||e<this.steps.length)}static \u0275fac=function(i){return new(i||t)};static \u0275dir=S({type:t,selectors:[["","cdkStepper",""]],contentQueries:function(i,n,l){if(i&1&&V(l,me,5)(l,ve,5),i&2){let h;x(h=C())&&(n._steps=h),x(h=C())&&(n._stepHeader=h)}},inputs:{linear:[2,"linear","linear",Q],selectedIndex:[2,"selectedIndex","selectedIndex",Ve],selected:"selected",orientation:"orientation"},outputs:{selectionChange:"selectionChange",selectedIndexChange:"selectedIndexChange"},exportAs:["cdkStepper"]})}return t})(),Wt=(()=>{class t{_stepper=f($);type="submit";constructor(){}static \u0275fac=function(i){return new(i||t)};static \u0275dir=S({type:t,selectors:[["button","cdkStepperNext",""]],hostVars:1,hostBindings:function(i,n){i&1&&I("click",function(){return n._stepper.next()}),i&2&&H("type",n.type)},inputs:{type:"type"}})}return t})(),Zt=(()=>{class t{_stepper=f($);type="button";constructor(){}static \u0275fac=function(i){return new(i||t)};static \u0275dir=S({type:t,selectors:[["button","cdkStepperPrevious",""]],hostVars:1,hostBindings:function(i,n){i&1&&I("click",function(){return n._stepper.previous()}),i&2&&H("type",n.type)},inputs:{type:"type"}})}return t})(),Yt=(()=>{class t{static \u0275fac=function(i){return new(i||t)};static \u0275mod=ee({type:t});static \u0275inj=X({imports:[le]})}return t})();var li=(t,o,e)=>({index:t,active:o,optional:e});function pi(t,o){if(t&1&&M(0,2),t&2){let e=_();m("ngTemplateOutlet",e.iconOverrides[e.state])("ngTemplateOutletContext",Le(2,li,e.index,e.active,e.optional))}}function di(t,o){if(t&1&&(s(0,"span",7),p(1),r()),t&2){let e=_(2);a(),u(e._getDefaultTextForState(e.state))}}function ci(t,o){if(t&1&&(s(0,"span",8),p(1),r()),t&2){let e=_(3);a(),u(e._intl.completedLabel)}}function mi(t,o){if(t&1&&(s(0,"span",8),p(1),r()),t&2){let e=_(3);a(),u(e._intl.editableLabel)}}function ui(t,o){if(t&1&&(v(0,ci,2,1,"span",8)(1,mi,2,1,"span",8),s(2,"mat-icon",7),p(3),r()),t&2){let e=_(2);b(e.state==="done"?0:e.state==="edit"?1:-1),a(3),u(e._getDefaultTextForState(e.state))}}function hi(t,o){if(t&1&&v(0,di,2,1,"span",7)(1,ui,4,2),t&2){let e,i=_();b((e=i.state)==="number"?0:1)}}function fi(t,o){t&1&&(s(0,"div",4),M(1,9),r()),t&2&&(a(),m("ngTemplateOutlet",o.template))}function _i(t,o){if(t&1&&(s(0,"div",4),p(1),r()),t&2){let e=_();a(),u(e.label)}}function gi(t,o){if(t&1&&(s(0,"div",5),p(1),r()),t&2){let e=_();a(),u(e._intl.optionalLabel)}}function vi(t,o){if(t&1&&(s(0,"div",6),p(1),r()),t&2){let e=_();a(),u(e.errorMessage)}}var Jt=["*"];function bi(t,o){}function yi(t,o){if(t&1&&(Z(0),te(1,bi,0,0,"ng-template",0)),t&2){let e=_();a(),m("cdkPortalOutlet",e._portal)}}var xi=["animatedContainer"],Xt=t=>({steps:t}),ei=t=>({step:t});function Ci(t,o){t&1&&Z(0)}function Si(t,o){if(t&1&&(s(0,"div",5),M(1,9)(2,6),r()),t&2){let e=_(2),i=k(6);a(),m("ngTemplateOutlet",e.headerPrefix()),a(),m("ngTemplateOutlet",i)("ngTemplateOutletContext",Y(3,Xt,e.steps))}}function Mi(t,o){if(t&1&&M(0,6),t&2){let e=_(2),i=k(6);m("ngTemplateOutlet",i)("ngTemplateOutletContext",Y(2,Xt,e.steps))}}function Ii(t,o){if(t&1&&(s(0,"div",10,2),M(2,9),r()),t&2){let e=o.$implicit,i=o.$index,n=_(2);re("mat-horizontal-stepper-content-"+n._getAnimationDirection(i)),m("id",n._getStepContentId(i)),L("aria-labelledby",n._getStepLabelId(i))("inert",n.selectedIndex===i?null:""),a(2),m("ngTemplateOutlet",e.content)}}function Di(t,o){if(t&1&&(s(0,"div",3),v(1,Si,3,5,"div",5)(2,Mi,1,4,"ng-container",6),s(3,"div",7),N(4,Ii,3,6,"div",8,ie),r()()),t&2){let e=_();a(),b(e.headerPrefix()?1:2),a(3),O(e.steps)}}function Ei(t,o){if(t&1&&M(0,9),t&2){let e=_(2);m("ngTemplateOutlet",e.headerPrefix())}}function ki(t,o){if(t&1&&(s(0,"div",11),M(1,6),s(2,"div",12,2)(4,"div",13)(5,"div",14),M(6,9),r()()()()),t&2){let e=o.$implicit,i=o.$index,n=o.$index,l=o.$count,h=_(2),ue=k(4);a(),m("ngTemplateOutlet",ue)("ngTemplateOutletContext",Y(11,ei,e)),a(),F("mat-stepper-vertical-line",n!==l-1)("mat-vertical-content-container-active",h.selectedIndex===i),L("inert",h.selectedIndex===i?null:"")("aria-label",h.ariaLabel),a(2),m("id",h._getStepContentId(i)),L("aria-labelledby",h._getStepLabelId(i)),a(2),m("ngTemplateOutlet",e.content)}}function wi(t,o){if(t&1&&(s(0,"div",4),v(1,Ei,1,1,"ng-container",9),N(2,ki,7,13,"div",11,ie),r()),t&2){let e=_();a(),b(e.headerPrefix()?1:-1),a(),O(e.steps)}}function Ti(t,o){if(t&1){let e=ne();s(0,"mat-step-header",15),I("click",function(){let n=q(e).step;return G(n.select())})("keydown",function(n){q(e);let l=_();return G(l._onKeydown(n))}),r()}if(t&2){let e=o.step,i=_();F("mat-horizontal-stepper-header",i.orientation==="horizontal")("mat-vertical-stepper-header",i.orientation==="vertical"),m("tabIndex",i._getFocusIndex()===e.index()?0:-1)("id",i._getStepLabelId(e.index()))("index",e.index())("state",e.indicatorType())("label",e.stepLabel||e.label)("selected",e.isSelected())("active",e.isNavigable())("optional",e.optional)("errorMessage",e.errorMessage)("iconOverrides",i._iconOverrides)("disableRipple",i.disableRipple||!e.isNavigable())("color",e.color||i.color),L("role",i.orientation==="horizontal"?"tab":"button")("aria-posinset",i.orientation==="horizontal"?e.index()+1:null)("aria-setsize",i.orientation==="horizontal"?i.steps.length:null)("aria-selected",i.orientation==="horizontal"?e.isSelected():null)("aria-current",i.orientation==="vertical"&&e.isSelected()?"step":null)("aria-disabled",i.orientation==="vertical"&&e.isSelected()?"true":null)("aria-expanded",i.orientation==="vertical"?e.isSelected():null)("aria-controls",i._getStepContentId(e.index()))("aria-label",e.ariaLabel||null)("aria-labelledby",!e.ariaLabel&&e.ariaLabelledby?e.ariaLabelledby:null)("aria-disabled",e.isNavigable()?null:!0)}}function Ni(t,o){t&1&&g(0,"div",17)}function Oi(t,o){if(t&1&&(M(0,6),v(1,Ni,1,0,"div",17)),t&2){let e=o.$implicit,i=o.$index,n=o.$count;_(2);let l=k(4);m("ngTemplateOutlet",l)("ngTemplateOutletContext",Y(3,ei,e)),a(),b(i!==n-1?1:-1)}}function Fi(t,o){if(t&1&&(s(0,"div",16),N(1,Oi,2,5,null,null,ie),r()),t&2){let e=o.steps,i=_();L("aria-label",i.ariaLabel),a(),O(e)}}var ye=(()=>{class t extends be{static \u0275fac=(()=>{let e;return function(n){return(e||(e=j(t)))(n||t)}})();static \u0275dir=S({type:t,selectors:[["","matStepLabel",""]],features:[T]})}return t})(),zi=(()=>{class t{changes=new J;optionalLabel="Optional";completedLabel="Completed";editableLabel="Editable";static \u0275fac=function(i){return new(i||t)};static \u0275prov=ke({token:t,factory:t.\u0275fac,providedIn:"root"})}return t})(),xe=(()=>{class t extends ve{_intl=f(zi);_focusMonitor=f(je);_intlSubscription;state;label;errorMessage;iconOverrides;index;selected=!1;active=!1;optional=!1;disableRipple=!1;color;constructor(){super();let e=f(Ue);e.load(et),e.load(Ke);let i=f(se);this._intlSubscription=this._intl.changes.subscribe(()=>i.markForCheck())}ngAfterViewInit(){this._focusMonitor.monitor(this._elementRef,!0)}ngOnDestroy(){this._intlSubscription.unsubscribe(),this._focusMonitor.stopMonitoring(this._elementRef)}focus(e,i){e?this._focusMonitor.focusVia(this._elementRef,e,i):this._elementRef.nativeElement.focus(i)}_stringLabel(){return this.label instanceof ye?null:this.label}_templateLabel(){return this.label instanceof ye?this.label:null}_getHostElement(){return this._elementRef.nativeElement}_getDefaultTextForState(e){return e=="number"?`${this.index+1}`:e=="edit"?"create":e=="error"?"warning":e}_hasEmptyLabel(){return!this._stringLabel()&&!this._templateLabel()&&!this._hasOptionalLabel()&&!this._hasErrorLabel()}_hasOptionalLabel(){return this.optional&&this.state!=="error"}_hasErrorLabel(){return this.state==="error"}static \u0275fac=function(i){return new(i||t)};static \u0275cmp=E({type:t,selectors:[["mat-step-header"]],hostAttrs:["role","",1,"mat-step-header"],hostVars:4,hostBindings:function(i,n){i&2&&(re("mat-"+(n.color||"primary")),F("mat-step-header-empty-label",n._hasEmptyLabel()))},inputs:{state:"state",label:"label",errorMessage:"errorMessage",iconOverrides:"iconOverrides",index:"index",selected:"selected",active:"active",optional:"optional",disableRipple:"disableRipple",color:"color"},features:[T],decls:10,vars:17,consts:[["matRipple","",1,"mat-step-header-ripple","mat-focus-indicator",3,"matRippleTrigger","matRippleDisabled"],[1,"mat-step-icon-content"],[3,"ngTemplateOutlet","ngTemplateOutletContext"],[1,"mat-step-label"],[1,"mat-step-text-label"],[1,"mat-step-optional"],[1,"mat-step-sub-label-error"],["aria-hidden","true"],[1,"cdk-visually-hidden"],[3,"ngTemplateOutlet"]],template:function(i,n){if(i&1&&(g(0,"div",0),s(1,"div")(2,"div",1),v(3,pi,1,6,"ng-container",2)(4,hi,2,1),r()(),s(5,"div",3),v(6,fi,2,1,"div",4)(7,_i,2,1,"div",4),v(8,gi,2,1,"div",5),v(9,vi,2,1,"div",6),r()),i&2){let l;m("matRippleTrigger",n._getHostElement())("matRippleDisabled",n.disableRipple),a(),re(Ae("mat-step-icon-state-",n.state," mat-step-icon")),F("mat-step-icon-selected",n.selected),a(2),b(n.iconOverrides&&n.iconOverrides[n.state]?3:4),a(2),F("mat-step-label-active",n.active)("mat-step-label-selected",n.selected)("mat-step-label-error",n.state=="error"),a(),b((l=n._templateLabel())?6:n._stringLabel()?7:-1,l),a(2),b(n._hasOptionalLabel()?8:-1),a(),b(n._hasErrorLabel()?9:-1)}},dependencies:[Xe,ge,pe],styles:[`.mat-step-header {
  overflow: hidden;
  outline: none;
  cursor: pointer;
  position: relative;
  box-sizing: content-box;
  -webkit-tap-highlight-color: transparent;
}
.mat-step-header:focus-visible .mat-focus-indicator::before {
  content: "";
}
.mat-step-header:hover[aria-disabled=true] {
  cursor: default;
}
.mat-step-header:hover:not([aria-disabled]), .mat-step-header:hover[aria-disabled=false] {
  background-color: var(--mat-stepper-header-hover-state-layer-color, color-mix(in srgb, var(--mat-sys-on-surface) calc(var(--mat-sys-hover-state-layer-opacity) * 100%), transparent));
  border-radius: var(--mat-stepper-header-hover-state-layer-shape, var(--mat-sys-corner-medium));
}
.mat-step-header.cdk-keyboard-focused, .mat-step-header.cdk-program-focused {
  background-color: var(--mat-stepper-header-focus-state-layer-color, color-mix(in srgb, var(--mat-sys-on-surface) calc(var(--mat-sys-focus-state-layer-opacity) * 100%), transparent));
  border-radius: var(--mat-stepper-header-focus-state-layer-shape, var(--mat-sys-corner-medium));
}
@media (hover: none) {
  .mat-step-header:hover {
    background: none;
  }
}
@media (forced-colors: active) {
  .mat-step-header {
    outline: solid 1px;
  }
  .mat-step-header[aria-selected=true] .mat-step-label {
    text-decoration: underline;
  }
  .mat-step-header[aria-disabled=true] {
    outline-color: GrayText;
  }
  .mat-step-header[aria-disabled=true] .mat-step-label,
  .mat-step-header[aria-disabled=true] .mat-step-icon,
  .mat-step-header[aria-disabled=true] .mat-step-optional {
    color: GrayText;
  }
}

.mat-step-optional {
  font-size: 12px;
  color: var(--mat-stepper-header-optional-label-text-color, var(--mat-sys-on-surface-variant));
}

.mat-step-sub-label-error {
  font-size: 12px;
  font-weight: normal;
}

.mat-step-icon {
  border-radius: 50%;
  height: 24px;
  width: 24px;
  flex-shrink: 0;
  position: relative;
  color: var(--mat-stepper-header-icon-foreground-color, var(--mat-sys-surface));
  background-color: var(--mat-stepper-header-icon-background-color, var(--mat-sys-on-surface-variant));
}

.mat-step-icon-content {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  display: flex;
}

.mat-step-icon .mat-icon {
  font-size: 16px;
  height: 16px;
  width: 16px;
}

.mat-step-icon-state-error {
  background-color: var(--mat-stepper-header-error-state-icon-background-color, transparent);
  color: var(--mat-stepper-header-error-state-icon-foreground-color, var(--mat-sys-error));
}
.mat-step-icon-state-error .mat-icon {
  font-size: 24px;
  height: 24px;
  width: 24px;
}

.mat-step-label {
  display: inline-block;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  min-width: 50px;
  vertical-align: middle;
  font-family: var(--mat-stepper-header-label-text-font, var(--mat-sys-title-small-font));
  font-size: var(--mat-stepper-header-label-text-size, var(--mat-sys-title-small-size));
  font-weight: var(--mat-stepper-header-label-text-weight, var(--mat-sys-title-small-weight));
  color: var(--mat-stepper-header-label-text-color, var(--mat-sys-on-surface-variant));
}
.mat-step-label.mat-step-label-active {
  color: var(--mat-stepper-header-selected-state-label-text-color, var(--mat-sys-on-surface-variant));
}
.mat-step-label.mat-step-label-error {
  color: var(--mat-stepper-header-error-state-label-text-color, var(--mat-sys-error));
  font-size: var(--mat-stepper-header-error-state-label-text-size, var(--mat-sys-title-small-size));
}
.mat-step-label.mat-step-label-selected {
  font-size: var(--mat-stepper-header-selected-state-label-text-size, var(--mat-sys-title-small-size));
  font-weight: var(--mat-stepper-header-selected-state-label-text-weight, var(--mat-sys-title-small-weight));
}
.mat-step-header-empty-label .mat-step-label {
  min-width: 0;
}

.mat-step-text-label {
  text-overflow: ellipsis;
  overflow: hidden;
}

.mat-step-header .mat-step-header-ripple {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  pointer-events: none;
}

.mat-step-icon-selected {
  background-color: var(--mat-stepper-header-selected-state-icon-background-color, var(--mat-sys-primary));
  color: var(--mat-stepper-header-selected-state-icon-foreground-color, var(--mat-sys-on-primary));
}

.mat-step-icon-state-done {
  background-color: var(--mat-stepper-header-done-state-icon-background-color, var(--mat-sys-primary));
  color: var(--mat-stepper-header-done-state-icon-foreground-color, var(--mat-sys-on-primary));
}

.mat-step-icon-state-edit {
  background-color: var(--mat-stepper-header-edit-state-icon-background-color, var(--mat-sys-primary));
  color: var(--mat-stepper-header-edit-state-icon-foreground-color, var(--mat-sys-on-primary));
}
`],encapsulation:2,changeDetection:0})}return t})(),Pi=(()=>{class t{templateRef=f(A);name;constructor(){}static \u0275fac=function(i){return new(i||t)};static \u0275dir=S({type:t,selectors:[["ng-template","matStepperIcon",""]],inputs:{name:[0,"matStepperIcon","name"]}})}return t})(),Ri=(()=>{class t{_template=f(A);constructor(){}static \u0275fac=function(i){return new(i||t)};static \u0275dir=S({type:t,selectors:[["ng-template","matStepContent",""]]})}return t})(),Ce=(()=>{class t extends me{_errorStateMatcher=f(ce,{skipSelf:!0});_viewContainerRef=f(Fe);_isSelected=Me.EMPTY;stepLabel=void 0;color;_lazyContent;_portal;ngAfterContentInit(){this._isSelected=this._stepper.steps.changes.pipe(Ee(()=>this._stepper.selectionChange.pipe(De(e=>e.selectedStep===this),w(this._stepper.selected===this)))).subscribe(e=>{e&&this._lazyContent&&!this._portal&&(this._portal=new ct(this._lazyContent._template,this._viewContainerRef))})}ngOnDestroy(){this._isSelected.unsubscribe()}isErrorState(e,i){let n=this._errorStateMatcher.isErrorState(e,i),l=!!(e&&e.invalid&&this.interacted);return n||l}static \u0275fac=(()=>{let e;return function(n){return(e||(e=j(t)))(n||t)}})();static \u0275cmp=E({type:t,selectors:[["mat-step"]],contentQueries:function(i,n,l){if(i&1&&V(l,ye,5)(l,Ri,5),i&2){let h;x(h=C())&&(n.stepLabel=h.first),x(h=C())&&(n._lazyContent=h.first)}},hostAttrs:["hidden",""],inputs:{color:"color"},exportAs:["matStep"],features:[fe([{provide:ce,useExisting:t},{provide:me,useExisting:t}]),T],ngContentSelectors:Jt,decls:1,vars:0,consts:[[3,"cdkPortalOutlet"]],template:function(i,n){i&1&&(W(),te(0,yi,2,1,"ng-template"))},dependencies:[mt],encapsulation:2,changeDetection:0})}return t})(),Se=(()=>{class t extends ${_ngZone=f(Te);_renderer=f(Oe);_animationsDisabled=Je();_cleanupTransition;_isAnimating=y(!1);_stepHeader=void 0;_animatedContainers;_steps=void 0;steps=new K;_icons;animationDone=new B;disableRipple=!1;color;labelPosition="end";headerPosition="top";ariaLabel=null;headerPrefix=He(null);_iconOverrides={};get animationDuration(){return this._animationDuration}set animationDuration(e){this._animationDuration=/^\d+$/.test(e)?e+"ms":e}_animationDuration="";_isServer=!f(Ge).isBrowser;constructor(){super();let i=f(U).nativeElement.nodeName.toLowerCase();this.orientation=i==="mat-vertical-stepper"?"vertical":"horizontal"}ngAfterContentInit(){super.ngAfterContentInit(),this._icons.forEach(({name:e,templateRef:i})=>this._iconOverrides[e]=i),this.steps.changes.pipe(D(this._destroyed)).subscribe(()=>this._stateChanged()),this.selectedIndexChange.pipe(D(this._destroyed)).subscribe(()=>{let e=this._getAnimationDuration();e==="0ms"||e==="0s"?this._onAnimationDone():this._isAnimating.set(!0)}),this._ngZone.runOutsideAngular(()=>{this._animationsDisabled||setTimeout(()=>{this._elementRef.nativeElement.classList.add("mat-stepper-animations-enabled"),this._cleanupTransition=this._renderer.listen(this._elementRef.nativeElement,"transitionend",this._handleTransitionend)},200)})}ngAfterViewInit(){if(super.ngAfterViewInit(),typeof queueMicrotask=="function"){let e=!1;this._animatedContainers.changes.pipe(w(null),D(this._destroyed)).subscribe(()=>queueMicrotask(()=>{e||(e=!0,this.animationDone.emit()),this._stateChanged()}))}}ngOnDestroy(){super.ngOnDestroy(),this._cleanupTransition?.()}_getAnimationDuration(){return this._animationsDisabled?"0ms":this.animationDuration?this.animationDuration:this.orientation==="horizontal"?"500ms":"225ms"}_handleTransitionend=e=>{let i=e.target;if(!i)return;let n=this.orientation==="horizontal"&&e.propertyName==="transform"&&i.classList.contains("mat-horizontal-stepper-content-current"),l=this.orientation==="vertical"&&e.propertyName==="grid-template-rows"&&i.classList.contains("mat-vertical-content-container-active");(n||l)&&this._animatedContainers.find(ue=>ue.nativeElement===i)&&this._onAnimationDone()};_onAnimationDone(){this._isAnimating.set(!1),this.animationDone.emit()}static \u0275fac=function(i){return new(i||t)};static \u0275cmp=E({type:t,selectors:[["mat-stepper"],["mat-vertical-stepper"],["mat-horizontal-stepper"],["","matStepper",""]],contentQueries:function(i,n,l){if(i&1&&V(l,Ce,5)(l,Pi,5),i&2){let h;x(h=C())&&(n._steps=h),x(h=C())&&(n._icons=h)}},viewQuery:function(i,n){if(i&1&&ae(xe,5)(xi,5),i&2){let l;x(l=C())&&(n._stepHeader=l),x(l=C())&&(n._animatedContainers=l)}},hostVars:14,hostBindings:function(i,n){i&2&&(Re("--mat-stepper-animation-duration",n._getAnimationDuration()),F("mat-stepper-horizontal",n.orientation==="horizontal")("mat-stepper-vertical",n.orientation==="vertical")("mat-stepper-label-position-end",n.orientation==="horizontal"&&n.labelPosition=="end")("mat-stepper-label-position-bottom",n.orientation==="horizontal"&&n.labelPosition=="bottom")("mat-stepper-header-position-bottom",n.headerPosition==="bottom")("mat-stepper-animating",n._isAnimating()))},inputs:{disableRipple:"disableRipple",color:"color",labelPosition:"labelPosition",headerPosition:"headerPosition",ariaLabel:[0,"aria-label","ariaLabel"],headerPrefix:[1,"headerPrefix"],animationDuration:"animationDuration"},outputs:{animationDone:"animationDone"},exportAs:["matStepper","matVerticalStepper","matHorizontalStepper"],features:[fe([{provide:$,useExisting:t}]),T],ngContentSelectors:Jt,decls:7,vars:2,consts:[["stepTemplate",""],["horizontalStepsTemplate",""],["animatedContainer",""],[1,"mat-horizontal-stepper-wrapper"],[1,"mat-vertical-stepper-wrapper"],[1,"mat-horizontal-stepper-header-wrapper"],[3,"ngTemplateOutlet","ngTemplateOutletContext"],[1,"mat-horizontal-content-container"],["role","tabpanel",1,"mat-horizontal-stepper-content",3,"id","class"],[3,"ngTemplateOutlet"],["role","tabpanel",1,"mat-horizontal-stepper-content",3,"id"],[1,"mat-step"],[1,"mat-vertical-content-container"],["role","region",1,"mat-vertical-stepper-content",3,"id"],[1,"mat-vertical-content"],[3,"click","keydown","tabIndex","id","index","state","label","selected","active","optional","errorMessage","iconOverrides","disableRipple","color"],["aria-orientation","horizontal","role","tablist",1,"mat-horizontal-stepper-header-container"],[1,"mat-stepper-horizontal-line"]],template:function(i,n){if(i&1&&(W(),v(0,Ci,1,0),v(1,Di,6,1,"div",3)(2,wi,4,1,"div",4),te(3,Ti,1,27,"ng-template",null,0,_e)(5,Fi,3,1,"ng-template",null,1,_e)),i&2){let l;b(n._isServer?0:-1),a(),b((l=n.orientation)==="horizontal"?1:l==="vertical"?2:-1)}},dependencies:[ge,xe],styles:[`.mat-stepper-vertical,
.mat-stepper-horizontal {
  display: block;
  font-family: var(--mat-stepper-container-text-font, var(--mat-sys-body-medium-font));
  background: var(--mat-stepper-container-color, var(--mat-sys-surface));
}

.mat-horizontal-stepper-header-wrapper {
  align-items: center;
  display: flex;
}

.mat-horizontal-stepper-header-container {
  white-space: nowrap;
  display: flex;
  align-items: center;
  flex-grow: 1;
}
.mat-stepper-label-position-bottom .mat-horizontal-stepper-header-container {
  align-items: flex-start;
}
.mat-stepper-header-position-bottom .mat-horizontal-stepper-header-container {
  order: 1;
}

.mat-stepper-horizontal-line {
  border-top-width: 1px;
  border-top-style: solid;
  flex: auto;
  height: 0;
  margin: 0 -16px;
  min-width: 32px;
  border-top-color: var(--mat-stepper-line-color, var(--mat-sys-outline));
}
.mat-stepper-label-position-bottom .mat-stepper-horizontal-line {
  margin: 0;
  min-width: 0;
  position: relative;
  top: calc(calc((var(--mat-stepper-header-height, 72px) - 24px) / 2) + 12px);
}

.mat-stepper-label-position-bottom .mat-horizontal-stepper-header:not(:first-child)::before, [dir=rtl] .mat-stepper-label-position-bottom .mat-horizontal-stepper-header:not(:last-child)::before, .mat-stepper-label-position-bottom .mat-horizontal-stepper-header:not(:last-child)::after, [dir=rtl] .mat-stepper-label-position-bottom .mat-horizontal-stepper-header:not(:first-child)::after {
  border-top-width: 1px;
  border-top-style: solid;
  content: "";
  display: inline-block;
  height: 0;
  position: absolute;
  width: calc(50% - 20px);
}

.mat-horizontal-stepper-header {
  display: flex;
  overflow: hidden;
  align-items: center;
  padding: 0 24px;
  height: var(--mat-stepper-header-height, 72px);
}
.mat-horizontal-stepper-header .mat-step-icon {
  margin-right: 8px;
  flex: none;
}
[dir=rtl] .mat-horizontal-stepper-header .mat-step-icon {
  margin-right: 0;
  margin-left: 8px;
}
.mat-horizontal-stepper-header.mat-step-header-empty-label .mat-step-icon {
  margin: 0;
}
.mat-horizontal-stepper-header::before, .mat-horizontal-stepper-header::after {
  border-top-color: var(--mat-stepper-line-color, var(--mat-sys-outline));
}
.mat-stepper-label-position-bottom .mat-horizontal-stepper-header {
  padding: calc((var(--mat-stepper-header-height, 72px) - 24px) / 2) 24px;
}
.mat-stepper-label-position-bottom .mat-horizontal-stepper-header::before, .mat-stepper-label-position-bottom .mat-horizontal-stepper-header::after {
  top: calc(calc((var(--mat-stepper-header-height, 72px) - 24px) / 2) + 12px);
}
.mat-stepper-label-position-bottom .mat-horizontal-stepper-header {
  box-sizing: border-box;
  flex-direction: column;
  height: auto;
}
.mat-stepper-label-position-bottom .mat-horizontal-stepper-header:not(:last-child)::after, [dir=rtl] .mat-stepper-label-position-bottom .mat-horizontal-stepper-header:not(:first-child)::after {
  right: 0;
}
.mat-stepper-label-position-bottom .mat-horizontal-stepper-header:not(:first-child)::before, [dir=rtl] .mat-stepper-label-position-bottom .mat-horizontal-stepper-header:not(:last-child)::before {
  left: 0;
}
[dir=rtl] .mat-stepper-label-position-bottom .mat-horizontal-stepper-header:last-child::before, [dir=rtl] .mat-stepper-label-position-bottom .mat-horizontal-stepper-header:first-child::after {
  display: none;
}
.mat-stepper-label-position-bottom .mat-horizontal-stepper-header .mat-step-icon {
  margin-right: 0;
  margin-left: 0;
}
.mat-stepper-label-position-bottom .mat-horizontal-stepper-header .mat-step-label {
  padding: 16px 0 0 0;
  text-align: center;
  width: 100%;
}

.mat-vertical-stepper-header {
  display: flex;
  align-items: center;
  height: 24px;
  padding: calc((var(--mat-stepper-header-height, 72px) - 24px) / 2) 24px;
}
.mat-vertical-stepper-header .mat-step-icon {
  margin-right: 12px;
}
[dir=rtl] .mat-vertical-stepper-header .mat-step-icon {
  margin-right: 0;
  margin-left: 12px;
}

.mat-horizontal-stepper-wrapper {
  display: flex;
  flex-direction: column;
}

.mat-horizontal-stepper-content {
  visibility: hidden;
  overflow: hidden;
  outline: 0;
  height: 0;
}
.mat-stepper-animations-enabled .mat-horizontal-stepper-content {
  transition: transform var(--mat-stepper-animation-duration, 0) cubic-bezier(0.35, 0, 0.25, 1);
}
.mat-horizontal-stepper-content.mat-horizontal-stepper-content-previous {
  transform: translate3d(-100%, 0, 0);
}
.mat-horizontal-stepper-content.mat-horizontal-stepper-content-next {
  transform: translate3d(100%, 0, 0);
}
.mat-horizontal-stepper-content.mat-horizontal-stepper-content-current {
  visibility: visible;
  transform: none;
  height: auto;
}
.mat-stepper-horizontal:not(.mat-stepper-animating) .mat-horizontal-stepper-content.mat-horizontal-stepper-content-current {
  overflow: visible;
}

.mat-horizontal-content-container {
  overflow: hidden;
  padding: 0 24px 24px 24px;
}
@media (forced-colors: active) {
  .mat-horizontal-content-container {
    outline: solid 1px;
  }
}
.mat-stepper-header-position-bottom .mat-horizontal-content-container {
  padding: 24px 24px 0 24px;
}

.mat-vertical-content-container {
  display: grid;
  grid-template-rows: 0fr;
  grid-template-columns: 100%;
  margin-left: 36px;
  border: 0;
  position: relative;
}
.mat-stepper-animations-enabled .mat-vertical-content-container {
  transition: grid-template-rows var(--mat-stepper-animation-duration, 0) cubic-bezier(0.4, 0, 0.2, 1);
}
.mat-vertical-content-container.mat-vertical-content-container-active {
  grid-template-rows: 1fr;
}
.mat-step:last-child .mat-vertical-content-container {
  border: none;
}
@media (forced-colors: active) {
  .mat-vertical-content-container {
    outline: solid 1px;
  }
}
[dir=rtl] .mat-vertical-content-container {
  margin-left: 0;
  margin-right: 36px;
}
@supports not (grid-template-rows: 0fr) {
  .mat-vertical-content-container {
    height: 0;
  }
  .mat-vertical-content-container.mat-vertical-content-container-active {
    height: auto;
  }
}

.mat-stepper-vertical-line::before {
  content: "";
  position: absolute;
  left: 0;
  border-left-width: 1px;
  border-left-style: solid;
  border-left-color: var(--mat-stepper-line-color, var(--mat-sys-outline));
  top: calc(8px - calc((var(--mat-stepper-header-height, 72px) - 24px) / 2));
  bottom: calc(8px - calc((var(--mat-stepper-header-height, 72px) - 24px) / 2));
}
[dir=rtl] .mat-stepper-vertical-line::before {
  left: auto;
  right: 0;
}

.mat-vertical-stepper-content {
  overflow: hidden;
  outline: 0;
  visibility: hidden;
}
.mat-stepper-animations-enabled .mat-vertical-stepper-content {
  transition: visibility var(--mat-stepper-animation-duration, 0) linear;
}
.mat-vertical-content-container-active > .mat-vertical-stepper-content {
  visibility: visible;
}

.mat-vertical-content {
  padding: 0 24px 24px 24px;
}
`],encapsulation:2,changeDetection:0})}return t})(),ti=(()=>{class t extends Wt{static \u0275fac=(()=>{let e;return function(n){return(e||(e=j(t)))(n||t)}})();static \u0275dir=S({type:t,selectors:[["button","matStepperNext",""]],hostAttrs:[1,"mat-stepper-next"],hostVars:1,hostBindings:function(i,n){i&2&&H("type",n.type)},features:[T]})}return t})(),ii=(()=>{class t extends Zt{static \u0275fac=(()=>{let e;return function(n){return(e||(e=j(t)))(n||t)}})();static \u0275dir=S({type:t,selectors:[["button","matStepperPrevious",""]],hostAttrs:[1,"mat-stepper-previous"],hostVars:1,hostBindings:function(i,n){i&2&&H("type",n.type)},features:[T]})}return t})(),ni=(()=>{class t{static \u0275fac=function(i){return new(i||t)};static \u0275mod=ee({type:t});static \u0275inj=X({providers:[ce],imports:[ut,Yt,de,nt,Se,xe,le]})}return t})();var Ai=(t,o)=>o.v;function Li(t,o){if(t&1&&(s(0,"mat-option",19),p(1),d(2,"t"),r()),t&2){let e=o.$implicit;m("value",e.v),a(),u(c(2,2,e.k))}}function Hi(t,o){if(t&1){let e=ne();s(0,"div",27)(1,"mat-form-field",10)(2,"mat-label"),p(3),d(4,"t"),r(),g(5,"input",12),r(),s(6,"mat-form-field",10)(7,"mat-label"),p(8),d(9,"t"),r(),g(10,"input",14),r(),s(11,"mat-form-field",10)(12,"mat-label"),p(13),d(14,"t"),r(),s(15,"mat-select",35),N(16,Li,3,4,"mat-option",19,Ai),r()(),s(18,"mat-form-field",10)(19,"mat-label"),p(20),d(21,"t"),r(),g(22,"input",36),r(),s(23,"mat-form-field",10)(24,"mat-label"),p(25),d(26,"t"),r(),g(27,"input",37),r(),s(28,"button",38),I("click",function(){let n=q(e).$index,l=_();return G(l.removeContact(n))}),s(29,"mat-icon"),p(30,"delete"),r()()()}if(t&2){let e=o.$index,i=_();m("formGroupName",e),a(3),u(c(4,7,"fullName.family")),a(5),z("",c(9,9,"fullName.given")," *"),a(5),u(c(14,11,"contact.relation")),a(3),O(i.relations),a(4),z("",c(21,13,"contact.phone")," *"),a(5),u(c(26,15,"auth.email")),a(3),m("disabled",i.contacts.length===1)}}function Vi(t,o){t&1&&(s(0,"mat-form-field",32)(1,"mat-label"),p(2),d(3,"t"),r(),g(4,"input",37),r()),t&2&&(a(2),u(c(3,1,"auth.email")))}var ai=class t{fb=f(It);api=f(jt);notify=f(Ut);router=f($e);relations=[{v:"Father",k:"contact.relation.Father"},{v:"Mother",k:"contact.relation.Mother"},{v:"Guardian",k:"contact.relation.Guardian"},{v:"Grandfather",k:"contact.relation.Grandfather"},{v:"Grandmother",k:"contact.relation.Grandmother"},{v:"Babysitter",k:"contact.relation.Babysitter"},{v:"Sibling",k:"contact.relation.Sibling"},{v:"Undefined",k:"contact.relation.Undefined"}];loading=y(!1);youthForm=this.fb.group({givenName:["",P.required],middleName:[""],familyName:[""],christName:[""],dateOfBirth:[null,P.required],isMale:[!0,P.required],address:[""],baptismDate:[null],baptismPlace:[""]});contactsForm=this.fb.group({contacts:this.fb.array([this.makeContact()])});accountForm=this.fb.group({hasAccount:[!1],email:[""]});contacts=this.contactsForm.get("contacts");addContact(){this.contacts.push(this.makeContact())}removeContact(o){this.contacts.length>1&&this.contacts.removeAt(o)}submit(){if(this.youthForm.invalid||this.contactsForm.invalid)return;let o=this.youthForm.getRawValue(),e={youth:{fullName:{givenName:o.givenName,middleName:o.middleName||null,familyName:o.familyName||null,christName:o.christName||null},dateOfBirth:this.toIsoDate(o.dateOfBirth),isMale:o.isMale,address:o.address||null,baptism:o.baptismDate?{date:this.toIsoDate(o.baptismDate),place:o.baptismPlace||null}:null},contacts:this.contacts.getRawValue(),accountEmail:this.accountForm.get("hasAccount")?.value?this.accountForm.get("email")?.value:null};this.loading.set(!0),this.api.post("/registration/youth",e).subscribe({next:()=>{this.loading.set(!1),this.notify.success("Registration submitted"),this.router.navigate(["/"])},error:()=>{this.loading.set(!1),this.notify.error("Submit failed")}})}makeContact(){return this.fb.group({givenName:["",P.required],familyName:[""],relation:["Father",P.required],phoneNumber:["",P.required],email:[""]})}toIsoDate(o){if(!o)return null;let e=o.getFullYear(),i=String(o.getMonth()+1).padStart(2,"0"),n=String(o.getDate()).padStart(2,"0");return`${e}-${i}-${n}`}static \u0275fac=function(e){return new(e||t)};static \u0275cmp=E({type:t,selectors:[["app-register"]],decls:125,vars:100,consts:[["dob",""],["bp",""],["color","primary"],[1,"title"],[1,"register-page"],[1,"register-card"],["linear",""],[3,"stepControl","label"],[1,"step-form",3,"formGroup"],[1,"grid-2"],["appearance","outline"],["matInput","","formControlName","christName"],["matInput","","formControlName","familyName"],["matInput","","formControlName","middleName"],["matInput","","formControlName","givenName","required",""],["matInput","","formControlName","dateOfBirth","required","",3,"matDatepicker"],["matIconSuffix","",3,"for"],[1,"field-gender"],["formControlName","isMale"],[3,"value"],["appearance","outline",1,"span-2"],["matInput","","formControlName","address"],["matInput","","formControlName","baptismDate",3,"matDatepicker"],["matInput","","formControlName","baptismPlace"],[1,"step-actions"],["mat-button","","matStepperNext","","type","button"],["formArrayName","contacts"],[1,"contact-row",3,"formGroupName"],["mat-stroked-button","","type","button",3,"click"],["mat-button","","matStepperPrevious","","type","button"],[3,"label"],["formControlName","hasAccount"],["appearance","outline",1,"full-width","mt-16"],[1,"step-form"],["mat-flat-button","","color","primary","type","button",3,"click","disabled"],["formControlName","relation"],["matInput","","formControlName","phoneNumber","required",""],["matInput","","type","email","formControlName","email"],["mat-icon-button","","type","button","color","warn",3,"click","disabled"]],template:function(e,i){if(e&1&&(s(0,"mat-toolbar",2)(1,"mat-icon"),p(2,"person_add"),r(),s(3,"span",3),p(4),d(5,"t"),r()(),s(6,"div",4)(7,"mat-card",5)(8,"mat-card-content")(9,"mat-stepper",6)(10,"mat-step",7),d(11,"t"),s(12,"form",8)(13,"div",9)(14,"mat-form-field",10)(15,"mat-label"),p(16),d(17,"t"),r(),g(18,"input",11),r(),s(19,"mat-form-field",10)(20,"mat-label"),p(21),d(22,"t"),r(),g(23,"input",12),r(),s(24,"mat-form-field",10)(25,"mat-label"),p(26),d(27,"t"),r(),g(28,"input",13),r(),s(29,"mat-form-field",10)(30,"mat-label"),p(31),d(32,"t"),r(),g(33,"input",14),r(),s(34,"mat-form-field",10)(35,"mat-label"),p(36),d(37,"t"),r(),g(38,"input",15)(39,"mat-datepicker-toggle",16)(40,"mat-datepicker",null,0),r(),s(42,"div",17)(43,"label"),p(44),d(45,"t"),r(),s(46,"mat-radio-group",18)(47,"mat-radio-button",19),p(48),d(49,"t"),r(),s(50,"mat-radio-button",19),p(51),d(52,"t"),r()()(),s(53,"mat-form-field",20)(54,"mat-label"),p(55),d(56,"t"),r(),g(57,"input",21),r(),s(58,"mat-form-field",10)(59,"mat-label"),p(60),d(61,"t"),r(),g(62,"input",22)(63,"mat-datepicker-toggle",16)(64,"mat-datepicker",null,1),r(),s(66,"mat-form-field",10)(67,"mat-label"),p(68),d(69,"t"),r(),g(70,"input",23),r()(),s(71,"div",24)(72,"button",25),p(73),d(74,"t"),r()()()(),s(75,"mat-step",7),d(76,"t"),s(77,"form",8)(78,"div",26),N(79,Hi,31,17,"div",27,Pe),r(),s(81,"button",28),I("click",function(){return i.addContact()}),s(82,"mat-icon"),p(83,"add"),r(),p(84),d(85,"t"),r(),s(86,"div",24)(87,"button",29),p(88),d(89,"t"),r(),s(90,"button",25),p(91),d(92,"t"),r()()()(),s(93,"mat-step",30),d(94,"t"),s(95,"form",8)(96,"mat-checkbox",31),p(97),d(98,"t"),r(),v(99,Vi,5,3,"mat-form-field",32),s(100,"div",24)(101,"button",29),p(102),d(103,"t"),r(),s(104,"button",25),p(105),d(106,"t"),r()()()(),s(107,"mat-step",30),d(108,"t"),s(109,"div",33)(110,"p"),p(111),r(),s(112,"p"),p(113),d(114,"t"),r(),s(115,"p"),p(116),d(117,"t"),r(),s(118,"div",24)(119,"button",29),p(120),d(121,"t"),r(),s(122,"button",34),I("click",function(){return i.submit()}),p(123),d(124,"t"),r()()()()()()()()),e&2){let n,l=k(41),h=k(65);a(4),u(c(5,46,"register.title")),a(6),m("stepControl",i.youthForm)("label",c(11,48,"register.step.youth")),a(2),m("formGroup",i.youthForm),a(4),u(c(17,50,"fullName.christ")),a(5),u(c(22,52,"fullName.family")),a(5),u(c(27,54,"fullName.middle")),a(5),z("",c(32,56,"fullName.given")," *"),a(5),z("",c(37,58,"youth.dateOfBirth")," *"),a(2),m("matDatepicker",l),a(),m("for",l),a(5),u(c(45,60,"youth.gender")),a(3),m("value",!0),a(),u(c(49,62,"common.male")),a(2),m("value",!1),a(),u(c(52,64,"common.female")),a(4),u(c(56,66,"youth.address")),a(5),u(c(61,68,"register.baptismDate")),a(2),m("matDatepicker",h),a(),m("for",h),a(5),u(c(69,70,"milestone.place")),a(5),u(c(74,72,"common.next")),a(2),m("stepControl",i.contactsForm)("label",c(76,74,"register.step.contacts")),a(2),m("formGroup",i.contactsForm),a(2),O(i.contacts.controls),a(5),z(" ",c(85,76,"common.add")," "),a(4),u(c(89,78,"common.back")),a(3),u(c(92,80,"common.next")),a(2),m("label",c(94,82,"register.step.account")),a(2),m("formGroup",i.accountForm),a(2),u(c(98,84,"register.hasAccount")),a(2),b((n=i.accountForm.get("hasAccount"))!=null&&n.value?99:-1),a(3),u(c(103,86,"common.back")),a(3),u(c(106,88,"common.next")),a(2),m("label",c(108,90,"register.step.review")),a(4),Be("",i.youthForm.value.christName," ",i.youthForm.value.familyName," ",i.youthForm.value.middleName," ",i.youthForm.value.givenName),a(2),he("",c(114,92,"youth.dateOfBirth"),": ",i.youthForm.value.dateOfBirth),a(3),he("",c(117,94,"youth.contacts"),": ",i.contacts.length),a(4),u(c(121,96,"common.back")),a(2),m("disabled",i.loading()),a(),z(" ",c(124,98,"common.submit")," ")}},dependencies:[Qe,Dt,bt,ft,gt,vt,Mt,St,Ct,yt,xt,dt,lt,pt,ni,Ce,Se,ti,ii,Tt,wt,Et,kt,Ot,Nt,Pt,zt,Ft,At,Rt,Bt,rt,at,tt,de,pe,Qt,Lt,Ht,Vt,$t,Gt,qt,st,ot,ht],styles:[".title[_ngcontent-%COMP%]{margin-left:8px;font-weight:500}.register-page[_ngcontent-%COMP%]{display:flex;justify-content:center;padding:24px 16px;background:var(--mat-sys-surface-container);min-height:calc(100vh - 64px)}.register-card[_ngcontent-%COMP%]{max-width:900px;width:100%}.step-form[_ngcontent-%COMP%]{padding:16px 0}.grid-2[_ngcontent-%COMP%]{display:grid;grid-template-columns:repeat(2,1fr);gap:12px}.grid-2[_ngcontent-%COMP%]   .span-2[_ngcontent-%COMP%]{grid-column:1/-1}.field-gender[_ngcontent-%COMP%]{display:flex;flex-direction:column;gap:8px;justify-content:center}.field-gender[_ngcontent-%COMP%]   mat-radio-group[_ngcontent-%COMP%]{display:flex;gap:16px}.contact-row[_ngcontent-%COMP%]{display:grid;grid-template-columns:1fr 1fr 1fr 1fr 1.2fr auto;gap:8px;align-items:center;margin-bottom:8px}.step-actions[_ngcontent-%COMP%]{display:flex;justify-content:flex-end;gap:8px;margin-top:16px}.full-width[_ngcontent-%COMP%]{width:100%}.mt-16[_ngcontent-%COMP%]{margin-top:16px}@media(max-width:720px){.grid-2[_ngcontent-%COMP%], .contact-row[_ngcontent-%COMP%]{grid-template-columns:1fr}}"]})};export{ai as RegisterPage};
