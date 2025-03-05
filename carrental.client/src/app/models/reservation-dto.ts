export interface ReservationDto {
  reservationId: number;
  vehicleId: number;
  vehicleRegistrationNumber: string;
  customerId: number;
  startTime: string;
  endTime: string;
  status: string;
  createdAt: string;
}
