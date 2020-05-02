import React, {useCallback, useEffect, useState} from 'react'
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import TableContainer from "@material-ui/core/TableContainer";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import TablePagination from "@material-ui/core/TablePagination";
import Paper from "@material-ui/core/Paper";
import {makeStyles} from "@material-ui/core/styles";
import {useHttp} from "../../hooks/http.hook";
import TextField from "@material-ui/core/TextField";
import {Constants} from "../../const";

const API = Constants.API

export const ProductsHistory = () => {
  const classes = useStyles()
  const [products, setProducts] = useState([])
  const [shops, setShops] = useState([])
  const [shop, setShop] = useState(0)
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const {loading, request} = useHttp()
  const [startDate, setStartDate] = useState(null)
  const [endDate, setEndDate] = useState(null)

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  const handleChangeShop = (event) => {
    setShop(event.target.value);
  };

  const getProducts = useCallback(async () => {
    try {
      const data = await request(API + 'api/productsHistory')
      setProducts(data.map(product => (
        {...product, date: new Date(product.dateTime)}
      )))
    }
    catch (e) {}
  }, [request])

  const getShops = useCallback(async () => {
    try {
      const data = await request(API + 'api/shops')
      setShops(data)
    }
    catch (e) {}
  }, [request])

  useEffect(() => {
    getProducts()
    getShops()
  }, [getProducts, getShops])

  return(
    <Paper>
      <FormControl className={classes.formControl}>
        <InputLabel id="demo-simple-select-label">Магазин</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={shop}
          onChange={handleChangeShop}
        >
          <MenuItem value={0}>Выбрать магазин</MenuItem>
          {shops.map(shop => (
            <MenuItem key={shop.id} value={shop.id}>{shop.title}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <TextField
          label="Дата с"
          type="date"
          defaultValue=""
          className={classes.textField}
          InputLabelProps={{
            shrink: true,
          }}
          onChange={event => setStartDate(new Date(event.target.value))}
        />
      </FormControl>
      <FormControl className={classes.formControl}>
        <TextField
          label="Дата по"
          type="date"
          defaultValue=""
          className={classes.textField}
          InputLabelProps={{
            shrink: true,
          }}
          onChange={event => setEndDate(new Date(event.target.value))}
        />
      </FormControl>
      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Дата</TableCell>
              <TableCell>Товар</TableCell>
              <TableCell>Количество</TableCell>
              <TableCell>Поставщик</TableCell>
              <TableCell>Тип</TableCell>
              <TableCell>Магазин</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {products
              .filter(row => shop !== 0
                ? row.shopId === shop
                : true)
              .filter(row => startDate
                ? row.date >= startDate
                : true)
              .filter(row => endDate
                ? row.date <= endDate
                : true)
              .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
              .map(row => (
              <TableRow key={row.id}>
                <TableCell>{row.date.toLocaleDateString()}</TableCell>
                <TableCell>{row.productTitle}</TableCell>
                <TableCell>{row.amount}</TableCell>
                <TableCell>{row.supplierName}</TableCell>
                <TableCell>{row.type}</TableCell>
                <TableCell>{row.shopTitle}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <TablePagination
        rowsPerPageOptions={[10, 25, 50, 100]}
        component='div'
        count={products
          .filter(row => shop !== 0
            ? row.shopId === shop
            : true)
          .filter(row => startDate
            ? row.date >= startDate
            : true)
          .filter(row => endDate
            ? row.date <= endDate
            : true)
          .length}
        rowsPerPage={rowsPerPage}
        page={page}
        onChangePage={handleChangePage}
        onChangeRowsPerPage={handleChangeRowsPerPage}
      />
    </Paper>
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