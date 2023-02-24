//Ramya Added
var objShowDialogPayMode;
UrlHelper = UrlHelper.replace("GetEditLCPayMode", ""); 
UrlDet = UrlDet.replace("_LocalPayment", "");

var PayModeModel = function (id) {
    var self = this;
    self.PayModeLCEditArray = ko.observableArray();
    self.ECFPaymentArray = ko.observableArray();

    this.LoadEditLCPayMode = function () {
        $.ajax({
            type: "post",
            url: UrlHelper + "GetEditLCPayMode",
            data: "{}",
            async: false,
            contentType: "application/json;",
            success: function (response) {
                var Data1 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null)
                    Data1 = JSON.parse(response.Data1);
                self.PayModeLCEditArray(Data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    };

    this.LoadECFDetails = function () {  

        $.ajax({
            type: "post",
            url: UrlDet + "GetLocalPayment", 
            data:{},
            contentType: "application/json;",
            success: function (response) {                
                var Data1 = ""; 
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);
                    self.ECFPaymentArray(Data1);
                }   
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) { 
            }
        });
    };

    this.EditInvoiceCreditDetails = function (Index, flag)
    {
        var _tmpData = self.ECFPaymentArray()[Index];
        $("#hfAccNo").val(_tmpData.ecfcreditline_ref_no);
        $("#hfGLId").val(_tmpData.ecfcreditline_gid);
        $("#hfPayMode").val(_tmpData.ecfcreditline_pay_mode);
        $("#hfAmount").val(_tmpData.ecfcreditline_amount);
        $("#hfDesc").val(_tmpData.ecfcreditline_desc); 
        if (flag == 0) {
            $("#txtPayMode").val($("#hfPayMode").val());
            $("#txtPayMode").css("display", "block");
            $("#txtPayMode").prop('disabled', true);
            $("#ddlPayMode1").css("display", "none");

            $("#txtPayRefNo1").val($("#hfAccNo").val());
            $("#txtPayRefNo1").prop('disabled', true);

            $("#txtPaymentAmount1").val($("#hfAmount").val());
            $("#txtPaymentAmount1").prop('disabled', true);

            $("#txtPayDescription1").val($("#hfDesc").val());
            $("#txtPayDescription1").prop('disabled', true);

            $("#btnCreditLineSubmit1").css("display", "none");
            objShowDialogPayMode.dialog({ title: 'Payment Details', width: '560', height: '380' });

            objShowDialogPayMode.dialog("open");
        }
        else {
            $("#ddlPayMode1").val($("#hfPayMode").val());
            if ($("#ddlPayMode1").val() == "0") {
                $("#ddlPayMode1").removeClass("valid");
                $("#ddlPayMode1").addClass("required");
            }
            else {
                $("#ddlPayMode1").addClass("valid");
                $("#ddlPayMode1").removeClass("required");
            }
            $("#ddlPayMode1").css("display", "block");
            $("#txtPayMode").css("display", "none");

            $("#txtPayRefNo1").val("211100037");
            $("#hfAccNo").val("211100037");
            $("#txtPaymentAmount1").val($("#hfAmount").val());
            $("#txtPaymentAmount1").prop('disabled', false);

            $("#txtPayDescription1").val($("#hfDesc").val());
            $("#txtPayDescription1").prop('disabled', false);

            $("#btnCreditLineSubmit1").css("display", "block");
            objShowDialogPayMode.dialog({ title: 'Payment Details', width: '560', height: '380' });
            objShowDialogPayMode.dialog("open");
        }
    }

    this.SetInvoiceCreditDetails=  function (flag) { 
        objShowDialogPayMode.dialog({
            autoOpen: false,
            modal: true,
            close: close,
            width: 560,
            height: 380,
            duration: 300
        });
        var paymode = "", RefNo = "", Beneficiary = "", Amount = "", Desc = "";
        var PayBank = 0;
        var CreditlineGId = $("#hfGLId").val();
        var Ecfid = "0";
        var InvId = "0";
        var RefId = $("#hfAccNo").val();
        var IfscCode = "";

        paymode = $("#ddlPayMode1 option:selected").text();
        PayBank = "0";
        RefNo = $("#txtPayRefNo1").val();
        Beneficiary = "";
        Amount = $("#txtPaymentAmount1").val();
        Desc = $("#txtPayDescription1").val();
        var IsRemoved = "0";

        if (paymode == "0" || paymode == "-- Select One --") {
            jAlert("Ensure Payment Mode!", "Message");
            return false;
        }

        if (paymode == "EFT" || paymode == "RRP" || paymode == "ERA") {
            if ($.trim(RefNo) == "") {
                jAlert("Ensure Account No!", "Message");
                return false;
            }
        }

        if ($.trim(Amount) == "" || parseFloat(Amount) == 0) {
            jAlert("Ensure Payment Amount!", "Message");
            return false;
        }

        if ($.trim(Desc) == "") {
            jAlert("Ensure Description!", "Message");
            return false;
        }

        var InvoiceCreditLine = {
            CreditlineGId: CreditlineGId,
            Ecfid: Ecfid,
            InvId: InvId,
            RefId: RefId,
            paymode: paymode,
            RefNo: RefNo,
            Beneficiary: Beneficiary,
            Amount: Amount,
            BankId: PayBank,
            Desc: Desc,
            IsRemoved: IsRemoved,
            IfscCode: IfscCode
        }; 
        $.ajax({
            type: "post",
            url: UrlHelper + "SetEditECFCreditLineDetails",
            data: JSON.stringify(InvoiceCreditLine),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var Data1 = "", Data2 = "";
                if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                    Data1 = JSON.parse(response.Data1);

                    if (Data1[0].Clear == false) {
                        jAlert(Data1[0].Msg, "Message");
                    }

                    else {
                        self.LoadECFDetails();
                        jAlert(Data1[0].Msg, "Message");
                        objShowDialogPayMode.dialog("close"); 
                    }
                }
                
            },
            error: function (XMLHttpRequest, textStatus, errorThrown, response) {
                alert(response);
            }
        });
    };

    this.DeleteInvoiceCreditDetails = function (glid) { 
        $("#hfCreditlineGId").val(glid);
        jConfirm("Are you sure? Want to delete Payment!", "Confirm", function (callback) {
            if (callback == true) {
                var IsRemoved = "1";
                var InvoiceCreditLine = {
                    CreditlineGId: glid,
                    Ecfid: 0,
                    InvId: 0,
                    RefId: 0,
                    paymode: "RRP",
                    BankId: 0,
                    RefNo: 0,
                    Beneficiary: "",
                    Amount: 0,
                    Desc: "",
                    IsRemoved: IsRemoved,
                    IfscCode: ""
                };
                $.ajax({
                    type: "post",
                    url: UrlHelper + "SetEditECFCreditLineDetails",
                    data: JSON.stringify(InvoiceCreditLine),
                    contentType: "application/json;",
                    success: function (response) {
                        var Data1 = "", Data2 = "";
                        if (response.Data1 != null && response.Data1 != "" && JSON.parse(response.Data1) != null) {
                            Data1 = JSON.parse(response.Data1);
                            if (Data1[0].Clear == false)
                                jAlert(Data1[0].Msg, "Message");
                            else {
                                self.LoadECFDetails();
                                jAlert(Data1[0].Msg, "Message");
                                objShowDialogPayMode.dialog("close");  
                            }
                        } 
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            } else {
                return false;
            }
        });


    };
    
    self.LoadECFDetails();
    self.LoadEditLCPayMode();
};
var mainViewModel = new PayModeModel();
ko.applyBindings(mainViewModel, document.getElementById("wgempPaymentlocal")); 

$(document).ready(function () { 
    objShowDialogPayMode = $("[id$='ShowDialogPayMode']");
    objShowDialogPayMode.dialog({
        autoOpen: false,
        modal: true,
        close: close,
        width: 560,
        height: 380,
        duration:300
    });
});
  
 
function CloseDetails(flag) {
    objShowDialogPayMode.dialog("close"); 
};


 
