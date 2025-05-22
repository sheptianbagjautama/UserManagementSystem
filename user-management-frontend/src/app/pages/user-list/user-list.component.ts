import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NotificationService } from '../../services/notification.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './user-list.component.html'
})
export class UserListComponent implements OnInit {
  users: any[] = [];

  constructor(private userService: UserService, private notificationService: NotificationService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAll().subscribe(data => {
      this.users = data;
    });
  }

  deleteUser(id: number) {
  if (confirm('Are you sure?')) {
    this.userService.delete(id).subscribe({
      next: () => {
         this.notificationService.success('User deleted successfully!');
        this.loadUsers();
      },
      error: err => {
        this.notificationService.error('Failed to delete user: ' + err.message);
      }
    });
  }
}
}
