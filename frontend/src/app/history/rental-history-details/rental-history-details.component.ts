import { Component, Inject } from '@angular/core';
import { Role } from "../../auth/model/role.enum";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { HistoryEntry } from "../model/history-entry.interface";
import { GetFileUri } from "../get-file-uri";
import { FileUriResponse } from "../model/file-uri-response";

@Component({
  selector: 'app-rental-history-details',
  templateUrl: './rental-history-details.component.html',
  styleUrls: ['./rental-history-details.component.css']
})
export class RentalHistoryDetailsComponent{
  entry : HistoryEntry | undefined;

  Client = Role.Client;

  constructor(@Inject(MAT_DIALOG_DATA) public data: {
    entry: HistoryEntry
  }, private downloadFileService: GetFileUri) {
    this.entry = data.entry;
  }

  formatDate(date: string | undefined): string {
    return (date) ? new Date(Date.parse(date)).toLocaleString('pl') : "";
  }

  public getFileUri(fileName: string | undefined) : void {
    if(!fileName)
      return;
    this.downloadFileService.getFileSas(fileName).subscribe((response: FileUriResponse) => {
      if(response && response.uri)
        window.open(response.uri)
    });
  }
}
