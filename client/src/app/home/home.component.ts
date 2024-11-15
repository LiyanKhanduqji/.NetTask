import { Component, inject } from '@angular/core';
import { RegisterComponent } from '../register/register.component';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  accountService = inject(AccountService);
  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
  registerMode = false;
  users: any;

  registerToggle() {
    this.registerMode = !this.registerMode;
  }
}
