import { Injectable } from '@angular/core';
import { CarFilter } from "../model/car-filter.interface";
import { Observable } from "rxjs";
import { Car } from "../model/car.interface";
import { filter, first, map, take, tap } from "rxjs/operators";

@Injectable()
export class CarSearchFilterPresenter {
  getDefaultFilerValues(): CarFilter {
    return {
      brands: [],
      models: [],
      categories: [],
      capacityMin: 2,
      capacityMax: 11,
      productionYearMin : 1950,
      productionYearMax : 2021,
      horsePowerMin: 0,
      horsePowerMax: 5200
    };
  }

  mapCarsToCategories(cars: Observable<Car[]>): Observable<string[]> {
    return cars.pipe(
      filter(cars => !!cars && cars.length > 0),
      map(cars => cars
        .map(c => c.category)
        .filter((v, i, a) => a.indexOf(v) === i)),
      first()
    );
  }

  mapCarsToBrands(cars: Observable<Car[]>): Observable<Map<string, string[]>> {
    return cars.pipe(
      filter(cars => !!cars && cars.length > 0),
      map(c => c.reduce((total, current) => {
        total.set(current.brand, [...(total.get(current.brand) ?? []), current.model].filter((v, i, a) => a.indexOf(v) === i));
        return total;
      }, new Map<string, string[]>())),
      first()
    );
  }
}
