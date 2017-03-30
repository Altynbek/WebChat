using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebChat.Classes.Db.Structure;

namespace WebChat.Models
{
    public class ApplicationUserManager : UserManager<DbUser>
    {
        public ApplicationUserManager(IUserStore<DbUser> store)
            : base(store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
                                            IOwinContext context)
        {
            DbContext db = context.Get<DbContext>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<DbUser>(db));
            return manager;
        }
    }

    
}