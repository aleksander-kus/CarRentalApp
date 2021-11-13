import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CarSearchComponent} from './car-search/car-search.component';
import {CarSearchFilterComponent} from './car-search-filter/car-search-filter.component';
import {MatButtonModule} from '@angular/material/button';
import {MatTableModule} from '@angular/material/table';
import {FlexLayoutModule} from "@angular/flex-layout";
import {MatSelectModule} from "@angular/material/select";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatCardModule} from "@angular/material/card";
import {FormsModule} from "@angular/forms";
import {NgxSliderModule} from "@angular-slider/ngx-slider";


@NgModule({
  declarations: [
    CarSearchComponent,
    CarSearchFilterComponent,
  ],
  exports: [
    CarSearchComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatTableModule,
    FlexLayoutModule,
    MatSelectModule,
    MatCheckboxModule,
    MatCardModule,
    FormsModule,
    NgxSliderModule
  ]
})
export class CarSearchModule {
}
