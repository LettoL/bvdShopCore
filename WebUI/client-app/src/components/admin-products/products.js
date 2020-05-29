import React, {useState} from "react";
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

export const Products = props => {
  const classes = useStyles()

  const {shops, categories, products} = props

  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [shop, setShop] = useState(0);
  const [category, setCategory] = useState(0);
  const [title, setTitle] = useState('');

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

  const handleChangeShop = (event) => {
    setShop(event.target.value);
  };

  const handleChangeCategory = event => {
    setCategory(event.target.value)
  }

  const handleChangeTitle = event => {
    setTitle(event.target.value)
  }

  return (
    <>
      <ProductTableFilters
        shops={shops}
        shop={shop}
        handleChangeShop={handleChangeShop}
        categories={categories}
        category={category}
        handleChangeCategory={handleChangeCategory}
        handleChangeTitle={handleChangeTitle}
      />
      <ProductsTable
        products={products}
        shop={shop}
        category={category}
        title={title}
        page={page}
        rowsPerPage={rowsPerPage}
        handleOpenDialog={handleOpenDialog}
      />
      <ProductsTablePaginator
        products={products}
        shop={shop}
        category={category}
        title={title}
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