import { Airport } from "./airport";
import { FlightOption } from "./flightOption";

export interface FlightDetail {
    originAirport: Airport; // Airport arayüzünü daha önce tanımlamıştık
    destinationAirport: Airport; // Airport arayüzünü daha önce tanımlamıştık
    flightOptions: FlightOption[]; // FlightOption arayüzünü tanımlayacağız
  }
  