var objInexDet, objECFDoc, objDialogyAddAttachment1;

UrlDet = UrlDet.replace("GetSupplierInvoiceMaker", ""); 
UrlDet1 = UrlDet1.replace("GetInexDetails", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");

var InexReviewDispatchModel = function () {
    var self = this;

    self.InexReviewArray = ko.observableArray();
    self.ECFAttachmentArray = ko.observableArray();
    self.InexReasonArray = ko.observableArray();
    self.AttachmentTypeArray = ko.observableArray();

    this.GetInexDocument = function () {
        var ViewType = $("#hfViewType").val();
        var InexFilter = {
            ViewType: ViewType
        };
        $.ajax({
            type: "post",
            url: UrlDet1 + "GetInexDetails",
            data: JSON.stringify(InexFilter),
            contentType: "application/json;",
            success: function (response) {

                $("#gvDispatch").DataTable({
                    "autoWidth": false,
                    "scrollX": true,
                    "destroy": true
                }).destroy();
                self.InexReviewArray.removeAll();

                var Data1 = "", Data2 = "";
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.InexReviewArray(Data2);

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

    self.InexDetails = function (item) {
        $("#hdfRPInexId").val(item.InexGId);
        $("#hdfEcfId").val(item.EcfGId);
        $("#lblDocType").val(item.DocType);
        $("#lblDocNo").val(item.DocNo);
        $("#txtInexDate").val(item.InexDate);
        $("#txtInexBy").val(item.InexByName);
        $("#txtInexReason").val(item.InexReason);

        var InexFilter = {
            ECFGId: item.EcfGId
        };
        $.ajax({
            type: "post",
            url: UrlDet1 + "GetInexReason",
            data: JSON.stringify(InexFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.ECFAttachmentArray(Data1);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.InexReasonArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
        InexDet();
    };

    self.ViewDocument = function () {
        var _tmpData = {
            DocNo: $("#lblDocNo").val()
        };
        var ecfId = "0", DocSubTypeId = "0";
        $.ajax({
            type: "post",
            url: UrlHelper + "GetDocumentECFDetails",
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
            ECFDoc(ecfId, DocSubTypeId);
        }
    };

    self.DatatableCall = function () {
        if ($("#gvDispatch > tbody > tr").length == self.InexReviewArray().length) {
            $("#gvDispatch").DataTable({
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
        $("#gvDispatch").DataTable({
            "autoWidth": false,
            "scrollX": true,
            "destroy": true
        }).destroy();
        self.InexReviewArray.removeAll();
    }

    this.LoadAttachmentType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetAttachmentType",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.AttachmentTypeArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.CloseDetails = function (flag) {
        if (flag == 1)
            objInexDet.dialog("close");
        if (flag == 2)
            objDialogyAddAttachment1.dialog("close");
    };

    this.SetAttachmentDetails = function () {

        var EcfId = $("#hdfEcfId").val();
        var AttachmentId = $("#hfAttachmetId").val();
        var AttachmentName = "";
        var AttachmentType = $("#ddlAttachmentType1").val();
        var Desc = $("#txtAttachDescription1").val();
        var IsRemoved = "0";
        var Attachment = {
            EcfId: EcfId,
            AttachmentId: AttachmentId,
            AttachmentName: AttachmentName,
            AttachmentType: AttachmentType,
            Desc: Desc,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetAttachmentDetails",
            data: JSON.stringify(Attachment),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        objDialogyAddAttachment1.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (Data1[0].Clear == true)
                    self.ECFAttachmentArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.DeleteAttachmentDetails = function (AttachmentId) {

        var EcfId = $("#hdfEcfId").val();
        var AttachmentName = "";
        var AttachmentType = "";
        var Desc = "";
        var IsRemoved = "1";
        var Attachment = {
            EcfId: EcfId,
            AttachmentId: AttachmentId,
            AttachmentName: AttachmentName,
            AttachmentType: AttachmentType,
            Desc: Desc,
            IsRemoved: IsRemoved
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetAttachmentDetails",
            data: JSON.stringify(Attachment),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false)
                        jAlert(Data1[0].Msg, "Message");
                    else {
                        objDialogyAddAttachment.dialog("close");
                        jAlert(Data1[0].Msg, "Message");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                if (Data1[0].Clear == true)
                    self.ECFAttachmentArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    this.AttachmentDownLoad = function (Id, FileName) {
	var FileName = FileName.replace("&", "-");
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        var FormattedFileName = FileName.replace("&", "-");
        form.attr("action", UrlDet + "Download?id=" + Id + "&FileName=" + FormattedFileName);
        //form.attr("action", UrlDet + "Download?id=" + Id + "&FileName=" + FileName);
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
    };

    self.Reprocess = function () {
        var InexId = $("#hdfRPInexId").val();
        var Remarks = $("#txtRPRemarks").val();

        if ($.trim(InexId) == "0" || $.trim(InexId) == "") {
            jAlert("Ensure Valid Data!", "Message");
            return false;
        }

        if ($.trim(Remarks) == "") {
            jAlert("Ensure Remarks!", "Message");
            return false;
        }
        var data = {
            InexId: InexId,
            Remarks: Remarks
        };
        $.ajax({
            type: "post",
            url: UrlDet1 + "DispatchReprocessInex",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }
                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        objInexDet.dialog("close");
                        self.GetInexDocument();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.loadGrid();

    self.GetInexDocument();
    self.LoadAttachmentType();


};

var mainViewModel = new InexReviewDispatchModel();
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

    objInexDet = $("[id$='ShowInexDialog']");
    objInexDet.dialog({
        autoOpen: false,
        modal: true,
        width: 900,
        height: 375,
        duration: 300
    });

    objECFDoc = $("[id$='ShwECFDet']");
    objECFDoc.dialog({
        autoOpen: false,
        modal: true,
        width: 1100,
        height: 550,
        duration: 300
    });

    objDialogyAddAttachment1 = $("[id$='objDialogyAddAttachment']");
    objDialogyAddAttachment1.dialog({
        autoOpen: false,
        modal: true,
        width: 560,
        height: 300,
        duration: 300
    });

    $("#ddlAttachmentType1").change(function () {
        var AttachmentType = $("#ddlAttachmentType1").val();
        if (AttachmentType != "0") {
            $("#ddlAttachmentType1").removeClass("required");
            $("#ddlAttachmentType1").addClass("valid");
        }
        else {
            $("#ddlAttachmentType1").removeClass("valid");
            $("#ddlAttachmentType1").addClass("required");
        }
    });

    $("#txtAttachDescription1").keyup(function () {
        var Desc = $.trim($("#txtAttachDescription1").val());
        if (Desc != "") {
            $("#txtAttachDescription1").removeClass("required");
            $("#txtAttachDescription1").addClass("valid");
        }
        else {
            $("#txtAttachDescription1").removeClass("valid");
            $("#txtAttachDescription1").addClass("required");
        }
    });

    $("#txtRPRemarks").keyup(function () {
        var Desc = $.trim($("#txtRPRemarks").val());
        if (Desc != "") {
            $("#txtRPRemarks").removeClass("required");
            $("#txtRPRemarks").addClass("valid");
        }
        else {
            $("#txtRPRemarks").removeClass("valid");
            $("#txtRPRemarks").addClass("required");
        }
    });


    $(".attUploader").on('change', function (e) {
        var Attach_list = Attachment_list();
        var Attachment_fomat = Attached_fileformat();  //Pandiaraj 18-11-2019 
        var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#uploaderForm');
        //form.attr("action", UrlDet + "UploadAttachment");
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

function InexDet() {

    objInexDet.dialog({ title: 'Inex Details', width: '1100', height: '560' });
    objInexDet.dialog("open");
    return false;
}

function AddAttachment1() {
    $('#ShowDialog3').attr("style", "display:block;");

    if ($("#ddlAttachmentType1").hasClass("valid")) {
        $("#ddlAttachmentType1").removeClass("valid");
        $("#ddlAttachmentType1").addClass("required");
    }
    if ($("#txtAttachDescription1").hasClass("valid")) {
        $("#txtAttachDescription1").removeClass("valid");
        $("#txtAttachDescription1").addClass("required");
    }

    objDialogyAddAttachment1.dialog({ title: 'Attachment Details', width: '560', height: '300' });
    $("#attUploader").replaceWith($("#attUploader").val('').clone(true));
    $("#ddlAttachmentType1").val("0");
    $("#txtAttachDescription1").val("");
    objDialogyAddAttachment1.dialog("open");
    return false;
}

function ECFDoc(ecfId, DocSubTypeId) {
    $('#ShwECFDet').attr("style", "display:block;");

    var url = 'DocumentDetails/' + ecfId + '/' + DocSubTypeId;
    objECFDoc.load(url);
    objECFDoc.dialog({ title: 'Document Detail' });
    objECFDoc.dialog("open");
    return false;
}
