import {Constants} from "../const";
import {createEffect, createStore} from "effector";
import { setError, clearError } from "../shared/store/error-store";

const API_URL = Constants.API
const API_CATEGORIES = API_URL + 'api/categories'

export const fetchCategoriesFx = createEffect({
  async handler() {
    const res = await fetch(API_CATEGORIES)
    return res.json()
  }
})

fetchCategoriesFx.finally.watch(data => {
  if(data.error)
    setError(`При загрузке списка категорий, произошла ошибка: ${data.error}`)
  else 
    clearError()
})

export const createCategoryFx = createEffect({
  async handler(title) {
    const data = {title}

    const res = await fetch(API_CATEGORIES, {
      headers: {
        'Content-Type': 'application/json'
      },
      method: 'POST',
      body: JSON.stringify(data)
    })

    return res.json()
  }
})

createCategoryFx.finally.watch(data => {
  if(data.error)
    setError(`При создании категории, произошла ошибка: ${data.error}`)
})

export const categories = createStore([])
  .on(fetchCategoriesFx.doneData, (state, categories) => [...categories])
  .on(createCategoryFx.doneData, (state, category) => [...state, category])