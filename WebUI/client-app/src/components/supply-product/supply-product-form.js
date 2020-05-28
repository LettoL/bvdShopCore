import React, {useState} from "react";
import TextField from "@material-ui/core/TextField";
import FormControl from "@material-ui/core/FormControl";
import {makeStyles} from "@material-ui/core/styles";
import Select from "@material-ui/core/Select";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";

export const SupplyProductForm = () => {
  const classes = useStyles()

  const [form, setForm] = useState({
    productId: 0,
    amount: 0,
    shopId: 0,
    supplierId: 0,
    procurementCost: 0
  })

  const products = [{id: 1, title: 'product1'}, {id: 2, title: 'product2'}]
  const shops = [{id: 2, title: 'shop1'}]
  const suppliers = [{id: 1, title: 'supplier1'}]

  const changeFormHandler = event => {
    setForm({...form, [event.target.name]: event.target.value})
  }

  return(
    <>
      <FormControl className={classes.formControl}>
        <InputLabel id='product-select'>Товар</InputLabel>
        <Select
          labelId='product-select'
          name='productId'
          value={form.productId}
          onChange={changeFormHandler}
        >
          <MenuItem value={0}>Выбрать товар</MenuItem>
          {products.map(product => (
            <MenuItem key={product.id} value={product.id}>{product.title}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <TextField
          label='Количество'
          value={form.amount}
          onChange={changeFormHandler}
          name='amount'
        />
      </FormControl>
      <FormControl className={classes.formControl}>
        <InputLabel id='shop-select'>Магазин</InputLabel>
        <Select
          labelId='shop-select'
          name='shopId'
          value={form.shopId}
          onChange={changeFormHandler}
        >
          <MenuItem value={0}>Выбрать магазин</MenuItem>
          {shops.map(shop => (
            <MenuItem key={shop.id} value={shop.id}>{shop.title}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <InputLabel id='supplier-select'>Поставщик</InputLabel>
        <Select
          labelId='supplier-select'
          name='supplierId'
          value={form.supplierId}
          onChange={changeFormHandler}
        >
          <MenuItem value={0}>Выбрать поставшика</MenuItem>
          {suppliers.map(supplier => (
            <MenuItem key={supplier.id} value={supplier.id}>{supplier.title}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <TextField
          label='Закупочная стоимость'
          value={form.procurementCost}
          onChange={changeFormHandler}
          name='procurementCost'
        />
      </FormControl>
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  }
}));