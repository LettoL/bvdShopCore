import React, {useEffect} from "react";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";
import Grid from "@material-ui/core/Grid";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import TextField from "@material-ui/core/TextField";
import {makeStyles} from "@material-ui/core/styles";
import {useStore} from "effector-react";
import {
  $cashlessPayment,
  activateCashlessPayment, changeCashlessSum,
  deactivateCashlessPayment, selectMoneyWorkerId,
} from "../models/sale-create";
import {$moneyWorkers, fetchMoneyWorkersFx} from "../models/money-workers-store";

export const CashlessPayment = () => {
  const classes = useStyles()

  const state = useStore($cashlessPayment)
  const moneyWorkers = useStore($moneyWorkers)

  const changeActive = e => {
    if(e.target.checked) activateCashlessPayment()
    else deactivateCashlessPayment()
  }

  const selectMoneyWorker = e => {
    selectMoneyWorkerId(e.target.value)
  }

  useEffect(() => {
    fetchMoneyWorkersFx()
  }, [])

  return (
    <>
      <FormControlLabel
        className={classes.marginDefault}
        control={<Checkbox name="cashless" />}
        onChange={changeActive}
        label="Безналичный платеж"
        labelPlacement="start"
      />
      {state.active && (
        <Grid container spacing={1}>
          <Grid item xs={6}>
            <FormControl className={classes.formControl}>
              <InputLabel id="demo-simple-select-label">Тип счета</InputLabel>
              <Select
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                defaultValue={0}
              >
                <MenuItem value={1}>Держатель карты</MenuItem>
                <MenuItem value={2}>Рассчетный счет</MenuItem>
                <MenuItem value={3}>Магазин</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={6}>
            <FormControl className={classes.formControl}>
              <InputLabel id="demo-simple-select-label">Счет</InputLabel>
              <Select
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                name="moneyWorkerId"
                onChange={selectMoneyWorker}
                value={state.moneyWorkerId}
              >
                {moneyWorkers.map((moneyWorker) => (
                  <MenuItem key={moneyWorker.id} value={moneyWorker.id}>{moneyWorker.title}</MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12}>
            <TextField
              className={classes.formControl}
              required
              id="standard-required"
              label="Сумма"
              name="cashlessSum"
              value={state.sum}
              onChange={e => changeCashlessSum(e.target.value)} />
          </Grid>
        </Grid>)}
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    width: '100%'
  },
  marginDefault: {
    margin: theme.spacing(1),
    marginLeft: 0,
    display: 'block'
  },
}))