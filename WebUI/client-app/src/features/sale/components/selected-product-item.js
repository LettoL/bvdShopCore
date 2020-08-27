import React from "react";
import Grid from "@material-ui/core/Grid";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import DeleteIcon from "@material-ui/icons/Delete";
import {makeStyles} from "@material-ui/core/styles";
import {removeProductFromSale} from "../models/sale-create";

export const SelectedProductItem = (props) => {
  const classes = useStyles()

  const {product} = props

  const removeProductHandler = () => {
    removeProductFromSale(product)
  }

  return (
    <Grid container spacing={1} className={classes.productItem}>
      <Grid item xs={2}>
        <b>{product.title}</b>
      </Grid>
      <Grid item xs={2}>
        <TextField required id="standard-required" label="Количество" defaultValue="1" />
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