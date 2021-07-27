// Write your JavaScript code.
$(function () {
    console.log("page is ready")

   /* $(document).on ("click", ".game-button", function (event) {
        event.preventDefault();

        var buttonNumber = $(this).val();
        console.log("button " + buttonNumber + " was clicked");
        doButtonUpdate(buttonNumber);
    })
    */
    $(document).on("mousedown", ".game-button", function (event) {
        event.preventDefault();
        switch (event.which) {
            case 1:
                var buttonNumber = $(this).val();
                console.log("Button Number " + buttonNumber + " was left clicked");
                doButtonUpdate(buttonNumber, 'Button/ShowOneButton');
                break;
            case 2:
                alert("middle mouse clicked");
                break;
            case 3:
                var buttonNumber = $(this).val();
                console.log("Button Number " + buttonNumber + " was right clicked");
                doButtonUpdate(buttonNumber, 'Button/RightClickShowOneButton');
                break;
            default:
                alert("nothing")
        }
    })


    //   $(document).bind("contextmenu", function (e) {
    //    e.preventDefault();
    //    console.log("right click. prevent context menu");
    //   })


    function doButtonUpdate(buttonNumber, urlString) {
        $.ajax({
            datatype: "json",
            method: 'POST',
            url: urlString,
            data: {
                "buttonNumber": buttonNumber
            },

            success: function (data) {

                //data should be a json object with part1 and part2 per the return value of the button controller.
                console.log(data);
                $("#" + buttonNumber).html(data.part1);
                $("#messageArea").html(data.part2);
            }
        })
    }
})