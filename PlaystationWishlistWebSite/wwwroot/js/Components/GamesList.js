$(document).ready(function () {
    $("#btnGameSearch").click(function () {
        {
            $("#followersrefresh").empty();
            var _url = $(this).data("request-url");
            $.ajax({
                type: "GET",
                url: _url,
                data: { gameName: document.getElementById("gameName").value },
                success: function (result) {
                    $("#gamesListViewComponent").html(result);
                },
            });
        }
    });
});