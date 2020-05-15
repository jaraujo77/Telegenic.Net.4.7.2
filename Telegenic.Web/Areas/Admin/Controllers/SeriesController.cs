﻿using System;
using System.Linq;
using System.Web.Mvc;
using Telegenic.Entities.Models;
using Telegenic.Entities.ViewModels;
using Telegenic.Repository;
using Telegenic.Repository.Interfaces;

namespace Telegenic.Web.Areas.Admin.Controllers
{
    public class SeriesController : Controller
    {
        SeriesRepository _seriesRepository;
        GenreRepository _genreRepository;

        public SeriesController(ISeriesRepository seriesRepository, IGenreRepository genreRepository)
        {
            _genreRepository = (GenreRepository)genreRepository; 
            _seriesRepository = (SeriesRepository)seriesRepository;
        }

        // GET: Admin/SingleSeries - load search panel
        public ActionResult Index()
        {
            var vm = new vmSearch("Search Series");
            return View("_searchPanel", vm);
        }

        //[HttpPost]
        //public ActionResult Index(vmSearch vm)
        //{
        //    return View();
        //}

        //public ActionResult Find()
        //{
        //    var results = _seriesRepository.GetAll();

        //    return PartialView("_gridResultsPanel", results);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Find(vmSearch vm)
        {
            var results = !string.IsNullOrWhiteSpace(vm.SearchTerm) ? _seriesRepository.GetByTitle(vm.SearchTerm) : _seriesRepository.GetAll();

            return PartialView("_gridResultsPanel", results);
        }

        // GET: Admin/Series/Save
        public ActionResult Save(int? id)
        {
            var vm = new vmEntity(_genreRepository.GetAll().OrderBy(x => x.Title));
            vm.Series = id != 0 ? _seriesRepository.GetById(id.GetValueOrDefault()) : new Series();
            vm.PageHeading = id != null ? string.Format("Edit Series: {0}", vm.Series.Title) : string.Format("Add New Series");

            return PartialView("_savePanel", vm);
        }

        // POST: Admin/Series/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(vmEntity vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _seriesRepository.Save(vm.Series);
                    return RedirectToAction("Detail", new { id = vm.Series.Id });
                }
                catch (Exception ex)
                {
                    return PartialView("_savePanel", vm);
                }

            }

            return PartialView("_savePanel", vm);
        }

        public ActionResult Detail(int id)
        {
            var vm = new vmEntity(_genreRepository.GetAll().OrderBy(x => x.Title));
            vm.Series = id > 0 ? _seriesRepository.GetById(id) : new Series();
            vm.PageHeading = string.Format("Series: {0}", vm.Series.Title);

            return PartialView("_detailPanel", vm);
        }

        public ActionResult Delete(int id)
        {
            var series = _seriesRepository.GetById(id);
            _seriesRepository.Delete(series);
            return RedirectToAction("Find");
        }
    }
}
