import React, {Fragment, useState} from "react";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import Grid from "@material-ui/core/Grid";
import {ProductItem} from "./product-item";
import List from "@material-ui/core/List";
import Select from "@material-ui/core/Select";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import {makeStyles} from "@material-ui/core/styles";

export const Form = (props) => {
  const classes = useStyles()
  const {shops, suppliers, categories, saveForm} = props

  const [form, setForm] = useState({
    productsText: '',
    shop: null,
    supplier: null,
    supplyType: null,
    category: null
  })
  const [products, setProducts] = useState([])
  const [disabled, setDisabled] = useState(false)

  const changeFormHandler = event => {
    setForm({...form, [event.target.name]: event.target.value})
  }

  const productsTextHandle = () => {
    if(form.productsText.length <= 0)
      return

    const productsText = form.productsText.split('\n');
    const productsProperties = productsText.map(x => x.split('\t'));

    const products = productsProperties.map(x => {
      return {
        number: +(x[0].replace(',', '.').replace(/\s+/g,'')),
        code: x[1],
        title: x[2],
        amount: +(x[3].replace(',', '.').replace(/\s+/g,'')),
        price: +(x[5].replace(',', '.').replace(/\s+/g,'')),
        sum: +(x[6].replace(',', '.').replace(/\s+/g,''))
      }
    });

    setProducts(products)
  }

  const editProduct = (product) => {
    const productIndex = products.findIndex(x => x.number === product.number)
    setProducts([...products.slice(0, productIndex), product, ...products.slice(productIndex + 1)])
  }

  const saveHandler = () => {
    setDisabled(true)

    const productsForSave = products.map(product => ({
      ...product, ['price']: product.price + ''
    }))

    saveForm({...form, products: productsForSave})
  };

  return (
    <Fragment>
      <Grid item xs={4}>
        <FormControl className={classes.formControl}>
          <InputLabel id='shop-select'>Магазин</InputLabel>
          <Select
            labelId='shop-select'
            value={form.shop}
            onChange={changeFormHandler}
            name='shop'
          >
            <MenuItem value={0}>Выбрать магазин</MenuItem>
            {shops.map(shop => (
              <MenuItem key={shop.id} value={shop.id}>{shop.title}</MenuItem>
            ))}
          </Select>
        </FormControl>
        <FormControl className={classes.formControl}>
          <InputLabel id='shop-select'>Поставщика</InputLabel>
          <Select
            labelId='shop-select'
            value={form.supplier}
            onChange={changeFormHandler}
            name='supplier'
          >
            <MenuItem value={0}>Выбрать поставщика</MenuItem>
            {suppliers.map(supplier => (
              <MenuItem key={supplier.id} value={supplier.id}>{supplier.name}</MenuItem>
            ))}
          </Select>
        </FormControl>
        <FormControl className={classes.formControl}>
          <InputLabel id='shop-select'>Тип товара</InputLabel>
          <Select
            labelId='shop-select'
            value={form.supplyType}
            onChange={changeFormHandler}
            name='supplyType'
          >
            <MenuItem value={0}>Выбрать тип товара</MenuItem>
            <MenuItem value={1}>Оплаченный товар</MenuItem>
            <MenuItem value={2}>Товар под реализацию</MenuItem>
            <MenuItem value={3}>Отсроченый платёж</MenuItem>
          </Select>
        </FormControl>
        <FormControl className={classes.formControl}>
          <InputLabel id='shop-select'>Категория</InputLabel>
          <Select
            labelId='shop-select'
            value={form.category}
            onChange={changeFormHandler}
            name='category'
          >
            <MenuItem value={0}>Выбрать категорию</MenuItem>
            {categories.map(category => (
              <MenuItem key={category.id} value={category.id}>{category.title}</MenuItem>
            ))}
          </Select>
        </FormControl>
        <br/>
        <TextField
          label="Внесите товары"
          multiline
          rows={10}
          variant="outlined"
          value={form.productsText}
          onChange={changeFormHandler}
          name="productsText"
          fullWidth
        />
        <br/>
        <Button
          variant="contained"
          color='primary'
          onClick={productsTextHandle}
        >
          Обработать
        </Button>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <Button
          variant='contained'
          color='primary'
          onClick={saveHandler}
          disabled={disabled}
        >
          Сохранить
        </Button>
      </Grid>
      <Grid item xs={7}>
        <List>
          {products.map(product => (
            <ProductItem
              key={product.number}
              product={product}
              editProduct={editProduct}
            />
          ))}
        </List>
      </Grid>
    </Fragment>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  }
}));