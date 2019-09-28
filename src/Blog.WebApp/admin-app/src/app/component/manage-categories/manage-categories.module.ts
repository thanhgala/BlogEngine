import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageCategoriesComponent } from './manage-categories.component';

import { ManageCategoriesRoutingModule } from './manage-categories-routing.module';

@NgModule({
  imports: [
    CommonModule,
    ManageCategoriesRoutingModule
  ],
  declarations: [ManageCategoriesComponent]
})
export class ManageCategoriesModule { }
