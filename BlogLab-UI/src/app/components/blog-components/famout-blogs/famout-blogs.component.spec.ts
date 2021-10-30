import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FamoutBlogsComponent } from './famout-blogs.component';

describe('FamoutBlogsComponent', () => {
  let component: FamoutBlogsComponent;
  let fixture: ComponentFixture<FamoutBlogsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FamoutBlogsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FamoutBlogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
