import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [FormsModule], 
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private authService: AuthService, private router: Router, private notificationService: NotificationService) {}

  login() {
    this.authService.login(this.username, this.password).subscribe({
      next: res => {
        localStorage.setItem('token', res.token);
        this.router.navigate(['/users']);
        this.notificationService.success('Login successful!');
      },
      error: err => {
        this.notificationService.error(err.error.message || 'Login failed!');
      } 
    });
  }
}
