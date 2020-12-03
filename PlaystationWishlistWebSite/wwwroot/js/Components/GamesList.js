function configureInitialStateOfButtons() {
    if ($(this).hasClass("add-to-wishlist")) {
        $(this).html("Add to wishlist");
        $(this).addClass("btn-primary");
        $(this).addClass("add-to-wishlist");
        $(this).removeClass("btn-danger");
        $(this).removeClass("remove-from-wishlist");
    } else {
        $(this).html("Remove from wishlist");
        $(this).addClass("btn-danger");
        $(this).addClass("remove-from-wishlist");
        $(this).removeClass("btn-primary");
        $(this).removeClass("add-to-wishlist");
    }

    $(this).prop("disabled", false);
};

function switchButton(button) {
    if (button.hasClass("remove-from-wishlist")) {
        button.html("Add to wishlist");
        button.addClass("btn-primary");
        button.addClass("add-to-wishlist");
        button.removeClass("btn-danger");
        button.removeClass("remove-from-wishlist");
    } else {
        button.html("Remove from wishlist");
        button.addClass("btn-danger");
        button.addClass("remove-from-wishlist");
        button.removeClass("btn-primary");
        button.removeClass("add-to-wishlist");
    }
};

$(document).ready(function () {
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