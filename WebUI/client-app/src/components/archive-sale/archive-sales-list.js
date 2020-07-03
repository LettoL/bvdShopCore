import React from "react";
import TableContainer from "@material-ui/core/TableContainer";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";

export const ArchiveSalesList = () => {

  const sales = []

  return (
    <TableContainer>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Номер</TableCell>
            <TableCell>Дата</TableCell>
            <TableCell>Сумма</TableCell>
            <TableCell>Себестоимость</TableCell>
            <TableCell>Магазин</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {sales
            .map(row => (
              <TableRow key={row.id}>
                <TableCell>{row.id}</TableCell>
                <TableCell>{row.date}</TableCell>
                <TableCell>{row.sum}</TableCell>
                <TableCell>{row.procurementCost}</TableCell>
                <TableCell>{row.shopTitle}</TableCell>ыф
              </TableRow>
            ))}
        </TableBody>
      </Table>
    </TableContainer>
  )
}