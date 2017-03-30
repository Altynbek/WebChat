using System;
using System.Linq;
using System.Linq.Expressions;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.Exceptions;

namespace WebChat.Classes.DB.Repositories
{
    public class UserRepository : IRepository<DbUser>
    {
        private readonly DbContext _context = null;

        public UserRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }


        public void Delete(DbUser entity)
        {
           _context.Users.Remove(entity);
           _context.SaveChanges();
        }

        public IQueryable<DbUser> GetAll()
        {
            return _context.Users;
        }

        public DbUser GetById(object id)
        {
            if (id is string)
            {
                var user =_context.Users.SingleOrDefault(x => x.Id == id.ToString());
                if (user == null)
                    throw new UserNotFoundException("The User with a given id was not found in database.");

                return user;
            }

            throw new ArgumentException("Given Id argument should have a string data type");
        }

        public void Insert(DbUser entity)
        {
           _context.Users.Add(entity);
           _context.SaveChanges();
        }

        public IQueryable<DbUser> SearchFor(Expression<Func<DbUser, bool>> predicate)
        {
            var foundUsers =_context.Users.Where(predicate);
            return foundUsers;
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}