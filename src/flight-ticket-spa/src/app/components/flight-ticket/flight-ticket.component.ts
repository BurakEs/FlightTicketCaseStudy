import { Component, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { AirportService } from '../../services/airport.service';
import { Airport } from '../../models/airport';
import { FlightSearchService } from '../../services/flight-search.service';
import { FlightSearchRequest } from '../../models/flightSearchRequest ';

@Component({
  selector: 'app-flight-ticket',
  templateUrl: './flight-ticket.component.html',
  styleUrls: ['./flight-ticket.component.css']
})
export class FlightTicketComponent {

  departureTerm: string = '';
  departureAirport!: Airport;
  destinationTerm: string = '';
  destinationAirport!: Airport;

  departureAirports: Airport[] = [];
  destinationAirports: Airport[] = [];
  
  filteredDepartureAirports: Airport[] = [];
  filteredDestinationAirports: Airport[] = [];
  isDepartureDropdownVisible: boolean = false;
  isDestinationDropdownVisible: boolean = false;
  departureDate: string = '';
  returnDate: string = '';
  isRoundTrip: boolean = false;

  private departureSearchTerms = new Subject<string>();
  private destinationSearchTerms = new Subject<string>();

  private lastDepartureTerm: string = '';
  private lastDestinationTerm: string = '';

  constructor(private airportService: AirportService,private flightSearchService:FlightSearchService) {
    // Kalkış yeri için debounce işlemi
    this.departureSearchTerms.pipe(
      debounceTime(1000),
      switchMap(term => {
        if (term.length > 4) {
          this.lastDepartureTerm = term;
          return this.airportService.getAirportSearch(term);
        } else {
          return []; 
        }
      })
    ).subscribe(response => {
      this.departureAirports = response.data || [];
      // Sonuçlar boşsa filtreleme yapma
      if (this.departureAirports.length > 0) {
        this.filteredDepartureAirports = this.departureAirports; // İlk arama sonuçlarını sakla
      } else {
        this.filteredDepartureAirports = [];
      }
      this.isDepartureDropdownVisible = this.filteredDepartureAirports.length > 0;
    });

    // Varış yeri için debounce işlemi
    this.destinationSearchTerms.pipe(
      debounceTime(1000),
      switchMap(term => {
        if (term.length > 4) {
          this.lastDestinationTerm = term;
          return this.airportService.getAirportSearch(term);
        } else {
          return []; 
        }
      })
    ).subscribe(response => {
      this.destinationAirports = response.data || [];
      // Sonuçlar boşsa filtreleme yapma
      if (this.destinationAirports.length > 0) {
        this.filteredDestinationAirports = this.destinationAirports; // İlk arama sonuçlarını sakla
      } else {
        this.filteredDestinationAirports = [];
      }
      this.isDestinationDropdownVisible = this.filteredDestinationAirports.length > 0;
    });
  }

  onDepartureChange(): void {
    if (this.departureTerm.trim() === '') {
      // Input temizlendiğinde dropdown'ı kapat ve sonuçları boşalt
      this.isDepartureDropdownVisible = false;
      this.filteredDepartureAirports = [];
    } else if (this.departureTerm.startsWith(this.lastDepartureTerm)  && this.departureAirports.length > 0) {
      // Kullanıcının girdiği terim son arama terimiyle aynıysa ve sonuçlar boş değilse filtreleme yap
      this.filteredDepartureAirports = this.departureAirports.filter(airport =>
        airport.name.toLowerCase().includes(this.departureTerm.toLowerCase())
      );
      this.isDepartureDropdownVisible = this.filteredDepartureAirports.length > 0;
    } else {
      this.departureSearchTerms.next(this.departureTerm); // Yeni terimi gönder
    }
  }

  onDestinationChange(): void {
    if (this.destinationTerm.trim() === '') {
      // Input temizlendiğinde dropdown'ı kapat ve sonuçları boşalt
      this.isDestinationDropdownVisible = false;
      this.filteredDestinationAirports = [];
    } else if (this.destinationTerm.startsWith(this.lastDestinationTerm ) && this.destinationAirports.length > 0) {
      // Kullanıcının girdiği terim son arama terimiyle aynıysa ve sonuçlar boş değilse filtreleme yap
      this.filteredDestinationAirports = this.destinationAirports.filter(airport =>
        airport.name.toLowerCase().includes(this.destinationTerm.toLowerCase())
      );
      this.isDestinationDropdownVisible = this.filteredDestinationAirports.length > 0;
    } else {
      this.destinationSearchTerms.next(this.destinationTerm); // Yeni terimi gönder
    }
  }

  selectDeparture(airport: Airport): void {
    this.departureTerm = airport.name;
    this.departureAirport = airport;
    this.isDepartureDropdownVisible = false;
  }

  selectDestination(airport: Airport): void {
    this.destinationTerm = airport.name;
    this.destinationAirport = airport;
    this.isDestinationDropdownVisible = false;
  }


  onSubmit(): void {
    this.flightSearchService.resetResults();
    const flightSearchReq: FlightSearchRequest = {
      originAirport: this.departureAirport, // Seçilen kalkış yeri
      destinationAirport: this.destinationAirport, // Seçilen varış yeri
      isRoundTrip: this.isRoundTrip ? true : false, // Tek yön mü gidiş-dönüş mü
      departureDate: new Date(this.departureDate), // Kalkış tarihi
      returnDate: this.isRoundTrip ? new Date(this.returnDate) : undefined // Gidiş-dönüşse dönüş tarihini ekle
    };
    this.flightSearchService.getFlightSearch(flightSearchReq).subscribe(response => {
      if (response.success) {
        this.flightSearchService.setFlightSearchResult(response.data);
      }
    });
  }
}
