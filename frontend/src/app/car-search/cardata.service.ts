import {Injectable} from '@angular/core';
import {Car} from './model/car.interface';
import {CarCategory} from './model/carcategory.enum';

@Injectable({
  providedIn: 'root'
})
export class CarDataService {
  // A placeholder array for testing the list
  // Pulling actual entries will be added later
  private carList: Car[] = [{
    Brand: "Opel",
    Model: "Astra",
    ProductionDate: new Date(2020, 1),
    Category: CarCategory.Medium,
    Capacity: 5
  },
    {Brand: "Honda", Model: "Civic", ProductionDate: new Date(2019, 2), Category: CarCategory.Small, Capacity: 4},
    {Brand: "Seat", Model: "Ibiza", ProductionDate: new Date(2014, 1), Category: CarCategory.Big, Capacity: 6},
    {Brand: "Honda", Model: "Escapado", ProductionDate: new Date(2019, 2), Category: CarCategory.XXL, Capacity: 7},
    {Brand: "Opel", Model: "Insignia", ProductionDate: new Date(2019, 2), Category: CarCategory.Big, Capacity: 6},
    {Brand: "Opel", Model: "Insignia", ProductionDate: new Date(2019, 2), Category: CarCategory.Big, Capacity: 6},
  ]

  public getData(): Car[] {
    return this.carList;
  }
}
