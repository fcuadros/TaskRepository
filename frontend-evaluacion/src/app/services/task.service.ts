import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, CreateTaskRequest } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'http://localhost:5124'; // Ajusta el puerto según tu backend

  constructor(private http: HttpClient) { }

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.apiUrl}/list-tasks`);
  }

  getTask(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/get-task/${id}`);
  }

  createTask(task: CreateTaskRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/create-task`, task);
  }

  updateTask(id: number, task: Task): Observable<any> {
    return this.http.put(`${this.apiUrl}/update-task/${id}`, task);
  }

  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete-task/${id}`);
  }
}