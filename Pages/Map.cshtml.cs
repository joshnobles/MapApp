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

        public List<MapPoint>? MapPoints { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var IdUser = HttpContext.Session.GetInt32("IdUser");

            if (IdUser is null)
                return Redirect("/Index?session=expired");

            CurrentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == IdUser);

            if (CurrentUser is null)
                return Redirect("/Index");

            CurrentUser.Username = await Service.DecryptStringAsync(CurrentUser.Username);

            MapPoints = await _context.MapPoints.Where(m => m.IdUser == CurrentUser.IdUser).ToListAsync();

            return Page();
        }

        public async Task<JsonResult> OnGetSavePoint()
        {
            var IdUser = HttpContext.Session.GetInt32("IdUser");

            if (IdUser is null)
                return new JsonResult("failure");

            var lat = float.Parse(Request.Query["lat"].ToString());
            var lng = float.Parse(Request.Query["lng"].ToString());
            var name = Request.Query["name"].ToString();
            var desc = Request.Query["desc"].ToString();

            var newPoint = new MapPoint()
            {
                IdUser = (int)IdUser,
                PointName = name,
                PointDesc = desc,
                Lat = lat,
                Lng = lng
            };

            await _context.MapPoints.AddAsync(newPoint);
            await _context.SaveChangesAsync();

            return new JsonResult("success");
        }

        public async Task<JsonResult> OnGetPoints()
        {
            var IdUser = HttpContext.Session.GetInt32("IdUser");

            if (IdUser is null)
                return new JsonResult("failure");

            var t = new JsonResult(await _context.MapPoints.Where(m => m.IdUser == IdUser).ToListAsync());

            return new JsonResult(await _context.MapPoints.Where(m => m.IdUser == IdUser).ToListAsync());
        }

    }
}
