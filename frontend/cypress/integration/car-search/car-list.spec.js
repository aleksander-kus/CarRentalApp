
context("Car list", () => {
  beforeEach(() => {
    cy.visit(Cypress.env('APP_URL'));
  });

  it("should display non empty list of cars", () => {
    cy.get('tr[cy-id="car-list-row"]').should('have.length.gt', 0);
  });

  it("should allow to filter by brand", () => {
    cy.get('[cy-id="brand-filter-checkbox"]').first().click();
    cy.get('button[cy-id="search-button"]').click();

    cy.get('[cy-id="brand-filter-checkbox"]').first().then(brand => {
      cy.get('td[cy-id="brand-col"]').each(
        col => cy.wrap(col).should('contain.text', brand.first().attr('data-value')));
    });
  });

  it("should allow to filter by model", () => {
    cy.get('[cy-id="model-filter-checkbox"]').last().click();
    cy.get('button[cy-id="search-button"]').click();

    cy.get('[cy-id="model-filter-checkbox"]').last().then(model => {
      cy.get('td[cy-id="model-col"]').each(
        col => cy.wrap(col).should('contain.text', model.first().attr('data-value')));
    });
  });

  it("should allow to filter by category", () => {
    cy.get('[cy-id="category-filter-checkbox"]').first().click();
    cy.get('button[cy-id="search-button"]').click();

    cy.get('[cy-id="category-filter-checkbox"]').first().then(brand => {
      cy.get('td[cy-id="category-col"]').each(
        col => cy.wrap(col).should('contain.text', brand.first().attr('data-value')));
    });
  });

  it("should allow to filter with sliders", () => {
    cy.get('[cy-id="capacity-filter-slider"] .ngx-slider-pointer-min').focus().type("{rightarrow}{rightarrow}{rightarrow}");
    cy.get('button[cy-id="search-button"]').click();

    cy.get('td[cy-id="capacity-col"]').each(col => {
      cy.wrap(parseInt(col.text())).should('be.gte', 5);
    });
  });

  it("should allow to clear filters", () => {
    cy.get('[cy-id="category-filter-checkbox"]').first().click();
    cy.get('[cy-id="model-filter-checkbox"]').last().click();
    cy.get('[cy-id="brand-filter-checkbox"]').first().click();

    cy.get('button[cy-id="clear-filters-button"]').click();

    cy.get('app-car-search-filter input:checked').should('not.exist');
  });
});
