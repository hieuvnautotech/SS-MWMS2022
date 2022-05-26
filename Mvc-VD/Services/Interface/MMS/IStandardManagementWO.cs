using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Interface
{
    public interface IStandardManagementWO
    {
        #region Standard Management (WO)

        #region WO Common
        Task<IEnumerable<CommMt>> GetListCommMT();
        Task<CommMt> GetCommMTById(int mt_id);
        Task<int> CheckCommMT();
        Task<IEnumerable<WOCommonResponse>> GetListComMTDetail(string mt_cd);
        Task<IEnumerable<CommMt>> SearchCommMT(string mt_cd, string mt_nm, string mt_exp);
        Task<int> checkCommDT(string mt_cd, string dt_cd);
        Task<int> InsertIntoCommDT(CommCode item);
        Task<int> UpdateCommDT(CommCode item);
        Task<int> DeleteCommDT(int cdid);
        Task<int> InsertIntoCommMT(CommMt item);
        Task<int> UpdateCommMT(CommMt item);
        Task<int> DeleteCommMT(int mt_id);
        Task<CommCode> GetCommDTById(int cdid);
        #endregion

        #region Work Policy
        Task<IEnumerable<PolicyMT>> GetListPolicyMT();
        Task<int> CountListPolicyMT();
        Task<int> InsertIntoPolicyMT(PolicyMT item);
        Task<PolicyMT> GetPolicyMTById(int wid);
        Task<int> DeletePolicyMTById(int wid);
        Task<int> UpdatePolicy(PolicyMT item);
        #endregion

        #endregion

        #region WIP

        #region Inverntory
        Task<IEnumerable<LocationInfo>> GetListLocationInfo();
        Task<IEnumerable<LocationInfo>> SearchLocationInfọ(string lct_cd, string lct_nm);
        Task<LocationInfo> GetDataLocationInfoById(int lct_no);
        Task<IEnumerable<LocationInfo>> GetListLocationInfoByCode(string lct_cd);
        Task<int> UpdateDataLocationInfo(LocationInfo item);
        Task<int> InsertIntoDataLocationInfo(LocationInfo item);
        Task<int> DeleteDataLocationInfoById(int lctno);


        Task<IEnumerable<LocationInfo>> GetListLocationInfo(string lct_cd);
        #endregion

        #endregion
    }
}