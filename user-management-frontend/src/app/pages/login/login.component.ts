import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [FormsModule, RouterModule, CommonModule], 
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private authService: AuthService, private router: Router, private notificationService: NotificationService) {}

  login() {
    this.authService.login(this.username, this.password).subscribe({
      next: res => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('username', this.username);
        this.router.navigate(['/users']);
        this.notificationService.success('Login successful!');
      },
      error: err => {
        this.notificationService.error(err.error.message || 'Login failed!');
      } 
    });
  }
}
