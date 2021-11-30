import { Component, EventEmitter, Input, Output } from "@angular/core";
import {Car} from "../model/car.interface";
import { Options } from "@angular-slider/ngx-slider";
import { CarFilter } from "../model/car-filter.interface";
import { Observable } from "rxjs";
import { CarSearchFilterPresenter } from "./car-search-filter.presenter";

@Component({
  selector: 'app-car-search-filter',
  templateUrl: './car-search-filter.component.html',
  styleUrls: ['./car-search-filter.component.css'],
  providers: [CarSearchFilterPresenter]
})
export class CarSearchFilterComponent {
  @Input()
  set cars$(c$: Observable<Car[]>) {
    this.categories$ = this.presenter.mapCarsToCategories(c$);
    this.brands$ = this.presenter.mapCarsToBrands(c$);
  }
  @Output()
  change: EventEmitter<CarFilter> = new EventEmitter<CarFilter>();

  productionYearSliderConfig: Options = {floor: 1950, ceil: (new Date()).getFullYear()};
  capacitySliderConfig: Options = {floor: 2, ceil: 11};
  horsePowerSliderConfig: Options = {floor: 0, ceil: 500};

  filters = this.presenter.getDefaultFilerValues();
  categories$: Observable<string[]>;
  brands$: Observable<Map<string, string[]>>;

  constructor(public presenter: CarSearchFilterPresenter) { }

  isBrandSelected(brand: string): boolean {
    return this.filters.brands.indexOf(brand) !== -1;
  }

  setBrandSelected(brand: string, value: boolean): void {
    this.filters.brands = [...this.filters.brands, brand].filter(v => v !== brand || value);
    this.filterChanged();
  }

  isModelSelected(model: string): boolean {
    return this.filters.brands.indexOf(model) !== -1;
  }

  setModelSelected(model: string, brand: string, value: boolean): void {
    this.filters.models = [...this.filters.models, model].filter(v => v !== model || value);

    if (!value) {
      this.setBrandSelected(brand, false);
    }

    this.filterChanged();
  }

  isCategorySelected(category: string): boolean {
    return this.filters.brands.indexOf(category) !== -1;
  }

  setCategorySelected(category: string, value: boolean): void {
    this.filters.categories = [...this.filters.categories, category].filter(v => v !== category || value);
    this.filterChanged();
  }

  filterChanged(): void {
    this.change.emit(this.filters);
  }

  clearFilters(): void {
    this.filters = this.presenter.getDefaultFilerValues();
    this.filterChanged();
  }
}

