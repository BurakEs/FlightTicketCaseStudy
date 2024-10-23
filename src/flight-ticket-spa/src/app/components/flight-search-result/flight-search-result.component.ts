import { Component } from '@angular/core';
import { FlightSearchResponse } from '../../models/flightSearchResponse ';
import { FlightSearchService } from '../../services/flight-search.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FlightOption } from '../../models/flightOption';
import { FlightDetail } from '../../models/flightDetail';

@Component({
  selector: 'app-flight-search-result',
  templateUrl: './flight-search-result.component.html',
  styleUrl: './flight-search-result.component.css'
})
export class FlightSearchResultComponent {
  flightSearchResult: FlightSearchResponse | null = null;

  selectedDepartureFlight: { flight: FlightOption; details: FlightDetail } | null = null;
  selectedReturnFlight: { flight?: FlightOption; details?: FlightDetail } | null = null;
  

  constructor(private flightSearchService: FlightSearchService,private modalService: NgbModal) { }

  ngOnInit(): void {
    this.flightSearchService.getFlightSearchResult().subscribe(result => {
      this.flightSearchResult = result;
    });
  }

onSelectFlight(option: FlightOption, type: string, flightDetails: FlightDetail) {
  if (type === 'departure') {
    this.selectedDepartureFlight = { flight: option, details: flightDetails };
  } else if (type === 'return') {
    this.selectedReturnFlight = { flight: option, details: flightDetails };
  }
}


  onConfirmSelection(content: any) {
    if ((this.selectedDepartureFlight && this.selectedReturnFlight) 
      || (this.selectedDepartureFlight && this.flightSearchResult?.returnAirportDetails == undefined)) {
      this.showPopup(content);  // content burada flightDetailsModal'ı temsil ediyor
    } 
    else {
      alert('Lütfen hem gidiş hem de dönüş uçuşu seçiniz.');
    }
  }

  // Modal'ı açan fonksiyon
  showPopup(content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
  }
  closePopup(){
    location.reload();
  }
  // Sonuçları sıfırlayan metot
  resetResults(): void {
    this.flightSearchResult = null;
    this.selectedDepartureFlight= null;
    this.selectedReturnFlight= null;
  }
}
