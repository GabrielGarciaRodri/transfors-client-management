import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Cliente } from '../../../models/cliente.model';
import { ClienteService } from '../../../services/cliente.service';

@Component({
  selector: 'app-clientes-list',
  imports: [FormsModule, RouterLink],
  templateUrl: './clientes-list.html',
  styleUrl: './clientes-list.scss'
})
export class ClientesList implements OnInit {
  private readonly service = inject(ClienteService);

  protected readonly clientes = signal<Cliente[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);

  protected search = '';
  protected estadoFiltro: '' | 'true' | 'false' = '';

  private debounce?: ReturnType<typeof setTimeout>;

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);

    const estado = this.estadoFiltro === '' ? null : this.estadoFiltro === 'true';

    this.service.getAll(this.search, estado).subscribe({
      next: data => {
        this.clientes.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No se pudo cargar la lista de clientes. Verifica que la API esté en ejecución.');
        this.loading.set(false);
      }
    });
  }

  /** Búsqueda con debounce para no golpear la API en cada tecla. */
  onSearchChange(): void {
    clearTimeout(this.debounce);
    this.debounce = setTimeout(() => this.load(), 350);
  }

  eliminar(cliente: Cliente): void {
    const ok = confirm(
      `¿Eliminar al cliente ${cliente.nombres} ${cliente.apellidos} (${cliente.numeroDocumento})?`
    );
    if (!ok) return;

    this.service.delete(cliente.id).subscribe({
      next: () => this.load(),
      error: () => this.error.set('No se pudo eliminar el cliente.')
    });
  }
}
