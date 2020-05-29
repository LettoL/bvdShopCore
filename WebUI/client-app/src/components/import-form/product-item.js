import React from "react";
import ListItem from "@material-ui/core/ListItem";
import Divider from "@material-ui/core/Divider";
import Typography from "@material-ui/core/Typography";
import TextField from "@material-ui/core/TextField";
import {makeStyles} from "@material-ui/core/styles";
import Grid from "@material-ui/core/Grid";

export const ProductItem = props => {
  const classes = useStyles()
  const {product, editProduct} = props

  const changeHandler = event => {
    const newProduct = {...product, [event.target.name]: [event.target.value]}
    editProduct(newProduct)
  }

  return (
    <Grid container>
      <ListItem key={product.number}>
        <Grid item xs={1}>
          <Typography variant='h6'>{product.number}</Typography>
        </Grid>
        <Grid item container xs={11}>
          <Grid item xs={12}>
            <TextField
              value={product.title}
              variant='outlined'
              label='Название'
              size='small'
              name='title'
              onChange={changeHandler}
              className={classes.maxWidth}
            />
          </Grid>
          <Grid item xs={4}>
            <TextField
              value={product.code}
              variant='outlined'
              label='Артикул'
              size='small'
              name='code'
              onChange={changeHandler}
              className={classes.maxWidth}
            />
          </Grid>
          <Grid item xs={4}>
            <TextField
              className={classes.maxWidth}
              value={product.amount}
              variant='outlined'
              label='Количество'
              size='small'
              name='amount'
              onChange={changeHandler}
            />
          </Grid>
          <Grid item xs={4}>
            <TextField
              value={product.price}
              variant='outlined'
              label='Закупочная стоимость'
              size='small'
              name='price'
              onChange={changeHandler}
              className={classes.maxWidth}
            />
          </Grid>
        </Grid>
      </ListItem>
      <Divider variant='inset' component='li'/>
    </Grid>
  )
}

const useStyles = makeStyles(theme => ({
  maxWidth: {
    width: '100%',
    padding: 5
  }
}))