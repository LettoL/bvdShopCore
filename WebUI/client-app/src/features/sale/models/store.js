import {combine} from "effector";
import {$productsInStock} from "../../product/models/store";

export const $availableProducts = combine(
  $productsInStock,
  (products) => products
    .map(x => ({...x, availableAmount: x.stockAmount - x.incompleteAmount - x.bookingAmount}))
    .filter(x => x.availableAmount > 0)
)