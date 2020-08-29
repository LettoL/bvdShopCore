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
import { $saleInfo, updateSaleInfo, fxCreateSale } from '../models/sale-create';
import { $managers, fetchManagersFx } from '../../../models/manager/manager.store';
import { SelectProducts } from './select-products';
import {SelectedProductsList} from "./selected-products-list";
import {PaymentMethods} from "./payment-methods";
import {SaleInfo} from "./sale-info";


export const SaleCreate = () => {
  const classes = useStyles();

  useEffect(() => {
    fetchManagersFx()
  }, [])

  const managers = useStore($managers);
  const saleCreateForm = useStore($saleInfo);

  const handleSaleFormChange = (event) => {
    updateSaleInfo(event);
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
            <SaleInfo/>
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