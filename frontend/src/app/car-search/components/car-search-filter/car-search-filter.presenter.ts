import { Injectable } from '@angular/core';
import { CarFilter } from "../../model/car-filter.interface";
import { Observable } from "rxjs";
import { Car } from "../../model/car.interface";
import { filter, first, map, reduce, scan, take, tap } from "rxjs/operators";
import { K } from "@angular/cdk/keycodes";

function onlyUnique<T>(value: T, index: number, self: T[]) {
  return self.indexOf(value) === index;
}

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
        .filter(c => !!c)),
      scan(
        (acc, val) => [...acc, ...val]
          .filter(onlyUnique), [] as string[])
    );
  }

  mapCarsToBrands(cars: Observable<Car[]>): Observable<Map<string, string[]>> {
    return cars.pipe(
      filter(cars => !!cars && cars.length > 0),
      map(c => c.reduce((total, current) => {
        total.set(current.brand, [...(total.get(current.brand) ?? []), current.model].filter((v, i, a) => a.indexOf(v) === i));
        return total;
      }, new Map<string, string[]>())),
      scan((acc, val: Map<string, string[]>) =>  {
        val.forEach((value, key) => {
          if (acc.has(key)) {
            acc.set(key, [...value, ...(acc.get(key) as string[])].filter(onlyUnique))
          } else {
           acc.set(key, value);
          }
        });
        return acc;
      }, new Map<string, string[]>())
    );
  }
}
