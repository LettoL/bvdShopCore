import React from "react";
import Grid from "@material-ui/core/Grid";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import DeleteIcon from "@material-ui/icons/Delete";
import {makeStyles} from "@material-ui/core/styles";
import {changeProductAmount, changeProductPrice, removeProductFromSale} from "../model/sale-create";

export const SelectedProductItem = (props) => {
  const classes = useStyles()

  const {product} = props

  const removeProductHandler = () => {
    removeProductFromSale(product)
  }

  const changePrice = e => {
    changeProductPrice({id: product.id, price: e.target.value})
  }

  const changeAmount = e => {
    changeProductAmount({id: product.id, amount: e.target.value})
  }

  return (
    <Grid container spacing={1} className={classes.productItem}>
      <Grid item xs={2}>
        <b>{product.title}</b>
      </Grid>
      <Grid item xs={2}>
        <TextField
          required
          id="standard-required"
          label="Количество"
          value={product.amount}
          onChange={changeAmount}
        />
      </Grid>
      <Grid item xs={3}>
        <TextField
          id="standard-required"
          label="Цена за шт."
          value={product.price}
          onChange={changePrice}
        />
      </Grid>
      <Grid item xs={2}>
        <Button
          variant="contained"
          color="secondary"
          startIcon={<DeleteIcon />}
          onClick={removeProductHandler}
        >
          Удалить
        </Button>
      </Grid>
    </Grid>
  )
}

const useStyles = makeStyles((theme) => ({
  productItem: {
    padding: theme.spacing(1),
    display: "flex",
    border: '1px solid #eee',
    alignItems: 'center',
    fontSize: 18
  },
}))