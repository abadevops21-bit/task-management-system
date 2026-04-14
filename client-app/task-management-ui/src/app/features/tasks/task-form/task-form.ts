import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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

  private route = inject(ActivatedRoute);

  taskId: any | null = null;
  isEditMode = signal(false);

  private fb = inject(FormBuilder);
  private taskService = inject(TaskService);
  private router = inject(Router);

  loading = signal(false);
  errorMessage = signal('');

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required]],
    description: [''],
    isCompleted: false 
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.taskId = id;
      this.isEditMode.set(true);
      this.loadTask();
    }
  }

  loadTask() {
    this.taskService.getTask(this.taskId!).subscribe(task => {
      this.form.patchValue({
        title: task.title,
        description: task.description,
        isCompleted: task.isCompleted 
      });
    });
  }

  goBack() {
    this.router.navigate(['/tasks']);
  }
  submit() {
    if (this.form.invalid) return;

    this.loading.set(true);

    const request = this.form.getRawValue();

    if (this.isEditMode()) {
      // UPDATE
      this.taskService.updateTask(this.taskId!, request).subscribe({
        next: () => this.goBack(),
        error: () => this.loading.set(false)
      });
    } else {
      // CREATE
      this.taskService.createTask(request).subscribe({
        next: () => this.goBack(),
        error: () => this.loading.set(false)
      });
    }
  }
}