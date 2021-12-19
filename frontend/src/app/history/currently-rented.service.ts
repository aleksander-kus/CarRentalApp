import { Injectable } from '@angular/core';
import { HistoryEntry } from "./model/history-entry.interface";
import { of } from "rxjs";
import { CarProvider } from "../car-search/model/car-provider.interface";

@Injectable({
  providedIn: 'root'
})
export class CurrentlyRentedService {
  private provider : CarProvider = {id: "DNR", name: "DotNetRulez"}
  private entryList: HistoryEntry[]=[
    {rentDate : new Date('2021-12-09T12:00:00'), returnDate: null, carId: 1, provider: this.provider, brand:"Opel", model:"Astra"},
    {rentDate : new Date('2021-12-02T12:00:00'), returnDate: null, carId: 2, provider: this.provider, brand:"Seat", model:"Ibiza"},
    {rentDate : new Date('2021-11-09T12:00:00'), returnDate: null, carId: 3, provider: this.provider, brand:"Honda", model:"Civic"},
    {rentDate : new Date('2021-12-09T11:00:00'), returnDate: null, carId: 4, provider: this.provider, brand:"Porsche", model:"Panamera"},
    {rentDate : new Date('2020-12-09T12:00:00'), returnDate: null, carId: 5, provider: this.provider, brand:"Audi", model:"A4"},
  ]

  public entries$ = of(this.entryList);

  public getEntries(): void {
    true;
  }
}
