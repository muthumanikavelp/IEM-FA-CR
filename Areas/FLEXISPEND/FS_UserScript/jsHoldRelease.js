var objShowHoldData;
var objECFDet;

MUrlDet = MUrlDet.replace("FetchHoldDetails", "");
MUrlHelper = MUrlHelper.replace("GetAutoCompleteCourier", "");

var HoldReprocessModel = function () {

    var self = this;
    self.HoldId = ko.observable("");
    self.DocType = ko.observable("");
    self.DocNo = ko.observable("");
    self.HoldDate = ko.observable("");
    self.HoldByName = ko.observable("");
    self.HoldReason = ko.observable("");

    var HoldDetails = {
        HoldId: self.HoldId,
        DocType: self.DocType,
        DocNo: self.DocNo,
        HoldDate: self.HoldDate,
        HoldByName: self.HoldByName,
        HoldReason: self.HoldReason
    };

    self.HoldReleaseArray = ko.observableArray();

    //document type dropdown array
    self.DocumentTypeArray = ko.observableArray();

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
        var HoldFilter = {
            DocType: $("#ddlSDocType").val(),
            DocNo: $("#txtSDocNumber").val(),
            DocStatus: $("#txtSDocStatus").val(),
            HoldDate: $("#txtSHoldDate").val(),
            HoldBy: $("#hdfSHoldById").val(),
            Reason: $("#txtSReason").val()
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "FetchHoldDetails",
            async: false,
            data: JSON.stringify(HoldFilter),
            contentType: "application/json;",
            success: function (response) {

                $("#gvholdReprocess").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.HoldReleaseArray.removeAll();

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
                    self.HoldReleaseArray(Data2);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#gvholdReprocess > tbody > tr").length == self.HoldReleaseArray().length) {
            $("#gvholdReprocess").DataTable({
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

    self.reprocess = function (HoldDetails) {
        $("#hdfHoldId").val("");
        $("#hdfHoldId").val(HoldDetails.HoldId);

        $("#txtDocType").val(HoldDetails.DocType);
        $("#txtDocNo").val(HoldDetails.DocNo);
        $("#txtHoldDate").val(HoldDetails.HoldDate);
        $("#txtHoldBy").val(HoldDetails.HoldByName);
        $("#txtHoldReason").val(HoldDetails.HoldReason);

        $("#txtHoldRemarks").val("");
        $("#txtHoldReleaseDate").val("");
        $("#attUploader").val("");

        $('#btnSubmit').attr('disabled', true);
                
        $("#txtHoldReleaseDate").removeClass('required').removeClass('valid');
        $("#txtHoldReleaseDate").addClass('required');

        $("#txtHoldRemarks").removeClass('required').removeClass('valid');
        $("#txtHoldRemarks").addClass('required');

        ShowHoldData();
    };

    self.updateHold = function () {
        var data = {
            Hold_id:$("#hdfHoldId").val(),
            ReleaseDate:$("#txtHoldReleaseDate").val(),
            Remarks:$("#txtHoldRemarks").val()
        };
        $.ajax({
            type: "post",
            url: MUrlDet + "SetHoldRelease",
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
                        objShowHoldData.dialog("close");
                        self.search();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.ViewDocument = function () {
        var _tmpData = {
            DocNo: $("#txtDocNo").val()
        };
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

    self.clearDetails = function () {
        $("#ddlSDocType").val("");
        $("#txtSDocNumber").val("");
        $("#txtSDocStatus").val("");
        $("#txtSHoldDate").val("");
        $("#hdfSHoldById").val("0");
        $("#txtSHoldBy").val("");
        $("#txtSReason").val("");

        $("#gvholdReprocess").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.HoldReleaseArray.removeAll();
    };

    self.showDet = function () {
        ShowHoldData();
    };

    self.loadGrid = function () {
        $("#gvholdReprocess").DataTable({
            "autoWidth": false,
            "destroy": true
        }).destroy();
        self.HoldReleaseArray.removeAll();
    }

    //load the Doctument Type DropDown.
    self.loadDocType();

    self.loadGrid();
};

var mainViewModel = new HoldReprocessModel();
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

    objShowHoldData = $("[id$='ShowHoldData']");
    objShowHoldData.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 250,
        duration: 300
    });

    objECFDet = $("[id$='ShwECFDet']");
    objECFDet.dialog({
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 550,
        duration: 300
    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();
    $(".datePicker").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        dateFormat: 'dd/mm/yy'
    });

    $("#txtHoldReleaseDate").change(function () {        
        checkValidation();
        var txtDate = $("#txtHoldReleaseDate").val();
        if (txtDate.trim() != "") {
            $("#txtHoldReleaseDate").removeClass('required');
            $("#txtHoldReleaseDate").addClass('valid');
        }
        else {
            $("#txtHoldReleaseDate").removeClass('valid');
            $("#txtHoldReleaseDate").addClass('required');
        }
    });

    $("#txtHoldRemarks").keyup(function () {
        checkValidation();

        var _remarks = $("#txtHoldRemarks").val();
        if (_remarks.trim() != "") {
            $("#txtHoldRemarks").removeClass('required');
            $("#txtHoldRemarks").addClass('valid');
        }
        else {
            $("#txtHoldRemarks").removeClass('valid');
            $("#txtHoldRemarks").addClass('required');
        }
    });

    //Load Hold By Auto Complete
    $("#txtSHoldBy").keyup(function (e) {
        if (e.which != 13) {
            $("#hdfSHoldById").val("0");
        }

        $("#txtSHoldBy").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: MUrlHelper + "GetAutoCompleteEmployee",
                    data: "{ 'txt' : '" + $("#txtSHoldBy").val() + "'}",
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
                $("#hdfSHoldById").val(i.item.val);
                $("#txtSHoldBy").val(i.item.label);
            }
        });
    });

    $("#txtSHoldBy").focusout(function () {
        var hdfId = $("#hdfSHoldById").val();
        var txtCurName = $("#txtSHoldBy").val();
        if (txtCurName.trim() != "" && (hdfId.trim() == "" || hdfId.trim() == "0")) {
            $("#txtSHoldBy").val("");
        }
    });

    $(".attUploader").on('change', function (e) {
        var Attach_list = Attachment_list();
        var Attachment_fomat = Attached_fileformat();  //Pandiaraj 18-11-2019 
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        //form.attr("action", MUrlDet + "UploadAttachment");
        form.attr("action", UrlDet + "UploadAttachment/?attach=" + Attach_list + '&attaching_format=' + Attachment_fomat);  //Pandiaraj 18-11-2019 
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
        return false;
    });
});

function ShowHoldData() {
    $('#ShowHoldData').attr("style", "display:block;");

    objShowHoldData.dialog({ title: 'Hold Release Entry', width: '800', height: '250' });
    objShowHoldData.dialog("open");
    return false;
}

function ShowECFDet(ecfId, DocSubTypeId) {
    $('#ShwECFDet').attr("style", "display:block;");

    var url = MUrlDet + 'DocumentDetails/' + ecfId + '/' + DocSubTypeId;
    objECFDet.load(url);
    objECFDet.dialog({ title: 'Document Detail' });
    objECFDet.dialog("open");
    return false;
}

function checkValidation() {
    var _dt = $("#txtHoldReleaseDate").val();
    var _remarks = $("#txtHoldRemarks").val();
    
    if (_dt.trim() == "" || _remarks.trim()=="") {
        $('#btnSubmit').attr('disabled', true);
    }
    else {
        $('#btnSubmit').attr('disabled', false);
    }
}

function isEvent(evt) {
    return false;
}