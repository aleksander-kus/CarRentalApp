import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {HistoryEntry} from "../model/history-entry.interface";
import {FormControl} from "@angular/forms";
import {MatDialog} from "@angular/material/dialog";
import {AuthService} from "../../auth/auth.service";
import {filter, map, switchAll} from "rxjs/operators";
import {ReturnCarService} from "../return-car.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {HttpErrorResponse, HttpEventType} from "@angular/common/http";
import {UploadFileService} from "../upload-file.service";
import {UploadFileResponse} from "../model/upload-file-response.inteface";

@Component({
  selector: 'app-return-car',
  templateUrl: './return-car.component.html',
  styleUrls: ['./return-car.component.css']
})
export class ReturnCarComponent {
  historyEntry : HistoryEntry;

  carConditionControl = new FormControl();
  odometerValueControl = new FormControl();
  selectedPhotoFile: string;
  selectedPdfFile: string;
  photoFileId = "";
  pdfFileId = "";
  photoProgress: number;
  pdfProgress: number;


  constructor(@Inject(MAT_DIALOG_DATA) public data: {historyEntry: HistoryEntry},
              private auth: AuthService,
              private returnCar: ReturnCarService,
              private uploadFile: UploadFileService,
              private snackBar: MatSnackBar,
              private matModal: MatDialog) {
    this.historyEntry = data.historyEntry;
  }

  selectPhotoFile(event: Event): void {
    const files = (event.target as HTMLInputElement).files;
    if (files != null) {
      const file = <File>files.item(0);
      const formData = new FormData();
      formData.append(file.name, file);
      this.selectedPhotoFile = file.name;
      this.auth.requireLogin()
        .pipe(
          filter(v => v),
          map(() => this.uploadFile.uploadFile(formData)), switchAll()
        ).subscribe(event => {
          if (event.type === HttpEventType.UploadProgress) {
            const total = event?.total as number;
            this.photoProgress = Math.round(100*event.loaded / total);
          }
        if (event.type === HttpEventType.Response) {
          const response = event.body as UploadFileResponse;
          this.photoFileId = response.fileId;
        }
      }, (errorRes: HttpErrorResponse) => console.log(errorRes.error.error));
    }
  }
  selectPdfFile(event: Event): void {
    const files = (event.target as HTMLInputElement).files;
    if (files != null) {
      const file = <File>files.item(0);
      const formData = new FormData();
      formData.append(file.name, file);
      this.selectedPdfFile = file.name;
      this.auth.requireLogin()
        .pipe(
          filter(v => v),
          map(() => this.uploadFile.uploadFile(formData)), switchAll()
        ).subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          const total = event?.total as number;
          this.pdfProgress = Math.round(100*event.loaded / total);
        }
        if (event.type === HttpEventType.Response) {
          const response = event.body as UploadFileResponse;
          this.pdfFileId = response.fileId;
        }
      }, (errorRes: HttpErrorResponse) => console.log(errorRes.error.error));
    }
  }

  tryReturnCar(): void {
    if(this.historyEntry !== null && this.pdfFileId != "" && this.photoFileId != "") {
      const carReturnRequest =  {
        rentId: this.historyEntry.rentId,
        historyEntryId: this.historyEntry.id.toString(),
        userEmail: this.historyEntry.userEmail,
        rentDate: new Date(Date.parse(this.historyEntry.rentDate)),
        returnDate: new Date(Date.parse(this.historyEntry.returnDate)),
        carCondition: this.carConditionControl.value,
        odometerValue: this.odometerValueControl.value,
        photoFileId: this.photoFileId,
        pdfFileId: this.pdfFileId
      };
      this.auth.requireLogin()
        .pipe(
          filter(v => v),
          map(() => this.returnCar.returnCar(this.historyEntry.carProvider, this.historyEntry?.carId, carReturnRequest)),
          switchAll()
        ).subscribe(res => {
          this.snackBar.open(`Car successfully returned`, undefined,
            {duration: 10000, panelClass: 'snack-success'});
          this.matModal.closeAll();
        }, (errorRes: HttpErrorResponse) => this.snackBar.open(`Cannot return this car: ${errorRes.error.error}`,
        undefined, {duration: 10000, panelClass: 'snack-fail'}));
    }
  }
}
