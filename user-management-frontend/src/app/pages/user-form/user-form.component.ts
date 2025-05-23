import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class UserFormComponent implements OnInit {
  user: User = {
    username: '',
    fullName: '',
    password: '',
    gender: 'Male',
    birthDate: '',
    address: '',
    phone: '',
  };

  isEditMode = false;
  userId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode = true;
      this.userId = +idParam;
      this.userService.getById(this.userId).subscribe({
        next: (data) => {
          console.log('User data:', data);
          this.user = {
            username: data.username,
            fullName: data.fullName,
            password: '',
            gender: data.gender ?? 'Male',
            birthDate: data.birthDate ? data.birthDate.split('T')[0] : '',
            address: data.address ?? '',
            phone: data.phone ?? '',
          };
        },
        error: (err) => {
          this.notificationService.error(
            'Failed to load user: ' + err.error.message
          );
        },
      });
    }
  }

  submitForm(): void {
    if (this.isEditMode && this.userId !== null) {
      const updatedUser: Partial<User> = {
        username: this.user.username,
        fullName: this.user.fullName,
        gender: this.user.gender,
        birthDate: this.user.birthDate,
        address: this.user.address,
        phone: this.user.phone,
      };

      if (this.user.password && this.user.password.trim() !== '') {
        updatedUser.password = this.user.password;
      }

      this.userService.update(this.userId, updatedUser).subscribe({
        next: () => {
          this.notificationService.success('User updated successfully!');
          this.router.navigate(['/users']);
        },
        error: (err) => {
          console.log('Error updating user:', err);
          this.notificationService.error(err.error.message);
        },
      });
    } else {
      this.userService.create(this.user).subscribe({
        next: () => {
          this.notificationService.success('User created successfully!');
          this.router.navigate(['/users']);
        },
        error: (err) => {
          console.log('Error creating user:', err);
          this.notificationService.error(err.error.message);
        },
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/users']);
  }
}
