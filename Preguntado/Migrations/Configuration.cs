namespace Preguntado.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Preguntado.Models;
    using Preguntado.Models.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    internal sealed class Configuration : DbMigrationsConfiguration<Preguntado.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Preguntado.Models.ApplicationDbContext context)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            foreach (RoleEnum role in Enum.GetValues(typeof(RoleEnum)))
            {   
                if (!rm.RoleExists(role.ToString()))
                {
                    var roleResult = rm.Create(new IdentityRole(role.ToString()));
                    if (!roleResult.Succeeded)
                        throw new ApplicationException("Creating role " + role.ToString() + "failed with error(s): " + roleResult.Errors);
                }
            }
        }
    }
}
