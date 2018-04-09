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
    public class EFRecordRepository : IRepository<Record>
    {
        private UserContext _context;

        public EFRecordRepository(UserContext context)
        {
            _context = context;
        }

        public IEnumerable<Record> GetAll()
        {
            List<Record> list = _context.Records.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                list[i].NickName = Decoding.GetDecrypt(list[i].NickName);
            }

            return list;
        }

        public void Create(Record item)
        {
            _context.Records.Add(item);
            _context.SaveChanges();
        }

        public void Delete(Guid? id)
        {
            _context.Entry(GetElementById(id)).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Record GetElement(Record item)
        {
            var record = _context.Records.FirstOrDefault(m => m.Title == item.Title && m.Description == item.Description);
            return record;
        }

        public Record GetElementById(Guid? id)
        {
            var record = _context.Records.FirstOrDefault(m => m.Id == id);

            record.NickName = Decoding.GetDecrypt(record.NickName);

            return record;
        }

        public IEnumerable<Record> GetElementByUserId(Guid? id)
        {
            var records = _context.Records.Where(m => m.UserId == id).ToList();
            return records;
        }

        public void Update(Record item)
        {
            _context.Entry(GetElementById(item.Id)).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}