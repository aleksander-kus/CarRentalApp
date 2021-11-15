import {AfterViewInit, Component, OnChanges, OnInit, ViewChild} from '@angular/core';
import { Car } from "../model/car.interface";
import { CarDataService } from "../cardata.service";
import {MatTableDataSource} from "@angular/material/table";
import {MatSort, MatSortable} from "@angular/material/sort";

@Component({
  selector: 'app-car-search',
  templateUrl: './car-search.component.html',
  styleUrls: ['./car-search.component.css'],
  providers: [CarDataService]
})


export class CarSearchComponent implements AfterViewInit{
  displayedColumns: string[] = ['Brand', 'Model', 'ProductionYear', 'Capacity', 'Category', 'ShowDetails'];
  dataSource: MatTableDataSource<Car> = new MatTableDataSource<Car>();

  @ViewChild(MatSort) sort: MatSort

  ngAfterViewInit() {
    this.dataSource.sort = this.sort
  }

  getDataFromService(){
    this.dservice.getData().subscribe(cars => this.dataSource.data = cars);
  }
  constructor(private dservice: CarDataService) { }
}
