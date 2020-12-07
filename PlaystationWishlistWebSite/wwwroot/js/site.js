// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#loader-wrapper").show();

$(document).ready(function () {
    if (navigator.share) {
        $("#navButtonShare").find("a").click(function () {
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
    }

    configurePageNavbar();
    $("#loader-wrapper").hide();
});

function configurePageNavbar() {
    if ($(window).width() <= 576) {
        $("#topNavBar").hide();
        $("#bottomNavBar").show();
    } else {
        $("#bottomNavBar").hide();
        $("#topNavBar").show();
    }
}

window.onresize = function () {
    configurePageNavbar();
};