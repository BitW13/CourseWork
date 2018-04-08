using CC.Context;
using CC.Context.ContextModels;
using CC.Cryptor;
using CC.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CC.Models.Concrete
{
    public class EFUserRepository : IRepository<User>
    {
        private UserContext _context;

        public EFUserRepository(UserContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> list = _context.Users.ToList();

            foreach (var item in list)
            {
                Decoding.GetDecrypt(item.NickName);
                Decoding.GetDecrypt(item.UserName);
                Decoding.GetDecrypt(item.UserSurname);
            }

            return list;
        }
        //public IEnumerable<User> GetAll { get => _context.Users.ToList(); }

        public void Create(User item)
        {
            _context.Users.Add(item);
            _context.SaveChanges();
        }

        public void Delete(Guid? id)
        {
            _context.Entry(GetElementById(id)).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public User GetElement(User item)
        {
            var user = _context.Users.FirstOrDefault(m => m.NickName == item.NickName && m.UserName == item.UserName && m.UserSurname == item.UserSurname);
            return user;
        }

        public User GetElementById(Guid? id)
        {
            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            return user;
        }

        public IEnumerable<User> GetElementByUserId(Guid? id)
        {
            throw new NotImplementedException();
        }

        public void Update(User item)
        {
            _context.Entry(GetElementById(item.Id)).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}