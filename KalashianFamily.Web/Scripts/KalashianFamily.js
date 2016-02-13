$(function () {
    //// NAVIGATION [BEGIN] ////
    $("#menu-icon").click(function () {
        var icon = $(this).children(".glyphicon");
        if (icon.hasClass("glyphicon-menu-hamburger")) {
            icon.removeClass("glyphicon-menu-hamburger").addClass("glyphicon-remove");
            $("#nav-wrap").show(400);
        } else {
            icon.addClass("glyphicon-menu-hamburger").removeClass("glyphicon-remove");
            $("#nav-wrap").hide(400);
        }
    });

    //// NAVIGATION [END] ////

    //// WEDDING [BEGIN] ////

    /**
     * On-Change event for the "Are you attending?" radio button question.
     * Reveals the arrival date field if affirmative.
     */
    $("input[name='Attending']").change(function () {
        if ($(this).data("radio-bool") === "True") {
            $("#ArrivalDate").parent().show(400);
            $("#otherAttendeesGroup").hide(400);
        } else {
            $("#ArrivalDate").parent().hide(400);
            $("#otherAttendeesGroup").show(400);
        }
    });

    $("#ArrivalDate").bind("keyup change paste", function () {
        var regex = /(0[1-9]|1[012])[- \/.](0[1-9]|[12][0-9]|3[01])[- \/.](19|20)\d\d/;

        if (regex.test($(this).val())) {
            $("#otherAttendeesGroup").show(400);
        }
    });

    $("input[name='OtherAttendees']").change(function () {
        if ($(this).data("radio-bool") === "True") {
            $("#attendee-list").show(400);
        } else {
            $("#attendee-list").hide(400);
        }

        $("button[type=submit]").parent().show(400);
    });

    /**
     * Initializes the Bootstrap DatePicker for the Wedding Arrival Date input.
     */
    $("input#ArrivalDate").datepicker({
        autoclose: true,
        defaultViewDate: {
            year: 2016,
            month: 10,
            day: 9
        },
        startDate: new Date(2016, 10, 1),
        endDate: new Date(2016, 10, 21)
    });

    if ($("input[name='Name']").length) $("input[name='Name']").focus();
    if ($("input[name='CaptchaPass']").length) $("input[name='CaptchaPass']").val("KALASHIAN_NOT_ROBOT");

    /**
     * Performs operations immediately before the submission of the RSVP form.
     * @param {} formData
     * @param {} jqForm
     * @param {} options
     * @returns {}
     */
    var rsvpBeforeSubmit = function (formData, jqForm, options) {
        jqForm.hide(400);
        $(".form-loader").show(400);
    };

    /**
     * Performs operations on a successful submission of the RSVP form.
     * @param {} responseText
     * @param {} statusText
     * @param {} xhr
     * @param {} $form
     * @returns {}
     */
    var rsvpSuccess = function (responseText, statusText, xhr, $form) {
        $(".form-loader").hide(400);
        var outputDiv = $("#rsvp-form-output");
        var name = $("input[name='Name']").val();
        var attending = $("input[name='Attending'][data-radio-bool='True']").is(":checked");
        if (responseText.status === 0) {
            // Error
            outputDiv.addClass("alert alert-danger");
            outputDiv.html("<h3><strong>Error!</strong> Sorry " + name + ", it looks like we ran into some trouble recording your RSVP. Here's the error message: " + responseText.error + "</h3>");
        } else {
            // Success
            outputDiv.addClass("alert alert-success");
            var innerMsg = attending ? "We'll keep you up to date on the latest Wedding information using the email address you provided. We can't wait to see you there!" : "We're sorry you won't be able to attend and we'll miss you!";
            outputDiv.html("<h3><strong>Thanks " + name + "!</strong> We've successfully recorded your RSVP for Nick & Becky's Wedding. " + innerMsg + "</h3>");

            if (attending) {
                setTimeout(function () {
                    $("#rsvp-next").show(400);
                }, 1500);
            }
        }
    };

    /**
     * Prepares the RSVP form for AJAX-based submission.
     */
    $("#rsvp-form").ajaxForm({
        target: "#rsvp-form-output",
        beforeSubmit: rsvpBeforeSubmit,
        success: rsvpSuccess,
        dataType: "json"
    });

    $("#attendee-list").dynamiclist();

    var weddingDate = new Date(2016, 10, 11, 14);

    var weddingClock = $("#wedding-clock").FlipClock(Math.abs((new Date().getTime() - weddingDate.getTime()) / 1000), {
        clockFace: "DailyCounter",
        countdown: true
    });

    //// WEDDING [END] ////
});

(function(kalashian) {
    
})(window.kalashian || (window.kalashian = {}));