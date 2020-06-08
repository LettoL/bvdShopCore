import React, {useEffect, useState} from "react";
import {fetchSupplyProductsFx, supplyProducts} from "../../../store/supply-product-store";
import {useStore} from "effector-react";
import TableContainer from "@material-ui/core/TableContainer";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import TablePagination from "@material-ui/core/TablePagination";
import FormControl from "@material-ui/core/FormControl";
import TextField from "@material-ui/core/TextField";

export const SupplyProductsList = () => {

  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [minProcurementCost, setMinProcurementCost] = useState(0)

  useEffect(() => {
    fetchSupplyProductsFx()
  }, [])

  const data = useStore(supplyProducts)

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  return(
    <>
      <FormControl>
        <TextField
          label="Закупочная стоимость"
          defaultValue="0"
          onChange={event => setMinProcurementCost(event.target.value)}
        />
      </FormControl>
      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell>Товар</TableCell>
              <TableCell>Закупочная цена</TableCell>
              <TableCell>Кол-во на складе</TableCell>
              <TableCell>Поставленное кол-во</TableCell>
              <TableCell>Магазин</TableCell>
              <TableCell>Поставщик</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {data
              .filter(row => row.procurementCost >= minProcurementCost)
              //.sort((a, b) => (b.procurementCost - a.procurementCost))
              .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
              .map(row => (
                <TableRow key={row.supplyProductId}>
                  <TableCell>{row.supplyProductId}</TableCell>
                  <TableCell>{row.productTitle}</TableCell>
                  <TableCell>{row.procurementCost}</TableCell>
                  <TableCell>{row.shopAmount}</TableCell>
                  <TableCell>{row.suppliedAmount}</TableCell>
                  <TableCell>{row.shopTitle}</TableCell>
                  <TableCell>{row.supplierName}</TableCell>
                </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <TablePagination
        rowsPerPageOptions={[10, 25, 50, 100]}
        component='div'
        count={data
          .filter(row => row.procurementCost >= minProcurementCost)
          .length}
        rowsPerPage={rowsPerPage}
        page={page}
        onChangePage={handleChangePage}
        onChangeRowsPerPage={handleChangeRowsPerPage}
      />
    </>
  )
}