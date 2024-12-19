using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoApp.Models;

namespace ToDoApp
{
    public class MainPageViewModel : BaseViewModel
    {
        private string _toDoContent;
        private DateTime _dueDate = DateTime.Now;

        public string ToDoContent
        {
            get => _toDoContent;
            set => SetProperty(ref _toDoContent, value); // Using SetProperty here
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value); // Using SetProperty here
        }

        private readonly SQLiteDbContext _db;

        public ObservableCollection<ToDoItem> ToDoItems { get; set; } = new();
        public ObservableCollection<ToDoItem> InProgressItems { get; set; } = new();
        public ObservableCollection<ToDoItem> DoneItems { get; set; } = new();

        public ICommand ChangeStatusCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand OnAddToDoItemCommand { get; }

        public MainPageViewModel(SQLiteDbContext db)
        {
            _db = db;

            ChangeStatusCommand = new Command<ToDoItem>(ChangeStatus);
            DeleteItemCommand = new Command<ToDoItem>(DeleteItem);
            OnAddToDoItemCommand = new Command(OnAddToDoItem);

            LoadItems();
        }

        private async void LoadItems()
        {
            var allItems = await _db.GetItems();

            ToDoItems.Clear();
            InProgressItems.Clear();
            DoneItems.Clear();

            foreach (var item in allItems)
            {
                switch (item.Status)
                {
                    case Status.ToDo:
                        ToDoItems.Add(item);
                        break;
                    case Status.InProgress:
                        InProgressItems.Add(item);
                        break;
                    case Status.Done:
                        DoneItems.Add(item);
                        break;
                }
            }
        }

        private async void ChangeStatus(ToDoItem item)
        {
            if (item == null) return;

            switch (item.Status)
            {
                case Status.ToDo:
                    item.Status = Status.InProgress;
                    break;
                case Status.InProgress:
                    item.Status = Status.Done;
                    break;
            }

            await _db.UpdateItem(item);
            LoadItems();
        }

        private async void DeleteItem(ToDoItem item)
        {
            if (item == null) return;

            await _db.DeleteItem(item);
            LoadItems();
        }

        private async void OnAddToDoItem()
        {
            var newItem = new ToDoItem
            {
                ToDoContent = ToDoContent,
                DueTo = DueDate,
                Status = Status.ToDo
            };

            await _db.CreateItem(newItem);
            ToDoItems.Add(newItem);

            ToDoContent = string.Empty;
            DueDate = DateTime.Now;

            LoadItems();
        }

    }
}
