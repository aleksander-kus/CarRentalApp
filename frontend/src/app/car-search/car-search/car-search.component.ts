import { Component } from '@angular/core';
import { Car } from "../model/car.interface";
import { CarDataService } from "../cardata.service";
import {MatTableDataSource} from "@angular/material/table";

@Component({
  selector: 'app-car-search',
  templateUrl: './car-search.component.html',
  styleUrls: ['./car-search.component.css'],
  providers: [CarDataService]
})


export class CarSearchComponent {
  displayedColumns: string[] = ['brand', 'model', 'productionYear', 'capacity', 'category', 'showdetails'];
  dataSource: MatTableDataSource<Car> = new MatTableDataSource<Car>();

  getDataFromService(){
    this.dservice.getData().subscribe(cars => this.dataSource.data = cars)
  }
  constructor(private dservice: CarDataService) { this.getDataFromService() }
}
