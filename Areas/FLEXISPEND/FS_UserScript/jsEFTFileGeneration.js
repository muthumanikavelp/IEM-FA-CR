var objDialogyCancel;

UrlDet = UrlDet.replace("DownloadFile", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var ChequeInventoryModel = function () {
    var self = this;

    self.PayMode = ko.observable();
    //bank dropdown array
    self.BankArray = ko.observableArray();
    self.ClaimTypeArray = ko.observableArray();
    self.PaymentModeArray = ko.observableArray();

    //load grid array
    self.PaymentAdviceArray = ko.observableArray();

    self.loadClaimType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadClaimTypeWA",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.ClaimTypeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadPaymentMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadEFTPayMode",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.PaymentModeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadBank = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadBankDropDown",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.BankArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.ClaimTypeSelectChanged = function (obj, event) {
        if (event.originalEvent) {
            $("#txtEmpCode").removeAttr("disabled").removeAttr("style");
            $("#txtEmpName").removeAttr("disabled").removeAttr("style");
            $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
            $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

            $("#hdfEmpCodeId").val("0");
            $("#txtEmpCode").val("");
            $("#hdfEmpName").val("0");
            $("#txtEmpName").val("");
            $("#hdfSupplierId").val("0");
            $("#txtSupplierCode").val("");
            $("#hdfSupplierName").val("0");
            $("#txtSupplierName").val("");

            var chkbit = $("#ddlClaimType").val();
            if (chkbit == "E") {
                $("#txtSupplierCode").attr("disabled", "disabled");
                $("#txtSupplierName").attr("disabled", "disabled");
                $("#txtSupplierCode").attr("style", "background-color: gainsboro;");
                $("#txtSupplierName").attr("style", "background-color: gainsboro;");
            }

            if (chkbit == "S") {
                $("#txtEmpCode").attr("disabled", "disabled");
                $("#txtEmpName").attr("disabled", "disabled");
                $("#txtEmpCode").attr("style", "background-color: gainsboro;");
                $("#txtEmpName").attr("style", "background-color: gainsboro;");
            }
        } else {
        }
    }

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

    //self.chkBit = function (item) {
    //    var Memo = "MEMO";
    //    var Online = "ON";
    //    if (item.indexOf(Online) != -1) {
    //        return 1;
    //    } else if (item.indexOf(Memo) != -1) {
    //        return 2;
    //    } else {
    //        return 0;
    //    }
    //};

    self.Search = function () {
        var txtPVAmtFrom = $("#txtPVAmountFrom").val().replace(/,/g, '');
        var txtPVAmtTo = $("#txtPVAmountTo").val().replace(/,/g, '');
        var PayBankGId = $("#ddlBank").val();

        if (((txtPVAmtFrom == "" || txtPVAmtFrom == "0") && (txtPVAmtTo != "" && txtPVAmtTo != "0"))
            || ((txtPVAmtFrom != "" && txtPVAmtFrom != "0") && (txtPVAmtTo == "" || txtPVAmtTo == "0"))) {
            jAlert("Ensure PV Amount From/To!", "Message");
            return false;
        }
        if (txtPVAmtFrom != "" && txtPVAmtTo != "" && txtPVAmtFrom != "0" && txtPVAmtTo != "0") {
            if (parseFloat(txtPVAmtTo) < parseFloat(txtPVAmtFrom)) {
                jAlert("Ensure Valid PV Amount Range!", "Message");
                return false;
            }
        }
        if ((PayBankGId == "" || PayBankGId == "0") && $("#ddlPaymode option:selected").text() != 'RRP') {
            jAlert("Ensure Bank Name!", "Message");
            return false;
        }

        var data = {
            PvDateFrom: $("#txtPVFrom").val(),
            PvDateTo: $("#txtPVTo").val(),
            PvNo: $("#txtPVNo").val(),
            PvAmountFrom: txtPVAmtFrom,
            PvAmountTo: txtPVAmtTo,
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            ClaimType: $("#ddlClaimType").val(),
            PayMode: $("#ddlPaymode").val(),
            BankId: $("#ddlBank").val(),
            BatchNo: $("#txtPayBatchNo").val(),
            MemoNo: $("#txtMemoNo").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchEFTMemoDetails",
            data: JSON.stringify(data),
            async: false,
            contentType: "application/json;",
            success: function (response) {

                //$("#gvPaymentAdvice").DataTable({
                //    "autoWidth": false,
                //    "destroy": true
                //}).destroy();
                //self.PaymentAdviceArray.removeAll();
                self.PayMode($("#ddlPaymode").val());
                self.loadGrid();

                var Data1 = "", Data2 = "";
                $("#chkHSelete").prop('checked', false);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.PaymentAdviceArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    } else {
                        if (self.PaymentAdviceArray().length == 0) {
                            jAlert("Sorry no records found!", "Message");
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                self.PayMode($("#ddlPaymode").val());
            }
        });
    };

    self.Load = function () {
        debugger;
        var txtPVAmtFrom = $("#txtPVAmountFrom").val().replace(/,/g, '');
        var txtPVAmtTo = $("#txtPVAmountTo").val().replace(/,/g, '');
        var PayBankGId = $("#ddlBank").val();

        if (((txtPVAmtFrom == "" || txtPVAmtFrom == "0") && (txtPVAmtTo != "" && txtPVAmtTo != "0"))
            || ((txtPVAmtFrom != "" && txtPVAmtFrom != "0") && (txtPVAmtTo == "" || txtPVAmtTo == "0"))) {
            jAlert("Ensure PV Amount From/To!", "Message");
            return false;
        }
        if (txtPVAmtFrom != "" && txtPVAmtTo != "" && txtPVAmtFrom != "0" && txtPVAmtTo != "0") {
            if (parseFloat(txtPVAmtTo) < parseFloat(txtPVAmtFrom)) {
                jAlert("Ensure Valid PV Amount Range!", "Message");
                return false;
            }
        }
        if ((PayBankGId == "" || PayBankGId == "0") && $("#ddlPaymode option:selected").text() != 'RRP') {
            jAlert("Ensure Bank Name!", "Message");
            return false;
        }

        var data = {
            PvDateFrom: $("#txtPVFrom").val(),
            PvDateTo: $("#txtPVTo").val(),
            PvNo: $("#txtPVNo").val(),
            PvAmountFrom: txtPVAmtFrom,
            PvAmountTo: txtPVAmtTo,
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            ClaimType: $("#ddlClaimType").val(),
            PayMode: $("#ddlPaymode").val(),
            BankId: $("#ddlBank").val(),
            BatchNo: $("#txtPayBatchNo").val(),
            MemoNo: $("#txtMemoNo").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchEFTMemoDetails",
            data: JSON.stringify(data),
            async: false,
            contentType: "application/json;",
            success: function (response) {

                //$("#gvPaymentAdvice").DataTable({
                //    "autoWidth": false,
                //    "destroy": true
                //}).destroy();
                //self.PaymentAdviceArray.removeAll();
                self.PayMode($("#ddlPaymode").val());
                self.loadGrid();

                var Data1 = "", Data2 = "";
                $("#chkHSelete").prop('checked', false);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.PaymentAdviceArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        //jAlert(Data1[0].Message, "Message");
                    } else {
                        if (self.PaymentAdviceArray().length == 0) {
                           // jAlert("Sorry no records found!", "Message");
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                self.PayMode($("#ddlPaymode").val());
            }
        });
    };

    self.Memo = function () {
        var Option1 = "0";
        var array = new Array();
        $('#ContentECF input:checkbox:checked:visible').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _chkPrintDetails = "";
        $.each(array, function (index) {
            _chkPrintDetails += array[index].split('_')[1] + '|';
        });

        if (_chkPrintDetails.trim() == "") {
            jAlert("Please select a record!", "Message");
            return false;
        }

        jConfirm("Do you want to Generate Memo ?", "Message", function (callback) {
            if (callback == true) {
                self.SetMemo(_chkPrintDetails, Option1);
            } else {
                return false;
            }
        });
    };

    self.SetMemo = function (_chkPrintDetails, Option1) {
        var PayBankGId = $("#ddlBank").val();
        var data = {
            PvIds: _chkPrintDetails,
            Status: "Memo",
            CancelPvId: "0",
            Reason: "",
            PayMode: $("#ddlPaymode").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetEFTMemoDetails",
            data: JSON.stringify(data),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "" && (Data1[0].Clear == "false" || Data1[0].Clear == "False" || Data1[0].Clear == "0")) {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                var data = {
                                    Id: $("#ddlPaymode").val(),
                                    subId: "1",
                                    PayBankGId: PayBankGId
                                };

                                $.ajax({
                                    type: "post",
                                    url: UrlDet + "GenerateDocuments",
                                    async: false,
                                    data: JSON.stringify(data),
                                    contentType: "application/json;",
                                    success: function (response) {
                                        self.Load();

                                    },
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        //alert(errorThrown);
                                        self.Load();
                                    }
                                });


                                //var _printFmt = Option1 == "0" ? 'RTGS' : 'NEFT';
                                // window.open(UrlDet +'Print/' + _printFmt, "PopUpWindows", 'scrollbars=yes, width=900px, height=800px, top=100,left=100');
                                //self.Search();
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Online = function () {
        //download notepad file
        var array = new Array();
        $('#ContentECF input:checkbox:checked:visible').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _chkPrintDetails = "";
        $.each(array, function (index) {
            _chkPrintDetails += array[index].split('_')[1] + '|';
        });

        if (_chkPrintDetails.trim() == "") {
            jAlert("Please select a record!", "Message");
            return false;
        }

        var data = {
            PvIds: _chkPrintDetails,
            Status: "Online",
            CancelPvId: "0",
            Reason: "",
            PayMode: $("#ddlPaymode").val()
        };

        $.ajax({
            type: "post",
            url: UrlDet + "SetEFTMemoDetails",
            data: JSON.stringify(data),
            contentType: "application/json;",
            async: false,
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "" && (Data1[0].Clear == "false" || Data1[0].Clear == "False" || Data1[0].Clear == "0")) {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                // ko.utils.postJson(UrlDet + "DownloadFile");

                                var data = {
                                    Id: $("#ddlPaymode").val(),
                                    subId: "0"
                                };

                                $.ajax({
                                    type: "post",
                                    url: UrlDet + "GenerateDocuments",
                                    async: false,
                                    data: JSON.stringify(data),
                                    contentType: "application/json;",
                                    success: function (response) {
                                        self.Load();

                                    },
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        //alert(errorThrown);
                                        self.Load();
                                    }
                                });
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.OpenCancel = function (item) {
        $("#lblPVId").val(item.PvId);

        $("#lblMemoNo").text(item.MemoNo);
        $("#lblPVNo").text(item.PVNo);
        $("#lblPVDate").text(item.PVDate);
        $("#lblBankDet").text(item.Bankname);
        $("#lblClaimType").text(item.ClaimType);
        $("#lblSEDetails").text(item.EmployeeSupplierCode + '-' + item.EmployeeSupplierName);

        $("#txtReason").val("");

        $("#txtReason").removeClass('required').removeClass('valid');
        $("#txtReason").addClass('required');

        MemoCancelEntry();
        if (item.MemoNo != null && item.MemoNo != "") {
            $('#ShowDialogCancel').attr("style", "display:block;");
            objDialogyCancel.dialog({ title: 'Cancel Memo' });
            objDialogyCancel.dialog("open");
        } else {
            jAlert("Memo Number not allocated!", "Message");
        }
    };

    self.CancelMemo = function () {
        var data = {
            PvIds: "",
            Status: "",
            CancelPvId: $("#lblPVId").val(),
            Reason: $("#txtReason").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetEFTMemoDetails",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Message != "") {
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {

                                    $('#ShowDialogCancel').attr("style", "display:none;");
                                    objDialogyCancel.dialog("close");
                                    self.Search();
                                }
                            }
                        });
                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Print = function (item) {
        var data = {
            PvIds: item.PvId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "PrintExisting",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response == "OK") {
                    jConfirm("", "Which Format ?", function (callback) {
                        if (callback == true) {
                            window.open(UrlDet + 'Print/RTGS', "PopUpWindows", 'scrollbars=yes, width=900px, height=800px, top=100,left=100');
                        } else {
                            window.open(UrlDet + 'Print/NEFT', "PopUpWindows", 'scrollbars=yes, width=900px, height=800px, top=100,left=100');
                        }
                    });

                    //remove the cancel button from dialog box
                    $("#popup_ok").attr("style", "padding-top: 5px; padding-bottom: 5px; padding-left: 10px; padding-right: 10px;");
                    $("#popup_ok").attr("value", "RTGS");
                    $("#popup_cancel").attr("style", "padding-top: 5px; padding-bottom: 5px; padding-left: 10px; padding-right: 10px;");
                    $("#popup_cancel").attr("value", "NEFT");
                }
                else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DownloadOnline = function (item) {
        var data = {
            PvIds: item.PvId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "PrintExisting",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response == "OK") {
                    ko.utils.postJson(UrlDet + "DownloadFile");
                }
                else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.checkChange = function (obj, event) {
        if (event.originalEvent) {
            var array = new Array();
            $('#ContentECF input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });

            var arrayTmp = new Array();
            $('#ContentECF input:checkbox:visible').each(function (index) {
                arrayTmp[index] = $(this).attr('id');
            });

            var chkBit = array.length == arrayTmp.length ? true : false;
            $("#chkHSelete").prop('checked', chkBit);
        }
    };

    self.clearFilter = function () {
        $("#txtPVFrom").val("");
        $("#txtPVTo").val("");
        $("#txtPVNo").val("");
        $("#txtPVAmountFrom").val("");
        $("#txtPVAmountTo").val("");
        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");
        $("#hdfSupplierId").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");
        $("#ddlClaimType").val("");
        $("#ddlBank").val("");
        $("#txtPayBatchNo").val("");
        $("#txtMemoNo").val("");

        $("#ddlPaymode").val("");

        $("#txtEmpCode").removeAttr("disabled").removeAttr("style");
        $("#txtEmpName").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

        var _data = $("#ddlBank").val();
        if (_data.trim() != "" && _data.trim() != "0") {
            $("#ddlBank").removeClass('required');
            $("#ddlBank").addClass('valid');
        }
        else {
            $("#ddlBank").removeClass('valid');
            $("#ddlBank").addClass('required');
        }

        //$("#gvPaymentAdvice").DataTable({
        //    "autoWidth": false,
        //    "destroy": true
        //}).destroy();
        //self.PaymentAdviceArray.removeAll();

        self.loadGrid();
    };

    self.DatatableCall = function () {
        if ($("#gvPaymentAdvice > tbody > tr").length == self.PaymentAdviceArray().length) {
            $("#gvPaymentAdvice").DataTable({
                "autoWidth": false,
                "destroy": true,
                "bFilter": false,
                "bLengthChange": false,
                "bPaginate": false,
                "bInfo": false,
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

    self.loadGrid = function () {
        $("#gvPaymentAdvice").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.PaymentAdviceArray.removeAll();
    }

    self.loadGrid();

    //load the Bank DropDown.
    self.loadBank();
    self.loadClaimType();
    self.loadPaymentMode();
};

var mainViewModel = new ChequeInventoryModel();
ko.applyBindings(mainViewModel);


$(document).ready(function () {
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

    $("#btnPrint").attr("style", "display:none");
    $("#btnMail").attr("style", "display:none");

    objDialogyCancel = $("[id$='ShowDialogCancel']");
    objDialogyCancel.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 350,
        duration: 300
    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsdatepicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    //Load Employee Code Auto Complete
    $("#txtEmpCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfEmpCodeId").val("0");
        }

        $("#txtEmpCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtEmpCode").val() + "'}",
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
                $("#hdfEmpCodeId").val(i.item.val);
                $("#txtEmpCode").val(i.item.label);
            }
        });
    });

    $("#txtEmpCode").focusout(function () {
        var hdfId = $("#hdfEmpCodeId").val();
        var txtCurName = $("#txtEmpCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEmpCode").val("");
        }
    });

    //Load txtSSupplier Code Auto Complete
    $("#txtSupplierCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSupplierId").val("0");
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
                $("#hdfSupplierId").val(i.item.val);
                $("#txtSupplierCode").val(i.item.label);
            }
        });
    });

    $("#txtSupplierCode").focusout(function () {
        var hdfId = $("#hdfSupplierId").val();
        var txtCurName = $("#txtSupplierCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplierCode").val("");
        }
    });

    //Load Employee Name Auto Complete
    $("#txtEmpName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfEmpName").val("0");
        }

        $("#txtEmpName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtEmpName").val() + "'}",
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
                $("#hdfEmpName").val(i.item.val);
                $("#txtEmpName").val(i.item.label);
            }
        });
    });

    $("#txtEmpName").focusout(function () {
        var hdfId = $("#hdfEmpName").val();
        var txtCurName = $("#txtEmpName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEmpName").val("");
        }
    });

    //Load Supplier Name Auto Complete
    $("#txtSupplierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSupplierName").val("0");
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
                $("#hdfSupplierName").val(i.item.val);
                $("#txtSupplierName").val(i.item.label);
            }
        });
    });

    $("#txtSupplierName").focusout(function () {
        var hdfId = $("#hdfSupplierName").val();
        var txtCurName = $("#txtSupplierName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSupplierName").val("");
        }
    });

    $("#txtReason").keyup(function () {
        MemoCancelEntry();

        var _data = $("#txtReason").val();
        if (_data.trim() != "") {
            $("#txtReason").removeClass('required');
            $("#txtReason").addClass('valid');
        }
        else {
            $("#txtReason").removeClass('valid');
            $("#txtReason").addClass('required');
        }
    });

    $("#ddlBank").change(function () {
        var _data = $("#ddlBank").val();
        if (_data.trim() != "" && _data.trim() != "0") {
            $("#ddlBank").removeClass('required');
            $("#ddlBank").addClass('valid');
        }
        else {
            $("#ddlBank").removeClass('valid');
            $("#ddlBank").addClass('required');
        }
    });

    $("#chkHSelete").click(function (e) {
        $(this).closest('table').find('td input:checkbox:visible').prop('checked', this.checked);
    });

    $("#txtPVAmountFrom,#txtPVAmountTo").keyup(function (event) {
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
});

function MemoCancelEntry() {
    var _reason = $("#txtReason").val();

    if (_reason.trim() == "") {
        $('#btnCancel').attr('disabled', true);
    } else {
        $('#btnCancel').attr('disabled', false);
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isNumberAndLetterAndSpace(evt) {
    var key = evt.keyCode;
    return ((key >= 65 && key <= 90) || key == 8 || (key >= 97 && key <= 122) || (key >= 48 && key <= 57) || key == 127 || key == 32);
}

function isEvent(evt) {
    return false;
}