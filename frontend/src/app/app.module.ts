import { NgModule } from "@angular/core";
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MsalRedirectComponent } from "@azure/msal-angular";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthModule } from "./auth/auth.module";
import { CarSearchModule } from "./car-search/car-search.module";
import { CarDetailsComponent } from './car-search/car-details/car-details.component';
import {MatDialogModule} from "@angular/material/dialog";
import {MatListModule} from "@angular/material/list";
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";


@NgModule({
  declarations: [
    AppComponent
  ],
    imports: [
        AuthModule,
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        CarSearchModule,
        MatDialogModule,
        MatListModule,
        MatButtonModule,
        MatIconModule
    ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule {
}
