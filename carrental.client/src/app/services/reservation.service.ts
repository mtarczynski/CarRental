import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { CreateReservationDto } from "../models/create-reservation-dto";
import { Observable } from "rxjs";
import { ReservationDto } from "../models/reservation-dto";

@Injectable({
  providedIn: 'root'
})

export class ReservationService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createReservation(dto: CreateReservationDto): Observable<ReservationDto> {
    return this.http.post<ReservationDto>(`${this.apiUrl}/reservations`, dto);
  }

  getMyReservations(): Observable<ReservationDto[]> {
    return this.http.get<ReservationDto[]>(`${this.apiUrl}/reservations/my`);
  }

  returnVehicle(reservationId: number): Observable<ReservationDto> {
    return this.http.patch<ReservationDto>(`https://localhost:44380/api/reservations/${reservationId}/return`, {});
  }
}
