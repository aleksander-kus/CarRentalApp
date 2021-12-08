import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RentalHistoryComponent } from "./history/rental-history/rental-history.component";
import { EmployeeGuard } from "./auth/guards/employee.guard";
import { UnauthorizedPageComponent } from "./auth/components/unauthorized-page/unauthorized-page.component";
import { CurrentlyRentedComponent } from "./history/currently-rented/currently-rented.component";
import { CarSearchComponent } from "./car-search/components/car-search/car-search.component";
import { ClientGuard } from "./auth/guards/client.guard";

const routes: Routes = [
  {
    path: '',
    component: CarSearchComponent
  },
  {
    path: 'history',
    component: RentalHistoryComponent,
    canActivate: [EmployeeGuard]
  },
  {
    path: 'currentlyRented',
    component: CurrentlyRentedComponent,
    canActivate: [ClientGuard]
  },
  {
    path: 'unauthorized',
    component: UnauthorizedPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
