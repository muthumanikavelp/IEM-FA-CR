var objDialogyAdd;
var objDialogyEdit;

UrlDet = UrlDet.replace("AmortSearchDetails", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var AmortEditModel = function () {
    var self = this;
    self.ECFId = ko.observable("");
    self.ECFDate = ko.observable("");
    self.ECFNO = ko.observable("");
    self.suppliercode = ko.observable("");
    self.suppliername = ko.observable("");
    self.ecfamount = ko.observable("");
    self.completed = ko.observable("");
    self.balance = ko.observable("");
    self.cycle = ko.observable("");
    self.splitmonth = ko.observable("");
    self.completedmonth = ko.observable("");
    self.Balancemonth = ko.observable("");
    self.CurrentMonthAmount = ko.observable("");
    self.AmortStartDate = ko.observable("");
    self.AmortEndDate = ko.observable("");
    self.Amortmonth = ko.observable("");
    self.Slno = ko.observable("");
    self.Amount = ko.observable("");
    self.supplierId = ko.observable("");

    var AmortEditDetails = {
        ECFId: self.ECFId,
        ECFDate: self.ECFDate,
        ECFNO: self.ECFNO,
        suppliercode: self.suppliercode,
        supplierId: self.supplierId,
        suppliername: self.suppliername,
        ecfamount: self.ecfamount,
        completed: self.completed,
        balance: self.balance,
        cycle: self.cycle,
        splitmonth: self.splitmonth,
        completedmonth: self.completedmonth,
        Balancemonth: self.Balancemonth,
        CurrentMonthAmount: self.CurrentMonthAmount,
        AmortStartDate: self.AmortStartDate,
        AmortEndDate: self.AmortEndDate,

    };

    var AmortReScheduleArray = {
        Slno: self.Slno,
        Amortmonth: self.Amortmonth,
        Amount: self.Amount
    };

    self.AmortReScheduleArray = ko.observableArray();
    self.AmortEdit = ko.observable();
    self.AmortCycle = ko.observableArray();
    self.AmortEditDetails = ko.observableArray();
    self.AmortDebitLineArray = ko.observableArray();


    self.loadAmortCycle = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadAmortCycle",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.AmortCycle(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.formatNumber = function (num) {
        //return Number(num).toFixed(2);
        var currentval = Number(num).toFixed(2);

        var testDecimal;
        currentval.match(/\./g) === null ? testDecimal = 0 : testDecimal = currentval.match(/\./g);
        if (testDecimal.length > 1) {
            currentval = currentval.slice(0, -1);
        }

        var components = currentval.toString().split(".");
        if (components.length === 1)
            components[0] = currentval;
        components[0] = components[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        if (components.length === 2)
            components[1] = components[1].replace(/\D/g, "");
        return components.join(".");
    };

    self.search = function () {
        var AmortDetails = {
            ECFFrom: $("#txtECFFrom").val(),
            ECFTo: $("#txtECFTo").val(),
            ECFNo: $("#txtEcfNumber").val(),
            ECFAmount: $("#txtEcfAmount").val().replace(/,/g, ''),
            SupplierCodeId: $("#hdfSSupplierId").val(),
            SupplierNameId: $("#hdfSSupplierName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "AmortSearchDetails",
            async: false,
            data: JSON.stringify(AmortDetails),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.AmortEditDetails(Data2);
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }

                    if (Data1[0].Msg == "" && self.AmortEditDetails().length == 0) {
                        jAlert("No Records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvAmortEditDetails > tbody > tr").length == self.AmortEditDetails().length) {
            $("#gvAmortEditDetails").DataTable({
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

    self.view = function (item) {
        $("#txtEcfNoView").val(item.ECFNO);
        $("#txtEcfAmountView").val(self.formatNumber(item.ecfamount));
        $("#txtEcfDateView").val(item.ECFDate);
        $("#txtAmortCycleView").val(item.cycle);
        $("#txtAmountStartedOnView").val(item.StartOn);
        $("#txtSupplierCodeView").val(item.suppliercode);
        $("#txtAmortSplitView").val(item.splitmonth);
        $("#txtCompletedMonthView").val(item.completedmonth);
        $("#txtCompletedAmountView").val(item.completed);
        $("#txtSupplierNameView").val(item.suppliername);
        $("#txtBalanceMonthView").val(item.Balancemonth);
        $("#txtBalanceAmountView").val(self.formatNumber(item.balance));
        $("#txtAmortAmountView").val(self.formatNumber(item.amortamount));
        ShowDialog();
    };

    self.Edit = function (item) {

        $("#txtEcfNoEdit").val(item.ECFNO);
        $("#hdnSupplierId").val(item.supplierId);
        $("#txtEcfAmountEdit").val(self.formatNumber(item.ecfamount));
        $("#txtEcfDateEdit").val(item.ECFDate);
        $("#txtAmortCycleEdit").val(item.cycle);
        $("#txtAmountStartedOnEdit").val(item.StartOn);
        $("#txtSupplierCodeEdit").val(item.suppliercode);
        $("#txtAmortSplitEdit").val(item.splitmonth);
        $("#txtCompletedMonthEdit").val(item.completedmonth);
        $("#txtCompletedAmountEdit").val(self.formatNumber(item.completed));
        $("#txtSupplierNameEdit").val(item.suppliername);
        $("#txtBalanceMonthEdit").val(item.Balancemonth);
        $("#txtBalanceAmountEdit").val(self.formatNumber(item.balance));
        $("#txtEditBalance").val(self.formatNumber(item.balance));
        $("#txtAmortAmountEdit").val(self.formatNumber(item.amortamount));
        $("#hdnECFId").val(item.ECFId);
        $("#hdnInvId").val(item.InvId);
        $("#hdnAmortId").val(item.AmortID);

        
        $("#txtEditCurrentMonthAmount").val("0");
        $("#txtEditAmortSplit").val(item.splitmonth);
        self.AmortReScheduleArray.removeAll();

        //$("#ddlAmortCycle").val("0");
        //$("#txtEditAmountStartDate").val("");
        //$("#txtEditAmountEndDate").val("");

        $("#ddlAmortCycle").val(item.Frequencygid);
        $("#txtEditAmountStartDate").val(item.StartDate);
        $("#txtEditAmountEndDate").val(item.EndDate);


        $("#ddlAmortCycle").removeClass('required').addClass('valid');
        $("#txtEditAmountStartDate").removeClass('required').addClass('valid');
        $("#txtEditAmountEndDate").removeClass('required').addClass('valid');

        //$("#txtEditAmountStartDate").addClass('required');
        //$("#ddlAmortCycle").addClass('required');
        //$("#txtEditAmountEndDate").addClass('required');

        $("#btnReschedule").attr("disabled", true);
        $("#btnConfirm").attr("disabled", true);
        ShowEditDialog();


        var AmortDetails = {
            ECFFrom: "",
            ECFTo: "",
            ECFNo: item.ECFNO,
            ECFAmount: 0,
            SupplierCodeId: "",
            SupplierNameId: ""
        };
        $.ajax({
            type: "post",
            url: UrlDet + "AmortDebitLineDetails",
            async: false,
            data: JSON.stringify(AmortDetails),
            contentType: "application/json;",
            success: function (response) {

                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.AmortDebitLineArray(Data2);
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }

                    if (Data1[0].Msg == "" && self.AmortEditDetails().length == 0) {
                        jAlert("No Records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    self.RescheduleView = function () {

        if ($("#txtEditAmortSplit").val().trim() == "" || $("#txtEditAmortSplit").val().trim() == "0") {
            jAlert("Ensure Amort Split!", "Message");
            return false;
        }

        if ($("#txtEditBalance").val().trim() == "" || $("#txtEditBalance").val().trim() == "0") {
            jAlert("Ensure Balance!", "Message");
            return false;
        }

        var Data = {
            InvId: $("#hdnInvId").val(),
            Amount: $("#txtEditBalance").val().replace(/,/g, ''),
            AmortCycle: $("#ddlAmortCycle").val(),
            AmortStartDate: $("#txtEditAmountStartDate").val(),
            AmortEndDate: $("#txtEditAmountEndDate").val(),
            AmortSplit: $("#txtEditAmortSplit").val()
        };

        $.ajax({
            type: "post",
            url: UrlDet + "AmortReScheduleDetails",
            async: false,
            data: JSON.stringify(Data),
            contentType: "application/json;",
            success: function (response) {

                self.AmortReScheduleArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);


                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.AmortReScheduleArray(Data2);
                    $("#btnConfirm").attr("disabled", false);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    self.textChange = function (obj, event) {
        if (event.originalEvent) {
            var total = 0;
            //if value is empty--update text box to 0
            if ($("#" + event.target.id).val().trim() == "") {
                ko.utils.arrayForEach(self.AmortDebitLineArray(), function (item, index) {
                    if (item.DPercent == "" || item.DPercent == null) {
                        item.DPercent = "0";
                    }
                });
                $("#" + event.target.id).val("0");
            }
            //validate the debit line to not exceed 100%
            if ($("#" + event.target.id).val().trim() != "" && $("#" + event.target.id).val().trim() != "0") {
                var items = ko.utils.arrayForEach(self.AmortDebitLineArray(), function (item) {
                    if (item.DPercent != null && item.DPercent != "") {
                        total += parseInt(ko.utils.unwrapObservable(item.DPercent));
                    }
                });
                if (total > 100) {
                    var GId = event.target.id.split('_')[1];
                    ko.utils.arrayForEach(self.AmortDebitLineArray(), function (item, index) {
                        if (GId == item.GId) {
                            item.DPercent = "0";
                        }
                    });
                    $("#" + event.target.id).val("0");
                    jAlert("Overall percentage not exceed 100.", "Message");
                }
            }
        }
    };

    self.Confirm = function () {
        var Data = {
            AmortId: $("#hdnAmortId").val(),
            ECFId: $("#hdnECFId").val(),
            InvId: $("#hdnInvId").val(),
            Amount: $("#txtEditBalance").val().replace(/,/g, ''),
            CurrentAmount: $("#txtEditCurrentMonthAmount").val().replace(/,/g, ''),
            AmortCycle: $("#ddlAmortCycle").val(),
            AmortStartDate: $("#txtEditAmountStartDate").val(),
            AmortEndDate: $("#txtEditAmountEndDate").val(),
            AmortSplit: $("#txtEditAmortSplit").val(),
            SupplierId: $("#hdnSupplierId").val(),
            DebitlinePercent: JSON.stringify(self.AmortDebitLineArray.slice())
        };

        $.ajax({
            type: "post",
            url: UrlDet + "SaveAmortEditDetails",
            async: false,
            data: JSON.stringify(Data),
            contentType: "application/json;",
            success: function (response) {
                var clear = response.Clear;
                if (clear == "True" || clear == "true" || clear == "1") {
                    jAlert(response.MessageText, "Message");
                    self.search();
                    objDialogyEdit.dialog("close");
                } else {
                    jAlert(response.MessageText, "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    self.Clear = function () {
        $("#txtECFFrom").val("");
        $("#txtECFTo").val("");
        $("#txtEcfNumber").val("");
        $("#txtEcfAmount").val("");
        $("#hdfSSupplierId").val("0");
        $("#hdfSSupplierName").val("0");
        $("#txtSupplierCode").val("");
        $("#txtSupplierName").val("");

        $("#gvAmortEditDetails").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.AmortEditDetails.removeAll();
    };

    self.loadGrid = function () {
        $("#gvAmortEditDetails").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.AmortEditDetails.removeAll();
    }
    self.loadGrid();

    self.loadAmortCycle();
}

var mainViewModel = new AmortEditModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    $("#btnReschedule").attr("disabled", true);
    $("#btnConfirm").attr("disabled", true);

    objDialogyEdit = $("[id$='EditDialog']");
    objDialogyEdit.dialog({
        autoOpen: false,
        modal: true,
        width: 850,
        height: 500,
        duration: 300
    });

    objDialogyAdd = $("[id$='ViewDialog']");
    objDialogyAdd.dialog({
        autoOpen: false,
        modal: true,
        width: 850,
        height: 500,
        duration: 300
    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatePicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $(".fsDatePicker").keyup(function (e) {
        if (e.keyCode == 8) {
            $.datepicker._clearDate(this);
        }
        if (e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    $("#txtEditAmountStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtEditAmountEndDate").datepicker("option", "minDate", selected)
            fnValidateIsRequired("txtEditAmountStartDate");
            fnCheckValidation();
        }
    });

    $("#txtEditAmountEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        onSelect: function (selected) {
            $("#txtEditAmountStartDate").datepicker("option", "maxDate", selected)
            fnValidateIsRequired("txtEditAmountEndDate");
            fnCheckValidation();
        }
    });

    $("#txtEditAmountStartDate").keyup(function (e) {
        if (e.keyCode == 8) {
            $.datepicker._clearDate(this);
        }
        if (e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    $("#txtEditAmountEndDate").keyup(function (e) {
        if (e.keyCode == 8) {
            $.datepicker._clearDate(this);
        }
        if (e.keyCode == 46) {
            $.datepicker._clearDate(this);
        }
        $(this).datepicker('show');
    });

    $("#txtSupplierCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSSupplierId").val("0");
        }

        $("#txtSupplierCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteSupplierCode",
                    data: "{ 'txt' : '" + $("#txtSupplierCode").val() + "'}",
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
                        //alert(response.responseText);
                    },
                    failure: function (response) {
                        //alert(response.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (e, i) {
                $("#hdfSSupplierId").val(i.item.val);
                $("#txtSupplierCode").val(i.item.label);
            }
        });
    });

    $("#txtSupplierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSSupplierName").val("0");
        }

        $("#txtSupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteSupplier",
                    data: "{ 'txt' : '" + $("#txtSupplierName").val() + "'}",
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
                        //alert(response.responseText);
                    },
                    failure: function (response) {
                        //alert(response.responseText);
                    }
                });
            },
            minLength: 1,
            select: function (e, i) {
                $("#hdfSSupplierName").val(i.item.val);
                $("#txtSupplierName").val(i.item.label);
            }
        });
    });

    $("#ddlAmortCycle").change(function () {
        fnValidateIsRequired("ddlAmortCycle");
        fnCheckValidation();
    });

    $("#txtEcfAmount,#txtEditCurrentMonthAmount,#txtEditBalance").keyup(function (event) {
        if (event.which >= 37 && event.which <= 40) {
            event.preventDefault();
        }

        var currentval = $(this).val();
        var testDecimal;
        currentval.match(/\./g) === null ? testDecimal = 0 : testDecimal = currentval.match(/\./g);
        if (testDecimal.length > 1) {
            currentval = currentval.slice(0, -1);
        }

        var components = currentval.toString().split(".");
        if (components.length === 1)
            components[0] = currentval;
        components[0] = components[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        if (components.length === 2)
            components[1] = components[1].replace(/\D/g, "");
        $(this).val(components.join("."));
    });

    /* $("#txtEditAmountStartDate").change(function () {
         fnValidateIsRequired("txtEditAmountStartDate");
         fnCheckValidation();
     });
 
     $("#txtEditAmountEndDate").change(function () {
         fnValidateIsRequired("txtEditAmountEndDate");
         fnCheckValidation();
     });*/

    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "date-uk-pre": function (a) {
            a = a.split(">")[1].split("<")[0];
            var ukDatea = a.split("/");
            return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
        },
        "date-uk-asc": function (a, b) {
            return ((a < b) ? -1 : ((a > b) ? 1 : 0));
        },
        "date-uk-desc": function (a, b) {
            return ((a < b) ? 1 : ((a > b) ? -1 : 0));
        }
    });
});

function fnCheckValidation() {
    var CurrrentAmount = $("#txtEditCurrentMonthAmount").val();
    var AmortCycle = $("#ddlAmortCycle").val();
    var AmortStartDate = $("#txtEditAmountStartDate").val();
    var AmortEndDate = $("#txtEditAmountEndDate").val();
    if (AmortCycle != "" && AmortStartDate != "" && AmortEndDate != "") {
        $("#btnReschedule").attr("disabled", false);

        var Data = {
            startdate: $("#txtEditAmountStartDate").val(),
            enddate: $("#txtEditAmountEndDate").val(),
            frequencygid: $("#ddlAmortCycle").val()
        };

        $.ajax({
            type: "post",
            url: UrlDet + "AmortSplitDetails",
            async: false,
            data: JSON.stringify(Data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                        $("#btnReschedule").attr("disabled", true);
                    }
                    $("#txtEditAmortSplit").val(Data1[0].Split);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    }
    else
        $("#btnReschedule").attr("disabled", true);
};

function ShowDialog() {
    $('#ViewDialog').attr("style", "display:block;");
    objDialogyAdd.dialog({ title: 'Amort Details', width: '1000', height: '350' });
    objDialogyAdd.dialog("open");
    return false;
}

function ShowEditDialog() {
    $('#EditDialog').attr("style", "display:block;");
    objDialogyEdit.dialog({ title: 'Amort Details', width: '1000', height: '750' });
    objDialogyEdit.dialog("open");
    return false;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

