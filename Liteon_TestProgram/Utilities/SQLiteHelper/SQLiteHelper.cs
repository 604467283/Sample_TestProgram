using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.SQLiteHelper
{
    internal class SQLiteHelper
    {

        private string connectionString;

        public SQLiteHelper(string databasePath)
        {
            connectionString = $"Data Source={databasePath};Version=3;";
        }

        // 创建一个表
        public void CreateTable(string tableName, string createTableQuery)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                Console.WriteLine($"Table {tableName} created successfully.");
            }
        }

        // 增加列
        public void AddColumn(string tableName, string columnName, string columnType)
        {
            string alterTableQuery = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnType}";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(alterTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                Console.WriteLine($"Column {columnName} added to table {tableName} successfully.");
            }
        }

        // 插入数据
        public void InsertData(string tableName, string insertDataQuery)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(insertDataQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                Console.WriteLine($"Data inserted into {tableName} successfully.");
            }
        }


        // 更新数据
        public void UpdateData(string tableName, string updateDataQuery)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(updateDataQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                Console.WriteLine($"Data in {tableName} updated successfully.");
            }
        }

        // 查询数据
        public void QueryData(string tableName, string selectQuery)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // 假设查询结果只有一列，可以根据实际情况修改
                            Console.WriteLine(reader[0].ToString());
                        }
                    }
                }
            }
        }



       

    }
}
