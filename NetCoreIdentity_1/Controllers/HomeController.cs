using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NetCoreIdentity_1.Models;
using NetCoreIdentity_1.Models.Entities;
using NetCoreIdentity_1.Models.ViewModels.AppUsers.PureVms;
using System.Diagnostics;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace NetCoreIdentity_1.Controllers
{
    [AutoValidateAntiforgeryToken]//Get ile gelen sayfada verilen �zel bir token sayesinde Post'un bu tokensiz yap�lamamas�n� saglar...PostMan gibi third part software'lerinden g�zlemlediginizde direkt Post taraf�na ulasamad�g�n�z� g�receksiniz...
    public class HomeController : Controller
    {
        //Identity k�t�phanesi crud ve servis i�lemleri icin bir tak�m class'lara sahiptir... Bu Manager Class'lar� sizin ilgili Identity yap�lar�n�z�n Crud i�lemlerine ve baska business logic i�lemlerine girmesini saglarlar...
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            if (ModelState.IsValid)
            {
                //Mapping i�lemi (UserRegisterRequestModel tipindeki model'deki bilgileri AppUser class'�ndan instance alarak oraya aktar�yoruz)
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);//�ifreyi burada vermemizin sebebi �ifrenin Identity taraf�ndan otomatik �ifrelenmesi i�in buray� kullanmam�z gerkiyor.

                if (result.Succeeded)
                {
                    #region AdminEklemekIcinTekKullanimlikKodlar
                    //AppRole role = await _roleManager.FindByNameAsync("Admin"); //Admin ismindeki rolu bulabilirse Role nesnesini appRole'e atacak bulamazsa appRole null olacak
                    //if (role == null) await _roleManager.CreateAsync(new() { Name = "Admin" });//Admin isminde bir rol yaratt�k

                    //await _userManager.AddToRoleAsync(appUser, "Admin"); //appUser degi�keninin tuttugu kullan�c� nesnesini Admin isimli Role'e ekledik...

                    #endregion

                    #region MemberEklemekIcinKodlar
                    AppRole role = await _roleManager.FindByNameAsync("Member");
                    if (role == null) await _roleManager.CreateAsync(new() { Name = "Member" });

                    await _userManager.AddToRoleAsync(appUser, "Member"); //Register olan kullan�c� art�k bu kod sayesinde direkt Member rol�ne sahip olacakt�r... 
                    #endregion
                    return RedirectToAction("Register");

                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


            }
            return View(model);
        }

        public IActionResult SignIn(string returnUrl)
        {
            UserSignInRequestModel usModel = new()
            {
                ReturnUrl = returnUrl
            };
            return View(usModel);
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInRequestModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByNameAsync(model.UserName); 

                SignInResult result = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    IList<string> roles = await _userManager.GetRolesAsync(appUser);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel"); //Eger gitmek istediginiz sayfa bir baska Area'da ise routeData parametresine Anonymus type olarak arg�man vererek g�nderirsiniz return RedirectToAction("AdminPanel",new {Area = "Admin"})
                    }
                    else if (roles.Contains("Member"))
                    {
                        return RedirectToAction("MemberPanel");
                    }

                    return RedirectToAction("Panel");
                }
                else if (result.IsLockedOut)
                {
                    DateTimeOffset? lockOutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);

                    ModelState.AddModelError("", $"Hesab�n�z {(lockOutEndDate.Value.UtcDateTime - DateTime.UtcNow).Minutes} dakika s�reyle ask�ya al�nm�st�r");
                }
                else
                {
                    string message = "";
                    if (appUser != null)
                    {
                        int maxFailedAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts; //middleware'deki maksimum hata say�n�z...

                        message = $"Eger {maxFailedAttempts - await _userManager.GetAccessFailedCountAsync(appUser)} kez daha yanl�s giri� yaparsan�z hesab�n�z gecici olarak ask�ya al�nacakt�r";
                    }
                    else
                    {
                        message = "Kullan�c� bulunamad�";
                    }

                    ModelState.AddModelError("", message);

                }

            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult MemberPanel()
        {
            return View();
        }

        public IActionResult Panel()
        {
            return View();
        }
    }
}
