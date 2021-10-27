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
    scopes: ['https://dotnetrulezcarrental.onmicrosoft.com/e24648ec-e7b0-497b-b82f-80e5d521073e/access_as_user']
  }
}

export const loginRequest = {
  scopes: ['openid', 'profile', 'email']
};
