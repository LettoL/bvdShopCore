import {Constants} from "../../const";
import {combine, createEffect, createEvent, createStore, sample} from "effector";

const API_URL = Constants.API
const API_PRODUCTS = API_URL + 'api/products'

export const setShopFilter = createEvent()
export const setCategoryFilter = createEvent()
export const setTitleFilter = createEvent()
export const setMinAmountFilter = createEvent()
export const setSupplierFilter = createEvent()

export const fetchProductsFx = createEffect({
  async handler() {
    const res = await fetch(API_PRODUCTS)
    return res.json()
  }
})

export const fetchProductsBySupplierFx = createEffect({
  async handler(id) {
    const res = await fetch(API_PRODUCTS + "/GetBySupplier/" + id)
    return res.json()
  }
})

export const $products = createStore([])
  .on(fetchProductsFx.doneData, (state, products) => [...products])
  .on(fetchProductsBySupplierFx.doneData, (state, products) => [...products])

export const $filterShopId = createStore(0)
  .on(setShopFilter, (_, shopId) => shopId)

export const $filterCategoryId = createStore(0)
  .on(setCategoryFilter, (_, categoryId) => categoryId)

export const $filterSupplierId = createStore(0)
  .on(setSupplierFilter, (_, supplierId) => supplierId)

export const $filterTitle = createStore('')
  .on(setTitleFilter, (_, title) => title)

export const $filterMinAmount = createStore(0)
  .on(setMinAmountFilter, (_, amount) => amount)

export const $filteredProducts = combine(
  $products,
  $filterShopId,
  $filterCategoryId,
  $filterTitle,
  $filterMinAmount,
  (products, shopId, categoryId, title, minAmount) => products
    .filter(product => shopId !== 0
      ? product.shopId === shopId
      : true)
    .filter(product => categoryId !== 0
      ? product.categoryId === categoryId
      : true)
    .filter(product => title !== ''
      ? product.title.toLowerCase().includes(title.toLowerCase())
      : true)
    .filter(product => product.amount >= minAmount)
)