import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { protectedResources } from "../../../auth.config";
import { ApiResponse } from "../../common/api-response.interface";
import {CarReturnResponse} from "../model/car-return-response.interface";
import {CarReturnRequest} from "../model/car-return-request.interface";
import {finalize} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})

export class ReturnCarService {
  private loadSubject = new BehaviorSubject(false);
  loading$ = this.loadSubject.asObservable();

  constructor(private http: HttpClient) { }

  returnCar(providerId: string, carId: string, carReturnRequest: CarReturnRequest): Observable<ApiResponse<CarReturnResponse>> {
    this.loadSubject.next(true);

    return this.http.post<ApiResponse<CarReturnResponse>>(protectedResources.carReturnApi(providerId, carId).endpoint, carReturnRequest)
      .pipe(
        finalize(() => this.loadSubject.next(false))
      );
  }
}
