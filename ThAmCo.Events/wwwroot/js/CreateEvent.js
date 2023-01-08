

$(document).ready(function () {
    $("#Availability_btn").click(function () {
        event.preventDefault();
        $('#EventDate').removeAttr("disabled")
        loadAvailableDates();
    });
    loadEventType();
    loadMenus();
});
function loadEventType() {
    $.ajax({
        url: "https://localhost:7088/api/eventtypes",
        type: "GET",

        success: function (result) {
            $('#SelectedEventType').html('');     // Empty select list
            var options = '';
            

            for (var i = 0; i < result.length; i++) {
                options += '<option value="' + result[i].id + "," + result[i].title  + '">' +
                    result[i].title + '</option>';
            }
            $('#SelectedEventType').append(options);
        },
        error: function () {
            alert("Unable to load event types.");
        }
    });
}
function loadMenus() {
    $.ajax({
        url: "https://localhost:7173/api/Menus",
        type: "GET",

        success: function (result) {
            $('#SelectedMenuIdAndName').html('');     // Empty select list
            var options = '';
            options += '<option value="0,No Catering">Catering Not Required</option>';
            for (var i = 0; i < result.length; i++) {
                options += '<option value="' + result[i].menuId + "," + result[i].menuName + '">' +
                    result[i].menuName + '</option>';
            }
            $('#SelectedMenuIdAndName').append(options);
        },
        error: function () {
            alert("Unable to load Menus.");
        }
    });
}

function loadAvailableDates() {
    $.ajax({
        //api/Availability?eventType=WED&beginDate=2022-11-01&endDate=2022-11-30
        url: "https://localhost:7088/api/Availability?eventType=" + $("#SelectedEventType").val().split(',')[0] + "&beginDate="
            + $("#EventDateStart").val() + "&endDate=" + $("#EventDateEnd").val(),
        type: "GET",
        success: function (result) {

            $('#EventDate').html('');     // Empty select list
            var options = '';

            for (var i = 0; i < result.length; i++) {

                options += '<option value="' + result[i].date + "," + result[i].code + "," + result[i].name + "," + result[i].costPerHour + '">' + "Date: " +
                    result[i].date.split('T')[0] + "  Venue: " + result[i].name + "  Seating capacity: " +
                    result[i].capacity + "  Cost: £" + result[i].costPerHour + " per hour " +
                    '</option>';
            }
            $('#SelectedEventDate').append(options);
            $('#SelectedEventDate').removeAttr("disabled")
            return;
        },

        error: function () {
            alert("Unable to load available venues and dates.");
        }
    });
}