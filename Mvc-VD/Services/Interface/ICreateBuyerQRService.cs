using Mvc_VD.Classes;
using Mvc_VD.Models.NewVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Services.Interface
{
    public interface ICreateBuyerQRService
    {
        Task<IEnumerable<StampMaster>> GetAllStampMaster();

        Task<IEnumerable<BuyerQRModel>> GetStamp(int id);

        Task<int> CountListStampDetail( string tempQR);

        Task<int> InsertStampDetail(List<StampDetail> item);

        Task<IEnumerable<StampDetail>> GetListStampDetailByCurrent(int From, int To);

        Task<IEnumerable<BuyerQRModel>> GetStampNameByCode(string stamp_code);
        Task<ListIntModel> GetCountNumberBuyer(string tempQR, string shift);
        Task<IEnumerable<BuyerQRModel>> InsertStampDetail2(List<StampDetail> item);
    }
}