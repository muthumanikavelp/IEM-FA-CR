var objDialogy;
UrlDet = UrlDet.replace("GetPaymodeConversion", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var objDialogyBenificiary;
var CreditNoteCheckerModel = function () {
    var self = this;
    var supid = "";
    self.MakerDetailArray = ko.observableArray();
    self.PaymentModeArray = ko.observableArray();
    self.PayBankArray = ko.observableArray();
    self.BenificiaryArray = ko.observableArray();
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

    self.loadPaymentMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "LoadPayModeType",
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

    self.search = function () {
        var data = {
            PVNoFrom: $("#txtPVFrom").val(),
            PVNoTo: $("#txtPVTo").val(),
            PVNo: $("#txtPVNo").val(),
            Amount: $("#txtAmount").val().replace(/,/g, ''),
            EmployeeCode: $("#hdfEmpCodeId").val(),
            EmployeeName: $("#hdfEmpName").val(),
            SupplierCode: $("#hdfSupplierId").val(),
            SupplierName: $("#hdfSupplierName").val(),
            BounceFrom: $("#txtBounceFrom").val(),
            BounceTo: $("#txtBounceTo").val(),
            ChequeNo: $("#txtChequeNo").val(),
            MemoNo: $("#txtMemoNo").val(),
            PayMode: $("#ddlPayMode").val(),
            ViewType: "0"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPaymodeConversion",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";                
                self.loadGrid();
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.MakerDetailArray(Data2);
                } else {
                    //show Message if error
                    if (Data1[0].Message == "" && self.MakerDetailArray().length == 0) {
                        jAlert("Sorry no record found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Reissue = function (item) {
        ShowDialog();
        var data = {
            PVNoFrom: $("#txtPVFrom").val(),
            PVNoTo: $("#txtPVTo").val(),
            PVNo: item.PvNo,
            Amount: $("#txtAmount").val().replace(/,/g, ''),
            EmployeeCode: $("#hdfEmpCodeId").val(),
            EmployeeName: $("#hdfEmpName").val(),
            SupplierCode: $("#hdfSupplierId").val(),
            SupplierName: $("#hdfSupplierName").val(),
            BounceFrom: $("#txtBounceFrom").val(),
            BounceTo: $("#txtBounceTo").val(),
            ChequeNo: $("#txtChequeNo").val(),
            MemoNo: $("#txtMemoNo").val(),
            PayMode: $("#ddlPayMode").val(),
            ViewType: "1"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPaymodeConversion",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
              
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    
                    $("#lblPVDate").text(Data2[0].PvDate);
                    $("#lblPVNo").text(Data2[0].PvNo);
                    $("#lblPVAmount").text(self.formatNumber(Data2[0].PvAmount));
                    $("#lblPayMode").text(Data2[0].PayMode);
                    $("#lblChequeNo").text(Data2[0].ChqNo);
                    $("#lblMemoNo").text(Data2[0].MemoNo);
                    $("#lblBankName").text(Data2[0].Bank);
                    $("#lblAcNo").text(Data2[0].accNo);
                    $("#lblIFSCCode").text(Data2[0].IFSCcode);
                    $("#lblType").text(Data2[0].EmployeeSupplierType);
                    $("#lblSECode").text(Data2[0].EmployeeSupplierCode);
                    $("#lblSEName").text(Data2[0].EmployeeSupplierName);
                    $("#lblbounceDate").text(Data2[0].BounceDate);
                    $("#txtBounceReason").text(Data2[0].BounceRemark);
                    supid = Data2[0].supplierheader_gid;
                    var suptype = Data2[0].EmployeeSupplierType;
                    
                    if (suptype == "Employee") {
                        $("#txtMBeneficiaryName").val(Data2[0].EmployeeSupplierName);
                    }
                    else {
                        $("#txtMBeneficiaryName").val(Data2[0].Beneficiary);
                    }
                    $("#txtMAcNo").val(Data2[0].accNo);
                    $("#txtMIFSCCode").val(Data2[0].IFSCcode);
                    $("#txtMBankName").val(Data2[0].BankId);
                    $("#hfPvIdReissue").val(Data2[0].PvId);
                    $("#hfBankIdReissue").val(Data2[0].BankId);

                    var _data = $("#txtMBankName").val();
                    if (_data.trim() != "0") {
                        $("#txtMBankName").removeClass('required');
                        $("#txtMBankName").addClass('valid');
                    }
                    else {
                        $("#txtMBankName").removeClass('valid');
                        $("#txtMBankName").addClass('required');
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.Submit = function () {
        //var count = $("#ddlPayModeReissue").val();
        //if (count == "" || count == "0") {
        //    jAlert("Ensure New Pay Mode!", "Error");
        //    return false;
        //}

        var data = {
            PVId: $("#hfPvIdReissue").val(),
            PayMode: $("#ddlPayModeReissue").val(),
            Beneficiary: $("#txtMBeneficiaryName").val(),
            AccNo: $("#txtMAcNo").val(),
            IfscCode: $("#txtMIFSCCode").val(),
            BankId: $("#txtMBankName").val(),
            Desc: $("#txtMDescription").val(),
            Remarks: $("#txtMRemark").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPaymodeConversion",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 == "Unauthorized User!") {
                    jAlert("Unauthorized User!");
                }
                else if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message
                    if (Data1[0].Clear == "True" || Data1[0].Clear == "true" || Data1[0].Clear == "1") {
                        objDialogy.dialog("close");
                        jConfirm(Data1[0].Message, "Message", function (callback) {
                            if (callback == true) {

                                self.search();
                            } else {
                                self.search();
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");
                    } else {
                        //show Message if error
                        if (Data1[0].Message != "") {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvMaker > tbody > tr").length == self.MakerDetailArray().length) {
            $("#gvMaker").DataTable({
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

    self.clearFilter = function () {
        $("#txtPVFrom").val("");
        $("#txtPVTo").val("");
        $("#txtPVNo").val("");
        $("#txtAmount").val("");
        $("#txtEmpCode").val("");
        $("#hdfEmpCodeId").val("0");
        $("#txtEmpName").val("");
        $("#hdfEmpName").val("0");
        $("#txtSupplierCode").val("");
        $("#hdfSupplierId").val("0");
        $("#txtSupplierName").val("");
        $("#hdfSupplierName").val("0");
        $("#txtBounceFrom").val("");
        $("#txtBounceTo").val("");

        $("#txtChequeNo").val("");
        $("#txtMemoNo").val("");
        $("#ddlPayMode").val("");

        self.loadGrid();
    };
    this.FetchBenificiary = function () {
       
        
        var PayMode = $("#ddlPayModeReissue").val();
      
        if (PayMode == "0")
            return false;
        var InputFilter = {
            PayMode: PayMode,
            SupplierId: supid
        };
        $.ajax({
            type: "post",
            url: UrlDet + "FetchBenificiaryDetails",
            data: JSON.stringify(InputFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.BenificiaryArray(Data1);
                $('#BenificiaryDialog').attr("style", "display:block;");
                objDialogyBenificiary.dialog({ title: 'Benificiary Details', width: '700', height: '400' });
                objDialogyBenificiary.dialog("open");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };
    this.SelectBenificiary = function (item) {
        $("#txtMAcNo").val(item.AccountNo);
        $("#txtMBeneficiaryName").val(item.BeneficiaryName);
        $("#txtMIFSCCode").val(item.IFSCCode);

        if ($("#txtMAcNo").val() == "") {
            $("#txtMAcNo").removeClass("valid");
            $("#txtMAcNo").addClass("required");
        }
        else {
            $("#txtMAcNo").addClass("valid");
            $("#txtMAcNo").removeClass("required");
        }

        if ($("#txtMBeneficiaryName").val() == "") {
            $("#txtMBeneficiaryName").removeClass("valid");
            $("#txtMBeneficiaryName").addClass("required");
        }
        else {
            $("#txtMBeneficiaryName").addClass("valid");
            $("#txtMBeneficiaryName").removeClass("required");
        }
        objDialogyBenificiary.dialog("close");
    };
    this.LoadPayBank = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetPayBank",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.PayBankArray(Data1);

                if (response.Data2 != null && response.Data2 != "") {
                    self.DefaultPayBank(response.Data2)
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadGrid = function () {
        $("#gvMaker").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.MakerDetailArray.removeAll();
    }
    self.loadGrid();
    self.loadPaymentMode();

    self.LoadPayBank();
};

var mainViewModel = new CreditNoteCheckerModel();
ko.applyBindings(mainViewModel);


$(document).ready(function () {

    //$("#btnSubmit").attr("disabled", true);
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

    objDialogy = $("[id$='ShowDialog']");
    objDialogy.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 750,
        duration: 300
    });

    objDialogyBenificiary = $("[id$='BenificiaryDialog']");
    objDialogyBenificiary.dialog({
        autoOpen: false,
        modal: true,
        width: 700,
        height: 400,
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

    
    $("#txtMRemark, #txtMBeneficiaryName, #txtMAcNo, #txtMIFSCCode").keyup(function () {
        checkValidation();

        var _data = $(this).val();
        if (_data.trim() != "") {
            $(this).removeClass('required');
            $(this).addClass('valid');
        }
        else {
            $(this).removeClass('valid');
            $(this).addClass('required');
        }
    });

    $("#txtMBankName").change(function () {
        checkValidation();

        var _data = $(this).val();
        if (_data.trim() != "0") {
            $(this).removeClass('required');
            $(this).addClass('valid');
        }
        else {
            $(this).removeClass('valid');
            $(this).addClass('required');
        }
    });

    $("#txtAmount").keyup(function (event) {
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

function checkValidation() {
    var _data = $("#txtMRemark").val();
    var _payMode = $("#ddlPayModeReissue").val();
    var _BeneficiaryName = $("#txtMBeneficiaryName").val();
    var txtMAcNo = $("#txtMAcNo").val();
    var txtMIFSCCode = $("#txtMIFSCCode").val();
    var txtMBankName = $("#txtMBankName").val();
    if (_payMode == "CHQ") {
        if (_data.trim() == "" || _payMode == "" || _payMode == "0" || _BeneficiaryName == ""|| txtMBankName == "0") {
            $('#btnSubmit').attr('disabled', true);
        }
        else {
            $('#btnSubmit').attr('disabled', false);
        }
    }
    else {
        if (_data.trim() == "" || _payMode == "" || _payMode == "0" || _BeneficiaryName == "" || txtMAcNo == "" || txtMIFSCCode == "" || txtMBankName == "0") {
            $('#btnSubmit').attr('disabled', true);
        }
        else {
            $('#btnSubmit').attr('disabled', false);
        }

    }
    
}

function ShowDialog() {

    $("#txtMRemark").val("");
    $("#txtMRemark").removeClass('required').removeClass('valid');
    $("#txtMRemark").addClass('required');

    checkValidation();

    $('#ShowDialog').attr("style", "display:block;");
    objDialogy.dialog({ title: 'Payment Reissue - Maker', width: '900', height: '630' });
    objDialogy.dialog("open");
    return false;
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