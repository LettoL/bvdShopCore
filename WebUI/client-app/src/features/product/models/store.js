import {combine, createEffect, createStore} from "effector";
import {Constants} from "../../../const";

const API_PRODUCTS = Constants.API + 'api/products'

export const fxFetchProducts = createEffect()

export const $products = createStore([])
  .on(fxFetchProducts.doneData, (_, data) => data)

export const $productsInStock = combine(
  $products,
  (products) => products
    .filter(x => x.stockAmount > 0)
)

fxFetchProducts.use(async () => {
  const res = await fetch(API_PRODUCTS)
  return res.json()
})

fxFetchProducts.doneData.watch(x => console.log(x))

/*fxFetchProducts.use(async () => {
  return [
    {id: '1231', title: 'Product1', price: 2500, shopId: '122', stockAmount: 10, incompleteAmount: 2, bookingAmount: 1},
    {id: '1232', title: 'Product2', price: 25000, shopId: '122', stockAmount: 0, incompleteAmount: 0, bookingAmount: 0},
    {id: '1233', title: 'Product3', price: 1000, shopId: '122', stockAmount: 10, incompleteAmount: 2, bookingAmount: 1},
    {id: '1234', title: 'Product4', price: 1500, shopId: '120', stockAmount: 10, incompleteAmount: 2, bookingAmount: 1},
    {id: '1235', title: 'Product5', price: 1800, shopId: '122', stockAmount: 0, incompleteAmount: 0, bookingAmount: 0},
    {id: '1236', title: 'Product6', price: 20000, shopId: '120', stockAmount: 10, incompleteAmount: 2, bookingAmount: 1},
    {id: '1237', title: 'Product7', price: 12000, shopId: '120', stockAmount: 10, incompleteAmount: 2, bookingAmount: 1}
  ]
})*/