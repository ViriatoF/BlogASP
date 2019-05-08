using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MyBlog.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyBlog.Startup))]
namespace MyBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!RoleManager.RoleExists("Admin"))
            {
                IdentityRole Role = new IdentityRole();
                Role.Name = "Admin";
                RoleManager.Create(Role);

                ApplicationUser User = new ApplicationUser();
                User.UserName = "";
                User.Email = "";

                string UserPWD = "";

                IdentityResult ChkUser = UserManager.Create(User, UserPWD);

                if (ChkUser.Succeeded)
                {
                    var Result1 = UserManager.AddToRole(User.Id, "Admin");
                }
            }

            if (!RoleManager.RoleExists("Abonné"))
            {
                IdentityRole Role = new IdentityRole();
                Role.Name = "Abonné";
                RoleManager.Create(Role);
            }

            if (!RoleManager.RoleExists("Auteur"))
            {
                IdentityRole Role = new IdentityRole();
                Role.Name = "Auteur";
                RoleManager.Create(Role);
            }
        }
    }
}
