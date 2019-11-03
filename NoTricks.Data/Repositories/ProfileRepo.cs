using System.Collections.Generic;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories
{
    public interface IProfileRepo : IRepository<Profile>{}
    
    public class ProfileRepo : IProfileRepo
    {
        private readonly string _connStr;

        public ProfileRepo(NoTricksConnectionString connStr)
        {
            _connStr = connStr.Value;
        }
        
        public int Insert(Profile model)
        {
            throw new System.NotImplementedException();
        }

        public Profile GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Profile> GetAll() {
            throw new System.NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Profile model)
        {
            throw new System.NotImplementedException();
        }
    }
}