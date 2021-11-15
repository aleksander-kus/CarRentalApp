import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { protectedResources } from 'src/carsearch.config';
import { Car } from './model/car.interface';

@Injectable({
  providedIn: 'root'
})
export class CarDataService {

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }

  public getData(): Observable<Car[]> {
    return this.http.get<Car[]>(protectedResources.carSearchEndpoint)
    .pipe(
      catchError(this.handleError<Car[]>('getData', []))
    );
  }

  constructor(private http: HttpClient) { }
}
