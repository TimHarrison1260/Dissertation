/// <reference path="Demo.js" />
/// <reference path="../jquery-1.8.2.js" />

$(document).ready(function () {

    //  Set up handlers for the ajax start and completed events
    //  so that we can display a 'loading' overlay
    $('#loadingdata').ajaxStart(function () {
        $(this).show();
    });
    $('#loadingdata').ajaxComplete(function () {
        $(this).hide();
    });
    $('#loadingdata').ajaxStop(function () {
        $(this).hide();
    });

    //  Set up the Ajax Error handler
    $.ajaxSetup({
        error: function (xhr) {
            $('#loadingdata').hide();       //  hide if not already hidden
            $('#links').html('');
            $('#results').html('<h3>Error occurred</h3><p>Request Status: ' + xhr.status + ': ' + xhr.statusText + '</p><p>Message: ' + xhr.responseText + '</p>');
            $('#returnvalues').show();      //  Show the return div so message is visible on the page
        }
    });

    $('#loadingdata').hide();

    $('#returnvalues').hide();

    //  Add click handlers to the jsMenu items
    $('#jsMenu li a').click(function () {
        //  Build the ajax call and call the action method to get
        //  the partial page and show the results in the partPage div.
        //  Get the method to call from the title attriute of the anchor
        var partialPage = $(this).attr("data-page");

        var urlAction = $('#UrlAction').attr("data-urlaction").valueOf();  // use a dummy to resolve the url at run-time
        var url = urlAction.replace("PLACEHOLDER", partialPage);

        $('#links').html('');
        $('#results').html('');
        $('#returnvalues').hide();          //  hide in case there are any results showing

        $.ajax({
            url: url,
            type: 'GET',
            success: function (result) {
                //  Load the partial page into the partPage div
                $("#partPage").html(result);
                //  Add click handlers
                $('#apilinks ul li a').click(Demo.Api.click);
                $('#submiturl').click(Demo.Api.createUrl);
            }
        });
    });

});