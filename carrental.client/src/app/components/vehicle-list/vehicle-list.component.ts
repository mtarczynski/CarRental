import { Component, OnInit } from '@angular/core';
import { VehicleDto } from '../../models/vehicle-dto';
import { VehicleService } from '../../services/vehicle.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vehicle-list',
  standalone: false,
  templateUrl: './vehicle-list.component.html',
  styleUrl: './vehicle-list.component.css'
})
export class VehicleListComponent implements OnInit {
  vehicles: VehicleDto[] = [];

  startTime: Date = new Date();
  endTime: Date = new Date();

  displayedColumns: string[] = ['brand', 'model', 'year', 'vehicleTypeName', 'action'];

  constructor(
    private vehicleService: VehicleService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.searchVehicles();
  }

  searchVehicles(): void {
    const startIso = this.startTime.toISOString();
    const endIso = this.endTime.toISOString();

    this.vehicleService.getAvailableVehicles(startIso, endIso).subscribe({
      next: (data) => {
        this.vehicles = data;
      },
      error: (err) => {
        console.error('Błąd podczas pobierania listy pojazdów:', err);
      }
    });
  }

  onReserve(vehicle: VehicleDto): void {
    this.router.navigate(['/reserve', vehicle.id], {
      queryParams: {
        start: this.startTime.toISOString(),
        end: this.endTime.toISOString()
      }
    });
  }
}
