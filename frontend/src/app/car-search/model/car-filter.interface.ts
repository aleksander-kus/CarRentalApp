export interface CarFilter {
  brands: string[];
  models: string[];
  categories: string[];
  capacityMin: number;
  capacityMax: number;
  productionYearMin : number;
  productionYearMax : number;
  horsePowerMin: number;
  horsePowerMax: number;
}
