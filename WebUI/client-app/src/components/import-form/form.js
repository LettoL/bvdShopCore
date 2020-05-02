import React from "react";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import {makeStyles} from "@material-ui/core/styles";
import Grid from "@material-ui/core/Grid";
import ListItem from "@material-ui/core/ListItem";
import List from "@material-ui/core/List";
import ListItemText from "@material-ui/core/ListItemText";

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  }
}));

export const Form = (props) => {
  const classes = useStyles()

  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={1}>
          <List>
            <ListItem button>
              <ListItemText primary={'Импорт'}/>
            </ListItem>
          </List>
        </Grid>
        <Grid container item xs={11}>
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
        </Grid>
      </Grid>
    </div>
  )
}