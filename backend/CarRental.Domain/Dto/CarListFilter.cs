using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarListFilter
    {
        [JsonPropertyName("brands")]
        public string[] Brands { get; set; } = Array.Empty<string>();
        [JsonPropertyName("models")]
        public string[] Models { get; set; } = Array.Empty<string>();
        [JsonPropertyName("categories")]
        public string[] Categories { get; set; } = Array.Empty<string>();
        [JsonPropertyName("capacityMin")]
        [Range(0, 20)] public int CapacityMin { get; set; } = 0;
        [JsonPropertyName("capacityMax")]
        [Range(0, 20)] public int CapacityMax { get; set; } = 10;
        [JsonPropertyName("productionYearMin")]
        [Range(0, 2100)] public int ProductionYearMin { get; set; } = 0;
        [JsonPropertyName("productionYearMax")]
        [Range(0, 2100)] public int ProductionYearMax { get; set; } = DateTime.Now.Year;
        [JsonPropertyName("horsePowerMin")]
        [Range(0, 2100)] public int HorsePowerMin { get; set; } = 0;
        [JsonPropertyName("horsePowerMax")]
        [Range(0, 2100)] public int HorsePowerMax { get; set; } = 1000;

        public void Validate()
        {
            Brands = FixStringArray(Brands);
            Models = FixStringArray(Models);
            Categories = FixStringArray(Categories);
        }

        private string[] FixStringArray(string[] array)
        {
            if (array == null || array.Length != 1 || array[0] == null)
                return Array.Empty<string>();
            return array[0].Split(',');
        }

        public static CarListFilter All = new CarListFilter();
    }
}