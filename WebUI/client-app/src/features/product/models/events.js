import {combine, createEffect, createEvent, createStore} from 'effector'

export const productSearch = createEvent()
export const changeTab = createEvent()
export const changeCurrentProduct = createEvent()


export const fxFetchProducts = createEffect()


export const $products = createStore([])

export const $filterProductName = createStore('')
  .on(productSearch, (_, productName) => productName)

export const $currentProduct = createStore(null)
  .on(changeCurrentProduct, (_, product) => product)

export const $currentTab = createStore(1)
  .on(changeTab, (_, number) => number)


export const $productNames = combine(
  $products,
  $filterProductName,
  (products, productName) => products
    .filter(product => productName !== ''
      ? product.title.toLowerCase().includes(productName.toLowerCase())
      : true)
)
