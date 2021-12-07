import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from "rxjs";
import { Car } from "./model/car.interface";
import { HttpClient, HttpParams } from "@angular/common/http";
import { CarProvider } from "./model/car-provider.interface";
import { protectedResources } from "../../auth.config";
import { finalize, map, mergeAll } from "rxjs/operators";
import { CarFilter } from "./model/car-filter.interface";
import { ApiResponse } from "../common/api-response.interface";

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

    let cars: Car[] = [];

    this.http.get<CarProvider[]>(protectedResources.carProvidersApi.endpoint)
      .pipe(
        map(providers => providers
          .map(p => this.http.get<ApiResponse<Car[]>>(protectedResources.carSearchApi(p.id).endpoint, {params: httpParams}))),
        mergeAll(),
        mergeAll(),
        finalize(() => this.isLoadingSubject.next(false)),
      ).subscribe(
      part => {
          if (part.data) {
            cars = [...cars, ...(part.data)];
            this.carsSubject.next(cars);
          }
        },
        error => console.error(error)
    );
  }
}
