import React, {useState} from "react";
import TableCell from "@material-ui/core/TableCell";
import TableRow from "@material-ui/core/TableRow";
import IconButton from "@material-ui/core/IconButton";
import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import Collapse from "@material-ui/core/Collapse";
import {Box} from "@material-ui/core";
import Grid from "@material-ui/core/Grid";
import Typography from "@material-ui/core/Typography";

export const ArchiveSaleListItem = props => {
  const {sale} = props
  const [open, setOpen] = useState(false)

  const detailOpen = () => {
    setOpen(!open)
  }

  return (
    <>
      <TableRow>
        <TableCell>{sale.number}</TableCell>
        <TableCell>{sale.date}</TableCell>
        <TableCell>{sale.sum}</TableCell>
        <TableCell>{sale.margin}</TableCell>
        <TableCell>{sale.procurementCost}</TableCell>
        <TableCell>{sale.shopTitle}</TableCell>
        <TableCell>{sale.saleType}</TableCell>
        <TableCell>{sale.discount}</TableCell>
        <TableCell>{sale.buyer}</TableCell>
        <TableCell>
          <IconButton aria-lavel="expand row" size='small' onClick={detailOpen}>
            {open ? <KeyboardArrowUpIcon/> : <KeyboardArrowDownIcon/>}
          </IconButton>
        </TableCell>
      </TableRow>
      <TableRow>
        <TableCell style={{paddingBottom: 0, paddingTop: 0}} colSpan={10}>
          <Collapse in={open} timeout={'auto'} unmountOnExit>
            <Box margin={1}>
              <Grid container>
                <Grid item xs={3}>
                  {sale.payments.map(payment => (
                    <>
                      <span>Счёт: {payment.account} | </span>
                      <span>Сумма: {payment.sum}</span>
                      <br/>
                    </>
                  ))}
                  <span>
                    Дата удаления: {sale.deletedDate !== null ? sale.deletedDate : "Не указана" }
                  </span>
                  <br/>
                </Grid>
                <Grid item xs={9}>
                  {sale.products.map(product => (
                    <>
                      <span>Название: {product.title} | </span>
                      <span>Код: {product.code} | </span>
                      <span>Кол-во: {product.amount} | </span>
                      <span>Цена: {product.price} | </span>
                      <span>Закупочная: {product.procurementCost} | </span>
                      <span>Поставщик: {product.supplierName}</span>
                      <br/>
                    </>
                  ))}
                </Grid>
              </Grid>
            </Box>
          </Collapse>
        </TableCell>
      </TableRow>
    </>
  )
}