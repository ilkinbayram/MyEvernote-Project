using CommonLayer;
using MyEvernote.CommonLayer;
using MyEvernote.Core.Abstract;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    // REPOSITORY PATTERN
    public class Repository<T> : RepositoryBaseSingletonContext, IDataAccess<T> where T: class, new()
    {
        private DbSet<T> _objSet;
        public Repository()
        {
            _objSet = context.Set<T>(); // 'context' Miras alinan  Singleton Patterninden gelen protected deyisenle taninir !!!
        }

        
        public List<T> List()
        {
            return _objSet.ToList();
        }
        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objSet.Where(where).ToList();
        }
        public IQueryable<T> GetByQuery(Expression<Func<T,bool>> where)
        {
            return _objSet.Where(where);
        }
        public T Get(Expression<Func<T,bool>> where)
        {
            return _objSet.FirstOrDefault(where);
        }
        public int Insert(T entity)
        {
            if (entity is MyBaseEntity)
            {
                MyBaseEntity obj = entity as MyBaseEntity;
                DateTime now = DateTime.Now;
                obj.CreatedOn = now;
                obj.ModifiedOn = now;
                obj.ModifiedUsername = App.Common.GetCurrentUsername();
            }
            _objSet.Add(entity);
            return Save();
        }
        public int Update(T entity)
        {
            if (entity is MyBaseEntity)
            {
                MyBaseEntity obj = entity as MyBaseEntity;
                DateTime now = DateTime.Now;
                obj.ModifiedOn = now;
                obj.ModifiedUsername = App.Common.GetCurrentUsername();
            }
            var modifyEntity = context.Entry(entity);
            modifyEntity.State = EntityState.Modified;
            return Save();
        }
        public int Delete(T entity)
        {
            if (entity is MyBaseEntity)
            {
                MyBaseEntity obj = entity as MyBaseEntity;
                DateTime now = DateTime.Now;
                obj.ModifiedOn = now;
                obj.ModifiedUsername = App.Common.GetCurrentUsername();
                obj.IsDeleted = true;
            }
            var modifyEntity = context.Entry(entity);
            modifyEntity.State = EntityState.Modified;
            return Save();
        }

        public int PermanentlyDelete(T entity)
        {
            var deleteEntity = context.Entry(entity);
            deleteEntity.State = EntityState.Deleted;
            return Save();
        }
        public int Save()
        {
            int result = context.SaveChanges();
            return result;
        }

        public IQueryable<T> GetListWithQueryable()
        {
            return _objSet.AsQueryable<T>();
        }


    }
}
