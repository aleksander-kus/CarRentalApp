using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain.CarList
{
    public class CarListFilter
    {
        public string[] ExcludedBrands { get; set; } = Array.Empty<string>();
        public string[] ExcludedModels { get; set; } = Array.Empty<string>();
        public string[] ExcludedCategories { get; set; } = Array.Empty<string>();
        [Range(0, 10)] public int CapacityMin { get; set; } = 0;
        [Range(0, 10)] public int CapacityMax { get; set; } = 10;
        [Range(0, 2100)] public int ProductionYearMin { get; set; } = 0;
        [Range(0, 2100)] public int ProductionYearMax { get; set; } = DateTime.Now.Year;
        [Range(0, 2100)] public int HorsePowerMin { get; set; } = 0;
        [Range(0, 2100)] public int HorsePowerMax { get; set; } = 1000;

        public void Validate()
        {
            ExcludedBrands = FixStringArray(ExcludedBrands);
            ExcludedModels = FixStringArray(ExcludedModels);
            ExcludedCategories = FixStringArray(ExcludedCategories);
        }

        private string[] FixStringArray(string[] array)
        {
            if (array == null || array.Length != 1 || array[0] == null)
                return Array.Empty<string>();
            return array[0].Split(',');
        }
    }
}