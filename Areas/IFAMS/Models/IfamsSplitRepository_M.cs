using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public interface IfamsSplitRepository_M
    {
        IEnumerable<AssetSplitModel> GetAssetDetail(string assetdetid);
        string GetAssetDetIDNew(string lcode, string acode, Models.AssetSplitModel status, string splitamount);
        IEnumerable<AssetSplitModel> GetAssetDetailChk();
        IEnumerable<AssetSplitModel> GetAssetDetailHp();
        IEnumerable<AssetSplitModel> GetSplitDetail(string id);
        IEnumerable<AssetSplitModel> GetMakSplitDetail(string id);
        string AppRejAsset(string id, string chstatus, string remark);
        IEnumerable<AssetSplitModel> GetSplit(string loginid);
        List<AssetSplitModel> SPAutoAssetsummary(string Assetdetid);
        List<AssetSplitModel> SPAutoAsset(string Assetdetid, string loginid);
        List<AssetSplitModel> SPAutoAssetChk(string Assetdetid);
        //IEnumerable<AssetSplitModel> GetserachSplit();
    }
}