using System;
using System.Collections.Generic;
using System.Linq;
using UniWeb.API.DataContext;
using UniWeb.API.Entities;
using UniWeb.API.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Dynamic;

namespace UniWeb.API.Services
{
    public class PlaceService
    {
        private EFDataContext _dbcontext;
        private static IEnumerable<Country> _countries = null;
        private static IDictionary<string, object> _countrySubdivisions = null;
        private static Dictionary<string, int[]> _currenciesByTimezone = new Dictionary<string, int[]>()
        {
            { "India Standard Time", new int [] {1}},
            { "Indian Standard Time", new int [] {1}},
            { "Arabian Standard Time", new int [] {2, 4}},
            { "Gulf Standard Time", new int [] {3, 5}},
            { "Eastern Standard Time", new int[] {6}},
            { "Central Standard Time", new int[] {6}},
            { "Mountain Standard Time", new int[] {6}},
            { "Pacific Standard Time", new int[] {6}},
            { "Alaska Standard Time", new int[] {6}},
            { "Hawaii-Aleutian Standard Time", new int[] {6}},
            { "Eastern Daylight Time", new int[] {6}},
            { "Central Daylight Time", new int[] {6}},
            { "Mountain Daylight Time", new int[] {6}},
            { "Pacific Daylight Time", new int[] {6}},
            { "Alaska Daylight Time", new int[] {6}},
            { "Hawaii-Aleutian Daylight Time", new int[] {6}},
            { "Australian Eastern Daylight Time", new int[] {7}},
            { "Australian Eastern Standard Time", new int[] {7}},
            { "Norfolk Island Daylight Time", new int[] {7}},
            { "Norfolk Island Standard Time", new int[] {7}},
            { "Australian Central Daylight Time", new int[] {7}},
            { "Australian Central Standard Time", new int[] {7}},
            { "Australian Western Standard Time", new int[] {7}},
            { "Australian Western Daylight Time", new int[] {7}},
            { "Christmas Island Time", new int[] {7}},
            { "China Standard Time", new int[] {8}}
        };

        public PlaceService(EFDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public List<CurrencyUnitDto> GetCurrenciesByTimeZone(string timeZoneStandardName)
        {
            if (_currenciesByTimezone.ContainsKey(timeZoneStandardName))
            {
                var currencyUnitIds = _currenciesByTimezone[timeZoneStandardName];
                var currencyUnits = _dbcontext.CurrencyUnits.Where(x => currencyUnitIds.Contains(x.Id))
                .ToList();
                return currencyUnits.Select(x => new CurrencyUnitDto() {
                    Code = x.Code,
                    Country = x.Country,
                    Currency = x.Currency,
                    Id = x.Id,
                    Symbol = x.Symbol
                }).ToList();
            } else  {
                var currencyUnits = _dbcontext.CurrencyUnits.Where(x => x.Id == 1)
                .ToList();
                return currencyUnits.Select(x => new CurrencyUnitDto() {
                    Code = x.Code,
                    Country = x.Country,
                    Currency = x.Currency,
                    Id = x.Id,
                    Symbol = x.Symbol
                }).ToList();                
            }
        }


        

  

        public IEnumerable<TimeZoneDto> GetTimeZones()
        {
            var allTimeZones = TimeZoneInfo.GetSystemTimeZones();
            var items = allTimeZones.Select(x => new TimeZoneDto()
            {
                Id = x.Id,
                DisplayName = x.DisplayName,
                Name = x.StandardName
            });
            return items;
        }

        public Country GetCountry(string countryCode)
        {
            var countries = GetCountries();
            var country = countries.SingleOrDefault(x => x.Code == countryCode);
            if (null == country)
            {
                throw new InvalidOperationException($"Invalid country code {countryCode}");
            }
            return country;
        }

        public IEnumerable<Country> GetCountries()
        {
            if (null == PlaceService._countries)
            {
                var data = Convert.FromBase64String(StaticData.CountriesBase64);
                var json = Encoding.UTF8.GetString(data);
                PlaceService._countries = JsonConvert
                .DeserializeObject<IEnumerable<Country>>(json);
            }
            return _countries;
        }
    }
}