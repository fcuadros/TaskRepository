import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FacturaSimple } from '../models/factura-simple.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FacturaService {
  private apiUrl = 'http://localhost:5124/listar-solo-serie-cliente';

  constructor(private http: HttpClient) {}

  obtenerFacturas(): Observable<FacturaSimple[]> {
    return this.http.get<FacturaSimple[]>(this.apiUrl);
  }
}