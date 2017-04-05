using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.Exceptions;

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
            if (id.GetType() != typeof(int))
                throw new ArgumentException("The id parameter sended to the GetById method should have a non negative integer value");

            var dialogue = _context.Dialogs.SingleOrDefault(x => x.Id == (int)id);
            if (dialogue == null)
                throw new DialogueNotFoundException("The dialogue with the given key was not found");

            return dialogue;
        }

        public void Insert(DbDialogue entity)
        {
            bool contactExist = _context.Dialogs.Count(x => x.Id == entity.Id && x.Name == entity.Name) > 0;
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

        public int Insert(List<DbUserDialogue> dbUserDialogues)
        {
            _context.UserDialogues.AddRange(dbUserDialogues);
            int insertedRecordsCound = _context.SaveChanges();
            return insertedRecordsCound;
        }

        public List<string> GetDialogueCompanions(int dialogueId)
        {
            var dbUsersId = _context.UserDialogues.Where(x => x.DialogueId == dialogueId).Select(x => x.UserId).ToList();
            return dbUsersId;
        }
    }
}