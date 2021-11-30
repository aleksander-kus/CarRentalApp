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
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NgxSliderModule} from "@angular-slider/ngx-slider";
import { HttpClientModule } from '@angular/common/http';
import {MatSortModule} from "@angular/material/sort";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { CarSearchService } from "./car-search.service";
import { CarDetailsComponent } from "./car-details/car-details.component";
import { MatListModule } from "@angular/material/list";
import { MatIconModule } from "@angular/material/icon";
import { MatDialogModule } from "@angular/material/dialog";


@NgModule({
  declarations: [
    CarSearchComponent,
    CarDetailsComponent,
    CarSearchFilterComponent,
  ],
  exports: [
    CarSearchComponent
  ],
  providers: [
    CarSearchService
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
    NgxSliderModule,
    HttpClientModule,
    MatSortModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    MatListModule,
    MatIconModule,
    MatDialogModule
  ]
})
export class CarSearchModule {
}
