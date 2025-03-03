import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { VehicleDto } from "../models/vehicle-dto";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class VehicleService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAvailableVehicles(start: string, end: string): Observable<VehicleDto[]> {
    const params = new HttpParams()
      .set('start', start)
      .set('end', end);

    return this.http.get<VehicleDto[]>(`${this.apiUrl}/vehicles/available`, { params });
  }
}
