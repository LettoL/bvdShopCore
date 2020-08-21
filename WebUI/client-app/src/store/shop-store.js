import {createEffect, createStore} from "effector";
import {Constants} from "../const";
import {setError, clearError} from "../shared/store/error-store"

const API_URL = Constants.API
const API_SHOPS = API_URL + 'api/shops'

export const fetchShopsFx = createEffect({
  async handler() {
    const res = await fetch(API_SHOPS)
    return res.json()
  }
})

fetchShopsFx.finally.watch(data => {
  if(data.error)
    setError(`При загрузке списка магазинов, произошла ошибка: ${data.error}`)
})

export const saveShopFx = createEffect({
  async handler(title) {
    const data = {title}

    const res = await fetch(API_SHOPS, {
      headers: {
        'Content-Type': 'application/json'
      },
      method: 'POST',
      body: JSON.stringify(data)
    })

    return res.json()
  }
})

saveShopFx.finally.watch(data => {
  if(data.error)
    setError(`При сохранении магазина, произошла ошибка: ${data.error}`)
  else 
    clearError()
})

export const shops = createStore([])
  .on(fetchShopsFx.doneData, (state, shops) => [...shops])
  .on(saveShopFx.doneData, (state, shop) => [...state, shop])