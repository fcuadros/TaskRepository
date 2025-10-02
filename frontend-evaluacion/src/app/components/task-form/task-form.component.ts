import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { Task, CreateTaskRequest } from '../../models/task.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss']
})
export class TaskFormComponent implements OnInit {
  taskForm: FormGroup;
  isEdit = false;
  taskId?: number;
  loading = false;
  error = '';

  states = [
    { value: 1, label: 'Pendiente' },
    { value: 2, label: 'En Progreso' },
    { value: 3, label: 'Completada' }
  ];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required]],
      dueDate: ['', [Validators.required]],
      state: [1, [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.taskId = this.route.snapshot.params['id'];
    if (this.taskId) {
      this.isEdit = true;
      this.loadTask(this.taskId);
    }
  }

  loadTask(id: number): void {
    this.loading = true;
    this.taskService.getTask(id).subscribe({
      next: (task) => {
        this.taskForm.patchValue({
          title: task.title,
          description: task.description,
          dueDate: new Date(task.dueDate).toISOString().split('T')[0],
          state: task.state
        });
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading task:', err);
        this.error = 'Error al cargar la tarea';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      this.loading = true;
      this.error = '';
      
      const formValue = this.taskForm.value;
      const dueDate = new Date(formValue.dueDate);
      
      if (this.isEdit && this.taskId) {
        const task: Task = {
          id: this.taskId,
          title: formValue.title,
          description: formValue.description,
          dueDate: dueDate,
          state: formValue.state
        };
        
        this.taskService.updateTask(this.taskId, task).subscribe({
          next: () => {
            this.loading = false;
            this.navigateToList();
          },
          error: (err) => {
            console.error('Error updating task:', err);
            this.error = 'Error al actualizar la tarea';
            this.loading = false;
          }
        });
      } else {
        const taskRequest: CreateTaskRequest = {
          title: formValue.title,
          description: formValue.description,
          dueDate: dueDate,
          state: formValue.state
        };
        
        this.taskService.createTask(taskRequest).subscribe({
          next: () => {
            this.loading = false;
            this.navigateToList();
          },
          error: (err) => {
            console.error('Error creating task:', err);
            this.error = 'Error al crear la tarea';
            this.loading = false;
          }
        });
      }
    } else {
      // Marcar todos los campos como touched para mostrar errores
      Object.keys(this.taskForm.controls).forEach(key => {
        this.taskForm.get(key)?.markAsTouched();
      });
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.taskForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }

  // Método público para navegar a la lista
  navigateToList(): void {
    this.router.navigate(['/tasks']);
  }

  // Método público para cancelar
  cancel(): void {
    this.navigateToList();
  }
}