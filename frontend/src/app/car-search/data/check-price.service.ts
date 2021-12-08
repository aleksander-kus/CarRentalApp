import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { CarCheckPrice } from "../model/car-check-price.interface";
import { CarPrice } from "../model/car-price.interface";
import { protectedResources } from "../../../auth.config";
import { finalize } from "rxjs/operators";
import { ApiResponse } from "../../common/api-response.interface";

@Injectable()
export class CheckPriceService {
  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  public isLoading$ = this.isLoadingSubject.asObservable();

  constructor(private http: HttpClient) { }

  public checkPrice(providerId: string, carId: string, checkRequest: CarCheckPrice): Observable<ApiResponse<CarPrice>> {
    this.isLoadingSubject.next(true);

    return this.http.post<ApiResponse<CarPrice>>(protectedResources.carCheckPriceApi(providerId, carId).endpoint, checkRequest)
      .pipe(
        finalize(() => this.isLoadingSubject.next(false))
      );
  }
}
