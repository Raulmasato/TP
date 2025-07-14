using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using aspgrupo2.Data;
using aspgrupo2.Models;

public class SportsArticlesController : Controller
{
    private readonly AppDbContext _context;
    private const int PageSize = 10; // Items por página

    public SportsArticlesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? page)
    {
        int pageNumber = page ?? 1;

        var articles = await _context.SportsArticles
            .OrderBy(a => a.Nombre)
            .ToPagedListAsync(pageNumber, PageSize);

        return View(articles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SportsArticle article)
    {
        if (ModelState.IsValid)
        {
            _context.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(article);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var article = await _context.SportsArticles.FindAsync(id);
        if (article == null) return NotFound();

        return View(article);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SportsArticle article)
    {
        if (id != article.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(article);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(article.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(article);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var article = await _context.SportsArticles
            .FirstOrDefaultAsync(m => m.Id == id);

        if (article == null) return NotFound();

        return View(article);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var article = await _context.SportsArticles.FindAsync(id);
        if (article != null)
        {
            _context.SportsArticles.Remove(article);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ArticleExists(int id)
    {
        return _context.SportsArticles.Any(e => e.Id == id);
    }
}