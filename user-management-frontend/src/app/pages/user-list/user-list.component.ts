import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { NotificationService } from '../../services/notification.service';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  standalone: true,
  imports: [RouterModule,CommonModule],
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  user = { username : 'Guest'};

  constructor(
    private userService: UserService,
    private notificationService: NotificationService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const saveUsername = localStorage.getItem('username');
    if(saveUsername) this.user.username = saveUsername;
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAll().subscribe({
      next: (data) => (this.users = data),
      error: (err) => this.notificationService.error('Failed to load users: ' + err.message)
    });
  }

  deleteUser(id: number): void {
    if (confirm('Are you sure to delete this user?')) {
      this.userService.delete(id).subscribe({
        next: () => {
          this.notificationService.success('User deleted successfully!');
          this.loadUsers();
        },
        error: (err) => this.notificationService.error('Delete failed: ' + err.message)
      });
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
