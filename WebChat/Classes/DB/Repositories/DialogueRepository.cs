using System;
using System.Linq;
using System.Linq.Expressions;
using WebChat.Classes.Db.Structure;

namespace WebChat.Classes.DB.Repositories
{
    public class DialogueRepository : IRepository<DbDialogue>
    {
        private DbContext _context = null;

        public DialogueRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public void Delete(DbDialogue entity)
        {
            _context.Dialogs.Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<DbDialogue> GetAll()
        {
            return _context.Dialogs;
        }

        public DbDialogue GetById(object id)
        {
            throw new NotImplementedException();
            throw new ArgumentException("The id parameter sended to the GetById method should have a non negative integer value");
        }

        public void Insert(DbDialogue entity)
        {
            bool contactExist = _context.Dialogs.Count(x=>x.Id == entity.Id && x.Name == entity.Name) > 0;
            if (contactExist)
                return;

            _context.Dialogs.Add(entity);
            _context.SaveChanges();
        }

        public void Insert(DbDialogue entity, out int insertedRecordId)
        {
            var dbDialogue = _context.Dialogs.SingleOrDefault(x => x.Id == entity.Id && x.Name == entity.Name);
            if (dbDialogue != null)
            {
                insertedRecordId = dbDialogue.Id;
                return;
            }

            _context.Dialogs.Add(entity);
            _context.SaveChanges();
            insertedRecordId = entity.Id;
        }

        public IQueryable<DbDialogue> SearchFor(Expression<Func<DbDialogue, bool>> predicate)
        {
            return _context.Dialogs.Where(predicate);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}