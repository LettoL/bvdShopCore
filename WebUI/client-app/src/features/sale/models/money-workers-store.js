import { createEffect, createStore } from "effector"
import { Constants } from "../../../const"
import { setError } from "../../../shared/store/error-store"

const API_URL = Constants.API
const API_FETCH_MONEYWORKERS = API_URL + 'не забудь вписать url'

export const fetchMoneyWorkersFx = createEffect(
  {
    async handler(value) {
      //const res = await fetch(`${API_FETCH_MONEYWORKERS}/${value}`)

      const zaglushka = new Promise((resolve, reject) => {
        setTimeout(() => {
          resolve([{
              id: 1,
              title: 'Тайтл',
            },
            {
              id: 2,
              title: 'calculatedScore',
            }
          ])
        }, 100)
      })

      return zaglushka
    }
  }
)

fetchMoneyWorkersFx.finally.watch(data => {
  if (data.error)
    setError(`При загрузке безналичных счетов, произошла ошибка: ${data.error}`)
})

export const $moneyWorkers = createStore([]);


$moneyWorkers.on(fetchMoneyWorkersFx.doneData, (state, moneyWorkers) => [...moneyWorkers]);