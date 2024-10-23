export interface Airport {
    id: number;
    ident: string;
    type: string;
    name: string;
    latitudeDeg: number;
    longitudeDeg: number;
    elevationFt?: number;
    continent?: string;
    isoCountry?: string;
    isoRegion?: string;
    municipality?: string;
    scheduledService?: string;
    gpsCode?: string;
    iataCode?: string;
    localCode?: string;
    homeLink?: string;
    wikipediaLink?: string;
    keywords?: string;
}
