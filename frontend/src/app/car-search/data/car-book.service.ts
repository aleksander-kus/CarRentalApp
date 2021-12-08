import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, of } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { CarBookRequest } from "../model/car-book-request.interface";
import { protectedResources } from "../../../auth.config";
import { catchError, finalize, map, tap } from "rxjs/operators";
import { ApiResponse } from "../../common/api-response.interface";
import { CarBookResponse } from "../model/car-book-response.interface";


@Injectable()
export class CarBookService {
  private loadSubject = new BehaviorSubject(false);
  loading$ = this.loadSubject.asObservable();

  constructor(private http: HttpClient) {
  }

  tryBookCar(carId: string, providerId: string, request: CarBookRequest): Observable<ApiResponse<CarBookResponse>> {
    this.loadSubject.next(true);

    return this.http.post<ApiResponse<CarBookResponse>>(protectedResources.carRentApi(providerId, carId).endpoint, request)
      .pipe(
        finalize(() => this.loadSubject.next(false))
      );
  }
}
