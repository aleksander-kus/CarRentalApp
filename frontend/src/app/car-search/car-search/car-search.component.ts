import { Component } from '@angular/core';
import { Car } from "../model/car.interface";
import { CarDataService } from "../cardata.service";

@Component({
  selector: 'app-car-search',
  templateUrl: './car-search.component.html',
  styleUrls: ['./car-search.component.css'],
  providers: [CarDataService]
})

export class CarSearchComponent {
  displayedColumns: string[] = ['brand', 'model', 'productionDate', 'capacity', 'category', 'showdetails'];
  cars: Car[]=[];

  getDataFromService(){
    this.cars = this.dservice.getData()
  }
  constructor(private dservice: CarDataService) { this.getDataFromService() }
}
