namespace TestWebAppV8.Controllers
{
    using System.Web.Mvc;
    using TestWebAppV8.Models;
    using Umbraco.Core.Services;
    using Umbraco.Web.Mvc;
    using Umbraco.Web.Security;

    public class AccountSurfaceController : SurfaceController
    {
        private readonly MembershipHelper _membershipHelper;

        public AccountSurfaceController(IMemberService memberService, MembershipHelper membershipHelper)
        {
            _membershipHelper = membershipHelper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginFormModel vm)
        {    
            // Check validity
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Authenticate member
            if (!_membershipHelper.Login(vm.Username, vm.Password))
            {
                TempData["LoginFailed"] = true;
            }

            return RedirectToCurrentUmbracoPage();
        }

        [ChildActionOnly]
        public PartialViewResult MemberDetails()
        {
            // Check if logged on
            if (_membershipHelper.IsLoggedIn())
            {
                // Get currently logged in member
                var member = _membershipHelper.GetCurrentMember();

                // Return partial view with populated view model
                return PartialView("_MemberDetails", new MemberDetailsViewModel
                {
                    Name = member.Name,
                });
            }

            return null;
        }

        public RedirectResult LogOut()
        {
            if (_membershipHelper.IsLoggedIn())
            {
                _membershipHelper.Logout();
            }

            return Redirect("/");
        }
    }
}