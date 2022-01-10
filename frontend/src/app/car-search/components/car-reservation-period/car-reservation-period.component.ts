import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormBuilder, FormControl } from "@angular/forms";
import { CarReservationPeriod } from "./car-reservation-period.interface";

@Component({
  selector: 'app-car-reservation-period',
  templateUrl: './car-reservation-period.component.html',
  styleUrls: ['./car-reservation-period.component.css']
})
export class CarReservationPeriodComponent {
  today = new Date();
  minDate = new Date(this.today.getFullYear(), this.today.getMonth(), this.today.getDay());
  start = new FormControl(this.minDate);

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: { daysCount: number },
    private dialog: MatDialogRef<CarReservationPeriod>,
    private fb: FormBuilder) { }

  tryBook(): void {
    const data: CarReservationPeriod = {
      from: this.start.value as Date,
      to: new Date((this.start.value as Date).getTime() + (1000 * 60 * 60 * 24 * this.data.daysCount))
    };

    this.dialog.close(data);
  }

  cancel(): void {
    this.dialog.close(null);
  }
}
