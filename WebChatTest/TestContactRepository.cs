using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebChat.Classes.DB.Repositories;
using WebChat.Classes.Db.Structure;
using System.Linq;

namespace WebChatTest
{
    [TestClass]
    public class TestContactRepository
    {
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ThrowArgumentNullExceptionWhenDbContextIsNull()
        {
            ContactRepository repo = new ContactRepository(null);
        }

        [TestMethod]
        public void TestInsertContactIntoRepository()
        {
            var context = new DbContext();
            var userRepo = new UserRepository(context);
            var dbUsers = userRepo.GetAll().ToList();

            if (dbUsers.Count <= 2)
                Assert.Inconclusive();

            var user1 = dbUsers[0];
            var user2 = dbUsers[1];

            var contactRepo = new ContactRepository(context);
            var contact1 = new DbUserContact()
            {
                Confirmed = true,
                ContactId = user2.UserId,
                FriendsheepInitiator = user1.UserId,
                OwnerId = user1.UserId
            };

            var contact2 = new DbUserContact()
            {
                Confirmed = true,
                ContactId = user1.UserId,
                FriendsheepInitiator = user1.UserId,
                OwnerId = user2.UserId
            };

            int countBefore = contactRepo.GetAll().Count();
            contactRepo.Insert(contact1);
            contactRepo.Insert(contact2);
            var contacts = contactRepo.GetAll().ToList();
            int countAfter = contacts.Count;
            Assert.AreEqual(countBefore + 2, countAfter);

            foreach(var item in contacts)
                contactRepo.Delete(item);

            userRepo.Dispose();
            contactRepo.Dispose();
        }

        [TestMethod]
        public void TestContactRemoving()
        {
            var context = new DbContext();
            var contactRepo = new ContactRepository(context);
            var userRepo = new UserRepository(context);

            var users = userRepo.GetAll().ToList();
            if (users.Count <= 2)
                Assert.Inconclusive();
            
            var user1 = users[0];
            var user2 = users[1];
            var contact = new DbUserContact()
            {
                Confirmed = true,
                ContactId = user2.UserId,
                OwnerId = user1.UserId,
                FriendsheepInitiator = user1.UserId
            };
            contactRepo.Insert(contact);


            var contacts = contactRepo.GetAll().ToList();
            int countBefore = contacts.Count;
            contactRepo.Delete(contacts[0]);
            var countAfter = contactRepo.GetAll().Count();
            Assert.AreEqual(countBefore - 1, countAfter);

            contactRepo.Dispose();
        }

        [TestMethod]
        public void TestAnAcceptanceOfContacts()
        {
            var context = new DbContext();
            var userRepo = new UserRepository(context);
            var dbUsers = userRepo.GetAll().ToList();

            if (dbUsers.Count <= 2)
                Assert.Inconclusive();

            var user1 = dbUsers[0];
            var user2 = dbUsers[1];
            var contactRepo = new ContactRepository(context);

            var contact1 = new DbUserContact()
            {
                Confirmed = true,
                ContactId = user2.UserId,
                FriendsheepInitiator = user1.UserId,
                OwnerId = user1.UserId
            };
            var contact2 = new DbUserContact()
            {
                Confirmed = false,
                ContactId = user1.UserId,
                FriendsheepInitiator = user1.UserId,
                OwnerId = user2.UserId
            };

            contactRepo.Insert(contact1);
            contactRepo.Insert(contact2);

            int unConfirmedContactsCountBefore = contactRepo.SearchFor(x => x.Confirmed == false).Count();
            int recordsUpdated = contactRepo.AcceptContact(user2.UserId, user1.UserId);
            Assert.AreNotEqual(0, recordsUpdated);

            int unconfirmedContactsCountAfter = contactRepo.SearchFor(x => x.Confirmed == false).Count();
            Assert.AreNotEqual(unConfirmedContactsCountBefore, unconfirmedContactsCountAfter);

            var removedContacts = contactRepo.SearchFor(x => x.ContactId == user1.UserId && x.OwnerId == user2.UserId
                || x.ContactId == user2.UserId && x.OwnerId == user1.UserId).ToList();
            foreach (var item in removedContacts)
                contactRepo.Delete(item);
            
            userRepo.Dispose();
            contactRepo.Dispose();
        }
    }
}
