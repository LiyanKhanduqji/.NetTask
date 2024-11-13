import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css',
})
export class ServerErrorComponent {
  error: any;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error'];
  }

  goHome() {
    this.router.navigateByUrl('/');
  }
}

//constructor(private router: Router): This injects Angular’s Router service into the component, allowing it to access the current navigation state (the current route, URL, etc.) and any data passed during navigation.
//This error property can now be used in the template (server-error.component.html) to display the error message or details to the user.
