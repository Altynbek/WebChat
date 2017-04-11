using System.Data.Entity.Migrations;
using WebChat.Classes.Db.Structure;

namespace WebChat.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DbContext context)
        {
            context.Users.AddOrUpdate(
                new DbUser() { UserName = "IvanoVII", PasswordHash = "AHY/SnnUp7lwjhHwGwnPwkQb7au6s29E/uiIiunKxWIoxipEpauwsVhfEJIoCkknqw==", Email = "IvanoII@mail.ru", EmailConfirmed = true },
                new DbUser() { UserName = "PeptrovPP", PasswordHash = "AHY/SnnUp7lwjhHwGwnPwkQb7au6s29E/uiIiunKxWIoxipEpauwsVhfEJIoCkknqw==", Email = "PeptrovPP@mail.ru", EmailConfirmed = true },
                new DbUser() { UserName = "PolinaPP", PasswordHash = "AHY/SnnUp7lwjhHwGwnPwkQb7au6s29E/uiIiunKxWIoxipEpauwsVhfEJIoCkknqw==", Email = "PolinaPP@mail.ru", EmailConfirmed = true },
                new DbUser() { UserName = "KristinaKK", PasswordHash = "AHY/SnnUp7lwjhHwGwnPwkQb7au6s29E/uiIiunKxWIoxipEpauwsVhfEJIoCkknqw==", Email = "KristinaKK@mail.ru", EmailConfirmed = true },
                new DbUser() { UserName = "ElenaEE", PasswordHash = "AHY/SnnUp7lwjhHwGwnPwkQb7au6s29E/uiIiunKxWIoxipEpauwsVhfEJIoCkknqw==", Email = "ElenaEE@mail.ru", EmailConfirmed = true });
            context.SaveChanges();
        }
    }
}
