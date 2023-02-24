var objDialogysupplierdetails;
UrlDet = UrlDet.replace("test", "");
var SearchModel1 = function (id) {
    var self = this;
    self.Approvedsupplierlistarray = ko.observableArray();
    self.CurrencyArray = ko.observableArray();






    self.Supplierdetails = function (item) {
        $.ajax({
            type: "post",
            url: UrlDet + "Getsupplierlist",
            data: "{}",
            contentType: "application/json;",
            async: false,
            success: function (response) {
                objDialogysupplierdetails.dialog("open");
                var Data1 = "";
                Data1 = JSON.parse(response.Data1);
                self.Approvedsupplierlistarray(Data1);
            }
        });
        //objDialogysupplierdetails.dialog("open");
        return false;
    }


    this.selectsupplier = function (item) {
        $("#txtSuppliercode").val(item.supplierheader_suppliercode);
        $("#txtSuppliername").val(item.supplierheader_name);
        $("#hfSupplierId").val(item.supplierheader_gid)
        objDialogysupplierdetails.dialog("close");
        return false;
    };

    $("#supcode").keyup(function (e) {
        if (e.which != 13 && e.which != 9) {
            $("#supcode").removeClass("valid");
            $("#supcode").addClass("required");
        }
        $("#supcode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlDet + "GetAutoCompleteSupplierCode",
                    data: "{ 'txt' : '" + $("#supcode").val() + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[1],
                                val: item.split('~')[0]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (e, i) {
                $("#supcode").val(i.item.label);
                $("#supcode").addClass("valid");
                $("#supcode").removeClass("required");
            }
        });
    });


    $("#supname").keyup(function (e) {

        $("#supname").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlDet + "GetAutoCompleteSupplier",
                    data: "{ 'txt' : '" + $("#supname").val() + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[1],
                                val: item.split('~')[0]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (e, i) {
                    $("#supname").removeClass("required").removeClass("valid").addClass("valid");
            }
        });
    });


    self.DatatableCall = function () {
        if ($("#gvSummary > tbody > tr").length == self.Approvedsupplierlistarray().length) {
            $("#gvSummary").DataTable({
                "autoWidth": false,
                "destroy": true,
                "aoColumnDefs": [{
                    "aTargets": ["nosort"],
                    "bSortable": false
                },
                {
                    "aTargets": ["colDate"],
                    "bSortable": true,
                    "sType": "date-uk"
                }]
            });
        }
    };

    self.Selectsearch = function () {
        debugger;
        var suppliercode = $("#supcode").val();
        var suppliername = $("#supname").val();
        var InputFilter = {
            suppliercode: suppliercode,
            suppliername: suppliername
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SearchSupplierDetails",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.Approvedsupplierlistarray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };


    self.loadCurrency = function () {
        $.ajax({
            type: "post",
            url: UrlDet + "GetCurrency",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                Data1 = JSON.parse(response.Data1);
                self.CurrencyArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };
    self.loadCurrency();
    $("#currencyNames").change(function () {
        var Currency = $("#currencyNames").val();
        if (Currency != "0") {
            $("#currencyNames").removeClass("required");
            var RateFilter = {
                Currency: Currency
            };
            $.ajax({
                type: "post",
                url: UrlDet + "GetCurrencyRate",
                async: false,
                data: JSON.stringify(RateFilter),
                contentType: "application/json;",
                success: function (response) {
                    var Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                        Data1 = JSON.parse(response.Data1);
                    if (Data1.length > 0) {
                        $("#txtexrate").val(Data1[0].CurrencyRate);
                        var curramountexrate = $("#txtexrate").val();
                        var curramount = $("#txtecfcurramount").val();
                        var curramountnew = curramount.replace(/,/g, "");
                       
                        if (curramountnew != null && curramountnew != "" && curramountexrate != null && curramountexrate != "") {
                            var currentvalnew = $('#txtexrate').val().replace(/,/g, "") * parseFloat(curramountnew);
                            var currentval = currentvalnew.toFixed(2);
                            var testDecimal = currentval.match(/\./g) === null ? count = 0 : count = currentval.match(/\./g);


                            if (testDecimal.length > 1) {
                                currentval = currentval.slice(0, -1);
                            }


                          //  currentval = currentval.slice(0, -1);
                            var tolnew = currentval;
                            $('#txtecfamont').val(tolnew);

                        }
                    }
                    else
                        $("#txtexrate").val("0");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
        else {
            $("#txtexrate").val("0");
            $("#currencyNames").removeClass("valid");
            $("#currencyNames").addClass("required");
        }
        
    });



    $("#txtecfcurramount,#txtexrate").change(function () {
        var curramountexrate = $("#txtexrate").val();
        var curramount = $("#txtecfcurramount").val();
        var curramountnew = curramount.replace(/,/g, "");

        $("#txtecfamont").val('');
        if (curramountexrate == null || curramountexrate == "") {
            jAlert("Please Enter Exchange Rate.", "Message");
            $("#txtexrate").focus();
            return false;
        }
        if (curramount == null || curramount == "") {
            $("#txtecfamont").val('');
            return false;
        }
        if (curramountnew != null && curramountnew != "" && curramountexrate != null && curramountexrate != "") {
            var currentvalnew = $('#txtexrate').val().replace(/,/g, "") * parseFloat(curramountnew);
            var currentval = currentvalnew.toFixed(2);
            var testDecimal =  currentval.match(/\./g) === null ? count = 0 : count = currentval.match(/\./g);
            if (testDecimal.length > 1) {
                currentval = currentval.slice(0, -1);
            }
            //currentval = currentval.slice(0, -1);
            var tolnew = currentval;
            $('#txtecfamont').val(tolnew);

        }
    });


    $("#txtecfcurramount,#txtexrate").keyup(function () {
        var curramountexrate = $("#txtexrate").val();
        var curramount = $("#txtecfcurramount").val();
        var curramountnew = curramount.replace(/,/g, "");

        $("#txtecfamont").val('');
        if (curramountexrate == null || curramountexrate == "") {
            jAlert("Please Enter Exchange Rate.", "Message");
            $("#txtexrate").focus();
            return false;
        }
        if (curramount == null || curramount == "") {
            $("#txtecfamont").val('');
            return false;
        }
        if (curramountnew != null && curramountnew != "" && curramountexrate != null && curramountexrate != "") {
            var currentvalnew = $('#txtexrate').val().replace(/,/g, "") * parseFloat(curramountnew);
            var currentval = currentvalnew.toFixed(2);
            var testDecimal =currentval.match(/\./g) === null ? count = 0 : count = currentval.match(/\./g);
            if (testDecimal.length > 1) {
                currentval = currentval.slice(0, -1);
            }
          //  currentval = currentval.slice(0, -1);
            var tolnew = currentval;
            $('#txtecfamont').val(tolnew);

        }
    });

};

var mainViewModel1 = new SearchModel1();
//ko.cleanNode(document.getElementById('_supplierFirst'));
ko.applyBindings(mainViewModel1, document.getElementById("_supplierFirst"));

$(document).ready(function () {
    objDialogysupplierdetails = $("[id$='ShowDialog1']");
    objDialogysupplierdetails.dialog({
        autoOpen: false,
        modal: true,
        width: 1200,
        height: 420,
        duration: 300

    });
});

$(document).ready(function () {
    $("#txtecfdates").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-mm-yy',
        maxDate: '-id',
        minDate: '+id',
    });
    $('#txtecfdates').datepicker().datepicker('setDate', 'today');
    var txtclaimmonthh = $("#txtclaimmonthh").val();
    if (txtclaimmonthh.trim() != "") {
        $("#txtecfcurramount").val(txtclaimmonthh);
    }
    var txtecfdescriptionh = $("#txtecfdescriptionh").val();
    if (txtecfdescriptionh.trim() != "") {
        $("#txtecfdescription").val(txtecfdescriptionh);
    }
    $("#txtecfdescriptionh").val('');
    var txtecfdescription = $("#txtecfdescription").val();
    if (txtecfdescription.trim() != "") {
        $("#txtecfdescription").addClass('valid');
    }
    else {
        $("#txtecfdescription").addClass('required');
    }
    var curramountid = $("#currencyNames").val();
    var txtContractFrom1 = $("#currencyNames option:selected").text();
    if (txtContractFrom1 == "INR") {
        $("#txtexrate").val('1.00');
        $("#CurrencyNamevalid").val('INR');
        $("#Currencyidvalid").val(curramountid);
    }
});
$(document).ready(function () {
    $("#txtSuppliercode").keyup(function () {
        var txtContractFrom = $("#txtSuppliercode").val();
        if (txtContractFrom.trim() != "") {
            $("#txtSuppliercode").removeClass('required');
            $("#txtSuppliercode").addClass('valid');
        }
        else {
            $("#txtSuppliercode").removeClass('valid');
            $("#txtSuppliercode").addClass('required');
        }
    });
    $("#txtSuppliercode").change(function () {
        var txtContractFrom = $("#txtSuppliercode").val();
        if (txtContractFrom.trim() != "") {
            $("#txtSuppliercode").removeClass('required');
            $("#txtSuppliercode").addClass('valid');
        }
        else {
            $("#txtSuppliercode").removeClass('valid');
            $("#txtSuppliercode").addClass('required');
        }
    });

    $("#txtSuppliername").keyup(function () {
        var txtContractFrom = $("#txtSuppliername").val();
        if (txtContractFrom.trim() != "") {
            $("#txtSuppliername").removeClass('required');
            $("#txtSuppliername").addClass('valid');
        }
        else {
            $("#txtSuppliername").removeClass('valid');
            $("#txtSuppliername").addClass('required');
        }
    });
    $("#txtSuppliername").change(function () {
        var txtContractFrom = $("#txtSuppliername").val();
        if (txtContractFrom.trim() != "") {
            $("#txtSuppliername").removeClass('required');
            $("#txtSuppliername").addClass('valid');
        }
        else {
            $("#txtSuppliername").removeClass('valid');
            $("#txtSuppliername").addClass('required');
        }
    });

    $("#currencyNames").keyup(function () {
        var txtContractFrom = $("#currencyNames option:selected").text();
        if (txtContractFrom.trim() != "-- Select --") {
            $("#currencyNames").removeClass('required');
            $("#currencyNames").addClass('valid');
        }
        else {
            $("#currencyNames").removeClass('valid');
            $("#currencyNames").addClass('required');
        }
    });
    $("#currencyNames").change(function () {
        var txtContractFrom = $("#currencyNames option:selected").text();
        if (txtContractFrom.trim() != "-- Select --") {
            $("#currencyNames").removeClass('required');
            $("#currencyNames").addClass('valid');
        }
        else {
            $("#currencyNames").removeClass('valid');
            $("#currencyNames").addClass('required');
        }
    });
    $("#txtecfdescription").keyup(function () {
        var txtContractFrom = $("#txtecfdescription").val();
        if (txtContractFrom.trim() != "") {
            $("#txtecfdescription").removeClass('required');
            $("#txtecfdescription").addClass('valid');
        }
        else {
            $("#txtecfdescription").removeClass('valid');
            $("#txtecfdescription").addClass('required');
        }
    });
    $("#txtecfdescription").change(function () {
        var txtContractFrom = $("#txtecfdescription").val();
        if (txtContractFrom.trim() != "") {
            $("#txtecfdescription").removeClass('required');
            $("#txtecfdescription").addClass('valid');
        }
        else {
            $("#txtecfdescription").removeClass('valid');
            $("#txtecfdescription").addClass('required');
        }
    });
});
$(document).ready(function () {


    $('input[type=radio][name=rblAmort]').change(function () {

        if (this.value == 'Yes') {

            $("#amotrd1").fadeIn();
            $("#amotrd2").fadeIn();
            $("#amotrd3").fadeIn();

            var stylesSelect = $('#txtAmortto');
            stylesSelect.attr('disabled', false);
            var stylesSelect = $('#txtAmortDescriptionid');
            stylesSelect.attr('disabled', false);
            var stylesSelect = $('#txtAmortfron');
            stylesSelect.attr('disabled', false);
            $("#txtAmortfron").addClass('required');
            $("#txtAmortto").addClass('required');
        }
        else {

            $("#amotrd1").fadeOut();
            $("#amotrd2").fadeOut();
            $("#amotrd3").fadeOut();

            $("#txtAmortfron").val('')
            $("#txtAmortto").val('')
            var stylesSelect = $('#txtAmortfron');
            stylesSelect.attr('disabled', true);
            var stylesSelect = $('#txtAmortDescriptionid');
            stylesSelect.attr('disabled', true);
            var stylesSelect = $('#txtAmortto');
            stylesSelect.attr('disabled', true);
            $("#txtAmortfron").removeClass('required');
            $("#txtAmortto").removeClass('required');
            $("#txtAmortfron").removeClass('valid');
            $("#txtAmortto").removeClass('valid');
        }
    });
});
$(document).ready(function () {
    $("#txtAmortfron").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-mm-yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtAmortfron").addClass('valid');
            $("#txtAmortto").datepicker("option", "minDate", selected);
        }
    });
    $("#txtAmortto").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-mm-yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtAmortto").addClass('valid');
            $("#txtAmortfron").datepicker("option", "maxDate", selected);
        }
    });


    var selmode = $("#raisermodeNamet option:selected").text();
    if (selmode.trim() != "") {
        $("#txtmodes").text(selmode);
    }
    else {
        $("#txtmodes").text('Self');
    }
});
$(document).ready(function () {
    $("#amotrd1").hide();
    $("#amotrd2").hide();
    $("#amotrd3").hide();
    var stylesSelect = $('#txtAmortfron');
    stylesSelect.attr('disabled', true);

    var stylesSelect = $('#txtAmortDescriptionid');
    stylesSelect.attr('disabled', true);

    var stylesSelect = $('#txtAmortto');
    stylesSelect.attr('disabled', true);

    var $radios = $('input:radio[name=rblAmort]');
    var txtecfdateh = $("#txtAmort").val();
    if(txtecfdateh != null)
    {
        if (txtecfdateh.trim() == "Yes") {
            $("#amotrd1").fadeIn();
            $("#amotrd2").fadeIn();
            $("#amotrd3").fadeIn();
            $radios.filter('[value=Yes]').prop('checked', true);

            var stylesSelect = $('#txtAmortto');
            stylesSelect.attr('disabled', false);
            var stylesSelect = $('#txtAmortDescriptionid');
            stylesSelect.attr('disabled', false);
            var stylesSelect = $('#txtAmortfron');
            stylesSelect.attr('disabled', false);
        }
        else {
            $("#amotrd1").fadeOut();
            $("#amotrd2").fadeOut();
            $("#amotrd3").fadeOut();
            $radios.filter('[value=No]').prop('checked', true);
            var stylesSelect = $('#txtAmortto');
            stylesSelect.attr('disabled', true);
            var stylesSelect = $('#txtAmortDescriptionid');
            stylesSelect.attr('disabled', true);
            var stylesSelect = $('#txtAmortfron');
            stylesSelect.attr('disabled', true);
        }
    }


    //Submit ECF header details

    $('#btnsupplier').click(function () {
        showProgress();
        var ecfdescription = $("#txtecfdescription").val();
        var ecfdate = $("#txtecfdates").val();
        var doctypeNames = $("#doctypeNames").val();
        var Suppliercode = $("#txtSuppliercode").val();
        var Suppliername = $("#txtSuppliername").val();
        var currencyNames = $("#currencyNames").val();
        var exrate = $("#txtexrate").val();
        var ecfcurramount = $("#txtecfcurramount").val();
        var currencyNames1 = $("#currencyNames option:selected").text();
        var suppliercodegid = $("#txtSuppliercodegid").val();
        if (suppliercodegid == null || suppliercodegid == 0 || suppliercodegid == "") {
            jAlert("Please Select supplier code.", "Message"); hideProgress();
            $("#txtSuppliercode").focus();
            return false;
        }
        if (ecfdate == null || ecfdate == "") {
            jAlert("Please Select ECF Date.", "Message"); hideProgress();
            $("#txtecfdate").focus();
            return false;
        }
        if (doctypeNames == null || doctypeNames == "0") {
            jAlert("Please Select Doc Type.", "Message");
            hideProgress();
            return false;
        }
        if (Suppliercode == null || Suppliercode == "") {
            jAlert("Please Select Supplier Code.", "Message"); hideProgress();
            return false;
        }
        if (Suppliername == null || Suppliername == "") {
            jAlert("Please Select Supplier Name.", "Message"); hideProgress();
            return false;
        }
        if (currencyNames1 == null || currencyNames1 == "-- Select --") {
            jAlert("Please Select Currency.", "Message"); hideProgress();
            return false;
        }

        if (exrate == null || exrate == "") {
            jAlert("Please Enter Currency Rate.", "Message"); hideProgress();
            return false;
        }
        if (ecfcurramount == null || ecfcurramount == "") {
            jAlert("Please Enter Currency Amount.", "Message"); hideProgress();
            $("#txtecfamount").focus();
            return false;
        }

        var rodio = $("input[name=rblAmort]:checked").val()
        if (rodio == 'undefined' || rodio == null) {
            jAlert("Please Select Amort.", "Message"); hideProgress();
            $("#rblAmort").focus();
            return false;
        }
        else if (rodio == "Yes") {
            var txtContractFrom = $("#txtAmortfron").val();
            if (txtContractFrom.trim() == "") {
                jAlert("Please Enter Amort From Amount.", "Message"); hideProgress();
                return false;
            }
            var txtContractto = $("#txtAmortto").val();
            if (txtContractto.trim() == "") {
                jAlert("Please Enter Amort To Date.", "Message"); hideProgress();
                return false;
            }
            var amortdesc = $("#txtAmortDescriptionid").val();
            if (amortdesc == null || amortdesc == "" || amortdesc == undefined) {
                jAlert("Please Enter Amort Description.", "Message"); hideProgress();
                return false;
            }

        }
        if (ecfdescription == null || ecfdescription == "") {
            jAlert("Please Enter ECF Description.", "Message"); hideProgress();
            return false;
        }
        hideProgress();
        //var data = {
        //    Suppliergid: suppliercodegid,
        //    ecf_date: ecfdate,
        //    ecfdescription: ecfdescription,
        //    CurrencyId: currencyNames,
        //    CurrencyName: currencyNames1,
        //    Exrate: exrate,
        //    ECF_Amount: ecfcurramount,
        //    Currencyamount: ecfcurramount,
        //    ecfremark: ecfdescription,
        //    amort: rodio,
        //    from: txtContractFrom,
        //    to: txtContractto,
        //    amortdec: amortdesc

        //};

    });

});

