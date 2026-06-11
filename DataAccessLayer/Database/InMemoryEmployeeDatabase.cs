using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Database
{
    public class InMemoryEmployeeDatabase : IDbWrapper<Employee>
    {
        private readonly Dictionary<Tuple<string, string>, Employee> _database;

        public InMemoryEmployeeDatabase()
        {
            _database = new Dictionary<Tuple<string, string>, Employee>();
        }

        private Tuple<string, string> GetKey(Employee employee) =>
            Tuple.Create(employee.SiteId, employee.EmployeeCode);

        public bool Insert(Employee data)
        {
            try
            {
                _database.Add(GetKey(data), data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Employee data)
        {
            try
            {
                var key = GetKey(data);
                if (!_database.ContainsKey(key))
                    return false;

                _database.Remove(key);
                _database.Add(key, data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Employee> Find(Expression<Func<Employee, bool>> expression)
        {
            try
            {
                return _database.Values.Where(expression.Compile());
            }
            catch
            {
                return Enumerable.Empty<Employee>();
            }
        }

        public IEnumerable<Employee> FindAll()
        {
            try
            {
                return _database.Values.ToList();
            }
            catch
            {
                return Enumerable.Empty<Employee>();
            }
        }

        public bool Delete(Expression<Func<Employee, bool>> expression)
        {
            try
            {
                var toDelete = _database.Values.Where(expression.Compile()).ToList();
                foreach (var employee in toDelete)
                    _database.Remove(GetKey(employee));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAll()
        {
            try
            {
                _database.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateAll(Expression<Func<Employee, bool>> filter, string fieldToUpdate, object newValue)
        {
            try
            {
                var toUpdate = _database.Values.Where(filter.Compile()).ToList();
                foreach (var employee in toUpdate)
                {
                    var prop = typeof(Employee).GetProperty(fieldToUpdate,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (prop == null)
                        throw new Exception("Property not found");

                    prop.SetValue(employee, newValue, null);
                    var key = GetKey(employee);
                    _database.Remove(key);
                    _database.Add(key, employee);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> InsertAsync(Employee data) => Task.FromResult(Insert(data));
        public Task<bool> UpdateAsync(Employee data) => Task.FromResult(Update(data));
        public Task<IEnumerable<Employee>> FindAsync(Expression<Func<Employee, bool>> expression) => Task.FromResult(Find(expression));
        public Task<IEnumerable<Employee>> FindAllAsync() => Task.FromResult(FindAll());
        public Task<bool> DeleteAsync(Expression<Func<Employee, bool>> expression) => Task.FromResult(Delete(expression));
        public Task<bool> DeleteAllAsync() => Task.FromResult(DeleteAll());
        public Task<bool> UpdateAllAsync(Expression<Func<Employee, bool>> filter, string fieldToUpdate, object newValue) =>
            Task.FromResult(UpdateAll(filter, fieldToUpdate, newValue));
    }
}
