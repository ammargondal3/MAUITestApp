using FluentValidation;
using FluentValidation.Results;
using MAUI_CRUD_SQLite.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MAUI_CRUD_SQLite.Pages;

public partial class AddUser : ContentPage
{
    UserVM userVM = new UserVM();
    List<CountryVM> countryList = new List<CountryVM>();
    ObservableCollection<CountryVM> countries = new ObservableCollection<CountryVM>();
    List<CityVM> cityList = new List<CityVM>();
    ObservableCollection<CityVM> cities = new ObservableCollection<CityVM>();
    Response response = new Response();
    public AddUser(UserVM _userVM = null)
	{
		InitializeComponent();
        if(_userVM != null)
        {
            userVM = _userVM;
            BindData(userVM);
        }
        GetCountriesList();
    }

    private void BindData(UserVM userVM)
    {
        userNameEntry.Text = userVM.UserName;
        countryCombobox.SelectedItem = userVM.CountryName;
        cityCombobox.SelectedItem = userVM.CityName;
        if (userVM.Gender == "Female")
            femaleGender.IsChecked = true;
        else if (userVM.Gender == "Male")
            maleGender.IsChecked = true;
        else if (userVM.Gender == "Others")
            otherGender.IsChecked = true;

        
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
                    countryCombobox.ItemsSource = countries;
                    //For edit purpose
                    if (!string.IsNullOrEmpty(userVM.CountryName))
                    {
                        countryCombobox.SelectedItem = userVM.CountryName;
                    }
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

    private void userNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(userNameEntry.Text))
            userNameError.IsVisible = false;
        else
        {
            userNameError.IsVisible = true;
        }
    }

    private void countryChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        try
        {
            CountryVM data = e.CurrentSelection.FirstOrDefault() as CountryVM;
            if (data != null)
            {
                userVM.CountryId = data.CountryId;
                userVM.CountryName = data.CountryName;
                countryNameError.IsVisible = false;
                GetCitiesData(data.CountryId);
            }
            else if (!string.IsNullOrEmpty(userVM.CityName))
            {
                //For edit purpose
                // cityCombobox.SelectedItem = userVM.CityName;
                
                GetCitiesById(userVM.CountryId??0);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private async void GetCitiesData(int countryId)
    {
        try
        {
            cityCombobox.SelectedItem = null;
            userVM.CityName = "";
            userVM.CityId = null ;
            GetCitiesById(countryId);

        }
        catch (Exception ex)
        {

        }
    }
    private async void GetCitiesById(int countryId)
    {
        try
        {
            
            response = await App.SQLiteDb.GetCitiesAsyncByCountryId(countryId);
            if (response != null && response.Status == ResponseStatus.OK)
            {
                var data = JsonConvert.SerializeObject(response.ResultData);
                cityList = JsonConvert.DeserializeObject<List<CityVM>>(data);
                if (cityList?.Count > 0)
                {
                    cities.Clear();
                    foreach (var item in cityList)
                    {
                        cities.Add(item);
                    }
                    cityCombobox.ItemsSource = cities;
                    if (!string.IsNullOrEmpty(userVM.CityName))
                    {
                        cityCombobox.SelectedItem = userVM.CityName;
                    }
                }

            }
            else
            {

            }

        }
        catch (Exception ex)
        {

        }
    }
    private void cityChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        try
        {
            CityVM data = e.CurrentSelection.FirstOrDefault() as CityVM;
            if (data != null)
            {
                userVM.CityId = data.CityId;
                userVM.CityName = data.CityName;
                cityNameError.IsVisible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private async void SubmitClick(object sender, EventArgs e)
    {
        try
        {
            userVM.UserName = userNameEntry.Text;
            if (Validation(userVM))
            {
                if (userVM?.UserId > 0)
                {
                    response = await App.SQLiteDb.UpdateUserAsync(userVM);
                    if (response != null)
                    {
                        if (response.Status == ResponseStatus.OK)
                        {
                            cities.Clear();
                            countries.Clear();
                            await DisplayAlert("Info", "Data Updated Successfully", "Ok");
                            userNameEntry.Text = "";
                            userNameError.IsVisible = false;
                            countryCombobox.SelectedItem = null;
                            cityCombobox.SelectedItem = null;
                            userVM = new UserVM();
                            userVM.Gender = "Male";
                            GetCountriesList();
                        }
                        else if (response.Status == ResponseStatus.Restrected)
                        {
                            await DisplayAlert("Info", "Already Exist", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Error", "Something went wrong", "Ok");
                        }

                    }
                }
                else
                {
                    response = await App.SQLiteDb.SaveUserAsync(userVM);
                    if (response != null)
                    {
                        if (response.Status == ResponseStatus.OK)
                        {
                            cities.Clear();
                            countries.Clear();
                            await DisplayAlert("Info", "Data saved Successfully", "Ok");
                            userNameEntry.Text = "";
                            userNameError.IsVisible = false;
                            countryCombobox.SelectedItem = null;
                            cityCombobox.SelectedItem = null;
                            userVM = new UserVM();
                            userVM.Gender = "Male";
                            maleGender.IsChecked = true;
                            GetCountriesList();
                        }
                        else if (response.Status == ResponseStatus.Restrected)
                        {
                            userNameError.IsVisible = true;
                            userNameError.Text = "User Name Already Exist";
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

    private bool Validation(UserVM userVM)
    {
        try
        {
            userNameError.IsVisible = false;
            countryNameError.IsVisible = false;
            cityNameError.IsVisible = false;
            genderError.IsVisible = false;
            ValidationResult result = new UserValidator().Validate(userVM);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case "UserName":
                            userNameError.IsVisible = true;
                            userNameError.Text = failure.ErrorMessage;
                            break;
                        case "CountryId":
                            countryNameError.IsVisible = true;
                            countryNameError.Text = failure.ErrorMessage;
                            break; 
                        case "CityId":
                            cityNameError.IsVisible = true;
                            cityNameError.Text = failure.ErrorMessage;
                            break; 
                        case "Gender":
                            genderError.IsVisible = true;
                            genderError.Text = failure.ErrorMessage;
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

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            RadioButton radioButton = (RadioButton)sender;
            userVM.Gender = radioButton.Value.ToString();
        }
        catch (Exception ex)
        {

        }

    }
}