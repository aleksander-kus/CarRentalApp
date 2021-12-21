import {CurrentlyRentedCar} from "./currently-rented-car.interface";

export interface HistoryEntry
{
  rentDate: string,
  returnDate: string,
  car: CurrentlyRentedCar
}
