import React, {useEffect} from "react";
import Grid from "@material-ui/core/Grid";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import TextField from "@material-ui/core/TextField";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";
import {makeStyles} from "@material-ui/core/styles";
import {useStore} from "effector-react";
import {$managers, fetchManagersFx} from "../../../models/manager/manager.store";
import {
  $cost,
  $saleInfo,
  changeDeferred,
  changeDiscount,
  changeForRussia,
  selectManagerId
} from "../models/sale-create";

export const SaleInfo = () => {
  const classes = useStyles()

  const managers = useStore($managers)
  const state = useStore($saleInfo)
  const cost = useStore($cost)

  useEffect(() => {
    fetchManagersFx()
  }, [])

  return (
    <>
      <Grid container spacing={1}>
        <Grid item xs={12}>
          <FormControl className={classes.formControl}>
            <InputLabel id="demo-simple-select-label">Менеджер</InputLabel>
            <Select
              labelId="demo-simple-select-label"
              id="demo-simple-select"
              name="managerId"
              value={state.managerId}
              onChange={e => selectManagerId(e.target.value)}
            >
              {managers.map(manager => (
                <MenuItem key={manager.id} value={manager.id}>{manager.name}</MenuItem>
              ))}
              <MenuItem value={20}>Менеджер 2</MenuItem>
              <MenuItem value={30}>Менеджер 3</MenuItem>
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={6}>
          <TextField
            className={classes.formControl}
            name="discount"
            value={state.discount}
            required
            id="standard-required"
            label="Скидка"
            onChange={e => changeDiscount(e.target.value)}
          />
        </Grid>
        <Grid item xs={6}>
          <TextField
            required
            InputProps={{readOnly: true}}
            className={classes.formControl}
            id="standard-required"
            label="Стоимость"
            name="cost"
            value={cost}
          />
        </Grid>
        <Grid item xs={4}>
          <FormControlLabel
            control={<Checkbox name="deferred" />}
            label="Отложенная"
            value={state.deferred}
            onChange={e => changeDeferred(e.target.value)}
            labelPlacement="start"
          />
        </Grid>
        <Grid item xs={4}>
          <FormControlLabel
            control={<Checkbox />}
            label="По России"
            name="forRussia"
            value={state.forRussia}
            onChange={e => changeForRussia(e.target.value)}
            labelPlacement="start"
          />
        </Grid>
      </Grid>
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    width: '100%'
  },
}))