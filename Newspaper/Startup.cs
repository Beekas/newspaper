using Microsoft.Owin;
using Newspaper.Models.DAL;
using Newspaper.Models;
using RegistrationAndLogin;
using Owin;
using System.Linq;
using System;

[assembly: OwinStartupAttribute(typeof(Newspaper.Startup))]
namespace Newspaper
{
    public partial class Startup
    {
        //public void Configuration(IAppBuilder app)
        //{
        //    ConfigureAuth(app);
        //}
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createUsers();
        }

        // In this method we will create default User roles and Admin user for login
        private void createUsers()
           {
            NewspaperEntities context = new NewspaperEntities();
            
            //  var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            // var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating  a default Admin User 

            var user = new Admin();
            user.EmployeeId = "100";
            user.UserName = "gpsuperadmin";
            user.FullName = "Super Admin";
            user.Email = "bgorkhapatra207@gmail.com";
            user.Category = "SuperAdmin";
            user.Phone = "01-4220638";
            user.Password = Crypto.Hash("admin123");
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.PPSizePhoto = "~/img/17.jpg";
            user.Status = true;

            if (context.Admin.ToList().Count == 0)
            {
                context.Admin.Add(user);
                context.SaveChanges();
            }   

            //string userPWD = "admin123";


            //var chkUser = UserManager.Create(user, userPWD);


        }
    }

}
