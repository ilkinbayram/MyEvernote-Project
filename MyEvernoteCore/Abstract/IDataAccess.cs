using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Core.Abstract
{
    public interface IDataAccess<T>
    {
        List<T> List();
        IQueryable<T> GetListWithQueryable();
        List<T> List(Expression<Func<T, bool>> where);
        IQueryable<T> GetByQuery(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where);
        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        int Save();
        int PermanentlyDelete(T entity);
    }
}
