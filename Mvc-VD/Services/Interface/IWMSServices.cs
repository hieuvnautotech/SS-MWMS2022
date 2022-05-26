using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using Mvc_VD.Models.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_VD.Services.Interface
{
    public interface IWMSServices
    {
        Task<IReadOnlyList<WMaterialInfo>> GetlistMaterialInfo(string MtNo, string StyleNo, int intpage, int introw);
        Task<int> UpdateSDinfo(int alert, string status, string chg_id, string sd_no, DateTime chg_dt);
        Task<int> InsertShippingdMaterial(shippingsdmaterial item);
        Task<string> GetDMaterialInfo(int idmaterial);
        Task<int> CheckShippingMaterial(string SdNo, string MtNo);
        Task<int> UpdateDuplicateShipping(string SdNo, string MtNo, string ChangeId, DateTime ChangeDate);
        Task<IReadOnlyList<PickingScanResponse>> GetListMaterialInfoBySdNo(string SdNo, string MtNo);
        Task<IReadOnlyList<WMaterialInfo>> GetListPickingScanBySdNo(string sd_no, string MtNo);
        Task<IReadOnlyList<shippingsdmaterial>> GetListShippngMaterial(string SdNo, string MtNo);
        Task<IEnumerable<MbInfo>> GetListMbInfo(string Id);
        Task<SdInfo> GetListSdInfoById(int Id);
        Task<string> GetListDatabySdNo(string sd_no);
        Task<int> UpdateSdInfoTable(SdInfo item);
        Task<int> DeleteSdInfo(int sid);
        Task<int> DeleteShippingSDInfo(string sd_no);
        Task<IEnumerable<MaterialInfoMemo>> GetListMaterialInfoMemo(string sd_no, string md_cd, string lot_no, string style_no);
        Task<IEnumerable<StyleInfo>> GetListDataStyleInfo(string style_no);
        Task<int> InsertIntoMaterialInfoMemo(MaterialInfoMemo item);
        Task<MaterialInfoMemo> GetMaterialInfoMemoById(int Id);
        Task<int> RemoveMaterialInfoMemo(int Id);
        Task<int> UpdateMaterialInfoMemo(MaterialInfoMemo item);
        Task<MaterialInfoTam> CheckMaterialInfoTam(string mt_cd);
        Task<shippingsdmaterial> CheckMaterialInfoTam(int id);
        Task<int> UpdateshippingMaterial(int id, int Qty);
        Task<IEnumerable<MaterialInfoMMS>> GetListWMaterial(string sd_no, string mt_no);
        Task<IEnumerable<WMaterialInfoResponse>> GetListMaterialInfoBySdNo(string sd_no);
        Task<IEnumerable<SdInfo>> GetPickingScanPP(string sd_no);
        Task<IEnumerable<MaterialInfoMemo>> GetPickingScanPP_Memo(string sd_no);
        Task<IEnumerable<MaterialInfoTam>> GetPickingScanListPP(string sd_no);
        Task<IEnumerable<GetPickingScanPPCountMtNoResponse>> GetPickingScanPP_Count_MT_no(string sd_no);
        Task<int> UpdateMaterialInfoTam(string wmtid, string sd_no, string time);
        Task<int> UpdateMaterialInfoTam(string mt_cd, string sd_no);
        Task<int> UpdateSdNoInfo(string sd_no, int alert, string status);
        Task<int> UpdateMaterialInfoTam(MaterialInfoTam Item);
        Task<MaterialInfoTam> CountMaterialInfoTamByMtCd(string mt_cd);
        Task<MaterialInfoTam> CountMaterialInfoTamBySdNo(string sd_no);
        Task<IEnumerable<MaterialInfoTam>> GetListDataMaterialInfoTam(string mt_no, string mt_cd);
        Task<MaterialInfoTam> GetMaterialInfoTamById(int id);
        Task<IEnumerable<MaterialInfoTam>> GetMaterialInfoTamByMtCd(string mt_cd);
        Task<int> CheckMaterialInfo(string mt_no);
        Task<int> DeleteShippingMaterial(int id);
        Task<int> CountListMaterialInfo(string MtNo, string StyleNo);
        Task<int> UpdateshippingMeterMaterial(int id, int Qty);
        Task<string> CheckMaterialInfoToUpload(string mt_no);
        Task<IEnumerable<MaterialShippingMemo>> GetListSearchShowMemo(string datemonth, string product, string material, string date);
        Task<IEnumerable<MaterialShipping>> GetListSearchShowMaterialShipping(string style_no, string mt_no, string recei_dt, string datemonth);
        Task<IEnumerable<MaterialShipping>> getShowMaterialShippingDetail(string style_no, string mt_no, string recei_dt, string datemonth);

    }
}
