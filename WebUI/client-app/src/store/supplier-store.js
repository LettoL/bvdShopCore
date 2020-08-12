import {Constants} from "../const";
import {createEffect, createStore} from "effector";
import { setError } from "../shared/store/error-store";

const API_URL = Constants.API
const API_SUPPLIERS = API_URL + 'api/suppliers'

export const fetchSuppliersFx = createEffect({
  async handler() {
    const res = await fetch(API_SUPPLIERS)
    return res.json()
  }
})

fetchSuppliersFx.finally.watch(data => {
  if(data.error)
    setError(`При загрузке списка поставщиков, произошла ошибка: ${data.error}`)
})

export const createSupplierFx = createEffect({
  async handler(name, phone, email) {
    const data = {name, phone, email}

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

createSupplierFx.finally.watch(data => {
  if(data.error)
    setError(`При создании поставщика, произошла ошибка: ${data.error}`)
})

export const suppliers = createStore([])
  .on(fetchSuppliersFx.doneData, (state, suppliers) => [...suppliers])
  .on(createSupplierFx.doneData, (state, supplier) => [...state, supplier])