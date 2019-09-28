import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ManageCategoriesComponent } from './manage-categories.component';

const routes: Routes = [
  {
    path: "",
    component: ManageCategoriesComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageCategoriesRoutingModule { }
