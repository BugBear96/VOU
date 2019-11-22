import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';

import { TenantCategoryComponent } from './tenant-category/tenant-category.component';
import { StateComponent } from './branch/state/state.component';

import { LocationComponent } from './branch/location/location.component';
import { LocationVoucherPlatformComponent } from './branch/location/location-voucher-platform/location-voucher-platform.component';

import { VoucherPlatformComponent } from './voucher/voucher-platform/voucher-platform.component';
import { EditVoucherPlatformComponent } from './voucher/voucher-platform/edit-voucher-platform/edit-voucher-platform.component';
import { AddVoucherPlatformComponent } from './voucher/voucher-platform/add-voucher-platform/add-voucher-platform.component';


import { TenantProfileComponent } from './tenant-profile/tenant-profile.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'home', component: HomeComponent,  canActivate: [AppRouteGuard] },
                    { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },

                    { path: 'tenant', component: TenantProfileComponent, data: { permission: 'Tenant' }, canActivate: [AppRouteGuard] },

                    { path: 'tenant/category', component: TenantCategoryComponent, data: { permission: 'Administration.Categories' }, canActivate: [AppRouteGuard] },
                    { path: 'states', component: StateComponent, data: { permission: 'Administration.States' }, canActivate: [AppRouteGuard] },

                    { path: 'branches', component: LocationComponent, data: { permission: 'Branch' }, canActivate: [AppRouteGuard] },
                    { path: 'branch/voucher-platform/:id', component: LocationVoucherPlatformComponent, data: { permission: 'Branch' }, canActivate: [AppRouteGuard] },

                    { path: 'voucher-platform', component: VoucherPlatformComponent, data: { permission: 'VoucherPlatform' }, canActivate: [AppRouteGuard] },
                    { path: 'voucher-platform/edit/:id', component: EditVoucherPlatformComponent, data: { permission: 'VoucherPlatform' }, canActivate: [AppRouteGuard] },
                    { path: 'voucher-platform/add', component: EditVoucherPlatformComponent, data: { permission: 'VoucherPlatform.Create' }, canActivate: [AppRouteGuard] },

                    { path: 'about', component: AboutComponent },
                    { path: 'update-password', component: ChangePasswordComponent }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
