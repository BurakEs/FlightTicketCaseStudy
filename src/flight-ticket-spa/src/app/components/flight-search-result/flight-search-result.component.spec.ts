import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightSearchResultComponent } from './flight-search-result.component';

describe('FlightSearchResultComponent', () => {
  let component: FlightSearchResultComponent;
  let fixture: ComponentFixture<FlightSearchResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FlightSearchResultComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FlightSearchResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
