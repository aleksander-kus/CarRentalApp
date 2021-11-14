// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000',
  azureADClientId: '7ceb0184-5a19-470b-83ac-22b635c03602',
  azureADTenantId: 'c86be5ce-4cbf-4086-8d66-d55d50619e17',
  azureADApiScope: 'https://dotnetrulezcarrental.onmicrosoft.com/e24648ec-e7b0-497b-b82f-80e5d521073e/access_as_user',
  azureADRedirectUrl: 'http://localhost:4200/auth/complete'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
