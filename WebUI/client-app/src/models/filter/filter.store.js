import {Constants} from "../../const";
import {createEffect, createStore} from "effector";

const API_URL = Constants.API
const API_SHOPS = API_URL + 'api/shops'
const API_CATEGORIES = API_URL + 'api/categories'
const API_SUPPLIERS = API_URL + 'api/suppliers'

export const fetchShopsFx = createEffect({
  async handler() {
    const res = await fetch(API_SHOPS)
    return res.json()
  }
})

export const fetchCategoriesFx = createEffect({
  async handler() {
    const res = await fetch(API_CATEGORIES)
    return res.json()
  }
})

export const fetchSupplierFx = createEffect({
  async handler() {
    const res = await fetch(API_SUPPLIERS)
    return res.json()
  }
})

export const $shopsFilter = createStore([])
  .on(fetchShopsFx.doneData, (_, shops) => [...shops])

export const $categoriesFilter = createStore([])
  .on(fetchCategoriesFx.doneData, (_, categories) => [...categories])

export const $suppliersFilter = createStore([])
  .on(fetchSupplierFx.doneData, (_, suppliers) => [...suppliers])