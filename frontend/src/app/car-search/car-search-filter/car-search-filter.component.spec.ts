import {ComponentFixture, TestBed} from '@angular/core/testing';

import {CarSearchFilterComponent} from './car-search-filter.component';
import {MatCardModule} from "@angular/material/card";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {NgxSliderModule} from "@angular-slider/ngx-slider";

const testCarList = [{id: 0, brand: "Opel", model: "Astra", productionYear: 2020, category: "Medium",
  capacity: 5, horsePower: 34, providerCompany: "A"},
  {id: 1, brand: "Honda", model: "Civic", productionYear: 2019, category: "Small", capacity: 4,
    horsePower: 134, providerCompany: "B"},
  {id: 2, brand: "Seat", model: "Ibiza", productionYear: 2014, category: "Big", capacity: 6, horsePower: 34,
    providerCompany: "A"},
  {id: 3, brand: "Honda", model: "Escapado", productionYear: 2019, category: "XXL", capacity: 7,
    horsePower: 104, providerCompany: "C"},
  {id: 4, brand: "Opel", model: "Insignia", productionYear: 2019, category: "Big", capacity: 6,
    horsePower: 314, providerCompany: "D"},
  {id: 5, brand: "Opel", model: "Insignia", productionYear: 2019, category: "Big", capacity: 6,
    horsePower: 324, providerCompany: "A"},
]


describe('CarSearchFilterComponent brand and model filter checkboxes', () => {
  let component: CarSearchFilterComponent;
  let fixture: ComponentFixture<CarSearchFilterComponent>;
  let element: HTMLElement;
  let filterSection: HTMLElement | null;
  let brandFilterGroups: NodeListOf<HTMLElement> | undefined;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CarSearchFilterComponent],
      imports: [MatCardModule, MatCheckboxModule, NgxSliderModule],
    });
    fixture = TestBed.createComponent(CarSearchFilterComponent);
    component = fixture.componentInstance;
    component.cars = testCarList;
    component.parseCarData();
    element = fixture.nativeElement;
    fixture.detectChanges();
    filterSection = element.querySelector(".brand-and-model-filter");
    brandFilterGroups = filterSection?.querySelectorAll(".brand-filter-group");
  });

  it('should get data about cars from data provider', function () {
    expect(component.cars).toEqual(testCarList);
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

  it('should have slider values according to min and max values of car properties', () => {
    expect(component.productionYearSliderConfig.value).toEqual(2014)
    expect(component.productionYearSliderConfig.highValue).toEqual(2020)
    expect(component.capacitySliderConfig.value).toEqual(4)
    expect(component.capacitySliderConfig.highValue).toEqual(7)
    expect(component.horsePowerSliderConfig.value).toEqual(34)
    expect(component.horsePowerSliderConfig.highValue).toEqual(324)
  });
});
