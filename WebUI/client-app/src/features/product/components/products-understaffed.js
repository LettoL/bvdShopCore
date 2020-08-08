import React from 'react'

import Paper from '@material-ui/core/Paper'
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { useStore } from 'effector-react';
import { $currentProduct } from '../models/product-info-store';

export const ProductsUnderstaffed = () => {

  const understaffed = useStore($currentProduct).understaffed || [];

  return (
    <TableContainer component={Paper}>
      <Table aria-label="simple table">
        <TableHead>
          <TableRow>
            <TableCell>Id</TableCell>
            <TableCell align="right">Название</TableCell>
            <TableCell align="right">Магазин</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {understaffed.map((row) => (
            <TableRow key={row.id}>
              <TableCell component="th" scope="row">
                {row.id}
              </TableCell>
              <TableCell align="right">{row.title}</TableCell>
              <TableCell align="right">{row.shopTitle}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  )
}