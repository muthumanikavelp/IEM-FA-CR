var objDialogyAdd;
UrlDet = UrlDet.replace("GetAuditWorkTray", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var MakerModel = function () {
    var self = this;

    //Document Type
    self.DTSupplierPO = ko.observableArray();
    self.DTSupplierWO = ko.observableArray();
    self.DTSupplierNonPO = ko.observableArray();
    self.DTEmployeeClaim = ko.observableArray();
    self.DTUtility = ko.observableArray();
    self.ARFPrePaid = ko.observableArray();
    self.RetentionRelease = ko.observableArray();

    self.SIDSA = ko.observableArray();
    self.ECTraval = ko.observableArray();
    self.ECBulkConveyance = ko.observableArray();
    self.ECPettyCash = ko.observableArray();

    //Payment Adjustment
    self.PPXLiquidation = ko.observableArray();
    self.CreditNoteAdjustment = ko.observableArray();
    self.RetentionAdjustment = ko.observableArray();
    self.SuspensePaymode = ko.observableArray();

    //Document Other Tag
    self.Amort = ko.observableArray();
    self.UrgentPayment = ko.observableArray();
    self.ForeignCurrency = ko.observableArray();
    self.ManualECF = ko.observableArray();
    self.LeaseModule = ko.observableArray();
    self.Insurance = ko.observableArray();
    self.Insuranceadvance = ko.observableArray();

    //document type dropdown array
    self.DocumentTypeArray = ko.observableArray();

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

    self.Clear = function () {

        $("#txtSDocNumber").val('');
        $("#txtSInvoiceNumber").val('');
        $("#ddlSDocType").val('');
        $("#txtSAmount").val(''),
        $("#hdfSEmpCodeId").val('0');
        $("#hdfSEmpName").val('0');
        $("#hdfSSupplierId").val('0');
        $("#hdfSSupplierName").val('0');

        $("#txtSEmpCode").val('');
        $("#txtSEmpName").val('');
        $("#txtSSupplierCode").val('');
        $("#txtSSupplierName").val('');
        self.search();

    };

    self.search = function () {

        showProgress(); 
        var _LoadTabBit = $("#txtSDocNumber").val().trim() != "" ? "1" : "0";

        var MakerFilter = {
            DocNo: $("#txtSDocNumber").val(),
            InvoiceNo: $("#txtSInvoiceNumber").val(),
            DocTypeId: $("#ddlSDocType").val(),
            DocAmount: $("#txtSAmount").val().replace(/,/g, ''),
            EmpCodeId: $("#hdfSEmpCodeId").val(),
            EmpNameId: $("#hdfSEmpName").val(),
            SuppCodeId: $("#hdfSSupplierId").val(),
            SuppNameId: $("#hdfSSupplierName").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetAuditWorkTray",
            data: JSON.stringify(MakerFilter),
            contentType: "application/json;",
            success: function (response) {
                //Document Type
                self.DTSupplierPO.removeAll();
                self.DTSupplierWO.removeAll();
                self.DTSupplierNonPO.removeAll();
                self.DTEmployeeClaim.removeAll();
                self.DTUtility.removeAll();
                self.ARFPrePaid.removeAll();
                self.RetentionRelease.removeAll();

                self.SIDSA.removeAll();
                self.ECTraval.removeAll();
                self.ECBulkConveyance.removeAll();
                self.ECPettyCash.removeAll();

                //Payment Adjustment
                self.PPXLiquidation.removeAll();
                self.CreditNoteAdjustment.removeAll();
                self.RetentionAdjustment.removeAll();
                self.SuspensePaymode.removeAll();

                //Document Other Tag
                self.Amort.removeAll();
                self.UrgentPayment.removeAll();
                self.ForeignCurrency.removeAll();
                self.ManualECF.removeAll();
                self.LeaseModule.removeAll();
                self.Insurance.removeAll();
                self.Insuranceadvance.removeAll();

                var Data0 = "", Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "",
                    Data6 = "", Data7 = "", Data8 = "", Data9 = "", Data10 = "", Data11 = "", Data12 = "", Data13 = "", Data14 = "", Data15 = "", Data16 = "",
                    Data17 = "", Data18 = "", Data19 = "", Data20 = "",Data21="",Data22="";
                if (response.Data0 != null && response.Data0 != "" && JSON.parse(response.Data0) != null) {
                    Data0 = JSON.parse(response.Data0);

                    //show Message if error
                    if (Data0[0].Message != "") {
                        jAlert(Data0[0].Message, "Message");
                    }
                    $("#lblECF").text("ECF " + Data0[0].Role);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.Amort(Data1);

                    self.OpenRecordTab(_LoadTabBit, 3, 0);
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.UrgentPayment(Data2);

                    self.OpenRecordTab(_LoadTabBit, 3, 1);
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.ForeignCurrency(Data3);

                    self.OpenRecordTab(_LoadTabBit, 3, 2);
                }

                if (response.Data19 != null && response.Data19 != "" && JSON.parse(response.Data19) != null) {
                    Data19 = JSON.parse(response.Data19);
                    self.ManualECF(Data19);

                    self.OpenRecordTab(_LoadTabBit, 3, 3);
                }

                if (response.Data20 != null && response.Data20 != "" && JSON.parse(response.Data20) != null) {
                    Data20 = JSON.parse(response.Data20);
                    self.LeaseModule(Data20);

                    self.OpenRecordTab(_LoadTabBit, 3, 4);
                }

                if (response.Data21 != null && response.Data21 != "" && JSON.parse(response.Data21) != null) {
                    Data21 = JSON.parse(response.Data21);
                    self.Insurance(Data21);

                    self.OpenRecordTab(_LoadTabBit, 3, 5);
                }

                if (response.Data22 != null && response.Data22 != "" && JSON.parse(response.Data22) != null) {
                    Data22 = JSON.parse(response.Data22);
                    self.Insuranceadvance(Data22);

                    self.OpenRecordTab(_LoadTabBit, 3, 6);
                }

                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                    Data4 = JSON.parse(response.Data4);
                    self.PPXLiquidation(Data4);

                    self.OpenRecordTab(_LoadTabBit, 2, 0);
                }

                if (response.Data5 != null && response.Data5 != "" && JSON.parse(response.Data5) != null) {
                    Data5 = JSON.parse(response.Data5);
                    self.CreditNoteAdjustment(Data5);

                    self.OpenRecordTab(_LoadTabBit, 2, 1);
                }

                if (response.Data6 != null && response.Data6 != "" && JSON.parse(response.Data6) != null) {
                    Data6 = JSON.parse(response.Data6);
                    self.RetentionAdjustment(Data6);

                    self.OpenRecordTab(_LoadTabBit, 2, 2);
                }

                if (response.Data7 != null && response.Data7 != "" && JSON.parse(response.Data7) != null) {
                    Data7 = JSON.parse(response.Data7);
                    self.SuspensePaymode(Data7);

                    self.OpenRecordTab(_LoadTabBit, 2, 3);
                }

                if (response.Data8 != null && response.Data8 != "" && JSON.parse(response.Data8) != null) {
                    Data8 = JSON.parse(response.Data8);
                    self.DTSupplierPO(Data8);

                    self.OpenRecordTab(_LoadTabBit, 1, 0);
                }

                if (response.Data9 != null && response.Data9 != "" && JSON.parse(response.Data9) != null) {
                    Data9 = JSON.parse(response.Data9);
                    self.DTSupplierWO(Data9);

                    self.OpenRecordTab(_LoadTabBit, 1, 1);
                }

                if (response.Data10 != null && response.Data10 != "" && JSON.parse(response.Data10) != null) {
                    Data10 = JSON.parse(response.Data10);
                    self.DTSupplierNonPO(Data10);

                    self.OpenRecordTab(_LoadTabBit, 1, 2);
                }

                if (response.Data11 != null && response.Data11 != "" && JSON.parse(response.Data11) != null) {
                    Data11 = JSON.parse(response.Data11);
                    self.DTEmployeeClaim(Data11);

                    self.OpenRecordTab(_LoadTabBit, 1, 6);
                }

                if (response.Data12 != null && response.Data12 != "" && JSON.parse(response.Data12) != null) {
                    Data12 = JSON.parse(response.Data12);
                    self.DTUtility(Data12);

                    self.OpenRecordTab(_LoadTabBit, 1, 4);
                }

                if (response.Data13 != null && response.Data13 != "" && JSON.parse(response.Data13) != null) {
                    Data13 = JSON.parse(response.Data13);
                    self.ARFPrePaid(Data13);

                    self.OpenRecordTab(_LoadTabBit, 1, 10);
                }

                if (response.Data14 != null && response.Data14 != "" && JSON.parse(response.Data14) != null) {
                    Data14 = JSON.parse(response.Data14);
                    self.RetentionRelease(Data14);

                    self.OpenRecordTab(_LoadTabBit, 1, 5);
                }
                if (response.Data15 != null && response.Data15 != "" && JSON.parse(response.Data15) != null) {
                    Data15 = JSON.parse(response.Data15);
                    self.SIDSA(Data15);

                    self.OpenRecordTab(_LoadTabBit, 1, 3);
                }
                if (response.Data16 != null && response.Data16 != "" && JSON.parse(response.Data16) != null) {
                    Data16 = JSON.parse(response.Data16);
                    self.ECTraval(Data16);

                    self.OpenRecordTab(_LoadTabBit, 1, 7);
                }
                if (response.Data17 != null && response.Data17 != "" && JSON.parse(response.Data17) != null) {
                    Data17 = JSON.parse(response.Data17);
                    self.ECBulkConveyance(Data17);

                    self.OpenRecordTab(_LoadTabBit, 1, 8);
                }
                if (response.Data18 != null && response.Data18 != "" && JSON.parse(response.Data18) != null) {
                    Data18 = JSON.parse(response.Data18);
                    self.ECPettyCash(Data18);

                    self.OpenRecordTab(_LoadTabBit, 1, 9);
                }
                hideProgress();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
                hideProgress();
            }
        });
    };

    self.OpenRecordTab = function (_LoadTabBit, _TabType, _TabIndex) {
        if (_LoadTabBit == "1") {
            if (_TabType == 1) {
                var tabs = $('#DTtabs').tabs();
                tabs.tabs('select', _TabIndex);
            }

            if (_TabType == 2) {
                var tabs = $('#PAtabs').tabs();
                tabs.tabs('select', _TabIndex);
            }

            if (_TabType == 3) {
                var tabs = $('#DOTtabs').tabs();
                tabs.tabs('select', _TabIndex);
            }
            _LoadTabBit = "0";
        }
    }

    self.audit = function (ecfId, DocSubTypeId) {
        var MakerFilter = {
            ecfId: ecfId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetIsAssigned",
            data: JSON.stringify(MakerFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data0 = "";
                if (response.Data0 != null && response.Data0 != "" && JSON.parse(response.Data0) != null) {
                    Data0 = JSON.parse(response.Data0);
                    if (Data0[0].Clear == true) {
                        jAlert(Data0[0].Message, "Message");
                        return false;
                    }
                    else {
                        ko.utils.postJson(UrlDet + "SupplierInvoiceMaker", { data: ecfId, data1: DocSubTypeId });
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
        
    }

    //load the Doctument Type DropDown.
    self.loadDocType();
    self.search();

    

    self.Approval = function () {
        var array = "";
        $('input.chkSelect:checkbox:checked').each(function (index) {
            array += $(this).attr('id') + "|";
        });
        if (array == "") {
            jAlert("Select one or mode document!", "Message");
            return false;
        }
        var MakerFilter = {
            ECFIds: array,
            Status: "AuthorizerApprove",  //kathir - previous - Approve
            Remarks: ""
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetLeaseBulkApproval",
            data: JSON.stringify(MakerFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data0 = "";
                if (response.Data0 != null && response.Data0 != "" && JSON.parse(response.Data0) != null) {
                    Data0 = JSON.parse(response.Data0);
                    if (Data0[0].Clear == true) {
                        jAlert(Data0[0].Message, "Message");
                        self.search();
                    }
                    else
                    {
                        jConfirm(Data0[0].Message, "Confirm", function (callback) {
                            if (callback == true) {
                                ko.utils.postJson(UrlDet + "DownloadErrorLeaseDocument");
                            } else {
                                return false;
                            }
                        });
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    }
};

var mainViewModel = new MakerModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objDialogyAdd = $("[id$='ShowDialog']");
    objDialogyAdd.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 300,
        duration: 300
    });

    $("#chkHSelect").click(function () {
        if ($("#chkHSelect").prop("checked")) {
            $(".chkSelect").attr("checked", "checked");
        }
        else {
            $(".chkSelect").removeAttr("checked");
        }
    });

    //Load Employee Code Auto Complete
    $("#txtSEmpCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSEmpCodeId").val("0");
        }

        $("#txtSEmpCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployeeCode",
                    data: "{ 'txt' : '" + $("#txtSEmpCode").val() + "'}",
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
                $("#hdfSEmpCodeId").val(i.item.val);
                $("#txtSEmpCode").val(i.item.label);
            }
        });
    });

    $("#txtSEmpCode").focusout(function () {
        var hdfId = $("#hdfSEmpCodeId").val();
        var txtCurName = $("#txtSEmpCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSEmpCode").val("");
        }
    });

    //Load txtSSupplier Code Auto Complete
    $("#txtSSupplierCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSSupplierId").val("0");
        }

        $("#txtSSupplierCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteSupplierCode",
                    data: "{ 'txt' : '" + $("#txtSSupplierCode").val() + "'}",
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
                $("#txtSSupplierCode").val(i.item.label);
            }
        });
    });

    $("#txtSSupplierCode").focusout(function () {
        var hdfId = $("#hdfSSupplierId").val();
        var txtCurName = $("#txtSSupplierCode").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSSupplierCode").val("");
        }
    });

    //Load Employee Name Auto Complete
    $("#txtSEmpName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSEmpName").val("0");
        }

        $("#txtSEmpName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtSEmpName").val() + "'}",
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
                $("#hdfSEmpName").val(i.item.val);
                $("#txtSEmpName").val(i.item.label);
            }
        });
    });

    $("#txtSEmpName").focusout(function () {
        var hdfId = $("#hdfSEmpName").val();
        var txtCurName = $("#txtSEmpName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSEmpName").val("");
        }
    });

    //Load Supplier Name Auto Complete
    $("#txtSSupplierName").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSSupplierName").val("0");
        }

        $("#txtSSupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: UrlHelper + "GetAutoCompleteSupplier",
                    data: "{ 'txt' : '" + $("#txtSSupplierName").val() + "'}",
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
                $("#txtSSupplierName").val(i.item.label);
            }
        });
    });

    $("#txtSSupplierName").focusout(function () {
        var hdfId = $("#hdfSSupplierName").val();
        var txtCurName = $("#txtSSupplierName").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSSupplierName").val("");
        }
    });

    $("#txtSAmount").keyup(function (event) {
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

function ShowDialog() {
    $('#ShowDialog').attr("style", "display:block;");

    objDialogyAdd.dialog({ title: 'Urgent Tag', width: '600', height: '300' });
    objDialogyAdd.dialog("open");
    return false;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}