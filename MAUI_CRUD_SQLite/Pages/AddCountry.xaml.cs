using FluentValidation.Results;
using MAUI_CRUD_SQLite.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MAUI_CRUD_SQLite.Pages;

public partial class AddCountry : ContentPage
{
    CountryVM countryVM = new CountryVM();
    List<CountryVM> countryList = new List<CountryVM>();
    ObservableCollection<CountryVM> countries = new ObservableCollection<CountryVM>();
    Response response = new Response();
    public AddCountry()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        PopulateCountryList();
        base.OnAppearing();
    }
    private async void PopulateCountryList()
    {
        try
        {
            response = await App.SQLiteDb.GetCountryAsync();
            if (response != null && response.Status == ResponseStatus.OK)
            {
                var data = JsonConvert.SerializeObject(response.ResultData);
                countryList = JsonConvert.DeserializeObject<List<CountryVM>>(data);
                if(countryList?.Count > 0)
                {
                    foreach (var item in countryList)
                    {
                        countries.Add(item);
                    }

                    countriesListView.ItemsSource = countries;
                    listStack.IsVisible = true;
                    //AddCountryLayout.IsVisible = false;
                }
                else
                {
                    listStack.IsVisible = false;
                }
                
            }
            else
                listStack.IsVisible = false;

        }
        catch (Exception ex)
        {
            listStack.IsVisible = false;
        }
    }
    private async void SubmitClick(object sender, EventArgs e)
	{
        try
        {
            countryVM.CountryName = countryNameEntry.Text;
            if (Validation(countryVM))
            {
                if (countryVM?.CountryId > 0)
                {
                    response = await App.SQLiteDb.UpdateCountryAsync(countryVM);
                    if (response != null)
                    {
                        if (response.Status == ResponseStatus.OK)
                        {
                            countries.Clear();
                            await DisplayAlert("Info", "Data updated Successfully", "Ok");
                            countryNameEntry.Text = "";
                            countryVM = new CountryVM();
                            countryNameError.IsVisible = false;
                            PopulateCountryList();
                        }
                        else if (response.Status == ResponseStatus.Restrected)
                        {
                            countryNameError.IsVisible = true;
                            countryNameError.Text = "Country Name Already Exist";
                        }
                        else
                        {
                            await DisplayAlert("Error", "Something went wrong", "Ok");
                        }

                    }
                }
                else
                {
                    response = await App.SQLiteDb.SaveCountryAsync(countryVM);
                    if (response != null)
                    {
                        if (response.Status == ResponseStatus.OK)
                        {
                            countries.Clear();
                            await DisplayAlert("Info", "Data saved Successfully", "Ok");
                            countryNameEntry.Text = "";
                            countryVM = new CountryVM();
                            countryNameError.IsVisible = false;
                            PopulateCountryList();
                        }
                        else if (response.Status == ResponseStatus.Restrected)
                        {
                            countryNameError.IsVisible = true;
                            countryNameError.Text = "Country Name Already Exist";
                        }
                        else
                        {
                            await DisplayAlert("Error", "Something went wrong", "Ok");
                        }

                    }
                }
                

            }
        }
        catch (Exception ex)
        {

        }
    }
    private bool Validation(CountryVM countryVM)
    {
        try
        {
            countryNameError.IsVisible = false;
            ValidationResult result = new CountryValidator().Validate(countryVM);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case "CountryName":
                            countryNameError.IsVisible = true;
                            countryNameError.Text = failure.ErrorMessage;
                            break;
                        
                    }
                }
            }
            return result.IsValid;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private void countryNameEntry_TextChanged(object sender, TextChangedEventArgs e)
	{
        if (!string.IsNullOrEmpty(countryNameEntry.Text))
            countryNameError.IsVisible = false;
        else
        {
            countryNameError.IsVisible = true;
        }
    }


    private async void OnSwipeDelete(object sender, EventArgs e)
    {
        try
        {
            SwipeItem swipeItem = (SwipeItem)sender;
            //var id = swipeItem.CommandParameter;
            CountryVM countryVM = (CountryVM)swipeItem.BindingContext;
            response = await App.SQLiteDb.DeleteCountryAsync(countryVM);
            if (response != null)
            {
                if (response.Status == ResponseStatus.OK)
                {
                    countries.Clear();
                    await DisplayAlert("Message", "Data deleted", "Ok");
                    PopulateCountryList();
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

    private async void OnSwipeEdit(object sender, EventArgs e)
    {
        SwipeItem swipeItem = (SwipeItem)sender;
        countryVM = (CountryVM)swipeItem.BindingContext;

        //listStack.IsVisible = false;
        //AddCountryLayout.IsVisible = true;
        countryNameEntry.Text = countryVM.CountryName;
    }
}