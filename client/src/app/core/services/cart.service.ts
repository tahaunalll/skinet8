import { Injectable, computed, inject, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);

  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0)
  });

  selectedDelivery = signal<DeliveryMethod | null> (null);

  totals = computed(() => {
    const cart = this.cart();
    const delivery = this.selectedDelivery();
    if (!cart) return null;
    const subtotal = cart.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
    const shipping = delivery? delivery.price : 0;
    const discount = 0;
    return {
      subtotal,
      shipping,
      discount,
      total: subtotal + shipping - discount
    }
  })

  public getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map(cart => {
        this.cart.set(cart);
        return cart;
      })
    )
  }

  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: cart => this.cart.set(cart)
    })
  }

  addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCart();

    if (this.isProduct(item)) {
      console.log("product olduğu anlaşıldı")
      item = this.mapProductToCartItem(item);
    }
    console.log("maplendi")
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    this.setCart(cart);
  }

  removeItemFromCart(productId: number, quantity = 1) {
    const cart = this.cart();
    if (!cart) return;
    const index = cart.items.findIndex(x => x.productID === productId);
    if (index !== -1) {
      if (cart.items[index].quantity > quantity) {
        cart.items[index].quantity -= quantity;
      } else {
        cart.items.splice(index, 1);
      }
      if (cart.items.length === 0) {
        this.deleteCart();
      }
      else {
        this.setCart(cart);
      }
    }
  }
  deleteCart() {
    this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      }
    })
  }

  private addOrUpdateItem(items: CartItem[], item: CartItem, quantity: number): CartItem[] {

    //Hata çözümünde kullandım : 

    // // Log items and item being processed for debugging
    // console.log("Items:", items);
    // console.log("Item to Add/Update:", item);

    // // Check data types of productId
    // console.log("Type of item.productId:", typeof item.productID);
    // this.cart()?.items.forEach(existingItem => {
    //   console.log("Type of existingItem.productId:", typeof existingItem.productID);
    // });


    // if (item.productID === undefined) {
    //   console.error("Item productId is undefined");
    //   return items;
    // }

    const index = items.findIndex(existingItem => existingItem.productID == item.productID);
    console.log("Index", index)

    if (index === -1) {
      // Öğeyi bulamadık, yeni öğe ekle
      item.quantity = quantity;
      items.push(item);
    } else {
      console.log("items[index]e gir")
      // Öğeyi bulduk, miktarı güncelle

      items[index].quantity += quantity;
    }

    return items;

    //const index = items.findIndex(x=>x.productId === item.productId);
    //addOrUpdateItemdan index değeri kaç döndüğünün kontrolü için yazıldı
    //console.log("Product Index:", index, "Product ID:", item.productId);

  }

  private mapProductToCartItem(item: Product): CartItem {
    return {
      productID: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.type
    }
  }

  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }


  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }

}


