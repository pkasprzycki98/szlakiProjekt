import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrailViewComponent } from './trail-view.component';

describe('TrailViewComponent', () => {
  let component: TrailViewComponent;
  let fixture: ComponentFixture<TrailViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrailViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TrailViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
