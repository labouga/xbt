// FileName: CreateDocumentIndex.js
// Location: /Scripts/ViewModels/Indexing
//
// Performs logic to:
// 1. Check if there are any Document Groups
// 2. Toggle between activating certain elements based on the one selected
// 3. Do form validation
// 4. Save the meta data using ajax
// 5. Move between documents << Next, Previous

// This event handler if for the instance when a customer is clicked from search results
// When a customer is clicked, the customer field should be updated with the details of the clicked customer
function populateCustomer(customer) {
    $("#customers").val(customer);

    // them hide the search results
    $(".search-results").slideUp();
}

//
// Before anything is done, ensure that all the components of the page are loaded - css, html and javascript
//

$(function () {

    // Retrieve the document groups

    $.ajax({
        type: "GET",
        url: "/DigitalArchive/DocumentGroups/FetchAll",
        dataType: "json",
        success: function (response) {

            // If there are document groups returned
            if (response != null) {

                // loop through each object in the response
                $.each(response, function (index, object) {
                    // append the key=>value pair to the document group dropdown
                    $(".document-group").append("<option value=" + object["Key"] + ">" + object["Value"] + "</option>");
                });

            }
            else {
                // if no document groups were returned, display warning message
                $(".document-group-wrapper .warning").fadeIn();
            }
        },
        error: function (error) {
            // Display the warning message
            $(".document-group-wrapper .warning").fadeIn();
        }
    });

    // When a document group is selected, show the document type
    $(".document-group").change(function () {

        // Hide the errors and warnings that may have showed up on a previous submit
        $(".document-group-wrapper .error, .document-group-wrapper .warning").hide();

        // Get the selected value of the document group
        var documentGroupId = $(this).val();

        // if this value is not null, go ahead and show the document type
        // else, do nothing
        if (documentGroupId != null || documentGroupId.length != 0) {
            // $(".document-type-wrapper .warning").fadeIn();

            // Populate the document type dropdown with the documents types under the selected document group
            // First make an ajax call to fetch the document types

            $.ajax({
                type: "GET",
                url: "/DigitalArchive/DocumentTypes/FilterByDocumentGroup/",
                data: {
                    documentGroupId: documentGroupId,
                },
                dataType: "json",
                success: function (response) {

                    $(".document-type option").remove();

                    // loop through the object and append each value to the document type dropdown
                    $.each(response, function (key, object) {
                        $(".document-type").append("<option value=" + object['Key'] + ">" + object['Value'] + "</option>");
                    });
                },
                error: function () {
                    // if there is an error, just show the warning message
                    $(".document-type-wrapper .warning").fadeIn();
                }
            });
        }

    });


    // When a document type is selected, show the document tag element
    $(".document-type").change(function () {

        // Hide the errors and warnings that may have showed up on a previous submit
        $(".document-type-wrapper .error, .document-type-wrapper .warning").hide();

        // Get the selected value of the document type
        var documentTypeId = $(this).val();

        // if this value is not null, go ahead and show the document tag element
        // else, do nothing
        if (documentTypeId != null || documentTypeId.length != 0) {
            // $(".document-tag-wrapper").fadeIn();

            // Populate the document tag dropdown with the documents tags under the selected document type
            // First make an ajax call to fetch the document tags

            $.ajax({
                type: "GET",
                url: "/DigitalArchive/DocumentTags/FilterByDocumentType/",
                data: {
                    documentTypeId: documentTypeId,
                },
                dataType: "json",
                success: function (tags) {
                    // loop through the object and append each value to the document tags dropdown
                    $.each(tags, function (key, object) {
                        $(".document-tag").append("<option value=" + object['Key'] + ">" + object['Value'] + "</option>");
                    });
                },
                error: function () {
                    // if there is an error, just show the warning message
                    $(".document-tag-wrapper .warning span#error-message").html("An error occured while retrieving the tags");
                    $(".document-tag-wrapper .warning").fadeIn();
                }
            });
        }
    });

    /*
     * * * * *
     * This section handles the functionality of the customer search 
     * * * * *
     */

    // Since the results will be in form of a list, make the odd elements in the list have a background color
    $(".search-results ul li:odd").css({
        'backgroundColor': '#F1F1F1'
    });

    // Listen to an onKeyUp event on the search customer textbox
    $("#customers").keyup(function () {
        // get the value of the search textbox
        var keyword = $(this).val();

        // First show the search results' box
        $(".search-results").slideDown('fast');

        // If the search textbox is empty, then hide the search result box
        // Else make ajax call to fetch the customers based on the search term
        if (keyword == null || keyword.length == 0) {
            $(".search-results").slideUp('fast');
        } else {
            // Retrieve the customers
            $.ajax({
                type: "GET",
                url: "/DigitalArchive/Customers/Search",
                dataType: "json",
                data: {
                    keyword: keyword
                },
                success: function (customers) {
                    // if there are any matches, please populate the search box list
                    if (customers != null) {
                        $.each(customers, function (key, object) {
                            $(".search-results ul li:odd").css({
                                'backgroundColor': '#F1F1F1'
                            });

                            $(".search-results ul").append('<li onclick="populateCustomer(this.id)" id="' + object['Key'] + ', ' + object['Value'] + '" >' + object['Key'] + ', ' + object['Value'] + '</li>');
                        });
                    } else {
                        $(".search-results").html("No matching results were found.").addClass('alert alert-danger');
                    }
                },
                error: function (error) {
                    $(".search-results").html("This should be a system failure.").addClass('alert alert-warning');
                }
            });
        }
    });



    // When the save button is clicked,
    $("#submit-button").click(function () {
        // get the values of all the form elements
        var documentGroup = $(".document-group option:selected").val();
        var documentType = $("#docType option:selected").val();
        var documentTags = [];

        // var tags = documentTags.join("- ");
        $(".document-tag ul.select2-choices li.select2-search-choice").each(function() {
            documentTags.push($(".document-tag option:contains(" + $("div", this).text() + ")").val());
        });

        console.log(documentTags.join(","));
        var tags = documentTags.join(",");

        var customer = $("#customers").val();
        var accountNumber = $(".account-number").val();
        var documentTitle = $("#document-title").val();

        // check if each of the required elements does not have a value (is null)
        // Document group
        if (documentGroup == null || documentGroup.length == 0) {
            // show the error message
            $(".document-group-wrapper .error").fadeIn();
        }

        // Document type
        if (documentType == null || documentType.length == 0) {
            // show the error message
            $(".document-type-wrapper .error").fadeIn();
        }

        // Customer
        if (customer == null || customer.length == 0) {
            // show the error message
            $(".customer-wrapper .error").fadeIn();
        }

        // Title
        if (documentTitle == null || documentTitle.length == 0) {
            // show the error message
            $(".title-wrapper .error").fadeIn();
        }

            // If all the required elements pass the validation test, 
            // Submit the form data for saving
            // This is done with the KnockoutJS Library
        else {
            // get the current url path from the browser
            var urlPath = window.location.pathname;

            // This function uses KO bindings to create a ViewModel
            // Then makes an ajax post to the respective controller action
            // so that the record is added to the database
            
            $.ajax({
                url: urlPath,
                dataType: 'json',
                data: {
                    documentGroup: documentGroup, 
                    documentType: documentType, 
                    documentTags: tags,
                    documentOwner: customer,
                    acountNumber: accountNumber,
                    description: documentTitle
                },
                type: 'POST',
                success: function (result) {
                    // if the action suceeds, show a success message
                },
                error: function (err) {
                    // on failure, please alert the user accordingly
                }
            });
        }

        return false;

    });



});