export interface ReservationDto {
  reservationId: number;
  vehicleId: number;
  vehicleRegistrationNumber: string;
  customerId: number;
  startTime: number;
  endTime: number;
  status: string;
  createdAt: string;
}
