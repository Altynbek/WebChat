using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebChat.Classes.Db.Structure;

namespace WebChat.Classes.DB.Repositories
{
    public class ContactRepository : IRepository<DbUserContact>
    {
        private DbContext _context = null;

        public ContactRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public void Delete(DbUserContact entity)
        {
            _context.UserContacts.Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<DbUserContact> GetAll()
        {
            return _context.UserContacts;
        }

        public DbUserContact GetById(object id)
        {
            if (id.GetType() == typeof(int))
            {
                int identificator = (int)id;
                var contact = _context.UserContacts.SingleOrDefault(x => x.Id == identificator);
                return contact;
            }

            throw new ArgumentException("The id parameter sended to the GetById method should have a non negative integer value");
        }

        public void Insert(DbUserContact entity)
        {
            bool contactExist = _context.UserContacts.Count(x => x.ContactId == entity.ContactId && x.OwnerId == entity.OwnerId) > 0;
            if (contactExist)
                return;

            _context.UserContacts.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<DbUserContact> SearchFor(Expression<Func<DbUserContact, bool>> predicate)
        {
            return _context.UserContacts.Where(predicate);
        }

        public int DeleteById(string ownerId, string companionId)
        {
            int result = 0;
            var dbContact = _context.UserContacts.SingleOrDefault(x => x.OwnerId == ownerId && x.ContactId == companionId);
            if (dbContact != null)
            {
                _context.UserContacts.Remove(dbContact);
                result = _context.SaveChanges();
            }
            return result;
        }

        public int AcceptContact(string ownerId, string companionId)
        {
            var contacts = _context.UserContacts
                .Where( x => 
                    (x.OwnerId == ownerId && x.ContactId == companionId) || 
                    (x.OwnerId == companionId && x.ContactId == ownerId)).ToList();
            foreach (var contact in contacts)
            {
                contact.Confirmed = true;
            }
            int result = _context.SaveChanges();
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}