﻿using System;
using System.Web.Mvc;
using Telegenic.Entities.Models;
using Telegenic.Entities.ViewModels;
using Telegenic.Repository;
using Telegenic.Repository.Interfaces;

namespace Telegenic.Web.Areas.Admin.Controllers
{
    public class GenreController : Controller
    {
        GenreRepository _genreRepository;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = (GenreRepository)genreRepository;
        }

        // GET: Admin/Genre
        public ActionResult Index()
        {
            var vm = new vmSearch("Search Genre");
            return View("_searchPanel", vm);
        }

        public ActionResult Find()
        {
            var results = _genreRepository.GetAll();

            return PartialView("_gridResultsPanel", results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Find(vmSearch vm)
        {
            var results = !string.IsNullOrWhiteSpace(vm.SearchTerm) ? _genreRepository.GetByTitle(vm.SearchTerm) : _genreRepository.GetAll();

            return PartialView("_gridResultsPanel", results);
        }

        // GET: Admin/Genre/Save
        public ActionResult Save(int? _genreId)
        {
            var vm = new vmEntity();
            vm.Genre = _genreId != 0 ? _genreRepository.GetById(_genreId.GetValueOrDefault()) : new Genre();
            vm.PageHeading = _genreId != null ? string.Format("Edit Genre: {0}", vm.Genre.Title) : string.Format("Add New Genre");

            return PartialView("_savePanel", vm);
        }

        // POST: Admin/Genre/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(vmEntity vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _genreRepository.Save(vm.Genre);
                    return RedirectToAction("Detail", new { _genreId = vm.Genre.Id });
                }
                catch (Exception ex)
                {
                    return PartialView("_savePanel", vm);
                }

            }

            return PartialView("_savePanel", vm);
        }

        public ActionResult Detail(int _genreId)
        {
            var vm = new vmEntity();
            vm.Genre = _genreId > 0 ? _genreRepository.GetById(_genreId) : new Genre();
            vm.PageHeading = string.Format("Genre: {0}", vm.Genre.Title);

            return PartialView("_detailPanel", vm);
        }

        public ActionResult Delete(int _genreId)
        {
            var genre = _genreRepository.GetById(_genreId);
            _genreRepository.Delete(genre);
            return RedirectToAction("Find");
        }
    }
}
