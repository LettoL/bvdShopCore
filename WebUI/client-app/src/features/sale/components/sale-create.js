import React, { useState, useEffect } from 'react'
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import Paper from '@material-ui/core/Paper';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import TextField from '@material-ui/core/TextField';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import Button from '@material-ui/core/Button';
import DeleteIcon from '@material-ui/icons/Delete';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { fetchProductsFx, $products } from '../../../models/product-table/product.store';
import { useStore } from 'effector-react';
import { $saleCreate, addProductToSale, removeProductFromSale, updateSaleCreateState, createSaleFx, resetSelectedMoneyWorker } from './models/sale-create-store';
import { fetchMoneyWorkersFx, $moneyWorkers } from './models/money-workers-store';

const useStyles = makeStyles((theme) => ({
    formControl: {
        width: '100%'
    },
    root: {
        flexGrow: 1,
        padding: theme.spacing(3),
    },
    marginDefault: {
        margin: theme.spacing(1),
        marginLeft: 0,
        display: 'block'
    },
    productItem: {
        padding: theme.spacing(1),
        display: "flex",
        border: '1px solid #eee',
        alignItems: 'center',
        fontSize: 18
    },
    selectProducts: {
        position: "fixed",
        right: 0,
        bottom: 0,
        top: 0,
        paddingLeft: '3%',
        width: '75%',
        boxSizing: 'border-box',
        zIndex: 99,
        backgroundColor: '#fff',
        boxShadow: '0px 6px 16px 5px rgba(0,0,0,0.2), 0px 1px 1px 0px rgba(0,0,0,0.14), 0px 1px 3px 0px rgba(0,0,0,0.12)',
        marginRight: '-72%',
        transition: '0.3s',
        '&:hover': {
            marginRight: '0'
        }
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

}));

export const SaleCreate = () => {
    const classes = useStyles();
    useEffect(() => {
        fetchProductsFx()
    }, []);

    const products = useStore($products);
    const moneyWorkers = useStore($moneyWorkers);
    const saleCreateForm = useStore($saleCreate);

    if(products.length === 0) {
        products.push({
            id: products.length,
            title: 'test',
            amount: 1,
            cost: '14000'
        })
    }
    
    const [cashPayment, setCashPayment] = useState(false);
    const [cashlessPayment, setCashlessPayment] = useState(false);

    const handleAddProductToSale = product => {
        addProductToSale(product);
    }

    const handleRemoveProductFromSale = product => {
        removeProductFromSale(product);
    }

    const handleSaleFormChange = (event) => {
        updateSaleCreateState(event);
    }

    const handleCreateSale = () => {
        createSaleFx(saleCreateForm);
    }

    const handleFetchMoneyWorkers = (value) => {
        fetchMoneyWorkersFx(value)
        resetSelectedMoneyWorker();
    }

    return (
        <>
            <Paper className={classes.root}>
                <div >
                    <h2>Новая продажа</h2>
                </div>
                <hr />

                <Grid container spacing={4}>

                    <Grid item xs={5}>
                        <h3>Основная информация по продаже</h3>
                        <Grid container spacing={1}>
                            <Grid item xs={12}>
                                <FormControl className={classes.formControl}>
                                    <InputLabel id="demo-simple-select-label">Менеджер</InputLabel>
                                    <Select
                                        labelId="demo-simple-select-label"
                                        id="demo-simple-select"
                                        name="managerId"
                                        value={saleCreateForm.managerId}
                                        onChange={handleSaleFormChange}
                                    >
                                        <MenuItem value={10}>Менеджер 1</MenuItem>
                                        <MenuItem value={20}>Менеджер 2</MenuItem>
                                        <MenuItem value={30}>Менеджер 3</MenuItem>
                                    </Select>
                                </FormControl>
                            </Grid>
                            <Grid item xs={6}>
                                <TextField className={classes.formControl} name="discount" value={saleCreateForm.discount} required id="standard-required" label="Скидка" onChange={handleSaleFormChange} />
                            </Grid>
                            <Grid item xs={6}>
                                <TextField required InputProps={{
                                    readOnly: true,
                                }}
                                    className={classes.formControl}
                                    id="standard-required"
                                    label="Стоимость"
                                    name="cost"
                                    value={saleCreateForm.cost}
                                    onChange={handleSaleFormChange} />
                            </Grid>
                            <Grid item xs={4}>
                                <FormControlLabel
                                    control={<Checkbox name="deferred" />}
                                    label="Отложенная"
                                    value={saleCreateForm.deferred}
                                    onChange={handleSaleFormChange}
                                    labelPlacement="start"
                                />
                            </Grid>
                            <Grid item xs={4}>
                                <FormControlLabel
                                    control={<Checkbox />}
                                    label="По России"
                                    name="forRussia"
                                    value={saleCreateForm.forRussia}
                                    onChange={handleSaleFormChange}
                                    labelPlacement="start"
                                />
                            </Grid>
                        </Grid>

                        <h3>Информация по платежам</h3>

                        <FormControlLabel className={classes.marginDefault}
                            control={<Checkbox name="cash" />}
                            onChange={event => setCashPayment(event.target.checked)}
                            label="Наличный платеж"
                            labelPlacement="start"
                        />

                        {cashPayment && (
                            <Grid container spacing={1}>
                                <Grid item xs={4}>
                                    <TextField className={classes.formControl} required id="standard-required" label="Сумма" name="cashSum" value={saleCreateForm.cashSum} onChange={handleSaleFormChange}/>
                                </Grid>
                            </Grid>
                        )}

                        <FormControlLabel className={classes.marginDefault}
                            control={<Checkbox name="cashless" />}
                            onChange={event => setCashlessPayment(event.target.checked)}
                            label="Безналичный платеж"
                            labelPlacement="start"
                        />

                        {cashlessPayment && (
                            <Grid container spacing={1}>                              
                                <Grid item xs={6}>
                                    <FormControl className={classes.formControl}>
                                        <InputLabel id="demo-simple-select-label">Тип счета</InputLabel>
                                        <Select
                                            labelId="demo-simple-select-label"
                                            id="demo-simple-select"
                                            defaultValue={0}
                                            onChange={(event) => handleFetchMoneyWorkers(event.target.value)}
                                        >
                                            <MenuItem value={1}>Держатель карты</MenuItem>
                                            <MenuItem value={2}>Рассчетный счет</MenuItem>
                                            <MenuItem value={3}>Магазин</MenuItem>
                                        </Select>
                                    </FormControl>
                                </Grid>
                                <Grid item xs={6}>
                                    <FormControl className={classes.formControl}>
                                        <InputLabel id="demo-simple-select-label">Счет</InputLabel>
                                        <Select
                                            labelId="demo-simple-select-label"
                                            id="demo-simple-select"
                                            name="moneyWorkerId"
                                            onChange={handleSaleFormChange}
                                            value={saleCreateForm.moneyWorkerId}
                                        >
                                            {
                                                moneyWorkers.map((moneyWorker) => (
                                                    <MenuItem key={moneyWorker.id} value={moneyWorker.id}>{moneyWorker.title}</MenuItem>
                                                ))
                                            }
                                            
                                        </Select>
                                    </FormControl>
                                </Grid>
                                <Grid item xs={12}>
                                    <TextField className={classes.formControl} required id="standard-required" label="Сумма" name="cashlessSum" value={saleCreateForm.cashlessSum} onChange={handleSaleFormChange} />
                                </Grid>
                            </Grid>)}
                    </Grid>
                    <Grid item xs={7}>
                        <h3>Выбранные товары</h3>
                        <Grid container spacing={1}>
                            {
                                saleCreateForm.saleProducts.map((product) => (
                                    <Grid key={product.id} container spacing={1} className={classes.productItem}>
                                        <Grid item xs={2}>
                                            <b>{product.title}</b>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <TextField required id="standard-required" label="Количество" defaultValue="1" />
                                        </Grid>
                                        <Grid item xs={3}>
                                            <FormControlLabel
                                                control={<Checkbox name="additional" />}
                                                label="Дополнительный"
                                                labelPlacement="start"
                                            />
                                        </Grid>
                                        <Grid item xs={2}>
                                            <Button
                                                variant="contained"
                                                color="secondary"
                                                startIcon={<DeleteIcon />}
                                                onClick={() => handleRemoveProductFromSale(product)}
                                            >
                                                Удалить
                                            </Button>
                                        </Grid>
                                    </Grid>
                                ))
                            }

                        </Grid>
                    </Grid>
                </Grid>

                <Button className={classes.marginDefault} variant="contained" color="primary" onClick={() => handleCreateSale()}>
                    Создать
                </Button>

            </Paper>

            <div className={classes.selectProducts} >
                <div className={classes.sectionTitle}>Товары</div>
                <TableContainer >
                    <TextField id="standard-required" label="Поиск товара" defaultValue="" />
                    <Table className={classes.table} aria-label="simple table">
                        <TableHead>
                            <TableRow>
                                <TableCell>Название</TableCell>
                                <TableCell align="center">Количество</TableCell>
                                <TableCell align="center">Действие</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {products.map((row) => (
                                <TableRow key={row.title}>
                                    <TableCell component="th" scope="row">
                                        {row.title}
                                    </TableCell>
                                    <TableCell align="center">{row.amount}</TableCell>
                                    <TableCell align="center">
                                        <Button variant="contained" color="primary" onClick={() => handleAddProductToSale(row)}>
                                            Добавить
                                        </Button>
                                    </TableCell>

                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
        </>
    )
}