import {AfterViewInit, Component, ViewChild} from '@angular/core';
import {Car} from "../model/car.interface";
import {CarDataService} from "../cardata.service";
import {MatTableDataSource} from "@angular/material/table";
import {MatSort} from "@angular/material/sort";
import {MatDialog} from "@angular/material/dialog";
import {CarDetailsComponent} from "../car-details/car-details.component";
import {CarSearchFilterComponent} from "../car-search-filter/car-search-filter.component";

@Component({
  selector: 'app-car-search',
  templateUrl: './car-search.component.html',
  styleUrls: ['./car-search.component.css'],
  providers: [CarDataService]
})


export class CarSearchComponent implements AfterViewInit{
  displayedColumns: string[] = ['brand', 'model', 'productionYear', 'capacity', 'horsePower', 'category', 'providerCompany', 'showDetails'];
  dataSource: MatTableDataSource<Car> = new MatTableDataSource<Car>();

  @ViewChild(MatSort) sort: MatSort
  @ViewChild(CarSearchFilterComponent) filterComponent : CarSearchFilterComponent;

  ngAfterViewInit() {
    this.dataSource.sort = this.sort
    this.getDataFromService()
  }

  getDataFromService(){
    console.log(this.filterComponent.getFilter())
    this.carDataService.getData(this.filterComponent.getFilter()).subscribe(cars =>
    {
      this.dataSource.data = cars
      if(this.filterComponent.cars == null || this.filterComponent.cars.length == 0)
      {
        this.filterComponent.cars = cars
        this.filterComponent.parseCarData()
      }
    });
  }
  constructor(private carDataService: CarDataService, private detailsDialog : MatDialog) {}

  openDetailsDialog(car : Car) {
    this.detailsDialog.open(CarDetailsComponent, {
      data : {
        car : car,
        cars : this.dataSource
      },
      panelClass: 'details-dialog'
    });
  }

}
