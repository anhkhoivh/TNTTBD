import{a as mt}from"./chunk-XVQDM2QA.js";import{a as Ke,b as $e,c as Ye,d as Je,e as nt}from"./chunk-UJ2UGYIG.js";import{a as We,b as Ze}from"./chunk-2UQS5AU6.js";import"./chunk-L4BKUYED.js";import"./chunk-R3HFD3F4.js";import{a as ot,c as st,e as dt,f as lt,g as ct}from"./chunk-XLVFNDKB.js";import{a as rt,b as it}from"./chunk-ZBKGOEJE.js";import{a as Fe}from"./chunk-MOYO6C6C.js";import"./chunk-5AL2SDHA.js";import{a as Ie}from"./chunk-WOPUPVC7.js";import{a as Ue,b as j,c as qe,e as fe}from"./chunk-4MZPLR3U.js";import{a as at}from"./chunk-HOV3TQSL.js";import"./chunk-QHBGS32Z.js";import"./chunk-JMZ5XG7L.js";import"./chunk-PTFPPLIF.js";import"./chunk-TBZDRLNV.js";import{a as Ge,b as Qe,c as tt}from"./chunk-FOIS2F75.js";import{b as Ee,d as Te,e as Be,h as Le,i as Xe,j as He}from"./chunk-CSGIYDIV.js";import{$ as je,F as Ae,I as Re,J as Oe,N as ze,Z as he,_ as Ve,ba as S,f as Se,ia as Ne,ja as ee,la as et,v as J,w as ue,y as Pe}from"./chunk-UTAGKRPA.js";import{$ as W,$b as ce,Aa as U,Ab as _,B as we,Bb as g,C as N,Ca as I,Da as ke,Eb as xe,Fb as Me,G as ye,Gb as G,H as Ce,Hb as a,I as ie,Ib as o,Jb as P,Kb as $,Lb as Y,Mb as Q,Mc as V,Qb as A,Qc as pe,S as ae,U as f,Vb as u,Xa as d,Xb as w,Yb as k,Zb as h,_b as le,ab as de,ac as x,ba as X,bc as M,da as i,db as q,fc as me,gc as D,h as y,hc as b,ia as E,ic as De,ja as T,jc as c,kb as p,kc as v,lb as K,ma as oe,na as se,pb as H,qa as B,ra as F,t as L,tc as R,va as Z,yc as O,zb as C,zc as z}from"./chunk-WJUDORRU.js";var re=["*"],wt=["content"],yt=[[["mat-drawer"]],[["mat-drawer-content"]],"*"],Ct=["mat-drawer","mat-drawer-content","*"];function kt(r,s){if(r&1){let e=A();a(0,"div",1),u("click",function(){E(e);let n=w();return T(n._onBackdropClicked())}),o()}if(r&2){let e=w();b("mat-drawer-shown",e._isShowingBackdrop())}}function xt(r,s){r&1&&(a(0,"mat-drawer-content"),h(1,2),o())}var Mt=[[["mat-sidenav"]],[["mat-sidenav-content"]],"*"],Dt=["mat-sidenav","mat-sidenav-content","*"];function St(r,s){if(r&1){let e=A();a(0,"div",1),u("click",function(){E(e);let n=w();return T(n._onBackdropClicked())}),o()}if(r&2){let e=w();b("mat-drawer-shown",e._isShowingBackdrop())}}function Et(r,s){r&1&&(a(0,"mat-sidenav-content"),h(1,2),o())}var Tt=`.mat-drawer-container {
  position: relative;
  z-index: 1;
  color: var(--mat-sidenav-content-text-color, var(--mat-sys-on-background));
  background-color: var(--mat-sidenav-content-background-color, var(--mat-sys-background));
  box-sizing: border-box;
  display: block;
  overflow: hidden;
}
.mat-drawer-container[fullscreen] {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
}
.mat-drawer-container[fullscreen].mat-drawer-container-has-open {
  overflow: hidden;
}
.mat-drawer-container.mat-drawer-container-explicit-backdrop .mat-drawer-side {
  z-index: 3;
}
.mat-drawer-container.ng-animate-disabled .mat-drawer-backdrop,
.mat-drawer-container.ng-animate-disabled .mat-drawer-content, .ng-animate-disabled .mat-drawer-container .mat-drawer-backdrop,
.ng-animate-disabled .mat-drawer-container .mat-drawer-content {
  transition: none;
}

.mat-drawer-backdrop {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  display: block;
  z-index: 3;
  visibility: hidden;
}
.mat-drawer-backdrop.mat-drawer-shown {
  visibility: visible;
  background-color: var(--mat-sidenav-scrim-color, color-mix(in srgb, var(--mat-sys-neutral-variant20) 40%, transparent));
}
.mat-drawer-transition .mat-drawer-backdrop {
  transition-duration: 400ms;
  transition-timing-function: cubic-bezier(0.25, 0.8, 0.25, 1);
  transition-property: background-color, visibility;
}
@media (forced-colors: active) {
  .mat-drawer-backdrop {
    opacity: 0.5;
  }
}

.mat-drawer-content {
  position: relative;
  z-index: 1;
  display: block;
  height: 100%;
  overflow: auto;
}
.mat-drawer-content.mat-drawer-content-hidden {
  opacity: 0;
}
.mat-drawer-transition .mat-drawer-content {
  transition-duration: 400ms;
  transition-timing-function: cubic-bezier(0.25, 0.8, 0.25, 1);
  transition-property: transform, margin-left, margin-right;
}

.mat-drawer {
  position: relative;
  z-index: 4;
  color: var(--mat-sidenav-container-text-color, var(--mat-sys-on-surface-variant));
  box-shadow: var(--mat-sidenav-container-elevation-shadow, none);
  background-color: var(--mat-sidenav-container-background-color, var(--mat-sys-surface));
  border-top-right-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-bottom-right-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  width: var(--mat-sidenav-container-width, 360px);
  display: block;
  position: absolute;
  top: 0;
  bottom: 0;
  z-index: 3;
  outline: 0;
  box-sizing: border-box;
  overflow-y: auto;
  transform: translate3d(-100%, 0, 0);
}
@media (forced-colors: active) {
  .mat-drawer, [dir=rtl] .mat-drawer.mat-drawer-end {
    border-right: solid 1px currentColor;
  }
}
@media (forced-colors: active) {
  [dir=rtl] .mat-drawer, .mat-drawer.mat-drawer-end {
    border-left: solid 1px currentColor;
    border-right: none;
  }
}
.mat-drawer.mat-drawer-side {
  z-index: 2;
}
.mat-drawer.mat-drawer-end {
  right: 0;
  transform: translate3d(100%, 0, 0);
  border-top-left-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-bottom-left-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
}
[dir=rtl] .mat-drawer {
  border-top-left-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-bottom-left-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
  transform: translate3d(100%, 0, 0);
}
[dir=rtl] .mat-drawer.mat-drawer-end {
  border-top-right-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-bottom-right-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-top-left-radius: 0;
  border-bottom-left-radius: 0;
  left: 0;
  right: auto;
  transform: translate3d(-100%, 0, 0);
}
.mat-drawer-transition .mat-drawer {
  transition: transform 400ms cubic-bezier(0.25, 0.8, 0.25, 1);
}
.mat-drawer:not(.mat-drawer-opened):not(.mat-drawer-animating) {
  visibility: hidden;
  box-shadow: none;
}
.mat-drawer:not(.mat-drawer-opened):not(.mat-drawer-animating) .mat-drawer-inner-container {
  display: none;
}
.mat-drawer.mat-drawer-opened.mat-drawer-opened {
  transform: none;
}

.mat-drawer-side {
  box-shadow: none;
  border-right-color: var(--mat-sidenav-container-divider-color, transparent);
  border-right-width: 1px;
  border-right-style: solid;
}
.mat-drawer-side.mat-drawer-end {
  border-left-color: var(--mat-sidenav-container-divider-color, transparent);
  border-left-width: 1px;
  border-left-style: solid;
  border-right: none;
}
[dir=rtl] .mat-drawer-side {
  border-left-color: var(--mat-sidenav-container-divider-color, transparent);
  border-left-width: 1px;
  border-left-style: solid;
  border-right: none;
}
[dir=rtl] .mat-drawer-side.mat-drawer-end {
  border-right-color: var(--mat-sidenav-container-divider-color, transparent);
  border-right-width: 1px;
  border-right-style: solid;
  border-left: none;
}

.mat-drawer-inner-container {
  width: 100%;
  height: 100%;
  overflow: auto;
}

.mat-sidenav-fixed {
  position: fixed;
}
`;var Bt=new X("MAT_DRAWER_DEFAULT_AUTOSIZE",{providedIn:"root",factory:()=>!1}),be=new X("MAT_DRAWER_CONTAINER"),te=(()=>{class r extends j{_platform=i(J);_changeDetectorRef=i(V);_container=i(ge);constructor(){let e=i(I),t=i(Ue),n=i(F);super(e,t,n)}ngAfterContentInit(){this._container._contentMarginChanges.subscribe(()=>{this._changeDetectorRef.markForCheck()})}_shouldBeHidden(){if(this._platform.isBrowser)return!1;let{start:e,end:t}=this._container;return e!=null&&e.mode!=="over"&&e.opened||t!=null&&t.mode!=="over"&&t.opened}static \u0275fac=function(t){return new(t||r)};static \u0275cmp=p({type:r,selectors:[["mat-drawer-content"]],hostAttrs:[1,"mat-drawer-content"],hostVars:6,hostBindings:function(t,n){t&2&&(D("margin-left",n._container._contentMargins.left,"px")("margin-right",n._container._contentMargins.right,"px"),b("mat-drawer-content-hidden",n._shouldBeHidden()))},features:[R([{provide:j,useExisting:r}]),H],ngContentSelectors:re,decls:1,vars:0,template:function(t,n){t&1&&(k(),h(0))},encapsulation:2,changeDetection:0})}return r})(),_e=(()=>{class r{_elementRef=i(I);_focusTrapFactory=i(Oe);_focusMonitor=i(Pe);_platform=i(J);_ngZone=i(F);_renderer=i(q);_interactivityChecker=i(Re);_doc=i(se);_container=i(be,{optional:!0});_focusTrap=null;_elementFocusedBeforeDrawerWasOpened=null;_eventCleanups;_isAttached=!1;_anchor=null;get position(){return this._position}set position(e){e=e==="end"?"end":"start",e!==this._position&&(this._isAttached&&this._updatePositionInParent(e),this._position=e,this.onPositionChanged.emit())}_position="start";get mode(){return this._mode}set mode(e){this._mode=e,this._updateFocusTrapState(),this._modeChanged.next()}_mode="over";get disableClose(){return this._disableClose}set disableClose(e){this._disableClose=S(e)}_disableClose=!1;get autoFocus(){let e=this._autoFocus;return e??(this.mode==="side"?"dialog":"first-tabbable")}set autoFocus(e){(e==="true"||e==="false"||e==null)&&(e=S(e)),this._autoFocus=e}_autoFocus;get opened(){return this._opened()}set opened(e){this.toggle(S(e))}_opened=Z(!1);_openedVia=null;_animationStarted=new y;_animationEnd=new y;openedChange=new B(!0);_openedStream=this.openedChange.pipe(N(e=>e),L(()=>{}));openedStart=this._animationStarted.pipe(N(()=>this.opened),ie(void 0));_closedStream=this.openedChange.pipe(N(e=>!e),L(()=>{}));closedStart=this._animationStarted.pipe(N(()=>!this.opened),ie(void 0));_destroyed=new y;onPositionChanged=new B;_content;_modeChanged=new y;_injector=i(oe);_changeDetectorRef=i(V);constructor(){this.openedChange.pipe(f(this._destroyed)).subscribe(e=>{e?(this._elementFocusedBeforeDrawerWasOpened=this._doc.activeElement,this._takeFocus()):this._isFocusWithinDrawer()&&this._restoreFocus(this._openedVia||"program")}),this._eventCleanups=this._ngZone.runOutsideAngular(()=>{let e=this._renderer,t=this._elementRef.nativeElement;return[e.listen(t,"keydown",n=>{n.keyCode===27&&!this.disableClose&&!ze(n)&&this._ngZone.run(()=>{this.close(),n.stopPropagation(),n.preventDefault()})}),e.listen(t,"transitionend",this._handleTransitionEvent),e.listen(t,"transitioncancel",this._handleTransitionEvent)]}),this._animationEnd.subscribe(()=>{this.openedChange.emit(this.opened)})}_forceFocus(e,t){this._interactivityChecker.isFocusable(e)||(e.tabIndex=-1,this._ngZone.runOutsideAngular(()=>{let n=()=>{l(),m(),e.removeAttribute("tabindex")},l=this._renderer.listen(e,"blur",n),m=this._renderer.listen(e,"mousedown",n)})),e.focus(t)}_focusByCssSelector(e,t){let n=this._elementRef.nativeElement.querySelector(e);n&&this._forceFocus(n,t)}_takeFocus(){if(!this._focusTrap)return;let e=this._elementRef.nativeElement;switch(this.autoFocus){case!1:case"dialog":return;case!0:case"first-tabbable":de(()=>{!this._focusTrap.focusInitialElement()&&typeof e.focus=="function"&&e.focus()},{injector:this._injector});break;case"first-heading":this._focusByCssSelector('h1, h2, h3, h4, h5, h6, [role="heading"]');break;default:this._focusByCssSelector(this.autoFocus);break}}_restoreFocus(e){this.autoFocus!=="dialog"&&(this._elementFocusedBeforeDrawerWasOpened?this._focusMonitor.focusVia(this._elementFocusedBeforeDrawerWasOpened,e):this._elementRef.nativeElement.blur(),this._elementFocusedBeforeDrawerWasOpened=null)}_isFocusWithinDrawer(){let e=this._doc.activeElement;return!!e&&this._elementRef.nativeElement.contains(e)}ngAfterViewInit(){this._isAttached=!0,this._position==="end"&&this._updatePositionInParent("end"),this._platform.isBrowser&&(this._focusTrap=this._focusTrapFactory.create(this._elementRef.nativeElement),this._updateFocusTrapState())}ngOnDestroy(){this._eventCleanups.forEach(e=>e()),this._focusTrap?.destroy(),this._anchor?.remove(),this._anchor=null,this._animationStarted.complete(),this._animationEnd.complete(),this._modeChanged.complete(),this._destroyed.next(),this._destroyed.complete()}open(e){return this.toggle(!0,e)}close(){return this.toggle(!1)}_closeViaBackdropClick(){return this._setOpen(!1,!0,"mouse")}toggle(e=!this.opened,t){e&&t&&(this._openedVia=t);let n=this._setOpen(e,!e&&this._isFocusWithinDrawer(),this._openedVia||"program");return e||(this._openedVia=null),n}_setOpen(e,t,n){return e===this.opened?Promise.resolve(e?"open":"close"):(this._opened.set(e),this._container?._transitionsEnabled?(this._setIsAnimating(!0),setTimeout(()=>this._animationStarted.next())):setTimeout(()=>{this._animationStarted.next(),this._animationEnd.next()}),this._elementRef.nativeElement.classList.toggle("mat-drawer-opened",e),!e&&t&&this._restoreFocus(n),this._changeDetectorRef.markForCheck(),this._updateFocusTrapState(),new Promise(l=>{this.openedChange.pipe(Ce(1)).subscribe(m=>l(m?"open":"close"))}))}_setIsAnimating(e){this._elementRef.nativeElement.classList.toggle("mat-drawer-animating",e)}_getWidth(){return this._elementRef.nativeElement.offsetWidth||0}_updateFocusTrapState(){this._focusTrap&&(this._focusTrap.enabled=this.opened&&!!this._container?._isShowingBackdrop())}_updatePositionInParent(e){if(!this._platform.isBrowser)return;let t=this._elementRef.nativeElement,n=t.parentNode;e==="end"?(this._anchor||(this._anchor=this._doc.createComment("mat-drawer-anchor"),n.insertBefore(this._anchor,t)),n.appendChild(t)):this._anchor&&this._anchor.parentNode.insertBefore(t,this._anchor)}_handleTransitionEvent=e=>{let t=this._elementRef.nativeElement;e.target===t&&this._ngZone.run(()=>{e.type==="transitionend"&&this._setIsAnimating(!1),this._animationEnd.next(e)})};static \u0275fac=function(t){return new(t||r)};static \u0275cmp=p({type:r,selectors:[["mat-drawer"]],viewQuery:function(t,n){if(t&1&&ce(wt,5),t&2){let l;x(l=M())&&(n._content=l.first)}},hostAttrs:[1,"mat-drawer"],hostVars:12,hostBindings:function(t,n){t&2&&(C("align",null)("tabIndex",n.mode!=="side"?"-1":null),D("visibility",!n._container&&!n.opened?"hidden":null),b("mat-drawer-end",n.position==="end")("mat-drawer-over",n.mode==="over")("mat-drawer-push",n.mode==="push")("mat-drawer-side",n.mode==="side"))},inputs:{position:"position",mode:"mode",disableClose:"disableClose",autoFocus:"autoFocus",opened:"opened"},outputs:{openedChange:"openedChange",_openedStream:"opened",openedStart:"openedStart",_closedStream:"closed",closedStart:"closedStart",onPositionChanged:"positionChanged"},exportAs:["matDrawer"],ngContentSelectors:re,decls:3,vars:0,consts:[["content",""],["cdkScrollable","",1,"mat-drawer-inner-container"]],template:function(t,n){t&1&&(k(),a(0,"div",1,0),h(2),o())},dependencies:[j],encapsulation:2,changeDetection:0})}return r})(),ge=(()=>{class r{_dir=i(Ne,{optional:!0});_element=i(I);_ngZone=i(F);_changeDetectorRef=i(V);_animationDisabled=je();_transitionsEnabled=!1;_allDrawers;_drawers=new ke;_content;_userContent;get start(){return this._start}get end(){return this._end}get autosize(){return this._autosize}set autosize(e){this._autosize=S(e)}_autosize=i(Bt);get hasBackdrop(){return this._drawerHasBackdrop(this._start)||this._drawerHasBackdrop(this._end)}set hasBackdrop(e){this._backdropOverride=e==null?null:S(e)}_backdropOverride=null;backdropClick=new B;_start=null;_end=null;_left=null;_right=null;_destroyed=new y;_doCheckSubject=new y;_contentMargins={left:null,right:null};_contentMarginChanges=new y;get scrollable(){return this._userContent||this._content}_injector=i(oe);constructor(){let e=i(J),t=i(qe);this._dir?.change.pipe(f(this._destroyed)).subscribe(()=>{this._validateDrawers(),this.updateContentMargins()}),t.change().pipe(f(this._destroyed)).subscribe(()=>this.updateContentMargins()),!this._animationDisabled&&e.isBrowser&&this._ngZone.runOutsideAngular(()=>{setTimeout(()=>{this._element.nativeElement.classList.add("mat-drawer-transition"),this._transitionsEnabled=!0},200)})}ngAfterContentInit(){this._allDrawers.changes.pipe(ae(this._allDrawers),f(this._destroyed)).subscribe(e=>{this._drawers.reset(e.filter(t=>!t._container||t._container===this)),this._drawers.notifyOnChanges()}),this._drawers.changes.pipe(ae(null)).subscribe(()=>{this._validateDrawers(),this._drawers.forEach(e=>{this._watchDrawerToggle(e),this._watchDrawerPosition(e),this._watchDrawerMode(e)}),(!this._drawers.length||this._isDrawerOpen(this._start)||this._isDrawerOpen(this._end))&&this.updateContentMargins(),this._changeDetectorRef.markForCheck()}),this._ngZone.runOutsideAngular(()=>{this._doCheckSubject.pipe(ye(10),f(this._destroyed)).subscribe(()=>this.updateContentMargins())})}ngOnDestroy(){this._contentMarginChanges.complete(),this._doCheckSubject.complete(),this._drawers.destroy(),this._destroyed.next(),this._destroyed.complete()}open(){this._drawers.forEach(e=>e.open())}close(){this._drawers.forEach(e=>e.close())}updateContentMargins(){let e=0,t=0;if(this._left&&this._left.opened){if(this._left.mode=="side")e+=this._left._getWidth();else if(this._left.mode=="push"){let n=this._left._getWidth();e+=n,t-=n}}if(this._right&&this._right.opened){if(this._right.mode=="side")t+=this._right._getWidth();else if(this._right.mode=="push"){let n=this._right._getWidth();t+=n,e-=n}}e=e||null,t=t||null,(e!==this._contentMargins.left||t!==this._contentMargins.right)&&(this._contentMargins={left:e,right:t},this._ngZone.run(()=>this._contentMarginChanges.next(this._contentMargins)))}ngDoCheck(){this._autosize&&this._isPushed()&&this._ngZone.runOutsideAngular(()=>this._doCheckSubject.next())}_watchDrawerToggle(e){e._animationStarted.pipe(f(this._drawers.changes)).subscribe(()=>{this.updateContentMargins(),this._changeDetectorRef.markForCheck()}),e.mode!=="side"&&e.openedChange.pipe(f(this._drawers.changes)).subscribe(()=>this._setContainerClass(e.opened))}_watchDrawerPosition(e){e.onPositionChanged.pipe(f(this._drawers.changes)).subscribe(()=>{de({read:()=>this._validateDrawers()},{injector:this._injector})})}_watchDrawerMode(e){e._modeChanged.pipe(f(we(this._drawers.changes,this._destroyed))).subscribe(()=>{this.updateContentMargins(),this._changeDetectorRef.markForCheck()})}_setContainerClass(e){let t=this._element.nativeElement.classList,n="mat-drawer-container-has-open";e?t.add(n):t.remove(n)}_validateDrawers(){this._start=this._end=null,this._drawers.forEach(e=>{e.position=="end"?(this._end!=null,this._end=e):(this._start!=null,this._start=e)}),this._right=this._left=null,this._dir&&this._dir.value==="rtl"?(this._left=this._end,this._right=this._start):(this._left=this._start,this._right=this._end)}_isPushed(){return this._isDrawerOpen(this._start)&&this._start.mode!="over"||this._isDrawerOpen(this._end)&&this._end.mode!="over"}_onBackdropClicked(){this.backdropClick.emit(),this._closeModalDrawersViaBackdrop()}_closeModalDrawersViaBackdrop(){[this._start,this._end].filter(e=>e&&!e.disableClose&&this._drawerHasBackdrop(e)).forEach(e=>e._closeViaBackdropClick())}_isShowingBackdrop(){return this._isDrawerOpen(this._start)&&this._drawerHasBackdrop(this._start)||this._isDrawerOpen(this._end)&&this._drawerHasBackdrop(this._end)}_isDrawerOpen(e){return e!=null&&e.opened}_drawerHasBackdrop(e){return this._backdropOverride==null?!!e&&e.mode!=="side":this._backdropOverride}static \u0275fac=function(t){return new(t||r)};static \u0275cmp=p({type:r,selectors:[["mat-drawer-container"]],contentQueries:function(t,n,l){if(t&1&&le(l,te,5)(l,_e,5),t&2){let m;x(m=M())&&(n._content=m.first),x(m=M())&&(n._allDrawers=m)}},viewQuery:function(t,n){if(t&1&&ce(te,5),t&2){let l;x(l=M())&&(n._userContent=l.first)}},hostAttrs:[1,"mat-drawer-container"],hostVars:2,hostBindings:function(t,n){t&2&&b("mat-drawer-container-explicit-backdrop",n._backdropOverride)},inputs:{autosize:"autosize",hasBackdrop:"hasBackdrop"},outputs:{backdropClick:"backdropClick"},exportAs:["matDrawerContainer"],features:[R([{provide:be,useExisting:r}])],ngContentSelectors:Ct,decls:4,vars:2,consts:[[1,"mat-drawer-backdrop",3,"mat-drawer-shown"],[1,"mat-drawer-backdrop",3,"click"]],template:function(t,n){t&1&&(k(yt),_(0,kt,1,2,"div",0),h(1),h(2,1),_(3,xt,2,0,"mat-drawer-content")),t&2&&(g(n.hasBackdrop?0:-1),d(3),g(n._content?-1:3))},dependencies:[te],styles:[`.mat-drawer-container {
  position: relative;
  z-index: 1;
  color: var(--mat-sidenav-content-text-color, var(--mat-sys-on-background));
  background-color: var(--mat-sidenav-content-background-color, var(--mat-sys-background));
  box-sizing: border-box;
  display: block;
  overflow: hidden;
}
.mat-drawer-container[fullscreen] {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
}
.mat-drawer-container[fullscreen].mat-drawer-container-has-open {
  overflow: hidden;
}
.mat-drawer-container.mat-drawer-container-explicit-backdrop .mat-drawer-side {
  z-index: 3;
}
.mat-drawer-container.ng-animate-disabled .mat-drawer-backdrop,
.mat-drawer-container.ng-animate-disabled .mat-drawer-content, .ng-animate-disabled .mat-drawer-container .mat-drawer-backdrop,
.ng-animate-disabled .mat-drawer-container .mat-drawer-content {
  transition: none;
}

.mat-drawer-backdrop {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  display: block;
  z-index: 3;
  visibility: hidden;
}
.mat-drawer-backdrop.mat-drawer-shown {
  visibility: visible;
  background-color: var(--mat-sidenav-scrim-color, color-mix(in srgb, var(--mat-sys-neutral-variant20) 40%, transparent));
}
.mat-drawer-transition .mat-drawer-backdrop {
  transition-duration: 400ms;
  transition-timing-function: cubic-bezier(0.25, 0.8, 0.25, 1);
  transition-property: background-color, visibility;
}
@media (forced-colors: active) {
  .mat-drawer-backdrop {
    opacity: 0.5;
  }
}

.mat-drawer-content {
  position: relative;
  z-index: 1;
  display: block;
  height: 100%;
  overflow: auto;
}
.mat-drawer-content.mat-drawer-content-hidden {
  opacity: 0;
}
.mat-drawer-transition .mat-drawer-content {
  transition-duration: 400ms;
  transition-timing-function: cubic-bezier(0.25, 0.8, 0.25, 1);
  transition-property: transform, margin-left, margin-right;
}

.mat-drawer {
  position: relative;
  z-index: 4;
  color: var(--mat-sidenav-container-text-color, var(--mat-sys-on-surface-variant));
  box-shadow: var(--mat-sidenav-container-elevation-shadow, none);
  background-color: var(--mat-sidenav-container-background-color, var(--mat-sys-surface));
  border-top-right-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-bottom-right-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  width: var(--mat-sidenav-container-width, 360px);
  display: block;
  position: absolute;
  top: 0;
  bottom: 0;
  z-index: 3;
  outline: 0;
  box-sizing: border-box;
  overflow-y: auto;
  transform: translate3d(-100%, 0, 0);
}
@media (forced-colors: active) {
  .mat-drawer, [dir=rtl] .mat-drawer.mat-drawer-end {
    border-right: solid 1px currentColor;
  }
}
@media (forced-colors: active) {
  [dir=rtl] .mat-drawer, .mat-drawer.mat-drawer-end {
    border-left: solid 1px currentColor;
    border-right: none;
  }
}
.mat-drawer.mat-drawer-side {
  z-index: 2;
}
.mat-drawer.mat-drawer-end {
  right: 0;
  transform: translate3d(100%, 0, 0);
  border-top-left-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-bottom-left-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
}
[dir=rtl] .mat-drawer {
  border-top-left-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-bottom-left-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
  transform: translate3d(100%, 0, 0);
}
[dir=rtl] .mat-drawer.mat-drawer-end {
  border-top-right-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-bottom-right-radius: var(--mat-sidenav-container-shape, var(--mat-sys-corner-large));
  border-top-left-radius: 0;
  border-bottom-left-radius: 0;
  left: 0;
  right: auto;
  transform: translate3d(-100%, 0, 0);
}
.mat-drawer-transition .mat-drawer {
  transition: transform 400ms cubic-bezier(0.25, 0.8, 0.25, 1);
}
.mat-drawer:not(.mat-drawer-opened):not(.mat-drawer-animating) {
  visibility: hidden;
  box-shadow: none;
}
.mat-drawer:not(.mat-drawer-opened):not(.mat-drawer-animating) .mat-drawer-inner-container {
  display: none;
}
.mat-drawer.mat-drawer-opened.mat-drawer-opened {
  transform: none;
}

.mat-drawer-side {
  box-shadow: none;
  border-right-color: var(--mat-sidenav-container-divider-color, transparent);
  border-right-width: 1px;
  border-right-style: solid;
}
.mat-drawer-side.mat-drawer-end {
  border-left-color: var(--mat-sidenav-container-divider-color, transparent);
  border-left-width: 1px;
  border-left-style: solid;
  border-right: none;
}
[dir=rtl] .mat-drawer-side {
  border-left-color: var(--mat-sidenav-container-divider-color, transparent);
  border-left-width: 1px;
  border-left-style: solid;
  border-right: none;
}
[dir=rtl] .mat-drawer-side.mat-drawer-end {
  border-right-color: var(--mat-sidenav-container-divider-color, transparent);
  border-right-width: 1px;
  border-right-style: solid;
  border-left: none;
}

.mat-drawer-inner-container {
  width: 100%;
  height: 100%;
  overflow: auto;
}

.mat-sidenav-fixed {
  position: fixed;
}
`],encapsulation:2,changeDetection:0})}return r})(),ne=(()=>{class r extends te{static \u0275fac=(()=>{let e;return function(n){return(e||(e=U(r)))(n||r)}})();static \u0275cmp=p({type:r,selectors:[["mat-sidenav-content"]],hostAttrs:[1,"mat-drawer-content","mat-sidenav-content"],features:[R([{provide:j,useExisting:r}]),H],ngContentSelectors:re,decls:1,vars:0,template:function(t,n){t&1&&(k(),h(0))},encapsulation:2,changeDetection:0})}return r})(),ve=(()=>{class r extends _e{get fixedInViewport(){return this._fixedInViewport}set fixedInViewport(e){this._fixedInViewport=S(e)}_fixedInViewport=!1;get fixedTopGap(){return this._fixedTopGap}set fixedTopGap(e){this._fixedTopGap=ue(e)}_fixedTopGap=0;get fixedBottomGap(){return this._fixedBottomGap}set fixedBottomGap(e){this._fixedBottomGap=ue(e)}_fixedBottomGap=0;static \u0275fac=(()=>{let e;return function(n){return(e||(e=U(r)))(n||r)}})();static \u0275cmp=p({type:r,selectors:[["mat-sidenav"]],hostAttrs:[1,"mat-drawer","mat-sidenav"],hostVars:16,hostBindings:function(t,n){t&2&&(C("tabIndex",n.mode!=="side"?"-1":null)("align",null),D("top",n.fixedInViewport?n.fixedTopGap:null,"px")("bottom",n.fixedInViewport?n.fixedBottomGap:null,"px"),b("mat-drawer-end",n.position==="end")("mat-drawer-over",n.mode==="over")("mat-drawer-push",n.mode==="push")("mat-drawer-side",n.mode==="side")("mat-sidenav-fixed",n.fixedInViewport))},inputs:{fixedInViewport:"fixedInViewport",fixedTopGap:"fixedTopGap",fixedBottomGap:"fixedBottomGap"},exportAs:["matSidenav"],features:[R([{provide:_e,useExisting:r}]),H],ngContentSelectors:re,decls:3,vars:0,consts:[["content",""],["cdkScrollable","",1,"mat-drawer-inner-container"]],template:function(t,n){t&1&&(k(),a(0,"div",1,0),h(2),o())},dependencies:[j],encapsulation:2,changeDetection:0})}return r})(),ut=(()=>{class r extends ge{_allDrawers=void 0;_content=void 0;static \u0275fac=(()=>{let e;return function(n){return(e||(e=U(r)))(n||r)}})();static \u0275cmp=p({type:r,selectors:[["mat-sidenav-container"]],contentQueries:function(t,n,l){if(t&1&&le(l,ne,5)(l,ve,5),t&2){let m;x(m=M())&&(n._content=m.first),x(m=M())&&(n._allDrawers=m)}},hostAttrs:[1,"mat-drawer-container","mat-sidenav-container"],hostVars:2,hostBindings:function(t,n){t&2&&b("mat-drawer-container-explicit-backdrop",n._backdropOverride)},exportAs:["matSidenavContainer"],features:[R([{provide:be,useExisting:r},{provide:ge,useExisting:r}]),H],ngContentSelectors:Dt,decls:4,vars:2,consts:[[1,"mat-drawer-backdrop",3,"mat-drawer-shown"],[1,"mat-drawer-backdrop",3,"click"]],template:function(t,n){t&1&&(k(Mt),_(0,St,1,2,"div",0),h(1),h(2,1),_(3,Et,2,0,"mat-sidenav-content")),t&2&&(g(n.hasBackdrop?0:-1),d(3),g(n._content?-1:3))},dependencies:[ne],styles:[Tt],encapsulation:2,changeDetection:0})}return r})(),ht=(()=>{class r{static \u0275fac=function(t){return new(t||r)};static \u0275mod=K({type:r});static \u0275inj=W({imports:[fe,ee,fe]})}return r})();function It(r,s){r&1&&Q(0,"div",2)}var Pt=new X("MAT_PROGRESS_BAR_DEFAULT_OPTIONS");var _t=(()=>{class r{_elementRef=i(I);_ngZone=i(F);_changeDetectorRef=i(V);_renderer=i(q);_cleanupTransitionEnd;constructor(){let e=Ve(),t=i(Pt,{optional:!0});this._isNoopAnimation=e==="di-disabled",e==="reduced-motion"&&this._elementRef.nativeElement.classList.add("mat-progress-bar-reduced-motion"),t&&(t.color&&(this.color=this._defaultColor=t.color),this.mode=t.mode||this.mode)}_isNoopAnimation;get color(){return this._color||this._defaultColor}set color(e){this._color=e}_color;_defaultColor="primary";get value(){return this._value}set value(e){this._value=ft(e||0),this._changeDetectorRef.markForCheck()}_value=0;get bufferValue(){return this._bufferValue||0}set bufferValue(e){this._bufferValue=ft(e||0),this._changeDetectorRef.markForCheck()}_bufferValue=0;animationEnd=new B;get mode(){return this._mode}set mode(e){this._mode=e,this._changeDetectorRef.markForCheck()}_mode="determinate";ngAfterViewInit(){this._ngZone.runOutsideAngular(()=>{this._cleanupTransitionEnd=this._renderer.listen(this._elementRef.nativeElement,"transitionend",this._transitionendHandler)})}ngOnDestroy(){this._cleanupTransitionEnd?.()}_getPrimaryBarTransform(){return`scaleX(${this._isIndeterminate()?1:this.value/100})`}_getBufferBarFlexBasis(){return`${this.mode==="buffer"?this.bufferValue:100}%`}_isIndeterminate(){return this.mode==="indeterminate"||this.mode==="query"}_transitionendHandler=e=>{this.animationEnd.observers.length===0||!e.target||!e.target.classList.contains("mdc-linear-progress__primary-bar")||(this.mode==="determinate"||this.mode==="buffer")&&this._ngZone.run(()=>this.animationEnd.next({value:this.value}))};static \u0275fac=function(t){return new(t||r)};static \u0275cmp=p({type:r,selectors:[["mat-progress-bar"]],hostAttrs:["role","progressbar","aria-valuemin","0","aria-valuemax","100","tabindex","-1",1,"mat-mdc-progress-bar","mdc-linear-progress"],hostVars:10,hostBindings:function(t,n){t&2&&(C("aria-valuenow",n._isIndeterminate()?null:n.value)("mode",n.mode),De("mat-"+n.color),b("_mat-animation-noopable",n._isNoopAnimation)("mdc-linear-progress--animation-ready",!n._isNoopAnimation)("mdc-linear-progress--indeterminate",n._isIndeterminate()))},inputs:{color:"color",value:[2,"value","value",pe],bufferValue:[2,"bufferValue","bufferValue",pe],mode:"mode"},outputs:{animationEnd:"animationEnd"},exportAs:["matProgressBar"],decls:7,vars:5,consts:[["aria-hidden","true",1,"mdc-linear-progress__buffer"],[1,"mdc-linear-progress__buffer-bar"],[1,"mdc-linear-progress__buffer-dots"],["aria-hidden","true",1,"mdc-linear-progress__bar","mdc-linear-progress__primary-bar"],[1,"mdc-linear-progress__bar-inner"],["aria-hidden","true",1,"mdc-linear-progress__bar","mdc-linear-progress__secondary-bar"]],template:function(t,n){t&1&&($(0,"div",0),Q(1,"div",1),_(2,It,1,0,"div",2),Y(),$(3,"div",3),Q(4,"span",4),Y(),$(5,"div",5),Q(6,"span",4),Y()),t&2&&(d(),D("flex-basis",n._getBufferBarFlexBasis()),d(),g(n.mode==="buffer"?2:-1),d(),D("transform",n._getPrimaryBarTransform()))},styles:[`.mat-mdc-progress-bar {
  --mat-progress-bar-animation-multiplier: 1;
  display: block;
  text-align: start;
}
.mat-mdc-progress-bar[mode=query] {
  transform: scaleX(-1);
}
.mat-mdc-progress-bar._mat-animation-noopable .mdc-linear-progress__buffer-dots,
.mat-mdc-progress-bar._mat-animation-noopable .mdc-linear-progress__primary-bar,
.mat-mdc-progress-bar._mat-animation-noopable .mdc-linear-progress__secondary-bar,
.mat-mdc-progress-bar._mat-animation-noopable .mdc-linear-progress__bar-inner.mdc-linear-progress__bar-inner {
  animation: none;
}
.mat-mdc-progress-bar._mat-animation-noopable .mdc-linear-progress__primary-bar,
.mat-mdc-progress-bar._mat-animation-noopable .mdc-linear-progress__buffer-bar {
  transition: transform 1ms;
}

.mat-progress-bar-reduced-motion {
  --mat-progress-bar-animation-multiplier: 2;
}

.mdc-linear-progress {
  position: relative;
  width: 100%;
  transform: translateZ(0);
  outline: 1px solid transparent;
  overflow-x: hidden;
  transition: opacity 250ms 0ms cubic-bezier(0.4, 0, 0.6, 1);
  height: max(var(--mat-progress-bar-track-height, 4px), var(--mat-progress-bar-active-indicator-height, 4px));
}
@media (forced-colors: active) {
  .mdc-linear-progress {
    outline-color: CanvasText;
  }
}

.mdc-linear-progress__bar {
  position: absolute;
  top: 0;
  bottom: 0;
  margin: auto 0;
  width: 100%;
  animation: none;
  transform-origin: top left;
  transition: transform 250ms 0ms cubic-bezier(0.4, 0, 0.6, 1);
  height: var(--mat-progress-bar-active-indicator-height, 4px);
}
.mdc-linear-progress--indeterminate .mdc-linear-progress__bar {
  transition: none;
}
[dir=rtl] .mdc-linear-progress__bar {
  right: 0;
  transform-origin: center right;
}

.mdc-linear-progress__bar-inner {
  display: inline-block;
  position: absolute;
  width: 100%;
  animation: none;
  border-top-style: solid;
  border-color: var(--mat-progress-bar-active-indicator-color, var(--mat-sys-primary));
  border-top-width: var(--mat-progress-bar-active-indicator-height, 4px);
}

.mdc-linear-progress__buffer {
  display: flex;
  position: absolute;
  top: 0;
  bottom: 0;
  margin: auto 0;
  width: 100%;
  overflow: hidden;
  height: var(--mat-progress-bar-track-height, 4px);
  border-radius: var(--mat-progress-bar-track-shape, var(--mat-sys-corner-none));
}

.mdc-linear-progress__buffer-dots {
  background-image: radial-gradient(circle, var(--mat-progress-bar-track-color, var(--mat-sys-surface-variant)) calc(var(--mat-progress-bar-track-height, 4px) / 2), transparent 0);
  background-repeat: repeat-x;
  background-size: calc(calc(var(--mat-progress-bar-track-height, 4px) / 2) * 5);
  background-position: left;
  flex: auto;
  transform: rotate(180deg);
  animation: mdc-linear-progress-buffering calc(250ms * var(--mat-progress-bar-animation-multiplier)) infinite linear;
}
@media (forced-colors: active) {
  .mdc-linear-progress__buffer-dots {
    background-color: ButtonBorder;
  }
}
[dir=rtl] .mdc-linear-progress__buffer-dots {
  animation: mdc-linear-progress-buffering-reverse calc(250ms * var(--mat-progress-bar-animation-multiplier)) infinite linear;
  transform: rotate(0);
}

.mdc-linear-progress__buffer-bar {
  flex: 0 1 100%;
  transition: flex-basis 250ms 0ms cubic-bezier(0.4, 0, 0.6, 1);
  background-color: var(--mat-progress-bar-track-color, var(--mat-sys-surface-variant));
}

.mdc-linear-progress__primary-bar {
  transform: scaleX(0);
}
.mdc-linear-progress--indeterminate .mdc-linear-progress__primary-bar {
  left: -145.166611%;
}
.mdc-linear-progress--indeterminate.mdc-linear-progress--animation-ready .mdc-linear-progress__primary-bar {
  animation: mdc-linear-progress-primary-indeterminate-translate calc(2s * var(--mat-progress-bar-animation-multiplier)) infinite linear;
}
.mdc-linear-progress--indeterminate.mdc-linear-progress--animation-ready .mdc-linear-progress__primary-bar > .mdc-linear-progress__bar-inner {
  animation: mdc-linear-progress-primary-indeterminate-scale calc(2s * var(--mat-progress-bar-animation-multiplier)) infinite linear;
}
[dir=rtl] .mdc-linear-progress.mdc-linear-progress--animation-ready .mdc-linear-progress__primary-bar {
  animation-name: mdc-linear-progress-primary-indeterminate-translate-reverse;
}
[dir=rtl] .mdc-linear-progress.mdc-linear-progress--indeterminate .mdc-linear-progress__primary-bar {
  right: -145.166611%;
  left: auto;
}

.mdc-linear-progress__secondary-bar {
  display: none;
}
.mdc-linear-progress--indeterminate .mdc-linear-progress__secondary-bar {
  left: -54.888891%;
  display: block;
}
.mdc-linear-progress--indeterminate.mdc-linear-progress--animation-ready .mdc-linear-progress__secondary-bar {
  animation: mdc-linear-progress-secondary-indeterminate-translate calc(2s * var(--mat-progress-bar-animation-multiplier)) infinite linear;
}
.mdc-linear-progress--indeterminate.mdc-linear-progress--animation-ready .mdc-linear-progress__secondary-bar > .mdc-linear-progress__bar-inner {
  animation: mdc-linear-progress-secondary-indeterminate-scale calc(2s * var(--mat-progress-bar-animation-multiplier)) infinite linear;
}
[dir=rtl] .mdc-linear-progress.mdc-linear-progress--animation-ready .mdc-linear-progress__secondary-bar {
  animation-name: mdc-linear-progress-secondary-indeterminate-translate-reverse;
}
[dir=rtl] .mdc-linear-progress.mdc-linear-progress--indeterminate .mdc-linear-progress__secondary-bar {
  right: -54.888891%;
  left: auto;
}

@keyframes mdc-linear-progress-buffering {
  from {
    transform: rotate(180deg) translateX(calc(var(--mat-progress-bar-track-height, 4px) * -2.5));
  }
}
@keyframes mdc-linear-progress-primary-indeterminate-translate {
  0% {
    transform: translateX(0);
  }
  20% {
    animation-timing-function: cubic-bezier(0.5, 0, 0.701732, 0.495819);
    transform: translateX(0);
  }
  59.15% {
    animation-timing-function: cubic-bezier(0.302435, 0.381352, 0.55, 0.956352);
    transform: translateX(83.67142%);
  }
  100% {
    transform: translateX(200.611057%);
  }
}
@keyframes mdc-linear-progress-primary-indeterminate-scale {
  0% {
    transform: scaleX(0.08);
  }
  36.65% {
    animation-timing-function: cubic-bezier(0.334731, 0.12482, 0.785844, 1);
    transform: scaleX(0.08);
  }
  69.15% {
    animation-timing-function: cubic-bezier(0.06, 0.11, 0.6, 1);
    transform: scaleX(0.661479);
  }
  100% {
    transform: scaleX(0.08);
  }
}
@keyframes mdc-linear-progress-secondary-indeterminate-translate {
  0% {
    animation-timing-function: cubic-bezier(0.15, 0, 0.515058, 0.409685);
    transform: translateX(0);
  }
  25% {
    animation-timing-function: cubic-bezier(0.31033, 0.284058, 0.8, 0.733712);
    transform: translateX(37.651913%);
  }
  48.35% {
    animation-timing-function: cubic-bezier(0.4, 0.627035, 0.6, 0.902026);
    transform: translateX(84.386165%);
  }
  100% {
    transform: translateX(160.277782%);
  }
}
@keyframes mdc-linear-progress-secondary-indeterminate-scale {
  0% {
    animation-timing-function: cubic-bezier(0.205028, 0.057051, 0.57661, 0.453971);
    transform: scaleX(0.08);
  }
  19.15% {
    animation-timing-function: cubic-bezier(0.152313, 0.196432, 0.648374, 1.004315);
    transform: scaleX(0.457104);
  }
  44.15% {
    animation-timing-function: cubic-bezier(0.257759, -0.003163, 0.211762, 1.38179);
    transform: scaleX(0.72796);
  }
  100% {
    transform: scaleX(0.08);
  }
}
@keyframes mdc-linear-progress-primary-indeterminate-translate-reverse {
  0% {
    transform: translateX(0);
  }
  20% {
    animation-timing-function: cubic-bezier(0.5, 0, 0.701732, 0.495819);
    transform: translateX(0);
  }
  59.15% {
    animation-timing-function: cubic-bezier(0.302435, 0.381352, 0.55, 0.956352);
    transform: translateX(-83.67142%);
  }
  100% {
    transform: translateX(-200.611057%);
  }
}
@keyframes mdc-linear-progress-secondary-indeterminate-translate-reverse {
  0% {
    animation-timing-function: cubic-bezier(0.15, 0, 0.515058, 0.409685);
    transform: translateX(0);
  }
  25% {
    animation-timing-function: cubic-bezier(0.31033, 0.284058, 0.8, 0.733712);
    transform: translateX(-37.651913%);
  }
  48.35% {
    animation-timing-function: cubic-bezier(0.4, 0.627035, 0.6, 0.902026);
    transform: translateX(-84.386165%);
  }
  100% {
    transform: translateX(-160.277782%);
  }
}
@keyframes mdc-linear-progress-buffering-reverse {
  from {
    transform: translateX(-10px);
  }
}
`],encapsulation:2,changeDetection:0})}return r})();function ft(r,s=0,e=100){return Math.max(s,Math.min(e,r))}var gt=(()=>{class r{static \u0275fac=function(t){return new(t||r)};static \u0275mod=K({type:r});static \u0275inj=W({imports:[ee]})}return r})();var Rt=(r,s)=>s.route;function Ot(r,s){if(r&1){let e=A();a(0,"a",17),u("click",function(){E(e);let n=w();return T(n.closeOnHandset())}),a(1,"mat-icon",18),c(2),o(),a(3,"span",19),c(4),O(5,"t"),o()()}if(r&2){let e=s.$implicit;G("routerLink",e.route),d(2),v(e.icon),d(2),v(z(5,3,e.labelKey))}}function zt(r,s){if(r&1){let e=A();a(0,"button",20)(1,"mat-icon"),c(2,"account_circle"),o()(),a(3,"mat-menu",null,2)(5,"div",21)(6,"div",22),c(7),o(),a(8,"div",23),c(9),o()(),P(10,"mat-divider"),a(11,"button",13),u("click",function(){E(e);let n=w();return T(n.logout())}),a(12,"mat-icon"),c(13,"logout"),o(),a(14,"span"),c(15),O(16,"t"),o()()()}if(r&2){let e,t,n=me(4),l=w();G("matMenuTriggerFor",n),d(7),v((e=l.auth.currentUser())==null?null:e.userName),d(2),v((t=l.auth.currentUser())==null?null:t.email),d(6),v(z(16,4,"nav.logout"))}}function Vt(r,s){r&1&&(a(0,"a",14),c(1),O(2,"t"),o()),r&2&&(d(),v(z(2,1,"nav.login")))}function jt(r,s){r&1&&P(0,"mat-progress-bar",15)}var bt=class r{breakpoint=i(Ae);theme=i(nt);loading=i(mt);translation=i(et);auth=i(Fe);UserRole=Ie;isHandset=at(this.breakpoint.observe([he.Handset,he.Small]).pipe(L(s=>s.matches)),{initialValue:!1});sidenavOpened=Z(!1);navItems=[{labelKey:"nav.dashboard",route:"/app/dashboard",icon:"dashboard"},{labelKey:"nav.youth",route:"/app/youth",icon:"groups",roles:["Admin","Catechist"]},{labelKey:"nav.catechists",route:"/app/catechists",icon:"school",roles:["Admin"]},{labelKey:"nav.classes",route:"/app/classes",icon:"class",roles:["Admin","Catechist","EucharisticYouth","EucharisticYouthContact"]},{labelKey:"nav.attendance",route:"/app/attendance",icon:"how_to_reg",roles:["Admin","Catechist"]},{labelKey:"nav.exams",route:"/app/exams",icon:"quiz",roles:["Admin","Catechist"]},{labelKey:"nav.admin",route:"/app/admin/users",icon:"admin_panel_settings",roles:["Admin"]}];visibleNav(){return this.navItems.filter(s=>!s.roles||this.auth.hasRole(s.roles))}toggleTheme(){this.theme.toggle()}async setLang(s){await this.translation.setLang(s)}logout(){this.auth.logout()}toggleSidenav(){this.sidenavOpened.update(s=>!s)}closeOnHandset(){this.isHandset()&&this.sidenavOpened.set(!1)}static \u0275fac=function(e){return new(e||r)};static \u0275cmp=p({type:r,selectors:[["app-shell"]],decls:39,vars:13,consts:[["sidenav",""],["langMenu","matMenu"],["userMenu","matMenu"],[1,"shell-container"],[1,"shell-sidenav",3,"openedChange","mode","opened"],[1,"sidenav-brand"],["mat-list-item","","routerLinkActive","active-link",3,"routerLink"],["color","primary",1,"shell-toolbar"],["mat-icon-button","","aria-label","Toggle navigation",3,"click"],[1,"toolbar-title"],[1,"spacer"],["mat-icon-button","",3,"click"],["mat-icon-button","","aria-label","Language",3,"matMenuTriggerFor"],["mat-menu-item","",3,"click"],["mat-button","","routerLink","/auth/login"],["mode","indeterminate",1,"shell-loading-bar"],[1,"shell-content"],["mat-list-item","","routerLinkActive","active-link",3,"click","routerLink"],["matListItemIcon",""],["matListItemTitle",""],["mat-icon-button","","aria-label","User menu",3,"matMenuTriggerFor"],["mat-menu-item","","disabled","",1,"user-info"],[1,"user-name"],[1,"user-email"]],template:function(e,t){if(e&1&&(a(0,"mat-sidenav-container",3)(1,"mat-sidenav",4,0),u("openedChange",function(l){return t.sidenavOpened.set(l)}),a(3,"div",5)(4,"mat-icon"),c(5,"church"),o(),a(6,"span"),c(7),O(8,"t"),o()(),P(9,"mat-divider"),a(10,"mat-nav-list"),xe(11,Ot,6,5,"a",6,Rt),o()(),a(13,"mat-sidenav-content")(14,"mat-toolbar",7)(15,"button",8),u("click",function(){return t.toggleSidenav()}),a(16,"mat-icon"),c(17,"menu"),o()(),a(18,"span",9),c(19),O(20,"t"),o(),P(21,"span",10),a(22,"button",11),u("click",function(){return t.toggleTheme()}),a(23,"mat-icon"),c(24),o()(),a(25,"button",12)(26,"mat-icon"),c(27,"translate"),o()(),a(28,"mat-menu",null,1)(30,"button",13),u("click",function(){return t.setLang("vi")}),c(31,"Ti\u1EBFng Vi\u1EC7t"),o(),a(32,"button",13),u("click",function(){return t.setLang("en")}),c(33,"English"),o()(),_(34,zt,17,6)(35,Vt,3,3,"a",14),o(),_(36,jt,1,0,"mat-progress-bar",15),a(37,"main",16),P(38,"router-outlet"),o()()()),e&2){let n=me(29);d(),G("mode",t.isHandset()?"over":"side")("opened",!t.isHandset()&&t.sidenavOpened()),d(6),v(z(8,9,"app.title")),d(4),Me(t.visibleNav()),d(8),v(z(20,11,"app.title")),d(3),C("aria-label","Toggle dark mode"),d(2),v(t.theme.darkMode()?"light_mode":"dark_mode"),d(),G("matMenuTriggerFor",n),d(9),g(t.auth.isAuthenticated()?34:35),d(2),g(t.loading.isLoading()?36:-1)}},dependencies:[Se,Te,Be,Ee,Ze,We,Qe,Ge,He,Xe,Le,ht,ve,ut,ne,ct,lt,dt,st,rt,ot,Je,$e,Ke,Ye,it,gt,_t,tt],styles:[".shell-container[_ngcontent-%COMP%]{height:100vh}.shell-loading-bar[_ngcontent-%COMP%]{position:sticky;top:0;z-index:10}.shell-sidenav[_ngcontent-%COMP%]{width:260px}.sidenav-brand[_ngcontent-%COMP%]{display:flex;align-items:center;gap:12px;padding:16px;font-weight:600;font-size:16px}.shell-toolbar[_ngcontent-%COMP%]{position:sticky;top:0;z-index:10;gap:4px}.toolbar-title[_ngcontent-%COMP%]{margin-left:8px;font-size:18px;font-weight:500}.spacer[_ngcontent-%COMP%]{flex:1 1 auto}.shell-content[_ngcontent-%COMP%]{padding:16px;min-height:calc(100vh - 64px);box-sizing:border-box}.active-link[_ngcontent-%COMP%]{background-color:var(--mat-sys-secondary-container);color:var(--mat-sys-on-secondary-container)}.user-info[_ngcontent-%COMP%]{pointer-events:none;opacity:1!important}.user-info[_ngcontent-%COMP%]   .user-name[_ngcontent-%COMP%]{font-weight:500}.user-info[_ngcontent-%COMP%]   .user-email[_ngcontent-%COMP%]{font-size:12px;color:var(--mat-sys-on-surface-variant)}"]})};export{bt as Shell};
