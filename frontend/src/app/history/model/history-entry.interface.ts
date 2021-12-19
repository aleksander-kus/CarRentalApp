import { CarProvider } from "../../car-search/model/car-provider.interface";

export interface HistoryEntry
{
  rentDate: Date,
  returnDate: Date | null,
  carId: number,
  brand: string,
  model: string,
  provider: CarProvider
}
