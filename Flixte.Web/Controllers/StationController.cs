using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flixte.Web.Controllers
{
    public class StationController : BaseController
    {
        // GET: Station
        public ActionResult Index()
        {
            ViewData["ListaRetorno"] = string.Join(",",Flixte.Core.Integration.YoutubeService.FindByFilter("tapioca").ToArray());
            return View(Flixte.Core.Services.EstacaoService.FindAllWithInactive());
        }

        // GET: Station/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Station/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Station/Create
        [HttpPost]
        public ActionResult Create(Estacao model)
        {
            try
            {
                model.Ativo = true;
                model.Data = DateTime.Now;
                model.Destaque = false;
                model.Excluido = false;
                
                model.IDUsuario = GetUserID();
                model.Privado = false;
                model.Senha = null;
                model.Tags = null;
                model.URLLogo = null;
                model.Users = 0;
                model.Views = 0;
                Flixte.Core.Services.EstacaoService.Insert(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                return View();
            }
        }

        // GET: Station/Edit/5
        public ActionResult Edit(int id)
        {
            return View(Flixte.Core.Services.EstacaoService.GetEstacao(id));
        }

        // POST: Station/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Estacao model)
        {
            try
            {
                model.ID = id;
                Flixte.Core.Services.EstacaoService.Update(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Station/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Station/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Flixte.Core.Services.EstacaoService.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
