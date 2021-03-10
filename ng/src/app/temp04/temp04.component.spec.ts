import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Temp04Component } from './temp04.component';

describe('Temp04Component', () => {
  let component: Temp04Component;
  let fixture: ComponentFixture<Temp04Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Temp04Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Temp04Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
