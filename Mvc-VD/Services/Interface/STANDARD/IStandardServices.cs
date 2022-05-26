using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Interface
{
    public interface IStandardServices
    {
        #region Standard Infomation
       
        #region Supplier Infomation
        Task<IEnumerable<CommCode>> GetListCountrỵ();
        Task<IEnumerable<SupplierInfo>> GetListSupplierInfo();
        Task<IEnumerable<SupplierInfo>> SearchSupplierInfo(string codeData, string nameData, string bsn_searchData);
        Task<int> InsertSupplierInfo(SupplierInfo item);
        Task<int> UpdateSupplierInfo(SupplierInfo item);
        Task<int> RemoveSupplierInfo(int spno);
        Task<int> CheckSupplierInfo(string sp_cd);
        Task<SupplierInfo> GetSupplierInfoById(int spno);

        #endregion

        #region Buyer
        Task<IEnumerable<BuyerInfo>> GetListBuyerInfo();
        Task<IEnumerable<BuyerInfo>> SearchBuyerInfo(string buyer_cd, string buyer_nm);
        Task<int> CheckBuyerInfo(string buyer_cd);
        Task<int> InsertIntoBuyerInfo(BuyerInfo item);
        Task<int> RemoveBuyerInfo(int byno);
        Task<int> UpdateBuyerInfo(BuyerInfo item);
        Task<BuyerInfo> GetBuyerInfoById(int byno);
        #endregion

        #region Supplier Type Infomation
        Task<IEnumerable<CommCode>> GetListSupplierType(string mt_cd);

        Task<IEnumerable<CommCode>> SearchSupplierType(string dt_cd, string dt_nm);

        Task<int> CheckSupplierType(string dt_cd, string mt_cd);

        Task<int> ÍnertIntoSupplierType(CommCode item);

        Task<int> RemoveSupplierType(int cdid);

        Task<int> UpdateSupplierType(CommCode item);

        Task<CommCode> GetSupplierTypeById(int cdid);

        #endregion

        #endregion

        #region System Management

        #region Authority Management
        Task<IEnumerable<AuthorInfo>> GetListAuthorInfo(string code, string name);
        Task<IEnumerable<MbInfo>> GetListMbInfo(string userid, string uname);
        Task<IEnumerable<MenuInfo>> GetListMenuInfo(string mn_cd, string mn_full, string at_cd);
        Task<int> RemoveMenuInfo(int Id);
        Task<int> RemoveMbMenuInfo(int Id);
        Task<IEnumerable<AuthorMenuInfo>> GetListAuthorMenuInfo(string at_cd);
        Task<IEnumerable<CommCode>> GetRole();
        Task<IEnumerable<MbAuthorInfo>> GetMbAuthorInfoByCode(string at_cd);
        Task<string> GetNameFromAuthorInfo(string at_cd);
        Task<int> CountMbAuthorInfo(string userid);
        Task<MbAuthorInfo> GetMbAuthorInfo(string userid);
        //Task<int> InsertIntoAuthorMenuInfo(string at_cd, DateTime time, string mn_id);
        Task<int> InsertIntoAuthorMenuInfo(string at_cd, DateTime time, string role, string mn_cd);
        Task<List<menu_info>> getmenuinfo(string at_cd, string kq);
        Task<int> RemoveAuthorMenuInfoWithAtCd(string at_cd);
        Task<int> RemoveAuthorMenuInfo(string bo_check, string at_cd);
        Task<int> CountAuthorInfo(string at_nm);
        Task<int> InsertIntoAuthorInfo(AuthorInfo item);
        Task<int> RemoveAuthorInfo(int Id);
        Task<int> RemoveAuthorMenuInfo(int Id);
        Task<int> RemoveMbAuthorInfo(int Id);
        Task<int> UpdateAuthorInfo(AuthorInfo item);
        Task<AuthorInfo> GetAuthorInfoById(int Id);
        Task<AuthorInfo> GetAuthorInfoByAt_Code(string at_cd);
        #endregion

        #region Common Management
        Task<IEnumerable<CommMt>> GetListCommonMT();
        Task<int> CountCommMTByName(string mt_name);
        Task<int> CountCommMtByCode();
        Task<IEnumerable<CommMt>> GetListCommMtByCode();
        Task<int> RemoveCommMt(int mt_id);
        Task<IEnumerable<GetDataCommDtResponse>> GetListDataCommDt(string mt_cd);
        Task<IEnumerable<CommCode>> GetListCommDT(string mt_cd);
        Task<int> RemoveCommDT(int cdid);
        Task<CommCode> GetCommCodeById(string dt_cd, string mt_cd);
        #endregion

        #region Manual Management

        Task<NoticeBoard> GetNoticeBoardById(int bno);
        Task<IEnumerable<NoticeBoard>> GetListNoticeBoard();
        Task<IEnumerable<NoticeBoard>> SearchDataNoticeBoard1(string title, string mn_cd);
        Task<IEnumerable<NoticeBoard>> SearchDataNoticeBoard2(string title, string mn_cd, string lng_cd);
        Task<int> CheckDataNoticeBoard(string mn_cd, string lng_cd);
        Task<IEnumerable<NoticeBoard>> GetListDataNoticeBoard(string mn_cd, string lng_cd);
        Task<int> InsertIntoNoticeBoard(NoticeBoard item);
        Task<int> UpdateNoticeBoard(NoticeBoard item);
        Task<int> DeleteNoticeBoard(int bno);
        Task<IEnumerable<NoticeBoard>> GetListNoticeBoardByMenuCode(string mn_cd);

        #endregion

        #endregion

    }
}