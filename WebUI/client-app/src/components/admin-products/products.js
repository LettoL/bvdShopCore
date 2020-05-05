import React, {useState} from "react";
import {ProductTableFilters} from "./product-table-filters";
import {ProductsTable} from "./products-table";
import {ProductsTablePaginator} from "./products-table-paginator";

export const Products = props => {
  const {shops, categories, products} = props

  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [shop, setShop] = useState(0);
  const [category, setCategory] = useState(0);
  const [title, setTitle] = useState('');

  const [openDialog, setOpenDialog] = useState(false)

  const handleOpenDialog = () => {
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
    </>
  )
}