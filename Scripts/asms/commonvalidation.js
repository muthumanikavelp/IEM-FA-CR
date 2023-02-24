//0-9 Delete BackSpace
function isNumber(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 47 && charCode < 58 || charCode == 127 || charCode == 8)
        return true;
    return false;
}
//0-9 (with only one dot)
function isNumber1(evt, element1) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        (charCode != 46 || $(element1).val().indexOf('.') != -1) &&
        (charCode != 8) &&
        (charCode < 48 || charCode > 57))
        return false;
    return true;
}
//0-9 (with only one dot and one minus)
function isAmount(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        //(charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 ) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57) && (charCode != 8))
        return false;
    return true;
}
//0-9 (with only one dot and comma)
//function isAmount(evt, element) {
//    var charCode = (evt.which) ? evt.which : event.keyCode
//    if (
//        //(charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
//        (charCode != 46 || $(element).val().indexOf('.') != -1) &&
//        (charCode != 44) &&   // “.” CHECK DOT, AND ONLY ONE.
//        (charCode < 48 || charCode > 57) && (charCode != 8))
//        return false;
//    return true;
//}
//a-z A-Z Space Delete BackSpace
function isAlphabet(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 127 || charCode == 8 || charCode == 32)
        return true;
    return false;
}
//a-z A-Z 0-9 Space Delete BackSpace
function isAlphaNumeric(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 64 && charCode < 91 || charCode > 96 && charCode < 123 || charCode > 47 && charCode < 58 || charCode == 127 || charCode == 8 || charCode == 32)
        return true;
    return false;
}
//a-z A-Z 0-9 Space Delete BackSpace
function isAlphaNumericCaps(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 64 && charCode < 91 || charCode > 96 && charCode < 123 || charCode > 47 && charCode < 58 || charCode == 127 || charCode == 8 || charCode == 32)
        return true;
    return false;
}
//a-z A-Z 0-9 . Space Delete BackSpace
function isAlphaNumericWithDot(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 64 && charCode < 91 || charCode > 96 && charCode < 123 || charCode > 47 && charCode < 58 || charCode == 127 || charCode == 8 || charCode == 32 || charCode == 46)
        return true;
    return false;
}
//except ' ` ~
function isAlphaNumericSpecialChars(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 39 || charCode == 96 || charCode == 126) {
        return false;
    }
    else {
        return true;
    }
}
//except ` ~
function isExceptTwoSplChars(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 96 || charCode == 126) {
        return false;
    }
    else {
        return true;
    }
}
//except '
function ExceptSingleQuote(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 39) {
        return false;
    }
    else {
        return true;
    }
}
function isValidEmail(email) {
    // var reg = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    var reg = /^([a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)$/i;
    return reg.test(email);
}

function isDecimal(value) {
    var reg = /^([\d]){0,1}(\.[\d]{0,1})$/
    return reg.test(value);
}

function isDate(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 47 && charCode < 58 || charCode == 127 || charCode == 8 || charCode == 45)
        return true;
    return false;
}

//a-z A-Z 0-9 ' ? @ , Space Delete BackSpace
function isSplAlphaNumeric(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 64 && charCode < 91 || charCode > 96 && charCode < 123 || charCode > 47 && charCode < 58 || charCode == 127 || charCode == 8 || charCode == 32 || charCode == 39 || charCode == 63 || charCode == 64 || charCode == 44)
        return true;
    return false;
}
//a-z A-Z 0-9 ' , Space Delete BackSpace
function isAlphaNumericWithSingleQuote(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 64 && charCode < 91 || charCode > 96 && charCode < 123 || charCode > 47 && charCode < 58 || charCode == 127 || charCode == 8 || charCode == 32 || charCode == 39 || charCode == 44)
        return true;
    return false;
}
//a-z A-Z ' Space Delete BackSpace
function isAlphabetWithSingleQuote(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 127 || charCode == 8 || charCode == 32 || charCode == 39)
        return true;
    return false;
}
//0-9 + - Space Delete BackSpace
function isPhoneNumber(evt, element1) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        (charCode != 43 || $(element1).val().indexOf('+') != -1) &&
        (charCode != 8) &&
         (charCode != 45 || $(element1).val().indexOf('-') != -1) &&
        (charCode < 48 || charCode > 57))
        return false;
    return true;
}
//a-z A-Z 0-9 > < Space Delete BackSpace
function isAlphaNumericWithLtGt(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 64 && charCode < 91 || charCode > 96 && charCode < 123 || charCode > 47 && charCode < 58 || charCode == 127 || charCode == 8 || charCode == 32 || charCode == 60 || charCode == 62)
        return true;
    return false;
}
//for Attachemnet -------------Pandiaraj 01-11-2019
function Attachment_list() {
    var Attach_list = ["text/plain", "application/octet-stream", "image/pjpeg", "application/pdf", "text/richtext", "image/x-png", "image/gif"];
    return Attach_list;
}

function Attached_fileformat() {
    var Attaching_format = [".png", ".jpg", ".pdf", ".xls", ".xlsx", ".doc", ".docx", ".gif", ".eml", ".msg", ".txt", ".rtf"]
    return Attaching_format
}
//for Attachemnet -------------Pandiaraj 01-11-2019

function Attachment_size() {
    var fsize = 8388608;
    return fsize;
}