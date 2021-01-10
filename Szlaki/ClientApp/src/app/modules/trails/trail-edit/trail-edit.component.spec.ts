import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrailEditComponent } from './trail-edit.component';

describe('TrailEditComponent', () => {
  let component: TrailEditComponent;
  let fixture: ComponentFixture<TrailEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrailEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TrailEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
