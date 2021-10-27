import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from "./auth.service";
import { AuthComponent } from './auth-component/auth.component';
import { MatButtonModule } from "@angular/material/button";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { AuthGuard } from "./auth.guard";
import { ClientGuard } from "./client.guard";
import { VisibleForDirective } from './visible-for.directive';
import { EmployeeGuard } from "./employee.guard";
import { MenubarComponent } from "./menubar-component/menubar.component";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatMenuModule } from "@angular/material/menu";
import { MatIconModule } from "@angular/material/icon";
import {
  MSAL_GUARD_CONFIG,
  MSAL_INSTANCE,
  MSAL_INTERCEPTOR_CONFIG,
  MsalBroadcastService, MsalGuard, MsalGuardConfiguration, MsalInterceptor, MsalInterceptorConfiguration, MsalModule,
  MsalService
} from "@azure/msal-angular";
import { InteractionType, IPublicClientApplication, PublicClientApplication } from "@azure/msal-browser";
import { loginRequest, msalConfig, protectedResources } from "../../auth.config";

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication(msalConfig);
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();

  protectedResourceMap.set(protectedResources.userDetailsApi.endpoint, protectedResources.userDetailsApi.scopes);

  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap
  };
}

export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: loginRequest
  };
}



@NgModule({
  declarations: [
    AuthComponent,
    MenubarComponent,
    VisibleForDirective
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MSALGuardConfigFactory
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory
    },
    MsalService,
    MsalBroadcastService,
    MsalGuard,
    AuthService,
    AuthGuard,
    ClientGuard,
    EmployeeGuard
  ],
  exports: [
    AuthComponent,
    MenubarComponent
  ],
  imports: [
    CommonModule,
    MsalModule,
    HttpClientModule,
    MatButtonModule,
    MatToolbarModule,
    MatMenuModule,
    MatIconModule
  ]
})
export class AuthModule { }
