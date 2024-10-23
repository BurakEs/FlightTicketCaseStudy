import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SingleResponseModel } from '../models/singleResponseModel';
import { BehaviorSubject, catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr'; 
import { FlightSearchResponse } from '../models/flightSearchResponse ';
import { FlightSearchRequest } from '../models/flightSearchRequest ';
import { ResponseModel } from '../models/responseModel';

@Injectable({
  providedIn: 'root'
})
export class FlightSearchService {
  
  private flightSearchResult = new BehaviorSubject<FlightSearchResponse | null>(null);

  constructor(private httpClient: HttpClient, private toastr: ToastrService) { } // ToastrService'i ekleyin
  apiUrl: string = "https://localhost:7000/api/Flight";

  getFlightSearch(flightSearchReq: FlightSearchRequest): Observable<SingleResponseModel<FlightSearchResponse>> {
    const newPath = `${this.apiUrl}/search`; // Template literal kullanarak daha okunaklı hale getirin
    console.log(flightSearchReq);
    return this.httpClient.post<SingleResponseModel<FlightSearchResponse>>(newPath, flightSearchReq)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          // Hata durumunu kontrol et
          if (error.error && error.error.Message) {
            // Hata gövdesinde Message alanını kontrol et
            const message = error.error.Message; // Hata mesajını al
            this.showToaster(message); // Hata mesajını göster
          } else {
            this.showToaster('Beklenmeyen bir hata oluştu.'); // Genel hata mesajı
          }
          return throwError(error); // Hata nesnesini yeniden fırlat
        })
      );
}


// Toaster mesajı gösteren bir fonksiyon
private showToaster(message: string): void {
    // Burada toast bildirimlerini göstermek için kullandığınız kütüphaneye bağlı olarak düzenleme yapın
    this.toastr.error(message); // Örneğin, ngx-toastr kullanıyorsanız
}

  // Sonucu diğer komponentlere yaymak için metot
  setFlightSearchResult(result: FlightSearchResponse): void {
    this.flightSearchResult.next(result);
  }

  // Diğer komponentlerin dinleyeceği metot
  getFlightSearchResult(): Observable<FlightSearchResponse | null> {
    return this.flightSearchResult.asObservable();
  }
  resetResults(): void {
    this.flightSearchResult.next(null); // Sonuçları sıfırlayın
  }
}
