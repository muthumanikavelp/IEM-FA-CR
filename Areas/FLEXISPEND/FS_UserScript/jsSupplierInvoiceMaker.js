var rbtclick = "";
var _reasonBoxCount = "";
var _reasonBoxText = "";
var model;

UrlDetMain = UrlDetMain.replace("GetAuditCheckList", "");
UrlHelperMain = UrlHelperMain.replace("GetAutoCompleteCourier", "");

var SupplierInvoiceMakerModel = function () {
    var self = this;

    self.ECFDetails = ko.observableArray();

    //Audit Checking Array
    self.ECFCheckArray = ko.observableArray();

    self.ECFCheckArrayDetails = ko.observableArray();

    this.LoadECFData = function () {
        var EcfFilter = {
            EcfId: $("#hdfEcfId").val()
        };

        $.ajax({
            type: "post",
            url: UrlDetMain + "GetAuditCheckList",
            data: JSON.stringify(EcfFilter),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data0 = "", Data1 = "", Data2 = "", Data3 = "", Data4="";
                if (response.Data0 != null && response.Data0 != "" && JSON.parse(response.Data0) != null) {
                    Data0 = JSON.parse(response.Data0);

                    //show Message if error
                    if (Data0[0].Message != "") {
                        jAlert(Data0[0].Message, "Message");

                        self.ECFCheckArray.removeAll();
                    }
                }
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.ECFDetails(Data1);

                    //Disable Approve Button On PageLoading --for Maker login.
                    if (Data1[0].Role == "M" || Data1[0].Role == "m") {
                        $('input:radio[name=chkMStatus][value=Inex]').prop('checked', true);
                        $('input:radio[name=chkMStatus][value=Approve]').attr("disabled", "disabled");
                    }
                }
                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.ECFCheckArray(Data2);
                }
                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);
                    self.ECFCheckArrayDetails(Data3);
                }
                if (response.Data4 != null && response.Data4 != "" && JSON.parse(response.Data4) != null) {
                    Data4 = JSON.parse(response.Data4);
                    
                    var txtString = Data4[0].Reason;
                    txtString = strReplaceAll(txtString, '<br />', '\n');
                    $('#txtReason').val(txtString);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DataHander = function () {
        if (self.ECFCheckArrayDetails().length != 0) {
            var array = new Array();
            var tmpArray = new Array();
            tmpArray = self.ECFCheckArrayDetails();
            array = tmpArray[0].CheckList.split('|');

            var name = ""; var value = "";
            var _rbtName = "rbt";
            $.each(array, function (index) {
                if (array[index] != null && array[index] != "") {
                    name = array[index].split('-')[0];
                    value = array[index].split('-')[1];
                    $("input:radio[name='" + _rbtName + name + "'][value='" + value + "']").prop('checked', true);
                }
            });

            var checkedCount = $('#divAuditList :radio:checked').length;
            var totList = $('#divAuditList :radio').length / 3;
            var NotOk = $('#divAuditList input[value=2]:radio:checked').length;
            var Ok = $('#divAuditList input[value=1]:radio:checked').length;
            
            if (checkedCount == totList && NotOk == "0" && Ok != "0") {
                //unblock the approve check option
                $('input:radio[name=chkMStatus][value=Approve]').removeAttr("disabled");
                $('input:radio[name=chkMStatus][value=Approve]').prop('checked', true);
            } else {
                //block the approve check option
                $('input:radio[name=chkMStatus][value=Inex]').prop('checked', true);
                $('input:radio[name=chkMStatus][value=Approve]').attr("disabled", "disabled");
            }
        }
    };

    this.CheckedNotOk = function (obj, event) {
                
        if (event.originalEvent) {
            var _inBit = event.originalEvent.target.defaultValue;
            rbtclick = event.originalEvent.target.name;
            model = obj;

            //start new added
            var msg = obj.Reason;
            var txt = ""
            var txtid = "";
            if (msg.indexOf('~') === -1) {
                _reasonBoxCount = "";
                $("#ReasonDet").html(msg);
            } else {

                var msgContent = msg.split("~");
                _reasonBoxText = msgContent;
                _reasonBoxCount = msgContent.length - 1;

                for (var i = 0; i < msgContent.length; i++) {
                    txtid = "txtcontent" + i;
                    txt += msgContent[i]
                    if (i < msgContent.length - 1) {
                        txt += '<input type="text" style="width: 80px; margin-top:-8px; padding-top:0px;" class="textboxStyleBig" id=txtid />'
                    } else { txt += ''; }
                    txt = txt.replace("txtid", txtid);
                }
                $("#ReasonDet").html(txt);
            }
            var Resondet = $("#ReasonDet").html();
            Resondet += '</br>';
            var tex1 = Resondet += '<textarea  type="text" style="width: 100%; height: 15%;  border-radius:4px;" class="textboxStyleBig" id=Txtsubmitreson />';

            tex1 += '</br>';
            $("#ReasonDet").html(tex1);
            //end new added

            //$("#txtPopReason").val(obj.Reason);

            if (_inBit == "2") {
                ShowReason();
            } else {

                /*var tmp = $("#txtReason").val();
                if (tmp.indexOf(obj.Reason + '\n') >= 0) {
                    tmp = tmp.replace(obj.Reason + '\n', '');
                    $("#txtReason").val(tmp);
                }*/

                //call sp to remove the string here.
                self.RemoveReason(obj.ChkLstId, _inBit, '');
            }

            var checkedCount = $('#divAuditList :radio:checked').length;
            var totList = $('#divAuditList :radio').length / 3;
            var NotOk = $('#divAuditList input[value=2]:radio:checked').length;
            var Ok = $('#divAuditList input[value=1]:radio:checked').length;
            if (checkedCount == totList && NotOk == "0" && Ok != "0") {
                //unblock the approve check option
                $('input:radio[name=chkMStatus][value=Approve]').removeAttr("disabled");
                $('input:radio[name=chkMStatus][value=Approve]').prop('checked', true);
            } else {
                //block the approve check option
                $('input:radio[name=chkMStatus][value=Inex]').prop('checked', true);
                $('input:radio[name=chkMStatus][value=Approve]').attr("disabled", "disabled");
            }
        } else {
        }
    }

    self.updateStatus = function () {
        //collect the auditor checking values
        //var q = $('#divAuditList :radio:checked').map(function () { return this.id; }).get();
        var _role = $("#hdfRoleId").val();
        var isDedup = $('#hfISDedup').val();
        if (isDedup == "0" && _role != "A") {
            jAlert("Should run Dedup atleast one time.", "Message");
            return false;
        }

        //find the check list value selected
        var checkedCount = $('#divAuditList :radio:checked').length;
        var totList = $('#divAuditList :radio').length / 3;

        if (checkedCount != totList) {
            jAlert("Ensure all the check verified.", "Message");
            return false;
        }

        var array = new Array();
        $('#divAuditList input:radio:checked').each(function (index) {
            array[index] = $(this).attr('id') + '-' + $(this).attr('value');
        });

        var _auditListParam = "";
        $.each(array, function (index) {
            _auditListParam += array[index].split('_')[1] + '|';
        });

        var _tmpStatus = "";
        
        var chkReason = $("#txtReason").val();

        if (_role == "M") {
            _tmpStatus = $("input[name=chkMStatus]:checked").val();
        }
        if (_role == "C") {
            _tmpStatus = $("input[name=chkCStatus]:checked").val();
        }
        if (_role == "A") {
            _tmpStatus = $("input[name=chkAStatus]:checked").val();
        }

        //check the constrain for Approve.
        if ($.trim(chkReason) != "") {
            if (_tmpStatus == "Approve" || _tmpStatus == "CheckerApprove" || _tmpStatus == "AuthorizerApprove") {
                jAlert("You could not approve this document!", "Message");
                return false;
            }
        }

        if (_tmpStatus == "") {
            jAlert("Ensure Status!", "Message");
            return false;
        }
        else {
            if ((_tmpStatus == "Inex" || _tmpStatus == "Hold" || _tmpStatus == "CheckerRe-Audit" || _tmpStatus == "AuthorizerRe-Audit") && $("#txtRemarks").val().length == 0) {
                jAlert("Ensure Remarks!", "Message");
                return false;
            }
        }

        if (_auditListParam.trim() == "" && (_tmpStatus == "Approve")) {
            jAlert("You could not approve this document!", "Message");
            return false;
        }
        
        //if ($("#hfDocSubType").val() == "10" && (_tmpStatus == "Inex" || _tmpStatus == "AuthorizerRe-Audit"))
        if ($("#hfDocSubType").val() == "10" && (_tmpStatus == "Inex"))
        {
            jConfirm("This is Lease document! You can't inex this document. It will consider as reject document. Do you want to continue?", "Confirm", function (callback) {
                if (callback == true) {
                    var data = {
                        EcfId: $("#hdfEcfId").val(),
                        ChkLstIds: _auditListParam,
                        Status: _tmpStatus,
                        Remarks: $("#txtRemarks").val(),
                        Reason: chkReason
                    };
                    $("#btnSubmitAudit").attr("disabled", "disabled");
                    $.ajax({
                        type: "post",
                        url: UrlDetMain + "SetAuditCheckList",
                        data: JSON.stringify(data),
                        contentType: "application/json;",
                        success: function (response) {
                            var Data1 = "";
                            if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                                Data1 = JSON.parse(response.Data1);

                                //show Message 
                                if (Data1[0].Message != "" && Data1[0].Clear != true) {
                                    jAlert(Data1[0].Message, "Message");
                                }

                                if (Data1[0].Clear != false) {
                                    jConfirm(Data1[0].Message, "Success", function (callback) {
                                        if (callback == true) {
                                            self.cancel()
                                        } else {
                                            self.cancel()
                                        }
                                    });

                                    //remove the cancel button from dialog box
                                    $("#popup_ok").attr("style", "margin-left: 50px;");
                                    $("#popup_cancel").attr("style", "visibility: hidden");
                                }

                            } else {
                                jAlert("unable to process your request!", "Message");
                            }
                            $("#btnSubmitAudit").removeAttr("disabled");
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            //alert(errorThrown);
                        }
                    });
                } else {
                    return false;
                }
            });
            return false;
        }

        var data = {
            EcfId: $("#hdfEcfId").val(),
            ChkLstIds: _auditListParam,
            Status: _tmpStatus,
            Remarks: $("#txtRemarks").val(),
            Reason: chkReason
        };
        $("#btnSubmitAudit").attr("disabled", "disabled");
        $.ajax({
            type: "post",
            url: UrlDetMain + "SetAuditCheckList",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message 
                    if (Data1[0].Message != "" && Data1[0].Clear != true) {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear != false) {
                        jConfirm(Data1[0].Message, "Success", function (callback) {
                            if (callback == true) {
                                self.cancel()
                            } else {
                                self.cancel()
                            }
                        });

                        //remove the cancel button from dialog box
                        $("#popup_ok").attr("style", "margin-left: 50px;");
                        $("#popup_cancel").attr("style", "visibility: hidden");

                    }

                } else {
                    jAlert("unable to process your request!", "Message");
                }
                $("#btnSubmitAudit").removeAttr("disabled");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.updateReason = function () {
        /*var _tmpModel = model;
        var _ref;
        ko.utils.arrayForEach(self.ECFCheckArray(), function (item, index) {
            if (_tmpModel.ParentId == item.GroupId) {
                $.each(item.ChildGroup, function (index) {
                    if (_tmpModel.ChkLstId == item.ChildGroup[index].ChkLstId) {
                        item.ChildGroup[index].Reason = $("#txtPopReason").val();
                        _ref = item.ChildGroup[index];
                    }
                });
            }
        });
        var obj = _ref;

        var tmp = $("#txtReason").val();
        $("#txtReason").val(tmp + obj.Reason + '\n');
        */
        var SubmitReason = model.Reason;
        SubmitReason += " " + $("#Txtsubmitreson").val().trim();
        if (_reasonBoxCount == "") {
            self.RemoveReason(model.ChkLstId, "2", SubmitReason);
        } else {
            for (var i = 0; i < _reasonBoxCount; i++) {
                if ($("#txtcontent" + i).val().trim() == "") {
                    jAlert("Ensure Input Field!", "Message");
                    return false;
                }
            }
            //_reasonBoxText += " " + $("#Txtsubmitreson").val().trim();
            var _tmpReason = "";
            for (var i = 0; i < _reasonBoxText.length; i++) {
                txtid = "#txtcontent" + i;
                _tmpReason += _reasonBoxText[i]
                if (i < _reasonBoxText.length - 1) {
                    _tmpReason += $(txtid).val();
                } else { _tmpReason += ''; }
            }
            _tmpReason += " " + $("#Txtsubmitreson").val().trim();
            //call the sp to update reason.
            self.RemoveReason(model.ChkLstId, "2", _tmpReason);
        }

        //blocking and Unblocking approve Option
        var _role = $("#hdfRoleId").val();
        if (_role == "M") {
            var array = new Array();
            $('#divAuditList input[value=2]:radio:checked').each(function (index) {
                array[index] = $(this).attr('id') + '-' + $(this).attr('value');
            });
            var _appChecking = array.length;
            if (_appChecking > 0) {
                //block the approve check option
                $('input:radio[name=chkMStatus][value=Inex]').prop('checked', true);
                $('input:radio[name=chkMStatus][value=Approve]').attr("disabled", "disabled");
            } else {
                //unblock the approve check option
                $('input:radio[name=chkMStatus][value=Approve]').removeAttr("disabled");
            }
        }

        //$("#txtPopReason").val("");
        $("#ReasonDet").html("");
        objReason.dialog("close");
    };

    self.cancel = function () {
        ko.utils.postJson(UrlDetMain);
    };

    self.RemoveReason = function (ChkLstId, Status, Reason) {
        var array = new Array();
        $('#divAuditList input:radio:checked').each(function (index) {
            array[index] = $(this).attr('id') + '-' + $(this).attr('value');
        });

        var _auditListParam = "";
        $.each(array, function (index) {
            _auditListParam += array[index].split('_')[1] + '|';
        });


        var data = {
            EcfId: $("#hdfEcfId").val(),
            ChkLstId: ChkLstId,
            Status: Status,
            Reason: Reason,
            ChkLstIds: _auditListParam
        };

        $.ajax({
            type: "post",
            url: UrlDetMain + "SetAuditCheckListReason",
            data: JSON.stringify(data),
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    
                    var txtString = Data1[0].Reason;
                    txtString = strReplaceAll(txtString,'<br />', '\n');
                    $('#txtReason').val(txtString);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.LoadECFData();
};

var ViewModel = new SupplierInvoiceMakerModel();
ko.applyBindings(ViewModel, document.getElementById("auditChkLst"));

$(document).ready(function () {
    objReason = $("[id$='showReason']");
    objReason.dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        height: 200,
        duration: 300
    });

    $(".ui-icon-closethick").click(function () {
        if (rbtclick != "") {
            $("input:radio[name='" + rbtclick + "'][value=1]").prop('checked', false);
            $("input:radio[name='" + rbtclick + "'][value=2]").prop('checked', false);
            $("input:radio[name='" + rbtclick + "'][value=3]").prop('checked', false);
            rbtclick = "";
        }
        //$("#txtPopReason").val("");
        $("#ReasonDet").html("");
        objReason.dialog("close");
    });

});
function ShowReason() {
    $('#showReason').attr("style", "display:block;");

    objReason.dialog({ title: 'Enter Reason', width: '500', height: '200' });
    objReason.dialog("open");
    return false;
}

function strReplaceAll(string, Find, Replace) {
    try{
        return string.replace(new RegExp(Find, "gi"), Replace);
    } catch (ex) {
        return string;
    }
}
