import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from "rxjs";
import { Car } from "../model/car.interface";
import { HttpClient, HttpParams } from "@angular/common/http";
import { CarProvider } from "../model/car-provider.interface";
import { finalize, map, mergeAll, tap } from "rxjs/operators";
import { CarFilter } from "../model/car-filter.interface";
import { environment } from "../../../environments/environment";
import { ApiResponse } from "../../common/api-response.interface";

@Injectable()
export class CarSearchService {
  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  public isLoading$ = this.isLoadingSubject.asObservable();
  private carsSubject = new BehaviorSubject<Car[]>([]);
  public cars$ = this.carsSubject.asObservable();

  constructor(private http: HttpClient) {
  }

  public search(filters: CarFilter): void {
    this.isLoadingSubject.next(true);

    const httpParams: HttpParams = Object.entries(filters ?? {})
      .reduce<HttpParams>((total, [key, value]) => {
        return total.append(key, value as string);
      }, new HttpParams());

    this.carsSubject.next([]);
    let cars: Car[] = [];

    this.http.get<CarProvider[]>(`${environment.apiUrl}/api/cars/providers`)
      .pipe(
        map(providers => providers
          .map(p => this.http.get<ApiResponse<Car[]>>(`${environment.apiUrl}/api/cars/${p.id}`, {params: httpParams}))),
        mergeAll(),
        mergeAll()
      ).subscribe(
      part => {
          if (part.data) {
            cars = [...cars, ...(part.data)];
            this.carsSubject.next(cars);
          }
        },
        error => console.error(error),
      () => this.isLoadingSubject.next(false)
    );
  }
}
