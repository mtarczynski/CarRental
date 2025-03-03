import { Component, OnInit } from '@angular/core';
import { ReservationService } from '../../services/reservation.service';
import { ReservationDto } from '../../models/reservation-dto';

@Component({
  selector: 'app-my-reservations',
  standalone: false,
  templateUrl: './my-reservations.component.html',
  styleUrl: './my-reservations.component.css'
})
export class MyReservationsComponent implements OnInit {
  reservations: ReservationDto[] = [];
  errorMessage: string = '';
  loading: boolean = false;

  displayedColumns: string[] = ['vehicle', 'startTime', 'endTime', 'status', 'actions'];

  constructor(private reservationService: ReservationService) { }

  ngOnInit(): void {
    this.fetchMyReservations();
  }

  fetchMyReservations(): void {
    this.loading = true;
    this.reservationService.getMyReservations().subscribe({
      next: (data) => {
        this.reservations = data;
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = err.error || err.message;
        this.loading = false;
      }
    });
  }
  onReturnVehicle(reservationId: number): void {
    this.reservationService.returnVehicle(reservationId).subscribe({
      next: (updatedReservation) => {
        this.fetchMyReservations();
      },
      error: (err) => {
        if (typeof err.error === 'string') {
          this.errorMessage = err.error;
        } else if (err.error && err.error.message) {
          this.errorMessage = err.error.message;
        } else {
          this.errorMessage = err.message || 'Wystąpił błąd.';
        }
      }
    });
  }
}
