using MAUI_CRUD_SQLite.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MAUI_CRUD_SQLite.Pages;

public partial class AllUsers : ContentPage
{
    UserVM userVM = new UserVM(); 
    List<UserVM> userList = new List<UserVM>();
    ObservableCollection<UserVM> users = new ObservableCollection<UserVM>();
    Response response = new Response();
    public AllUsers()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        GetUsersList();
        base.OnAppearing();
    }
    private async void GetUsersList()
    {
        try
        {
            users.Clear();
            response = await App.SQLiteDb.GetUserAsync();
            if (response != null && response.Status == ResponseStatus.OK)
            {
                var data = JsonConvert.SerializeObject(response.ResultData);
                userList = JsonConvert.DeserializeObject<List<UserVM>>(data);
                if (userList?.Count > 0)
                {
                    foreach (var item in userList)
                    {
                        users.Add(item);
                    }

                    UsersListView.ItemsSource = users;
                }
                else
                {
                }

            }

        }
        catch (Exception ex)
        {
        }
    }

    private async void OnSwipeDelete(object sender, EventArgs e)
    {
        try
        {
            SwipeItem swipeItem = (SwipeItem)sender;
            //var id = swipeItem.CommandParameter;
            UserVM userVM = (UserVM)swipeItem.BindingContext;
            response = await App.SQLiteDb.DeleteUserAsync(userVM);
            if (response != null)
            {
                if (response.Status == ResponseStatus.OK)
                {
                    users.Clear();
                    await DisplayAlert("Message", "Data deleted", "Ok");
                    GetUsersList();
                }
                else
                {
                    await DisplayAlert("Error", "Something went wrong", "Ok");
                }

            }
            else
            {
                await DisplayAlert("Error", "Something went wrong", "Ok");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Something went wrong", "Ok");
        }
    }

    private void OnSwipeEdit(object sender, EventArgs e)
    {
        SwipeItem swipeItem = (SwipeItem)sender;
        userVM = (UserVM)swipeItem.BindingContext;
        Navigation.PushAsync(new AddUser(userVM));
    }

    private void Add_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddUser());
    }
}