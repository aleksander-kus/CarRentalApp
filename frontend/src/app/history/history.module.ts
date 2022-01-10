import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RentalHistoryComponent } from './rental-history/rental-history.component';
import { CurrentlyRentedComponent } from './currently-rented/currently-rented.component';
import { MatTableModule } from "@angular/material/table";
import { MatSortModule } from "@angular/material/sort";
import { CurrentlyRentedService } from "./currently-rented.service";
import { MatButtonModule } from "@angular/material/button";
import { AuthModule } from "../auth/auth.module";
import {FlexModule} from "@angular/flex-layout";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {RouterModule} from "@angular/router";
import { ReturnCarComponent } from './return-car/return-car.component';
import {MatInputModule} from "@angular/material/input";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatIconModule} from "@angular/material/icon";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatProgressBarModule} from "@angular/material/progress-bar";

@NgModule({
  declarations: [
    RentalHistoryComponent,
    CurrentlyRentedComponent,
    ReturnCarComponent
  ],
  exports: [
    CurrentlyRentedComponent
  ],
  providers: [
    CurrentlyRentedService
  ],
    imports: [
        CommonModule,
        MatTableModule,
        MatSortModule,
        MatButtonModule,
        AuthModule,
        FlexModule,
        MatProgressSpinnerModule,
        RouterModule,
        MatInputModule,
        ReactiveFormsModule,
        MatIconModule,
        MatToolbarModule,
        FormsModule,
        MatProgressBarModule
    ]
})
export class HistoryModule { }
