import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  loading = true;
  error = '';

  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.loading = true;
    this.error = '';
    
    this.taskService.getTasks().subscribe({
      next: (data) => {
        this.tasks = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading tasks:', err);
        this.error = 'Error al cargar las tareas';
        this.loading = false;
      }
    });
  }

  deleteTask(id: number): void {
    if (confirm('¿Estás seguro de que quieres eliminar esta tarea?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => {
          this.tasks = this.tasks.filter(task => task.id !== id);
        },
        error: (err) => {
          console.error('Error deleting task:', err);
          alert('Error al eliminar la tarea');
        }
      });
    }
  }

  getStateText(state: number): string {
    switch (state) {
      case 1: return 'Pendiente';
      case 2: return 'En Progreso';
      case 3: return 'Completada';
      default: return 'Desconocido';
    }
  }

  getStateClass(state: number): string {
    switch (state) {
      case 1: return 'state-pending';
      case 2: return 'state-in-progress';
      case 3: return 'state-completed';
      default: return 'state-unknown';
    }
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('es-ES');
  }
}