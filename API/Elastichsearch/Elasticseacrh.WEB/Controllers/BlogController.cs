using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogServices _blogServices;

        public BlogController(BlogServices blogServices)
        {
            _blogServices = blogServices;
        }

        public IActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
            var isSucces = await _blogServices.SaveAsync(model);
            if(!isSucces)
            {
                TempData["result"] = "Kayıt başarısız";
                return RedirectToAction(nameof(BlogController.Save));
            }
            TempData["result"] = "Kayıt başarılı";
            return RedirectToAction(nameof(BlogController.Save));
        }
        public async Task<IActionResult> Search()
        {
            return View(await _blogServices.SearchAsync(string.Empty));
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {  
            ViewBag.SearchText = searchText;
            return View(await _blogServices.SearchAsync(searchText));
        }
    }
}
