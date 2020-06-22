import {createEffect, createStore} from "effector";
import {Constants} from "../../const";

const API_URL = Constants.API
const API_MANAGERS = API_URL + 'api/managers'

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
  /*async handler(startDate, endDate) {
    console.log(startDate.toISOString().slice(0,10))

    const res = await fetch(
      API_MANAGERS + '/filter?' +
      (startDate != null ? 'startDate=' + startDate.toString() : '') +
      (endDate != null ? 'endDate=' + endDate.toString() : ''))

    return res.json()
  }*/
})

export const $managers = createStore([])
  .on(fetchManagersFx.doneData, (_, managers) => [...managers])
  .on(filterManagersFx.doneData, (_, managers) => [...managers])

