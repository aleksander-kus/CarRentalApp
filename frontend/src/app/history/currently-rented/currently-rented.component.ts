import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {CurrentlyRentedService} from "../currently-rented.service";
import {MatSort} from "@angular/material/sort";
import {MatTableDataSource} from "@angular/material/table";
import {HistoryEntry} from "../model/history-entry.interface";
import {AuthService} from "../../auth/auth.service";
import {Role} from "../../auth/model/role.enum";
import {CarDetailsComponent} from "../../car-search/components/car-details/car-details.component";
import {MatDialog} from "@angular/material/dialog";
import {ReturnCarComponent} from "../return-car/return-car.component";

@Component({
  selector: 'app-currently-rented',
  templateUrl: './currently-rented.component.html',
  styleUrls: ['./currently-rented.component.css']
})
export class CurrentlyRentedComponent implements AfterViewInit, OnInit{
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['brand', 'model', 'rentDate', 'returnDate', 'provider', 'userEmail', 'returnMe'];
  dataSource: MatTableDataSource<HistoryEntry> = new MatTableDataSource<HistoryEntry>();
  public Employee = Role.Employee;
  loading$ = this.currentlyRented.loading$;

  constructor(private currentlyRented: CurrentlyRentedService,
              private authService: AuthService,
              private returnMeDialog : MatDialog) {
  }

  ngOnInit(): void {
    // if the user is an employee, return cars from all users, else just for the current user
    this.authService.user$.subscribe(user => this.getHistoryEntriesFromService(!!user && user.role === this.Employee));
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  getHistoryEntriesFromService(all: boolean): void {
    this.currentlyRented.loadEntries(all).subscribe(entries => this.dataSource.data = entries);
  }

  formatDate(date: string): string {
    return new Date(Date.parse(date)).toLocaleString('pl');
  }

  openReturnMeDialog(entry: HistoryEntry): void {
    this.returnMeDialog.open(ReturnCarComponent, {
      data: {
        historyEntry: entry
      },
      panelClass: 'return-me-dialog'
    });
  }
}
