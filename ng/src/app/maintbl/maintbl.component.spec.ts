import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaintblComponent } from './maintbl.component';

describe('MaintblComponent', () => {
  let component: MaintblComponent;
  let fixture: ComponentFixture<MaintblComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaintblComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaintblComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
