import {Component, Input} from "@angular/core";
import {AuthService} from "../../../auth/auth.service";
import {CarPrice} from "../../model/car-price.interface";
import {Car} from "../../model/car.interface";
import {CheckPriceService} from "../../data/check-price.service";
import {filter, map, switchAll} from "rxjs/operators";
import {MatDialog} from "@angular/material/dialog";
import {CarBookService} from "../../data/car-book.service";
import {CarReservationPeriodComponent} from "../car-reservation-period/car-reservation-period.component";
import {CarReservationPeriod} from "../car-reservation-period/car-reservation-period.interface";
import {MatSnackBar} from "@angular/material/snack-bar";
import {FormControl} from "@angular/forms";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-check-price',
  templateUrl: './check-price.component.html',
  styleUrls: ['./check-price.component.css']
})
export class CheckPriceComponent {
  @Input() car: Car;

  daysCountControl = new FormControl(1);
  carPrice: CarPrice | null = null;
  loadingPrice$ = this.checkPrice.isLoading$;
  loadingBooking$ = this.bookCar.loading$;

  constructor(
    private checkPrice: CheckPriceService,
    private bookCar: CarBookService,
    private matModal: MatDialog,
    private snackBar: MatSnackBar,
    private auth: AuthService) {
  }

  checkCarPrice(): void {
    this.auth.requireLogin()
      .pipe(
        filter(v => v),
        map(() => this.checkPrice.checkPrice(this.car.providerId, this.car.id, {
          daysCount: this.daysCountControl.value
        })),
        switchAll()
      ).subscribe(
        res => this.carPrice = res.data as CarPrice,
      (errorRes: HttpErrorResponse) => this.snackBar.open(`Cannot check price: ${errorRes.error.error}`,
        undefined, { duration: 10000, panelClass: 'snack-fail' })
    );
  }

  tryRentCar(): void {
    this.matModal.open(CarReservationPeriodComponent, { data: { daysCount: this.daysCountControl.value }}).afterClosed()
      .pipe(
        filter(v => v),
        map((v: CarReservationPeriod) => this.bookCar.tryBookCar(this.car.id, this.car.providerId, {
          rentFrom: v.from,
          rentTo: v.to,
          priceId: this.carPrice?.id as string,
          carBrand: this.car.brand,
          carModel: this.car.model
        })),
        switchAll()
      ).subscribe(res => {
          this.snackBar.open(`Car successfully booked, reservation id is ${res.data?.rentId}`,
            undefined, { duration: 10000, panelClass: 'snack-success' });
          this.matModal.closeAll();
      }, (errorRes: HttpErrorResponse) => this.snackBar.open(`Cannot book this car: ${errorRes.error.error}`,
      undefined, { duration: 10000, panelClass: 'snack-fail' }));
  }
}


