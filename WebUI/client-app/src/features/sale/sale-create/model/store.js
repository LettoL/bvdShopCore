import {combine, createEffect, createEvent, createStore} from "effector";
import {Constants} from "../../../../const";

export const changeFilterProductTitle = createEvent()
export const selectProductCategory = createEvent()

export const fxFetchAvailableProducts = createEffect()

export const $availableProducts = createStore([])
  .on(fxFetchAvailableProducts.doneData,
    (_, data) => [...data.map(x => ({...x, availableAmount: x.stockAmount, id: x.productId}))])
/*combine(
  $productsInStock,
  (products) => products
    .map(x => ({...x, availableAmount: x.stockAmount - x.incompleteAmount - x.bookingAmount}))
    .filter(x => x.availableAmount > 0)
)*/

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
    .filter(x => +(category) !== 0
      ? x.categoryId === +(category)
      : true)
)

const API_AVAILABLE_PRODUCTS = Constants.API + 'api/products/availableForSaleOld'

fxFetchAvailableProducts.use(async () => {
  const res = await fetch(API_AVAILABLE_PRODUCTS)
  return res.json()
})