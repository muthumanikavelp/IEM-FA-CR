var objDate1 = new Date();
var Presentyear1 = objDate1.getFullYear();

function SearchDate(id) {
    $("#" + id).datepicker({
        yearRange: '1900:' + Presentyear1,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });
}

function isEvent(evt) {
    return false;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function fnValidateIsRequired(id) {
    var txtValue = $("#" + id).val();
    if (txtValue.trim() != "") {
        $("#" + id).removeClass('required');
        $("#" + id).addClass('valid');
    }
    else {
        $("#" + id).removeClass('valid');
        $("#" + id).addClass('required');
    }
}