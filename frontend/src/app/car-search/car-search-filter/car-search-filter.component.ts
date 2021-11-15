import {OnInit, Component} from '@angular/core';
import {CarDataService} from "../cardata.service";
import {Car} from "../model/car.interface";

@Component({
  selector: 'app-car-search-filter',
  templateUrl: './car-search-filter.component.html',
  styleUrls: ['./car-search-filter.component.css']
})
export class CarSearchFilterComponent implements OnInit {
  productionYearSliderConfig = {value : 2000, highValue : 2021, options : {floor : 2000, ceil : 2021}};
  capacitySliderConfig = {value : 2, highValue : 7, options : {floor : 2, ceil : 7}};
  modelsByBrand: Map<string, string[]> = new Map<string, string[]>();
  uniqueCarCategories: string[] = [];
  uniqueBrands: string[] = [];
  cars: Car[] = [];

  private getUniqueCarBrands() {
    this.cars.forEach(car => {
      if(this.uniqueBrands.indexOf(car.Brand) == -1)
        this.uniqueBrands.push(car.Brand);
    });
  }

  private getUniqueCarModelsOfBrand(brand: string): string[] {
    const models: string[] = [];
    this.cars.forEach(car => {
      if(car.Brand == brand && models.indexOf(car.Model) == -1)
        models.push(car.Model);
    });
    return models;
  }

  private getUniqueCarCategories() {
    this.cars.forEach(car => {
      if(this.uniqueCarCategories.indexOf(car.Category) == -1)
        this.uniqueCarCategories.push(car.Category);
    })
  }

  getDataFromService(){
    this.dservice.getData().subscribe(cars =>
    {
      this.cars = cars;
      this.getUniqueCarBrands();
      this.uniqueBrands.forEach(brand => {
        this.modelsByBrand.set(brand, this.getUniqueCarModelsOfBrand(brand));
      })
      this.getUniqueCarCategories();
    });
  }
  constructor(private dservice: CarDataService) { }

  ngOnInit() {
    this.getDataFromService()
  }
}
