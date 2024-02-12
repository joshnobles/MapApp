using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecureSoftware.DataAccess;
using SecureSoftware.Entities;

namespace SecureSoftware.Pages
{
    public class MapModel : PageModel
    {
        private readonly Context _context;
        public MapModel(Context context)
        {
            _context = context;
        }

        public User? CurrentUser { get; set; }

        public Service Service = new();

        public async Task<IActionResult> OnGet()
        {
            var IdUser = HttpContext.Session.GetInt32("IdUser");

            if (IdUser is null)
                return Redirect("/Index?session=expired");

            CurrentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == IdUser);

            if (CurrentUser is null)
                return Redirect("/Index");

            CurrentUser.Username = await Service.DecryptStringAsync(CurrentUser.Username);

            return Page();
        }

        public async Task<JsonResult> OnGetSavePoint()
        {
            var IdUser = HttpContext.Session.GetInt32("IdUser");

            if (IdUser is null)
                return new JsonResult("failure");

            var lat = Request.Query["lat"].ToString();
            var lng = Request.Query["lng"].ToString();
            var name = Request.Query["name"].ToString();
            var desc = Request.Query["desc"].ToString();

            var newPoint = new MapPoint()
            {
                IdUser = (int)IdUser,
                PointName = await Service.EncryptStringAsync(name),
                PointDesc = await Service.EncryptStringAsync(desc),
                Lat = await Service.EncryptStringAsync(lat),
                Lng = await Service.EncryptStringAsync(lng)
            };

            await _context.MapPoints.AddAsync(newPoint);
            await _context.SaveChangesAsync();

            return new JsonResult(newPoint.IdMapPoint);
        }

        public async Task<JsonResult> OnGetPoints()
        {
            var IdUser = HttpContext.Session.GetInt32("IdUser");

            if (IdUser is null)
                return new JsonResult("failure");

            var pointList = await _context.MapPoints.Where(m => m.IdUser == IdUser).ToListAsync();

            foreach (var p in pointList)
            {
                p.PointName = await Service.DecryptStringAsync(p.PointName);
                p.PointDesc = await Service.DecryptStringAsync(p.PointDesc);
                p.Lat = await Service.DecryptStringAsync(p.Lat);
                p.Lng = await Service.DecryptStringAsync(p.Lng);
            }

            return new JsonResult(pointList);
        }

        public async Task<JsonResult> OnGetDelete()
        {
            var IdUser = HttpContext.Session.GetInt32("IdUser");

            if (IdUser is null)
                return new JsonResult("failure");

            var IdMapPoint = Convert.ToInt32(Request.Query["IdMapPoint"]);

            var Point = await _context.MapPoints.FirstOrDefaultAsync(m => m.IdUser == IdUser && m.IdMapPoint == IdMapPoint);

            if (Point is null)
                return new JsonResult("failure");

            _context.MapPoints.Remove(Point);
            await _context.SaveChangesAsync();

            return new JsonResult(string.Empty);
        }

    }
}
