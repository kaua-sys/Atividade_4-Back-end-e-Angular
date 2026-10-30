import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GerenciarPedidosComponent } from './gerenciar-pedidos.component';

describe('GerenciarPedidosComponent', () => {
  let component: GerenciarPedidosComponent;
  let fixture: ComponentFixture<GerenciarPedidosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GerenciarPedidosComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GerenciarPedidosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
