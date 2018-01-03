var urlPath = '/DigitalArchive/Documents/GetAllCustomerDocuments/';

$(function () {
    ko.applyBindings(customerDocumentsViewModel);
    customerDocumentsViewModel.loadCustomerDocuments();
});

var customerDocumentsViewModel = {
    CustomerDocuments: ko.observableArray([]),

    loadCustomerDocuments: function () {
        var self = this;
        //Ajax Call Get All Articles
        $.ajax({
            type: "GET",
            url: urlPath,
            contentType: "application/json; charset=utf-8",
            data: {
                customerNumber: 1// $.url().param('customerNumber')
            },
            dataType: "json",
            success: function (data) {
                console.log(data);
                self.CustomerDocuments(data); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });

    }
};

function CustomerDocuments(documents) {
    this.SerialNo = ko.observable(documents.SerialNo);
    this.Description = ko.observable(documents.Description);
    this.DocumentType = ko.observable(documents.DocumentType);
    this.DocumentTags = ko.observable(documents.DocumentTags);
    this.DateCreated = ko.observable(documents.DateCreated);
}