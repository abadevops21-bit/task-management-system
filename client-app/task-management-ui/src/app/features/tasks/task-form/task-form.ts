import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { signal } from '@angular/core';
import { TaskService } from '@core/services/task.service';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.css'
})
export class TaskForm {

  private fb = inject(FormBuilder);
  private taskService = inject(TaskService);
  private router = inject(Router);

  loading = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required]],
    description: ['']
  });

  goBack() {
    this.router.navigate(['/tasks']);
  }
  submit() {
    if (this.form.invalid) return;

    this.loading.set(true);

    this.taskService.createTask(this.form.getRawValue()).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/tasks']);
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Failed to create task');
        this.loading.set(false);
      }
    });
  }
}