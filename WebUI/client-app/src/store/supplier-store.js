import {Constants} from "../const";
import {createEffect, createStore} from "effector";

const API_URL = Constants.API
const API_SUPPLIERS = API_URL + 'api/suppliers'

export const fetchSuppliersFx = createEffect({
  async handler() {
    const res = await fetch(API_SUPPLIERS)
    return res.json()
  }
})

export const createSupplierFx = createEffect({
  async handler(name) {
    const data = {name}

    const res = await fetch(API_SUPPLIERS, {
      headers: {
        'Content-Type': 'application/json'
      },
      method: 'POST',
      body: JSON.stringify(data)
    })

    return res.json()
  }
})

export const suppliers = createStore([])
  .on(fetchSuppliersFx.doneData, (state, suppliers) => [...suppliers])
  .on(createSupplierFx.doneData, (state, supplier) => [...state, supplier])