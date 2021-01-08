using Flixte.APIV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flixte.APIV2.Controllers
{
    public class StationController : BaseController
    {
        public JsonResult List()
        {
            string message = "OK";
            try
            {
                return Json(new ResponseModel() { Success = true, Message = message, Data = Core.Services.EstacaoService.FindAll() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel (){  Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult GetHomeData()
        {
            string message = "OK";
            try
            {
                var lists = Core.Services.EstacaoService.FindByFilter(null, true, "cinema");
                return Json(new ResponseModel() { Success = true, Message = message, Data = new HomeReturn() { StationsByCategory = new List<StationsByCategory>() { new StationsByCategory() { CategoryID = 1, CategoryName = "Tapioca", Stations = lists.Where(x=>x.Tags.IndexOf("1")!=-1).Take(3).Select(x => new StationListReturn() { Name = x.Descricao, StationID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, new StationsByCategory() { CategoryID = 2, CategoryName = "Tapioca 2", Stations = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new StationListReturn() { Name = x.Descricao, StationID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, new StationsByCategory() { CategoryID = 3, CategoryName = "Tapioca 3", Stations = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new StationListReturn() { Name = x.Descricao, StationID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() } }, PlaylistsByCategory = new List<PlayListReturn>() { new PlayListReturn() { Name = "Tapioca1", NextVideoDuration = "11:1", NextVideoThumbURL = "", NextVideoTitle = "", PlayListID = 1 } }, FeaturedPlaylists = new List<PlayListReturn>() { new PlayListReturn() { Name = "P1", PlayListID = 2, NextVideoTitle = "PPPPP333", NextVideoThumbURL = "", NextVideoDuration = "12:12" } } } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult GetStationsByCategory()
        {
            string message = "OK";
            try
            {
                var lists = Core.Services.EstacaoService.FindByFilter(null, true, "cinema");
                return Json(new ResponseModel() { Success = true, Message = message, Data = new List<StationsByCategory>() { new StationsByCategory() { CategoryID = 1, CategoryName = "Tapioca", Stations = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new StationListReturn() { Name = x.Descricao, StationID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, new StationsByCategory() { CategoryID = 2, CategoryName = "Tapioca 2", Stations = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new StationListReturn() { Name = x.Descricao, StationID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, new StationsByCategory() { CategoryID = 3, CategoryName = "Tapioca 3", Stations = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new StationListReturn() { Name = x.Descricao, StationID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() } } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
       
        
        public JsonResult GetMoreByCategory(int categoryID, int count, string alreadyLoadedStations)
        {
            string message = "OK";
            try
            {
                if (string.IsNullOrEmpty(alreadyLoadedStations))
                    alreadyLoadedStations = "-1";
                string[] already = alreadyLoadedStations.Split(',');
                var list = Core.Services.EstacaoService.FindByFilter(null, false, categoryID.ToString()).Where(x => already.Contains(x.ID.ToString())).Take(count);
                return Json(new ResponseModel() { Success = true, Message = message, Data = new List<StationsByCategory>() { new StationsByCategory() { Stations = list.Take(count).Select(x => new StationListReturn() { Name = x.Descricao, StationID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() } } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult GetFeaturedStations(int count = 10)
        {
            string message = "OK";
            try
            {
                return Json(new ResponseModel() { Success = true, Message = message, Data = Core.Services.EstacaoService.FindByFilter(null, true, null).Take(count).Select(x => new StationListReturn() { Name = x.Descricao, StationID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult ListSelf()
        {
            string message = "OK";
            try
            {
                return Json(new ResponseModel() { Success = true, Message = message, Data = Core.Services.EstacaoService.FindAll() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult Create(RequestModel<Core.Models.Estacao> station)
        {
            string message = "OK";
            try
            {
                if (string.IsNullOrEmpty(station.Data.Nome))
                    message = "Nome é obrigatório";
                station.Data.Ativo = true;
                station.Data.Data = DateTime.Now;
                station.Data.Destaque = false;
                station.Data.Excluido = false;
                station.Data.Users = 0;
                station.Data.Views = 0;
                station.Data.IDUsuario = GetUserID();
                Flixte.Core.Services.EstacaoService.Insert(station.Data);
                return Json(new ResponseModel() { Success = true, Message = message }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult Update(RequestModel<Core.Models.Estacao> station)
        {
            string message = "OK";
            try
            {
                if (string.IsNullOrEmpty(station.Data.Nome))
                    message = "Nome é obrigatório";                
                Flixte.Core.Services.EstacaoService.Update(station.Data);
                return Json(new ResponseModel() { Success = true, Message = message }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult Delete(RequestModel<int?> id)
        {
            string message = "OK";
            try
            {
                if ((id==null) || (id.Data == null))
                    throw new Exception("Access Denied");
                var model = Core.Services.EstacaoService.GetEstacao(id.Data.Value);
                if (model.IDUsuario == GetUserID())
                    Flixte.Core.Services.EstacaoService.Delete(model.ID);
                else
                    throw new Exception("Access Denied");
                return Json(new ResponseModel() { Success = true, Message = message }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult Rate(RequestModel<Core.Models.EstacaoRate> estacao)
        {
            string message = "OK";
            try
            {
                if ((estacao == null) || (estacao.Data == null))
                    throw new Exception("Access Denied");
                var model = Core.Services.EstacaoService.GetEstacao(estacao.Data.ID);
                Flixte.Core.Services.EstacaoService.Rate(model.ID, estacao.Data.Rate, GetUserID());
                return Json(new ResponseModel() { Success = true, Message = message }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult Follow(RequestModel<int?> id)
        {
            string message = "OK";
            try
            {
                if ((id == null) || (id.Data == null))
                    throw new Exception("Access Denied");
                var model = Core.Services.EstacaoService.GetEstacao(id.Data.Value);
                Flixte.Core.Services.EstacaoService.Follow(model.ID, GetUserID());
                return Json(new ResponseModel() { Success = true, Message = message }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel(){ Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}