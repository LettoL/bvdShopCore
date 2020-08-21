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
import { useStore } from 'effector-react';
import { $saleCreate, addProductToSale, removeProductFromSale, updateSaleCreateState, createSaleFx, resetSelectedMoneyWorker, changeSaleCost } from '../models/sale-create-store';
import { fetchMoneyWorkersFx, $moneyWorkers } from '../models/money-workers-store';
import { categories, fetchCategoriesFx } from '../../../store/category-store';
import { $managers, fetchManagersFx } from '../../../models/manager/manager.store';
import { SelectProducts } from './select-products';

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
    
}));

export const SaleCreate = () => {
    const classes = useStyles();
    
    useEffect(() => {
        fetchManagersFx()
    }, [])

    const managers = useStore($managers);   
    const moneyWorkers = useStore($moneyWorkers);
    const saleCreateForm = useStore($saleCreate);

    const [cashPayment, setCashPayment] = useState(false);
    const [cashlessPayment, setCashlessPayment] = useState(false);

    const handleRemoveProductFromSale = product => {   
        removeProductFromSale(product);
        changeSaleCost(-product.price);
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
                                        {
                                            managers.map(manager => (
                                                <MenuItem key={manager.id} value={manager.id}>{manager.name}</MenuItem>
                                            ))
                                        }
                                        
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
                                    <TextField className={classes.formControl} required id="standard-required" label="Сумма" name="cashSum" value={saleCreateForm.cashSum} onChange={handleSaleFormChange} />
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

            <SelectProducts />
        </>
    )
}