import {Constants} from "../const";
import {createEffect, createStore} from "effector";

const API_URL = Constants.API
const API_CATEGORIES = API_URL + 'api/categories'

export const fetchCategoriesFx = createEffect({
  async handler() {
    const res = await fetch(API_CATEGORIES)
    return res.json()
  }
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

export const categories = createStore([])
  .on(fetchCategoriesFx.doneData, (state, categories) => [...categories])
  .on(createCategoryFx.doneData, (state, category) => [...state, category])