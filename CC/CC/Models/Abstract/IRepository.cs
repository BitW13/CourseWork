using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Models.Abstract
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetElementById(Guid? id);

        T GetElement(T item);

        IEnumerable<T> GetElementByUserId(Guid? id);

        void Create(T item);

        void Delete(Guid? id);

        void Update(T item);

        //void SaveChanges();
    }
}
