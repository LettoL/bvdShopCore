import React from "react";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import TextField from "@material-ui/core/TextField";
import {makeStyles} from "@material-ui/core/styles";

export const SupplyProductItem = props => {
  const classes = useStyles()

  const {index, changeFormHandler} = props
  const {productId, amount, shopId, supplierId, procurementCost} = props.supplyProduct

  const products = [{id: 1, title: 'product1'}, {id: 2, title: 'product2'}]
  const shops = [{id: 2, title: 'shop1'}]
  const suppliers = [{id: 1, title: 'supplier1'}]
  
  const changeProduct = event => {
    changeFormHandler(index, event)
  }

  return(
    <>
      <div>
      <FormControl className={classes.formControl}>
        <InputLabel id='product-select'>Товар</InputLabel>
        <Select
          labelId='product-select'
          name='productId'
          value={productId}
          onChange={changeProduct}
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
          value={amount}
          onChange={changeProduct}
          name='amount'
        />
      </FormControl>
      <FormControl className={classes.formControl}>
        <InputLabel id='shop-select'>Магазин</InputLabel>
        <Select
          labelId='shop-select'
          name='shopId'
          value={shopId}
          onChange={changeProduct}
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
          value={supplierId}
          onChange={changeProduct}
        >
          <MenuItem value={0}>Выбрать поставщика</MenuItem>
          {suppliers.map(supplier => (
            <MenuItem key={supplier.id} value={supplier.id}>{supplier.title}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <TextField
          label='Закупочная стоимость'
          value={procurementCost}
          onChange={changeProduct}
          name='procurementCost'
        />
      </FormControl>
      </div>
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  }
}));