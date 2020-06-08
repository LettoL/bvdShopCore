import {Constants} from "../const";
import {createEffect, createStore} from "effector";

const API_URL = Constants.API
const API_SUPPLYPRODUCTS = API_URL + 'api/supplyProducts'

export const fetchSupplyProductsFx = createEffect({
  async handler() {
    const res = await fetch(API_SUPPLYPRODUCTS)
    return res.json()
  }
})

export const supplyProducts = createStore([])
  .on(fetchSupplyProductsFx.doneData, (state, supplyProducts) => [...supplyProducts])