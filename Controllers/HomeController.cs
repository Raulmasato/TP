using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspgrupo2.Data;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Datos para la vista
        ViewBag.TotalArticles = await _context.SportsArticles.CountAsync();
        ViewBag.AvailableStock = await _context.SportsArticles.SumAsync(a => a.Stock);

        var recentArticles = await _context.SportsArticles
            .OrderByDescending(a => a.FechaCreacion)
            .Take(5)
            .ToListAsync();

        ViewBag.RecentArticles = recentArticles;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}