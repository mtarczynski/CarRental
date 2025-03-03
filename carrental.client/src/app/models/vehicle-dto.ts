export interface VehicleDto {
  id: number;
  brand: string;
  model: string;
  year: number;
  registrationNumber: string;

  vehicleTypeId: number;
  vehicleTypeName: string;
}
