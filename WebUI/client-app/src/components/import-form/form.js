import React, {Fragment} from "react";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import Grid from "@material-ui/core/Grid";

export const Form = (props) => {

  return (
    <Fragment>
      <Grid item xs={6}>
        <TextField
          label="Внесите товары"
          multiline
          rows={10}
          variant="outlined"
          value={props.productsText}
          onChange={props.productsTextChange}
          name="products"
          fullWidth
        />
        <br/>
        <Button
          variant="contained"
          color="primary"
          onClick={props.productsTextHandle}
        >
          Обработать
        </Button>
      </Grid>
      <Grid item xs={5}>
        <ul>
          {props.products.map(product => (
            <li key={product.number}>
              {product.number}
              {product.code}
              {product.title}
              {product.amount}
              {product.price}
              {product.sum}
            </li>
          ))}
        </ul>
      </Grid>
    </Fragment>
  )
}