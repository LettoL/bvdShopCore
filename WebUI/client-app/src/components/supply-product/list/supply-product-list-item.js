import React from "react";
import TableCell from "@material-ui/core/TableCell";
import TableRow from "@material-ui/core/TableRow";

export const SupplyProductListItem = props => {

  const {supplyProduct} = props

  return(
    <TableRow>
      <TableCell>{supplyProduct.supplyProductId}</TableCell>
      <TableCell>{supplyProduct.productTitle}</TableCell>
      <TableCell>{supplyProduct.procurementCost}</TableCell>
      <TableCell>{supplyProduct.shopAmount}</TableCell>
      <TableCell>{supplyProduct.suppliedAmount}</TableCell>
      <TableCell>{supplyProduct.shopTitle}</TableCell>
      <TableCell>{supplyProduct.supplierName}</TableCell>
    </TableRow>
  )
}