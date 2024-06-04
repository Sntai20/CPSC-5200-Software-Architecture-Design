namespace Identity.API.Controllers;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eShop.Identity.API.Data;
using eShop.Identity.API.Models.AccountViewModels;

public class RegisterController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegisterController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    // GET: Register
    public async Task<IActionResult> Index()
    {
        return View(await _context.RegisterViewModel.ToListAsync());
    }

    // GET: Register/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var registerViewModel = await _context.RegisterViewModel
            .FirstOrDefaultAsync(m => m.Id == id);
        if (registerViewModel == null)
        {
            return NotFound();
        }

        return View(registerViewModel);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                CardNumber = model.CardNumber,
                SecurityNumber = model.SecurityNumber,
                Expiration = model.Expiration,
                CardHolderName = model.CardHolderName,
                CardType = model.CardType,
                Street = model.Street,
                City = model.City,
                State = model.State,
                Country = model.Country,
                ZipCode = model.ZipCode,
                Name = model.Name,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    // GET: Register/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Register/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OriginalCreate([Bind("Id,Email,Password,ConfirmPassword")] RegisterViewModel registerViewModel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(registerViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(registerViewModel);
    }

    // GET: Register/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var registerViewModel = await _context.RegisterViewModel.FindAsync(id);
        if (registerViewModel == null)
        {
            return NotFound();
        }
        return View(registerViewModel);
    }

    // POST: Register/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,ConfirmPassword")] RegisterViewModel registerViewModel)
    {
        if (id != registerViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(registerViewModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisterViewModelExists(registerViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(registerViewModel);
    }

    // GET: Register/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var registerViewModel = await _context.RegisterViewModel
            .FirstOrDefaultAsync(m => m.Id == id);
        if (registerViewModel == null)
        {
            return NotFound();
        }

        return View(registerViewModel);
    }

    // POST: Register/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var registerViewModel = await _context.RegisterViewModel.FindAsync(id);
        if (registerViewModel != null)
        {
            _context.RegisterViewModel.Remove(registerViewModel);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RegisterViewModelExists(int id)
    {
        return _context.RegisterViewModel.Any(e => e.Id == id);
    }
}
