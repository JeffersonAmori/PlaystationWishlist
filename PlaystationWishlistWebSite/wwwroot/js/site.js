// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#loader-wrapper").show();

$(document).ready(function () {
    configurePageNavbar();

    if (navigator.canShare) {
        $("#linkButtonShare").click(function () {
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

    $(".nav-bottom > li").click(function() {
        navigate($(this));
    });
    //$("#navButtonBookmark").click(function () {
    //    navigate($(this));
    //});

    //$("#navButtonSearch").click(function () {
    //    navigate($(this));
    //});

    //$("#navButtonInfo").click(function () {
    //    navigate($(this));
    //});

    function configurePageNavbar() {
        if ($(window).width() <= 576) {
            $("#topNavBar").hide();
            $("#bottomNavBar").show();
        } else {
            $("#bottomNavBar").hide();
            $("#topNavBar").show();
        }
    }

    function navigate(el) {
        deactivateAllNavBottomButtons();
        $("#loader-wrapper").show();
        $("#mainContent").load(el.data("url"),
            function () {
                $("#loader-wrapper").hide();
            });

    };


    function deactivateAllNavBottomButtons() {
        $(".nav-bottom > li").removeClass("active");
    }

    window.onresize = function () {
        configurePageNavbar();
    };

    $("#loader-wrapper").hide();
});