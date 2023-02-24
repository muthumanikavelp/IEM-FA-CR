var objInex;
var objECFDet;
MUrlDet = MUrlDet.replace("FetchInexDetails", "");
MUrlHelper = MUrlHelper.replace("GetAutoCompleteCourier", "");
var InexReviewModel = function () {

    var self = this;

    self.inexId = ko.observable("");
    self.DocType = ko.observable("");
    self.DocNo = ko.observable("");
    self.InexDate = ko.observable("");
    self.InexByName = ko.observable("");    
    
    var InexDetails = {
        inexId: self.inexId,
        DocType: self.DocType,
        DocNo: self.DocNo,
        InexDate: self.InexDate,
        InexByName: self.InexByName        
    };
        
    self.InexReviewArray = ko.observableArray();
    self.DocDetails = ko.observableArray();
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

    self.search = function () {
        var InexFilter = {
            InexFrom: $("#txtSInexFrom").val(),
            InexTo: $("#txtSInexTo").val(),
            InexById: $("#hdfSInexById").val(),
            DocAmount: $("#txtSDocAmount").val().replace(/,/g, ''),
            DocFrom: $("#txtSDocFrom").val(),
            DocTo: $("#txtSDocTo").val(),
            DocTypeId: $("#ddlSDocType").val(),
            DocNo: $("#txtSDocNumber").val(),
            EmpCodeId: $("#hdfSEmpCodeId").val(),
            EmpNameId: $("#hdfSEmpName").val(),
            SuppCodeId: $("#hdfSSupplierId").val(),
            SuppNameId: $("#hdfSSupplierName").val(),
            ViewType: 0
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "FetchInexDetails",
            data: JSON.stringify(InexFilter),
            contentType: "application/json;",
            success: function (response) {

                $("#gvInexSummary").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.InexReviewArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.InexReviewArray(Data2);
                }

                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (self.InexReviewArray().length == 0 && Data1[0].Message == "") {
                        jAlert("Sorry! No Records Found.", "Message");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    //-----------------------Coded By Subburaj  19.04.2016---------------------------------//

    this.DownloadInexreviewReport = function () {
      
        var InexFrom = $("#txtSInexFrom").val();
        var InexTo = $("#txtSInexTo").val();
        var InexById = $("#hdfSInexById").val();
        var DocAmount = $("#txtSDocAmount").val();
        var DocFrom = $("#txtSDocFrom").val();
        var DocTo = $("#txtSDocTo").val();
        var DocTypeId = $("#ddlSDocType").val();
        var DocNo = $("#txtSDocNumber").val();
        var EmpCodeId = $("#hdfSEmpCodeId").val();
        var EmpNameId = $("#hdfSEmpName").val();
        var SuppCodeId = $("#hdfSSupplierId").val();
        var SuppNameId = $("#hdfSSupplierName").val();
        var ViewType = '0';

        ko.utils.postJson(MUrlDet + "DownloadInexreviewReport?InexFrom=" + InexFrom + "&InexTo=" + InexTo + "&InexID=" + InexById + "&Docamount=" + DocAmount + "&Docfrom=" + DocFrom + "&Docto=" + DocTo + "&doctypeid=" + DocTypeId + "&Docno=" + DocNo + "&empcodeid=" + EmpCodeId + "&Empnameid=" + EmpNameId + "&suppcodeid=" + SuppCodeId + "&suppnameid=" + SuppNameId + "&Viewtype=" + ViewType);
    }

    //--------------------------End------------------------------------------//

    self.review = function (InexDetails) {        
        $("#hdfInexId").val("");
        $("#hdfInexId").val(InexDetails.inexId);
        $("#lblDocType").val(InexDetails.DocType);
        $("#lblDocNo").val(InexDetails.DocNo);
        $("#txtInexDate").val(InexDetails.InexDate);
        $("#txtInexBy").val(InexDetails.InexByName);
        $("#rdApprove").prop("checked",true);
        $("#txtRemarks").val("");

        $("#txtRemarks").removeClass('required').removeClass('valid');
        $("#txtRemarks").addClass('required');

        ShowInexDialog();
    };

    self.updateReview = function () {
        var data = {
            InexId: $("#hdfInexId").val(),
            Status: $("input[name=rdStatus]:checked").val(),
            Remarks: $("#txtRemarks").val()
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "UpdateInexReview",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    
                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                   
                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        //self.InexReviewArray.removeAll();                        
                        objInex.dialog("close");
                        self.search();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.clearDetails = function () {
        $("#txtSInexFrom").val("");
        $("#txtSInexTo").val("");
        $("#hdfSInexById").val("0");
        $("#txtSInexBy").val("");
        $("#txtSDocAmount").val("");
        $("#txtSDocFrom").val("");
        $("#txtSDocTo").val("");
        $("#ddlSDocType").val("");
        $("#txtSDocNumber").val("");

        $("#hdfSEmpCodeId").val("0");
        $("#txtSEmpCode").val("");

        $("#hdfSEmpName").val("0");
        $("#txtSEmpName").val("");

        $("#hdfSSupplierId").val("0");
        $("#txtSSupplierCode").val("");

        $("#hdfSSupplierName").val("0");
        $("#txtSSupplierName").val("");

        $("#gvInexSummary").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.InexReviewArray.removeAll();
    };

    self.ViewDocument = function () {
        var _tmpData = {
            DocNo: $("#lblDocNo").val()
        };

        if ($.trim($("#lblDocNo").val()) == "") {
            jAlert("Ensure Document Number!", "Message");
            return false;
        }

        var ecfId = "0", DocSubTypeId = "0";

        $.ajax({
            type: "post",
            url: MUrlHelper + "GetDocumentECFDetails",
            data: JSON.stringify(_tmpData),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                if (response.ecfId != null && response.ecfId != "0" && JSON.parse(response.ecfId) != null) {
                    ecfId = response.ecfId;
                }
                if (response.docSubTypeId != null && response.docSubTypeId != "0" && JSON.parse(response.docSubTypeId) != null) {
                    DocSubTypeId = response.docSubTypeId;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
                ecfId = "0";
                DocSubTypeId = "0";
            }
        });

        if (ecfId == "0" || DocSubTypeId == "0") {
            jAlert("Invalid Document Number!", "Message");
            return false;
        } else {
            ShowECFDet(ecfId, DocSubTypeId);
        }
    };

    self.DatatableCall = function () {
        if ($("#gvInexSummary > tbody > tr").length == self.InexReviewArray().length) {
            $("#gvInexSummary").DataTable({
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
        $("#gvInexSummary").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.InexReviewArray.removeAll();
    }

    //load the Doctument Type DropDown.
    self.loadDocType();

    self.loadGrid();
};

var mainViewModel = new InexReviewModel();
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

    objInex = $("[id$='ShowInexDialog']");
    objInex.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 350,
        duration: 300
    });
    
    objECFDet = $("[id$='ShwECFDet']");
    objECFDet.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
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

    //Load Employee Code Auto Complete
    $("#txtSEmpCode").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSEmpCodeId").val("0");
        }

        $("#txtSEmpCode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteEmployeeCode",
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
                    url: MUrlHelper + "GetAutoCompleteSupplierCode",
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
                    url: MUrlHelper + "GetAutoCompleteEmployee",
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
                    url: MUrlHelper + "GetAutoCompleteSupplier",
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

    //Load Employee Name Auto Complete
    $("#txtSInexBy").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSInexById").val("0");
        }

        $("#txtSInexBy").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtSInexBy").val() + "'}",
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
                $("#hdfSInexById").val(i.item.val);
                $("#txtSInexBy").val(i.item.label);
            }
        });
    });

    $("#txtSInexBy").focusout(function () {
        var hdfId = $("#hdfSInexById").val();
        var txtCurName = $("#txtSInexBy").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSInexBy").val("");
        }
    });

    $("#txtRemarks").keyup(function () {
        chkReview();
        var remarks = $("#txtRemarks").val();
        if (remarks.trim() != "") {
            $("#txtRemarks").removeClass('required');
            $("#txtRemarks").addClass('valid');
        }
        else {
            $("#txtRemarks").removeClass('valid');
            $("#txtRemarks").addClass('required');
        }
    });

    $("#txtSDocAmount").keyup(function (event) {
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

function ShowInexDialog() {
    $('#ShowInexDialog').attr("style", "display:block;");

    $('#btnSummaryReview').attr('disabled', true);
    
    objInex.dialog({ title: 'Inex Review', width: '600', height: '350' });
    objInex.dialog("open");
    return false;
}

function ShowECFDet(ecfId, DocSubTypeId) {
    $('#ShwECFDet').attr("style", "display:block;");

    var url = MUrlDet + 'DocumentDetails/' + ecfId + '/' + DocSubTypeId;
    objECFDet.load(url);
    objECFDet.dialog({ title: 'Document Detail' });
    objECFDet.dialog("open");

    //objDialogyDocDet.dialog({ title: 'Document Detail', width: '500', height: '250' });
    //objDialogyDocDet.dialog("open");
    return false;
}



function chkReview() {
    var _Remarks = $("#txtRemarks").val();

    if (_Remarks == "") {
        $('#btnSummaryReview').attr('disabled', true);
    } else {
        $('#btnSummaryReview').attr('disabled', false);
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}