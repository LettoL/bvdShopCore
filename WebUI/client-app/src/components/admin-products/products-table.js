import React from "react";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import Button from "@material-ui/core/Button";
import TableContainer from "@material-ui/core/TableContainer";

export const ProductsTable = props => {
  const {products, page, rowsPerPage, handleOpenDialog} = props

  return (
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
            <TableCell>Действия</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {products
          .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
          .map(row => (
            <TableRow key={row.id}>
              <TableCell>{row.code}</TableCell>
              <TableCell>{row.title}</TableCell>
              <TableCell>{row.amount}</TableCell>
              <TableCell>{row.price}</TableCell>
              <TableCell>{row.shop}</TableCell>
              <TableCell>{row.category}</TableCell>
              <TableCell>
                <Button variant="contained" onClick={handleOpenDialog.bind(this, row)}>
                  Detail
                </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  )
}