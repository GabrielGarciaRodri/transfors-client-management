// Enums alineados con el backend (se serializan como texto).
export enum TipoDocumento {
  CedulaCiudadania = 'CedulaCiudadania',
  CedulaExtranjeria = 'CedulaExtranjeria',
  Pasaporte = 'Pasaporte',
  TarjetaIdentidad = 'TarjetaIdentidad',
  Nit = 'Nit'
}

export enum Genero {
  Masculino = 'Masculino',
  Femenino = 'Femenino',
  Otro = 'Otro',
  PrefieroNoDecir = 'PrefieroNoDecir'
}

/** Respuesta que devuelve la API. */
export interface Cliente {
  id: number;
  tipoDocumento: TipoDocumento;
  tipoDocumentoDescripcion: string;
  numeroDocumento: string;
  nombres: string;
  apellidos: string;
  fechaNacimiento: string; // ISO yyyy-MM-dd
  genero: Genero;
  generoDescripcion: string;
  telefono: string;
  correoElectronico: string;
  direccion: string;
  ciudad: string;
  estado: boolean;
  fechaCreacion: string;
  fechaModificacion: string | null;
}

/** Payload para crear/editar (coincide con ClienteRequest del backend). */
export interface ClienteRequest {
  tipoDocumento: TipoDocumento;
  numeroDocumento: string;
  nombres: string;
  apellidos: string;
  fechaNacimiento: string;
  genero: Genero;
  telefono: string;
  correoElectronico: string;
  direccion: string;
  ciudad: string;
  estado: boolean;
}

// Opciones para selects de la UI.
export const TIPOS_DOCUMENTO: { value: TipoDocumento; label: string }[] = [
  { value: TipoDocumento.CedulaCiudadania, label: 'Cédula de ciudadanía' },
  { value: TipoDocumento.CedulaExtranjeria, label: 'Cédula de extranjería' },
  { value: TipoDocumento.Pasaporte, label: 'Pasaporte' },
  { value: TipoDocumento.TarjetaIdentidad, label: 'Tarjeta de identidad' },
  { value: TipoDocumento.Nit, label: 'NIT' }
];

export const GENEROS: { value: Genero; label: string }[] = [
  { value: Genero.Masculino, label: 'Masculino' },
  { value: Genero.Femenino, label: 'Femenino' },
  { value: Genero.Otro, label: 'Otro' },
  { value: Genero.PrefieroNoDecir, label: 'Prefiero no decir' }
];
