/// <reference types="cypress" />

context('Authentication', () => {
  beforeEach(() => {
    cy.visit(Cypress.env('APP_URL'));
  });

  it('should redirect when login button is clicked', () => {
    cy.get('button[cy-id="login-button"]').click();

    cy.url().should('include', '.b2clogin.com/');
  });

  it('should ask for age, driving license years and postal code on registration', () => {
    cy.get('button[cy-id="login-button"]').click();

    cy.get('a#createAccount').click();

    cy.get('input#extension_Age').should('be.visible');
    cy.get('input#extension_YearsOfHavingDrivingLicense').should('be.visible');
    cy.get('input#postalCode').should('be.visible');
  });

  it('should validate age, driving license years and postal code on registration', () => {
    cy.get('button[cy-id="login-button"]').click();

    cy.get('a#createAccount').click();

    cy.fixture('new-user-data').then(c => {
      cy.get('input#email').type(c.email);
      cy.get('input#extension_Age').type(c.age);
      cy.get('input#extension_YearsOfHavingDrivingLicense').type(c.yodl);
      cy.get('input#postalCode').type(c.postalCode);
      cy.get('input#newPassword').type(c.password);
      cy.get('input#reenterPassword').type(c.password);
      cy.get('input#streetAddress').type(c.streetAddress);
      cy.get('input#givenName').type(c.givenName);
      cy.get('input#surname').type(c.surname);
      cy.get('select#country').select(c.country);
      cy.get('input#city').type(c.city);
      cy.get('input#state').type(c.state);

      cy.get('button#continue').click();

      cy.get('div#claimVerificationServerError').should('contain.text', "Only adults can register," +
        "Postal code in incorrect format,Years Of Having Driving License has to be positive");
    });
  });

  it('should allow to login with Azure B2C', () => {
    cy.get('button[cy-id="login-button"]').click();

    cy.fixture('user-credentials').then(c => {
      cy.get('input#email').type(c.email);
      cy.get('input#password').type(c.password);

      cy.get('button#next').click();

      cy.url().should('include', Cypress.env('APP_URL'));
    });
  });

  it('should display user information', () => {
    cy.get('button[cy-id="login-button"]').click();

    cy.get('button[cy-id="user-button"]').click();

    cy.get('h3[cy-id="current-user-name"]').should('contain.text', 'Kot Ala');
  });

  it('should allow to log out', () => {
    cy.get('button[cy-id="user-button"]').click();

    cy.get('button[cy-id="logout-button"]').click();

    cy.get('button[cy-id="login-button"]').should('be.visible');
  });
});
