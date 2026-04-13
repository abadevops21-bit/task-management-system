import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { TaskService } from '@core/services/task.service';
import { Task, TaskListResponse } from '@models/task.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css'
})
export class TaskList implements OnInit {

  private taskService = inject(TaskService);

  // Make Math available in template
  Math = Math;

  tasks: Task[] = [];
  loading = false;
  errorMessage = '';

  // pagination
  pageNumber = 1;
  pageSize = 5;
  totalCount = 0;

  // filter
  isCompleted: boolean | null = null;

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.loading = true;
    this.errorMessage = '';

    const params: {
      pageNumber?: number;
      pageSize?: number;
      isCompleted?: boolean;
    } = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    };

    if (this.isCompleted !== null) {
      params.isCompleted = this.isCompleted;
    }

    this.taskService.getTasks(params).subscribe({
      next: (res: TaskListResponse) => {
        this.tasks = res.data;
        this.totalCount = res.totalCount;
        this.loading = false;
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Failed to load tasks';
        this.loading = false;
        console.error('Error loading tasks:', err);
      }
    });
  }

  changeFilter(value: string) {
    if (value === 'all') this.isCompleted = null;
    else if (value === 'completed') this.isCompleted = true;
    else this.isCompleted = false;

    this.pageNumber = 1;
    this.loadTasks();
  }

  nextPage() {
    const maxPage = Math.ceil(this.totalCount / this.pageSize);
    if (this.pageNumber < maxPage) {
      this.pageNumber++;
      this.loadTasks();
    }
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadTasks();
    }
  }

  deleteTask(id: number) {
    if (!confirm('Are you sure you want to delete this task?')) return;

    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.loadTasks(); // Refresh the list after successful deletion
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Failed to delete task';
        console.error('Error deleting task:', err);
      }
    });
  }
}