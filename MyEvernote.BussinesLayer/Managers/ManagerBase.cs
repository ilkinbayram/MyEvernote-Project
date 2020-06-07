using MyEvernote.Core.Abstract;
using MyEvernote.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer.Managers
{
    public abstract class ManagerBase<T> : IDataAccess<T> where T: class,new()
    {
        private Repository<T> repo_base = new Repository<T>();
        public virtual int Delete(T entity)
        {
            return repo_base.Delete(entity);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return repo_base.Get(where);
        }

        public virtual IQueryable<T> GetByQuery(Expression<Func<T, bool>> where)
        {
            return repo_base.GetByQuery(where);
        }

        public virtual IQueryable<T> GetListWithQueryable()
        {
            return repo_base.GetListWithQueryable();
        }

        public virtual int Insert(T entity)
        {
            return repo_base.Insert(entity);
        }

        public virtual List<T> List()
        {
            return repo_base.List();
        }

        public virtual List<T> List(Expression<Func<T, bool>> where)
        {
            return repo_base.List(where);
        }

        public virtual int PermanentlyDelete(T entity)
        {
            return repo_base.PermanentlyDelete(entity);
        }

        public virtual int Save()
        {
            return repo_base.Save();
        }

        public virtual int Update(T entity)
        {
            return repo_base.Update(entity);
        }
    }
}
