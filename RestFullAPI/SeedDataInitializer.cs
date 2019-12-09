using Microsoft.AspNetCore.Identity;
using RestFullAPI.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFullAPI
{
    public static class SeedDataInitializer
    {
        public static void SeedData(UserManager<RestFullAPIUser> userManager)
        {
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<RestFullAPIUser> userManager)
        {
            if (userManager.FindByNameAsync("zubair").Result == null)
            {
                RestFullAPIUser user = new RestFullAPIUser();
                user.UserName = "zubair";
                user.Email = "zubair.shakoor@arbisoft.com";
                user.EmailConfirmed = true;
                IdentityResult result = userManager.CreateAsync
                (user, "st#1Zubair").Result;
            }


            if (userManager.FindByNameAsync("shehriyar").Result == null)
            {
                RestFullAPIUser user = new RestFullAPIUser();
                user.UserName = "shehriyar";
                user.Email = "shehriyar.zafar@arbisoft.com";
                user.EmailConfirmed = true;
                IdentityResult result = userManager.CreateAsync
                (user, "st#1Zubair").Result;
            }
        }
    }
}
