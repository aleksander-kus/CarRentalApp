import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {Car} from "../../model/car.interface";
import { Role } from "../../../auth/model/role.enum";

@Component({
  selector: 'app-car-details',
  templateUrl: './car-details.component.html',
  styleUrls: ['./car-details.component.css']
})
export class CarDetailsComponent {
  car : Car | undefined;
  companies : string[] = [];

  Client = Role.Client;

  constructor(@Inject(MAT_DIALOG_DATA) public data: {
    car: Car
    cars: Car[]
  }) {
    this.car = data.car;
  }

}
