import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { RootComponent } from './root.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { ErrorLayoutComponent } from './layouts/error-layout/error-layout.component';
import { RootRoutingModule } from './root.routing';
import { NavMenuComponent } from './layouts/nav-menu/nav-menu.component';

@NgModule({
  declarations: [
    RootComponent,
    MainLayoutComponent,
    ErrorLayoutComponent,
    NavMenuComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    RootRoutingModule
    //RouterModule.forRoot([
    //  { path: "", component: HomeComponent, pathMatch: "full" },
    //  { path: "counter", component: CounterComponent },
    //  { path: "fetch-data", component: FetchDataComponent }
    //])
  ],
  providers: [],
  bootstrap: [RootComponent]
})
export class RootModule { }
