var objUrgentTag;
var objECFDet;
MUrlDet = MUrlDet.replace("FetchUrgentTagDetails", "");
MUrlHelper = MUrlHelper.replace("GetAutoCompleteCourier", "");
var UrgentTagModel = function () {

    var self = this;

    self.UrgenTagId = ko.observable("");
    self.DocType = ko.observable("");
    self.DocRefNo = ko.observable("");
    self.ecfId = ko.observable("");
    self.ReqById = ko.observable("");
    self.ReqByName = ko.observable("");
    self.ReqDate = ko.observable("");
    self.ReqReason = ko.observable("");

    var TagDetails = {
        UrgenTagId: self.UrgenTagId,
        DocType: self.DocType,
        DocRefNo: self.DocRefNo,
        ecfId: self.ecfId,
        ReqById: self.ReqById,
        ReqByName: self.ReqByName,
        ReqDate: self.ReqDate,
        ReqReason: self.ReqReason
    };

    self.TagDetails = ko.observable();
    self.UrgentTagging = ko.observableArray();
    self.DocDetails = ko.observableArray();

    //document type dropdown array
    self.UrgentDocumentArray = ko.observableArray();
    
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
            url: MUrlHelper + "LoadDocumentType",
            data: '{}',
            contentType: "application/json;",
            success: function (response) {
                self.UrgentDocumentArray(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.search = function (id) {
        var UrgentFilter = {
            EmpCodeId: $("#hdfSEmpCodeId").val(),
            EmpNameId: $("#hdfSEmpName").val(),
            SuppCodeId: $("#hdfSSupplierId").val(),
            SuppNameId: $("#hdfSSupplierName").val(),
            DocTypeId: $("#ddlSDocType").val(),
            DocRefNo: $("#txtSDocNumber").val(),
            ApprDate: $("#txtSAppDate").val(),
            Amount: $("#txtSAmount").val().replace(/,/g, ''),
            ViewType: 0
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "FetchUrgentTagDetails",
            async: false,
            data: JSON.stringify(UrgentFilter),
            contentType: "application/json;",
            success: function (response) {

                $("#gvUrgentTag").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();

                self.UrgentTagging.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "" && id=="0") {
                        jAlert(Data1[0].Message, "Message");
                    }
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.UrgentTagging(Data2);
                } 
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.ViewDocument = function () {
        var _tmpData = {
            DocNo: $("#lblDocNo").val()
        };
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
        if ($("#gvUrgentTag > tbody > tr").length == self.UrgentTagging().length) {
            $("#gvUrgentTag").DataTable({
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

    self.add = function (TagDetails) {
        $('#btnUpdate').attr('disabled', true);
        $("#txtRequestDate").removeAttr("disabled");
        $("#txtRequestBy").removeAttr("disabled");
        
        if ($("#txtRequestDate").hasClass("valid")) {            
            $("#txtRequestDate").removeClass("valid");
            $("#txtRequestDate").addClass("required");
        }
        
        if ($("#txtRequestBy").hasClass("valid")) {
            $("#txtRequestBy").removeClass("valid");
            $("#txtRequestBy").addClass("required");
        }

        if (!$("#txtRequestDate").hasClass("required")) {
            $("#txtRequestDate").addClass("required");
        }

        if (!$("#txtRequestBy").hasClass("required")) {
            $("#txtRequestBy").addClass("required");
        }

        $("#hdfUTagId").val("");
        $("#hdfECFId").val(TagDetails.ecfId);
        $("#lblDocType").val(TagDetails.DocType);
        $("#lblDocNo").val(TagDetails.DocRefNo);
        $("#txtRequestDate").val("");
        $("#hdfRequestId").val("");
        $("#txtRequestBy").val("");
        $("#txtRemarks").val("");

        self.TagDetails(null);

        ShowUrgentTagDialog();
    };

    self.edit = function (TagDetails) {
        $("#txtRequestDate").attr('disabled', true);
        $('#btnUpdate').attr('disabled', false);
        
        $("#txtRequestBy").attr("disabled", "disabled");

        if ($("#txtRequestBy").hasClass("required")) {
            $("#txtRequestBy").removeClass("required");
        }

        if ($("#txtRequestDate").hasClass("required")) {
            $("#txtRequestDate").removeClass("required");
        }

        if ($("#txtRequestBy").hasClass("valid")) {
            $("#txtRequestBy").removeClass("valid");
        }

        if ($("#txtRequestDate").hasClass("valid")) {
            $("#txtRequestDate").removeClass("valid");
        }

        $("#hdfUTagId").val(TagDetails.UrgenTagId);
        $("#hdfECFId").val(TagDetails.ecfId);
        $("#lblDocType").val(TagDetails.DocType);
        $("#lblDocNo").val(TagDetails.DocRefNo);
        $("#txtRequestDate").val(TagDetails.ReqDate);
        $("#hdfRequestId").val(TagDetails.ReqById);
        $("#txtRequestBy").val(TagDetails.ReqByName);
        $("#txtRemarks").val(TagDetails.ReqReason);

        self.TagDetails(null);
        ShowUrgentTagDialog();
    };

    self.update = function () {
        var data = {
            UTagId: $("#hdfUTagId").val(),
            EcfId: $("#hdfECFId").val(),
            ReqBy: $("#hdfRequestId").val(),
            ReqDate: $("#txtRequestDate").val(),
            Remarks: $("#txtRemarks").val(),
            IsRemoved: 0
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "SetUrgentTag",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);

                //show Message if error
                if (Data1[0].Message != "") {
                    jAlert(Data1[0].Message, "Message");
                }
                if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {

                    $("#hdfUTagId").val("0");
                    $("#hdfECFId").val("0");
                    $("#lblDocType").val("");
                    $("#lblDocNo").val("");
                    $("#txtRequestDate").val("");
                    $("#hdfRequestId").val("0");
                    $("#txtRequestBy").val("");
                    $("#txtRemarks").val("");

                    self.TagDetails(null);
                    
                    objUrgentTag.dialog("close");
                    self.search("0");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.delete = function (UrgenTagId, ecfId) {
        jConfirm("Do you want to delete the record?", "Message", function (callback) {
            if (callback == true) {
                $("#hdfUTagId").val(UrgenTagId);
                $("#hdfECFId").val(ecfId);
                var data = {
                    UTagId: $("#hdfUTagId").val(),
                    EcfId: $("#hdfECFId").val(),
                    ReqBy: '',
                    ReqDate: '',
                    Remarks: '',
                    IsRemoved: 1
                };
                $.ajax({
                    type: "post",
                    url: MUrlDet + "SetUrgentTag",
                    data: JSON.stringify(data),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                            Data1 = JSON.parse(response.Data1);

                        //show Message if error
                        if (Data1[0].Message != "") {
                            jAlert(Data1[0].Message, "Message");
                        }
                        if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                            $("#hdfUTagId").val("0");
                            $("#hdfECFId").val("0");
                            $("#lblDocType").val("");
                            $("#lblDocNo").val("");
                            $("#txtRequestDate").val("");
                            $("#hdfRequestId").val("0");
                            $("#txtRequestBy").val("");
                            $("#txtRemarks").val("");

                            self.TagDetails(null);
                            
                            self.search("0");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //alert(errorThrown);
                    }
                });
            } else {
                return false;
            }
        });
    }

    self.clearFilter = function () {
        $("#hdfSEmpCodeId").val("0");
        $("#txtSEmpCode").val("");
        $("#hdfSEmpName").val("");
        $("#txtSSupplierCode").val("");
        $("#txtSSupplierName").val("");
        $("#txtSEmpName").val("");
        $("#hdfSSupplierId").val("0");
        $("#hdfSSupplierName").val("0");
        $("#ddlSDocType").val(0);
        $("#txtSDocNumber").val("");
        $("#txtSAppDate").val("");
        $("#txtSAmount").val("");

        self.search("1");
    };

    //load the Doctument Type DropDown.
    self.loadDocType();
    self.search("1");
};

var mainViewModel = new UrgentTagModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {
    objUrgentTag = $("[id$='UrgentTagEntry']");
    objUrgentTag.dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 300,
        duration: 300
    });

    objECFDet = $("[id$='ShowECFDet']");
    objECFDet.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 800,
        duration: 300
    });    

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $("#txtRequestDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $("#txtSAppDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    //Load Request By Auto Complete
    $("#txtRequestBy").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfRequestId").val("0");
        }

        $("#txtRequestBy").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtRequestBy").val() + "'}",
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
                $("#hdfRequestId").val(i.item.val);
                $("#txtRequestBy").val(i.item.label);

                var _Id = $("#hdfRequestId").val();
                if (_Id.trim() != "" && _Id!="0") {

                    $("#txtRequestBy").removeClass('required');
                    $("#txtRequestBy").addClass('valid');
                }
                else {
                    $("#txtRequestBy").removeClass('valid');
                    $("#txtRequestBy").addClass('required');
                }
            }
        });

    });

    $("#txtRequestDate").change(function () {
        checkValidAdd();
        var txtDate = $("#txtRequestDate").val();
        if (txtDate.trim() != "") {
            $("#txtRequestDate").removeClass('required');
            $("#txtRequestDate").addClass('valid');
        }
        else {
            $("#txtRequestDate").removeClass('valid');
            $("#txtRequestDate").addClass('required');
        }
    });

    $("#txtRequestBy").focusout(function () {
        checkValidAdd();
        var hdfId = $("#hdfRequestId").val();
        var txtRequestBy = $("#txtRequestBy").val();
        if (txtRequestBy.trim() != "" && hdfId.trim() != "" && hdfId.trim() != "0") {
            $("#txtRequestBy").removeClass('required');
            $("#txtRequestBy").addClass('valid');
        }
        else {
            $("#txtRequestBy").removeClass('valid');
            $("#txtRequestBy").addClass('required');
        }

        if ($("#txtRequestBy").hasClass("required")) {
            $("#txtRequestBy").val("");
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

function ShowUrgentTagDialog() {
    $('#UrgentTagEntry').attr("style", "display:block;");

    objUrgentTag.dialog({ title: 'Urgent Process Entry', width: '600', height: '300' });
    objUrgentTag.dialog("open");
    return false;
}

function ShowECFDet(ecfId, DocSubTypeId) {
    /*$('#ShowECFDet').attr("style", "display:block;");

    objECFDet.dialog({ title: 'Document Detail', width: '500', height: '250' });
    objECFDet.dialog("open");*/
    $('#ShwECFDet').attr("style", "display:block;");

    var url = MUrlDet + 'DocumentDetails/' + ecfId + '/' + DocSubTypeId;
    objECFDet.load(url);
    objECFDet.dialog({ title: 'Document Detail' });
    objECFDet.dialog("open");
    return false;
}


function checkValidAdd() {
    var _reqId = $("#hdfRequestId").val();
    var _date = $("#txtRequestDate").val();
    

    if (_reqId.trim() == "" || _reqId == "0" || _date.trim() == "" ) {
        $('#btnUpdate').attr('disabled', true);
    }

    else {
        $('#btnUpdate').attr('disabled', false);
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