var objDialogyShow;
var objDialog;
var objDedupDet;

UrlDet = UrlDet.replace("FetchPaymentRun", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var PaymentRunModel = function () {

    var chkBit = "0";
    var self = this;
    self.PaymentRunArray = ko.observableArray();
    self.DocumentTypeArray = ko.observableArray();

    //PopUp content
    self.PaymentProcessArray = ko.observableArray();
    self.PaymentVoucherArray = ko.observableArray();

    //dedup details
    self.DedupDetailsArray = ko.observableArray();

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

    self.loadDocType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "MasterDocumentType",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.DocumentTypeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    //search filter
    self.search = function () {
        var data = {
            Priorit: $("#ddlPriorityType").val(),
            DocNo: $("#txtDocNo").val(),
            DocFrom: $("#txtDocFrom").val(),
            DocTo: $("#txtDocTo").val(),
            SuppCodeId: $("#hdfSupplierCode").val(),
            SuppNameId: $("#hdfSupplierName").val(),
            RaiserCodeId: $("#hdfRaiserCode").val(),
            RaiserNameId: $("#hdfRaiserName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchPaymentRun",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {

                $("#gvPaymentRun").DataTable({
                    "autoWidth": false,
                    "destroy": true,
                    "bFilter": false,
                    "bLengthChange": false,
                    "bPaginate": false,
                    "bInfo": false
                }).destroy();
                self.PaymentRunArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Message.length > 0) {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.PaymentRunArray(Data2);
                }

                if (chkBit == "0" && response.Data1 != null && response.Data1 != "" && Data1[0].Message.length == 0 && self.PaymentRunArray().length == 0) {
                    jAlert("Sorry no records found!", "Message");
                }
                chkBit = "0";
                //$('#btnGeneratePayment').attr('disabled', false);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.checkChange = function (obj, event) {
        if (event.originalEvent) {
            var array = new Array();
            $('#ContentPaymentRun input:checkbox:checked').each(function (index) {
                array[index] = $(this).attr('id');
            });
            $("#lblTotalCount").text(array.length);

            var chkBit = array.length == self.PaymentRunArray().length ? true : false;
            $("#chkHSelete").prop('checked', chkBit);

        }
    };

    //Authorization Reverser Functions
    self.ShowAuthorizationReverse = function (item) {
        $("#hdfEcfId").val(item.ecfId);
        $("#lblDocType").text(item.DocType);
        $("#lblDocNo").text(item.DocNo);
        $("#lblDocAmount").text(self.formatNumber(item.DocAmount));
        $("#lblType").text(item.ClaimType);
        $("#lblName").text(item.Name);
        $("#txtReason").val("");

        $("#txtReason").removeClass('required').removeClass('valid');
        $("#txtReason").addClass('required');

        $("#hdfCheckShowing").val("0");
        
        ShowAuthorizer();
    };

    self.ShowAuthorizationReverseWithGL = function (item) {
        $("#hdfEcfId").val(item.ecfId);
        $("#lblDocType").text(item.DocType);
        $("#lblDocNo").text(item.DocNo);
        $("#lblDocAmount").text(self.formatNumber(item.DocAmount));
        $("#lblType").text(item.ClaimType);
        $("#lblName").text(item.Name);
        $("#txtReason").val("");

        $("#txtReason").removeClass('required').removeClass('valid');
        $("#txtReason").addClass('required');

        $("#hdfCheckShowing").val("1");

        ShowAuthorizer();
    }

    self.AuthorizationReverse = function () {
        var Reason = $("#txtReason").val();
        if (Reason.trim() == "") {
            jAlert("Ensure Reason!", "Message");
            return false;
        }
        var data = {
            EcfId: $("#hdfEcfId").val(),
            Reason: Reason
        };
        $.ajax({
            type: "post",
            url: UrlDet + "PaymentAuthorizationReverse",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == false || Data1[0].Clear == "false" || Data1[0].Clear == "0" || Data1[0].Clear == "False") {
                        jAlert(Data1[0].Message, "Message");
                    }
                    else {
                        //jAlert(Data1[0].Message, "Message");
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                self.search();

                                //close the Authorize dialog box.
                                objDialog.dialog("close");
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.AuthorizationReverseWithGL = function () {
        var Reason = $("#txtReason").val();
        if (Reason.trim() == "") {
            jAlert("Ensure Reason!", "Message");
            return false;
        }
        var data = {
            EcfId: $("#hdfEcfId").val(),
            Reason: Reason
        };
        $.ajax({
            type: "post",
            url: UrlDet + "PaymentAuthorizationReverseWithGL",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    
                    if (Data1[0].Clear == false || Data1[0].Clear == "false" || Data1[0].Clear == "0" || Data1[0].Clear == "False") {
                        jAlert(Data1[0].Message, "Message");
                    }
                    else {
                        //jAlert(Data1[0].Message, "Message");
                        
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {
                                self.search();
                                //close the Authorize dialog box.
                                objDialog.dialog("close");
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    //Rundedup Functions
    self.RunDedup = function () {
        var array = new Array();
        $('#ContentPaymentRun input:checkbox:checked').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _ListParam = "";
        $.each(array, function (index) {
            _ListParam += array[index].split('_')[1] + '|';
        });

        if (_ListParam == "") {
            jAlert("Please select a records to process.", "Message");
            return false;
        }

        var data = {
            ecfId: _ListParam
        };
        $.ajax({
            type: "post",
            url: UrlDet + "PaymentRedupRun",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.DedupDetailsArray.removeAll();
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == false || Data1[0].Clear == "false" || Data1[0].Clear == "0" || Data1[0].Clear == "False") {
                        jAlert(Data1[0].Message, "Message");
                        //$('#btnGeneratePayment').attr('disabled', false);

                        ShowDedupData();

                        if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                            Data2 = JSON.parse(response.Data2);
                            self.DedupDetailsArray(Data2);
                        }
                    }
                    else {
                        jAlert(Data1[0].Message, "Message");
                        //$('#btnGeneratePayment').attr('disabled', false);
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.DedupDownload = function () {
        var array = new Array();
        $('#ContentPaymentRun input:checkbox:checked').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _ListParam = "";
        $.each(array, function (index) {
            _ListParam += array[index].split('_')[1] + '|';
        });

        if (_ListParam == "") {
            jAlert("Please select a records to process.", "Message");
            return false;
        }

        var data = {
            ecfId: _ListParam
        };
        $.ajax({
            type: "post",
            url: UrlDet + "tmpDedupKeep",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                if (response.msg != null && response.msg == "") {
                    ko.utils.postJson(UrlDet + "DownloadDedupData");
                } else {
                    if (response.msg != null && response.msg != "") {
                        jAlert(response.msg, "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    //Payment Generation
    self.GeneratePayment = function () {
        var array = new Array();
        $('#ContentPaymentRun input:checkbox:checked').each(function (index) {
            array[index] = $(this).attr('id');
        });

        var _ListParam = "";
        $.each(array, function (index) {
            _ListParam += array[index].split('_')[1] + '|';
        });

        if (_ListParam == "") {
            jAlert("Please select a records to process.", "Message");
            return false;
        }

        jConfirm("Do you like to Run Payment?", "Message", function (callback) {
            if (callback == true) {
                showProgress();
                var data = {
                    ecfId: _ListParam
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetPaymentRun",
                    data: JSON.stringify(data),
                    contentType: "application/json;",
                    success: function (response) {
                        hideProgress();
                        self.PaymentProcessArray.removeAll();
                        self.PaymentVoucherArray.removeAll();

                        var Data1 = "", Data2 = "", Data3 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);

                            if (Data1[0].Clear == false || Data1[0].Clear == "false" || Data1[0].Clear == "0" || Data1[0].Clear == "False") {
                                jAlert(Data1[0].Message, "Message");
                            }
                            else {
                                jAlert(Data1[0].Message, "Message");

                                //Filling contents to popup.
                                $("#lblPaymentBatch").text(Data1[0].BatchNo);

                                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                                    Data2 = JSON.parse(response.Data2);
                                    self.PaymentProcessArray(Data2);
                                }

                                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                                    Data3 = JSON.parse(response.Data3);
                                    self.PaymentVoucherArray(Data3);
                                }
                                ShowDialog();
                                chkBit = "1";
                                self.search();
                            }
                        } else {
                            hideProgress();
                            jAlert("unable to process your request. please try again!", "Message");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        hideProgress();
                    }
                });
            } else {
                hideProgress();
                return false;
            }
        });
    };

    //Clear the form Entry
    self.clearFilter = function () {
        $("#txtDocFrom").val("");
        $("#txtDocTo").val("");
        $("#hdfAuthCodeId").val("0");
        $("#txtAuthCode").val("");
        $("#hdfAuthNameId").val("0");
        $("#txtAuthName").val("");
        $("#txtDocNo").val("");
        $("#txtDocAmount").val("");
        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");

        $("#hdfSupplierCode").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");
        $("#hdfRaiserCode").val("0");
        $("#txtRaiserCode").val("");
        $("#hdfRaiserName").val("0");
        $("#txtRaiserName").val("");
        $("#ddlSDocType").val("0");
        $("#chkHSelete").prop('checked', false);

        $("#gvPaymentRun").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bSort": false,
            "bFilter": false,
            "bLengthChange": false
        }).destroy();
        self.PaymentRunArray.removeAll();

        //$('#btnGeneratePayment').attr('disabled', true);

    };

    //Datatable Plugin codeing
    self.DatatableCall = function () {
        if ($("#gvPaymentRun > tbody > tr").length == self.PaymentRunArray().length) {
            $("#gvPaymentRun").DataTable({
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
        $("#gvPaymentRun").DataTable({
            "autoWidth": false,
            "destroy": true,
            "bFilter": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bInfo": false
        }).destroy();
        self.PaymentRunArray.removeAll();
    }

    self.loadGrid();

    //load the doc Type dropdown
    self.loadDocType();
};

var mainViewModel = new PaymentRunModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogyShow = $("[id$='ShowDialog']");
    objDialogyShow.dialog({
        autoOpen: false,
        modal: true,
        width: 1024,
        height: 350,
        duration: 300
    });

    objDialog = $("[id$='ShowAuthorizer']");
    objDialog.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 350,
        duration: 300
    });

    objDedupDet = $("[id$='ShowDedupDetail']");
    objDedupDet.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 400,
        duration: 300
    });

    //$('#btnGeneratePayment').attr('disabled', true);

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".fsDatepicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
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
            $("#hdfSupplierCode").val("0");
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
                $("#hdfSupplierCode").val(i.item.val);
                $("#txtSupplierCode").val(i.item.label);
            }
        });
    });

    $("#txtSupplierCode").focusout(function () {
        var hdfId = $("#hdfSupplierCode").val();
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

    //Load Raiser Code Auto Complete
    $("#txtRaiserCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfRaiserCode").val("0");
        }

        $("#txtRaiserCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtRaiserCode").val() + "'}",
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
                $("#hdfRaiserCode").val(i.item.val);
                $("#txtRaiserCode").val(i.item.label);
            }
        });
    });

    $("#txtRaiserCode").focusout(function () {
        var hdfId = $("#hdfRaiserCode").val();
        var txtCurName = $("#txtRaiserCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRaiserCode").val("");
        }
    });

    //Load Raiser Name Auto Complete
    $("#txtRaiserName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfRaiserName").val("0");
        }

        $("#txtRaiserName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtRaiserName").val() + "'}",
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
                $("#hdfRaiserName").val(i.item.val);
                $("#txtRaiserName").val(i.item.label);
            }
        });
    });

    $("#txtRaiserName").focusout(function () {
        var hdfId = $("#hdfRaiserName").val();
        var txtCurName = $("#txtRaiserName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRaiserName").val("");
        }
    });

    //Load Authorizer Code Auto Complete
    $("#txtAuthCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfAuthCodeId").val("0");
        }

        $("#txtAuthCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtAuthCode").val() + "'}",
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
                $("#hdfAuthCodeId").val(i.item.val);
                $("#txtAuthCode").val(i.item.label);
            }
        });
    });

    $("#txtAuthCode").focusout(function () {
        var hdfId = $("#hdfAuthCodeId").val();
        var txtCurName = $("#txtAuthCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtAuthCode").val("");
        }
    });

    //Load Authorizer Name Auto Complete
    $("#txtAuthName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfAuthNameId").val("0");
        }

        $("#txtAuthName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtAuthName").val() + "'}",
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
                $("#hdfAuthNameId").val(i.item.val);
                $("#txtAuthName").val(i.item.label);
            }
        });
    });

    $("#txtAuthName").focusout(function () {
        var hdfId = $("#hdfAuthNameId").val();
        var txtCurName = $("#txtAuthName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtAuthName").val("");
        }
    });

    $("#chkHSelete").click(function (e) {
        $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);
    });

    $("#txtReason").keyup(function () {
        chkReview();
        var _reason = $("#txtReason").val();
        if (_reason.trim() != "") {
            $("#txtReason").removeClass('required');
            $("#txtReason").addClass('valid');
        }
        else {
            $("#txtReason").removeClass('valid');
            $("#txtReason").addClass('required');
        }
    });
});

function ShowDialog() {
    $('#ShowDialog').attr("style", "display:block;");
    objDialogyShow.dialog({ title: 'Payment Batch Summary', width: '1024', height: '350' });
    objDialogyShow.dialog("open");
    return false;
}

function ShowAuthorizer() {
    chkReview();

    var IsCheckShow = $("#hdfCheckShowing").val();
    if (IsCheckShow == "0") {
        $('#ShowAuthorizer').attr("style", "display:block;");
        objDialog.dialog({ title: 'Authorization Reverse', width: '600', height: '350' });
        objDialog.dialog("open");
        return false;
    } else if (IsCheckShow == "1") {
        $('#ShowAuthorizer').attr("style", "display:block;");
        objDialog.dialog({ title: 'Authorization Reverse With GL', width: '600', height: '350' });
        objDialog.dialog("open");
        return false;
    }
}

function ShowDedupData() {

    $('#ShowDedupDetail').attr("style", "display:block;");
    objDedupDet.dialog({ title: 'Dedup Details', width: '800', height: '400' });
    objDedupDet.dialog("open");
    return false;
}

function chkReview() {
    var _reason = $("#txtReason").val();

    if (_reason == "") {
        $('#btnAuthrizerReverse').attr('disabled', true);
        $('#btnAuthrizerReverseWithGL').attr('disabled', true);
    } else {
        $('#btnAuthrizerReverse').attr('disabled', false);
        $('#btnAuthrizerReverseWithGL').attr('disabled', false);
    }

    var IsCheckShow = $("#hdfCheckShowing").val();
    if (IsCheckShow == "0") {
        $('#btnAuthrizerReverse').show();
        $('#btnAuthrizerReverseWithGL').hide();
    } else if (IsCheckShow == "1") {
        $('#btnAuthrizerReverse').hide();
        $('#btnAuthrizerReverseWithGL').show();
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isEvent(evt) {
    return false;
}