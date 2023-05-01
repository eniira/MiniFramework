using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace MineFramework
{
    class Database
    {
        public SQLiteConnection OpenServerConnection()
        {
            return new SQLiteConnection("Data Source= database.db;Version=3;New=True;Compress=True;");

        }

        public SQLiteConnection OpenDBConnection()
        {
            return new SQLiteConnection("Data Source= database.db;Version=3;New=True;Compress=True;");
        }

        public void createDB()
        {
            try
            {
                using(var conn = OpenServerConnection())
                {
                    conn.Open();
                    using (var comm = conn.CreateCommand())
                    {
                        comm.CommandText = "CREATE DATABASE teste";
                        comm.ExecuteNonQuery();
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                
                throw;
            }
        }

        public void createTable()
        {
            try
            {
                using(var conn = OpenDBConnection())
                {
                    conn.Open();
                    using (var comm = conn.CreateCommand())
                    {
                        comm.CommandText = "CREATE TABLE teste1";
                        comm.ExecuteNonQuery();
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                
                throw;
            }
        }
    }
}