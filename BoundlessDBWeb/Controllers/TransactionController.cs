using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BoundlessDBWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using BoundlessDBWeb.Models;

namespace BoundlessDBWeb.Controllers
{
    public class TransactionController : Controller
    {
        BoundlessDbContext context;
        // GET: Transaction
        public ActionResult Index()
        {
            context = HttpContext.RequestServices.GetService(typeof(BoundlessDBWeb.Data.BoundlessDbContext)) as BoundlessDbContext;
            List<Transaction> transactions = context.GetTransactions();
            return View(transactions);
        }

        // GET: Transaction/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            context = HttpContext.RequestServices.GetService(typeof(BoundlessDBWeb.Data.BoundlessDbContext)) as BoundlessDbContext;
            Transaction model = new Transaction();
            model.ItemList = new List<SelectListItem>();
            model.LocationList = new List<SelectListItem>();
            List<string> names = context.GetItemNames();
            List<string> locNames = context.GetLocationNames();
            foreach (var item in names)
            {
                model.ItemList.Add(new SelectListItem { Text = item});
            }
            foreach (string locName in locNames)
            {
                model.LocationList.Add(new SelectListItem { Text = locName, Value = locName });
            }
            model.Date = DateTime.Now;
            return View(model);
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction transaction)
        {
            context = HttpContext.RequestServices.GetService(typeof(BoundlessDBWeb.Data.BoundlessDbContext)) as BoundlessDbContext;
            try
            {
                if (context.SaveTransaction(transaction))
                {
                    ViewData["Message"] = $"Transaction: {transaction.ItemName} was added";
                    Transaction model = new Transaction();
                    model.ItemList = new List<SelectListItem>();
                    model.LocationList = new List<SelectListItem>();
                    List<string> names = context.GetItemNames();
                    List<string> locNames = context.GetLocationNames();
                    foreach (var item in names)
                    {
                        model.ItemList.Add(new SelectListItem { Text = item });
                    }
                    foreach (string locName in locNames)
                    {
                        model.LocationList.Add(new SelectListItem { Text = locName, Value = locName });
                    }
                    model.Date = DateTime.Now;
                    return View(model);

                }
                else
                {
                    return Content("Error connecting to database " + context.ErrorMessage);
                }
            }
            catch
            {
                return Content("There was an error");
            }
        }

        // GET: Transaction/Edit/5
        public ActionResult Edit(int id)
        {
            context = GetContext();
            Transaction transaction = context.GetTransaction(id);
            transaction.ItemList = new List<SelectListItem>();
            transaction.LocationList = new List<SelectListItem>();
            List<string> names = context.GetItemNames();
            List<string> locNames = context.GetLocationNames();
            foreach (var item in names)
            {
                transaction.ItemList.Add(new SelectListItem { Text = item });
            }
            foreach (string locName in locNames)
            {
                transaction.LocationList.Add(new SelectListItem { Text = locName, Value = locName });
            }
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {
            context = GetContext();
            try
            {
                if (context.UpdateTransaction(transaction))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content(context.ErrorMessage);
                }
                
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        // GET: Transaction/Delete/5
        public ActionResult Delete(int id)
        {
            context = HttpContext.RequestServices.GetService(typeof(BoundlessDBWeb.Data.BoundlessDbContext)) as BoundlessDbContext;
            Transaction transaction = context.GetTransaction(id);
            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            context = GetContext();
            try
            {
                if (context.DeleteTransaction(id))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("There was an error interfacing with the database : \n" + context.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        private BoundlessDbContext GetContext()
        {
            return HttpContext.RequestServices.GetService(typeof(BoundlessDBWeb.Data.BoundlessDbContext)) as BoundlessDbContext;
        }
    }
}