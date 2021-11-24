import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {Car} from "../model/car.interface";

@Component({
  selector: 'app-car-details',
  templateUrl: './car-details.component.html',
  styleUrls: ['./car-details.component.css']
})
export class CarDetailsComponent {
  car : Car | undefined;
  companies : string[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) public data: {
    car: Car
    cars: Car[]
  }) {
    this.car = data.car;
  }

  openCheckPriceDialog() {
    //TODO: add check price dialog and remove this log below
    console.log("open dialog")
  }

}
