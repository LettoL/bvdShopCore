import React, { useEffect } from 'react'
import { $products, fetchProductsFx } from "../../../models/product-table/product.store";
import { categories, fetchCategoriesFx } from "../../../store/category-store";
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import Grid from '@material-ui/core/Grid';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { addProductToSale, changeSaleCost } from "../models/sale-create-store";
import { makeStyles } from '@material-ui/core';
import { useStore } from 'effector-react';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { FixedSizeList } from 'react-window';

export const SelectProducts = () => {
    const classes = useStyles();

    useEffect(() => {
        fetchProductsFx()
    }, []);

    useEffect(() => {
        fetchCategoriesFx()
    },[])

    const products = useStore($products);
    const categoriesList = useStore(categories);


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
            changeSaleCost(product.price);
        };

        return (
          <ListItem button key={index} style={style} onClick={addProduct}>
            <ListItemText primary={`${product.title} ${product.amount}`} />
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
                    <Grid item xs={6}>
                        <FormControl className={classes.formControl}>
                            <InputLabel id="demo-simple-select-label">Категория</InputLabel>
                            <Select
                                labelId="demo-simple-select-label"
                                id="demo-simple-select"
                                name="managerId"
                            >
                                {
                                    categoriesList.map(category => (
                                        <MenuItem key={category.id} value={category.id}>{category.title}</MenuItem>
                                    ))
                                }
                            </Select>
                        </FormControl>
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
