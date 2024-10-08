import { Component, inject, OnInit } from '@angular/core';
import { CartService } from '../../core/services/cart.service';
import { CartItemComponent } from "./cart-item/cart-item.component";
import { OrderSummaryComponent } from "../../shared/components/order-summary/order-summary.component";
import { CartItem } from '../../shared/models/cart';
import { CommonModule } from '@angular/common';
import { InitService } from '../../core/services/init.service';
import { EmptyStateComponent } from "../../shared/components/empty-state/empty-state.component";






@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CartItemComponent,
    OrderSummaryComponent,
    CommonModule,
    EmptyStateComponent
],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent  {

  cartService = inject(CartService);
 




}
