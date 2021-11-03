import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CarSearchComponent } from "./car-search/car-search/car-search.component";
import { RentalHistoryComponent } from "./history/rental-history/rental-history.component";
import { EmployeeGuard } from "./auth/guards/employee.guard";
import { UnauthorizedPageComponent } from "./auth/components/unauthorized-page/unauthorized-page.component";

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
    path: 'unauthorized',
    component: UnauthorizedPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
