var urlPath = '/DigitalArchive/Documents/FetchMyDocuments/';

$(function () {
    ko.applyBindings(myDocumentsViewModel);
    myDocumentsViewModel.loadMyDocuments();
});

var myDocumentsViewModel = {
    MyDocuments: ko.observableArray([]),

    loadMyDocuments: function () {
        var self = this;
        //Ajax Call Get All Articles
        $.ajax({
            type: "GET",
            url: urlPath,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);
                self.MyDocuments(data); //Put the response in ObservableArray
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });

    }
};

function MyDocuments(documents) {
    this.SerialNo = ko.observable(documents.SerialNo);
    this.Description = ko.observable(documents.Description);
    this.DocumentType = ko.observable(documents.DocumentType);
    this.DocumentTags = ko.observable(documents.DocumentTags);
    this.DateCreated = ko.observable(documents.DateCreated);
}