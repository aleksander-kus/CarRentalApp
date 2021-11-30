import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import {Car} from "../model/car.interface";
import {MatTableDataSource} from "@angular/material/table";
import {MatSort} from "@angular/material/sort";
import {MatDialog} from "@angular/material/dialog";
import {CarDetailsComponent} from "../car-details/car-details.component";
import { CarSearchService } from "../car-search.service";
import { CarFilter } from "../model/car-filter.interface";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

@Component({
  selector: 'app-car-search',
  templateUrl: './car-search.component.html',
  styleUrls: ['./car-search.component.css']
})
export class CarSearchComponent implements AfterViewInit, OnDestroy, OnInit {
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['brand', 'model', 'productionYear', 'capacity', 'horsePower', 'category', 'providerCompany', 'showDetails'];
  dataSource: MatTableDataSource<Car> = new MatTableDataSource<Car>();
  loading$ = this.carSearch.isLoading$;
  cars$ = this.carSearch.cars$;

  private filter: CarFilter;
  private _ngUnsubscribe = new Subject();

  constructor(private carSearch: CarSearchService, private detailsDialog : MatDialog) {}

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.getDataFromService();
  }

  getDataFromService(): void {
    this.carSearch.search(this.filter);
  }

  filterUpdated(filter: CarFilter): void {
    this.filter = filter;
  }

  openDetailsDialog(car: Car): void {
    this.detailsDialog.open(CarDetailsComponent, {
      data: {
        car: car,
        cars: this.dataSource
      },
      panelClass: 'details-dialog'
    });
  }

  ngOnDestroy(): void {
    this._ngUnsubscribe.next();
    this._ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.cars$.pipe(takeUntil(this._ngUnsubscribe)).subscribe(cars => this.dataSource.data = cars);
  }

}
