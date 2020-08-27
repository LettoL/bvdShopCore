import React, { useEffect } from 'react'
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import Paper from '@material-ui/core/Paper';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import TextField from '@material-ui/core/TextField';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import Button from '@material-ui/core/Button';
import { useStore } from 'effector-react';
import { $saleCreate, updateSaleCreateState, fxCreateSale } from '../models/sale-create';
import { $managers, fetchManagersFx } from '../../../models/manager/manager.store';
import { SelectProducts } from './select-products';
import {SelectedProductsList} from "./selected-products-list";
import {PaymentMethods} from "./payment-methods";


export const SaleCreate = () => {
  const classes = useStyles();

  useEffect(() => {
    fetchManagersFx()
  }, [])

  const managers = useStore($managers);
  const saleCreateForm = useStore($saleCreate);

  const handleSaleFormChange = (event) => {
    updateSaleCreateState(event);
  }

  const handleCreateSale = () => {
    fxCreateSale(saleCreateForm);
  }

  return (
    <>
      <Paper className={classes.root}>
        <div >
          <h2>Новая продажа</h2>
        </div>
        <hr />
        <Grid container spacing={4}>
          <Grid item xs={5}>
            <h3>Основная информация по продаже</h3>
            <Grid container spacing={1}>
              <Grid item xs={12}>
                <FormControl className={classes.formControl}>
                  <InputLabel id="demo-simple-select-label">Менеджер</InputLabel>
                  <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    name="managerId"
                    value={saleCreateForm.managerId}
                    onChange={handleSaleFormChange}
                  >
                    {
                      managers.map(manager => (
                        <MenuItem key={manager.id} value={manager.id}>{manager.name}</MenuItem>
                      ))
                    }
                    <MenuItem value={20}>Менеджер 2</MenuItem>
                    <MenuItem value={30}>Менеджер 3</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={6}>
                <TextField className={classes.formControl} name="discount" value={saleCreateForm.discount} required id="standard-required" label="Скидка" onChange={handleSaleFormChange} />
              </Grid>
              <Grid item xs={6}>
                <TextField required InputProps={{
                  readOnly: true,
                }}
                           className={classes.formControl}
                           id="standard-required"
                           label="Стоимость"
                           name="cost"
                           value={saleCreateForm.cost}
                           onChange={handleSaleFormChange} />
              </Grid>
              <Grid item xs={4}>
                <FormControlLabel
                  control={<Checkbox name="deferred" />}
                  label="Отложенная"
                  value={saleCreateForm.deferred}
                  onChange={handleSaleFormChange}
                  labelPlacement="start"
                />
              </Grid>
              <Grid item xs={4}>
                <FormControlLabel
                  control={<Checkbox />}
                  label="По России"
                  name="forRussia"
                  value={saleCreateForm.forRussia}
                  onChange={handleSaleFormChange}
                  labelPlacement="start"
                />
              </Grid>
            </Grid>
            <PaymentMethods/>
          </Grid>
          <Grid item xs={7}>
            <h3>Выбранные товары</h3>
            <Grid container spacing={1}>
              <SelectedProductsList/>
            </Grid>
          </Grid>
        </Grid>
        <Button className={classes.marginDefault} variant="contained" color="primary" onClick={() => handleCreateSale()}>
          Создать
        </Button>
      </Paper>
      <SelectProducts />
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    width: '100%'
  },
  root: {
    flexGrow: 1,
    padding: theme.spacing(3),
  },
  marginDefault: {
    margin: theme.spacing(1),
    marginLeft: 0,
    display: 'block'
  },
  productItem: {
    padding: theme.spacing(1),
    display: "flex",
    border: '1px solid #eee',
    alignItems: 'center',
    fontSize: 18
  },
}));