using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IEM.Areas.IFAMS.Models;
using System.Data;
namespace IEM.Areas.IFAMS.Models
{
    public interface ifamsIRepositoryx
    {
        List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> GetImpDetail(string StatusFlage);
        List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> GetImpDetailChecker();
        List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> GetImpView(string id);
        List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> IOAuditTrial(int id);
        string getfilename(string toanumber);
        List<IFAMS.Models.Ifams_Propertyx.Attachment> Getattachment(string id, string refe, string type);
        string GetAttachType();
        string GetImapirStatusexcel(string assetdata, string valid, string action);
        //List<Attachment> Getattachment(string id, string refe, string type);
        string InsertImpair(DataTable assetdata, string filename);
        string RevokeImpair(string gid);
    }
}