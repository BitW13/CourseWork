using CC.Context;
using CC.Context.ContextModels;
using CC.Cryptor;
using CC.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CC.Models.Concrete
{
    public class EFHostRepostitory : IRepository<Host>
    {
        private UserContext _context;

        public EFHostRepostitory(UserContext context)
        {
            _context = context;
        }

        public void Create(Host item)
        {
            item.UserIp = item.UserIp;

            _context.Hosts.Add(item);
            _context.SaveChanges();
        }

        public void Delete(Guid? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Host> GetAll()
        {
            throw new NotImplementedException();
        }

        public Host GetElement(Host item)
        {
            return _context.Hosts.FirstOrDefault(m => m.UserIp == item.UserIp);
        }

        public Host GetElementById(Guid? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Host> GetElementByUserId(Guid? id)
        {
            throw new NotImplementedException();
        }

        public void Update(Host item)
        {
            throw new NotImplementedException();
        }
    }
}