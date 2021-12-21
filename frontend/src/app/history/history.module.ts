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

@NgModule({
  declarations: [
    RentalHistoryComponent,
    CurrentlyRentedComponent
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
        RouterModule
    ]
})
export class HistoryModule { }
