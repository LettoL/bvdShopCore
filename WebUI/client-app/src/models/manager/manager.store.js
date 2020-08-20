import {combine, createEffect, createEvent, createStore} from "effector";
import {Constants} from "../../const";

const API_URL = Constants.API
const API_MANAGERS = API_URL + 'api/managers'

export const clickMargin = createEvent()
export const clickTurnover = createEvent()

export const fetchManagersFx = createEffect({
  async handler() {
    const res = await fetch(API_MANAGERS)
    return res.json()
  }
})

export const filterManagersFx = createEffect({
  async handler(dates) {
    const data = dates

    const res = await fetch(API_MANAGERS + '/filter', {
      headers: {
        'Content-Type': 'application/json'
      },
      method: 'POST',
      body: JSON.stringify(data)
    })

    return res.json()
  }
})

export const $managers = createStore([])
  .on(fetchManagersFx.doneData, (_, managers) => [...managers])
  .on(filterManagersFx.doneData, (_, managers) => [...managers])

export const $marginOrdered = createStore(false)
    .on(clickMargin, (_) => !_)
    .on(clickTurnover, (_) => false)

export const $turnoverOrdered = createStore(false)
    .on(clickTurnover, (_) => !_)
    .on(clickMargin, (_) => false)


export const $orderedManagers = combine(
    $managers,
    $marginOrdered,
    $turnoverOrdered,
    (managers, margin, turnover) => [...managers
        .sort((a, b) => {
          if(margin) return b.margin - a.margin
          if(turnover) return b.sum - a.sum
          return 0
        })]
)



