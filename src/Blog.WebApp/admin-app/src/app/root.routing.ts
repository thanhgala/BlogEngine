import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { ErrorLayoutComponent } from './layouts/error-layout/error-layout.component';
const appRoutes: Routes = [
  {
    path: "",
    component: MainLayoutComponent,
    children: [
      { path: "", redirectTo: "dashboard", pathMatch: "full" },
      { path: "dashboard", loadChildren: "./component/dashboard/dashboard.module#DashboardModule" },
      { path: "manage-categories", loadChildren: "./component/manage-categories/manage-categories.module#ManageCategoriesModule" }
    ]
  },
  {
    path: "error",
    component: ErrorLayoutComponent,
    children: [
      { path: "", loadChildren: "./component/error/error.module#ErrorModule" }
    ]
  },
  { path: "**", redirectTo: "/error/page-not-found" }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes, {useHash:true})],
  exports: [RouterModule]
})

export class RootRoutingModule { }
