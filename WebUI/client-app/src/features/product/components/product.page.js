import React from 'react'
import Grid from '@material-ui/core/Grid'
import {ProductsList} from './products-list.components'
import {ProductInfo} from './product-info'
import {ProductTabs} from './product-tabs.component'

export const Product = () => {

  return(
    <Grid container>
      <Grid item xs={10}>
        <ProductInfo/>
        <ProductTabs/>
      </Grid>
      <Grid item xs={2}>
        <ProductsList/>
      </Grid>
    </Grid>
  )
}