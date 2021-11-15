import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {catchError} from 'rxjs/operators';
import {protectedResources} from 'src/carsearch.config';
import {Car} from './model/car.interface';

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

  public getData(filter : any): Observable<Car[]> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    let httpParams = new HttpParams();

    Object.keys(filter).forEach(function (key) {
      httpParams = httpParams.append(key, filter[key]);
    });
    console.log(httpParams)
    return this.http.get<Car[]>(protectedResources.carSearchEndpoint, {params: httpParams})
    .pipe(
      catchError(this.handleError<Car[]>('getData', []))
    );
  }

  constructor(private http: HttpClient) { }
}
