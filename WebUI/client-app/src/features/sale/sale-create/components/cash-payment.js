import React from "react";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";
import Grid from "@material-ui/core/Grid";
import TextField from "@material-ui/core/TextField";
import {makeStyles} from "@material-ui/core/styles";
import {useStore} from "effector-react";
import {$cashPayment, activateCashPayment, changeCashSum, deactivateCashPayment} from "../model/sale-create";

export const CashPayment = () => {
  const classes = useStyles()

  const state = useStore($cashPayment)

  const changeActive = e => {
    if(e.target.checked) activateCashPayment()
    else deactivateCashPayment()
  }

  const changeSum = e => {
    changeCashSum(e.target.value)
  }

  return (
    <>
      <FormControlLabel
        className={classes.marginDefault}
        control={<Checkbox name="cash" />}
        onChange={changeActive}
        label="Наличный платеж"
        labelPlacement="start"
      />
      {state.active && (
        <Grid container spacing={1}>
          <Grid item xs={4}>
            <TextField
              className={classes.formControl}
              required
              id="standard-required"
              label="Сумма"
              name="cashSum"
              value={state.sum}
              onChange={changeSum} />
          </Grid>
        </Grid>
      )}
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  marginDefault: {
    margin: theme.spacing(1),
    marginLeft: 0,
    display: 'block'
  },
}))