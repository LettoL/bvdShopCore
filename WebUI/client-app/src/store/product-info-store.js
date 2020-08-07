import {Constants} from "../const";
import {createEffect, createStore} from "effector";

const API_URL = Constants.API
const API_PRODUCT_INFO = API_URL + 'не забудь вписать url'

export const fetchProductInfoFx = createEffect({
  async handler() {
    //const res = await fetch(API_PRODUCT_INFO);

    const zaglushka = new Promise((resolve, reject) => {
        setTimeout(() => {
            resolve({
                id: 1,
                title: 'Тайтл',
                code: 'Код',
                price: 150,
                category: 'категория',
                supplies: [{ id: 1, title: 'test', amount: 1 }],
                understaffed: [{ id: 2, title: 'test'}]
            })
        }, 100)
    })

    return zaglushka
  }
})

export const productInfo = createStore({})
  .on(fetchProductInfoFx.doneData, (state, productInfo) => productInfo)