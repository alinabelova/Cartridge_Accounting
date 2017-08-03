﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CartridgeAccounting.DAL;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using CartridgeAccounting.DAL.Models;
using CartridgeAccounting.WEB.Models;
using HttpVerbs = System.Web.Mvc.HttpVerbs;

namespace CartridgeAccounting.WEB.Controllers
{
    public class CartridgesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<CartridgeType> _cartridgeTypes;
        
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// загрузка списка картриджей
        /// </summary>
        /// <param name="DataSourceRequest">request</param>
        /// <returns></returns>
        public ActionResult Cartridges_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = null;
            try
            {
                IQueryable<Cartridge> cartridges = db.Cartridges;
                _cartridgeTypes = db.CartridgeTypes.ToList();
                result = cartridges.ToDataSourceResult(request, cartridge => new
                {
                    Id = cartridge.Id,
                    Creation = cartridge.Creation,
                    DateOfChange = cartridge.DateOfChange,
                    Type = cartridge.Type?.Name,
                    Model = cartridge.Model,
                    CompatiblePrinter = cartridge.CompatiblePrinter,
                    Color = cartridge.Color,
                    Resource = cartridge.Resource,
                    WriteOff = cartridge.WriteOff
                });
                return Json(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return null;
            }
        }

     
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cartridges_Update([DataSourceRequest]DataSourceRequest request, Cartridge cartridge)
        {
            try
            {
                var cartridgeForUpdate = db.Cartridges.FirstOrDefault(c => c.Id == cartridge.Id);
                if (cartridgeForUpdate != null)
                {
                    cartridgeForUpdate.WriteOff = cartridge.WriteOff;
                    cartridgeForUpdate.DateOfChange = DateTime.Now;
                    db.Cartridges.Attach(cartridgeForUpdate);
                    db.Entry(cartridgeForUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Cartridges_Read(request);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return null;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cartridges_Destroy([DataSourceRequest]DataSourceRequest request, Cartridge cartridge)
        {
            try
            {
                var cartridgeForRemove = db.Cartridges.FirstOrDefault(c => c.Id == cartridge.Id);
                if (cartridgeForRemove != null)
                {
                    db.Cartridges.Remove(cartridgeForRemove);
                    db.SaveChanges();
                }
                return Json(new[] { cartridge }.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return null;
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Create()
        {
            LoadDataSources();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Create(CartridgeViewModel cartridgeViewModel)
        {
            if (!ModelState.IsValid)
            {
                LoadDataSources();
                return View(cartridgeViewModel);
            }
            var cartridge = new Cartridge
            {
                Id = cartridgeViewModel.Id,
                Creation = DateTime.Now,
                DateOfChange = DateTime.Now,
                Model = cartridgeViewModel.Model,
                CompatiblePrinter = cartridgeViewModel.CompatiblePrinter,
                Color = cartridgeViewModel.Color,
                Resource = cartridgeViewModel.Resource,
                WriteOff = cartridgeViewModel.WriteOff,
                Type = db.CartridgeTypes.FirstOrDefault(ct => cartridgeViewModel.TypeIds.Contains(ct.Id)),
            };
            try
            {
                db.Cartridges.Add(cartridge);
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException exc)
            {
                foreach (DbEntityValidationResult result in exc.EntityValidationErrors)
                {
                    foreach (DbValidationError error in result.ValidationErrors)
                    {
                        ModelState.AddModelError("", error.PropertyName + ":" + error.ErrorMessage);
                    }
                }
                LoadDataSources();
            }
            return View("Index");
        }

        private void LoadDataSources()
        {
            IEnumerable<SelectListItem> typeId = db.CartridgeTypes
             .Select(s => new SelectListItem
             {
                 Value = s.Id.ToString(),
                 Text = s.Name
             });
            ViewBag.Types = typeId;
        }
    }
}
