(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["main"],{

/***/ "./$$_lazy_route_resource lazy recursive":
/*!******************************************************!*\
  !*** ./$$_lazy_route_resource lazy namespace object ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncaught exception popping up in devtools
	return Promise.resolve().then(function() {
		var e = new Error("Cannot find module '" + req + "'");
		e.code = 'MODULE_NOT_FOUND';
		throw e;
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "./$$_lazy_route_resource lazy recursive";

/***/ }),

/***/ "./src/app/app-routing.module.ts":
/*!***************************************!*\
  !*** ./src/app/app-routing.module.ts ***!
  \***************************************/
/*! exports provided: AppRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppRoutingModule", function() { return AppRoutingModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _common_login_login_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./common/login/login.component */ "./src/app/common/login/login.component.ts");
/* harmony import */ var _common_home_home_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./common/home/home.component */ "./src/app/common/home/home.component.ts");
/* harmony import */ var _residents_dashboard_residents_dashboard_residents_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./residents/dashboard-residents/dashboard-residents.component */ "./src/app/residents/dashboard-residents/dashboard-residents.component.ts");
/* harmony import */ var _residents_enquires_enquires_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./residents/enquires/enquires.component */ "./src/app/residents/enquires/enquires.component.ts");
/* harmony import */ var _residents_enquires_enquires_edit_enquires_edit_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./residents/enquires/enquires-edit/enquires-edit.component */ "./src/app/residents/enquires/enquires-edit/enquires-edit.component.ts");
/* harmony import */ var _helpers__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./helpers */ "./src/app/helpers/index.ts");










const routes = [
    {
        path: '',
        component: _common_home_home_component__WEBPACK_IMPORTED_MODULE_3__["HomeComponent"],
        canActivate: [_helpers__WEBPACK_IMPORTED_MODULE_7__["AuthGuard"]]
    },
    {
        path: 'residents',
        component: _residents_dashboard_residents_dashboard_residents_component__WEBPACK_IMPORTED_MODULE_4__["DashboardResidentsComponent"],
        canActivate: [_helpers__WEBPACK_IMPORTED_MODULE_7__["AuthResidentsGuard"]],
    },
    {
        path: 'enquires',
        component: _residents_enquires_enquires_component__WEBPACK_IMPORTED_MODULE_5__["EnquiresComponent"],
        canActivate: [_helpers__WEBPACK_IMPORTED_MODULE_7__["AuthResidentsGuard"]]
    },
    {
        path: 'enquires-edit/:referenceId',
        component: _residents_enquires_enquires_edit_enquires_edit_component__WEBPACK_IMPORTED_MODULE_6__["EnquiresEditComponent"]
    },
    {
        path: 'enquires-add',
        component: _residents_enquires_enquires_edit_enquires_edit_component__WEBPACK_IMPORTED_MODULE_6__["EnquiresEditComponent"]
    },
    {
        path: 'login',
        component: _common_login_login_component__WEBPACK_IMPORTED_MODULE_2__["LoginComponent"]
    },
    {
        path: 'admin',
        loadChildren: () => __webpack_require__.e(/*! import() | admin-admin-module */ "admin-admin-module").then(__webpack_require__.bind(null, /*! ./admin/admin.module */ "./src/app/admin/admin.module.ts")).then(m => m.AdminModule),
        canActivate: [_helpers__WEBPACK_IMPORTED_MODULE_7__["AuthAdminGuard"]]
    },
    // // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
class AppRoutingModule {
}
AppRoutingModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: AppRoutingModule });
AppRoutingModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ factory: function AppRoutingModule_Factory(t) { return new (t || AppRoutingModule)(); }, providers: [], imports: [[_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forRoot(routes)], _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](AppRoutingModule, { imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]], exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AppRoutingModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forRoot(routes)],
                exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]],
                providers: []
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/app.component.ts":
/*!**********************************!*\
  !*** ./src/app/app.component.ts ***!
  \**********************************/
/*! exports provided: AppComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppComponent", function() { return AppComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./services/index */ "./src/app/services/index.ts");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");







const _c0 = function () { return ["residents"]; };
function AppComponent_li_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "li", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Residents");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("routerLink", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction0"](1, _c0));
} }
const _c1 = function () { return ["admin"]; };
function AppComponent_li_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "li", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Admin");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("routerLink", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction0"](1, _c1));
} }
function AppComponent_li_7_Template(rf, ctx) { if (rf & 1) {
    const _r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "li", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "a", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function AppComponent_li_7_Template_a_click_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r4); const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r3.logout(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, "Logout");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
const _c2 = function () { return ["/"]; };
class AppComponent {
    constructor(authenticationService, userService) {
        this.authenticationService = authenticationService;
        this.userService = userService;
        this.title = 'nyms-care-client';
    }
    ngOnInit() {
    }
    get isLoggedIn() {
        if (this.authenticationService.getToken()) {
            return true;
        }
        return false;
    }
    get isAuthorized() {
        //if (this.userService.isInRole('Manager') || this.userService.isInRole('Admin')) {
        return true;
        //}
        //return false;
    }
    get isAdmin() {
        //return this.userService.isInRole('Admin');
        return true;
    }
    logout() {
        this.userService.logout();
    }
}
AppComponent.ɵfac = function AppComponent_Factory(t) { return new (t || AppComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_index__WEBPACK_IMPORTED_MODULE_1__["AuthenticationService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_index__WEBPACK_IMPORTED_MODULE_1__["UserService"])); };
AppComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: AppComponent, selectors: [["app-root"]], decls: 11, vars: 5, consts: [[1, "navbar", "navbar-expand", "navbar-dark", "bg-dark"], [1, "navbar-nav", "mr-auto"], [1, "nav-item"], [1, "nav-link", 3, "routerLink"], ["class", "nav-item", 4, "ngIf"], [1, "navbar-text"], [1, "container"], [1, "nav-link", 3, "click"]], template: function AppComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "header", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ul", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "li", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "a", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Home");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](5, AppComponent_li_5_Template, 3, 2, "li", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, AppComponent_li_6_Template, 3, 2, "li", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](7, AppComponent_li_7_Template, 3, 0, "li", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](8, "span", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](10, "router-outlet");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("routerLink", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction0"](4, _c2));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isAuthorized);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isAdmin);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isLoggedIn);
    } }, directives: [_ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_2__["NgbNavbar"], _angular_router__WEBPACK_IMPORTED_MODULE_3__["RouterLinkWithHref"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"], _angular_router__WEBPACK_IMPORTED_MODULE_3__["RouterOutlet"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FwcC5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AppComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-root',
                templateUrl: './app.component.html',
                styleUrls: ['./app.component.css']
            }]
    }], function () { return [{ type: _services_index__WEBPACK_IMPORTED_MODULE_1__["AuthenticationService"] }, { type: _services_index__WEBPACK_IMPORTED_MODULE_1__["UserService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/app.module.ts":
/*!*******************************!*\
  !*** ./src/app/app.module.ts ***!
  \*******************************/
/*! exports provided: tokenGetter, AppModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "tokenGetter", function() { return tokenGetter; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppModule", function() { return AppModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/platform-browser */ "./node_modules/@angular/platform-browser/__ivy_ngcc__/fesm2015/platform-browser.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var _auth0_angular_jwt__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @auth0/angular-jwt */ "./node_modules/@auth0/angular-jwt/__ivy_ngcc__/fesm2015/auth0-angular-jwt.js");
/* harmony import */ var _app_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./app.component */ "./src/app/app.component.ts");
/* harmony import */ var _app_routing_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./app-routing.module */ "./src/app/app-routing.module.ts");
/* harmony import */ var _helpers__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./helpers */ "./src/app/helpers/index.ts");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
/* harmony import */ var _common_home_home_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./common/home/home.component */ "./src/app/common/home/home.component.ts");
/* harmony import */ var _common_login_login_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./common/login/login.component */ "./src/app/common/login/login.component.ts");
/* harmony import */ var _residents_dashboard_residents_dashboard_residents_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./residents/dashboard-residents/dashboard-residents.component */ "./src/app/residents/dashboard-residents/dashboard-residents.component.ts");
/* harmony import */ var _residents_enquires_enquires_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ./residents/enquires/enquires.component */ "./src/app/residents/enquires/enquires.component.ts");
/* harmony import */ var _residents_enquires_enquires_edit_enquires_edit_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ./residents/enquires/enquires-edit/enquires-edit.component */ "./src/app/residents/enquires/enquires-edit/enquires-edit.component.ts");
/* harmony import */ var _residents_enquires_enquires_list_enquires_list_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ./residents/enquires/enquires-list/enquires-list.component */ "./src/app/residents/enquires/enquires-list/enquires-list.component.ts");
/* harmony import */ var _residents_templ_edit_address_edit_address_edit_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ./residents/templ-edit/address-edit/address-edit.component */ "./src/app/residents/templ-edit/address-edit/address-edit.component.ts");
/* harmony import */ var _residents_templ_edit_resident_profile_edit_resident_profile_edit_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ./residents/templ-edit/resident-profile-edit/resident-profile-edit.component */ "./src/app/residents/templ-edit/resident-profile-edit/resident-profile-edit.component.ts");
/* harmony import */ var _residents_templ_edit_social_worker_detail_edit_social_worker_detail_edit_component__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! ./residents/templ-edit/social-worker-detail-edit/social-worker-detail-edit.component */ "./src/app/residents/templ-edit/social-worker-detail-edit/social-worker-detail-edit.component.ts");
/* harmony import */ var _residents_templ_edit_care_type_edit_care_type_edit_component__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(/*! ./residents/templ-edit/care-type-edit/care-type-edit.component */ "./src/app/residents/templ-edit/care-type-edit/care-type-edit.component.ts");
/* harmony import */ var _residents_templ_edit_room_location_edit_room_location_edit_component__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(/*! ./residents/templ-edit/room-location-edit/room-location-edit.component */ "./src/app/residents/templ-edit/room-location-edit/room-location-edit.component.ts");
/* harmony import */ var _residents_templ_edit_enquiry_misc_edit_enquiry_misc_edit_component__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(/*! ./residents/templ-edit/enquiry-misc-edit/enquiry-misc-edit.component */ "./src/app/residents/templ-edit/enquiry-misc-edit/enquiry-misc-edit.component.ts");








// import { CommonModule } from '@angular/common';



// import { UsersComponent } from './users/users.component';
// import { WeatherComponent } from './weather/weather.component';
// import { FinanceComponent } from './finance/finance.component';
// import { ResidentsComponent } from './residents/residents.component';












function tokenGetter() {
    return localStorage.getItem('id_token');
}
class AppModule {
}
AppModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: AppModule, bootstrap: [_app_component__WEBPACK_IMPORTED_MODULE_5__["AppComponent"]] });
AppModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ factory: function AppModule_Factory(t) { return new (t || AppModule)(); }, providers: [
        //{ provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AuthenticationService] },
        { provide: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HTTP_INTERCEPTORS"], useClass: _helpers__WEBPACK_IMPORTED_MODULE_7__["JwtInterceptor"], multi: true },
        { provide: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HTTP_INTERCEPTORS"], useClass: _helpers__WEBPACK_IMPORTED_MODULE_7__["ErrorInterceptorNoRefresh"], multi: true },
    ], imports: [[
            _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__["BrowserModule"],
            _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_8__["NgbModule"],
            _app_routing_module__WEBPACK_IMPORTED_MODULE_6__["AppRoutingModule"],
            _angular_forms__WEBPACK_IMPORTED_MODULE_2__["ReactiveFormsModule"],
            _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClientModule"],
            _auth0_angular_jwt__WEBPACK_IMPORTED_MODULE_4__["JwtModule"].forRoot({
                config: {
                    tokenGetter: tokenGetter,
                    authScheme: 'JWT'
                }
            })
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](AppModule, { declarations: [_app_component__WEBPACK_IMPORTED_MODULE_5__["AppComponent"],
        _common_login_login_component__WEBPACK_IMPORTED_MODULE_10__["LoginComponent"],
        _common_home_home_component__WEBPACK_IMPORTED_MODULE_9__["HomeComponent"],
        _residents_dashboard_residents_dashboard_residents_component__WEBPACK_IMPORTED_MODULE_11__["DashboardResidentsComponent"],
        _residents_enquires_enquires_component__WEBPACK_IMPORTED_MODULE_12__["EnquiresComponent"],
        _residents_enquires_enquires_edit_enquires_edit_component__WEBPACK_IMPORTED_MODULE_13__["EnquiresEditComponent"],
        _residents_enquires_enquires_list_enquires_list_component__WEBPACK_IMPORTED_MODULE_14__["EnquiresListComponent"],
        _residents_templ_edit_address_edit_address_edit_component__WEBPACK_IMPORTED_MODULE_15__["AddressEditComponent"],
        _residents_templ_edit_resident_profile_edit_resident_profile_edit_component__WEBPACK_IMPORTED_MODULE_16__["ResidentProfileEditComponent"],
        _residents_templ_edit_social_worker_detail_edit_social_worker_detail_edit_component__WEBPACK_IMPORTED_MODULE_17__["SocialWorkerDetailEditComponent"],
        _residents_templ_edit_care_type_edit_care_type_edit_component__WEBPACK_IMPORTED_MODULE_18__["CareTypeEditComponent"],
        _residents_templ_edit_room_location_edit_room_location_edit_component__WEBPACK_IMPORTED_MODULE_19__["RoomLocationEditComponent"],
        _residents_templ_edit_enquiry_misc_edit_enquiry_misc_edit_component__WEBPACK_IMPORTED_MODULE_20__["EnquiryMiscEditComponent"]], imports: [_angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__["BrowserModule"],
        _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_8__["NgbModule"],
        _app_routing_module__WEBPACK_IMPORTED_MODULE_6__["AppRoutingModule"],
        _angular_forms__WEBPACK_IMPORTED_MODULE_2__["ReactiveFormsModule"],
        _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClientModule"], _auth0_angular_jwt__WEBPACK_IMPORTED_MODULE_4__["JwtModule"]] }); })();
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AppModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
                imports: [
                    _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__["BrowserModule"],
                    _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_8__["NgbModule"],
                    _app_routing_module__WEBPACK_IMPORTED_MODULE_6__["AppRoutingModule"],
                    _angular_forms__WEBPACK_IMPORTED_MODULE_2__["ReactiveFormsModule"],
                    _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClientModule"],
                    _auth0_angular_jwt__WEBPACK_IMPORTED_MODULE_4__["JwtModule"].forRoot({
                        config: {
                            tokenGetter: tokenGetter,
                            authScheme: 'JWT'
                        }
                    })
                ],
                declarations: [
                    _app_component__WEBPACK_IMPORTED_MODULE_5__["AppComponent"],
                    _common_login_login_component__WEBPACK_IMPORTED_MODULE_10__["LoginComponent"],
                    _common_home_home_component__WEBPACK_IMPORTED_MODULE_9__["HomeComponent"],
                    _residents_dashboard_residents_dashboard_residents_component__WEBPACK_IMPORTED_MODULE_11__["DashboardResidentsComponent"],
                    _residents_enquires_enquires_component__WEBPACK_IMPORTED_MODULE_12__["EnquiresComponent"],
                    _residents_enquires_enquires_edit_enquires_edit_component__WEBPACK_IMPORTED_MODULE_13__["EnquiresEditComponent"],
                    _residents_enquires_enquires_list_enquires_list_component__WEBPACK_IMPORTED_MODULE_14__["EnquiresListComponent"],
                    _residents_templ_edit_address_edit_address_edit_component__WEBPACK_IMPORTED_MODULE_15__["AddressEditComponent"],
                    _residents_templ_edit_resident_profile_edit_resident_profile_edit_component__WEBPACK_IMPORTED_MODULE_16__["ResidentProfileEditComponent"],
                    _residents_templ_edit_social_worker_detail_edit_social_worker_detail_edit_component__WEBPACK_IMPORTED_MODULE_17__["SocialWorkerDetailEditComponent"],
                    _residents_templ_edit_care_type_edit_care_type_edit_component__WEBPACK_IMPORTED_MODULE_18__["CareTypeEditComponent"],
                    _residents_templ_edit_room_location_edit_room_location_edit_component__WEBPACK_IMPORTED_MODULE_19__["RoomLocationEditComponent"],
                    _residents_templ_edit_enquiry_misc_edit_enquiry_misc_edit_component__WEBPACK_IMPORTED_MODULE_20__["EnquiryMiscEditComponent"]
                ],
                providers: [
                    //{ provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AuthenticationService] },
                    { provide: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HTTP_INTERCEPTORS"], useClass: _helpers__WEBPACK_IMPORTED_MODULE_7__["JwtInterceptor"], multi: true },
                    { provide: _angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HTTP_INTERCEPTORS"], useClass: _helpers__WEBPACK_IMPORTED_MODULE_7__["ErrorInterceptorNoRefresh"], multi: true },
                ],
                bootstrap: [_app_component__WEBPACK_IMPORTED_MODULE_5__["AppComponent"]]
            }]
    }], null, null); })();


/***/ }),

/***/ "./src/app/common/home/home.component.ts":
/*!***********************************************!*\
  !*** ./src/app/common/home/home.component.ts ***!
  \***********************************************/
/*! exports provided: HomeComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "HomeComponent", function() { return HomeComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../services/index */ "./src/app/services/index.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");




function HomeComponent_div_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function HomeComponent_p_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", ctx_r1.user.userName, " - ", ctx_r1.user.firstName, "");
} }
class HomeComponent {
    constructor(userService) {
        this.userService = userService;
    }
    ngOnInit() {
        console.log('>>p>>p>>', this.userService.getStoreUser());
        //=== reload user on refresh =====================================
        // if user token found and userValue is null, must be reloading or refreshing the page
        // in-memory user is removed. so re-load user
        if (this.userService.hasUserToken() && this.userService.getStoreUser() == null) {
            console.log('>>sesion found. user hit F5, so get user again.');
            this.isLoading = true;
            this.userService.reloadUser().subscribe(u => {
                this.user = u;
                this.userFound = true;
                this.isLoading = false;
            });
        }
        else {
            this.user = this.userService.getStoreUser();
        }
        this.userFound = this.userService.getStoreUser() != null;
        // ================================================================
        // do other init for home page
    }
}
HomeComponent.ɵfac = function HomeComponent_Factory(t) { return new (t || HomeComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_index__WEBPACK_IMPORTED_MODULE_1__["UserService"])); };
HomeComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: HomeComponent, selectors: [["app-home"]], decls: 9, vars: 3, consts: [[1, "container", "mt-5"], [1, "row"], [1, "col-md"], [4, "ngIf"]], template: function HomeComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](3, HomeComponent_div_3_Template, 2, 0, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, " ==UserLogged: ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, HomeComponent_p_8_Template, 2, 2, "p", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isLoading);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ==UserLogged: ", ctx.user, " ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.userFound);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_2__["NgIf"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NvbW1vbi9ob21lL2hvbWUuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](HomeComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-home',
                templateUrl: './home.component.html',
                styleUrls: ['./home.component.css']
            }]
    }], function () { return [{ type: _services_index__WEBPACK_IMPORTED_MODULE_1__["UserService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/common/login/login.component.ts":
/*!*************************************************!*\
  !*** ./src/app/common/login/login.component.ts ***!
  \*************************************************/
/*! exports provided: LoginComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LoginComponent", function() { return LoginComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../services/index */ "./src/app/services/index.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");







function LoginComponent_div_14_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Username is required");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function LoginComponent_div_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LoginComponent_div_14_div_1_Template, 2, 0, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r0.f.username.errors.required);
} }
function LoginComponent_div_19_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "Password is required");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function LoginComponent_div_19_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, LoginComponent_div_19_div_1_Template, 2, 0, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.f.password.errors.required);
} }
function LoginComponent_span_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "span", 17);
} }
function LoginComponent_div_23_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 18);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](ctx_r3.error);
} }
const _c0 = function (a0) { return { "is-invalid": a0 }; };
class LoginComponent {
    constructor(formBuilder, route, router, userService) {
        this.formBuilder = formBuilder;
        this.route = route;
        this.router = router;
        this.userService = userService;
        this.loading = false;
        this.submitted = false;
        this.error = '';
    }
    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required],
            password: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_1__["Validators"].required]
        });
        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }
    // convenience getter for easy access to form fields
    get f() { return this.loginForm.controls; }
    onSubmit() {
        this.submitted = true;
        // stop here if form is invalid
        if (this.loginForm.invalid) {
            return;
        }
        this.loading = true;
        console.log('>>>in loging comp0');
        this.userService.login(this.f.username.value, this.f.password.value)
            .subscribe(data => {
            console.log('>>now fetch the carehomeuser for ', data);
            // if user found, then get CareHomeUser and set to store...
            this.userService.reloadUser();
            this.loading = false;
            this.router.navigate([this.returnUrl]);
        }, error => {
            console.log('>>>Error:', error);
            this.loading = false;
        });
    }
}
LoginComponent.ɵfac = function LoginComponent_Factory(t) { return new (t || LoginComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_2__["ActivatedRoute"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_2__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_index__WEBPACK_IMPORTED_MODULE_3__["UserService"])); };
LoginComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: LoginComponent, selectors: [["app-login"]], decls: 24, vars: 12, consts: [[1, "col-md-6", "offset-md-3", "mt-5"], [1, "alert", "alert-info"], [1, "card"], [1, "card-header"], [1, "card-body"], [3, "formGroup", "ngSubmit"], [1, "form-group"], ["for", "username"], ["type", "text", "formControlName", "username", 1, "form-control", 3, "ngClass"], ["class", "invalid-feedback", 4, "ngIf"], ["for", "password"], ["type", "password", "formControlName", "password", 1, "form-control", 3, "ngClass"], [1, "btn", "btn-primary", 3, "disabled"], ["class", "spinner-border spinner-border-sm mr-1", 4, "ngIf"], ["class", "alert alert-danger mt-3 mb-0", 4, "ngIf"], [1, "invalid-feedback"], [4, "ngIf"], [1, "spinner-border", "spinner-border-sm", "mr-1"], [1, "alert", "alert-danger", "mt-3", "mb-0"]], template: function LoginComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " Username: test");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "br");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, " Password: test ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "h4", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Angular 9 JWT Auth with Refresh Tokens");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "form", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("ngSubmit", function LoginComponent_Template_form_ngSubmit_9_listener() { return ctx.onSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "label", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "Username");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "input", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](14, LoginComponent_div_14_Template, 2, 1, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "label", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Password");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](18, "input", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](19, LoginComponent_div_19_Template, 2, 1, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "button", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, LoginComponent_span_21_Template, 1, 0, "span", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, " Login ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](23, LoginComponent_div_23_Template, 2, 1, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.loginForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](8, _c0, ctx.submitted && ctx.f.username.errors));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.submitted && ctx.f.username.errors);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngClass", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](10, _c0, ctx.submitted && ctx.f.password.errors));
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.submitted && ctx.f.password.errors);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("disabled", ctx.loading);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.loading);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.error);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgClass"], _angular_common__WEBPACK_IMPORTED_MODULE_4__["NgIf"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2NvbW1vbi9sb2dpbi9sb2dpbi5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](LoginComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-login',
                templateUrl: './login.component.html',
                styleUrls: ['./login.component.css']
            }]
    }], function () { return [{ type: _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormBuilder"] }, { type: _angular_router__WEBPACK_IMPORTED_MODULE_2__["ActivatedRoute"] }, { type: _angular_router__WEBPACK_IMPORTED_MODULE_2__["Router"] }, { type: _services_index__WEBPACK_IMPORTED_MODULE_3__["UserService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/helpers/auth-admin.guard.ts":
/*!*********************************************!*\
  !*** ./src/app/helpers/auth-admin.guard.ts ***!
  \*********************************************/
/*! exports provided: AuthAdminGuard */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AuthAdminGuard", function() { return AuthAdminGuard; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../services/index */ "./src/app/services/index.ts");




class AuthAdminGuard {
    constructor(router, userService) {
        this.router = router;
        this.userService = userService;
    }
    canActivate(route, state) {
        if (!this.userService.hasUserToken()) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        }
        // if (this.userService.userRoleValues.length === 0) {
        //     this.router.navigate([''], { });
        // } else {
        //     return this.userService.isInRole('Admin');
        // }
        return false; //temp
    }
}
AuthAdminGuard.ɵfac = function AuthAdminGuard_Factory(t) { return new (t || AuthAdminGuard)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_services_index__WEBPACK_IMPORTED_MODULE_2__["UserService"])); };
AuthAdminGuard.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: AuthAdminGuard, factory: AuthAdminGuard.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AuthAdminGuard, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{ providedIn: 'root' }]
    }], function () { return [{ type: _angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"] }, { type: _services_index__WEBPACK_IMPORTED_MODULE_2__["UserService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/helpers/auth-residents.guard.ts":
/*!*************************************************!*\
  !*** ./src/app/helpers/auth-residents.guard.ts ***!
  \*************************************************/
/*! exports provided: AuthResidentsGuard */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AuthResidentsGuard", function() { return AuthResidentsGuard; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../services/index */ "./src/app/services/index.ts");




// import 'rxjs/add/observable/of';
// import 'rxjs/add/operator/delay';
class AuthResidentsGuard {
    constructor(router, userService) {
        this.router = router;
        this.userService = userService;
    }
    canActivate(route, state) {
        if (!this.userService.hasUserToken()) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        }
        // console.log('>>>==', this.userService.userRoleValues);
        // if (this.userService.userRoleValues.length === 0) {
        //     // this.router.navigate([''], { });
        //     this.userService.reloadUser().subscribe(s => {
        //         if (this.userService.isInRole('Manager')){
        //             this.router.navigate([state.url], { });
        //         }
        //     });
        // } else {
        //     return this.userService.isInRole('Manager');
        // }
        return true; //temp
    }
}
AuthResidentsGuard.ɵfac = function AuthResidentsGuard_Factory(t) { return new (t || AuthResidentsGuard)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_services_index__WEBPACK_IMPORTED_MODULE_2__["UserService"])); };
AuthResidentsGuard.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: AuthResidentsGuard, factory: AuthResidentsGuard.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AuthResidentsGuard, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{ providedIn: 'root' }]
    }], function () { return [{ type: _angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"] }, { type: _services_index__WEBPACK_IMPORTED_MODULE_2__["UserService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/helpers/auth.guard.ts":
/*!***************************************!*\
  !*** ./src/app/helpers/auth.guard.ts ***!
  \***************************************/
/*! exports provided: AuthGuard */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AuthGuard", function() { return AuthGuard; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../services/index */ "./src/app/services/index.ts");




class AuthGuard {
    constructor(router, userService) {
        this.router = router;
        this.userService = userService;
    }
    canActivate(route, state) {
        if (!this.userService.hasUserToken()) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        }
        return true;
    }
}
AuthGuard.ɵfac = function AuthGuard_Factory(t) { return new (t || AuthGuard)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_services_index__WEBPACK_IMPORTED_MODULE_2__["UserService"])); };
AuthGuard.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: AuthGuard, factory: AuthGuard.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AuthGuard, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{ providedIn: 'root' }]
    }], function () { return [{ type: _angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"] }, { type: _services_index__WEBPACK_IMPORTED_MODULE_2__["UserService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/helpers/error.interceptor.no.refresh.ts":
/*!*********************************************************!*\
  !*** ./src/app/helpers/error.interceptor.no.refresh.ts ***!
  \*********************************************************/
/*! exports provided: ErrorInterceptorNoRefresh */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ErrorInterceptorNoRefresh", function() { return ErrorInterceptorNoRefresh; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../services/index */ "./src/app/services/index.ts");





class ErrorInterceptorNoRefresh {
    constructor(authenticationService) {
        this.authenticationService = authenticationService;
        this.isRefreshing = false;
        this.refreshTokenSubject = new rxjs__WEBPACK_IMPORTED_MODULE_1__["BehaviorSubject"](null);
    }
    intercept(request, next) {
        return next.handle(request).pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(err => {
            console.log('>>>errintnofresh:token expired.', err.status);
            if ([401, 403].includes(err.status)) {
                console.log('>>>status includes [401, 403]:');
                if (err && err.error) {
                    console.log('>>>', err.error.message);
                    if (err.error.message && err.error.message.includes('INVALID TOKEN:')) {
                        this.authenticationService.logout();
                    }
                }
            }
            const error = (err && err.error && err.error.message) || err.statusText;
            console.error('>>>>=-', err);
            return Object(rxjs__WEBPACK_IMPORTED_MODULE_1__["throwError"])(error);
        }));
    }
}
ErrorInterceptorNoRefresh.ɵfac = function ErrorInterceptorNoRefresh_Factory(t) { return new (t || ErrorInterceptorNoRefresh)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_services_index__WEBPACK_IMPORTED_MODULE_3__["AuthenticationService"])); };
ErrorInterceptorNoRefresh.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: ErrorInterceptorNoRefresh, factory: ErrorInterceptorNoRefresh.ɵfac });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ErrorInterceptorNoRefresh, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"]
    }], function () { return [{ type: _services_index__WEBPACK_IMPORTED_MODULE_3__["AuthenticationService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/helpers/index.ts":
/*!**********************************!*\
  !*** ./src/app/helpers/index.ts ***!
  \**********************************/
/*! exports provided: AuthGuard, AuthResidentsGuard, AuthAdminGuard, ErrorInterceptorNoRefresh, JwtInterceptor */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _auth_guard__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./auth.guard */ "./src/app/helpers/auth.guard.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "AuthGuard", function() { return _auth_guard__WEBPACK_IMPORTED_MODULE_0__["AuthGuard"]; });

/* harmony import */ var _auth_residents_guard__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./auth-residents.guard */ "./src/app/helpers/auth-residents.guard.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "AuthResidentsGuard", function() { return _auth_residents_guard__WEBPACK_IMPORTED_MODULE_1__["AuthResidentsGuard"]; });

/* harmony import */ var _auth_admin_guard__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./auth-admin.guard */ "./src/app/helpers/auth-admin.guard.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "AuthAdminGuard", function() { return _auth_admin_guard__WEBPACK_IMPORTED_MODULE_2__["AuthAdminGuard"]; });

/* harmony import */ var _error_interceptor_no_refresh__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./error.interceptor.no.refresh */ "./src/app/helpers/error.interceptor.no.refresh.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "ErrorInterceptorNoRefresh", function() { return _error_interceptor_no_refresh__WEBPACK_IMPORTED_MODULE_3__["ErrorInterceptorNoRefresh"]; });

/* harmony import */ var _jwt_interceptor__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./jwt.interceptor */ "./src/app/helpers/jwt.interceptor.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "JwtInterceptor", function() { return _jwt_interceptor__WEBPACK_IMPORTED_MODULE_4__["JwtInterceptor"]; });

//export * from './app.initializer';


// export * from './auth-residents.resolver';

// export * from './error.interceptor';

//export * from './fake-backend';



/***/ }),

/***/ "./src/app/helpers/jwt.interceptor.ts":
/*!********************************************!*\
  !*** ./src/app/helpers/jwt.interceptor.ts ***!
  \********************************************/
/*! exports provided: JwtInterceptor */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "JwtInterceptor", function() { return JwtInterceptor; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../environments/environment */ "./src/environments/environment.ts");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../services/index */ "./src/app/services/index.ts");

 // '@environments/environment';


class JwtInterceptor {
    constructor(authService) {
        this.authService = authService;
    }
    intercept(request, next) {
        console.log('>>>>', request.url);
        // add auth header with jwt if user is logged in and request is to the api url
        const isapiDomainUrl = request.url.startsWith(_environments_environment__WEBPACK_IMPORTED_MODULE_1__["environment"].apiDomainUrl);
        const token = this.authService.getToken();
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`,
            },
        });
        return next.handle(request);
    }
}
JwtInterceptor.ɵfac = function JwtInterceptor_Factory(t) { return new (t || JwtInterceptor)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_services_index__WEBPACK_IMPORTED_MODULE_2__["AuthenticationService"])); };
JwtInterceptor.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: JwtInterceptor, factory: JwtInterceptor.ɵfac });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](JwtInterceptor, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"]
    }], function () { return [{ type: _services_index__WEBPACK_IMPORTED_MODULE_2__["AuthenticationService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/helpers/store.ts":
/*!**********************************!*\
  !*** ./src/app/helpers/store.ts ***!
  \**********************************/
/*! exports provided: Store */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Store", function() { return Store; });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");

class Store {
    constructor(initialState) {
        this._state$ = new rxjs__WEBPACK_IMPORTED_MODULE_0__["BehaviorSubject"](initialState);
    }
    getValue() {
        return this._state$.getValue();
    }
    getState() {
        return this._state$.asObservable();
    }
    setState(nextState) {
        this._state$.next(nextState);
    }
}


/***/ }),

/***/ "./src/app/residents/dashboard-residents/dashboard-residents.component.ts":
/*!********************************************************************************!*\
  !*** ./src/app/residents/dashboard-residents/dashboard-residents.component.ts ***!
  \********************************************************************************/
/*! exports provided: DashboardResidentsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DashboardResidentsComponent", function() { return DashboardResidentsComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../services/index */ "./src/app/services/index.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");





function DashboardResidentsComponent_p_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate2"](" ", ctx_r0.user.userName, " - ", ctx_r0.user.firstName, "");
} }
class DashboardResidentsComponent {
    constructor(router, userService) {
        this.router = router;
        this.userService = userService;
    }
    ngOnInit() {
        console.log('>>>dash-res');
        //=== reload user on refresh =====================================
        // if user token found and userValue is null, must be reloading or refreshing the page
        // in-memory user is removed. so re-load user
        if (this.userService.hasUserToken() && this.userService.getStoreUser() == null) {
            this.userService.reloadUser().subscribe(u => {
                this.user = u;
                this.userFound = true;
            });
        }
        else {
            this.user = this.userService.getStoreUser();
        }
        this.userFound = this.userService.getStoreUser() != null;
        // ================================================================
    }
    navToEnquires() {
        this.router.navigate(['/enquires', {}]);
    }
}
DashboardResidentsComponent.ɵfac = function DashboardResidentsComponent_Factory(t) { return new (t || DashboardResidentsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services_index__WEBPACK_IMPORTED_MODULE_2__["UserService"])); };
DashboardResidentsComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: DashboardResidentsComponent, selectors: [["app-dashboard-residents"]], decls: 22, vars: 1, consts: [[4, "ngIf"], [1, "container", "mt-5"], [1, "row"], [1, "col-md"], [1, "card", 2, "width", "18rem", "height", "11rem"], [1, "card-body"], [1, "card-title"], [1, "card-text"], [1, "btn", "btn-primary", 3, "click"]], template: function DashboardResidentsComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](0, "\n==>>==");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, DashboardResidentsComponent_p_1_Template, 2, 2, "p", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "h5", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Enquires");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "p", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "Manage enquiry to admission");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "button", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DashboardResidentsComponent_Template_button_click_11_listener() { return ctx.navToEnquires(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "Enquires");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "h5", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Residents");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "p", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Manage residents information");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "button", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function DashboardResidentsComponent_Template_button_click_20_listener() { return ctx.navToEnquires(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Residents");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.userFound);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy9kYXNoYm9hcmQtcmVzaWRlbnRzL2Rhc2hib2FyZC1yZXNpZGVudHMuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](DashboardResidentsComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-dashboard-residents',
                templateUrl: './dashboard-residents.component.html',
                styleUrls: ['./dashboard-residents.component.css']
            }]
    }], function () { return [{ type: _angular_router__WEBPACK_IMPORTED_MODULE_1__["Router"] }, { type: _services_index__WEBPACK_IMPORTED_MODULE_2__["UserService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/residents/enquires/enquires-edit/enquires-edit.component.ts":
/*!*****************************************************************************!*\
  !*** ./src/app/residents/enquires/enquires-edit/enquires-edit.component.ts ***!
  \*****************************************************************************/
/*! exports provided: EnquiresEditComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EnquiresEditComponent", function() { return EnquiresEditComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _models_index__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../models/index */ "./src/app/residents/models/index.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _services__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../services */ "./src/app/residents/services/index.ts");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _templ_edit_resident_profile_edit_resident_profile_edit_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../../templ-edit/resident-profile-edit/resident-profile-edit.component */ "./src/app/residents/templ-edit/resident-profile-edit/resident-profile-edit.component.ts");
/* harmony import */ var _templ_edit_address_edit_address_edit_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../../templ-edit/address-edit/address-edit.component */ "./src/app/residents/templ-edit/address-edit/address-edit.component.ts");
/* harmony import */ var _templ_edit_social_worker_detail_edit_social_worker_detail_edit_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ../../templ-edit/social-worker-detail-edit/social-worker-detail-edit.component */ "./src/app/residents/templ-edit/social-worker-detail-edit/social-worker-detail-edit.component.ts");
/* harmony import */ var _templ_edit_care_type_edit_care_type_edit_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ../../templ-edit/care-type-edit/care-type-edit.component */ "./src/app/residents/templ-edit/care-type-edit/care-type-edit.component.ts");
/* harmony import */ var _templ_edit_room_location_edit_room_location_edit_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ../../templ-edit/room-location-edit/room-location-edit.component */ "./src/app/residents/templ-edit/room-location-edit/room-location-edit.component.ts");
/* harmony import */ var _templ_edit_enquiry_misc_edit_enquiry_misc_edit_component__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ../../templ-edit/enquiry-misc-edit/enquiry-misc-edit.component */ "./src/app/residents/templ-edit/enquiry-misc-edit/enquiry-misc-edit.component.ts");
















function EnquiresEditComponent_ng_template_4_label_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "label", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"]("Reference Id: ", ctx_r7.enquiryResident.referenceId, "");
} }
function EnquiresEditComponent_ng_template_4_Template(rf, ctx) { if (rf & 1) {
    const _r9 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](1, EnquiresEditComponent_ng_template_4_label_1_Template, 2, 1, "label", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "resident-profile-edit", 18);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("foreNameUpdated", function EnquiresEditComponent_ng_template_4_Template_resident_profile_edit_foreNameUpdated_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r8.onForeNameUpdated($event); })("surNameUpdated", function EnquiresEditComponent_ng_template_4_Template_resident_profile_edit_surNameUpdated_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r10.onSurNameUpdated($event); })("middleNameUpdated", function EnquiresEditComponent_ng_template_4_Template_resident_profile_edit_middleNameUpdated_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r11.onMiddleNameUpdated($event); })("dobUpdated", function EnquiresEditComponent_ng_template_4_Template_resident_profile_edit_dobUpdated_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r12.onDobUpdated($event); })("genderUpdated", function EnquiresEditComponent_ng_template_4_Template_resident_profile_edit_genderUpdated_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r13.onGenderUpdated($event); })("martialStatusUpdated", function EnquiresEditComponent_ng_template_4_Template_resident_profile_edit_martialStatusUpdated_2_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r9); const ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r14.onMartialStatusUpdated($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx_r1.enquiryResident.referenceId);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("foreName", ctx_r1.enquiryResident.foreName)("surName", ctx_r1.enquiryResident.surName)("middleName", ctx_r1.enquiryResident.middleName)("dob", ctx_r1.enquiryResident.dob)("gender", ctx_r1.enquiryResident.gender)("martialStatus", ctx_r1.enquiryResident.martialStatus);
} }
function EnquiresEditComponent_ng_template_6_Template(rf, ctx) { if (rf & 1) {
    const _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "address-edit", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("street1Updated", function EnquiresEditComponent_ng_template_6_Template_address_edit_street1Updated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16); const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r15.onStreet1Updated($event); })("street2Updated", function EnquiresEditComponent_ng_template_6_Template_address_edit_street2Updated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16); const ctx_r17 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r17.onStreet2Updated($event); })("cityUpdated", function EnquiresEditComponent_ng_template_6_Template_address_edit_cityUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16); const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r18.onCityUpdated($event); })("countyUpdated", function EnquiresEditComponent_ng_template_6_Template_address_edit_countyUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16); const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r19.onCountyUpdated($event); })("postCodeUpdated", function EnquiresEditComponent_ng_template_6_Template_address_edit_postCodeUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r16); const ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r20.onPostCodeUpdated($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("street1", ctx_r2.enquiryResident.address.street1)("street2", ctx_r2.enquiryResident.address.street2)("city", ctx_r2.enquiryResident.address.city)("county", ctx_r2.enquiryResident.address.county)("postCode", ctx_r2.enquiryResident.address.postCode);
} }
function EnquiresEditComponent_ng_template_8_Template(rf, ctx) { if (rf & 1) {
    const _r22 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "social-worker-detail-edit", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("swForeNameUpdated", function EnquiresEditComponent_ng_template_8_Template_social_worker_detail_edit_swForeNameUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22); const ctx_r21 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r21.onSwForeNameUpdated($event); })("swSurNameUpdated", function EnquiresEditComponent_ng_template_8_Template_social_worker_detail_edit_swSurNameUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22); const ctx_r23 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r23.onSwSurNameUpdated($event); })("swPhoneNumberUpdated", function EnquiresEditComponent_ng_template_8_Template_social_worker_detail_edit_swPhoneNumberUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22); const ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r24.onSwPhoneNumberUpdated($event); })("swEmailUpdated", function EnquiresEditComponent_ng_template_8_Template_social_worker_detail_edit_swEmailUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r22); const ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r25.onSwEmailUpdated($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("foreName", ctx_r3.enquiryResident.socialWorker.foreName)("surName", ctx_r3.enquiryResident.socialWorker.surName)("phoneNumber", ctx_r3.enquiryResident.socialWorker.phoneNumber)("email", ctx_r3.enquiryResident.socialWorker.email);
} }
function EnquiresEditComponent_ng_template_10_Template(rf, ctx) { if (rf & 1) {
    const _r27 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "care-type-edit", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("careCategoryUpdated", function EnquiresEditComponent_ng_template_10_Template_care_type_edit_careCategoryUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27); const ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r26.onCareCategoryUpdated($event); })("careNeedsUpdated", function EnquiresEditComponent_ng_template_10_Template_care_type_edit_careNeedsUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27); const ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r28.onCareNeedsUpdated($event); })("stayTypeUpdated", function EnquiresEditComponent_ng_template_10_Template_care_type_edit_stayTypeUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27); const ctx_r29 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r29.onStayTypeUpdated($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "room-location-edit", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("roomLocationUpdated", function EnquiresEditComponent_ng_template_10_Template_room_location_edit_roomLocationUpdated_1_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27); const ctx_r30 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r30.onRoomLocationUpdated($event); })("roomNumberUpdated", function EnquiresEditComponent_ng_template_10_Template_room_location_edit_roomNumberUpdated_1_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r27); const ctx_r31 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r31.onRoomNumberUpdated($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("careCategory", ctx_r4.enquiryResident.careCategory)("careNeeds", ctx_r4.enquiryResident.careNeeds)("stayType", ctx_r4.enquiryResident.stayType);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("roomLocations", ctx_r4.roomLocations)("reservedRoomLocation", ctx_r4.enquiryResident.reservedRoomLocation)("reservedRoomNumber", ctx_r4.enquiryResident.reservedRoomNumber);
} }
function EnquiresEditComponent_ng_template_12_Template(rf, ctx) { if (rf & 1) {
    const _r33 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "enquiry-misc-edit", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("moveInDateUpdated", function EnquiresEditComponent_ng_template_12_Template_enquiry_misc_edit_moveInDateUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r33); const ctx_r32 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r32.onMoveInDateUpdated($event); })("familyHomeVisitDateUpdated", function EnquiresEditComponent_ng_template_12_Template_enquiry_misc_edit_familyHomeVisitDateUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r33); const ctx_r34 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r34.onFamilyHomeVisitDateUpdated($event); })("enquiryDateUpdated", function EnquiresEditComponent_ng_template_12_Template_enquiry_misc_edit_enquiryDateUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r33); const ctx_r35 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r35.onEnquiryDateUpdated($event); })("responseDateUpdated", function EnquiresEditComponent_ng_template_12_Template_enquiry_misc_edit_responseDateUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r33); const ctx_r36 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r36.onResponseDateUpdated($event); })("responseUpdated", function EnquiresEditComponent_ng_template_12_Template_enquiry_misc_edit_responseUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r33); const ctx_r37 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r37.onResponseUpdated($event); })("commentsUpdated", function EnquiresEditComponent_ng_template_12_Template_enquiry_misc_edit_commentsUpdated_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r33); const ctx_r38 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"](); return ctx_r38.onCommentsUpdated($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("moveInDate", ctx_r5.enquiryResident.moveInDate)("familyHomeVisitDate", ctx_r5.enquiryResident.familyHomeVisitDate)("enquiryDate", ctx_r5.enquiryResident.enquiryDate)("responseDate", ctx_r5.enquiryResident.responseDate)("response", ctx_r5.enquiryResident.comments);
} }
function EnquiresEditComponent_option_21_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const status_r39 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", status_r39.key);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", status_r39.value, " ");
} }
class EnquiresEditComponent {
    constructor(_Activatedroute, _router, enquiresService) {
        this._Activatedroute = _Activatedroute;
        this._router = _router;
        this.enquiresService = enquiresService;
        this._enquiryResident = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](Object(_models_index__WEBPACK_IMPORTED_MODULE_1__["createInstanceofEnquiryResident"])());
        this.enquiryResident$ = this._enquiryResident.asObservable();
        this.roomLocations = [
            { id: 1, name: "Main Floor", rooms: [
                    { id: 10, locationId: 1, name: "R-1" },
                    { id: 11, locationId: 1, name: "R-2" },
                ] },
            { id: 2, name: "First Floor", rooms: [
                    { id: 20, locationId: 2, name: "R-20" },
                    { id: 21, locationId: 2, name: "R-21" },
                    { id: 22, locationId: 2, name: "R-22" },
                ] },
        ];
        this.statuses = [
            { key: 'active', value: 'Active' },
            { key: 'admit', value: 'Admit' },
            { key: 'closed', value: 'Closed' }
        ];
        this.enquiryEditForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroup"]({
            status: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''),
        });
        this.activeIds = ['panel-profile']; // default to open profile //, 'panel-address']
    }
    ngOnInit() {
        // init subscription for observables
        this._enquiryResident.subscribe((enq) => (this.enquiryResident = enq));
        this._Activatedroute.paramMap.subscribe((params) => {
            console.log(params);
            if (params && params.get('referenceId')) {
                let referenceId = params.get('referenceId');
                setTimeout(() => {
                    this.loadByReferenceId(referenceId);
                }, 2000);
            }
            else {
                // add new enquiry. get home details into drop down box
                // onchange of home, load room locations and room numbers
            }
        });
    }
    loadByReferenceId(referenceId) {
        if (referenceId === '' || referenceId === null) {
            throw new console.error('Reference not found');
        }
        this.enquiresService.loadEnquiryByReferenceId(referenceId).subscribe({
            next: (data) => {
                console.log('>>>>', data);
                this._enquiryResident.next(data);
                // once data is available setup THIS form related with data
                this.setupEnquiryEditForm(data);
            },
            error: (error) => {
                console.log('ERROR:', error);
            },
        });
    }
    loadCareHomeDetails() {
    }
    setupEnquiryEditForm(data) {
        if (data.status) {
            this.enquiryEditForm.controls['status'].setValue(data.status);
        }
    }
    //======================================================
    // === Name, Surname etc profile
    onForeNameUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { foreName: event.target.value });
        // this._enquiryResident.next(newState);
        this.updateState(newState);
    }
    onSurNameUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { surName: event.target.value });
        this.updateState(newState);
    }
    onMiddleNameUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { middleName: event.target.value });
        this.updateState(newState);
    }
    onDobUpdated(event) {
        console.log('InEnqEditForm=', event); // {year: 1962, month: 12, day: 16}
        if (event) {
            // const d = new Date(event.year, event.month-1, event.day);
            let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { dob: this.convertToJsDate(event) });
            this.updateState(newState);
        }
    }
    onGenderUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { gender: event.target.value });
        this.updateState(newState);
    }
    onMartialStatusUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { martialStatus: event.target.value });
        this.updateState(newState);
    }
    // =================================================
    // === address info
    onStreet1Updated(event) {
        console.log('InEnqEditForm=', event);
        let address = Object.assign(Object.assign({}, this._enquiryResident.getValue().address), { street1: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { address: address });
        this.updateState(newState);
    }
    onStreet2Updated(event) {
        let address = Object.assign(Object.assign({}, this._enquiryResident.getValue().address), { street2: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { address: address });
        this.updateState(newState);
    }
    onCityUpdated(event) {
        let address = Object.assign(Object.assign({}, this._enquiryResident.getValue().address), { city: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { address: address });
        this.updateState(newState);
    }
    onCountyUpdated(event) {
        let address = Object.assign(Object.assign({}, this._enquiryResident.getValue().address), { county: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { address: address });
        this.updateState(newState);
    }
    onPostCodeUpdated(event) {
        let address = Object.assign(Object.assign({}, this._enquiryResident.getValue().address), { postCode: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { address: address });
        this.updateState(newState);
    }
    // =================================================
    // === social worker info
    onSwForeNameUpdated(event) {
        let sw = Object.assign(Object.assign({}, this._enquiryResident.getValue().socialWorker), { foreName: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { socialWorker: sw });
        this.updateState(newState);
    }
    onSwSurNameUpdated(event) {
        let sw = Object.assign(Object.assign({}, this._enquiryResident.getValue().socialWorker), { surName: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { socialWorker: sw });
        this.updateState(newState);
    }
    onSwPhoneNumberUpdated(event) {
        let sw = Object.assign(Object.assign({}, this._enquiryResident.getValue().socialWorker), { phoneNumber: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { socialWorker: sw });
        this.updateState(newState);
    }
    onSwEmailUpdated(event) {
        let sw = Object.assign(Object.assign({}, this._enquiryResident.getValue().socialWorker), { email: event.target.value });
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { socialWorker: sw });
        this.updateState(newState);
    }
    // ==================================================
    // === care type ====================================
    onCareCategoryUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { careCategory: event.target.value });
        this.updateState(newState);
    }
    onCareNeedsUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { careNeeds: event.target.value });
        this.updateState(newState);
    }
    onStayTypeUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { stayType: event.target.value });
        this.updateState(newState);
    }
    // =================================================
    // === room loc and number updated ================
    onRoomLocationUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { reservedRoomLocation: event.target.value, reservedRoomNumber: null });
        this.updateState(newState);
    }
    onRoomNumberUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { reservedRoomNumber: event.target.value });
        this.updateState(newState);
    }
    // ===============================================
    // === misc input ================================
    onMoveInDateUpdated(event) {
        if (event) {
            // const d = new Date(event.year, event.month-1, event.day);
            let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { moveInDate: this.convertToJsDate(event) });
            this.updateState(newState);
        }
    }
    onFamilyHomeVisitDateUpdated(event) {
        if (event) {
            let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { familyHomeVisitDate: this.convertToJsDate(event) });
            this.updateState(newState);
        }
    }
    onEnquiryDateUpdated(event) {
        if (event) {
            let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { enquiryDate: this.convertToJsDate(event) });
            this.updateState(newState);
        }
    }
    onResponseDateUpdated(event) {
        if (event) {
            let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { responseDate: this.convertToJsDate(event) });
            this.updateState(newState);
        }
    }
    onResponseUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { response: event.target.value });
        this.updateState(newState);
    }
    onCommentsUpdated(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { comments: event.target.value });
        this.updateState(newState);
    }
    // ==============================================
    // === status and submit ========================
    onStatusChange(event) {
        let newState = Object.assign(Object.assign({}, this._enquiryResident.getValue()), { status: event.target.value });
        this.updateState(newState);
    }
    onSubmit() {
        // validation...
        console.log('>>>Ready to submit', this.enquiryResident);
    }
    onCancel() {
        this._router.navigate(['/enquires', {}]);
    }
    // =============================================
    // === helper methods ==========================
    updateState(state) {
        this._enquiryResident.next(state);
    }
    convertToJsDate(event) {
        return new Date(event.year, event.month - 1, event.day);
    }
}
EnquiresEditComponent.ɵfac = function EnquiresEditComponent_Factory(t) { return new (t || EnquiresEditComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_4__["ActivatedRoute"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_4__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services__WEBPACK_IMPORTED_MODULE_5__["EnquiresService"])); };
EnquiresEditComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: EnquiresEditComponent, selectors: [["app-enquires-edit"]], decls: 28, vars: 3, consts: [[1, "mt-5"], [3, "activeIds"], ["acc", "ngbAccordion"], ["id", "panel-profile", "title", "Profile"], ["ngbPanelContent", ""], ["id", "panel-address", "title", "Address"], ["title", "Social Worker"], ["title", "Care Type Room Details"], ["title", "Other"], [3, "formGroup"], [1, "form-group"], ["for", "status"], ["name", "status", "id", "status", "formControlName", "status", 1, "custom-select", 3, "change"], ["value", "", "selected", ""], [3, "value", 4, "ngFor", "ngForOf"], [1, "btn", "btn-secondary", 3, "click"], [1, "btn", "btn-primary", "ml-5", 3, "click"], ["for", "referenceId", 4, "ngIf"], [3, "foreName", "surName", "middleName", "dob", "gender", "martialStatus", "foreNameUpdated", "surNameUpdated", "middleNameUpdated", "dobUpdated", "genderUpdated", "martialStatusUpdated"], ["for", "referenceId"], [3, "street1", "street2", "city", "county", "postCode", "street1Updated", "street2Updated", "cityUpdated", "countyUpdated", "postCodeUpdated"], [3, "foreName", "surName", "phoneNumber", "email", "swForeNameUpdated", "swSurNameUpdated", "swPhoneNumberUpdated", "swEmailUpdated"], [3, "careCategory", "careNeeds", "stayType", "careCategoryUpdated", "careNeedsUpdated", "stayTypeUpdated"], [3, "roomLocations", "reservedRoomLocation", "reservedRoomNumber", "roomLocationUpdated", "roomNumberUpdated"], [3, "moveInDate", "familyHomeVisitDate", "enquiryDate", "responseDate", "response", "moveInDateUpdated", "familyHomeVisitDateUpdated", "enquiryDateUpdated", "responseDateUpdated", "responseUpdated", "commentsUpdated"], [3, "value"]], template: function EnquiresEditComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "ngb-accordion", 1, 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "ngb-panel", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, EnquiresEditComponent_ng_template_4_Template, 3, 7, "ng-template", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "ngb-panel", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](6, EnquiresEditComponent_ng_template_6_Template, 1, 5, "ng-template", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "ngb-panel", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](8, EnquiresEditComponent_ng_template_8_Template, 1, 4, "ng-template", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "ngb-panel", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](10, EnquiresEditComponent_ng_template_10_Template, 2, 6, "ng-template", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "ngb-panel", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](12, EnquiresEditComponent_ng_template_12_Template, 1, 5, "ng-template", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "form", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "label", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "select", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function EnquiresEditComponent_Template_select_change_18_listener($event) { return ctx.onStatusChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "option", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "Choose...");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](21, EnquiresEditComponent_option_21_Template, 2, 2, "option", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "button", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function EnquiresEditComponent_Template_button_click_24_listener() { return ctx.onCancel(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](25, "Cancel");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "button", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function EnquiresEditComponent_Template_button_click_26_listener() { return ctx.onSubmit(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](27, "Submit");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("activeIds", ctx.activeIds);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.enquiryEditForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.statuses);
    } }, directives: [_ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__["NgbAccordion"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__["NgbPanel"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_6__["NgbPanelContent"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_2__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgForOf"], _angular_common__WEBPACK_IMPORTED_MODULE_7__["NgIf"], _templ_edit_resident_profile_edit_resident_profile_edit_component__WEBPACK_IMPORTED_MODULE_8__["ResidentProfileEditComponent"], _templ_edit_address_edit_address_edit_component__WEBPACK_IMPORTED_MODULE_9__["AddressEditComponent"], _templ_edit_social_worker_detail_edit_social_worker_detail_edit_component__WEBPACK_IMPORTED_MODULE_10__["SocialWorkerDetailEditComponent"], _templ_edit_care_type_edit_care_type_edit_component__WEBPACK_IMPORTED_MODULE_11__["CareTypeEditComponent"], _templ_edit_room_location_edit_room_location_edit_component__WEBPACK_IMPORTED_MODULE_12__["RoomLocationEditComponent"], _templ_edit_enquiry_misc_edit_enquiry_misc_edit_component__WEBPACK_IMPORTED_MODULE_13__["EnquiryMiscEditComponent"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy9lbnF1aXJlcy9lbnF1aXJlcy1lZGl0L2VucXVpcmVzLWVkaXQuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](EnquiresEditComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-enquires-edit',
                templateUrl: './enquires-edit.component.html',
                styleUrls: ['./enquires-edit.component.css'],
            }]
    }], function () { return [{ type: _angular_router__WEBPACK_IMPORTED_MODULE_4__["ActivatedRoute"] }, { type: _angular_router__WEBPACK_IMPORTED_MODULE_4__["Router"] }, { type: _services__WEBPACK_IMPORTED_MODULE_5__["EnquiresService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/residents/enquires/enquires-list/enquires-list.component.ts":
/*!*****************************************************************************!*\
  !*** ./src/app/residents/enquires/enquires-list/enquires-list.component.ts ***!
  \*****************************************************************************/
/*! exports provided: EnquiresListComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EnquiresListComponent", function() { return EnquiresListComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");




function EnquiresListComponent_div_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](2, "date");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind1"](2, 1, ctx_r0.enquiryResident.dob));
} }
const _c0 = function (a1) { return ["/enquires-edit", a1]; };
class EnquiresListComponent {
    constructor() { }
    ngOnInit() {
    }
    remove(referenceId) {
        console.log('>>removing>>', referenceId);
    }
}
EnquiresListComponent.ɵfac = function EnquiresListComponent_Factory(t) { return new (t || EnquiresListComponent)(); };
EnquiresListComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: EnquiresListComponent, selectors: [["enquires-list"]], inputs: { enquiryResident: "enquiryResident" }, decls: 14, vars: 7, consts: [[1, "container"], [1, "row"], [1, "col-sm"], [4, "ngIf"], [3, "routerLink"]], template: function EnquiresListComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, EnquiresListComponent_div_9_Template, 3, 3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "Edit");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](13, "hr");
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ctx.enquiryResident.referenceId, " ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ctx.enquiryResident.foreName, " ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ctx.enquiryResident.surName, " ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.enquiryResident.dob);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("routerLink", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpureFunction1"](5, _c0, ctx.enquiryResident.referenceId));
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["NgIf"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterLinkWithHref"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["DatePipe"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy9lbnF1aXJlcy9lbnF1aXJlcy1saXN0L2VucXVpcmVzLWxpc3QuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](EnquiresListComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'enquires-list',
                templateUrl: './enquires-list.component.html',
                styleUrls: ['./enquires-list.component.css']
            }]
    }], function () { return []; }, { enquiryResident: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }] }); })();


/***/ }),

/***/ "./src/app/residents/enquires/enquires.component.ts":
/*!**********************************************************!*\
  !*** ./src/app/residents/enquires/enquires.component.ts ***!
  \**********************************************************/
/*! exports provided: EnquiresComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EnquiresComponent", function() { return EnquiresComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _services__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../services */ "./src/app/residents/services/index.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");
/* harmony import */ var _enquires_list_enquires_list_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./enquires-list/enquires-list.component */ "./src/app/residents/enquires/enquires-list/enquires-list.component.ts");






function EnquiresComponent_div_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "loading...");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} }
function EnquiresComponent_enquires_list_26_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](0, "enquires-list", 12);
} if (rf & 2) {
    const enquiry_r2 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("enquiryResident", enquiry_r2);
} }
;
//import { FormBuilder, FormGroup, Validators } from '@angular/forms';
class EnquiresComponent {
    // enquiryResidents: EnquiryResident;
    //enquiryForm: FormGroup;
    constructor(enquiresService, router) {
        this.enquiresService = enquiresService;
        this.router = router;
        //this.enquiryForm = this.formBuilder.group({
        // 'todo': ['', Validators.required]
        //});
    }
    ngOnInit() {
        //console.log('ngonit-enq');
        this.enquiryResidents$ = this.enquiresService.getState();
        // console.log('>>>', this.enquiresService.getValue());
        // console.log('>>>', this.enquiryResidents$);
        if (this.enquiresService.getValue() && this.enquiresService.getValue().length === 0) {
            this.isLoading = true;
            this.enquiresService.loadEnquiresAll()
                .subscribe(data => {
                console.log(data);
                this.isLoading = false;
            }, error => { console.log('>>>Error getting all enquires'); });
        }
    }
    navToAddEnquiry() {
        this.router.navigate(['/enquires-add']);
    }
}
EnquiresComponent.ɵfac = function EnquiresComponent_Factory(t) { return new (t || EnquiresComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_services__WEBPACK_IMPORTED_MODULE_1__["EnquiresService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_2__["Router"])); };
EnquiresComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: EnquiresComponent, selectors: [["app-enquires"]], decls: 28, vars: 4, consts: [[1, "container", "mt-5"], [1, "container"], [1, "row"], [1, "col-md"], [4, "ngIf"], [1, "float-right"], [1, "btn", "btn-primary", 3, "click"], [1, "container", "mt-3"], [1, "col-sm"], [1, "h6"], [1, "row", "mt-2"], [3, "enquiryResident", 4, "ngFor", "ngForOf"], [3, "enquiryResident"]], template: function EnquiresComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](4, EnquiresComponent_div_4_Template, 2, 0, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "button", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function EnquiresComponent_Template_button_click_7_listener() { return ctx.navToAddEnquiry(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Add");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "ReferenceId");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](16, "Fore Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "Sur Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "DOB");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](23, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](24, "div", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](26, EnquiresComponent_enquires_list_26_Template, 1, 1, "enquires-list", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipe"](27, "async");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.isLoading);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵpipeBind1"](27, 2, ctx.enquiryResidents$));
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["NgIf"], _angular_common__WEBPACK_IMPORTED_MODULE_3__["NgForOf"], _enquires_list_enquires_list_component__WEBPACK_IMPORTED_MODULE_4__["EnquiresListComponent"]], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_3__["AsyncPipe"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy9lbnF1aXJlcy9lbnF1aXJlcy5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](EnquiresComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'app-enquires',
                templateUrl: './enquires.component.html',
                styleUrls: ['./enquires.component.css']
            }]
    }], function () { return [{ type: _services__WEBPACK_IMPORTED_MODULE_1__["EnquiresService"] }, { type: _angular_router__WEBPACK_IMPORTED_MODULE_2__["Router"] }]; }, null); })();


/***/ }),

/***/ "./src/app/residents/models/enquiry.resident.ts":
/*!******************************************************!*\
  !*** ./src/app/residents/models/enquiry.resident.ts ***!
  \******************************************************/
/*! exports provided: createInstanceofEnquiryResident */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "createInstanceofEnquiryResident", function() { return createInstanceofEnquiryResident; });
function createInstanceofEnquiryResident() {
    let model = {
        referenceId: '',
        foreName: '',
        surName: '',
        middleName: '',
        dob: undefined,
        gender: '',
        martialStatus: '',
        address: { street1: '', street2: '', city: '', county: '', postCode: '' },
        // street1: '',
        // street2: '',
        // city: '',
        // county: '',
        // postCode: '',
        socialWorker: {
            foreName: '',
            surName: '',
            phoneNumber: '',
            email: '',
        },
        // socialWorkerForeName: '',
        // socialWorkerSurName: '',
        // phoneNumber: '',
        // email: '',
        careCategory: '',
        careNeeds: '',
        stayType: '',
        roomLocation: '',
        roomNumber: '',
        moveInDate: undefined,
        familyHomeVisitDate: undefined,
        enquiryDate: undefined,
        responseDate: undefined,
        response: '',
        status: '',
        comments: '',
    };
    return model;
}
// export function createInstanceofEnquiryResident() {
//   // let model: EnquiryResident = {
//   //   "referenceId": "",
//   //   "residentProfile": {} as ResidentProfile,
//   //   "address": {} as Address,
//   //   "socialWorker": {} as SocialWorker,
//   //   "careType": {} as CareType,
//   //   "roomDetail": {} as RoomDetail,
//   //   "enquiryMiscData": {} as EnquiryMiscData
//   // };
//   // return model;
//   return null;
// }
// export interface EnquiryResidentxx {
//   referenceId: string;
//   residentProfile: ResidentProfile;
//   address: Address;
//   socialWorker: SocialWorker;
//   careType: CareType;
//   roomDetail: RoomDetail;
//   enquiryMiscData: EnquiryMiscData;
// }


/***/ }),

/***/ "./src/app/residents/models/index.ts":
/*!*******************************************!*\
  !*** ./src/app/residents/models/index.ts ***!
  \*******************************************/
/*! exports provided: Resident, createInstanceofEnquiryResident */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _resident__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./resident */ "./src/app/residents/models/resident.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Resident", function() { return _resident__WEBPACK_IMPORTED_MODULE_0__["Resident"]; });

/* harmony import */ var _enquiry_resident__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./enquiry.resident */ "./src/app/residents/models/enquiry.resident.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "createInstanceofEnquiryResident", function() { return _enquiry_resident__WEBPACK_IMPORTED_MODULE_1__["createInstanceofEnquiryResident"]; });





/***/ }),

/***/ "./src/app/residents/models/resident.ts":
/*!**********************************************!*\
  !*** ./src/app/residents/models/resident.ts ***!
  \**********************************************/
/*! exports provided: Resident */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Resident", function() { return Resident; });
class Resident {
}


/***/ }),

/***/ "./src/app/residents/services/enquires.service.ts":
/*!********************************************************!*\
  !*** ./src/app/residents/services/enquires.service.ts ***!
  \********************************************************/
/*! exports provided: EnquiresService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EnquiresService", function() { return EnquiresService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _helpers_store__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../helpers/store */ "./src/app/helpers/store.ts");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _services_index__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../services/index */ "./src/app/services/index.ts");





// https://stackblitz.com/edit/angular-fh1kyp?file=src%2Fapp%2Ftodo.service.ts
class EnquiresService extends _helpers_store__WEBPACK_IMPORTED_MODULE_1__["Store"] {
    constructor(apiService) {
        super([]);
        this.apiService = apiService;
    }
    loadEnquiresAll() {
        return this.apiService.getEnquiresAll()
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["map"])((enqs) => {
            this.setState(enqs);
            return enqs;
        }));
    }
    loadEnquiryByReferenceId(referenceId) {
        return this.apiService.getEnquiryByReferenceId(referenceId);
    }
    loadEnquiresAllByCareHomeId(careHomeId) {
        return this.apiService.getEnquiresByHome(careHomeId)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["map"])((enqs) => {
            this.setState(enqs);
            return enqs;
        }));
    }
}
EnquiresService.ɵfac = function EnquiresService_Factory(t) { return new (t || EnquiresService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_services_index__WEBPACK_IMPORTED_MODULE_3__["ApiService"])); };
EnquiresService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: EnquiresService, factory: EnquiresService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](EnquiresService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root'
            }]
    }], function () { return [{ type: _services_index__WEBPACK_IMPORTED_MODULE_3__["ApiService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/residents/services/index.ts":
/*!*********************************************!*\
  !*** ./src/app/residents/services/index.ts ***!
  \*********************************************/
/*! exports provided: ResidentsService, EnquiresService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _residents_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./residents.service */ "./src/app/residents/services/residents.service.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "ResidentsService", function() { return _residents_service__WEBPACK_IMPORTED_MODULE_0__["ResidentsService"]; });

/* harmony import */ var _enquires_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./enquires.service */ "./src/app/residents/services/enquires.service.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "EnquiresService", function() { return _enquires_service__WEBPACK_IMPORTED_MODULE_1__["EnquiresService"]; });



//export * from './enquiry.service';


/***/ }),

/***/ "./src/app/residents/services/residents.service.ts":
/*!*********************************************************!*\
  !*** ./src/app/residents/services/residents.service.ts ***!
  \*********************************************************/
/*! exports provided: ResidentsService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ResidentsService", function() { return ResidentsService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");



//import { Resident } from '../../models'; // '@app/models';
class ResidentsService {
    constructor(http) {
        this.http = http;
    }
}
ResidentsService.ɵfac = function ResidentsService_Factory(t) { return new (t || ResidentsService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"])); };
ResidentsService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: ResidentsService, factory: ResidentsService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ResidentsService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root',
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_1__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/residents/templ-edit/address-edit/address-edit.component.ts":
/*!*****************************************************************************!*\
  !*** ./src/app/residents/templ-edit/address-edit/address-edit.component.ts ***!
  \*****************************************************************************/
/*! exports provided: AddressEditComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AddressEditComponent", function() { return AddressEditComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");




//import { Address } from '../../models/index';
class AddressEditComponent {
    constructor() {
        this.street1Updated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.street2Updated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.cityUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.countyUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.postCodeUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.addressForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroup"]({
            street1: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            street2: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            city: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            county: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            postCode: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
        });
    }
    ngOnInit() {
    }
    ngOnChanges(changes) {
        if (changes.street1) {
            this.addressForm.controls['street1'].setValue(changes.street1.currentValue);
        }
        if (changes.street2) {
            this.addressForm.controls['street2'].setValue(changes.street2.currentValue);
        }
        if (changes.city) {
            this.addressForm.controls['city'].setValue(changes.city.currentValue);
        }
        if (changes.county) {
            this.addressForm.controls['county'].setValue(changes.county.currentValue);
        }
        if (changes.postCode) {
            this.addressForm.controls['postCode'].setValue(changes.postCode.currentValue);
        }
    }
    onStreet1Change(event) {
        this.street1Updated.emit(event);
    }
    onStreet2Change(event) {
        this.street2Updated.emit(event);
    }
    onCityChange(event) {
        this.cityUpdated.emit(event);
    }
    onCountyChange(event) {
        this.countyUpdated.emit(event);
    }
    onPostCodeChange(event) {
        this.postCodeUpdated.emit(event);
    }
}
AddressEditComponent.ɵfac = function AddressEditComponent_Factory(t) { return new (t || AddressEditComponent)(); };
AddressEditComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: AddressEditComponent, selectors: [["address-edit"]], inputs: { street1: "street1", street2: "street2", city: "city", county: "county", postCode: "postCode" }, outputs: { street1Updated: "street1Updated", street2Updated: "street2Updated", cityUpdated: "cityUpdated", countyUpdated: "countyUpdated", postCodeUpdated: "postCodeUpdated" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]], decls: 23, vars: 1, consts: [[3, "formGroup"], [1, "form-group"], ["for", "street1"], ["type", "text", "id", "street1", "name", "street1", "formControlName", "street1", "placeholder", "", 1, "form-control", 3, "change"], ["for", "street2"], ["type", "text", "id", "street2", "name", "street2", "formControlName", "street2", "placeholder", "", 1, "form-control", 3, "change"], ["for", "city"], ["type", "text", "id", "city", "name", "city", "formControlName", "city", "placeholder", "", 1, "form-control", 3, "change"], ["for", "county"], ["type", "text", "id", "county", "name", "county", "formControlName", "county", "placeholder", "", 1, "form-control", 3, "change"], ["for", "postCode"], ["type", "text", "id", "postCode", "name", "postCode", "formControlName", "postCode", "placeholder", "", 1, "form-control", 3, "change"]], template: function AddressEditComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "address form");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "label", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Street 1");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "input", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function AddressEditComponent_Template_input_change_6_listener($event) { return ctx.onStreet1Change($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "label", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Street 2");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function AddressEditComponent_Template_input_change_10_listener($event) { return ctx.onStreet2Change($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "City");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function AddressEditComponent_Template_input_change_14_listener($event) { return ctx.onCityChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "County");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "input", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function AddressEditComponent_Template_input_change_18_listener($event) { return ctx.onCountyChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "label", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "PostCode");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "input", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function AddressEditComponent_Template_input_change_22_listener($event) { return ctx.onPostCodeChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.addressForm);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy90ZW1wbC1lZGl0L2FkZHJlc3MtZWRpdC9hZGRyZXNzLWVkaXQuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AddressEditComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'address-edit',
                templateUrl: './address-edit.component.html',
                styleUrls: ['./address-edit.component.css']
            }]
    }], function () { return []; }, { street1: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], street2: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], city: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], county: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], postCode: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], street1Updated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], street2Updated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], cityUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], countyUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], postCodeUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/residents/templ-edit/care-type-edit/care-type-edit.component.ts":
/*!*********************************************************************************!*\
  !*** ./src/app/residents/templ-edit/care-type-edit/care-type-edit.component.ts ***!
  \*********************************************************************************/
/*! exports provided: CareTypeEditComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CareTypeEditComponent", function() { return CareTypeEditComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");




class CareTypeEditComponent {
    constructor() {
        this.careCategoryUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.careNeedsUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.stayTypeUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.careTypeForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroup"]({
            careCategory: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            careNeeds: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            stayType: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
        });
    }
    ngOnInit() {
    }
    ngOnChanges(changes) {
        console.log('ChangesCareType:', changes);
        if (changes.careCategory) {
            this.careTypeForm.controls['careCategory'].setValue(changes.careCategory.currentValue);
        }
        if (changes.careNeeds) {
            this.careTypeForm.controls['careNeeds'].setValue(changes.careNeeds.currentValue);
        }
        if (changes.stayType) {
            this.careTypeForm.controls['stayType'].setValue(changes.stayType.currentValue);
        }
    }
    onCareCategoryChange(event) {
        this.careCategoryUpdated.emit(event);
    }
    onCareNeedsChange(event) {
        this.careNeedsUpdated.emit(event);
    }
    onStayTypeChange(event) {
        this.stayTypeUpdated.emit(event);
    }
}
CareTypeEditComponent.ɵfac = function CareTypeEditComponent_Factory(t) { return new (t || CareTypeEditComponent)(); };
CareTypeEditComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: CareTypeEditComponent, selectors: [["care-type-edit"]], inputs: { careCategory: "careCategory", careNeeds: "careNeeds", stayType: "stayType" }, outputs: { careCategoryUpdated: "careCategoryUpdated", careNeedsUpdated: "careNeedsUpdated", stayTypeUpdated: "stayTypeUpdated" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]], decls: 37, vars: 1, consts: [[3, "formGroup"], [1, "form-group"], ["for", "careCategory"], ["name", "careCategory", "id", "careCategory", "formControlName", "careCategory", 1, "custom-select", 3, "change"], ["value", "", "selected", ""], ["value", "dementia"], ["value", "residential"], ["value", "nursing"], ["for", "careNeeds"], ["name", "careNeeds", "id", "careNeeds", "formControlName", "careNeeds", 1, "custom-select", 3, "change"], ["value", "high"], ["value", "medium"], ["value", "low"], ["for", "stayType"], ["name", "stayType", "id", "stayType", "formControlName", "stayType", 1, "custom-select", 3, "change"], ["value", "permanent"], ["value", "respite"]], template: function CareTypeEditComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "p");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "care-type-edit works!");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "label", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Care Category");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "select", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CareTypeEditComponent_Template_select_change_6_listener($event) { return ctx.onCareCategoryChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "option", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Choose...");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "option", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "Dementia");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "option", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "Residential");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "option", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "Nursing");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Care Needs");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "select", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CareTypeEditComponent_Template_select_change_18_listener($event) { return ctx.onCareNeedsChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "option", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](20, "Choose...");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "option", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](22, "High");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "option", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Medium");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "option", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](26, "Low");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "label", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Stay Type");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "select", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function CareTypeEditComponent_Template_select_change_30_listener($event) { return ctx.onStayTypeChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "option", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "Choose...");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "option", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](34, "Permanent");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "option", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, "Respite");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.careTypeForm);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_x"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy90ZW1wbC1lZGl0L2NhcmUtdHlwZS1lZGl0L2NhcmUtdHlwZS1lZGl0LmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](CareTypeEditComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'care-type-edit',
                templateUrl: './care-type-edit.component.html',
                styleUrls: ['./care-type-edit.component.css']
            }]
    }], function () { return []; }, { careCategory: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], careNeeds: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], stayType: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], careCategoryUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], careNeedsUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], stayTypeUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/residents/templ-edit/enquiry-misc-edit/enquiry-misc-edit.component.ts":
/*!***************************************************************************************!*\
  !*** ./src/app/residents/templ-edit/enquiry-misc-edit/enquiry-misc-edit.component.ts ***!
  \***************************************************************************************/
/*! exports provided: EnquiryMiscEditComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EnquiryMiscEditComponent", function() { return EnquiryMiscEditComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");





//import { EnquiryMiscData } from '../models';
class EnquiryMiscEditComponent {
    constructor() {
        this.moveInDateUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.familyHomeVisitDateUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.enquiryDateUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.responseDateUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.responseUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.commentsUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.enquiryMiscForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroup"]({
            moveInDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](undefined),
            familyHomeVisitDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](undefined),
            enquiryDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](undefined),
            responseDate: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](undefined),
            response: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            comments: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
        });
    }
    ngOnInit() {
    }
    ngOnChanges(changes) {
        if (changes.moveInDate && changes.moveInDate.currentValue) {
            const d = new Date(changes.moveInDate.currentValue);
            this.enquiryMiscForm.controls['moveInDate'].setValue({ year: d.getFullYear(), month: d.getMonth() + 1, day: d.getDate() });
        }
        if (changes.familyHomeVisitDate && changes.familyHomeVisitDate.currentValue) {
            const d = new Date(changes.familyHomeVisitDate.currentValue);
            this.enquiryMiscForm.controls['familyHomeVisitDate'].setValue({ year: d.getFullYear(), month: d.getMonth() + 1, day: d.getDate() });
        }
        if (changes.enquiryDate && changes.enquiryDate.currentValue) {
            const d = new Date(changes.enquiryDate.currentValue);
            this.enquiryMiscForm.controls['enquiryDate'].setValue({ year: d.getFullYear(), month: d.getMonth() + 1, day: d.getDate() });
        }
        if (changes.responseDate && changes.responseDate.currentValue) {
            const d = new Date(changes.responseDate.currentValue);
            this.enquiryMiscForm.controls['responseDate'].setValue({ year: d.getFullYear(), month: d.getMonth() + 1, day: d.getDate() });
        }
        if (changes.response) {
            this.enquiryMiscForm.controls['response'].setValue(changes.response.currentValue);
        }
        if (changes.comments) {
            this.enquiryMiscForm.controls['comments'].setValue(changes.comments.currentValue);
        }
    }
    onMoveInDateChange(event) {
        this.moveInDateUpdated.emit(event);
    }
    onFamilyHomeVisitDateChange(event) {
        this.familyHomeVisitDateUpdated.emit(event);
    }
    onEnquiryDateChange(event) {
        this.enquiryDateUpdated.emit(event);
    }
    onResponseDateChange(event) {
        this.responseDateUpdated.emit(event);
    }
    onResponseChange(event) {
        this.responseUpdated.emit(event);
    }
    onCommentsChange(event) {
        this.commentsUpdated.emit(event);
    }
}
EnquiryMiscEditComponent.ɵfac = function EnquiryMiscEditComponent_Factory(t) { return new (t || EnquiryMiscEditComponent)(); };
EnquiryMiscEditComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: EnquiryMiscEditComponent, selectors: [["enquiry-misc-edit"]], inputs: { moveInDate: "moveInDate", familyHomeVisitDate: "familyHomeVisitDate", enquiryDate: "enquiryDate", responseDate: "responseDate", response: "response", comments: "comments" }, outputs: { moveInDateUpdated: "moveInDateUpdated", familyHomeVisitDateUpdated: "familyHomeVisitDateUpdated", enquiryDateUpdated: "enquiryDateUpdated", responseDateUpdated: "responseDateUpdated", responseUpdated: "responseUpdated", commentsUpdated: "commentsUpdated" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]], decls: 43, vars: 1, consts: [[3, "formGroup"], [1, "form-group"], ["for", "moveInDate"], [1, "input-group"], ["placeholder", "yyyy-mm-dd", "formControlName", "moveInDate", "name", "moveInDate", "ngbDatepicker", "", 1, "form-control", 3, "dateSelect"], ["d1", "ngbDatepicker"], [1, "input-group-append"], ["type", "button", 1, "btn", "btn-outline-secondary", "calendar", 3, "click"], ["for", "familyHomeVisitDate"], ["placeholder", "yyyy-mm-dd", "formControlName", "familyHomeVisitDate", "name", "familyHomeVisitDate", "ngbDatepicker", "", 1, "form-control", 3, "dateSelect"], ["d2", "ngbDatepicker"], ["for", "enquiryDate"], ["placeholder", "yyyy-mm-dd", "formControlName", "enquiryDate", "name", "enquiryDate", "ngbDatepicker", "", 1, "form-control", 3, "dateSelect"], ["d3", "ngbDatepicker"], ["for", "responseDate"], ["placeholder", "yyyy-mm-dd", "formControlName", "responseDate", "name", "responseDate", "ngbDatepicker", "", 1, "form-control", 3, "dateSelect"], ["d4", "ngbDatepicker"], ["for", "response"], ["matInput", "", "rows", "4", "id", "response", "name", "response", "formControlName", "response", "placeholder", "", 1, "form-control", 3, "change"], ["for", "comments"], ["rows", "4", "id", "comments", "name", "comments", "formControlName", "comments", "placeholder", "", 1, "form-control", 3, "change"]], template: function EnquiryMiscEditComponent_Template(rf, ctx) { if (rf & 1) {
        const _r4 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "p");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "enquiry-misc-edit works!");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "label", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Move In Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "input", 4, 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("dateSelect", function EnquiryMiscEditComponent_Template_input_dateSelect_7_listener($event) { return ctx.onMoveInDateChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "button", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function EnquiryMiscEditComponent_Template_button_click_10_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r4); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](8); return _r0.toggle(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Family Home Visit Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "input", 9, 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("dateSelect", function EnquiryMiscEditComponent_Template_input_dateSelect_15_listener($event) { return ctx.onFamilyHomeVisitDateChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "button", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function EnquiryMiscEditComponent_Template_button_click_18_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r4); const _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](16); return _r1.toggle(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "label", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](21, "Enquiry Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "input", 12, 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("dateSelect", function EnquiryMiscEditComponent_Template_input_dateSelect_23_listener($event) { return ctx.onEnquiryDateChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "button", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function EnquiryMiscEditComponent_Template_button_click_26_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r4); const _r2 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](24); return _r2.toggle(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](28, "label", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](29, "Response Date");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "input", 15, 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("dateSelect", function EnquiryMiscEditComponent_Template_input_dateSelect_31_listener($event) { return ctx.onResponseDateChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "button", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function EnquiryMiscEditComponent_Template_button_click_34_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r4); const _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](32); return _r3.toggle(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](36, "label", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](37, "Response");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "textarea", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function EnquiryMiscEditComponent_Template_textarea_change_38_listener($event) { return ctx.onResponseChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](40, "label", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](41, "Comments");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "textarea", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function EnquiryMiscEditComponent_Template_textarea_change_42_listener($event) { return ctx.onCommentsChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.enquiryMiscForm);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_2__["NgbInputDatepicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy90ZW1wbC1lZGl0L2VucXVpcnktbWlzYy1lZGl0L2VucXVpcnktbWlzYy1lZGl0LmNvbXBvbmVudC5jc3MifQ== */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](EnquiryMiscEditComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'enquiry-misc-edit',
                templateUrl: './enquiry-misc-edit.component.html',
                styleUrls: ['./enquiry-misc-edit.component.css']
            }]
    }], function () { return []; }, { moveInDate: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], familyHomeVisitDate: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], enquiryDate: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], responseDate: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], response: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], comments: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], moveInDateUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], familyHomeVisitDateUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], enquiryDateUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], responseDateUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], responseUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], commentsUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/residents/templ-edit/resident-profile-edit/resident-profile-edit.component.ts":
/*!***********************************************************************************************!*\
  !*** ./src/app/residents/templ-edit/resident-profile-edit/resident-profile-edit.component.ts ***!
  \***********************************************************************************************/
/*! exports provided: ResidentProfileEditComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ResidentProfileEditComponent", function() { return ResidentProfileEditComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/__ivy_ngcc__/fesm2015/ng-bootstrap.js");





class ResidentProfileEditComponent {
    constructor() {
        this.foreNameUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.surNameUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.middleNameUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.dobUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.genderUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.martialStatusUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.profileForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroup"]({
            foreName: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            surName: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            middleName: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            dob: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](undefined),
            gender: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            martialStatus: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
        });
    }
    ngOnInit() {
    }
    ngOnChanges(changes) {
        console.log('ngonchg', changes);
        if (changes.foreName) {
            this.profileForm.controls['foreName'].setValue(changes.foreName.currentValue);
        }
        if (changes.surName) {
            this.profileForm.controls['surName'].setValue(changes.surName.currentValue);
        }
        if (changes.middleName) {
            this.profileForm.controls['middleName'].setValue(changes.middleName.currentValue);
        }
        if (changes.dob) {
            if (changes.dob.currentValue) {
                const d = new Date(changes.dob.currentValue);
                this.profileForm.controls['dob'].setValue({ year: d.getFullYear(), month: d.getMonth() + 1, day: d.getDate() });
            }
        }
        if (changes.gender) {
            this.profileForm.controls['gender'].setValue(changes.gender.currentValue);
        }
    }
    onForeNameChange(event) {
        this.foreNameUpdated.emit(event);
    }
    onSurNameChange(event) {
        this.surNameUpdated.emit(event);
    }
    onMiddleNameChange(event) {
        this.middleNameUpdated.emit(event);
    }
    onDobChange(event) {
        this.dobUpdated.emit(event);
    }
    onGenderChange(event) {
        this.genderUpdated.emit(event);
    }
    onMartialStatusChange(event) {
        this.martialStatusUpdated.emit(event);
    }
}
ResidentProfileEditComponent.ɵfac = function ResidentProfileEditComponent_Factory(t) { return new (t || ResidentProfileEditComponent)(); };
ResidentProfileEditComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: ResidentProfileEditComponent, selectors: [["resident-profile-edit"]], inputs: { foreName: "foreName", surName: "surName", middleName: "middleName", dob: "dob", gender: "gender", martialStatus: "martialStatus" }, outputs: { foreNameUpdated: "foreNameUpdated", surNameUpdated: "surNameUpdated", middleNameUpdated: "middleNameUpdated", dobUpdated: "dobUpdated", genderUpdated: "genderUpdated", martialStatusUpdated: "martialStatusUpdated" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]], decls: 57, vars: 1, consts: [[3, "formGroup"], [1, "form-group"], ["for", "foreName"], ["type", "text", "id", "foreName", "name", "foreName", "formControlName", "foreName", "placeholder", "", 1, "form-control", 3, "change"], ["type", "text", "id", "surName", "name", "surName", "formControlName", "surName", "placeholder", "", 1, "form-control", 3, "change"], ["for", "middleName"], ["type", "text", "id", "middleName", "name", "middleName", "formControlName", "middleName", "placeholder", "", 1, "form-control", 3, "change"], ["for", "dob"], [1, "input-group"], ["placeholder", "yyyy-mm-dd", "formControlName", "dob", "name", "dob", "ngbDatepicker", "", 1, "form-control", 3, "dateSelect"], ["d", "ngbDatepicker"], [1, "input-group-append"], ["type", "button", 1, "btn", "btn-outline-secondary", "calendar", 3, "click"], ["for", "gender"], [1, "custom-control", "custom-radio", "custom-control-inline"], ["type", "radio", "id", "genderMale", "formControlName", "gender", "name", "gender", "value", "male", 1, "custom-control-input", 3, "change"], ["for", "genderMale", 1, "custom-control-label"], ["type", "radio", "id", "genderFemale", "formControlName", "gender", "name", "gender", "value", "female", 1, "custom-control-input", 3, "change"], ["for", "genderFemale", 1, "custom-control-label"], ["type", "radio", "id", "genderNotDisclosed", "formControlName", "gender", "name", "gender", "value", "notdisclosed", 1, "custom-control-input", 3, "change"], ["for", "genderNotDisclosed", 1, "custom-control-label"], ["for", "maritalStatus"], ["type", "radio", "id", "maritalStatusMarried", "formControlName", "martialStatus", "name", "martialStatus", "value", "married", 1, "custom-control-input", 3, "change"], ["for", "maritalStatusMarried", 1, "custom-control-label"], ["type", "radio", "id", "maritalStatusDivorced", "formControlName", "martialStatus", "name", "martialStatus", "value", "divorced", 1, "custom-control-input", 3, "change"], ["for", "maritalStatusDivorced", 1, "custom-control-label"], ["type", "radio", "id", "maritalStatusWidower", "formControlName", "martialStatus", "name", "martialStatus", "value", "widower", 1, "custom-control-input", 3, "change"], ["for", "maritalStatusWidower", 1, "custom-control-label"], ["type", "radio", "id", "maritalStatusSingle", "formControlName", "martialStatus", "name", "martialStatus", "value", "single", 1, "custom-control-input", 3, "change"], ["for", "maritalStatusSingle", 1, "custom-control-label"]], template: function ResidentProfileEditComponent_Template(rf, ctx) { if (rf & 1) {
        const _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵgetCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "label", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](3, "Fore Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "input", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_4_listener($event) { return ctx.onForeNameChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "label", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Sur Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "input", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_8_listener($event) { return ctx.onSurNameChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "label", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Middle Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "input", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_12_listener($event) { return ctx.onMiddleNameChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "label", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "DOB");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "div", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](17, "input", 9, 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("dateSelect", function ResidentProfileEditComponent_Template_input_dateSelect_17_listener($event) { return ctx.onDobChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](19, "div", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "button", 12);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function ResidentProfileEditComponent_Template_button_click_20_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵrestoreView"](_r1); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵreference"](18); return _r0.toggle(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](21, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](23, "label", 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](24, "Gender");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](25, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](26, "input", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_26_listener($event) { return ctx.onGenderChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "label", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](28, "Male");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](29, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](30, "input", 17);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_30_listener($event) { return ctx.onGenderChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](31, "label", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](32, "Female");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](33, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](34, "input", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_34_listener($event) { return ctx.onGenderChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](35, "label", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](36, "Not Disclosed");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](38, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](39, "label", 21);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](40, "Martial Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](41, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "input", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_42_listener($event) { return ctx.onMartialStatusChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](43, "label", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](44, "Married");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](45, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "input", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_46_listener($event) { return ctx.onMartialStatusChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](47, "label", 25);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](48, "Divorced");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](49, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "input", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_50_listener($event) { return ctx.onMartialStatusChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](51, "label", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](52, "Widower");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](53, "div", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "input", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function ResidentProfileEditComponent_Template_input_change_54_listener($event) { return ctx.onMartialStatusChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](55, "label", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](56, "Single");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.profileForm);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_2__["NgbInputDatepicker"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["RadioControlValueAccessor"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy90ZW1wbC1lZGl0L3Jlc2lkZW50LXByb2ZpbGUtZWRpdC9yZXNpZGVudC1wcm9maWxlLWVkaXQuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ResidentProfileEditComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'resident-profile-edit',
                templateUrl: './resident-profile-edit.component.html',
                styleUrls: ['./resident-profile-edit.component.css']
            }]
    }], function () { return []; }, { foreName: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], surName: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], middleName: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], dob: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], gender: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], martialStatus: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], foreNameUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], surNameUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], middleNameUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], dobUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], genderUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], martialStatusUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/residents/templ-edit/room-location-edit/room-location-edit.component.ts":
/*!*****************************************************************************************!*\
  !*** ./src/app/residents/templ-edit/room-location-edit/room-location-edit.component.ts ***!
  \*****************************************************************************************/
/*! exports provided: RoomLocationEditComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RoomLocationEditComponent", function() { return RoomLocationEditComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/common.js");





function RoomLocationEditComponent_option_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const rmloc_r2 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", rmloc_r2.id);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", rmloc_r2.name, " ");
} }
function RoomLocationEditComponent_option_16_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "option", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const room_r3 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("value", room_r3.id);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", room_r3.name, " ");
} }
class RoomLocationEditComponent {
    constructor() {
        this.reservedRoomLocation = 0;
        this.reservedRoomNumber = 0;
        this.roomNumbers = [];
        this.roomLocationUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.roomNumberUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.roomDetailForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroup"]({
            roomLocations: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            rooms: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
        });
    }
    ngOnInit() {
    }
    ngOnChanges(changes) {
        console.log('ngonchg==', changes);
        if (changes.reservedRoomLocation) {
            this.roomDetailForm.controls['roomLocations'].setValue(changes.reservedRoomLocation.currentValue);
        }
        if (changes.reservedRoomNumber) {
            if (this.reservedRoomLocation > 0) {
                this.loadRoomsByLocationId(this.reservedRoomLocation);
                this.roomDetailForm.controls['rooms'].setValue(changes.reservedRoomNumber.currentValue);
            }
        }
    }
    onRoomLocationChange(event) {
        this.loadRoomsByLocationId(event.target.value);
        this.roomLocationUpdated.emit(event);
    }
    onRoomChange(event) {
        this.roomNumberUpdated.emit(event);
    }
    loadRoomsByLocationId(locId) {
        const selLoc = this.roomLocations.filter(loc => loc.id == locId);
        this.roomNumbers = selLoc.map(x => x.rooms)[0];
    }
}
RoomLocationEditComponent.ɵfac = function RoomLocationEditComponent_Factory(t) { return new (t || RoomLocationEditComponent)(); };
RoomLocationEditComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: RoomLocationEditComponent, selectors: [["room-location-edit"]], inputs: { roomLocations: "roomLocations", reservedRoomLocation: "reservedRoomLocation", reservedRoomNumber: "reservedRoomNumber" }, outputs: { roomLocationUpdated: "roomLocationUpdated", roomNumberUpdated: "roomNumberUpdated" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]], decls: 17, vars: 3, consts: [[3, "formGroup"], [1, "form-group"], ["for", "roomLocations"], ["name", "roomLocations", "id", "roomLocations", "formControlName", "roomLocations", 1, "custom-select", 3, "change"], ["value", "", "selected", ""], [3, "value", 4, "ngFor", "ngForOf"], ["for", "rooms"], ["name", "rooms", "id", "rooms", "formControlName", "rooms", 1, "custom-select", 3, "change"], [3, "value"]], template: function RoomLocationEditComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "p");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "room-location-edit works!");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "label", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Room Location");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "select", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function RoomLocationEditComponent_Template_select_change_6_listener($event) { return ctx.onRoomLocationChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "option", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Choose...");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](9, RoomLocationEditComponent_option_9_Template, 2, 2, "option", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](12, "Room Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](13, "select", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function RoomLocationEditComponent_Template_select_change_13_listener($event) { return ctx.onRoomChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "option", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](15, "Choose...");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](16, RoomLocationEditComponent_option_16_Template, 2, 2, "option", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.roomDetailForm);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.roomLocations);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx.roomNumbers);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_2__["NgForOf"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy90ZW1wbC1lZGl0L3Jvb20tbG9jYXRpb24tZWRpdC9yb29tLWxvY2F0aW9uLWVkaXQuY29tcG9uZW50LmNzcyJ9 */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](RoomLocationEditComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'room-location-edit',
                templateUrl: './room-location-edit.component.html',
                styleUrls: ['./room-location-edit.component.css']
            }]
    }], function () { return []; }, { roomLocations: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], reservedRoomLocation: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], reservedRoomNumber: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], roomLocationUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], roomNumberUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/residents/templ-edit/social-worker-detail-edit/social-worker-detail-edit.component.ts":
/*!*******************************************************************************************************!*\
  !*** ./src/app/residents/templ-edit/social-worker-detail-edit/social-worker-detail-edit.component.ts ***!
  \*******************************************************************************************************/
/*! exports provided: SocialWorkerDetailEditComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SocialWorkerDetailEditComponent", function() { return SocialWorkerDetailEditComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/__ivy_ngcc__/fesm2015/forms.js");




class SocialWorkerDetailEditComponent {
    constructor() {
        this.swForeNameUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.swSurNameUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.swPhoneNumberUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.swEmailUpdated = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.socialWorkerForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroup"]({
            foreName: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            surName: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            phoneNumber: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
            email: new _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControl"](''),
        });
    }
    ngOnInit() { }
    ngOnChanges(changes) {
        if (changes.foreName) {
            this.socialWorkerForm.controls['foreName'].setValue(changes.foreName.currentValue);
        }
        if (changes.surName) {
            this.socialWorkerForm.controls['surName'].setValue(changes.surName.currentValue);
        }
        if (changes.phoneNumber) {
            this.socialWorkerForm.controls['phoneNumber'].setValue(changes.phoneNumber.currentValue);
        }
        if (changes.email) {
            this.socialWorkerForm.controls['email'].setValue(changes.email.currentValue);
        }
    }
    onSwForeNameChange(event) {
        this.swForeNameUpdated.emit(event);
    }
    onSwSurNameChange(event) {
        this.swSurNameUpdated.emit(event);
    }
    onSwPhoneNumberChange(event) {
        this.swPhoneNumberUpdated.emit(event);
    }
    onSwEmailChange(event) {
        this.swEmailUpdated.emit(event);
    }
}
SocialWorkerDetailEditComponent.ɵfac = function SocialWorkerDetailEditComponent_Factory(t) { return new (t || SocialWorkerDetailEditComponent)(); };
SocialWorkerDetailEditComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: SocialWorkerDetailEditComponent, selectors: [["social-worker-detail-edit"]], inputs: { foreName: "foreName", surName: "surName", phoneNumber: "phoneNumber", email: "email" }, outputs: { swForeNameUpdated: "swForeNameUpdated", swSurNameUpdated: "swSurNameUpdated", swPhoneNumberUpdated: "swPhoneNumberUpdated", swEmailUpdated: "swEmailUpdated" }, features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]], decls: 19, vars: 1, consts: [[3, "formGroup"], [1, "form-group"], ["for", "foreName"], ["type", "text", "id", "foreName", "name", "foreName", "formControlName", "foreName", "placeholder", "", 1, "form-control", 3, "change"], ["for", "surName"], ["type", "text", "id", "surName", "name", "surName", "formControlName", "surName", "placeholder", "", 1, "form-control", 3, "change"], ["for", "phoneNumber"], ["type", "text", "id", "phoneNumber", "name", "phoneNumber", "formControlName", "phoneNumber", "placeholder", "", 1, "form-control", 3, "change"], ["for", "email"], ["type", "text", "id", "email", "name", "email", "formControlName", "email", "placeholder", "", 1, "form-control", 3, "change"]], template: function SocialWorkerDetailEditComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "p");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "social-worker-detail-edit works!");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](2, "form", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](4, "label", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Fore Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "input", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function SocialWorkerDetailEditComponent_Template_input_change_6_listener($event) { return ctx.onSwForeNameChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "label", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](9, "Sur Name");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](10, "input", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function SocialWorkerDetailEditComponent_Template_input_change_10_listener($event) { return ctx.onSwSurNameChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](11, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "label", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](13, "Phone Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](14, "input", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function SocialWorkerDetailEditComponent_Template_input_change_14_listener($event) { return ctx.onSwPhoneNumberChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](16, "label", 8);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "Phone Number");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "input", 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("change", function SocialWorkerDetailEditComponent_Template_input_change_18_listener($event) { return ctx.onSwEmailChange($event); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("formGroup", ctx.socialWorkerForm);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_1__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormGroupDirective"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_1__["FormControlName"]], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Jlc2lkZW50cy90ZW1wbC1lZGl0L3NvY2lhbC13b3JrZXItZGV0YWlsLWVkaXQvc29jaWFsLXdvcmtlci1kZXRhaWwtZWRpdC5jb21wb25lbnQuY3NzIn0= */"] });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](SocialWorkerDetailEditComponent, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"],
        args: [{
                selector: 'social-worker-detail-edit',
                templateUrl: './social-worker-detail-edit.component.html',
                styleUrls: ['./social-worker-detail-edit.component.css'],
            }]
    }], function () { return []; }, { foreName: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], surName: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], phoneNumber: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], email: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }], swForeNameUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], swSurNameUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], swPhoneNumberUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }], swEmailUpdated: [{
            type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }] }); })();


/***/ }),

/***/ "./src/app/services/api.service.ts":
/*!*****************************************!*\
  !*** ./src/app/services/api.service.ts ***!
  \*****************************************/
/*! exports provided: ApiService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ApiService", function() { return ApiService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../environments/environment */ "./src/environments/environment.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");




class ApiService {
    constructor(http) {
        this.http = http;
    }
    // === User related ===
    authenticateUser(username, password) {
        //console.log('in api service. authenticateUser');
        return this.http.post(`/api/authentication/authenticate`, { username, password }, { withCredentials: true });
        // .post<any>(
        //   `${environment.apiDomainUrl}/api/authentication/authenticate`,
        //   { username, password },
        //   { withCredentials: true }
        // );
    }
    // change to ref id later
    // getUserById(id: number): Observable<User> {
    getCareHomeUserByReferenceId(referenceId) {
        return this.http.get(`/api/users/carehomeusers/${referenceId}`, { withCredentials: true });
        // return this.http.get<CareHomeUser>(
        //   `${environment.apiDomainUrl}/api/users/carehomeusers/${referenceId}`,
        //   { withCredentials: true }
        // );
    }
    //=== endof user related ===
    // === enquires related ===
    getEnquiresAll() {
        return this.http.get(`${_environments_environment__WEBPACK_IMPORTED_MODULE_1__["environment"].apiDomainUrl}/api/carehomes/enquires`);
    }
    getEnquiresByHome(careHomeId) {
        return this.http.get(`${_environments_environment__WEBPACK_IMPORTED_MODULE_1__["environment"].apiDomainUrl}/api/carehomes/${careHomeId}/enquires`);
    }
    getEnquiryByReferenceId(referenceId) {
        return this.http.get(`${_environments_environment__WEBPACK_IMPORTED_MODULE_1__["environment"].apiDomainUrl}/api/enquires/${referenceId}`);
    }
    createEnquiryResident(enqResident) {
        return this.http.post(`${_environments_environment__WEBPACK_IMPORTED_MODULE_1__["environment"].apiDomainUrl}/api/enquires`, enqResident);
    }
    updateEnquiryResident(enqResident) {
        return this.http.put(`${_environments_environment__WEBPACK_IMPORTED_MODULE_1__["environment"].apiDomainUrl}/api/enquires/${enqResident.referenceId}`, enqResident);
    }
}
ApiService.ɵfac = function ApiService_Factory(t) { return new (t || ApiService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"])); };
ApiService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: ApiService, factory: ApiService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](ApiService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root',
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/app/services/authentication.service.ts":
/*!****************************************************!*\
  !*** ./src/app/services/authentication.service.ts ***!
  \****************************************************/
/*! exports provided: AuthenticationService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AuthenticationService", function() { return AuthenticationService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm2015/index.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/__ivy_ngcc__/fesm2015/router.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");
/* harmony import */ var _auth0_angular_jwt__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @auth0/angular-jwt */ "./node_modules/@auth0/angular-jwt/__ivy_ngcc__/fesm2015/auth0-angular-jwt.js");
/* harmony import */ var _api_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./api.service */ "./src/app/services/api.service.ts");








class AuthenticationService {
    constructor(router, http, jwtHelper, apiService) {
        this.router = router;
        this.http = http;
        this.jwtHelper = jwtHelper;
        this.apiService = apiService;
    }
    login(username, password) {
        return this.apiService.authenticateUser(username, password)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["map"])((user) => {
            console.log('>>user received: ', user);
            this.setStorage(user);
            return user;
        }));
    }
    logout() {
        // this.http
        //   .post<any>(
        //     `${environment.apiDomainUrl}/api/authentication/revoke-token`,
        //     {},
        //     { withCredentials: true }
        //   )
        //   .subscribe();
        this.clearStorage();
        this.router.navigate(['/login']);
    }
    setStorage(authUserResult) {
        localStorage.setItem('tokenId', authUserResult.jwtToken);
    }
    clearStorage() {
        // localStorage.removeItem('tokenId');
    }
    isLoggedIn() {
        return !this.isTokenExpired();
    }
    getToken() {
        return localStorage.getItem('tokenId');
    }
    hasToken() {
        if (this.getToken()) {
            return true;
        }
        return false;
    }
    readToken() {
        return this.jwtHelper.decodeToken(this.getToken());
    }
    getTokenExpirationDate() {
        return this.jwtHelper.getTokenExpirationDate(this.getToken());
    }
    isTokenExpired() {
        if (this.getTokenExpirationDate() != null) {
            return !this.jwtHelper.isTokenExpired(this.getToken());
        }
        return true;
    }
    getUserReferenceIdFromToken() {
        const tkn = this.getToken();
        if (tkn) {
            const decToken = this.readToken();
            return decToken.unique_name;
        }
        return Object(rxjs__WEBPACK_IMPORTED_MODULE_1__["throwError"])('Reference Id not found in token');
    }
}
AuthenticationService.ɵfac = function AuthenticationService_Factory(t) { return new (t || AuthenticationService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_router__WEBPACK_IMPORTED_MODULE_3__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_auth0_angular_jwt__WEBPACK_IMPORTED_MODULE_5__["JwtHelperService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_api_service__WEBPACK_IMPORTED_MODULE_6__["ApiService"])); };
AuthenticationService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: AuthenticationService, factory: AuthenticationService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AuthenticationService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{ providedIn: 'root' }]
    }], function () { return [{ type: _angular_router__WEBPACK_IMPORTED_MODULE_3__["Router"] }, { type: _angular_common_http__WEBPACK_IMPORTED_MODULE_4__["HttpClient"] }, { type: _auth0_angular_jwt__WEBPACK_IMPORTED_MODULE_5__["JwtHelperService"] }, { type: _api_service__WEBPACK_IMPORTED_MODULE_6__["ApiService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/services/index.ts":
/*!***********************************!*\
  !*** ./src/app/services/index.ts ***!
  \***********************************/
/*! exports provided: AuthenticationService, UserService, ApiService, WeatherService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _authentication_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./authentication.service */ "./src/app/services/authentication.service.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "AuthenticationService", function() { return _authentication_service__WEBPACK_IMPORTED_MODULE_0__["AuthenticationService"]; });

/* harmony import */ var _user_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./user.service */ "./src/app/services/user.service.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "UserService", function() { return _user_service__WEBPACK_IMPORTED_MODULE_1__["UserService"]; });

/* harmony import */ var _api_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./api.service */ "./src/app/services/api.service.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "ApiService", function() { return _api_service__WEBPACK_IMPORTED_MODULE_2__["ApiService"]; });

/* harmony import */ var _weather_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./weather.service */ "./src/app/services/weather.service.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "WeatherService", function() { return _weather_service__WEBPACK_IMPORTED_MODULE_3__["WeatherService"]; });







/***/ }),

/***/ "./src/app/services/user.service.ts":
/*!******************************************!*\
  !*** ./src/app/services/user.service.ts ***!
  \******************************************/
/*! exports provided: UserService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "UserService", function() { return UserService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm2015/operators/index.js");
/* harmony import */ var _helpers_store__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../helpers/store */ "./src/app/helpers/store.ts");
/* harmony import */ var _api_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./api.service */ "./src/app/services/api.service.ts");
/* harmony import */ var _authentication_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./authentication.service */ "./src/app/services/authentication.service.ts");






class UserService extends _helpers_store__WEBPACK_IMPORTED_MODULE_2__["Store"] {
    constructor(apiService, authService) {
        super(null);
        this.apiService = apiService;
        this.authService = authService;
    }
    login(username, password) {
        console.log('>>initiate login for ', username);
        return this.authService.login(username, password)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["map"])((user) => {
            console.log('>>login is success.', user);
            // this.setState(user);
            // console.log('===refid', this.authService.getUserReferenceIdFromToken());
            //this.userSubject.next(user);
            //this.setUserRoleValues(user);
            return user;
        }));
    }
    // API calls
    // used to be reloaduser()...
    reloadUser() {
        const referenceId = this.authService.getUserReferenceIdFromToken(); //.getUserIdFromToken();
        console.log('>>fetching carehomeuser');
        return this.apiService.getCareHomeUserByReferenceId(referenceId) //.getUserById(id)
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_1__["map"])((user) => {
            console.log('setting state with care home user', user);
            this.setState(user);
            return user;
        }));
    }
    getStoreUser() {
        return this.getValue();
    }
    hasUserToken() {
        return this.authService.hasToken();
    }
    logout() {
        // this.userRolesSubject.next([]);
        // this.userSubject.next(null);
        this.authService.logout();
    }
}
UserService.ɵfac = function UserService_Factory(t) { return new (t || UserService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_api_service__WEBPACK_IMPORTED_MODULE_3__["ApiService"]), _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_authentication_service__WEBPACK_IMPORTED_MODULE_4__["AuthenticationService"])); };
UserService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: UserService, factory: UserService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](UserService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{ providedIn: 'root' }]
    }], function () { return [{ type: _api_service__WEBPACK_IMPORTED_MODULE_3__["ApiService"] }, { type: _authentication_service__WEBPACK_IMPORTED_MODULE_4__["AuthenticationService"] }]; }, null); })();


/***/ }),

/***/ "./src/app/services/weather.service.ts":
/*!*********************************************!*\
  !*** ./src/app/services/weather.service.ts ***!
  \*********************************************/
/*! exports provided: WeatherService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "WeatherService", function() { return WeatherService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../environments/environment */ "./src/environments/environment.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/__ivy_ngcc__/fesm2015/http.js");




class WeatherService {
    constructor(http) {
        this.http = http;
    }
    getAll() {
        return this.http.get(`${_environments_environment__WEBPACK_IMPORTED_MODULE_1__["environment"].apiDomainUrl}/api/weatherforecast`);
    }
}
WeatherService.ɵfac = function WeatherService_Factory(t) { return new (t || WeatherService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"])); };
WeatherService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: WeatherService, factory: WeatherService.ɵfac, providedIn: 'root' });
/*@__PURE__*/ (function () { _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](WeatherService, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"],
        args: [{
                providedIn: 'root',
            }]
    }], function () { return [{ type: _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"] }]; }, null); })();


/***/ }),

/***/ "./src/environments/environment.ts":
/*!*****************************************!*\
  !*** ./src/environments/environment.ts ***!
  \*****************************************/
/*! exports provided: environment */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "environment", function() { return environment; });
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
const environment = {
    production: false,
    apiDomainUrl: 'https://localhost:44360',
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.


/***/ }),

/***/ "./src/main.ts":
/*!*********************!*\
  !*** ./src/main.ts ***!
  \*********************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./environments/environment */ "./src/environments/environment.ts");
/* harmony import */ var _app_app_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./app/app.module */ "./src/app/app.module.ts");
/* harmony import */ var _angular_platform_browser__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/platform-browser */ "./node_modules/@angular/platform-browser/__ivy_ngcc__/fesm2015/platform-browser.js");




if (_environments_environment__WEBPACK_IMPORTED_MODULE_1__["environment"].production) {
    Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["enableProdMode"])();
}
_angular_platform_browser__WEBPACK_IMPORTED_MODULE_3__["platformBrowser"]().bootstrapModule(_app_app_module__WEBPACK_IMPORTED_MODULE_2__["AppModule"])
    .catch(err => console.error(err));


/***/ }),

/***/ 0:
/*!***************************!*\
  !*** multi ./src/main.ts ***!
  \***************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! C:\Users\major\Documents\projects2\aspmvc\nyms\client\nyms-care-client\src\main.ts */"./src/main.ts");


/***/ })

},[[0,"runtime","vendor"]]]);
//# sourceMappingURL=main.js.map