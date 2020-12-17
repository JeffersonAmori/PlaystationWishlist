function configureInitialStateOfButtons() {
    if ($(this).hasClass("btn-add-to-wishlist")) {
        $(this).children().addClass("fa-plus");
        $(this).children().removeClass("fa-minus");
        $(this).addClass("btn-add-to-wishlist");
        $(this).addClass("btn-success");
        $(this).removeClass("btn-remove-from-wishlist");
    } else {
        $(this).children().addClass("fa-minus");
        $(this).children().removeClass("fa-plus");
        $(this).addClass("btn-remove-from-wishlist");
        $(this).addClass("btn-danger");
        $(this).removeClass("btn-add-to-wishlist");
    }

    $(this).prop("disabled", false);
};

function switchButton(button) {
    if (button.hasClass("btn-remove-from-wishlist")) {
        button.children().addClass("fa-plus");
        button.children().removeClass("fa-minus");
        button.addClass("btn-add-to-wishlist");
        button.addClass("btn-success");
        button.removeClass("btn-remove-from-wishlist");
        button.removeClass("btn-danger");
    } else {
        button.children().addClass("fa-minus");
        button.children().removeClass("fa-plus");
        button.addClass("btn-remove-from-wishlist");
        button.addClass("btn-danger");
        button.removeClass("btn-add-to-wishlist");
        button.removeClass("btn-success");
    }
};

$(document).ready(function () {
    var input = document.getElementById("gameName");
    input.addEventListener("keyup", function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            document.getElementById("btnGameSearch").click();
        }
    });

    $("#btnGameSearch").click(function () {
        $("#loader-wrapper").show();
        //$("#gamesListViewComponentTable").replaceWith($("#loader-wrapper"));

        var url = $(this).data("request-url");
        $.ajax({
            type: "GET",
            url: url,
            data: { gameName: document.getElementById("gameName").value },
            success: function (result) {
                $("#gamesListViewComponent").html(result);
                $("#loader-wrapper").hide();
            }
        });
    });

    $(".btn-wishlist").each(configureInitialStateOfButtons);

    $(".btn-wishlist").click(function () {
        var originOfEvent = $(this);
        var remove = originOfEvent.hasClass("btn-remove-from-wishlist");
        originOfEvent.prop("disabled", true);
        originOfEvent.addClass("disabled");
        $.ajax({
            type: "POST",
            url: window.addOrRemoveGameOnWishlistUrl,
            data: { gameUrl: $(this).data("game-url"), remove: remove },
            success: function (result) {
                if (result.status === "NOK") {
                    $.toast({
                        title: "Wishlist",
                        subtitle: "Just now",
                        content: result.message,
                        type: "error",
                        delay: 3000,
                        pause_on_hover: true
                    });
                } else {
                    var text = remove ? "Removed from wishlist" : "Added to wishlist";
                    $.toast({
                        title: "Wishlist",
                        subtitle: "Just now",
                        content: text,
                        type: "success",
                        delay: 3000,
                        pause_on_hover: true
                    });

                    switchButton(originOfEvent);
                }
            },
            complete: function () {
                originOfEvent.removeClass("disabled");
                originOfEvent.prop("disabled", false);
            }
        });
    });
});