import {createEffect, createStore, createEvent, sample, combine} from "effector";
import { Constants } from "../../../const";
import { setError } from "../../../shared/store/error-store";

const API_URL = Constants.API
const API_SALE_CREATE = API_URL + 'не забудь вписать url'

export const addProductToSale = createEvent()
export const removeProductFromSale = createEvent()
export const changeProductPrice = createEvent()
export const changeProductAmount = createEvent()

export const activateCashPayment = createEvent()
export const deactivateCashPayment = createEvent()
export const changeCashSum = createEvent()

export const activateCashlessPayment = createEvent()
export const deactivateCashlessPayment = createEvent()
export const changeCashlessSum = createEvent()
export const selectMoneyWorkerId = createEvent()

export const selectManagerId = createEvent()
export const changeDiscount = createEvent()
export const changeDeferred = createEvent()
export const changeForRussia = createEvent()

export const addError = createEvent()
export const removeError = createEvent()

export const updateSaleInfo = createEvent();
export const resetSelectedMoneyWorker = createEvent();
export const changeSaleCost = createEvent();

const notEnoughMoney = 'Не достаточно средств внесено'
const notSelectedManager = 'Не выбран менеджер'
const incorrectProductPrice = 'Некорректная стоимость товара'


export const $saleInfo = createStore({
  managerId: 0,
  discount: 0,
  deferred: false,
  forRussia: false,
})
  .on(selectManagerId,
    (state, id) => ({...state, managerId: id}))
  .on(changeDiscount,
    (state, discount) => ({...state, discount: discount}))
  .on(changeDeferred,
    (state, value) => ({...state, deferred: value}))
  .on(changeForRussia,
    (state, value) => ({...state, forRussia: value}))

export const $saleProducts = createStore([])
  .on(addProductToSale, (state, product) => {
    const index = state.findIndex(x => x.id === product.id)
    if(index !== (-1)) {
      const curProduct = state.find(x => x.id === product.id)

      return [
        ...state.slice(0, index),
        {...curProduct, amount: curProduct.amount + 1},
        ...state.slice(index + 1)
      ]
    }
    else return [...state, {...product, amount: 1}]
  })
  .on(removeProductFromSale,
    (state, product) => state.filter(x => x.id !== product.id))
  .on(changeProductPrice,(state, product) => {
    const index = state.findIndex(x => x.id === product.id)
    const curProduct = state.find(x => x.id === product.id)

    return [
      ...state.slice(0, index),
      {...curProduct, price: product.price},
      ...state.slice(index + 1)
    ]
  })
  .on(changeProductAmount, (state, product) => {
    const index = state.findIndex(x => x.id === product.id)
    const curProduct = state.find(x => x.id === product.id)

    return [
      ...state.slice(0, index),
      {...curProduct, amount: product.amount},
      ...state.slice(index + 1)
    ]
  })

export const $cost = sample({
  source: $saleProducts,
  fn: (products) => (
    products.length > 0
      ? products.map(x => x.price * x.amount).reduce((acc, value) => (acc + value))
      : 0
  )
})

export const $cashPayment = createStore({
  active: false,
  sum: 0
})
  .on(activateCashPayment,
    (state, _) => ({...state, active: true}))
  .on(deactivateCashPayment,
    (state, _) => ({...state, active: false}))
  .on(changeCashSum,
    (state, sum) => ({...state, sum: sum}))

export const $cashlessPayment = createStore({
  active: false,
  sum: 0,
  moneyWorkerId: 0
})
  .on(activateCashlessPayment,
    (state, _) => ({...state, active: true}))
  .on(deactivateCashlessPayment,
    (state, _) => ({...state, active: false}))
  .on(changeCashlessSum,
    (state, sum) => ({...state, sum: sum}))
  .on(selectMoneyWorkerId,
    (state, id) => ({...state, moneyWorkerId: id}))

export const $erIncorrectProductPrice = sample({
  source: $saleProducts,
  fn: (products) => (products.length > 0 && products.map(x => +(x.price)).some(x => x <= 0))
})

export const $errors = combine(
  $saleProducts,
  $cashPayment,
  $cashlessPayment,
  $saleInfo,
  $cost,
  (saleProducts, cash, cashless, sale, cost) => {
    let result = []

    if(saleProducts.length > 0 && saleProducts.map(x => +(x.price)).some(x => x <= 0))
      result.push(incorrectProductPrice)

    if(sale.managerId === 0)
      result.push(notSelectedManager)

    if(((cash.active ? +cash.sum : 0) + (cashless.active ? +cashless.sum : 0)) < (cost - sale.discount))
      result.push(notEnoughMoney)

    return result
  }
)

$saleInfo
  .on(addProductToSale,
    (state, product) => ({...state, cost: state.cost + product.price}))
  .on(removeProductFromSale,
    (state, product) => ({...state, cost: state.cost - product.price}))
  .on(updateSaleInfo, (state, event) => {
    return {
      ...state,
      [event.name]: event.value
    }
  })
  .on(resetSelectedMoneyWorker, (state) => {
    return {
      ...state,
      moneyWorkerId: 0
    }
  })


export const fxCreateSale = createEffect(
  {
    async handler(sale) {
      const data = sale

      const res = await fetch(API_SALE_CREATE, {
        headers: {
          'Content-Type': 'application/json'
        },
        method: 'POST',
        body: JSON.stringify(data)
      })

      return res.json()
    }
  }
)

fxCreateSale.finally.watch(data => {
  if(data.error)
    setError(`При создании продажи, произошла ошибка: ${data.error}`)
})
