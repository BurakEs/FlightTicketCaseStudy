import { Airport } from "./airport";

export interface FlightSearchRequest {
    originAirport: Airport;
    destinationAirport: Airport;
    isRoundTrip: boolean;
    departureDate?: Date; // Veya string, tarihi string formatında kullanıyorsanız
    returnDate?: Date; // Opsiyonel, boş olabilir
  }