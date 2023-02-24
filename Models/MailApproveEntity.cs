using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Models
{
    public class MailApproveEntity
    {
        public SelectList Template { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }

        public SelectList TransactionType { get; set; }
        public string TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }

        public SelectList WorkFlowType { get; set; }
        public string WorkFlowId { get; set; }
        public string WorkFlowName { get; set; }

        public string IsFinalApprover { get; set; }

        public SelectList TriggerType { get; set; }
        public string TriggerTypeId { get; set; }
        public string TriggerTypeName { get; set; }


        public SelectList TriggerFreType { get; set; }
        public string TriggerTypeFreId { get; set; }
        public string TriggerTypeFreName { get; set; }
        public string Triggernodays { get; set; }

        public SelectList AudienceType { get; set; }
        public string AudienceId { get; set; }
        public string AudienceName { get; set; }

        public SelectList ToType { get; set; }
        public string ToTypeId { get; set; }
        public string ToTypeName { get; set; }

        public SelectList ToGetType { get; set; }
        public string ToGetTypeId { get; set; }
        public string ToGetTypeName { get; set; }

        public string TOid { get; set; }
        public string BCCid { get; set; }
        public string CCid { get; set; }
        public string Subject { get; set; }
        public string Includeflg { get; set; }

        public SelectList Attachment { get; set; }
        public string AttachmentId { get; set; }
        public string AttachmentName { get; set; }

        public string Content { get; set; }
        public string Signature { get; set; }

        public SelectList Tables { get; set; }
        public string TablesId { get; set; }
        public string TablesName { get; set; }

        public SelectList Tablescol { get; set; }
        public string TablescolId { get; set; }
        public string TablescolName { get; set; }

        public string[] TablescolNamenew { get; set; }

        public SelectList Moduledata { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }

        [AllowHtml]
        public string HtmlContent { get; set; }

        public int employeeGid { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }

        public string employee { get; set; }
        public string empdept { get; set; }
    }
    public class MailApprovalModel
    {
        public int _QueueID { get; set; }
        public string _SubmitDate { get; set; }
        public int _QueueFrom { get; set; }
        public string _QueueTo { get; set; }
        public string _QueueToType { get; set; }
        public string _ActionRemarks { get; set; }
        public int _QueueCurrentLevel { get; set; }
        public string _ActionFor { get; set; }
        public string _ActionDate { get; set; }
        public int _ActionBy { get; set; }
        public string _ActionStatus { get; set; }
        public string _SkipFlag { get; set; }

        public string _RequestType { get; set; }
        public string _NextApprovalGroup { get; set; }
        public string _NextpproverID { get; set; }
        public string _NextApprovalIsMandatory { get; set; }

    }
    public class MailApprovaleowModel
    {
        public string _EcfID { get; set; }
        public string _ApproveID { get; set; }
    }
}