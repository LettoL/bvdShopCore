import React, {useEffect, useState} from "react";
import {ProductTableFilters} from "./product-table-filters";
import {ProductsTable} from "./products-table";
import {ProductsTablePaginator} from "./products-table-paginator";
import Dialog from "@material-ui/core/Dialog";
import DialogTitle from "@material-ui/core/DialogTitle";
import TextField from "@material-ui/core/TextField";
import DialogContent from "@material-ui/core/DialogContent";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import InputLabel from "@material-ui/core/InputLabel";
import {makeStyles} from "@material-ui/core/styles";
import FormControl from "@material-ui/core/FormControl";
import {
  $filteredProducts, fetchProductsBySupplierFx,
  fetchProductsFx
} from "../../models/product-table/product.store";
import {useStore} from "effector-react";
import {CSVLink} from "react-csv";

export const Products = () => {
  const classes = useStyles()

  const products = useStore($filteredProducts)
  const productsLoading = useStore(fetchProductsBySupplierFx.pending)

  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  const [openDialog, setOpenDialog] = useState(false)
  const [detailProduct, setDetailProduct] = useState(null)

  const handleOpenDialog = (product) => {
    setDetailProduct(product)
    setOpenDialog(true)
  }

  const handleCloseDialog = () => {
    setOpenDialog(false)
  }

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  useEffect(() => {
    fetchProductsFx()
  }, [])

  const headers = [
    {label: "Артикул", key: 'code'},
    {label: "Наименование", key: 'title'},
    {label: "Кол-во", key: 'amount'},
    {label: "Забронировано", key: 'bookedCount'},
    {label: "Цена", key: 'price'},
    {label: "Себестоимость", key: 'primeCost'},
    {label: "Магазин", key: 'shop'},
    {label: "Категория", key: 'category'}
  ]

  const data = products.map(product => (
    {
      code: product.code,
      title: product.title,
      amount: product.amount,
      bookedCount: product.bookedCount,
      price: (product.price + '').replace('.', ','),
      primeCost: (product.primeCost + '').replace('.', ','),
      shop: product.shop,
      category: product.category
    }
  ));

  console.log(productsLoading)

  return (
    <>
      <CSVLink data={data} headers={headers} separator={';'}>
        Download me
      </CSVLink>;
      <ProductTableFilters/>
      {productsLoading
        ? "Загрузка"
        : <ProductsTable
        products={products}
        page={page}
        rowsPerPage={rowsPerPage}
        handleOpenDialog={handleOpenDialog}
        />
      }
      <ProductsTablePaginator
        products={products}
        rowsPerPage={rowsPerPage}
        page={page}
        handleChangePage={handleChangePage}
        handleChangeRowsPerPage={handleChangeRowsPerPage}
      />
      <Dialog open={openDialog} onClose={handleCloseDialog}>
        <DialogTitle>Product</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            label='Название'
            type='text'
            margin='dense'
          />
          <TextField
            label='Артикул'
            type='text'
            margin='dense'
          />
          <TextField
            label='Стоимость для продажи'
            type='number'
            margin='dense'
          />
          <FormControl className={classes.formControl}>
            <InputLabel id='category-select'>Категория</InputLabel>
            <Select
              labelId='category-select'
            >
              <MenuItem>1111</MenuItem>
            </Select>
          </FormControl>
          <FormControl className={classes.formControl}>
            <InputLabel id='shop-select'>Магазин</InputLabel>
            <Select
              labelId='shop-select'
            >
              <MenuItem>1111</MenuItem>
            </Select>
          </FormControl>
        </DialogContent>
      </Dialog>
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  }
}));