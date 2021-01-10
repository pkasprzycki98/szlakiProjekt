import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrailAddComponent } from './trail-add.component';

describe('TrailAddComponent', () => {
  let component: TrailAddComponent;
  let fixture: ComponentFixture<TrailAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrailAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TrailAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
