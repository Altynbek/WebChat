using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.Exceptions;

namespace WebChat.Classes.DB.Repositories
{
    public class MessageRepository : IRepository<DbMessage>
    {
        private readonly DbContext _context = null;

        public MessageRepository(DbContext context)
        {
            _context = context;
        }

        public void Delete(DbMessage entity)
        {
            _context.Messages.Remove(entity);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IQueryable<DbMessage> GetAll()
        {
            return _context.Messages;
        }

        public DbMessage GetById(object id)
        {
            if (id.GetType() != typeof(int))
                throw new ArgumentException("The id parameter should have the non negative numeric value");

            int msgId = (int)id;
            var message = _context.Messages.SingleOrDefault(x => x.Id == msgId);
            if (message == null)
                throw new MessageNotFoundException("The message with the given key was not found");

            return message;
        }

        public void Insert(DbMessage entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Messages.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<DbMessage> SearchFor(System.Linq.Expressions.Expression<Func<DbMessage, bool>> predicate)
        {
            var messages = _context.Messages.Where(predicate);
            return messages;
        }
    }
}