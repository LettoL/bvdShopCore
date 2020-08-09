import React, { createRef, useEffect, useState } from 'react'

import { makeStyles } from '@material-ui/core/styles';


import Paper from '@material-ui/core/Paper'
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { useStore } from 'effector-react';
import { $currentProduct, createUnderstaffedProductFx } from '../models/product-info-store';
import InputLabel from '@material-ui/core/InputLabel';

import FormControl from '@material-ui/core/FormControl';
import Button from '@material-ui/core/Button';
import MenuItem from '@material-ui/core/MenuItem';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import Select from '@material-ui/core/Select';
import { fetchShopsFx, shops } from '../../../store/shop-store';

const useStyles = makeStyles((theme) => ({
  formControl: {
    width: '100%',
  },
}));

export const ProductsUnderstaffed = () => {
  const classes = useStyles();

  const shopsList = useStore(shops);

  const [open, setOpen] = React.useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const currentProduct = useStore($currentProduct);

  const [newProductAmount, setNewProductAmount] = useState(0);
  const [newProductShopId, setNewProductShopId] = useState(0);  

  const createNewProduct = () => {
    const newProduct = {
      id: currentProduct.id,
      shopId: newProductShopId,
      amount: +newProductAmount
    }

    createUnderstaffedProductFx(newProduct);

    setNewProductAmount(0);
    setNewProductShopId(0);

    handleClose();
  }

  return (
    <div>
      <Button variant="outlined" color="primary" onClick={handleClickOpen}>
        Добавить некомплект
      </Button>
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle id="form-dialog-title">Создание некомплекта</DialogTitle>
        <DialogContent>
          <FormControl className={classes.formControl}>
            <InputLabel id="demo-simple-select-label">Магазин</InputLabel>
            <Select
              labelId="demo-simple-select-label"
              id="demo-simple-select"
              value={newProductShopId}
              onChange={event => setNewProductShopId(event.target.value)}
            >
              {shopsList.map((row) => (
                <MenuItem value={row.id}>{row.title}</MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField
            margin="dense"
            id="name"
            label="Количество"
            type="number"
            value={newProductAmount}
            onChange={event => setNewProductAmount(event.target.value)}
            fullWidth
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="primary">
            Отмена
          </Button>
          <Button onClick={createNewProduct} color="primary">
            Сохранить
          </Button>
        </DialogActions>
      </Dialog>
      <TableContainer component={Paper}>
        <Table aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell align="right">Название</TableCell>
              <TableCell align="right">Магазин</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {currentProduct.understaffed.map((row) => (
              <TableRow key={row.id}>
                <TableCell component="th" scope="row">
                  {row.id}
                </TableCell>
                <TableCell align="right">{row.title}</TableCell>
                <TableCell align="right">{row.shopTitle}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  )
}