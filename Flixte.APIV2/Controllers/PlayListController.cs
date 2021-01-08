using Flixte.APIV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flixte.APIV2.Controllers
{
    public class PlayListController : BaseController
    {
        public JsonResult GetPlaylistsByCategory()
        {
            string message = "OK";
            try
            {
                var lists = Core.Services.EstacaoService.FindByFilter(null, true, "cinema");
                return Json(new ResponseModel() { Success = true, Message = message, Data = new List<PlaylistsByCategory>() { new PlaylistsByCategory() { CategoryID = 1, CategoryName = "Tapioca", PlayLists = lists.Where(x => x.Tags.IndexOf("1")!=-1).Take(3).Select(x => new PlayListReturn() { Name = x.Descricao, PlayListID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, new PlaylistsByCategory() { CategoryID = 2, CategoryName = "Tapioca 2", PlayLists = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new PlayListReturn() { Name = x.Descricao, PlayListID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, new PlaylistsByCategory() { CategoryID = 3, CategoryName = "Tapioca 3", PlayLists = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new PlayListReturn() { Name = x.Descricao, PlayListID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() } } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult GetFeaturedPlaylistsByCategory()
        {
            string message = "OK";
            try
            {
                var lists = Core.Services.EstacaoService.FindByFilter(null, true, "cinema");
                return Json(new ResponseModel() { Success = true, Message = message, Data = new List<PlaylistsByCategory>() { new PlaylistsByCategory() { CategoryID = 1, CategoryName = "Tapioca", PlayLists = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new PlayListReturn() { Name = x.Descricao, PlayListID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, new PlaylistsByCategory() { CategoryID = 2, CategoryName = "Tapioca 2", PlayLists = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new PlayListReturn() { Name = x.Descricao, PlayListID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() }, new PlaylistsByCategory() { CategoryID = 3, CategoryName = "Tapioca 3", PlayLists = lists.Where(x => x.Tags.IndexOf("1") != -1).Take(3).Select(x => new PlayListReturn() { Name = x.Descricao, PlayListID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() } } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetMoreFeaturedPlaylists(int count, string alreadyLoadedPlaylists)
        {
            string message = "OK";
            try
            {
                if (string.IsNullOrEmpty(alreadyLoadedPlaylists))
                    alreadyLoadedPlaylists = "";
                string[] already = alreadyLoadedPlaylists.Split(',');
                var list = Core.Services.EstacaoService.FindByFilter(null, false, "").Where(x => already.Contains(x.ID.ToString())).Take(count);
                return Json(new ResponseModel() { Success = true, Message = message, Data = new List<PlaylistsByCategory>() { new PlaylistsByCategory() { CategoryID = 1, CategoryName = "Tapioca", PlayLists = list.Take(count).Select(x => new PlayListReturn() { Name = x.Descricao, PlayListID = x.ID, NextVideoDuration = "12:01", NextVideoThumbURL = x.URLLogo, NextVideoTitle = x.Descricao }).ToList() } } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new ResponseModel() { Success = false, Message = message }, JsonRequestBehavior.DenyGet);
            }
        }


    }
}