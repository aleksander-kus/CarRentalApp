import { LogLevel, Configuration, BrowserCacheLocation } from '@azure/msal-browser';
import { environment } from "./environments/environment";

const isIE = window.navigator.userAgent.indexOf("MSIE ") > -1 || window.navigator.userAgent.indexOf("Trident/") > -1;

export const b2cPolicies = {
  names: {
    signUpSignIn: "B2C_1_Dotnetrulez_Car_Rental",
  },
  authorities: {
    signUpSignIn: {
      authority: "https://dotnetrulezcarrental.b2clogin.com/dotnetrulezcarrental.onmicrosoft.com/B2C_1_Dotnetrulez_Car_Rental",
    }
  },
  authorityDomain: "dotnetrulezcarrental.b2clogin.com"
};

export const msalConfig: Configuration = {
  auth: {
    clientId: environment.azureADClientId,
    authority: b2cPolicies.authorities.signUpSignIn.authority,
    knownAuthorities: [b2cPolicies.authorityDomain],
    redirectUri: '/'
  },
  cache: {
    cacheLocation: BrowserCacheLocation.SessionStorage,
    storeAuthStateInCookie: isIE,
  },
  system: {
    loggerOptions: {
      loggerCallback(logLevel: LogLevel, message: string) {
        console.log(message);
      },
      logLevel: LogLevel.Verbose,
      piiLoggingEnabled: false
    }
  }
}

export const protectedResources = {
  userDetailsApi: {
    endpoint: `${environment.apiUrl}/api/user`,
    scopes: [environment.azureADApiScope]
  },
  carCheckPriceApi: (providerId: string, carId: string) => ({
    endpoint: `${environment.apiUrl}/api/cars/${providerId}/${carId}/price`,
    scopes: [environment.azureADApiScope]
  }),
  carRentApi: (providerId: string, carId: string) => ({
    endpoint: `${environment.apiUrl}/api/cars/${providerId}/${carId}/rent`,
    scopes: [environment.azureADApiScope]
  }),
  carReturnApi: (providerId: string, carId: string) => ({
    endpoint: `${environment.apiUrl}/api/cars/${providerId}/${carId}/return`,
    scopes: [environment.azureADApiScope]
  }),
  uploadFileApi: () => ({
    endpoint: `${environment.apiUrl}/api/files/upload`,
    scopes: [environment.azureADApiScope]
  }),
  fileUriApi: (fileName: string) => ({
    endpoint: `${environment.apiUrl}/api/files/download/${fileName}`,
    scopes: [environment.azureADApiScope]
  }),
  currentlyRentedApi: (all: boolean) => ({
    endpoint: all ? `${environment.apiUrl}/api/cars/currentlyRented` : `${environment.apiUrl}/api/cars/currentlyRentedByUser`,
    scopes: [environment.azureADApiScope]
  }),
  rentalHistoryApi: (all: boolean) => ({
    endpoint: all ? `${environment.apiUrl}/api/cars/rentalHistory` : `${environment.apiUrl}/api/cars/rentalHistoryByUser`,
    scopes: [environment.azureADApiScope]
  })
}

export const loginRequest = {
  scopes: ['openid', 'profile', 'email']
};
