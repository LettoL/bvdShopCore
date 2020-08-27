import React, { useEffect } from 'react'
import { categories, fetchCategoriesFx } from "../../../store/category-store";
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import Grid from '@material-ui/core/Grid';
import TextField from '@material-ui/core/TextField';
import { addProductToSale, changeSaleCost } from "../models/sale-create";
import { makeStyles } from '@material-ui/core';
import { useStore } from 'effector-react';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { FixedSizeList } from 'react-window';
import {$availableProducts} from "../models/store";
import {fxFetchProducts} from "../../product/models/store";

export const SelectProducts = () => {
  const classes = useStyles();

  useEffect(() => {
    fxFetchProducts()
  }, [])

  const products = useStore($availableProducts)

  const handleMouseOverSelectProducts = event => {
    event.target.style.marginRight = 0;
  }

  const handleMouseOutSelectProducts = event => {
    var e = event.toElement || event.relatedTarget;
    if (e.parentNode == this || e == this) {
      return;
    }
    else {
      event.target.style.marginRight = '-460px'
    }
  }


  function renderRow(props) {
    const { index, style } = props;

    const product = products[index];

    const addProduct = (e) => {
      e.preventDefault();
      addProductToSale(product);
    };

    return (
      <ListItem button key={index} style={style} onClick={addProduct}>
        <ListItemText primary={`${product.title} ${product.availableAmount}`} />
      </ListItem>
    );
  }

  return (
    <>
      <div className={classes.selectProducts} onMouseEnter={(event) => handleMouseOverSelectProducts(event)} onMouseLeave={event => handleMouseOutSelectProducts(event)}>
        <div className={classes.sectionTitle}>Товары</div>
        <Grid container spacing={5}>
          <Grid item xs={6}>
            <TextField id="standard-required" label="Поиск товара" defaultValue="" />
          </Grid>
        </Grid>
        <FixedSizeList className={classes.productList} height={780} width={480} itemSize={46} itemCount={products.length}>
          {renderRow}
        </FixedSizeList>

      </div>
    </>
  )
}


const useStyles = makeStyles(theme => ({
  formControl: {
    width: '100%'
  },
  selectProducts: {
    position: "fixed",
    right: 0,
    bottom: 0,
    top: 0,
    paddingLeft: '3%',
    width: '520px',
    boxSizing: 'border-box',
    zIndex: 99,
    backgroundColor: '#fff',
    boxShadow: '0px 6px 16px 5px rgba(0,0,0,0.2), 0px 1px 1px 0px rgba(0,0,0,0.14), 0px 1px 3px 0px rgba(0,0,0,0.12)',
    marginRight: '-460px',
    transition: '0.3s',
  },

  sectionTitle: {
    fontWeight: 'bold',
    transform: 'rotate(-90deg)',
    display: 'block',
    position: "absolute",
    top: '50%',
    left: '-5px',
    fontSize: '18px',
  },
  productList: {
    marginTop: 20
  }

}))
