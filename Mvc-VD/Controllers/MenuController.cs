using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mvc_VD.Commons;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using Mvc_VD.Services.Interface;

namespace Mvc_VD.Controllers
{
    public class MenuController : Controller
    {

        private readonly IMenuServices _iMenuServices;
        public MenuController(IMenuServices iMenuServices)
        {

            _iMenuServices = iMenuServices;

        }
        public async Task<ActionResult> Index()
        {
            if (Session["authorize"] != null)
            {
                var menuinfo = await _iMenuServices.GetListMenuInfo();
                return View(menuinfo);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Menu/Details/5

        public async Task<ActionResult> Details(int id = 0)
        {
            if (Session["authorize"] != null)
            {

                var menuinfo = await _iMenuServices.GetMenuInfoById(id);
                //menu_info menu_info = db.menu_info.Find(id);
                if (menuinfo == null)
                {
                    return HttpNotFound();
                }
                return View(menuinfo);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Menu/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Menu/Create

        [HttpPost]
        public async Task<ActionResult> Create(MenuInfo menu_info)
        {
            if (Session["authorize"] != null)
            {
                await _iMenuServices.InsertIntoMenuInfo(menu_info);
                return RedirectToAction("Index");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }


            return View(menu_info);
        }

        //
        // GET: /Menu/Edit/5

        public async Task<ActionResult> Edit(int id = 0)
        {
            if (Session["authorize"] != null)
            {

                var menuinfo = await _iMenuServices.GetMenuInfoById(id);
                //menu_info menu_info = db.menu_info.Find(id);
                if (menuinfo == null)
                {
                    return HttpNotFound();
                }
                return View(menuinfo);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Menu/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(MenuInfo menu_info)
        {
            if (Session["authorize"] != null)
            {
                if (ModelState.IsValid)
                {
                    await _iMenuServices.UpdateMenuInfo(menu_info);
                    return RedirectToAction("Index");
                }
                return View(menu_info);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }

        }

        //
        // GET: /Menu/Delete/5

        public async Task<ActionResult> Delete(int id = 0)
        {
            if (Session["authorize"] != null)
            {

                var menuinfo = await _iMenuServices.GetMenuInfoById(id);
                //menu_info menu_info = db.menu_info.Find(id);
                if (menuinfo == null)
                {
                    return HttpNotFound();
                }
                return View(menuinfo);
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Menu/Delete/5

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["authorize"] != null)
            {

                await _iMenuServices.RemoveMenuInfo(id);
                return RedirectToAction("Index");
            }
            else
            {
                return Json(new { result = false, message = Constant.ErrorAuthorize }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<PartialViewResult> Menu()
        {

            var list = await _iMenuServices.GetListMenuInfo();
            var listY = list.Where(item => item.use_yn == "Y").ToList();
            return PartialView(listY);

        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}