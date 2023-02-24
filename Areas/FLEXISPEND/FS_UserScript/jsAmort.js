var objDialogyAdd;

UrlDet = UrlDet.replace("AmortRunSearchDetails", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var AmortModel = function () {
    var self = this;
    
    self.SlNo = ko.observable("");
    self.ECFId = ko.observable("");
    self.amortgid = ko.observable("");
    self.ECFDate = ko.observable("");
    self.ECFNO = ko.observable("");
    self.suppliercode = ko.observable("");
    self.suppliername = ko.observable("");
    self.ecfamount = ko.observable("");
    self.completed = ko.observable("");
    self.balance = ko.observable("");
    self.cycle = ko.observable("");
    self.splitmonth = ko.observable("");
    self.Amortmonth = ko.observable("");
    self.AmortAmount = ko.observable("");
    self.NatureOfExpense = ko.observable("");
    self.ExpenseCategory = ko.observable("");
    self.SubCategory = ko.observable("");
    self.GLCode = ko.observable("");
    self.FC = ko.observable("");
    self.CC = ko.observable("");
    self.ProductCode = ko.observable("");
    self.OUCode = ko.observable("");
    self.Amount = ko.observable("");
   // self.canSubmit = ko.observable(false);

    self.StatusUpdate = ko.observable();
    self.StatusUpdateDetails = ko.observableArray();

    var AmortDetails = {
        ECFId: self.ECFId,
        amortgid: self.amortgid,
        ECFDate: self.ECFDate,
        ECFNO: self.ECFNo,
        suppliercode: self.suppliercode,
        suppliername: self.suppliername,
        ecfamount: self.ecfamount,
        completed: self.completed,
        balance: self.balance,
        cycle: self.cycle,
        splitmonth: self.splitmonth,
        Amortmonth: self.Amortmonth,
        AmortAmount: self.AmortAmount
    };

    self.AmortDebitLine = ko.observable();
    self.AmortDebitLineDetails = ko.observableArray();

    self.AmortScheduleView = ko.observable();
    self.AmortScheduleViewDetails = ko.observableArray();

    self.Amort = ko.observable();
    self.AmortDetails = ko.observableArray();


    self.checkChange = function (obj, event) {

        
        if (event.originalEvent) {
            var array = new Array();
            $('#Contentamortrun input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });

            var chkBit = array.length == self.AmortDetails().length ? true : false;
            //alert(chkBit);
            $("#chkHSelete").prop('checked', chkBit);
        }
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

    self.Search = function () {
       // $("#btnRunAmort").prop("disabled", false);
        var AmortRun = {
            Month: $("#txtMonth").val(),
            SupplierCodeId: $("#hdfSSupplierId").val(),
            SupplierNameId: $("#hdfSSupplierName").val(),
            AmortAmount: $("#txtAmortAmount").val().replace(/,/g, ''),
            ECFFrom: $("#txtEcfFrom").val(),
            ECFTo: $("#txtEcfTo").val(),
            ECFNumber: $("#txtEcfNumber").val(),
            ECFAmount: $("#txtEcfAmount").val().replace(/,/g, '')
        };
        $.ajax({
            type: "post",
            url: UrlDet + "AmortRunSearchDetails",
            async: false,
            data: JSON.stringify(AmortRun),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();

                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Msg != "") {
                        jAlert(Data1[0].Msg, "Message");
                    }

                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        self.AmortDetails(Data2);
                    }

                    if (Data1[0].Msg == "" && self.AmortDetails().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    self.DatatableCall = function () {
        if ($("#gvAmortDetails > tbody > tr").length == self.AmortDetails().length) {
            $("#gvAmortDetails").DataTable({
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
        $("#txtAmortAmountView").val(self.formatNumber(item.AmortAmount));
        $("#txtAmortCycleView").val(item.cycle);
        
        $("#txtSupplierCodeView").val(item.suppliercode);
        $("#txtAmortSplitView").val(item.splitmonth);
        $("#txtBalanceAmountView").val(self.formatNumber(item.balance));
        $("#txtCompletedAmountView").val(self.formatNumber(item.completed));
        $("#txtSupplierNameView").val(item.suppliername);

        $("#txtCompletedMonthView").val(item.completedmonth);
        $("#txtBalanceMonthView").val(item.Balancemonth);
        $("#txtAmountStartedOnView").val(item.StartOn);
        ShowDialog();
    };

    self.Clear = function () {
        $("#txtMonth").val("");
        $("#txtSupplierCode").val("");
        $("#txtSupplierName").val("");
        $("#txtAmortAmount").val("");
        $("#txtEcfFrom").val("");
        $("#txtEcfTo").val("");
        $("#txtEcfNumber").val("");
        $("#txtEcfAmount").val("");
        
        $("#btnsearch").attr("disabled", true);
        $("#txtMonth").removeClass('valid');
        $("#txtMonth").addClass('required');

        $("#chkHSelete").prop('checked', false);//Muthu
        self.loadGrid();
    };

    self.runAmort = function () {
       // debugger
        $("#btnRunAmort").attr("disabled", "disabled");

      //  showProgress();
        var array = new Array();
        $('#gvAmortDetails input:checkbox:checked').each(function (index) {
            if ($(this).attr('id') != 'chkHSelete' ) {//Muthu if added
                array[index] = $(this).attr('id');
            }
        });

        var _amortdata = "";
        $.each(array, function (index) {
            //alert(array[0]);
           // alert(array[index]);
            var a = undefined;
            var b = array[index];
            if (b != a) {
            _amortdata += array[index] + ',';
        }
        });
        
        var Data = {
            Month: $("#txtMonth").val(),
            AmortData: _amortdata
        };



       $.ajax({
            type: "post",
            url: UrlDet + "SaveAmortRunDetails",
            async: false,
            data: JSON.stringify(Data),
            contentType: "application/json;",
            success: function (response) {
                var clear = response.Clear;
              //  $("#btnRunAmort").prop("disabled", false);
                if (clear == "True" || clear == "true" || clear == "1") {
                    
                    jAlert(response.MessageText, "Message");
                    objDialogyAdd.dialog("close");
                    self.Search();
                   // hideProgress();
                  //  self.canSubmit(true);
                  // $("#btnRunAmort").removeAttr('disabled');
                } else {
                    // hideProgress();
                   // $("#btnRunAmort").prop("disabled", false);
                    jAlert(response.MessageText, "Message");
                    
                   // self.canSubmit(true);
                  //  $("#btnRunAmort").removeAttr('disabled');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
              //  $("#btnRunAmort").prop("disabled", false);
               // $("#btnRunAmort").removeAttr('disabled');
               // alert("errorThrown");
            }
        });
       
    };

    self.loadGrid = function () {
        $("#gvAmortDetails").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.AmortDetails.removeAll();
    }
    self.loadGrid();
}

var mainViewModel = new AmortModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    $("#btnsearch").attr("disabled", true);
    $('#txtMonth').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        maxDate: '-id',
        dateFormat: 'MM-yy'
    }).focus(function () {
        var thisCalendar = $(this);
        $('.ui-datepicker-calendar').detach();
        $('.ui-datepicker-header').css("width", "250px");
        $('.ui-datepicker-close').click(function () {
            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            thisCalendar.datepicker('setDate', new Date(year, month, 1));
            fnValidateIsRequired("txtMonth");
            fnCheckValidation();
        });

        $('#txtMonth').keyup(function (e) {
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

        $("#txtMonth").focusout(function () {
            fnValidateIsRequired("txtMonth");
            fnCheckValidation();
        });
    });

    $(document).ready(function () {
        objDialogyAdd = $("[id$='ViewDialog']");
        objDialogyAdd.dialog({
            autoOpen: false,
            modal: true,
            width: 850,
            height: 500,
            duration: 300
        });

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

    $("#txtEcfAmount,#txtAmortAmount").keyup(function (event) {
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


    //Muthu
    $("#chkHSelete").click(function (e) {

        $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);



    });

});

function fnCheckValidation() {
    var Month = $("#txtMonth").val();
    if (Month != "") {
        $("#btnsearch").attr("disabled", false);
    }
    else
        $("#btnsearch").attr("disabled", true);
}

function ShowDialog() {
    $('#ViewDialog').attr("style", "display:block;");
    objDialogyAdd.dialog({ title: 'Amort Details', width: '1000', height: '350' });
    objDialogyAdd.dialog("open");
    return false;
}

