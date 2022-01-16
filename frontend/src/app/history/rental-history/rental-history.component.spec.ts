import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Observable, of } from "rxjs";
import { fromArray } from "rxjs/internal/observable/fromArray";
import { RentalHistoryComponent } from "./rental-history.component";
import { HistoryEntry } from "../model/history-entry.interface";
import { RentalHistoryService } from "../services/rental-history.service";
import { AuthService } from "../../auth/auth.service";
import { User } from "../../auth/model/user.interface";
import { Role } from "../../auth/model/role.enum";
import { MatDialog } from "@angular/material/dialog";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { RouterTestingModule } from "@angular/router/testing";
import { CarSearchComponent } from "../../car-search/components/car-search/car-search.component";

const testHistoryEntries: HistoryEntry[] = [
  {id: "1", rentDate: "2022-01-01", returnDate: "2022-01-03", returned: true, carModel: "Opel", carBrand: "Astra",
    carId: "1", carProvider: "DNR", rentId: "1", userEmail: "example@example.com", carCondition: "good",
    photoFileId: "examplePhotoId", pdfFileId: "examplePdfId", odometerValue: "1900"},
  {id: "2", rentDate: "2022-01-01", returnDate: "2022-02-03", returned: false, carModel: "Opel", carBrand: "Insignia",
    carId: "2", carProvider: "DNR", rentId: "2", userEmail: "example@example.com", carCondition: "", photoFileId: "",
    pdfFileId: "", odometerValue: ""}
]

describe('RentalHistoryComponent table rendering', () => {
  let component: RentalHistoryComponent;
  let fixture: ComponentFixture<RentalHistoryComponent>;
  let element: HTMLElement;

  const rentalHistoryServiceStub: Partial<RentalHistoryService> = {
    loadEntries(all: boolean): Observable<HistoryEntry[]> {
      return fromArray([testHistoryEntries]);
    }
  };

  const user: User = {givenName: "Client", familyName: "Example", role: Role.Client}

  const authServiceStub: Partial<AuthService> = {
    user$: of(user)
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RentalHistoryComponent],
      imports: [MatTableModule, MatButtonModule, RouterTestingModule.withRoutes([{path: '', component: CarSearchComponent}])],
      providers: [
        {provide: RentalHistoryService, useValue: rentalHistoryServiceStub},
        {provide: AuthService, useValue: authServiceStub},
        {provide: MatDialog, useValue: {}},
      ],
    });
    fixture = TestBed.createComponent(RentalHistoryComponent);
    component = fixture.componentInstance;
    element = fixture.nativeElement;
    fixture.detectChanges();
  });

  it('should have a return to main page button', function () {
    const button = element.querySelector('.return-button');
    expect(button).toBeTruthy()
    expect(button!.textContent).toBe("Return")
  })

  it('should have a table with a header row and two data rows', function () {
    const table = element.querySelector('.mat-table')
    expect(table).toBeTruthy()
    const headerRow = table!.querySelector('.mat-header-row')
    const dataRows = table!.querySelectorAll('.mat-row')
    expect(headerRow).toBeTruthy()
    expect(dataRows).toBeTruthy()
    expect(dataRows.length).toBe(2)
  })

  it('should display see details button on the row with the returned car', function () {
    const table = element.querySelector('.mat-table')
    expect(table).toBeTruthy()
    const dataRows = table!.querySelectorAll('.mat-row')
    expect(dataRows).toBeTruthy()
    expect(dataRows[0]).toBeTruthy()
    const button = dataRows[0].querySelector('.mat-raised-button')
    expect(button).toBeTruthy()
    expect(button!.textContent).toBe('See details')
  })

  it('should not display see details button on the row with the returned car', function () {
    const table = element.querySelector('.mat-table')
    expect(table).toBeTruthy()
    const dataRows = table!.querySelectorAll('.mat-row')
    expect(dataRows).toBeTruthy()
    expect(dataRows[0]).toBeTruthy()
    const button = dataRows[1].querySelector('.mat-raised-button')
    expect(button).toBeNull()
  })
  // TODO: Fix tests
  // it('should properly detect unique car categories', function () {
  //   expect(component.presenter.mapCarsToCategories(fromArray([testCarList])).).toBeTruthy();
  //   expect(component.uniqueCarCategories.sort()).toEqual(['Small', 'Medium', 'Big', 'XXL'].sort());
  // });
  //
  // it('should properly assigning each unique model to correct brand', function () {
  //   expect(component.modelsByBrand).toBeTruthy();
  //   ['Opel', 'Honda', 'Seat'].forEach(brand => {
  //     expect(component.modelsByBrand.get(brand)).toBeTruthy();
  //   })
  //   expect(component.modelsByBrand.get('Opel')!.sort()).toEqual(['Astra', 'Insignia'].sort());
  //   expect(component.modelsByBrand.get('Honda')!.sort()).toEqual(['Civic', 'Escapado'].sort());
  //   expect(component.modelsByBrand.get('Seat')!).toEqual(['Ibiza']);
  // });
  //
  // it('should create brand checkboxes matching all unique brands', () => {
  //   expect(brandFilterGroups).toBeTruthy();
  //   const brandCheckboxes = Array.from(brandFilterGroups!).map(bfg => bfg.querySelector("mat-checkbox"));
  //   expect(brandCheckboxes).toBeTruthy();
  //   const brandNames = Array.from(brandCheckboxes).map(cb => cb?.textContent);
  //   for (let i = 0; i < brandNames.length; i++) {
  //     expect(brandNames[i]).toBeTruthy();
  //     brandNames[i] = brandNames[i]!.trim();
  //   }
  //   expect(brandNames.sort()).toEqual(Array.from(component.modelsByBrand.keys()).sort());
  // });
  //
  // it('should create model checkboxes matching all unique models of specific brand', () => {
  //   expect(brandFilterGroups).toBeTruthy();
  //   brandFilterGroups!.forEach(bfg => {
  //     const brandCheckbox = bfg.querySelector("mat-checkbox");
  //     expect(brandCheckbox).toBeTruthy();
  //     let brandName = brandCheckbox!.textContent;
  //     expect(brandName).toBeTruthy();
  //     brandName = brandName!.trim();
  //     expect(Array.from(component.modelsByBrand.keys())).toContain(brandName!);
  //
  //     const modelList = bfg.querySelector(".model-list");
  //     expect(modelList).toBeTruthy();
  //     const modelCheckboxes = modelList!.querySelectorAll("mat-checkbox");
  //     expect(modelCheckboxes).toBeTruthy();
  //     const modelNames = Array.from(modelCheckboxes!).map(mcb => mcb.textContent);
  //     for (let i = 0; i < modelNames.length; i++) {
  //       expect(modelNames[i]).toBeTruthy();
  //       modelNames[i] = modelNames[i]!.trim();
  //     }
  //     expect(modelNames.sort()).toEqual(component.modelsByBrand.get(brandName!)!.sort());
  //   });
  // });
  //
  // it('should have slider values according to min and max values of car properties', () => {
  //   expect(component.productionYearSliderConfig.value).toEqual(2014)
  //   expect(component.productionYearSliderConfig.highValue).toEqual(2020)
  //   expect(component.capacitySliderConfig.value).toEqual(4)
  //   expect(component.capacitySliderConfig.highValue).toEqual(7)
  //   expect(component.horsePowerSliderConfig.value).toEqual(34)
  //   expect(component.horsePowerSliderConfig.highValue).toEqual(324)
  // });
});
