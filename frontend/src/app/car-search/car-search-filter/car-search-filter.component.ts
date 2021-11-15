import {Component} from '@angular/core';
import {Car} from "../model/car.interface";
import {CarFilter} from "../model/carfilter";

@Component({
  selector: 'app-car-search-filter',
  templateUrl: './car-search-filter.component.html',
  styleUrls: ['./car-search-filter.component.css']
})
export class CarSearchFilterComponent {
  productionYearSliderConfig = {value: 2000, highValue: 2021, options: {floor: 2000, ceil: 2021}};
  capacitySliderConfig = {value: 2, highValue: 7, options: {floor: 2, ceil: 7}};
  horsePowerSliderConfig = {value: 0, highValue: 200, options: {floor: 0, ceil: 200}}
  modelsByBrand: Map<string, string[]> = new Map<string, string[]>();
  brandChecked: Map<string, boolean> = new Map<string, boolean>();
  modelChecked: Map<string, boolean> = new Map<string, boolean>();
  categoryChecked: Map<string, boolean> = new Map<string, boolean>();
  uniqueCarCategories: string[] = [];
  uniqueBrands: string[] = [];
  cars: Car[] = [];
  private filter: CarFilter = new CarFilter()

  private getUniqueCarBrands() {
    this.cars.forEach(car => {
      if (this.uniqueBrands.indexOf(car.brand) == -1)
        this.uniqueBrands.push(car.brand);
    });
  }

  private getUniqueCarModelsOfBrand(brand: string): string[] {
    const models: string[] = [];
    this.cars.forEach(car => {
      if (car.brand == brand && models.indexOf(car.model) == -1)
        models.push(car.model);
    });
    return models;
  }

  private getUniqueCarCategories() {
    this.cars.forEach(car => {
      if (this.uniqueCarCategories.indexOf(car.category) == -1)
        this.uniqueCarCategories.push(car.category);
    })
  }

  private setSliderConfig() {
    const years = this.cars.map(car => car.productionYear)
    const minProductionYear = Math.min(...years)
    const maxProductionYear = Math.max(...years)
    this.productionYearSliderConfig = {
      value: minProductionYear,
      highValue: maxProductionYear,
      options: {floor: minProductionYear, ceil: maxProductionYear}
    }
    const capacities = this.cars.map(car => car.capacity)
    const minCapacity = Math.min(...capacities)
    const maxCapacity = Math.max(...capacities)
    this.capacitySliderConfig = {
      value: minCapacity,
      highValue: maxCapacity,
      options: {floor: minCapacity, ceil: maxCapacity}
    }
    const horsePowers = this.cars.map(car => car.horsePower)
    const minHorsePower = Math.min(...horsePowers)
    const maxHorsePower = Math.max(...horsePowers)
    this.horsePowerSliderConfig = {
      value: minHorsePower,
      highValue: maxHorsePower,
      options: {floor: minHorsePower, ceil: maxHorsePower}
    }
  }

  public getFilter(): CarFilter {
    this.filter = new CarFilter()
    this.filter.CapacityMin = this.capacitySliderConfig.value
    this.filter.CapacityMax = this.capacitySliderConfig.highValue
    this.filter.ProductionYearMin = this.productionYearSliderConfig.value
    this.filter.ProductionYearMax = this.productionYearSliderConfig.highValue
    this.filter.HorsePowerMin = this.horsePowerSliderConfig.value
    this.filter.HorsePowerMax = this.horsePowerSliderConfig.highValue
    // if any category is checked, mark all others as excluded
    if(Array.from(this.categoryChecked.values()).some(value => value))
    {
      this.filter.ExcludedCategories = this.uniqueCarCategories.filter(category => this.categoryChecked.get(category) == false)
    }
    // if any model is checked, mark all others as excluded
    if(Array.from(this.modelChecked.values()).some(value => value))
    {
      this.filter.ExcludedModels = Array.from(this.modelChecked.keys()).filter(model => this.modelChecked.get(model) == false)
    }
    // if any brand is checked, mark all others as excluded
    if(Array.from(this.brandChecked.values()).some(value => value))
    {
      this.filter.ExcludedBrands = this.uniqueBrands.filter(brand => this.modelsByBrand.get(brand)?.every(model => this.modelChecked.get(model) == false))
    }
    return this.filter
  }

  public parseCarData() {
    if(this.cars == null || this.cars.length == 0)
      return
    this.setSliderConfig()
    this.getUniqueCarBrands();
    this.uniqueBrands.forEach(brand => {
      const models = this.getUniqueCarModelsOfBrand(brand)
      models.forEach(model => this.modelChecked.set(model, false))
      this.brandChecked.set(brand, false);
      this.modelsByBrand.set(brand, models);
    })
    this.getUniqueCarCategories();
    this.uniqueCarCategories.forEach(category => this.categoryChecked.set(category, false))
  }

  brandCheckChanged(value: boolean, brand: string) {
    this.brandChecked.set(brand, value)
    this.modelsByBrand.get(brand)?.forEach(model => this.modelChecked.set(model, value))
  }

  checkIfModelsChecked(brand: string): boolean {
    return !this.brandChecked.get(brand) && this.modelsByBrand.get(brand)!.filter(model => this.modelChecked.get(model) == true).length > 0
  }

  updateBrandChecked() {
    this.uniqueBrands.forEach(brand => this.brandChecked.set(brand, this.modelsByBrand.get(brand)!.every(model => this.modelChecked.get(model) == true)))
  }

  clearFilters(){
    this.productionYearSliderConfig.value = this.productionYearSliderConfig.options.floor
    this.productionYearSliderConfig.highValue = this.productionYearSliderConfig.options.ceil
    this.capacitySliderConfig.value = this.capacitySliderConfig.options.floor
    this.capacitySliderConfig.highValue = this.capacitySliderConfig.options.ceil
    this.horsePowerSliderConfig.value = this.horsePowerSliderConfig.options.floor
    this.horsePowerSliderConfig.highValue = this.horsePowerSliderConfig.options.ceil
    this.uniqueBrands.forEach(brand => {
      this.brandChecked.set(brand, false)
      this.modelsByBrand.get(brand)!.forEach(model => this.modelChecked.set(model, false))
    })
    this.uniqueCarCategories.forEach(category => this.categoryChecked.set(category, false))
  }
}

