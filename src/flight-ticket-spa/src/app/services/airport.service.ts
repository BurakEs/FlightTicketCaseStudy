import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Airport } from '../models/airport';
import { ListResponseModel } from '../models/listResponseModel';
import { Observable, catchError, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr'; // Toastr kütüphanesini ekleyin

@Injectable({
  providedIn: 'root'
})
export class AirportService {

  constructor(private httpClient: HttpClient, private toastr: ToastrService) { } // ToastrService'i ekleyin
  apiUrl: string = "https://localhost:7000/api/Airport"; 

  getAirportSearch(searchTerm: string): Observable<ListResponseModel<Airport>> {
    const newPath = `${this.apiUrl}/search?searchTerm=${searchTerm}`;
    return this.httpClient.get<ListResponseModel<Airport>>(newPath).pipe(
      catchError(this.handleError.bind(this)) // Hata durumunu yakalamak için handleError metodunu ekleyin
    );
  }

  // Hata yakalama metodu
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Bilinmeyen bir hata oluştu!';
    
    if (error.error instanceof ErrorEvent) {
      // İstemci tarafı hatası
      errorMessage = `Hata: ${error.error.message}`;
    } else {
      // Sunucu tarafı hatası
      errorMessage = `Hata Kodu: ${error.status}, Hata Mesajı: ${error.message}`;
    }

    this.toastr.error(errorMessage, 'Hata'); // Toastr ile hata mesajını gösterin
    return throwError(() => new Error(errorMessage)); // Hata mesajını döndürün
  }
}
