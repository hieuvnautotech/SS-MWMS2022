
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Mvc_VD.Controllers.DashBoardQCController;
using static Mvc_VD.Controllers.QCInformationController;

namespace Mvc_VD.Services.Interface.QMS
{
    public interface IQMSService
    {
        #region DashBoard
        Task<IEnumerable<Get_table_info_PQC_D_Model>> GetListFaclineQC(string start, string end, string ml_no);

        Task<IEnumerable<myChart_error_Item_Model_PQC>> GetListFaclineQCModel(string start, string end, string ml_no);

        Task<IEnumerable<Get_table_info_OQC_D_Model>> GetListProductQC(string start, string end, string ml_no);

        Task<IEnumerable<myChart_error_Item_Model_OQC>> GetListProductQCModel(string start, string end, string ml_no);

        Task<IEnumerable<QCItemMaterial>> GetListItemMaterial();

        Task<IEnumerable<CommCode>> GetListItemType(string mt_cd);

        Task<IEnumerable<CommCode>> GetListDefect(string mt_cd);

        #endregion

        #region Quanlity Control Management 

        Task<IEnumerable<QCItemMaterial>> SearchQCItemMaterial(string item_type, string item_cd, string item_nm, string item_exp);

        Task<IEnumerable<QCItemMaterial>> SearchQCItemMaterial(string item_cd, string item_nm, string item_exp);

        Task<IEnumerable<QCItemCheckMaterialResponse>> GetQCCheckMaterial(string item_vcd);

        Task<IEnumerable<QCItemCheckMaterialResponse>> GetQCCheckMaterialByIcNo(int icno);

        Task<IEnumerable<QCItemCheckMaterial>> CheckQCItemCheckMaterial(string item_vcd, string check_id);

        Task<int> InsertIntoQCItemCheckMaterial(QCItemCheckMaterial item);

        Task<IEnumerable<QCItemCheckDetail>> CheckQCItemCheckDetail(string check_id);

        Task<IEnumerable<QCItemCheckDetail>> CheckQCItemCheckDetail(string item_vcd, string check_id);

        Task<int> InsertIntoQCItemCheckDetail(QCItemCheckDetail item);

        Task<QCItemCheckMaterial> GetQCItemCheckMaterialById(int icno);

        Task<int> UpdateQCItemCheckDetailForDel_Yn(int icdno);

        Task<int> UpdateQCItemCheckMaterialForDel_Yn(int icno);

        Task<int> UpdateQCItemCheckMaterial(QCItemCheckMaterial item);

        Task<QCItemCheckDetail>GetQCItemCheckDetailById(int idcno);

        Task<int> UpdateQCItemCheckDetail(QCItemCheckDetail item);

        #endregion

        #region QC Infomation
        Task<IEnumerable<FaclineQC>> GetListFaclineQC(string fq_no, string ml_no, string start, string end);

        Task<IEnumerable<FaclineQCValue>> GetListFaclineQCValue(string fq_no, string item_vcd);

        Task<IEnumerable<ProductQC>> GetListProductQC(string pq_no, string ml_no, string start, string end);

        //Task<IEnumerable<GetQMSNGModel>> GetListDataQMSNG(string productCode, DateTime? date_ymd);

        Task<IEnumerable<ProductActivitionFailedDetailVm>> GetProductActivitionFailed(string productCode, string fromDate, string toDate);


        Task<IEnumerable<ProductActivitionFailedVm>> GetProductActivitionFaileds(string productCode, string fromDate, string toDate);
        Task<IEnumerable<Models.NewVersion.GetQMSNGModel>> GetListClassificationNG(string start_date_ymd, string end_date_ymd, string productCode, string at_no);
        Task<IEnumerable<qc_itemcheck_mt>> Getitemcheck_mt(string item_vcd);
        Task<IEnumerable<qc_itemcheck_dt>> GetitemcheckDetail(string item_vcd, string check_id);
        Task<IEnumerable<MFaclineQCValue>> GetFaclineQCValueDetail(string ProductCode, string at_no,string date_ymd, string shift);
        Task<mFaclineQC> Get_Facline_Qc(string ml_tims);
        Task<string> GetmfaclineQC(string fq_no);
        Task<int> InsertIntoMFaclineQC(m_facline_qc item);
        Task<int> InsertIntoFaclineQCValue(m_facline_qc_value item);
        Task<int> SunNGMFaclineQC(string fq_no);
        Task<m_facline_qc> Top1MFaclineQC(string fq_no);
        Task<int> UpdateMFaclineQC(m_facline_qc item);
        Task<IEnumerable<MFaclineQCValue>> GetFaclineQCValue(string ProductCode, string datetime, string shift);
        Task<m_facline_qc_value> Top1MFaclineQCValueById(int id);
        Task<int> DeleteMFaclineValue(int id);
        #endregion

    }
}

