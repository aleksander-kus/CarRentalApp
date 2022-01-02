import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {HistoryEntry} from "./model/history-entry.interface";
import {protectedResources} from "../../auth.config";
import {finalize} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class RentalHistoryService {
  private loadSubject = new BehaviorSubject(false);
  loading$ = this.loadSubject.asObservable();

  constructor(private http: HttpClient) { }

  public loadEntries(all: boolean): Observable<HistoryEntry[]> {
    this.loadSubject.next(true);

    return this.http.get<HistoryEntry[]>(protectedResources.rentalHistoryApi(all).endpoint)
      .pipe(
        finalize(() => this.loadSubject.next(false))
      );
  }
}
