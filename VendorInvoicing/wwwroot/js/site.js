//jQuery Datapicker
$(document).ready(function () {
    $('#datepicker').datepicker({
        buttonText: "Select date",
        dateFormat: 'yy/mm/dd',
        changeMonth: true,
        changeYear: true,
        yearRange: "-0:+100",
        beforeShow: function (input, inst) {
            setTimeout(function () {
                inst.dpDiv.css({
                    top: input.offsetTop - $(input).outerHeight() - inst.dpDiv.outerHeight(),
                    left: input.offsetLeft
                });
            }, 0);
        }
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
//jQuery Number Selector
$(document).ready(function () {
    $("#spinner").spinner({
        culture: "en-US",
        min: 5,
        step: 0.50,
        start: 1000,
        numberFormat: "C"
    });
});