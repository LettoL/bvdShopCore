import {createEffect, createStore, createEvent, forward} from "effector";
import { Constants } from "../../../const";
import { setError } from "../../../shared/store/error-store";

const API_URL = Constants.API
const API_SALE_CREATE = API_URL + 'не забудь вписать url'

export const addProductToSale = createEvent();
export const removeProductFromSale = createEvent();
export const updateSaleCreateState = createEvent();
export const resetSelectedMoneyWorker = createEvent();
export const changeSaleCost = createEvent();


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

export const $saleCreate = createStore({
  managerId: 0,
  discount: 0,
  cost: 0,
  deferred: false,
  forRussia: false,
  cashSum: 0,
  moneyWorkerId: 0,
  cashlessSum: 0
});

export const $saleProducts = createStore([])
  .on(addProductToSale,
    (state, product) => [...state, product])
  .on(removeProductFromSale,
    (state, product) => state.filter(x => x.id !== product.id))

export const $cashPayment = createStore({
  active: false,
  sum: 0
})

export const $cashlessPayment = createStore({
  active: false,
  sum: 0,
  moneyWorkerId: 0
})

$saleCreate
  .on(addProductToSale,
    (state, product) => ({...state, cost: state.cost + product.price}))
  .on(removeProductFromSale,
    (state, product) => ({...state, cost: state.cost - product.price}))
  .on(updateSaleCreateState, (state, event) => {
    return {
      ...state,
      [event.target.name]: event.target.value
    }
  })
  .on(resetSelectedMoneyWorker, (state) => {
    return {
      ...state,
      moneyWorkerId: 0
    }
  })
