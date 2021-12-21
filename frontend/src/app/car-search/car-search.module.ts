import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CarSearchComponent} from './components/car-search/car-search.component';
import {CarSearchFilterComponent} from './components/car-search-filter/car-search-filter.component';
import {MatButtonModule} from '@angular/material/button';
import {FlexLayoutModule} from "@angular/flex-layout";
import {MatSelectModule} from "@angular/material/select";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatCardModule} from "@angular/material/card";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NgxSliderModule} from "@angular-slider/ngx-slider";
import { HttpClientModule } from '@angular/common/http';
import {MatSortModule} from "@angular/material/sort";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { CheckPriceService } from "./data/check-price.service";
import { CarSearchService } from "./data/car-search.service";
import { CarDetailsComponent } from "./components/car-details/car-details.component";
import { MatTableModule } from "@angular/material/table";
import { MatListModule } from "@angular/material/list";
import { MatIconModule } from "@angular/material/icon";
import { MatDialogModule } from "@angular/material/dialog";
import { MatInputModule } from "@angular/material/input";
import { CheckPriceComponent } from "./components/check-price/check-price.component";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { CarBookService } from "./data/car-book.service";
import { MatNativeDateModule } from "@angular/material/core";
import { CarReservationPeriodComponent } from "./components/car-reservation-period/car-reservation-period.component";
import { AuthModule } from "../auth/auth.module";
import {HistoryModule} from "../history/history.module";


@NgModule({
  declarations: [
    CarDetailsComponent,
    CarSearchComponent,
    CarDetailsComponent,
    CarSearchFilterComponent,
    CarReservationPeriodComponent,
    CheckPriceComponent,
  ],
  exports: [
    CarSearchComponent
  ],
  providers: [
    CarSearchService,
    CarBookService,
    CheckPriceService
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatNativeDateModule,
    MatTableModule,
    MatSnackBarModule,
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
    MatDialogModule,
    MatIconModule,
    MatInputModule,
    MatDatepickerModule,
    AuthModule,
    HistoryModule
  ]
})
export class CarSearchModule {
}
