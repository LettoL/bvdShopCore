import React, {useEffect} from "react";
import TableContainer from "@material-ui/core/TableContainer";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import {useStore} from "effector-react";
import {$deletedSales, fetchDeletedSalesFx} from "../../models/archive-sale/archive-sale.store";
import {ArchiveSaleListItem} from "./archive-sale-list-item";

export const ArchiveSalesList = () => {

  const sales = useStore($deletedSales)

  useEffect(() => {
    fetchDeletedSalesFx()
  }, [])

  return (
    <TableContainer>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Номер</TableCell>
            <TableCell>Дата</TableCell>
            <TableCell>Сумма</TableCell>
            <TableCell>Маржа</TableCell>
            <TableCell>Себестоимость</TableCell>
            <TableCell>Магазин</TableCell>
            <TableCell>Тип</TableCell>
            <TableCell>Скидка</TableCell>
            <TableCell>Покупатель</TableCell>
            <TableCell></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {sales
            .map(row => (
              <ArchiveSaleListItem
                key={row.number}
                sale={row}
              />
            ))}
        </TableBody>
      </Table>
    </TableContainer>
  )
}