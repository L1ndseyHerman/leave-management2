using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2
{
    //  Static classes in C# mean you can call the methods of this class wo creating an instance of it?! Can Java do that?
    public static class SeedData
    {
        //  This method is public, gets called in Startup.cs. The other two are private, only get called by this method. 
        public static void Seed(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            //  This returns the user (object) instead of a boolean.
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                //  Yeah, this really looks like a JS object!
                var user = new IdentityUser
                {
                    UserName = "admin@localhost.com",
                    Email = "admin@localhost.com"
                };
                var result = userManager.CreateAsync(user, "P@ssword1").Result;
                if (result.Succeeded)
                {
                    //  "Wait()" = make sure this (thread?) finishes before other things happen!
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            //  Only create the role if it doesn't exist yet. Gets a boolean.
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                    //  Why is there this weird semicolon after the "{"? Is this like a JS object? Does C# have shorthand for that?
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Employee"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
