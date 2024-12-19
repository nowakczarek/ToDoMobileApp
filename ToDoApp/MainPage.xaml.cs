using System.Collections.ObjectModel;
using ToDoApp.Models;

namespace ToDoApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(SQLiteDbContext db)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(db);

        }

    }

}
