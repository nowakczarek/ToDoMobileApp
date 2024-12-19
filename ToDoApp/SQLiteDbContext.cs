using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ToDoApp.Models;

namespace ToDoApp
{
    public class SQLiteDbContext
    {
        private const string DatabaseFileName = "ToDoApp_v3.db3";
        private readonly SQLiteAsyncConnection _connection;

        public SQLiteDbContext()
        {       
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName));
            _connection.CreateTableAsync<ToDoItem>().Wait();
        }

        public async Task InitializeDatabaseAsync()
        {
            await _connection.CreateTableAsync<ToDoItem>();
        }

        public async Task<List<ToDoItem>> GetItems()
        {
            return await _connection.Table<ToDoItem>().ToListAsync();
        }

        //tescior
        public async Task<List<ToDoItem>> GetItemsByStatus(Status status)
        {
            return await _connection.Table<ToDoItem>().Where(x => x.Status == status).ToListAsync();
        }

        public async Task<ToDoItem> GetItemsById(int id)
        {
            return await _connection.Table<ToDoItem>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateItem(ToDoItem item)
        {
            await _connection.InsertAsync(item);
        }

        public async Task UpdateItem(ToDoItem item)
        {
            await _connection.UpdateAsync(item);
        }

        public async Task DeleteItem(ToDoItem item)
        {
            await _connection.DeleteAsync(item);
        }
    }
}
