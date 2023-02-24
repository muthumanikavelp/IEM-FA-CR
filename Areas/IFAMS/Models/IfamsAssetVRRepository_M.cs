using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public interface IfamsAssetVRRepository_M
    {
        IEnumerable<AssetVRModel> GetVRDetail();
        string Updateasset(Models.AssetVRModel status);
        List<AssetVRModel> VRAutoBranch(string loc);
        List<AssetVRModel> VRAutoAsset(string assetdetid);
        IEnumerable<AssetVRModel> GetMakerReduction(string usergid);
        IEnumerable<AssetVRModel> GetCheckerReduction(string usergid);
        IEnumerable<AssetVRModel> GetVRDetailsearch(string assetid);
        IEnumerable<AssetVRModel> GetVRchkDetailsearch(string assetid);
        IEnumerable<AssetVRModel> GetVRDetail(string assetid);
        string AbortVR(string assetid);
       // string subVRdetail(string assetid);
        //IEnumerable<AssetVRModel> VRChkApprovalStatus(string assetid, string status, string remarks);
        string VRChkApprovalStatus(string assetid, string chkstatus, string remarks);
        List<AssetVRModel> GetVRStatus();
        IEnumerable<AssetVRModel> GetVRDetailsearch(string asset, string location);
    }
}