using Demo.Data;
using Demo.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Demo.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly Web01Context db;

        public MenuLoaiViewComponent(Web01Context context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Categories.Select(loai => new MenuloaiVM
            {
             CatId=   loai.CatId, CatName = loai.CatName, SoLuong= loai.Products.Count
            }).OrderBy(p=> p.CatName);
            return View(data);
        }
    }
}
