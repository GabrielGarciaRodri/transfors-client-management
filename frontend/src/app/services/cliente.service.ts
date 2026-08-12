import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Cliente, ClienteRequest } from '../models/cliente.model';

@Injectable({ providedIn: 'root' })
export class ClienteService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/clientes`;

  /** Lista clientes con búsqueda de texto y filtro de estado opcionales. */
  getAll(search?: string, estado?: boolean | null): Observable<Cliente[]> {
    let params = new HttpParams();
    if (search && search.trim()) {
      params = params.set('search', search.trim());
    }
    if (estado !== null && estado !== undefined) {
      params = params.set('estado', String(estado));
    }
    return this.http.get<Cliente[]>(this.baseUrl, { params });
  }

  getById(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.baseUrl}/${id}`);
  }

  create(payload: ClienteRequest): Observable<Cliente> {
    return this.http.post<Cliente>(this.baseUrl, payload);
  }

  update(id: number, payload: ClienteRequest): Observable<Cliente> {
    return this.http.put<Cliente>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
