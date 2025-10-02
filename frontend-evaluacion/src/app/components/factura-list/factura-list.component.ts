import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FacturaService } from '../../services/factura.service';
import { FacturaSimple } from '../../models/factura-simple.model';

@Component({
   standalone: true,
  selector: 'app-factura-list',
  imports: [CommonModule],
  templateUrl: './factura-list.component.html',
  styleUrls: ['./factura-list.component.scss']
})
export class FacturaListComponent {
  facturas: FacturaSimple[] = [];

  constructor(private facturaService: FacturaService) {}

  ngOnInit(): void {
    console.log(1);
    this.facturaService.obtenerFacturas().subscribe({
      next: (data) => this.facturas = data,
      error: (err) => console.error( "error", err)
    });
  }
}
