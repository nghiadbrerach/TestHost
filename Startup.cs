using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(WebEnterprise.Startup))]

namespace WebEnterprise
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Authen/Login")
            });
            CreateUserRoles();
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }

        private void CreateUserRoles()
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var roleStore = new RoleStore<IdentityRole>();
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("ManagerMarketing"))
            {
                var role = new IdentityRole("ManagerMarketing");
                roleManager.Create(role);

                var user = new IdentityUser("managermarketing");
                var result = manager.Create(user, "123456");

                if (result.Succeeded)
                {
                    manager.AddToRole(user.Id, "ManagerMarketing");
                }
            }
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);

                var user = new IdentityUser("admin");
                var result = manager.Create(user, "123456");

                if (result.Succeeded)
                {
                    manager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("MarketingCoordinator"))
            {
                var role = new IdentityRole("MarketingCoordinator");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Student"))
            {
                var role = new IdentityRole("Student");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Guest"))
            {
                var role = new IdentityRole("Guest");
                roleManager.Create(role);
            }
        }
    }
}
