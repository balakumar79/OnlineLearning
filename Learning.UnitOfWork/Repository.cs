using Learning.Entities;
using Learning.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.UnitOfWork
{
    public class Repository<T>:IRepository<T> where T : class
    {
        protected readonly AppDBContext _dBContext;
        private readonly DbSet<T> _dbSet;

        public T GetById(object Id)
        {
            return _dbSet.Find(Id);
        }
        public Repository(AppDBContext dBContext)
        {
            _dBContext = dBContext;
            _dbSet = _dBContext.Set<T>();
        }
        public IList<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public void Add(T item)
        {
            _dbSet.Add(item);
        }
    }
}
