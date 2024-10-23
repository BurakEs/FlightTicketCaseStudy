import { FlightDetail } from "./flightDetail";

export interface FlightSearchResponse {
    departureAirportDetails: FlightDetail;
    returnAirportDetails?: FlightDetail; // Opsiyonel
  }
  