import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class UserFormComponent implements OnInit {
  user = {
    username: '',
    fullName: '',
    password: ''
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
      this.userService.getById(this.userId).subscribe(data => {
        this.user = {
          username: data.username,
          fullName: data.fullName,
          password: '' // tidak diisi password saat edit
        };
      });
    }
  }

  submitForm(): void {
    if (this.isEditMode && this.userId !== null) {
      const updatedUser = {
        username: this.user.username,
        fullName: this.user.fullName,
        password: this.user.password
      };

      this.userService.update(this.userId, updatedUser).subscribe({
        next: () => {
          this.notificationService.success('User updated successfully!');
          this.router.navigate(['/users']);
        },
        error: err => {
          this.notificationService.error('Failed to update user: ' + err.message);
        }
      });
    } else {
      this.userService.create(this.user).subscribe({
        next: () => {
          this.notificationService.success('User created successfully!');
          this.router.navigate(['/users']);
        },
        error: err => {
          this.notificationService.error('Failed to create user: ' + err.message);
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/users']);
  }
}
