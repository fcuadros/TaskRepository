export interface Task {
  id: number;
  title: string;
  description: string;
  dueDate: Date;
  state: number;
}

export interface CreateTaskRequest {
  title: string;
  description: string;
  dueDate: Date;
  state: number;
}