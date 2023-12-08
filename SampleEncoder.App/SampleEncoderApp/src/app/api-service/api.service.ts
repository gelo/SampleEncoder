import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

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
    return this.http.post<EncodeResponse>(url, body).pipe(
      catchError(this.handleError)
    );
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
    return this.http.post(url, {}).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<any> {
    let errorMessage = 'An error occurred.';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
