import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import { ReserveVehicleComponent } from './components/reserve-vehicle/reserve-vehicle.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { MyReservationsComponent } from './components/my-reservations/my-reservations.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: 'vehicles', component: VehicleListComponent },
  { path: 'reserve/:id', component: ReserveVehicleComponent },
  { path: 'my-reservations', component: MyReservationsComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
  { path: '**', redirectTo: 'vehicles' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
