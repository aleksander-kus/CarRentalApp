

context("Car details", () => {

  beforeEach(() => {
    cy.visit(Cypress.env('APP_URL'));
  });

  it("should open modal when show details button clicked", () => {
    cy.get('button[cy-id="details-button"]').first().click();

    cy.get('app-car-details').should('be.visible');
  });

  it("should load selected car data into modal", () => {
    cy.get('button[cy-id="details-button"]').first().click();

    cy.get('td[cy-id="brand-col"]').first().then(
      brand => cy.get('h1[cy-id="details-title"]').should('contain.text', brand.text().trim()));
    cy.get('td[cy-id="model-col"]').first().then(
      model => cy.get('h1[cy-id="details-title"]').should('contain.text', model.text().trim()));
    cy.get('td[cy-id="production-col"]').first().then(
      production => cy.get('span[cy-id="details-production"]').should('have.text', production.text().trim()));
    cy.get('td[cy-id="capacity-col"]').first().then(
      capacity => cy.get('span[cy-id="details-capacity"]').should('have.text', capacity.text().trim()));
    cy.get('td[cy-id="power-col"]').first().then(
      power => cy.get('span[cy-id="details-power"]').should('have.text', power.text().trim()));
    cy.get('td[cy-id="category-col"]').first().then(
      cat => cy.get('span[cy-id="details-category"]').should('have.text', cat.text().trim()));
    cy.get('td[cy-id="category-col"]').first().then(
      cat => cy.get('span[cy-id="details-description"]').should('have.text', description.text().trim()));

  });

  it("should enable to close modal with icon", () => {
    cy.get('button[cy-id="details-button"]').first().click();

    cy.get('button[cy-id="details-close"]').click();

    cy.get('app-car-details').should('not.exist');
  });
});
