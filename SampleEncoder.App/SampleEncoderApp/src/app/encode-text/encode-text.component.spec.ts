import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EncodeTextComponent } from './encode-text.component';

describe('EncodeTextComponent', () => {
  let component: EncodeTextComponent;
  let fixture: ComponentFixture<EncodeTextComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EncodeTextComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EncodeTextComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
