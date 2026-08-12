import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import {
  ClienteRequest,
  GENEROS,
  TIPOS_DOCUMENTO
} from '../../../models/cliente.model';
import { ClienteService } from '../../../services/cliente.service';

@Component({
  selector: 'app-cliente-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './cliente-form.html',
  styleUrl: './cliente-form.scss'
})
export class ClienteForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(ClienteService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  protected readonly tiposDocumento = TIPOS_DOCUMENTO;
  protected readonly generos = GENEROS;

  protected readonly saving = signal(false);
  protected readonly loading = signal(false);
  protected readonly serverError = signal<string | null>(null);
  protected readonly clienteId = signal<number | null>(null);
  protected readonly isEdit = computed(() => this.clienteId() !== null);

  // Validaciones espejo de las del backend (DataAnnotations).
  protected readonly form = this.fb.nonNullable.group({
    tipoDocumento: ['', Validators.required],
    numeroDocumento: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20),
      Validators.pattern(/^[A-Za-z0-9\-]+$/)]],
    nombres: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
    apellidos: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
    fechaNacimiento: ['', Validators.required],
    genero: ['', Validators.required],
    telefono: ['', [Validators.required, Validators.minLength(7), Validators.maxLength(20),
      Validators.pattern(/^[0-9\+\-\s]+$/)]],
    correoElectronico: ['', [Validators.required, Validators.email, Validators.maxLength(150)]],
    direccion: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
    ciudad: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
    estado: [true, Validators.required]
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.clienteId.set(Number(idParam));
      this.cargarCliente(Number(idParam));
    }
  }

  private cargarCliente(id: number): void {
    this.loading.set(true);
    this.service.getById(id).subscribe({
      next: c => {
        this.form.patchValue({
          tipoDocumento: c.tipoDocumento,
          numeroDocumento: c.numeroDocumento,
          nombres: c.nombres,
          apellidos: c.apellidos,
          fechaNacimiento: c.fechaNacimiento,
          genero: c.genero,
          telefono: c.telefono,
          correoElectronico: c.correoElectronico,
          direccion: c.direccion,
          ciudad: c.ciudad,
          estado: c.estado
        });
        this.loading.set(false);
      },
      error: () => {
        this.serverError.set('No se encontró el cliente solicitado.');
        this.loading.set(false);
      }
    });
  }

  guardar(): void {
    this.serverError.set(null);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const payload = this.form.getRawValue() as unknown as ClienteRequest;

    const request$ = this.isEdit()
      ? this.service.update(this.clienteId()!, payload)
      : this.service.create(payload);

    request$.subscribe({
      next: () => this.router.navigate(['/clientes']),
      error: (err: HttpErrorResponse) => {
        this.saving.set(false);
        this.serverError.set(this.mapError(err));
      }
    });
  }

  /** Traduce errores HTTP a mensajes legibles (409 conflicto, 400 validación). */
  private mapError(err: HttpErrorResponse): string {
    if (err.status === 409) {
      return err.error?.detail ?? 'El cliente ya existe.';
    }
    if (err.status === 400 && err.error?.errors) {
      const msgs = Object.values(err.error.errors as Record<string, string[]>).flat();
      return msgs.join(' ');
    }
    if (err.status === 0) {
      return 'No se pudo conectar con la API. Verifica que esté en ejecución.';
    }
    return 'Ocurrió un error al guardar el cliente.';
  }

  // Helpers de plantilla para mostrar errores por campo.
  protected invalido(campo: string): boolean {
    const c = this.form.get(campo);
    return !!c && c.invalid && (c.touched || c.dirty);
  }
}
