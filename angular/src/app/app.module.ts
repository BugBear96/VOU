import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AbpModule } from '@abp/abp.module';

import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';

import { HomeComponent } from '@app/home/home.component';
import { AboutComponent } from '@app/about/about.component';
import { TopBarComponent } from '@app/layout/topbar.component';
import { TopBarLanguageSwitchComponent } from '@app/layout/topbar-languageswitch.component';
import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-area.component';
import { SideBarNavComponent } from '@app/layout/sidebar-nav.component';
import { SideBarFooterComponent } from '@app/layout/sidebar-footer.component';
import { RightSideBarComponent } from '@app/layout/right-sidebar.component';

import { IonRangeSliderModule } from "ng2-ion-range-slider";

// tenants
import { TenantsComponent } from '@app/tenants/tenants.component';
import { CreateTenantDialogComponent } from './tenants/create-tenant/create-tenant-dialog.component';
import { EditTenantDialogComponent } from './tenants/edit-tenant/edit-tenant-dialog.component';
// roles
import { RolesComponent } from '@app/roles/roles.component';
import { CreateRoleDialogComponent } from './roles/create-role/create-role-dialog.component';
import { EditRoleDialogComponent } from './roles/edit-role/edit-role-dialog.component';
// users
import { UsersComponent } from '@app/users/users.component';
import { CreateUserDialogComponent } from '@app/users/create-user/create-user-dialog.component';
import { EditUserDialogComponent } from '@app/users/edit-user/edit-user-dialog.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { ResetPasswordDialogComponent } from './users/reset-password/reset-password.component';
import { TenantCategoryComponent } from './tenant-category/tenant-category.component';

import { CreateCategoryComponent } from './tenant-category/create-category/create-category.component';
import { EditCategoryComponent } from './tenant-category/edit-category/edit-category.component';

import { StateComponent } from './branch/state/state.component';
import { CreateStateComponent } from './branch/state/create-state/create-state.component';
import { EditStateComponent } from './branch/state/edit-state/edit-state.component';

import { LocationComponent } from './branch/location/location.component';
import { CreateLocationComponent } from './branch/location/create-location/create-location.component';
import { EditLocationComponent } from './branch/location/edit-location/edit-location.component';

import { VoucherPlatformComponent } from './voucher/voucher-platform/voucher-platform.component';
import { EditVoucherPlatformComponent } from './voucher/voucher-platform/edit-voucher-platform/edit-voucher-platform.component';
import { AddVoucherPlatformComponent } from './voucher/voucher-platform/add-voucher-platform/add-voucher-platform.component';

import { LocationVoucherPlatformComponent } from './branch/location/location-voucher-platform/location-voucher-platform.component';
import { CreateBranchVoucherPlatformComponent } from './branch/location/location-voucher-platform/create-branch-voucher-platform/create-branch-voucher-platform.component';
import { EditBranchVoucherPlatformComponent } from './branch/location/location-voucher-platform/edit-branch-voucher-platform/edit-branch-voucher-platform.component';

import { TenantProfileComponent } from './tenant-profile/tenant-profile.component';
import { UpdateProfileComponent } from './tenant-profile/update-profile/update-profile.component';
import { UpdateBannerComponent } from './voucher/voucher-platform/update-banner/update-banner.component';
import { TermConditionsComponent } from './voucher/voucher-platform/term-conditions/term-conditions.component';
import { UpdateBranchCoverComponent } from './branch/location/update-branch-cover/update-branch-cover.component';
import { LocationDetailsComponent } from './/branch/location/location-details/location-details.component';

@NgModule({
  declarations: [
        AppComponent,
        HomeComponent,
        AboutComponent,
        TopBarComponent,
        TopBarLanguageSwitchComponent,
        SideBarUserAreaComponent,
        SideBarNavComponent,
        SideBarFooterComponent,
        RightSideBarComponent,
        // tenants
        TenantsComponent,
        CreateTenantDialogComponent,
        EditTenantDialogComponent,
        // roles
        RolesComponent,
        CreateRoleDialogComponent,
        EditRoleDialogComponent,
        // users
        UsersComponent,
        CreateUserDialogComponent,
        EditUserDialogComponent,
        ChangePasswordComponent,
        ResetPasswordDialogComponent,

        // categories
        TenantCategoryComponent,
        CreateCategoryComponent,
        EditCategoryComponent,

        // states
        StateComponent,
        CreateStateComponent,
        EditStateComponent,

        // locations
        LocationComponent,
        CreateLocationComponent,
        EditLocationComponent,
        UpdateBranchCoverComponent,
        LocationDetailsComponent,

        // voucher platforms
        VoucherPlatformComponent,
        EditVoucherPlatformComponent,
        TermConditionsComponent,
        //AddVoucherPlatformComponent,
        UpdateBannerComponent,

        // branch voucher platform
        LocationVoucherPlatformComponent,
        CreateBranchVoucherPlatformComponent,
        EditBranchVoucherPlatformComponent,


        // tenant personal
        TenantProfileComponent,
        UpdateProfileComponent,
        
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ModalModule.forRoot(),
    AbpModule,
    AppRoutingModule,
    ServiceProxyModule,
    SharedModule,
    NgxPaginationModule,
    IonRangeSliderModule
  ],
  providers: [],
  entryComponents: [
        // tenants
        CreateTenantDialogComponent,
        EditTenantDialogComponent,
        // roles
        CreateRoleDialogComponent,
        EditRoleDialogComponent,
        // users
        CreateUserDialogComponent,
        EditUserDialogComponent,
        ResetPasswordDialogComponent,

        // categories
        CreateCategoryComponent,
        EditCategoryComponent,

        // states
        CreateStateComponent,
        EditStateComponent,

        // locations
        CreateLocationComponent,
        EditLocationComponent,
        UpdateBranchCoverComponent,

        // branch voucher platforms
        CreateBranchVoucherPlatformComponent,
        EditBranchVoucherPlatformComponent,

        // tenant change profile
        UpdateProfileComponent,

        // voucher platform
        UpdateBannerComponent,
  ]
})
export class AppModule {}
