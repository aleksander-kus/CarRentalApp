import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CurrentlyRentedService } from "../currently-rented.service";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { HistoryEntry } from "../model/history-entry.interface";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { AuthService } from "../../auth/auth.service";
import { Role } from "../../auth/model/role.enum";

@Component({
  selector: 'app-currently-rented',
  templateUrl: './currently-rented.component.html',
  styleUrls: ['./currently-rented.component.css']
})
export class CurrentlyRentedComponent implements AfterViewInit, OnInit, OnDestroy {
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['brand', 'model', 'rentDate', 'provider', 'returnMe'];
  dataSource: MatTableDataSource<HistoryEntry> = new MatTableDataSource<HistoryEntry>();
  entries$ = this.currentlyRented.entries$;
  public Employee = Role.Employee;

  private _ngUnsubscribe = new Subject();

  constructor(private currentlyRented: CurrentlyRentedService,
              private authService: AuthService) {
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.getDataFromService()
  }

  getDataFromService(): void {
    this.currentlyRented.getEntries()
  }

  ngOnDestroy(): void {
    this._ngUnsubscribe.next();
    this._ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.entries$.pipe(takeUntil(this._ngUnsubscribe)).subscribe(entries => this.dataSource.data = entries);
  }
}
