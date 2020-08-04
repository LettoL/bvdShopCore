import React, {useEffect, useState} from 'react'
import Grid from "@material-ui/core/Grid";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import {ProductListItem} from "./product-list-item";
import TextField from "@material-ui/core/TextField";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import {$shopsFilter, $suppliersFilter, fetchShopsFx, fetchSupplierFx} from '../../models/filter/filter.store'
import {useStore} from 'effector-react'
import Button from '@material-ui/core/Button'
import makeStyles from '@material-ui/core/styles/makeStyles'

export const Product = () => {
  const classes = useStyles()

  const products = ['product1', 'product2', 'product3']
  const uncomplects = [
    {supplierTitle: 'supplier1', amount: 2, comment: 'adsf'},
    {supplierTitle: 'supplier1', amount: 1, comment: 'asdfadsfasdfasdf'},
    {supplierTitle: 'supplier2', amount: 2, comment: 'asdfa'},
  ]

  const [supplierId, setSupplierId] = useState(null)
  const [shopId, setShopId] = useState(null)
  const [amount, setAmount] = useState(0)
  const [comment, setComment] = useState('')
  const [productId, setProductId] = useState(null)

  const suppliers = useStore($suppliersFilter)
  const shops = useStore($shopsFilter)

  useEffect(() => {
    fetchSupplierFx()
    fetchShopsFx()
  }, [])

  return (
    <Grid container>
      <Grid item container xs={10}>
        <FormControl className={classes.formControl}>
          <InputLabel>Поставщик</InputLabel>
          <Select
            value={supplierId}
            onChange={event => setSupplierId(event.target.value)}
          >
            {suppliers.map(supplier => (
              <MenuItem value={supplier.id}>{supplier.name}</MenuItem>
            ))}
          </Select>
        </FormControl>
        <FormControl className={classes.formControl}>
          <InputLabel>Магазин</InputLabel>
          <Select
            value={shopId}
            onChange={event => setShopId(event.target.value)}
          >
            {shops.map(shop => (
              <MenuItem value={shop.id}>{shop.title}</MenuItem>
            ))}
          </Select>
        </FormControl>
        <FormControl className={classes.formControl}>
          <TextField
            label={'Количество'}
            value={amount}
            onChange={event => setAmount(event.target.value)}
          />
        </FormControl>
        <FormControl className={classes.formControl}>
          <TextField
            multiline
            label={'Комментарий'}
            value={comment}
            onChange={event => setComment(event.target.value)}
          />
        </FormControl>
        <Button
          variant={'contained'}
          color={'primary'}
        >
          Сохранить
        </Button>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Поставщик</TableCell>
              <TableCell>Количество</TableCell>
              <TableCell>Комментарий</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {uncomplects.map(uncomplect => (
              <TableRow>
                <TableCell>{uncomplect.supplierTitle}</TableCell>
                <TableCell>{uncomplect.amount}</TableCell>
                <TableCell>{uncomplect.comment}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Grid>
      <Grid item xs={2}>
        <TextField variant='outlined' label='Название '/>
        <List>
          {products.map(product => (
            <ProductListItem
              title={product}
            />
          ))}
        </List>
      </Grid>
    </Grid>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  },
  selectEmpty: {
    marginTop: theme.spacing(2),
  },
}));