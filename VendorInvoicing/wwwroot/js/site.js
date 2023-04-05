//jQuery Datapicker
$(document).ready(function () {
    $('#datepicker').datepicker({
        buttonText: "Select date",
        dateFormat: 'yy/mm/dd',
        changeMonth: true,
        changeYear: true,
        yearRange: "-0:+100"
    });
});
//Timeout for Alerts to disappear after
$(document).ready(function () {
    $("[data-dismiss-timeout]").each(function () {
        var $elem = $(this);
        var timeout = $elem.data("dismiss-timeout");
        setTimeout(function () {
            $elem.alert("close");
        }, timeout);
    });
});
//Remove Query from Url (Not Needed but i thought was nice)
$(document).ready(function () {
    var uri = window.location.toString();

    if (uri.indexOf("?") > 0) {
        var clean_uri = uri.substring(0, uri.indexOf("?"));
        window.history.replaceState({}, document.title, clean_uri);
    }
});