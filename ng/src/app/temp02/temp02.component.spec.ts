import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Temp02Component } from './temp02.component';

describe('Temp02Component', () => {
  let component: Temp02Component;
  let fixture: ComponentFixture<Temp02Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Temp02Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Temp02Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
