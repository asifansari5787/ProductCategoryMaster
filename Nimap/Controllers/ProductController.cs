using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nimap.Data;
using Nimap.Models;
using System.Linq;

namespace Nimap.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext db;

        public ProductController(DataContext context)
        {
            db = context;
        }

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var products = db.Products.Include("Category").OrderBy(p => p.ProductId);
            var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalCount = products.Count();
            ViewBag.PageSize = pageSize;
            return View(pagedProducts);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return NotFound();
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var product = db.Products.Include("Category").SingleOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
