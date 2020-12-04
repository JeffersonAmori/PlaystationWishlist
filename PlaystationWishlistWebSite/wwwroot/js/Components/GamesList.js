function configureInitialStateOfButtons() {
    if ($(this).hasClass("add-to-wishlist")) {
        $(this).children().addClass("fa-plus");
        $(this).children().removeClass("fa-minus");
        $(this).addClass("btn-primary");
        $(this).addClass("add-to-wishlist");
        $(this).removeClass("btn-danger");
    } else {
        $(this).children().addClass("fa-minus");
        $(this).children().removeClass("fa-plus");
        $(this).addClass("btn-danger");
        $(this).addClass("remove-from-wishlist");
        $(this).removeClass("btn-primary");
    }

    $(this).prop("disabled", false);
};

function switchButton(button) {
    if (button.hasClass("remove-from-wishlist")) {
        button.children().addClass("fa-plus");
        button.children().removeClass("fa-minus");
        button.addClass("btn-primary");
        button.addClass("add-to-wishlist");
        button.removeClass("btn-danger");
    } else {
        button.children().addClass("fa-minus");
        button.children().removeClass("fa-plus");
        button.addClass("btn-danger");
        button.addClass("remove-from-wishlist");
        button.removeClass("btn-primary");
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
        $("#followersrefresh").empty();
        var url = $(this).data("request-url");
        $.ajax({
            type: "GET",
            url: url,
            data: { gameName: document.getElementById("gameName").value },
            success: function (result) {
                $("#gamesListViewComponent").html(result);
            }
        });
    });

    $(".btn-wishlist").each(configureInitialStateOfButtons);

    $(".btn-wishlist").click(function () {
        var originOfEvent = $(this);
        originOfEvent.prop("disabled", true);
        $.ajax({
            type: "POST",
            url: "Games/AddOrRemoveGameToWishList",
            data: { gameUrl: $(this).data("game-url"), remove: originOfEvent.hasClass("remove-from-wishlist") },
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
                    $.toast({
                        title: "Wishlist",
                        subtitle: "Just now",
                        content: "Done!",
                        type: "success",
                        delay: 3000,
                        pause_on_hover: true
                    });

                    switchButton(originOfEvent);
                }
            },
            complete: function () {
                originOfEvent.prop("disabled", false);
            }
        });
    });
});