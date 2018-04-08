using CC.Context;
using CC.Context.ContextModels;
using CC.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CC.Models.Concrete
{
    public class EFCafeRepository : IRepository<Cafe>
    {
        private UserContext _context;

        public EFCafeRepository(UserContext context)
        {
            _context = context;
        }

        public IEnumerable<Cafe> GetAll()
        {
            IEnumerable<Cafe> list = _context.Cafes.ToList();
            return list;
        }

        public void Create(Cafe item)
        {
            _context.Cafes.Add(item);
            _context.SaveChanges();
        }

        public void Delete(Guid? id)
        {
            _context.Entry(GetElementById(id)).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Cafe GetElement(Cafe item)
        {
            var cafe = _context.Cafes.FirstOrDefault(m => m.Name == item.Name);
            return cafe;
        }

        public IEnumerable<Cafe> GetElementByUserId(Guid? id)
        {
            IEnumerable<Cafe> cafe = _context.Cafes.Where(m => m.Id == id).ToList();
            return cafe;
        }

        public Cafe GetElementById(Guid? id)
        {
            var cafe = _context.Cafes.FirstOrDefault(m => m.Id == id);
            return cafe;
        }

        public void Update(Cafe item)
        {
            _context.Entry(GetElementById(item.Id)).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}