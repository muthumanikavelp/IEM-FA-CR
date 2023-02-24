using IEM.Areas.IFAMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public interface IfamsAssetMergeRepository
    {
        List<AssetParentModel> GetAssetParent();
        List<AssetParentModel> GetAssetIdParent(string id);
        List<AssetParentModel> GetAssetIdChild();
        List<AssetParentModel> GetAssettmergeDetails();
        List<AssetParentModel> GetAssettmergeCheckerDetails();
        List<AssetParentModel> GetAssetInGroup(string assetid ,string grpid);
        List<AssetParentModel> GetAssetMergemakersummary(string Generateid);
        List<AssetParentModel> GetAssetMergemakersummarynew(string Generateid, string status);
        List<AssetParentModel> GetAssetMergecheckersummarynew(string Generateid, string status);
        List<AssetParentModel> GetAssetMergecheckersummary(string Generateid);
        List<AssetParentModel> Getsearchmakersummary(string AssetId, string Getsearchmakersummary);
        List<AssetParentModel> getsearchckeckerdetails(string get,string beranchid);
        //public string GetAssetMerging1(string assetid, string[] assetgid);
        //IEnumerable<AssetParentModel1> GetAssetParent1();
    }
}