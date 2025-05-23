import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
})
export class RegisterComponent {
  user = {
    username: '',
    password: '',
    fullName: '',
    gender: '',
    birthDate: '',
    address: '',
    phone: '',
  };

  confirmPassword: string = '';
  passwordMismatch: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private notification: NotificationService
  ) {}

  register() {
    this.passwordMismatch = false;

  if (this.user.password !== this.confirmPassword) {
    this.passwordMismatch = true;
    return;
  }

    this.authService.register(this.user).subscribe({
      next: () => {
        this.notification.success('Registration successful!');
        this.router.navigate(['/login']);
      },
      error: err => {
        this.notification.error('Registration failed: ' + err.error?.message || err.message);
      }
    });
  }
}
