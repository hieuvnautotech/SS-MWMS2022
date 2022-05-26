using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.TIMS;
using Mvc_VD.Models.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_VD.Services.Interface
{
    public interface IcommonService
    {
        Task<IReadOnlyList<WMaterialInfo>> GetListMAterialNo(string mt_no, string style_no);
        Task<IReadOnlyList<SdInfos>> GetListSDInfo(string SdNo, string SdName, string ProductCode, string remark);
        Task<int> InsertSdInfo(SdInfo item);
        Task<string> GetLastSdNo();
        Task<IReadOnlyList<lct_info>> GetLocationFactory();
        Task<IReadOnlyList<ListPickingScan>> GetListPickingScan(string SdNo, string SdNm, string ProductCode, int intpage, int introw);
        int TottalRowPickingScan(string SdNo, string SdNm, string ProductCode);
        Task<TmpWMaterialInfo> GetTmpMaterialInfo(string MtNo);
        Task<string> CheckSDInfo(string SdNo);
        Task<int> InsertInventoryProduct(InventoryProduct item);
        Task<InventoryProduct> CheckExistMaterialProduct(string MtCode);
        Task<bool> CheckMaterialNoShipp(string MtNo, string SdNo);
        Task<bool> CheckTmpMaterial(string MtNo, DateTime datenow);
        Task<int> DeleteTmpMaterial(string MtNo, string ChangeId, DateTime ChangeDate);
        Task<IReadOnlyList<d_bobbin_lct_hist>> GetlistBobbinHist(string BbNo);
        Task<d_bobbin_info> GetBobbinInfo(string BbNo);
        Task<int> UpdateBobbinInfo(string BbNo, string MtCd, string ChangeId, DateTime ChangeDate);
        Task<Models.NewVersion.MaterialInfoMMS> GetMaterialInfoMMS(string bb_no);
        Task<DateTime> GetEndDateProcessUnit(float timeNow);
        Task<IEnumerable<CommCode>> getListRoutingProccess();
        Task<int> checkStaffIsWorking(string Id);
        Task<int> checkMachineOrMoldIsWorking(string Id);
        Task<WMaterialnfo> FindOneWMaterialInfoLikeTIMS(string bbno);
        Task<WMaterialnfo> FindOneWMaterialInfoLike(string bbno);
    }
}
