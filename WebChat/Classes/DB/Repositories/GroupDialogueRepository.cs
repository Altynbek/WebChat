using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebChat.Classes.Db.Structure;

namespace WebChat.Classes.DB.Repositories
{
    public class GroupDialogueRepository : IRepository<DbUserDialogue>
    {
        private readonly DbContext _context = null;

        public GroupDialogueRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public void Delete(DbUserDialogue entity)
        {
            _context.UserDialogues.Remove(entity);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IQueryable<DbUserDialogue> GetAll()
        {
            return _context.UserDialogues;
        }

        public DbUserDialogue GetById(object id)
        {
            if (id.GetType() != typeof(int))
                throw new ArgumentException("The id parameter sended to the GetById method should have a non negative integer value");

            var userDialogue = _context.UserDialogues.SingleOrDefault(x => x.Id == (int)id);
            return userDialogue;
        }

        public void Insert(DbUserDialogue entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DbUserDialogue> SearchFor(Expression<Func<DbUserDialogue, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}