$(function() {
    var availableHeight = $(document).height() - $("header").outerHeight();
    var mainStatsHeight = $("#main-stats").height();
    $(".main-content").width($(window).width() - $("#sidebar-nav").width());
    $(".main-content").height(availableHeight);
    $("#metadata-section").height($(".main-content").height());
    $("#document-viewer").height(availableHeight - mainStatsHeight - 2);
    $("#doc").height($("#document-viewer").height() - $("#docPath").outerHeight());
    

    $(window).resize(function () {
        var height = $(window).height() - $("header").outerHeight();
        var stats = $("#main-stats").height();
        $(".main-content").width($(window).width() - $("#sidebar-nav").width());
        $(".main-content").height(height);
        $("#metadata-section").height($(".main-content").height());
        $("#document-viewer").height(height - stats - 2);
        $("#doc").height($("#document-viewer").height() - $("#docPath").outerHeight());
    });

});

