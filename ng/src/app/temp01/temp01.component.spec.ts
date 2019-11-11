import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Temp01Component } from './temp01.component';

describe('Temp01Component', () => {
  let component: Temp01Component;
  let fixture: ComponentFixture<Temp01Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Temp01Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Temp01Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
