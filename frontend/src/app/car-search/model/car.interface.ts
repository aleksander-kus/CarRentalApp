import { CarCategory } from "./carcategory.enum";

export interface Car {
    Brand: string;
    Model: string;
    ProductionDate: Date;
    Capacity: number;
    Category: CarCategory;
}
