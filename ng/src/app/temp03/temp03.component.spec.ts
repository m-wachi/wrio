import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Temp03Component } from './temp03.component';

describe('Temp03Component', () => {
  let component: Temp03Component;
  let fixture: ComponentFixture<Temp03Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Temp03Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Temp03Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
