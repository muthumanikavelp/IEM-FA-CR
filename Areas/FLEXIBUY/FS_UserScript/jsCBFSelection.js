var objDialogyCBFDetailsAttachment;

UrlDet = UrlDet.replace("CBFSelection", "");
UrlHelper = UrlHelper.replace("GetProjectOwner", "");


debugger
// var token1 = document.getElementById('antiForgeryForm').childNodes[1].value;
var token = $('[name=__RequestVerificationToken]').val();//$("#txtantiforgery").val();
//alert(token);
debugger
$.ajaxSetup({
    cache: false,
    headers: { "__RequestVerificationToken": token }
});

var SearchModel = function (id) {

    var self = this;

    self.POCBFHeaderArray = ko.observableArray();
    self.POCBFDetailsArray = ko.observableArray();
    self.POCBFDetailsArray1 = ko.observableArray();
    self.CBFDetailsAttachmentArray = ko.observableArray();

    self.RequestByArray = ko.observableArray();

    this.GetCBFSearch = function (flag) {

        if (flag == "0") {
            $("#txtCBFNo").val("");
            $("#txtCBFDate").val("");
            $("#txtCBFEndDate").val("");
            $("#ddlDepartment").val("0");
        }

        var CBFNo = $("#txtCBFNo").val();
        var CBFDateFrom = $("#txtCBFDate").val();
        var CBFDateTo = $("#txtCBFEndDate").val();
        var CBFReqFor = $("#ddlDepartment").val();

        var inputFilter = {
            CBFNo: CBFNo,
            CBFDateFrom: CBFDateFrom,
            CBFDateTo: CBFDateTo,
            CBFReqFor: CBFReqFor
        };
        $.ajax({
            type: "post",
            async: false,
            url: UrlDet + "GetPOCBFHeader",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "";
                $("#CBFHeaderGrid").DataTable({
                    "autoWidth": false,
                    "destroy": true
                }).destroy();
                self.POCBFHeaderArray([]);
                                
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null)
                    Data2 = JSON.parse(response.Data2);
                self.POCBFHeaderArray(Data2);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.POCBFSelect = function (CBFGId) {
        $("#hfCBF").val(CBFGId);
        $("#CBFHeaderGrid > tbody > tr").removeClass("ColorSelect");
        $("#CBFHeader" + CBFGId).addClass("ColorSelect");

        var inputFilter = {
            CBFHeaderGId: CBFGId
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetPOCBFDetails",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {                   
                    Data1 = JSON.parse(response.Data1); 
                } 
                self.POCBFDetailsArray1(Data1); //Pandiaraj 21-05-2019
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.SubmitCBFSelection = function () {

        var CBFDetailGIds = "";
        $("#CBFDetailsGrid > tbody > tr > td > input").each(function () {
            if ($(this).is(":checked"))
                CBFDetailGIds = CBFDetailGIds + $(this).attr("id").replace("rdoCBFDetails", "") + "|"
        });
        if ($.trim(CBFDetailGIds) == "") {
            jAlert("Select Atleast One CBF Details!", "Message");
            return false;
        }
        var inputFilter = {
            CBFDetailGIds: CBFDetailGIds
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SetPOCBF",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    if (Data1[0].Clear == false) {
                        jAlert(Data1[0].Message, "Message");
                        return false;
                    }
                    else {
                        location = UrlSuccess + "/" + Data1[0].POHeaderGId;
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });

    };

    this.POCBFDetailSelect = function (item) {
        if ($("#rdoCBFDetails" + item.CBFDetailsId).is(":checked")) {
            $("#CBFDetails" + item.CBFDetailsId).addClass("ColorSelect");
            //  var dd = dd.push($("#CBFDetails" + item.CBFDetailsId));         //Pandiaraj 21-05-2019
           
            if (self.POCBFDetailsArray.length > 0)
                self.POCBFDetailsArray.push(item);
            else
                self.POCBFDetailsArray.push(item); 
        }
        else {
            $("#CBFDetails" + item.CBFDetailsId).removeClass("ColorSelect");
            self.POCBFDetailsArray.remove(this);
        } 
        return true;
    };

    this.CBFDetailsBOQ = function (CBFDetailsId) {

        self.CBFDetailsAttachmentArray("");
        var inputFilter = {
            RefGId: CBFDetailsId,
            RefFlag: "3",
            AttachTypeGId: "3"
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetAttachment",
            data: JSON.stringify(inputFilter),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.CBFDetailsAttachmentArray(Data1);
                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
        $('#CBFDetailsAttachment').attr("style", "display:block;");
        objDialogyCBFDetailsAttachment.dialog({ title: 'CBF Details Attachment', width: '800', height: '350' });
        objDialogyCBFDetailsAttachment.dialog("open");

    };

    this.AttachmentDownLoad = function (AttachmentGId, FileName) {
        var iframe = $('<iframe name="postiframe1" id="postiframe1" style="display: none"></iframe>');
        $("body").append(iframe);
        var form = $('#frmDeactivation');
        form.attr("action", UrlDet + "Download?Id=" + AttachmentGId + "&FileName=" + FileName);
        form.attr("method", "post");
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.attr("target", "postiframe");
        form.attr("file", $('#attUploader').val());
        form.submit();
    };

    this.LoadRequestBy = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetRequestBy",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    Data1[0].Combo = "-- Select All --";
                }
                self.RequestByArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DatatableCall = function () {
        if ($("#CBFHeaderGrid > tbody > tr").length == self.POCBFHeaderArray().length) {
            $("#CBFHeaderGrid").DataTable({
                "autoWidth": false,
                "destroy": true,
                "scrollX": true,
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

    self.LoadRequestBy();
    self.GetCBFSearch("0");
};

var mainViewModel = new SearchModel();
ko.applyBindings(mainViewModel);

$(document).ready(function () {

    objDialogyCBFDetailsAttachment = $("[id$='CBFDetailsAttachment']");
    objDialogyCBFDetailsAttachment.dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 400,
        duration: 300

    });

    var objDate = new Date();
    var Presentyear = objDate.getFullYear();

    $("#txtCBFDate").datepicker({
        yearRange: '1900:' + Presentyear,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: 'd',
        onSelect: function (selected) {
            $("#txtCBFEndDate").datepicker("option", "minDate", selected)
            $("#txtCBFEndDate").val("");
        }
    });

    $("#txtCBFEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        minDate: 'd',
        onSelect: function (selected) {

        }
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

function CloaseDialog(flag) {
    if (flag == "0")
        objDialogyCBFDetailsAttachment.dialog("close");
    return false;
}




