import {Constants} from "../../const";
import {createEffect, createStore} from "effector";

const API_URL = Constants.API
const API_DELETED_SALES = API_URL + 'api/deletedSales'

export const fetchDeletedSalesFx = createEffect({
  async handler() {
    const res = await fetch(API_DELETED_SALES)
    return res.json()
  }
})

export const $deletedSales = createStore([])
  .on(fetchDeletedSalesFx.doneData, (state, deletedSales) => [...deletedSales])