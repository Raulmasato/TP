using aspgrupo2.Data;
using aspgrupo2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace aspgrupo2.Controllers;

public class TestController : Controller
{
    private readonly AppDbContext _context;

    public TestController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> InsertTestData()
    {

        try
        {
            var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "admin");

            if (adminUser == null)
            {
                // Si no existe, lo creamos
                using var hmac = new System.Security.Cryptography.HMACSHA512();
                adminUser = new User
                {
                    UserName = "admin",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Admin123")),
                    Salt = hmac.Key,
                    IsActive = true
                };
                _context.Users.Add(adminUser);
            }
            else
            {
                // Si existe, actualizamos
                using var hmac = new System.Security.Cryptography.HMACSHA512();
                adminUser.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Admin123"));
                adminUser.Salt = hmac.Key;
                _context.Users.Update(adminUser);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Contraseña de admin actualizada a 'Admin123'",
                User = adminUser.UserName,
                Salt = Convert.ToBase64String(adminUser.Salt),
                PasswordHash = Convert.ToBase64String(adminUser.PasswordHash)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Error = "Error al actualizar contraseña",
                Details = ex.Message
            });
        }
    }
}