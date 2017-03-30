using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebChat.Classes.Db.Structure;

namespace WebChat.Classes.DB.Repositories
{
    public class UserDialogueRepository : IRepository<DbUserDialogue>
    {
        private DbContext _context = null;

        public UserDialogueRepository(DbContext context)
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

        public IQueryable<DbUserDialogue> GetAll()
        {
            return _context.UserDialogues;
        }

        public DbUserDialogue GetById(object id)
        {
            if (id.GetType() == typeof(int))
            {
                int identificator = (int)id;
                var contact = _context.UserDialogues.SingleOrDefault(x => x.Id == identificator);
                return contact;
            }

            throw new ArgumentException("The id parameter sended to the GetById method should have a non negative integer value");
        }

        public void Insert(DbUserDialogue entity)
        {
            bool contactExist = _context.UserDialogues.Count(x=>x.UserId == entity.UserId && x.DialogueId == entity.DialogueId) > 0 ;
            if (contactExist)
                return;

            _context.UserDialogues.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<DbUserDialogue> SearchFor(Expression<Func<DbUserDialogue, bool>> predicate)
        {
            return _context.UserDialogues.Where(predicate);
        }

        public int Count(Expression<Func<DbUserDialogue, bool>> predicate)
        {
            return _context.UserDialogues.Count(predicate);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}