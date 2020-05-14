context("CRUD blog posts", () => {
    it('CRUD blog posts', () => {
        expect(true).to.equal(true);
    });

    it('Load Home Page', () => {
        cy.visit("/");
        cy.contains('Blazor WebAssembly');
    });

    it('Load blog add page', () => {
        cy.get('body > app > div.sidebar > div.collapse > ul > li:nth-child(2) > a').click();
        cy.contains('Blog Administration');
        cy.wait(1000);
        cy.get('#divBlogPostAdminAddLink > button').click();
        cy.wait(1000);
        cy.contains('Add Blog');
    });

    it('Add Test Blog', () => {
        cy.get('#ddlBlogAuthor').click().find('li').click();
        cy.get('#ddlBlogStatus').click().find('li:nth-child(1)').click();
        cy.get('input[name=txtBlogDisplayOrder]').focus().type("{backspace}99");
        cy.get('#txtBlogDisplayName').click().type("My Test Blog");
        cy.get('#txtBlogLookupName').click().type("MyTestBlog02");
        cy.get('#txtBlogDescription').click().type("This is a sample description");
        cy.get('#tagsBlogTags').click().find('li:nth-child(1)').click();
        cy.get('#tagsBlogTags').click().find('li:nth-child(1)').click();
        cy.get('#tagsBlogTags').click().find('li:nth-child(1)').click();
        cy.get('#btnFormSubmit').click();
        cy.contains('Successfully saved blog.');
    });

});
