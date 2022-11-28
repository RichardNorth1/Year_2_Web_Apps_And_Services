

$(document).ready(function () {
    $("#button_1").click(function () {
        $('#EventDate').removeAttr("disabled")
        loadAvailableDates();
    });
});

function loadAvailableDates() {
    $.ajax({
        url: "https://localhost:7088/api/Availability" + $("#EventTypeId") + $("#EventDateStart") + $("#EventDateEnd")
                type: "GET",
        success: function (result) {
            $('#EventDate').html('');     // Empty select list
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < result.length; i++) {
                options += '<option value="' + result[i].date + '">' +
                    result[i].date + " " + result[i].name + " " +
                    result[i].capacity + " " + result[i].costPerHour +
                    '</option>';
            }
            $('#EventDate').append(options);
            $('#EventDate').removeAttr("disabled")
        },
        error: function () {
            alert("Unable to load movies list.");
        }
    });
}