import React from "react";
import TablePagination from "@material-ui/core/TablePagination";

export const ProductsTablePaginator = props => {
  const {products, rowsPerPage, page, handleChangePage, handleChangeRowsPerPage} = props

  return (
    <TablePagination
      rowsPerPageOptions={[10, 25, 50, 100]}
      component='div'
      count={products.length}
      rowsPerPage={rowsPerPage}
      page={page}
      onChangePage={handleChangePage}
      onChangeRowsPerPage={handleChangeRowsPerPage}
    />
  )
}