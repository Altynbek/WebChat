using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.DB.Repositories;
using WebChat.Models.Contact;

namespace WebChat.Classes.Worker
{
    public class ContactWorker : IDisposable
    {
        private readonly ContactRepository _contactRepository = null;
        private readonly UserRepository _userRepository = null;

        public ContactWorker(DbContext context)
        {
            _contactRepository = new ContactRepository(context);
            _userRepository = new UserRepository(context);
        }

        public ContactListModel GetContacts(string userId)
        {
            var model = new ContactListModel();
            List<DbUserContact> dbContacts = _contactRepository.SearchFor(item => item.OwnerId == userId).ToList();
            List<string> usersId = dbContacts.Select(x => x.ContactId).ToList();
            List<DbUser> usersProfile = _userRepository.SearchFor(x => usersId.Contains(x.Id)).ToList();

            foreach (var dbc in dbContacts)
            {
                var profile = usersProfile.SingleOrDefault(x => x.Id == dbc.ContactId);
                var contact = new ContactModel()
                {
                    Id = dbc.Id,
                    ContactId = dbc.ContactId,
                    Confirmed = dbc.Confirmed,
                    ContactName = profile?.UserName,
                    ContactPhotoUrl = profile?.PhotoUrl,
                    FriendsheepInitiator = dbc.FriendsheepInitiator
                };

                model.Contacts.Add(contact);
            }
            return model;
        }

        public DbUserContact GetContactInfo(int contactId)
        {
            var contact = _contactRepository.GetById(contactId);
            return contact;
        }

        public void Dispose()
        {
            _contactRepository.Dispose();
            _userRepository.Dispose();
        }
    }
}