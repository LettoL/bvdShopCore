import {createEffect, createStore} from 'effector'
import {Constants} from "../../const";

const API_URL = Constants.API
const API_INCOMPLETE_PRODUCTS = API_URL + 'api/incompleteProducts'

export const fetchIncompleteProductsFx = createEffect()
export const saveIncompleteProductFx = createEffect()

export const $incompleteProducts = createStore([])
  .on(fetchIncompleteProductsFx.doneData, (_, data) => [...data])

fetchIncompleteProductsFx.use(async () => await fetch(API_INCOMPLETE_PRODUCTS).then(req => req.json()))
saveIncompleteProductFx.use(async incompleteProduct => {
  const data = {incompleteProduct}

  const res = await fetch(API_INCOMPLETE_PRODUCTS, {
    headers: {
      'Content-Type': 'application/json'
    },
    method: 'POST',
    body: JSON.stringify(data)
  })

  return res.json()
})