// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#loader-wrapper").show();

$(document).ready(function () {
    configurePageNavbar();
    
    //$(".nav-bottom > li").click(function () {
    $("li[data-url]").click(function () {
        navigate($(this));
    });

    if (navigator.canShare) {
        $("#navButtonShare").off("click");
        $("#navButtonShare").click(function () {
            if (navigator.share) {
                navigator.share({
                    title: "Playstation Wishlist",
                    url: window.location.origin
                }).then(() => {
                    console.log("Thanks for sharing!");
                })
                    .catch(console.error);
            } else {
                window.alert("Functionality not supported by this browser.");
            }
        });
    } else {
        $("#navButtonShare").hide();
    }

    function navigate(el) {
        deactivateAllNavBottomButtons();
        $("#loader-wrapper").show();
        $("#mainContent").load(el.data("url"),
            function () {
                $("#loader-wrapper").hide();
            });
    };

    function configurePageNavbar() {
        if ($(window).width() <= 576) {
            $("#topNavBar").hide();
            $("#bottomNavBar").show();
        } else {
            $("#bottomNavBar").hide();
            $("#topNavBar").show();
        }
    }

    function deactivateAllNavBottomButtons() {
        $(".nav-bottom > li > button").removeClass("active");
    }

    window.onresize = function () {
        configurePageNavbar();
    };

    $("#loader-wrapper").hide();
});