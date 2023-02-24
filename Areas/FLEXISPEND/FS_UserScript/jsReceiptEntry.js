var objDialogyReceipt;
var objDialogGL;
UrlDet = UrlDet.replace("GetReceiptEntry", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var ChequeInventoryModel = function () {
    var self = this;

    //dropdown array
    self.SourceArray = ko.observableArray();
    self.ReceiptFromArray = ko.observableArray();
    self.BankArray = ko.observableArray();
    self.ReceiptModeArray = ko.observableArray();
    self.SearchReceiptFromArray = ko.observableArray();

    //Grid Details
    self.ReceiptEntryArray = ko.observableArray();
    self.tmpReceiptDetArray = ko.observableArray();
    self.ReceiptEntryDetailArray = ko.observableArray();

    self.loadReceiptFromWA = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadClaimTypeWA",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.SearchReceiptFromArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadReceiptFrom = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadClaimType",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.ReceiptFromArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadReceiptMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadReceiptMode",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.ReceiptModeArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadSource = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadReceiptSource",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.SourceArray(response);
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

    self.ReceiptFromChanged = function (obj, event) {
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

            var chkbit = $("#ddlReceiptFrom").val();
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

    self.Search = function () {
        var data = {
            ReceiptDateFrom: $("#txtReceiptDateFrom").val(),
            ReceiptDateTo: $("#txtReceiptDateTo").val(),
            ReceiptNo: $("#txtReceiptNo").val(),
            ReceiptFrom: $("#ddlReceiptFrom").val(),
            EmpCodeId: $("#hdfEmpCodeId").val(),
            EmpNameId: $("#hdfEmpName").val(),
            SuppCodeId: $("#hdfSupplierId").val(),
            SuppNameId: $("#hdfSupplierName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetReceiptEntry",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                var Data1 = "", Data2 = "", Data3 = "";

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.ReceiptEntryArray(Data2);
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.tmpReceiptDetArray(Data3);
                } else {
                    self.tmpReceiptDetArray.removeAll();
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    } else if (self.ReceiptEntryArray().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.ClearFilter = function () {
        $("#txtReceiptDateFrom").val("");
        $("#txtReceiptDateTo").val("");
        $("#txtReceiptNo").val("");
        $("#ddlReceiptFrom").val("0");

        $("#hdfEmpCodeId").val("0");
        $("#txtEmpCode").val("");
        $("#hdfEmpName").val("0");
        $("#txtEmpName").val("");
        $("#hdfSupplierId").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierName").val("0");
        $("#txtSupplierName").val("");

        $("#ddlClaimType").val("0");
        $("#ddlBank").val("0");
        $("#txtPayBatchNo").val("");
        $("#txtMemoNo").val("");

        $("#txtEmpCode").removeAttr("disabled").removeAttr("style");
        $("#txtEmpName").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierCode").removeAttr("disabled").removeAttr("style");
        $("#txtSupplierName").removeAttr("disabled").removeAttr("style");

        self.loadGrid();
    };

    self.EReceiptFromChanged = function (obj, event) {
        if (event.originalEvent) {
            $("#hdfEReceiptCode").val("0");
            $("#txtEReceiptCode").val("");
            $("#hdfEReceiptName").val("0");
            $("#txtEReceiptName").val("");

            $('#txtEReceiptCode').removeClass('required').removeClass('valid');
            $('#txtEReceiptCode').addClass('required');

            $('#txtEReceiptName').removeClass('required').removeClass('valid');
            $('#txtEReceiptName').addClass('required');
        } else {
        }
    }

    self.AddNew = function () {
        $("#hdfReceiptId").val("0");

        $('#txtEReceiptName').removeClass('required').removeClass('valid');
        $('.receiptentry').removeClass('required').removeClass('valid');
        $('#txtEReceiptName').addClass('required');
        $('.receiptentry').addClass('required');

        $("#ddlEBankName").val("0");
        $("#ddlEReceiptMode").val("0");
        $("#ddlESource").val("0");
        $("#ddlEReceiptFrom").val("0");

        $("#txtEReceiptDate").val("");
        $("#txtEReceiptName").val("");
        $("#txtEAmount").val("");
        $("#txtEPayRefNo").val("");
        $("#txtETranDate").val("");
        $("#txtERemarks").val("");
        $("#txtRaiserName").val("");

        $("#hdfEReceiptName").val("0");
        $("#hdfRaiserName").val("0");

        $('#txtRaiserName').removeClass('valid').addClass('required');

        if (self.ReceiptEntryDetailArray() != null) {
            self.ReceiptEntryDetailArray([]);
        }

        ShowReceiptDetails("0");
        IsValidSubmit();
        $('#btnAddGL').attr('disabled', true);
        self.checkSubmit();
    };

    self.ViewDetails = function (item) {
        var tmpArray = new Array();
        self.clearReceiptField();

        $('#txtEReceiptName').removeClass('required').removeClass('valid');
        $('.receiptentry').removeClass('required').removeClass('valid');
        $('#txtEReceiptName').addClass('valid');
        $('.receiptentry').addClass('valid');

        $("#hdfReceiptId").val(item.receiptId);
        $("#ddlEBankName").val(item.bankId);
        $("#ddlEReceiptMode").val(item.payMode);
        $("#ddlESource").val(item.receiptMode);
        $("#ddlEReceiptFrom").val(item.receiptFrom);

        $("#txtEReceiptDate").val(item.receiptDate);
        $("#txtEReceiptName").val(item.EmployeeSupplierName);
        $("#txtEAmount").val(self.formatNumber(item.amount));
        $("#txtEPayRefNo").val(item.payRefNo);
        $("#txtETranDate").val(item.tranDate);
        $("#txtERemarks").val(item.makerRemark);

        $("#hdfEReceiptName").val(item.EmployeeSupplierId);

        if (self.ReceiptEntryDetailArray() != null) {
            self.ReceiptEntryDetailArray([]);
        }

        tmpArray = ko.utils.arrayFilter(self.tmpReceiptDetArray(), function (aitem) {
            return item.receiptId == aitem.receiptId;
        });

        self.ReceiptEntryDetailArray(tmpArray.slice());
        ShowReceiptDetails("0");

        $('#btnMSubmit').attr('disabled', true);
        $('#btnAddGL').attr('disabled', true);
        $('#btnSubmit').attr('disabled', true);
    };

    self.Select = function (item) {
        var tmpArray = new Array();
        self.clearReceiptField();

        $('#txtEReceiptName').removeClass('required').removeClass('valid');
        $('.receiptentry').removeClass('required').removeClass('valid');
        $('#txtEReceiptName').addClass('valid');
        $('.receiptentry').addClass('valid');

        $("#hdfReceiptId").val(item.receiptId);
        $("#ddlEBankName").val(item.bankId);
        $("#ddlEReceiptMode").val(item.payMode);
        $("#ddlESource").val(item.receiptMode);
        $("#ddlEReceiptFrom").val(item.receiptFrom);

        $("#hdfRaiserName").val(item.RaiserId);
        $("#txtRaiserName").val(item.Raiser);

        $("#txtEReceiptDate").val(item.receiptDate);
        $("#txtEReceiptName").val(item.EmployeeSupplierName);
        $("#txtEAmount").val(self.formatNumber(item.amount));
        $("#txtEPayRefNo").val(item.payRefNo);
        $("#txtETranDate").val(item.tranDate);
        $("#txtERemarks").val(item.makerRemark);

        $("#hfBankName").val($("#ddlEBankName option:selected").text());
        $("#hfAmount").val($("#txtEAmount").val());
       $("#hfDocno").val($("#txtEPayRefNo").val());
        
      
        $("#hdfEReceiptName").val(item.EmployeeSupplierId);

        if (self.ReceiptEntryDetailArray() != null) {
            self.ReceiptEntryDetailArray([]);
        }

        tmpArray = ko.utils.arrayFilter(self.tmpReceiptDetArray(), function (aitem) {
            return item.receiptId == aitem.receiptId;
        });

        self.ReceiptEntryDetailArray(tmpArray.slice());
        ShowReceiptDetails("0");
        IsValidSubmit();
        $('#btnAddGL').attr('disabled', false);
        self.checkSubmit();
    };

    self.SaveReceipt = function () {
        var data = {
            ReceiptId: $("#hdfReceiptId").val(),
            ReceiptDate: $("#txtEReceiptDate").val(),
            ReceiptFrom: $("#ddlEReceiptFrom").val(),
            Source: $("#ddlESource").val(),
            Raiser: $("#hdfRaiserName").val(),
            EmpSuppNameId: $("#hdfEReceiptName").val(),
            ReceiptMode: $("#ddlEReceiptMode").val(),
            Amount: $("#txtEAmount").val().replace(/,/g, ''),
            PayRefNo: $("#txtEPayRefNo").val(),
            TranDate: $("#txtETranDate").val(),
            BankId: $("#ddlEBankName").val(),
            Remarks: $("#txtERemarks").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetReceiptEntry",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                    return false;
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    jAlert(Data1[0].Message, "Message");

                    if (Data1[0].Clear == true) {
                        $('#btnAddGL').attr('disabled', false);
                        $("#hdfReceiptId").val(Data1[0].ReceiptGId);
                        self.ClearFilter();
                        self.Search();
                        $("#hfBankName").val($("#ddlEBankName option:selected").text());
                        $("#hfAmount").val($("#txtEAmount").val());
                        $("#hfDocno").val($("#txtEPayRefNo").val());
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.MakerSubmit = function () {
        var data = {
            ReceiptId: $("#hdfReceiptId").val(),
            Status: "IsMakerRaised",
            Remarks: ""
        };
        $.ajax({
            type: "post",
            url: UrlDet + "UpdateReceiptStatus",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                    return false;
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    jAlert(Data1[0].Message, "Message");

                    if (Data1[0].Clear == true) {
                        ShowReceiptDetails("1");
                        self.Search();
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.checkSubmit = function () {
        var _MstId = $("#hdfReceiptId").val();
        var _DetId = self.ReceiptEntryDetailArray().length;

        if (_MstId.trim() == "" || _MstId.trim() == "0" || _DetId == null || _DetId == "0" ) {
            $('#btnMSubmit').attr('disabled', true);
        }
        else {
            $('#btnMSubmit').attr('disabled', false);
        }
    }

    //GL Details
    self.AddGl = function () {
       
        ShowGLDetails("0");

        $("#txtEGLAC").removeClass('required').removeClass('valid');
        $("#txtEGLAC").addClass('required');

        $('.glEntry').removeClass('required').removeClass('valid');
        $('.glEntry').addClass('required');

        $("#hdfEGLUploadId").val("0");
        $("#txtEGLCreditFrom").val("");
        $("#txtEGLDocNo").val("");
        $("#txtEGLAC").val("");
        $("#hdfEGLAC").val("0");
        $("#txtEGLAmount").val("");
        $("#txtEGLDescription").val("");

        $("#txtEGLCreditFrom,#txtEGLAmount,#txtEGLAC").removeClass('required').removeClass('valid');
        $("#txtEGLCreditFrom,#txtEGLAmount,#txtEGLAC").addClass('valid');

        var tmpArray = new Array();
        var _receiptId = $("#hdfReceiptId").val();

        tmpArray = ko.utils.arrayFilter(self.ReceiptEntryArray(), function (aitem) {
            return _receiptId == aitem.receiptId;
        });
        var model = tmpArray.slice();
        //$("#txtEGLCreditFrom").val(model[0].EmployeeSupplierName);
        //$("#txtEGLAmount").val(self.formatNumber(model[0].amount));

        $("#txtEGLCreditFrom").val($("#hfBankName").val());
        $("#txtEGLAmount").val($("#hfAmount").val());
        $("#txtEGLDocNo").val($("#hfDocno").val());
        IsGLValidSubmit();
    };

    self.EditGl = function (item) {
        ShowGLDetails("0");
        $("#hdfEGLUploadId").val(item.receiptGLId);
        $("#txtEGLCreditFrom").val(item.CRFrom);
        $("#txtEGLDocNo").val(item.docNo);
        $("#hdfEGLAC").val(item.GLNo);
        $("#txtEGLAC").val(item.GLNo);
        $("#txtEGLAmount").val(self.formatNumber(item.GLamount));
        $("#txtEGLDescription").val(item.Desc);

        //$("#txtEGLAC").removeClass('required').removeClass('valid');
        //$("#txtEGLAC").addClass('valid');

        $('.glEntry').removeClass('required').removeClass('valid');
        $('.glEntry').addClass('valid');

        IsGLValidSubmit();
    };

    self.DeleteGl = function (item) {

        jConfirm("Do you want to delete the record?", "Message", function (callback) {
            if (callback == true) {
                var data = {
                    ReceiptGLId: item.receiptGLId,
                    ReceiptId: item.receiptId,
                    CRFrom: "",
                    DocNo: "",
                    CRGlNo: "",
                    Description: "",
                    Amount: "",
                    IsRemoved: "1"
                };
                $.ajax({
                    type: "post",
                    url: UrlDet + "SetGLUploads",
                    data: JSON.stringify(data),
                    contentType: "application/json;",
                    success: function (response) {
                        self.loadGrid();
                        var Data1 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);

                            if (Data1[0].Message != "") {
                                jAlert(Data1[0].Message, "Message");
                            }

                            if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                                Data2 = JSON.parse(response.Data2);

                                self.ReceiptEntryDetailArray(Data2);
                            } else {
                                self.ReceiptEntryDetailArray.removeAll();
                            }
                            self.ClearFilter();
                            self.Search();
                        } else {
                            jAlert("unable to process your request. please try again!", "Message");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            } else {
                return false;
            }
        });
    };

    self.SubmitGL = function () {
        var data = {
            ReceiptGLId: $("#hdfEGLUploadId").val(),
            ReceiptId: $("#hdfReceiptId").val(),
            CRFrom: $("#txtEGLCreditFrom").val(),
            DocNo: $("#txtEGLDocNo").val(),
            CRGlNo: $("#txtEGLAC").val(),
            Description: $("#txtEGLDescription").val(),
            Amount: $("#txtEGLAmount").val().replace(/,/g, ''),
            IsRemoved: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetGLUploads",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                self.loadGrid();
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                    if (Data1[0].Clear == true) {
                        ShowGLDetails("1");
                        $("#hdfEGLUploadId").val("0");
                        self.ClearFilter();
                        self.Search();
                    }

                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);

                        self.ReceiptEntryDetailArray(Data2);
                    } else {
                        self.ReceiptEntryDetailArray.removeAll();
                    }
                } else {
                    jAlert("unable to process your request. please try again!", "Message");
                }
                self.checkSubmit();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    self.clearReceiptField = function () {
        $("#hdfReceiptId").val("0");

        $("#ddlEBankName").val("0");
        $("#ddlEReceiptMode").val("0");
        $("#ddlESource").val("0");
        $("#ddlEReceiptFrom").val("0");

        $("#txtEReceiptDate").val("");
        //$("#txtEReceiptCode").val("");
        $("#txtEReceiptName").val("");
        $("#txtEAmount").val("");
        $("#txtEPayRefNo").val("");
        $("#txtETranDate").val("");
        $("#txtERemarks").val("");

        //$("#hdfEReceiptCode").val("0");
        $("#hdfEReceiptName").val("0");

        if (self.ReceiptEntryDetailArray() != null) {
            self.ReceiptEntryDetailArray([]);
        }
    }

    self.DatatableCall = function () {
        if ($("#gvReceiptEntry > tbody > tr").length == self.ReceiptEntryArray().length) {
            $("#gvReceiptEntry").DataTable({
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

    self.loadGrid = function () {
        $("#gvReceiptEntry").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.ReceiptEntryArray.removeAll();
    }
    self.loadGrid();

    //load the Bank DropDown.
    self.loadBank();
    self.loadReceiptFrom();
    self.loadReceiptMode();
    self.loadReceiptFromWA();
    self.loadSource();
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

    objDialogyReceipt = $("[id$='digReceiptDet']");
    objDialogyReceipt.dialog({
        autoOpen: false,
        modal: true,
        width: 850,
        height: 700,
        duration: 300
    });

    objDialogGL = $("[id$='digGL']");
    objDialogGL.dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        height: 270,
        duration: 300
    });

    $(".fsDatePicker").datepicker({
        yearRange: '1900:+nn',
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $(".fsDatePicker").keyup(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46)
            $.datepicker._clearDate(this);

        $(this).datepicker('show');
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

    //Load Receipt Code Auto Complete
    //$("#txtEReceiptCode").keyup(function (e) {
    //    if (e.which != 13) {
    //        $("#hdfEReceiptCode").val("0");
    //    }

    //    $("#txtEReceiptCode").autocomplete({
    //        source: function (request, response) {
    //            $.ajax({
    //                url: "../Helper/GetAutoComplete" + getCodeUrl(),
    //                data: "{ 'txt' : '" + $("#txtEReceiptCode").val() + "'}",
    //                dataType: "json",
    //                type: "POST",
    //                contentType: "application/json; charset=utf-8",
    //                success: function (data) {
    //                    response($.map(data, function (item) {
    //                        return {
    //                            label: item.split('~')[1],
    //                            val: item.split('~')[0]
    //                        }
    //                    }))
    //                },
    //                error: function (response) {
    //                    //alert(response.responseText);
    //                },
    //                failure: function (response) {
    //                    //alert(response.responseText);
    //                }
    //            });
    //        },
    //        minLength: 1,
    //        select: function (e, i) {
    //            $("#hdfEReceiptCode").val(i.item.val);
    //            $("#txtEReceiptCode").val(i.item.label);
    //        }
    //    });
    //});

    //$("#txtEReceiptCode").focusout(function () {
    //    IsValidSubmit();
    //    var hdfId = $("#hdfEReceiptCode").val();
    //    var txtCurName = $("#txtEReceiptCode").val();
    //    $("#txtEReceiptCode").removeClass('required').removeClass('valid');
    //    if ((txtCurName.trim() == "" || txtCurName.trim() != "") && (hdfId.trim() == "" || hdfId.trim() == "0")) {
    //        $("#txtEReceiptCode").val("");
    //        $("#txtEReceiptCode").addClass('required');
    //    } else {
    //        $("#txtEReceiptCode").addClass('valid');
    //    }
    //});

    //Load Receipt Name Auto Complete
    $("#txtEReceiptName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfEReceiptName").val("0");
        }

        $("#txtEReceiptName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoReceipt" + getNameUrl(),
                    data: "{ 'txt' : '" + $("#txtEReceiptName").val() + "'}",
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
                $("#hdfEReceiptName").val(i.item.val);
                $("#txtEReceiptName").val(i.item.label);
            }
        });
    });

    $("#txtEReceiptName").focusout(function () {
        IsValidSubmit();
        var hdfId = $("#hdfEReceiptName").val();
        var txtCurName = $("#txtEReceiptName").val();
        $("#txtEReceiptName").removeClass('required').removeClass('valid');
        if ((txtCurName.trim() == "" || txtCurName.trim() != "") && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEReceiptName").val("");
            $("#txtEReceiptName").addClass('required');
        } else {
            $("#txtEReceiptName").addClass('valid');
            $("#txtEReceiptName").val(txtCurName.split('-')[1].trim());
        }
    });

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
        IsValidSubmit();
        var hdfId = $("#hdfRaiserName").val();
        var txtCurName = $("#txtRaiserName").val();
        $("#txtRaiserName").removeClass('required').removeClass('valid');
        if ((txtCurName.trim() == "" || txtCurName.trim() != "") && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRaiserName").val("");
            $("#txtRaiserName").addClass('required');
        } else {
            $("#txtRaiserName").addClass('valid');
        }
    });

    $('.glEntry').change(function () {
        IsGLValidSubmit();
        var _data = $(this).val();
        var trigerName = $(this).attr('id');
        if (_data.trim() != "") {
            $("#" + trigerName).removeClass('required').removeClass('valid');
            $("#" + trigerName).addClass('valid');
        }
        else {
            $("#" + trigerName).removeClass('valid').removeClass('required');
            $("#" + trigerName).addClass('required');
        }
    });

    $('.receiptentry').change(function () {
        IsValidSubmit();
        var _data = $(this).val();
        var trigerName = $(this).attr('id');
        if (_data.trim() != "") {
            $("#" + trigerName).removeClass('required').removeClass('valid');
            $("#" + trigerName).addClass('valid');
        }
        else {
            $("#" + trigerName).removeClass('valid').removeClass('required');
            $("#" + trigerName).addClass('required');
        }
    });

    $("#txtEGLAmount,#txtEAmount").keyup(function (event) {
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

        var _data = $(this).val();
        if (_data.trim() != "") {
            $(this).removeClass('required').removeClass('valid');
            $(this).addClass('valid');
        }
        else {
            $(this).removeClass('valid').removeClass('required');
            $(this).addClass('required');
        }

    });

    //Load GL A/C Auto Complete
    $("#txtEGLAC").keydown(function (e) {
        if (e.which != 13) {
            $("#hdfEGLAC").val("0");
        }
       
        $("#txtEGLAC").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteReceiptGLAC",
                    data: "{ 'txt' : '" + $("#txtEGLAC").val() + "'}",
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
                $("#hdfEGLAC").val(i.item.label);
                $("#txtEGLAC").val(i.item.label);
            }
        });
    });

    $("#txtEGLAC").focusout(function (e) {
        IsGLValidSubmit();
        var _data = $("#hdfEGLAC").val();
        $("#txtEGLAC").removeClass('required').removeClass('valid');
        if (_data.trim() != "" && _data.trim() != "0") {
            $("#txtEGLAC").addClass('valid');
        }
        else {
            $("#txtEGLAC").addClass('required');
            $("#txtEGLAC").val("");
        }
    });
});

function IsEnable(option) {
    $('#txtEReceiptName').attr('disabled', option);
    $('.receiptentry').attr('disabled', option);
    $("#ddlEBankName").attr('disabled', option);
    $("#ddlEReceiptMode").attr('disabled', option);
    $("#ddlESource").attr('disabled', option);
    $("#ddlEReceiptFrom").attr('disabled', option);
    $("#txtERemarks").attr('disabled', option);
}

function IsValidSubmit() {
    //var _recNo = $("#txtEReceiptNumber").val();
    var _recDate = $("#txtEReceiptDate").val();
    //var _recCode = $("#hdfEReceiptCode").val();
    var _recName = $("#hdfEReceiptName").val();
    var _raiserName = $("#hdfRaiserName").val();
    var _recName = $("#hdfEReceiptName").val();
    var _recAmount = $("#txtEAmount").val().trim();
    var _recPayRefNo = $("#txtEPayRefNo").val().trim();
    var _recTranDate = $("#txtETranDate").val().trim();
    var _recFrom = $("#ddlEReceiptFrom").val();
    var _recSource = $("#ddlESource").val();
    var _recMode = $("#ddlEReceiptMode").val();
    var _recBank = $("#ddlEBankName").val();
    //var _recRemarks = $("#txtERemarks").val();  _recNo.trim() == "" ||  _recCode.trim() == "" || _recCode.trim() == "0" ||

    if ( _recDate.trim() == "" || _recFrom == "" || _recFrom == "0" || _recSource == "" || _recSource == "0" || _recMode == "" || _recMode == "0" || _recBank == "" || _recBank == "0"
        || _recName.trim() == "" || _recName.trim() == "0" || _recAmount.trim() == "" || _recAmount.trim() == "0"
        || _recPayRefNo.trim() == "" || _recTranDate.trim() == "" || _raiserName == "" || _raiserName == "0") {
        $('#btnSubmit').attr('disabled', true);
    }
    else {
        $('#btnSubmit').attr('disabled', false);
    }
}

function IsGLValidSubmit() {
    var _glcreditFrom = $("#txtEGLCreditFrom").val();
    var _glDocNo = $("#txtEGLDocNo").val();
    var _glac = $("#hdfEGLAC").val();
    var _glamount = $("#txtEGLAmount").val();
    //_glDocNo.trim() == "" ||
    if (_glcreditFrom.trim() == "" || _glac == "" || _glac == "0" || _glamount == "" || _glamount == "0") {
        $('#btnGLSubmit').attr('disabled', true);
    }
    else {
        $('#btnGLSubmit').attr('disabled', false);
    }
}

function getNameUrl() {
    var _d = $("#ddlEReceiptFrom").val();
    if (_d != null && _d != "" && _d == "Employee") {
        return "Employee";
    } else {
        return "Supplier"
    }
}

function ShowReceiptDetails(IsShow) {
    if (IsShow == "0") {
        $('#digReceiptDet').attr("style", "display:block;");
        objDialogyReceipt.dialog({ title: 'Receipt Entry' });
        objDialogyReceipt.dialog("open");
    } else {
        objDialogyReceipt.dialog("close");
    }
}

function ShowGLDetails(IsShow) {
    if (IsShow == "0") {
        $('#digGL').attr("style", "display:block;");
        objDialogGL.dialog({ title: 'GL Uploads' });
        objDialogGL.dialog("open");
    } else {
        objDialogGL.dialog("close");
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