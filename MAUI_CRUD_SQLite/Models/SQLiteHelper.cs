using Java.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_CRUD_SQLite.Models
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<CountryVM>().Wait();
            db.CreateTableAsync<CityVM>().Wait();
            db.CreateTableAsync<UserVM>().Wait();
        }

        //Start User Crud
        //Insert  new record
        public async Task<Response> SaveUserAsync(UserVM user)
        {
            Response response = new Response();
            try
            {
                var data = await db.Table<UserVM>().Where(i => i.UserName == user.UserName).FirstOrDefaultAsync();
                if (data == null)
                {
                    var res = await db.InsertAsync(user);
                    response.Status = ResponseStatus.OK;
                    response.Message = "Saved";
                    response.ResultData = res;
                }
                else
                {
                    response.Status = ResponseStatus.Restrected;
                    response.Message = "Already exist";
                }

            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }
        //Update Record
        public async Task<Response> UpdateUserAsync(UserVM user)
        {
            Response response = new Response();
            try
            {
                if (user.UserId > 0)
                {
                    var res = await db.UpdateAsync(user);
                    response.Status = ResponseStatus.OK;
                    response.Message = "Updated";
                    response.ResultData = res;
                }
                else
                {
                    response.Status = ResponseStatus.Restrected;
                    response.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }
        //Delete
        public async Task<Response> DeleteUserAsync(UserVM user)
        {
            Response response = new Response();
            try
            {
                await db.DeleteAsync(user);
                response.Status = ResponseStatus.OK;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }

        //Read All Items
        public async Task<Response> GetUserAsync()
        {
            Response response = new Response();
            try
            {
                var list = await db.Table<UserVM>().ToListAsync();
                if (list?.Count > 0)
                {
                    response.Status = ResponseStatus.OK;
                    response.ResultData = list;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;

        }


        //Read Item
        public async Task<Response> GetUserAsyncById(int userId)
        {
            Response response = new Response();
            try
            {
                var data = await db.Table<UserVM>().Where(i => i.UserId == userId).FirstOrDefaultAsync();
                if (data != null)
                {
                    response.Status = ResponseStatus.OK;
                    response.ResultData = data;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;

        }
        // End User Crud


        //Start Country Crud
        //Insert  new record
        public async Task<Response> SaveCountryAsync(CountryVM country)
        {
            Response response = new Response();
            try
            {
                var data = await db.Table<CountryVM>().Where(i => i.CountryName.ToLower() == country.CountryName.ToLower()).FirstOrDefaultAsync();
                if (data == null)
                {
                    var res = await db.InsertAsync(country);
                    response.Status = ResponseStatus.OK;
                    response.Message = "Saved";
                    response.ResultData = res;
                }
                else
                {
                    response.Status = ResponseStatus.Restrected;
                    response.Message = "Already exist";
                }

            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }
        //Update Record
        public async Task<Response> UpdateCountryAsync(CountryVM Country)
        {
            Response response = new Response();
            try
            {
                if (Country.CountryId > 0)
                {
                    var data = await db.Table<CountryVM>().Where(i => i.CountryName.ToLower() == Country.CountryName.ToLower() && i.CountryId != Country.CountryId).FirstOrDefaultAsync();
                    if (data == null)
                    {
                        var res = await db.UpdateAsync(Country);
                        response.Status = ResponseStatus.OK;
                        response.Message = "Updated";
                        response.ResultData = res;
                    }
                    else
                    {
                        response.Status = ResponseStatus.Restrected;
                        response.Message = "Already exist";
                    }
                }
                else
                {
                    response.Status = ResponseStatus.Restrected;
                    response.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }
        //Delete
        public async Task<Response> DeleteCountryAsync(CountryVM Country)
        {
            Response response = new Response();
            try
            {
                await db.DeleteAsync(Country);
                response.Status = ResponseStatus.OK;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }

        //Read All Items
        public async Task<Response> GetCountryAsync()
        {
            Response response = new Response();
            try
            {
                var list = await db.Table<CountryVM>().ToListAsync();
                if (list?.Count > 0)
                {
                    response.Status = ResponseStatus.OK;
                    response.ResultData = list;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;

        }


        //Read Item
        public async Task<Response> GetCountryAsyncById(int CountryId)
        {
            Response response = new Response();
            try
            {
                var data = await db.Table<CountryVM>().Where(i => i.CountryId == CountryId).FirstOrDefaultAsync();
                if (data != null)
                {
                    response.Status = ResponseStatus.OK;
                    response.ResultData = data;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;

        }
        // End Country Crud


        //Start City Crud
        //Insert  new record
        public async Task<Response> SaveCityAsync(CityVM City)
        {
            Response response = new Response();
            try
            {
                var data = await db.Table<CityVM>().Where(i => i.CityName.ToLower() == City.CityName.ToLower()).FirstOrDefaultAsync();
                if(data == null)
                {
                    var res = await db.InsertAsync(City);
                    response.Status = ResponseStatus.OK;
                    response.Message = "Saved";
                    response.ResultData = res;
                }
                else
                {
                    response.Status = ResponseStatus.Restrected;
                    response.Message = "Already exist";
                }
                
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }
        //Update Record
        public async Task<Response> UpdateCityAsync(CityVM City)
        {
            Response response = new Response();
            try
            {
                if (City.CityId > 0)
                {
                    var data = await db.Table<CityVM>().Where(i => i.CityName.ToLower() == City.CityName.ToLower() && i.CityId != City.CityId).FirstOrDefaultAsync();
                    if (data == null)
                    {
                        var res = await db.UpdateAsync(City);
                        response.Status = ResponseStatus.OK;
                        response.Message = "Updated";
                        response.ResultData = res;
                    }
                    else
                    {
                        response.Status = ResponseStatus.Restrected;
                        response.Message = "Already exist";
                    }
                }
                else
                {
                    response.Status = ResponseStatus.Restrected;
                    response.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }
        //Delete
        public async Task<Response> DeleteCityAsync(CityVM City)
        {
            Response response = new Response();
            try
            {
                await db.DeleteAsync(City);
                response.Status = ResponseStatus.OK;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }
        //Read All Items
        public async Task<Response> GetCityAsync()
        {
            Response response = new Response();
            try
            {
                var list = await db.Table<CityVM>().ToListAsync();
                if (list?.Count > 0)
                {
                    response.Status = ResponseStatus.OK;
                    response.ResultData = list;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;

        }
        //Read Item
        public async Task<Response> GetCityAsyncById(int CityId)
        {
            Response response = new Response();
            try
            {
                var data = await db.Table<CityVM>().Where(i => i.CityId == CityId).FirstOrDefaultAsync();
                if (data != null)
                {
                    response.Status = ResponseStatus.OK;
                    response.ResultData = data;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;

        }
        // Get Cities BY CountryId
        public async Task<Response> GetCitiesAsyncByCountryId(int countryId)
        {
            Response response = new Response();
            try
            {
                var list = await db.Table<CityVM>().Where(i => i.CountryId == countryId).ToListAsync();
                if (list?.Count > 0)
                {
                    response.Status = ResponseStatus.OK;
                    response.ResultData = list;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;

        }
        // End cities Crud



        // Drop All tables
        public Task<int> DeleteAllItems<T>()
        {
             db.DropTableAsync<CountryVM>();
             db.DropTableAsync<CityVM>();
            return db.DropTableAsync<UserVM>();
             
        }
    }
}
