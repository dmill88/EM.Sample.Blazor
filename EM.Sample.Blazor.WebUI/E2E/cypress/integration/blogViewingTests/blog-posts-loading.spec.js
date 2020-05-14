context("Reading blog posts", () => {
    it('Reading blog posts', () => {
        expect(true).to.equal(true);
    });

    it('Load Home Page', () => {
        cy.visit("/");
        cy.contains('Blazor WebAssembly');
    });

    it('Load blog post', () => {
        cy.get('#blogListTable > div > table > tbody > tr:nth-child(1) > td:nth-child(4) > button').click();
        cy.contains('Posts');
        cy.wait(1000);
        cy.get('#divBlogPosts > div:nth-child(1) > a').click();
        cy.contains('Tags');
    });

});
