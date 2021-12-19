import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RentalHistoryComponent } from './rental-history/rental-history.component';
import { CurrentlyRentedComponent } from './currently-rented/currently-rented.component';
import { MatTableModule } from "@angular/material/table";
import { MatSortModule } from "@angular/material/sort";
import { CurrentlyRentedService } from "./currently-rented.service";
import { MatButtonModule } from "@angular/material/button";
import { AuthModule } from "../auth/auth.module";

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
    AuthModule
  ]
})
export class HistoryModule { }
