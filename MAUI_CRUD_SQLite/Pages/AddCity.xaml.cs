using FluentValidation;
using FluentValidation.Results;
using MAUI_CRUD_SQLite.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MAUI_CRUD_SQLite.Pages;

public partial class AddCity : ContentPage
{
    CityVM cityVM = new CityVM();
    List<CountryVM> countryList = new List<CountryVM>();
    ObservableCollection<CountryVM> countries = new ObservableCollection<CountryVM>();
    List<CityVM> cityList = new List<CityVM>();
    ObservableCollection<CityVM> cities = new ObservableCollection<CityVM>();
    Response response = new Response();
    public AddCity()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        GetCountriesList();
        GetCitiesList();
        base.OnAppearing();
    }
    private async void GetCitiesList()
    {
        try
        {
            response = await App.SQLiteDb.GetCityAsync();
            if (response != null && response.Status == ResponseStatus.OK)
            {
                var data = JsonConvert.SerializeObject(response.ResultData);
                cityList = JsonConvert.DeserializeObject<List<CityVM>>(data);
                if (cityList?.Count > 0)
                {
                    foreach (var item in cityList)
                    {
                        cities.Add(item);
                    }

                    citiesListView.ItemsSource = cities;
                    listStack.IsVisible = true;
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

    private async void GetCountriesList()
	{
        try
        {
            response = await App.SQLiteDb.GetCountryAsync();
            if (response != null && response.Status == ResponseStatus.OK)
            {
                var data = JsonConvert.SerializeObject(response.ResultData);
                countryList = JsonConvert.DeserializeObject<List<CountryVM>>(data);
                if (countryList?.Count > 0)
                {
                    foreach (var item in countryList)
                    {
                        countries.Add(item);
                    }

                    combobox.ItemsSource = countries;
                }
                else
                {
                }

            }

        }
        catch (Exception ex)
        {
            listStack.IsVisible = true;
        }
    }

	

	private void cityNameEntry_TextChanged(object sender, TextChangedEventArgs e)
	{
        if (!string.IsNullOrEmpty(cityNameEntry.Text))
            cityNameError.IsVisible = false;
        else
        {
            cityNameError.IsVisible = true;
        }
    }

	private async void SubmitClick(object sender, EventArgs e)
	{
        try
        {
            cityVM.CityName = cityNameEntry.Text;
            if (Validation(cityVM))
            {
                if (cityVM?.CityId > 0)
                {
                    response = await App.SQLiteDb.UpdateCityAsync(cityVM);
                    if (response != null)
                    {
                        if (response.Status == ResponseStatus.OK)
                        {
                            cities.Clear();
                            await DisplayAlert("Info", "Data updated Successfully", "Ok");
                            cityNameEntry.Text = "";
                            combobox.SelectedItem = null;
                            cityVM = new CityVM();
                            cityNameError.IsVisible = false;
                            GetCitiesList();
                        }
                        else if (response.Status == ResponseStatus.Restrected)
                        {
                            cityNameError.IsVisible = true;
                            cityNameError.Text = "City Name Already Exist";
                        }
                        else
                        {
                            await DisplayAlert("Error", "Something went wrong", "Ok");
                        }

                    }
                }
                else
                {
                    response = await App.SQLiteDb.SaveCityAsync(cityVM);
                    if (response != null)
                    {
                        if (response.Status == ResponseStatus.OK)
                        {
                            cities.Clear();
                            await DisplayAlert("Info", "Data saved Successfully", "Ok");
                            cityNameEntry.Text = "";
                            combobox.SelectedItem = null;
                            cityVM = new CityVM();
                            cityNameError.IsVisible = false;
                            GetCitiesList();
                        }
                        else if(response.Status == ResponseStatus.Restrected)
                        {
                            cityNameError.IsVisible = true;
                            cityNameError.Text = "City Name Already Exist";
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

    private bool Validation(CityVM cityVM)
    {
        try
        {
            cityNameError.IsVisible = false;
            countryNameError.IsVisible = false;
            ValidationResult result = new CityValidator().Validate(cityVM);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case "CityName":
                            cityNameError.IsVisible = true;
                            cityNameError.Text = failure.ErrorMessage;
                            break;
                        case "CountryId":
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

    private async void OnSwipeDelete(object sender, EventArgs e)
	{
        try
        {
            SwipeItem swipeItem = (SwipeItem)sender;
            //var id = swipeItem.CommandParameter;
            CityVM cityVM = (CityVM)swipeItem.BindingContext;
            response = await App.SQLiteDb.DeleteCityAsync(cityVM);
            if (response != null)
            {
                if (response.Status == ResponseStatus.OK)
                {
                    cities.Clear();
                    await DisplayAlert("Message", "Data deleted", "Ok");
                    GetCitiesList();
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
        cityVM = (CityVM)swipeItem.BindingContext;

        cityNameEntry.Text = cityVM.CityName;
        combobox.SelectedItem = cityVM.CountryName;
    }

	private void countryChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
	{
        try
        {
            CountryVM data = e.CurrentSelection.FirstOrDefault() as CountryVM;
            if(data != null)
            {
                cityVM.CountryId = data.CountryId;
                cityVM.CountryName = data.CountryName;
                countryNameError.IsVisible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

}