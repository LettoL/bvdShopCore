import React from "react";
import Grid from "@material-ui/core/Grid";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import {ProductListItem} from "./product-list-item";
import TextField from "@material-ui/core/TextField";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";

export const Product = () => {

  const products = ['product1', 'product2', 'product3']
  const uncomplects = [
    {supplierTitle: 'supplier1', amount: 2, comment: 'adsf'},
    {supplierTitle: 'supplier1', amount: 1, comment: 'asdfadsfasdfasdf'},
    {supplierTitle: 'supplier2', amount: 2, comment: 'asdfa'},
  ]

  return (
    <Grid container>
      <Grid item xs={10}>
        <div>
          <FormControl>
            <InputLabel>Поставщик</InputLabel>
            <Select>
              <MenuItem value={10}>Supplier1</MenuItem>
              <MenuItem value={20}>Supplier2</MenuItem>
            </Select>
          </FormControl>
        </div>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Поставщик</TableCell>
              <TableCell>Количество</TableCell>
              <TableCell>Комментарий</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {uncomplects.map(uncomplect => (
              <TableRow>
                <TableCell>{uncomplect.supplierTitle}</TableCell>
                <TableCell>{uncomplect.amount}</TableCell>
                <TableCell>{uncomplect.comment}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Grid>
      <Grid item xs={2}>
        <TextField variant='outlined' label='Название '/>
        <List>
          {products.map(product => (
            <ProductListItem title={product}/>
          ))}
        </List>
      </Grid>
    </Grid>
  )
}