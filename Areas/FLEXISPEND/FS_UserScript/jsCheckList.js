UrlDet = UrlDet.replace("GetCheckListGrid", "");
UrlHelper = UrlHelper.replace("GetAutoCompleteCourier", "");
var CheckListModel = function () {
    var self = this;

    self.ChecklistId = ko.observable("");
    self.ParentId = ko.observable("");
    self.PoType = ko.observable("");
    self.Title = ko.observable("");
    self.IsConfirmed = ko.observable("");
    self.Reason = ko.observable("");
    self.Active = ko.observable("");
    self.DisplayOrder = ko.observable("");

    var InnerDetails = {
        ChecklistId: self.ChecklistId,
        ParentId: self.ParentId,
        PoType: self.PoType,
        Title: self.Title,
        IsConfirmed: self.IsConfirmed,
        Reason: self.Reason,
        Active: self.Active,
        DisplayOrder: self.DisplayOrder
    };

    //document type dropdown array
    self.DocTypeArray = ko.observableArray();

    self.ParentDDArray = ko.observableArray();
    self.CheckListDetailArray = ko.observableArray();

    self.loadDocType = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetDocumentTypeCheckList",
            data: '{}',
            async: false,
            contentType: "application/json;",
            success: function (response) {
                self.DocTypeArray.removeAll();
                var Data0 = "";
                var id = "";
                if (response.Data0 != null && response.Data0 != "" && JSON.parse(response.Data0) != null) {
                    Data0 = JSON.parse(response.Data0);

                    id = Data0[0].ChildGroup[0].Id;
                    self.DocTypeArray(Data0);

                    self.loadInit(id);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.DocTypeSelectChanged = function (obj, event) {
        $("#hdfCHKLSTId").val("");
        $("#txtTitle").val("");
        $("#chkConfirmation").prop('checked', true);
        $("#txtReason").val("");
        $("#chkActive").prop('checked', true);
        $("#txtDisplayOrder").val("");
       
        var tmpQuery = {
            DocSubTypeId: $("#ddlDocType").val(),
            POType: $("#ddlPOType").val(),
            IsActive: $("#ddlLoadType").val()
        };

        $.ajax({
            type: "post",
            url: UrlDet + "GetMasterCheckList",
            data: JSON.stringify(tmpQuery),
            contentType: "application/json;",
            success: function (response) {
                var Data0 = "", Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.ParentDDArray(Data1);
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.CheckListDetailArray(Data2);
                } else {
                    self.CheckListDetailArray.removeAll();
                }
                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);

                    if (Data3[0].IsPoType == true) {
                        $("#divPOType").attr("style", "display:block");
                    } else {
                        $("#divPOType").attr("style", "display:none");
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    }

    self.POTypeSelectChanged = function (obj, event) {
        $("#hdfCHKLSTId").val("");
        $("#txtTitle").val("");
        $("#chkConfirmation").prop('checked', true);
        $("#txtReason").val("");
        $("#chkActive").prop('checked', true);
        $("#txtDisplayOrder").val("");
        $("#ddlParent").val("0");

        var tmpQuery = {
            DocSubTypeId: $("#ddlDocType").val(),
            ParentId: "0",
            POType: $("#ddlPOType").val(),
            IsActive: $("#ddlLoadType").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetMasterCheckList",
            data: JSON.stringify(tmpQuery),
            contentType: "application/json;",
            success: function (response) {
                var Data0 = "", Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.ParentDDArray(Data1);
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.CheckListDetailArray(Data2);
                } else {
                    self.CheckListDetailArray.removeAll();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    }

    self.ParentSelectChanged = function (obj, event) {
        if (event.originalEvent) {
            $("#hdfCHKLSTId").val("");
            $("#txtTitle").val("");
            $("#chkConfirmation").prop('checked', true);
            $("#txtReason").val("");
            $("#chkActive").prop('checked', true);
            $("#txtDisplayOrder").val("");

            var tmpQuery = {
                DocSubTypeId: $("#ddlDocType").val(),
                ParentId: $("#ddlParent").val(),
                POType: $("#ddlPOType").val(),
                IsActive: $("#ddlLoadType").val()
            };
            $.ajax({
                type: "post",
                url: UrlDet + "GetCheckListGrid",
                data: JSON.stringify(tmpQuery),
                contentType: "application/json;",
                success: function (response) {
                    var Data0 = "", Data1 = "";
                    if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                        Data1 = JSON.parse(response.Data1);

                        //show Message if error
                        if (Data1[0].Message != "") {
                            jAlert(Data1[0].Message, "Message");
                        }
                    }

                    if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                        Data2 = JSON.parse(response.Data2);
                        self.CheckListDetailArray(Data2);
                    } else {
                        self.CheckListDetailArray.removeAll();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        } else {
        }
    }

    self.loadInit = function (id) {
        var tmpQuery = {
            DocSubTypeId: id,
            POType: $("#ddlPOType").val(),
            IsActive: $("#ddlLoadType").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "GetMasterCheckList",
            data: JSON.stringify(tmpQuery),
            contentType: "application/json;",
            success: function (response) {
                var Data0 = "", Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.ParentDDArray(Data1);
                }

                if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                    Data2 = JSON.parse(response.Data2);
                    self.CheckListDetailArray(Data2);
                } else {
                    self.CheckListDetailArray.removeAll();
                }

                if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                    Data3 = JSON.parse(response.Data3);

                    if (Data3[0].IsPoType == true) {
                        $("#divPOType").attr("style", "display:block");
                    } else {
                        $("#divPOType").attr("style", "display:none");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.create = function () {
        var chkConfirm = "", chkActive = "";
        if ($("#chkConfirmation").prop("checked")) {
            chkConfirm = "Y";
        } else {
            chkConfirm = "N";
        }
        if ($("#chkActive").prop("checked")) {
            chkActive = "Y";
        } else {
            chkActive = "N";
        }

        if ($.trim($("#txtTitle").val()) == "") {
            jAlert("Ensure Title!", "Message");
            return false;
        }
        if ($.trim($("#txtReason").val()) == "") {
            jAlert("Ensure Reason!", "Message");
            return false;
        }
        if ($.trim($("#txtDisplayOrder").val()) == "") {
            jAlert("Ensure Display Order!", "Message");
            return false;
        }

        var data = {
            ChkLstId: $("#hdfCHKLSTId").val(),
            ParentId: $("#ddlParent").val(),
            DocSubTypeId: $("#ddlDocType").val(),
            PoType: $("#ddlPOType").val(),
            Title: $("#txtTitle").val(),
            IsConfirmed: chkConfirm,
            Reason: $("#txtReason").val(),
            IsActive: chkActive,
            DisOrder: $("#txtDisplayOrder").val(),
            IsActiveFilter: $("#ddlLoadType").val()
        };
        $.ajax({
            type: "post",
            url: UrlDet + "SaveCheckListGrid",
            data: JSON.stringify(data),
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "", Data2 = "", Data3 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    //show Message if error
                    if (Data1[0].Message != "") {
                        jAlert(Data1[0].Message, "Message");
                    }

                    if (Data1[0].Clear == "true" || Data1[0].Clear == "True" || Data1[0].Clear == "1") {
                        self.CheckListDetailArray.removeAll();

                        $("#hdfCHKLSTId").val("");
                        $("#txtTitle").val("");
                        $("#chkConfirmation").prop('checked', true);
                        $("#txtReason").val("");
                        $("#chkActive").prop('checked', true);
                        $("#txtDisplayOrder").val("");

                        //if ($("#ddlParent").val() == "0" || $("#ddlParent").val() == "-1") {
                            if (response.Data2 != null && response.Data2 != "" && JSON.parse(response.Data2) != null) {
                                Data2 = JSON.parse(response.Data2);
                                self.ParentDDArray(Data2);
                            }
                        //}

                        if (response.Data3 != null && response.Data3 != "" && JSON.parse(response.Data3) != null) {
                            Data3 = JSON.parse(response.Data3);
                            self.CheckListDetailArray(Data3);
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    };

    self.edit = function (InnerDetails) {
        $("#txtSlNo").val(InnerDetails.SlNo);
        $("#hdfCHKLSTId").val(InnerDetails.ChecklistId);
        $("#txtTitle").val(InnerDetails.Title);
        $("#chkConfirmation").prop('checked', (InnerDetails.IsConfirmed == "Yes" || InnerDetails.IsConfirmed == "yes") ? true : false);
        $("#txtReason").val(InnerDetails.Reason);
        $("#chkActive").prop('checked', (InnerDetails.Active == "Yes" || InnerDetails.Active == "yes") ? true : false);
        $("#txtDisplayOrder").val(InnerDetails.DisplayOrder);
    };

    self.renderedHandler1 = function () {
        if ($("#ddlDocType optgroup").length == self.DocTypeArray().length) {
            $("#ddlDocType").chosen({});
            $("#ddlDocType_chosen").css("width", "300px");
            $("#ddlDocType_chosen > optgroup > option").css("width", "300px");
        }
    }

    //load the Doctument Type DropDown.
    self.loadDocType();
};

var mainViewModel = new CheckListModel();
ko.applyBindings(mainViewModel);

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}