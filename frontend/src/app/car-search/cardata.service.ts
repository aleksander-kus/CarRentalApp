import { Injectable } from '@angular/core';
import { Car } from './model/car.interface';

@Injectable({
  providedIn: 'root'
})
export class CarDataService {
  // A placeholder array for testing the list
  // private carList: Car[]=[
  //   {brand:"Opel", model:"Astra", productionYear: 2020, category: "Medium", capacity: 5, horsePower: 100, id: "1", providerCompany: "AAA"},
  //   {brand:"Honda", model:"Civic", productionYear: 2019, category: "Small", capacity: 4, horsePower: 100, id: 1, providerCompany: "AAA"},
  //   {brand:"Seat", model:"Ibiza", productionYear: 2014, category: "Big", capacity: 6, horsePower: 100, id: 1, providerCompany: "AAA"},
  //   {brand:"Honda", model:"Escapado", productionYear: 2019, category: "XXL", capacity: 7, horsePower: 100, id: 1, providerCompany: "AAA"},
  //   {brand:"Opel", model:"Insignia", productionYear: 2019, category: "Big", capacity: 6, horsePower: 100, id: 1, providerCompany: "AAA"},
  //   {brand:"Opel", model:"Insignia", productionYear: 2019, category: "Big", capacity: 6, horsePower: 100, id: 1, providerCompany: "AAA"},
  // ]

  public getData():Car[]{
    return [];
  }
}
