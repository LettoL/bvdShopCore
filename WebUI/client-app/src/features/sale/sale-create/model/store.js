import {combine, createEvent, createStore} from "effector";
import {$productsInStock} from "../../../product/models/store";

export const changeFilterProductTitle = createEvent()
export const selectProductCategory = createEvent()

export const $availableProducts = combine(
  $productsInStock,
  (products) => products
    .map(x => ({...x, availableAmount: x.stockAmount - x.incompleteAmount - x.bookingAmount}))
    .filter(x => x.availableAmount > 0)
)

export const $filterProductTitle = createStore('')
  .on(changeFilterProductTitle, (_, value) => value)

export const $filterProductCategory = createStore(0)
  .on(selectProductCategory, (_, value) => value)

export const $filteredProducts = combine(
  $availableProducts,
  $filterProductTitle,
  $filterProductCategory,
  (products, title, category) => products
    .filter(x => title !== ''
      ? x.title.toLowerCase().includes(title.toLowerCase())
      : true)
    .filter(x => category !== 0
      ? x.categoryId === category
      : true)
)