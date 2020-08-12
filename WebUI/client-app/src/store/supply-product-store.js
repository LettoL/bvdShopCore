import {Constants} from "../const";
import {createEffect, createStore} from "effector";
import { setError } from "../shared/store/error-store";

const API_URL = Constants.API
const API_SUPPLYPRODUCTS = API_URL + 'api/supplyProducts'

export const fetchSupplyProductsFx = createEffect({
  async handler() {
    const res = await fetch(API_SUPPLYPRODUCTS)
    return res.json()
  }
})

fetchSupplyProductsFx.finally.watch(data => {
  if(data.error)
    setError(`При загрузке списка поставок, произошла ошибка: ${data.error}`)
})

export const supplyProducts = createStore([])
  .on(fetchSupplyProductsFx.doneData, (state, supplyProducts) => [...supplyProducts])