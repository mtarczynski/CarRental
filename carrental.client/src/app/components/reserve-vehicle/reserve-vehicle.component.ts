import { Component, OnInit } from '@angular/core';
import { ReservationService } from '../../services/reservation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CreateReservationDto } from '../../models/create-reservation-dto';
import { ReservationDto } from '../../models/reservation-dto';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-reserve-vehicle',
  standalone: false,
  templateUrl: './reserve-vehicle.component.html',
  styleUrl: './reserve-vehicle.component.css'
})
export class ReserveVehicleComponent implements OnInit{
  vehicleId!: number;
  startTime!: string;
  endTime!: string;

  customerId: number | null = null;

  successMessage: string = '';
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private reservationService: ReservationService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.vehicleId = Number(this.route.snapshot.paramMap.get('id'));

    this.startTime = this.route.snapshot.queryParamMap.get('start') || '';
    this.endTime = this.route.snapshot.queryParamMap.get('end') || '';

    this.customerId = this.authService.getUserId();
    if (!this.customerId) {
      this.router.navigate(['/login']);
    }
  }

  onConfirmReservation(): void {
    if (this.customerId === null) {
      this.errorMessage = 'Nie jesteś zalogowany!';
      return;
    }

    const dto: CreateReservationDto = {
      vehicleId: this.vehicleId,
      customerId: this.customerId,
      startTime: this.startTime,
      endTime: this.endTime
    };

    this.reservationService.createReservation(dto).subscribe({
      next: (res: ReservationDto) => {
        this.successMessage = `Rezerwacja utworzona! ID = ${res.reservationId}, Status = ${res.status}`;
        this.errorMessage = '';
      },
      error: (err) => {
        this.errorMessage = `Błąd przy tworzeniu rezerwacji: ${err.error || err.message}`;
        this.successMessage = '';
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/vehicles']);
  }
}
