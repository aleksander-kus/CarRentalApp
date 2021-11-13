import {ComponentFixture, TestBed} from '@angular/core/testing';

import {CarSearchFilterComponent} from './car-search-filter.component';
import {CarDataService} from "../cardata.service";
import {CarCategory} from "../model/carcategory.enum";
import {Car} from "../model/car.interface";
import {MatCardModule} from "@angular/material/card";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {NgxSliderModule} from "@angular-slider/ngx-slider";

const testCarList = [{Brand: "Opel", Model: "Astra", ProductionDate: new Date(2020, 1), Category: CarCategory.Medium, Capacity: 5},
  {Brand: "Honda", Model: "Civic", ProductionDate: new Date(2019, 2), Category: CarCategory.Small, Capacity: 4},
  {Brand: "Seat", Model: "Ibiza", ProductionDate: new Date(2014, 1), Category: CarCategory.Big, Capacity: 6},
  {Brand: "Honda", Model: "Escapado", ProductionDate: new Date(2019, 2), Category: CarCategory.XXL, Capacity: 7},
  {Brand: "Opel", Model: "Insignia", ProductionDate: new Date(2019, 2), Category: CarCategory.Big, Capacity: 6},
  {Brand: "Opel", Model: "Insignia", ProductionDate: new Date(2019, 2), Category: CarCategory.Big, Capacity: 6},
]


describe('CarSearchFilterComponent brand and model filter checkboxes', () => {
  let component: CarSearchFilterComponent;
  let fixture: ComponentFixture<CarSearchFilterComponent>;
  let carDataService: CarDataService;
  let element: HTMLElement;
  let filterSection: HTMLElement | null;
  let brandFilterGroups: NodeListOf<HTMLElement> | undefined;

  let carDataServiceStub: Partial<CarDataService>;

  beforeEach(() => {
    // create a stub data provider
    carDataServiceStub = {
      getData(): Car[] {
        return testCarList
      }
    };
    TestBed.configureTestingModule({
      declarations: [CarSearchFilterComponent],
      imports: [MatCardModule, MatCheckboxModule, NgxSliderModule],
      providers: [{provide: CarDataService, useValue: carDataServiceStub}],
    });
    fixture = TestBed.createComponent(CarSearchFilterComponent);
    component = fixture.componentInstance;
    carDataService = TestBed.inject(CarDataService);
    element = fixture.nativeElement;
    fixture.detectChanges();
    filterSection = element.querySelector(".brand-and-model-filter");
    brandFilterGroups = filterSection?.querySelectorAll(".brand-filter-group");
  });

  it('should get data about cars from data provider', function () {
    expect(component.cars).toEqual(carDataService.getData());
  });

  it('should properly detect unique car categories', function () {
    expect(component.uniqueCarCategories).toBeTruthy();
    expect(component.uniqueCarCategories.sort()).toEqual(['Small', 'Medium', 'Big', 'XXL'].sort());
  });

  it('should properly assigning each unique model to correct brand', function () {
    expect(component.modelsByBrand).toBeTruthy();
    ['Opel', 'Honda', 'Seat'].forEach(brand => {
      expect(component.modelsByBrand.get(brand)).toBeTruthy();
    })
    expect(component.modelsByBrand.get('Opel')!.sort()).toEqual(['Astra', 'Insignia'].sort());
    expect(component.modelsByBrand.get('Honda')!.sort()).toEqual(['Civic', 'Escapado'].sort());
    expect(component.modelsByBrand.get('Seat')!).toEqual(['Ibiza']);
  });

  it('should create brand checkboxes matching all unique brands', () => {
    expect(brandFilterGroups).toBeTruthy();
    const brandCheckboxes = Array.from(brandFilterGroups!).map(bfg => bfg.querySelector("mat-checkbox"));
    expect(brandCheckboxes).toBeTruthy();
    const brandNames = Array.from(brandCheckboxes).map(cb => cb?.textContent);
    for (let i = 0; i < brandNames.length; i++) {
      expect(brandNames[i]).toBeTruthy();
      brandNames[i] = brandNames[i]!.trim();
    }
    expect(brandNames.sort()).toEqual(Array.from(component.modelsByBrand.keys()).sort());
  });

  it('should create model checkboxes matching all unique models of specific brand', () => {
    expect(brandFilterGroups).toBeTruthy();
    brandFilterGroups!.forEach(bfg => {
      const brandCheckbox = bfg.querySelector("mat-checkbox");
      expect(brandCheckbox).toBeTruthy();
      let brandName = brandCheckbox!.textContent;
      expect(brandName).toBeTruthy();
      brandName = brandName!.trim();
      expect(Array.from(component.modelsByBrand.keys())).toContain(brandName!);

      const modelList = bfg.querySelector(".model-list");
      expect(modelList).toBeTruthy();
      const modelCheckboxes = modelList!.querySelectorAll("mat-checkbox");
      expect(modelCheckboxes).toBeTruthy();
      const modelNames = Array.from(modelCheckboxes!).map(mcb => mcb.textContent);
      for (let i = 0; i < modelNames.length; i++) {
        expect(modelNames[i]).toBeTruthy();
        modelNames[i] = modelNames[i]!.trim();
      }
      expect(modelNames.sort()).toEqual(component.modelsByBrand.get(brandName!)!.sort());
    });
  });
});
