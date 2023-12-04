// src/app/api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface EncodeResponse {
  Base64Result: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7040/base64';

  constructor(private http: HttpClient) {}

  encodeText(text: string): Observable<EncodeResponse> {
    const url = `${this.apiUrl}`;
    const body = { Text: text }; 
    return this.http.post<EncodeResponse>(url, body);
  }

  getStream(): Observable<any> {
    const url = `${this.apiUrl}/stream`;
    return new Observable(observer => {
      const eventSource = new EventSource(url);

      eventSource.onmessage = (event: any) => {
        const responseItem = JSON.parse(event.data);
        observer.next(responseItem);
      };

      eventSource.onerror = (error) => {
        observer.error('Error occurred in stream.');
        eventSource.close();
      };

      return () => {
        eventSource.close();
      };
    });
  }

  cancelEncoding(): Observable<any> {
    const url = `${this.apiUrl}/cancel`;
    return this.http.post(url, {});
  }
}
