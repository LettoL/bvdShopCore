import React, {useCallback, useEffect, useState} from 'react'
import {useHttp} from "../../hooks/http.hook";
import TableContainer from "@material-ui/core/TableContainer";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import Paper from "@material-ui/core/Paper";
import TablePagination from "@material-ui/core/TablePagination";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import {makeStyles} from "@material-ui/core/styles";
import TextField from "@material-ui/core/TextField";
import {Constants} from "../../const";

const API = Constants.API

export const ProductsPage = () => {
  const classes = useStyles();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [shop, setShop] = useState(0);
  const [category, setCategory] = useState(0);
  const [title, setTitle] = useState('');

  const {loading, request} = useHttp()
  const [products, setProducts] = useState([]);
  const [shops, setShops] = useState([])
  const [categories, setCategories] = useState([])

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

  const handleChangeCategory = event => {
    setCategory(event.target.value)
  }

  const handleChangeTitle = event => {
    setTitle(event.target.value)
  }

  const getProducts = useCallback(async () => {
    try {
      const data = await request(API + 'api/products')
      setProducts(data)
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

  const getCategories = useCallback(async () => {
    try {
      const data = await request(API + 'api/categories')
      setCategories(data)
    }
    catch (e) {
    }
  }, [request])

  useEffect(() => {
    getProducts()
    getShops()
    getCategories()
  }, [getProducts, getShops, getCategories])

  if(loading)
    return (
      <div>Загрузка...</div>
    )
  else
    return (
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
          <InputLabel id="demo-simple-select-label">Категория</InputLabel>
          <Select
            labelId="demo-simple-select-label"
            id="demo-simple-select"
            value={category}
            onChange={handleChangeCategory}
          >
            <MenuItem value={0}>Выбрать категорию</MenuItem>
            {categories.map(category => (
              <MenuItem key={category.id} value={category.id}>{category.title}</MenuItem>
            ))}
          </Select>
        </FormControl>
        <FormControl className={classes.formControl}>
          <TextField
            label="Поиск по названию"
            onChange={handleChangeTitle}
          />
        </FormControl>
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Артикул</TableCell>
                <TableCell>Название</TableCell>
                <TableCell>Количество</TableCell>
                <TableCell>Цена</TableCell>
                <TableCell>Магазин</TableCell>
                <TableCell>Категория</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {products
                .filter(row => shop !== 0
                  ? row.shopId === shop
                  : true)
                .filter(row => category !== 0
                  ? row.categoryId === category
                  : true)
                .filter(row => title !== ''
                  ? row.title.toLowerCase().includes(title.toLowerCase())
                  : true)
                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                .map(row => (
                <TableRow key={row.id}>
                  <TableCell>{row.code}</TableCell>
                  <TableCell>{row.title}</TableCell>
                  <TableCell>{row.amount}</TableCell>
                  <TableCell>{row.price}</TableCell>
                  <TableCell>{row.shop}</TableCell>
                  <TableCell>{row.category}</TableCell>
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
            .filter(row => category !== 0
              ? row.categoryId === category
              : true)
            .filter(row => title !== ''
              ? row.title.toLowerCase().includes(title.toLowerCase())
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