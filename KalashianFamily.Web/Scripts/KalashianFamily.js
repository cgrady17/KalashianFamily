$(function () {
    //// WEDDING [BEGIN] ////

    /**
     * On-Change event for the "Are you attending?" radio button question.
     * Reveals the arrival date field if affirmative.
     */
    $("input[name='Attending']").change(function () {
        if ($(this).data("radio-bool") === "True") {
            $("#ArrivalDate").parent().show(400);
        } else {
            $("#ArrivalDate").parent().hide(400);
        }
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

    /**
     * Performs operations immediately before the submission of the RSVP form.
     * @param {} formData 
     * @param {} jqForm 
     * @param {} options 
     * @returns {} 
     */
    var rsvpBeforeSubmit = function(formData, jqForm, options) {

    };

    /**
     * Performs operations on a successful submission of the RSVP form.
     * @param {} responseText 
     * @param {} statusText 
     * @param {} xhr 
     * @param {} $form 
     * @returns {} 
     */
    var rsvpSuccess = function(responseText, statusText, xhr, $form) {

    };

    /**
     * Prepares the RSVP form for AJAX-based submission.
     */
    $("#rsvp-form").ajaxForm({
        target: "#rsvp-form-output",
        beforeSubmit: rsvpBeforeSubmit,
        success: rsvpSuccess,
        type: "json"
    });

    //// WEDDING [END] ////
});