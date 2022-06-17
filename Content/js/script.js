$(function () {

    var popup = false;

    $("#button").click(function () {
        if (popup == false) {
            $("#overlayEffect, #popupContainer, #close").show();            
            
            center();
            popup = true;
        }
    });

    $("#close").click(function () {
        hidePopup();
    });

    $("#overlayEffect").click(function () {		
        hidePopup();
    });

    function center() {
        var windowWidth = document.documentElement.clientWidth;
        var windowHeight = document.documentElement.clientHeight;
        var popupHeight = $("#popupContainer").outerHeight();
        var popupWidth = $("#popupContainer").outerWidth();
        $("#popupContainer").css({
            "position": "absolute",
            "top": windowHeight / 2 - popupHeight / 2,
            "left": windowWidth / 2 - popupWidth / 2
        });
    }
    function hidePopup() {
        if (popup == true) {
            $("#overlayEffect, #popupContainer, #close").hide(); 
            popup = false;
        }
    }

}, jQuery);


