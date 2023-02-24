var objDialog;
var objDialogyAdd, objDialogyAddDebit, objDialogyAddPayment, objDialogyAddAttachment, objDialogyInvoice, objDialogyPPX,
  objDialogyCreditNote, objDialogyWHTax, objDialogyAmort, objDialogyPoMapping, objDialogyAddPayment1, objDialogyBenificiary,
  objDialogyGST, objDialogyAddDebit, objDialogyAddPayment, objDialogyAddAttachment, objDialogyPPX, objDialogyExpenseDetails,
  objDialogyAddPayment1, objDialogySUS, objDialogyAddAttachment, objDialogyInvoiceTravel, objDialogyGSTTravel, objDialogyInvoiceNonTravel,
  objDialogyGSTNonTravel;
var objDialogyInvoicePC;
var objDialogyGSTPC;


MUrlDet = MUrlDet.replace("GetQuery", "");
MUrlHelper = MUrlHelper.replace("GetAutoCompleteCourier", "");
var QueryModel = function () {
    var self = this;

    self.DocumentTypeArray = ko.observableArray();
    self.QueryArray = ko.observableArray();

    self.AuditTrailArray = ko.observableArray();
    //self.QueryArray = ko.observableArray();
    //self.QueryArray = ko.observableArray();

    self.loadDocumentType = function () {
        $.ajax({
            type: "post",
            url: MUrlHelper + "MasterDocumentType",
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

    self.Search = function () {
        var data = {
            ECFNo: $("#txtSECFNumber").val(),
            ECFDate: $("#txtSECFDate").val(),
            DocTypeId: $("#ddlDocType").val(),
            RaiserId: $("#hdfRaiser").val(),
            AuthrId: $("#hdfAuthorizer").val(),
            DocStatus: "",//$("#txtDateFrom").val(), 
            SuppCodeId: "0",//$("#hdfVendorCode").val(),
            SuppNameId: $("#hdfVendor").val(),
            InvoiceNo: $("#txtSInvoiceNumber").val(),
            InvAmount: $("#txtSInvAmt").val().replace(/,/g, ''),
            ECFAmount: $("#txtSECFAmount").val().replace(/,/g, ''),
            PVNo: $("#txtSPVNumber").val(),
            PVDate: $("#txtPVDate").val(),
            InwardAWBNo: $("#txtInwAwbNo").val(),
            ChqAWBNo: $("#txtChqAwbNo").val(),
            Employee: $("#hfEmployee").val(),
            ChequeNo: $("#txtChqueNo").val(),
            ChqueAmount: $("#txtChqueAmount").val().replace(/,/g, ''),
            PhyReceptDateFrom: $("#txtPDateFrom").val(),
            PhyReceptDateTo: $("#txtPDateTo").val(),
            pono: $("#txtpo").val(),
            cbfNo: $("#txtcbfno").val()
        };

      //  self.QueryArray.removeAll();
        $.ajax({
            type: "post",
            url: MUrlDet + "GetQuery",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                //  $('#tblFSQuery tbody').empty();
             //  alert('test')
                $("#tblFSQuery").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                $('#tblFSQuery tbody').empty();
                self.QueryArray("");
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);

                    self.QueryArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    } else if (self.QueryArray().length == 0) {
                        jAlert("Sorry no records found!", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    };

    //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

    this.DownloadqueryReport = function () {


        var ECFNo = $("#txtSECFNumber").val();
        var ECFDate = $("#txtSECFDate").val();
        var DocTypeId = $("#ddlDocType").val();
        var RaiserId = $("#hdfRaiser").val();
        var AuthrId = $("#hdfAuthorizer").val();
        var DocStatus = "";//$("#txtDateFrom").val(); 
        var SuppCodeId = "0";//$("#hdfVendorCode").val();
        var SuppNameId = $("#hdfVendor").val();
        var InvoiceNo = $("#txtSInvoiceNumber").val();
        var InvAmount = $("#txtSInvAmt").val().replace(/,/g, '');
        var ECFAmount = $("#txtSECFAmount").val().replace(/;/g, '');
        var PVNo = $("#txtSPVNumber").val();
        var PVDate = $("#txtPVDate").val();
        var InwardAWBNo = $("#txtInwAwbNo").val();
        var ChqAWBNo = $("#txtChqAwbNo").val();
        var Employee = $("#hfEmployee").val();
        var ChequeNo = $("#txtChqueNo").val();
        var ChqueAmount = $("#txtChqueAmount").val().replace(/,/g, '');
        var PhyReceptDateFrom = $("#txtPDateFrom").val();
        var PhyReceptDateTo = $("#txtPDateTo").val();

        var pono = $("#txtpo").val();
        var cbfNo = $("#txtcbfno").val();

        ko.utils.postJson(MUrlDet + "DownloadqueryReport?ECFNo=" + ECFNo + "&ECFDate=" + ECFDate + "&DocTypeId=" + DocTypeId + "&RaiserId=" + RaiserId + "&AuthrId=" + AuthrId + "&DocStatus=" + DocStatus + "&SuppCodeId=" + SuppCodeId + "&SuppNameId=" + SuppNameId + "&InvoiceNo=" + InvoiceNo + "&InvAmount=" + InvAmount + "&ECFAmount=" + ECFAmount + "&PVNo=" + PVNo + "&PVDate=" + PVDate + "&InwardAWBNo=" + InwardAWBNo + "&ChqAWBNo=" + ChqAWBNo + "&Employee=" + Employee + "&ChequeNo=" + ChequeNo + "&ChqueAmount=" + ChqueAmount + "&PhyReceptDateFrom=" + PhyReceptDateFrom + "&PhyReceptDateTo=" + PhyReceptDateTo + "&pono=" + pono + "&cbfNo=" + cbfNo);
    }

    //--------------------------End------------------------------------------//

    self.viewECF = function (item) {
        ShowDialog(item.ecfId, item.DocSubTypeId, item.DocNo);
    };

    self.Clear = function () {
        $("#txtSECFNumber").val("");
        $("#txtSECFDate").val("");
        $("#ddlDocType").val(0);
        $("#hdfRaiser").val("0");
        $("#txtRaiser").val("");
        $("#hdfAuthorizer").val("0");
        $("#txtAuthorizer").val("");
        //$("#txtDateFrom").val(); 
        //$("#hdfVendorCode").val("0");
        $("#hdfVendor").val("0");
        $("#txtVendor").val("");
        $("#txtSInvoiceNumber").val("");
        $("#txtSInvAmt").val("");
        $("#txtSECFAmount").val("");
        $("#txtSPVNumber").val("");
        $("#txtPVDate").val("");
        $("#txtInwAwbNo").val("");
        $("#txtChqAwbNo").val("");

        $("#txtEmployee").val("");
        $("#txtChqueNo").val("");
        $("#txtChqueAmount").val("");
        $("#txtPDateFrom").val("");
        $("#txtPDateTo").val("");
        $("#hfEmployee").val("0");
        $("#txtpo").val("");
        $("#txtcbfno").val("");
        self.QueryArray.removeAll();
        self.loadGrid();
    };

    self.loadDocumentType();

    self.DatatableCall = function () {
        if ($("#tblFSQuery > tbody > tr").length == self.QueryArray().length) {
            $("#tblFSQuery").DataTable({
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
        $("#tblFSQuery").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.QueryArray.removeAll();
    }
    self.loadGrid();
};

var mainViewModel = new QueryModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {

    objDialog = $("[id$='Dialog']");
    objDialog.dialog({
        autoOpen: false,
        modal: true,
        width: 980,
        height: 800,
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

    //Load Raiser Code Auto Complete
    $("#txtRaiser").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfRaiser").val("0");
        }

        $("#txtRaiser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoReceiptEmployee",
                    data: "{ 'txt' : '" + $("#txtRaiser").val() + "'}",
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
                $("#hdfRaiser").val(i.item.val);
                $("#txtRaiser").val(i.item.label);
            }
        });
    });

    $("#txtRaiser").focusout(function () {
        var hdfId = $("#hdfRaiser").val();
        var _data = $("#txtRaiser").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtRaiser").val("");
        }
    });

    //Load Raiser Code Auto Complete
    $("#txtEmployee").keyup(function (e) {
        if (e.which != 13) {
            $("#hfEmployee").val("0");
        }

        $("#txtEmployee").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoReceiptEmployee",
                    data: "{ 'txt' : '" + $("#txtEmployee").val() + "'}",
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
                $("#hfEmployee").val(i.item.val);
                $("#txtEmployee").val(i.item.label);
            }
        });
    });

    $("#txtEmployee").focusout(function () {
        var hdfId = $("#hfEmployee").val();
        var _data = $("#txtEmployee").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtEmployee").val("");
        }
    });

    //Load Authorizer Code Auto Complete
    $("#txtAuthorizer").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfAuthorizer").val("0");
        }

        $("#txtAuthorizer").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoReceiptEmployee",
                    data: "{ 'txt' : '" + $("#txtAuthorizer").val() + "'}",
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
                $("#hdfAuthorizer").val(i.item.val);
                $("#txtAuthorizer").val(i.item.label);
            }
        });
    });

    $("#txtAuthorizer").focusout(function () {
        var hdfId = $("#hdfAuthorizer").val();
        var _data = $("#txtAuthorizer").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtAuthorizer").val("");
        }
    });

    //Load Vendor Auto Complete
    $("#txtVendor").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfVendor").val("0");
        }

        $("#txtVendor").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteSupplierAll",
                    data: "{ 'txt' : '" + $("#txtVendor").val() + "'}",
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
                $("#hdfVendor").val(i.item.val);
                $("#txtVendor").val(i.item.label);
            }
        });
    });

    $("#txtVendor").focusout(function () {
        var hdfId = $("#hdfVendor").val();
        var _data = $("#txtVendor").val();
        if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtVendor").val("");
        }
    });

    //Load Vendor Code Auto Complete
    //$("#txtVendorCode").keyup(function (e) {
    //    if (e.which != 13) {
    //        $("#hdfVendorCode").val("0");
    //    }

    //    $("#txtVendorCode").autocomplete({
    //        source: function (request, response) {
    //            $.ajax({
    //                url: MUrlHelper + "GetAutoCompleteSupplierCode",
    //                data: "{ 'txt' : '" + $("#txtVendorCode").val() + "'}",
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
    //            $("#hdfVendorCode").val(i.item.val);
    //            $("#txtVendorCode").val(i.item.label);
    //        }
    //    });
    //});

    //$("#txtVendorCode").focusout(function () {
    //    var hdfId = $("#hdfVendorCode").val();
    //    var _data = $("#txtVendorCode").val();
    //    if (_data.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
    //        $("#txtVendorCode").val("");
    //    }
    //});

    $("#txtSECFAmount,#txtSInvAmt, #txtChqueAmount").keyup(function (event) {
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

function ShowDialog(ecfId, DocSubTypeId, ecfNo) {
    $('#Dialog').attr("style", "display:block;");

    var url = MUrlDet + 'DocumentDetails/' + ecfId + '/' + DocSubTypeId + '/' + ecfNo;
    objDialog.load(url);

    objDialog.dialog({
        title: 'ECF Details',         
        close: ReloadPage
      
    });
    objDialog.dialog("open");

     return false;
}
function ReloadPage() {
    
      
    if (objDialogyExpenseDetails != undefined)
        {
        objDialogyExpenseDetails.remove();
         }
    if (objDialogyInvoice != undefined)
    {
        objDialogyInvoice.remove();
    }
    if (objDialogyAdd != undefined)
    {
        objDialogyAdd.remove();
    }
    if (objDialogyAddDebit != undefined) {
        objDialogyAddDebit.remove();
    }
    if (objDialogyAddPayment != undefined) {
        objDialogyAddPayment.remove();
    }
    if (objDialogyAddAttachment != undefined) {
        objDialogyAddAttachment.remove();
    }
    if (objDialogyPPX != undefined) {
        objDialogyPPX.remove();
    }
    if (objDialogyCreditNote != undefined) {
        objDialogyCreditNote.remove();
    }
    if (objDialogyAmort != undefined) {
        objDialogyAmort.remove();
    }
    if (objDialogyPoMapping != undefined) {
        objDialogyPoMapping.remove();
    }
    if (objDialogyAddPayment1 != undefined) {
        objDialogyAddPayment1.remove();
    }
    if (objDialogyBenificiary != undefined) {
        objDialogyBenificiary.remove();
    }
    if (objDialogyGST != undefined) {
        objDialogyGST.remove();
    }
    if (objDialogySUS != undefined) {
        objDialogySUS.remove();
    }
    if(objDialogyAddAttachment!=undefined)
    {
         objDialogyAddAttachment.remove();  
    }
    if(objDialogyInvoiceTravel!=undefined)
    {
        objDialogyInvoiceTravel.remove();
    }
    if (objDialogyGSTTravel != undefined)
    {
        objDialogyGSTTravel.remove();
    }
    if (objDialogyInvoiceNonTravel != undefined)
        objDialogyInvoiceNonTravel.remove();

    if (objDialogyGSTNonTravel != undefined)
        objDialogyGSTNonTravel.remove();
    if (objDialogyWHTax != undefined)
        objDialogyWHTax.remove();

    if (objDialogyInvoicePC != undefined)
        objDialogyInvoicePC.remove();
    if (objDialogyGSTPC != undefined)
        objDialogyGSTPC.remove();
   
     $('#Dialog').dialog("refresh");
 
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