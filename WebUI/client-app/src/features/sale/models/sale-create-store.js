import { createEffect, createStore, createEvent } from "effector";
import { Constants } from "../../../const";
import { setError } from "../../../shared/store/error-store";

const API_URL = Constants.API
const API_SALE_CREATE = API_URL + 'не забудь вписать url'

export const addProductToSale = createEvent();
export const removeProductFromSale = createEvent();
export const updateSaleCreateState = createEvent();
export const resetSelectedMoneyWorker = createEvent();
export const changeSaleCost = createEvent();


export const createSaleFx = createEffect(
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

createSaleFx.finally.watch(data => {
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
  cashlessSum: 0,
  saleProducts: []
});

$saleCreate
  .on(addProductToSale, (state, product) => {
    return {
      ...state,
      saleProducts: [...state.saleProducts, product]
    }
  })
  .on(removeProductFromSale, (state, product) => {

    const index = state.saleProducts.findIndex(item => item.id === product.id);

    return {
      ...state,
      saleProducts: [
        ...state.saleProducts.slice(0, index),
        ...state.saleProducts.slice(index + 1)
      ]
    }
  })
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
  .on(changeSaleCost, (state, cost) => {
    return {
      ...state,
      cost: state.cost + cost
    }
  })
