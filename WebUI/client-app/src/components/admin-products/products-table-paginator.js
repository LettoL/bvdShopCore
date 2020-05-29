import React from "react";
import TablePagination from "@material-ui/core/TablePagination";

export const ProductsTablePaginator = props => {
  const {products, shop, category, title,
    rowsPerPage, page, handleChangePage, handleChangeRowsPerPage} = props

  return (
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
  )
}