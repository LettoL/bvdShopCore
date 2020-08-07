import React, { useEffect } from 'react'
import Card from '@material-ui/core/Card'
import CardContent from '@material-ui/core/CardContent'
import Typography from '@material-ui/core/Typography'
import {makeStyles} from '@material-ui/core/styles'
import Divider from '@material-ui/core/Divider'
import { productInfo, fetchProductInfoFx } from '../../../store/product-info-store'
import { useStore } from 'effector-react'

export const ProductInfo = () => {

  useEffect(() => {
    fetchProductInfoFx()
  }, [])
  
  const product = useStore(productInfo)

  const classes = useStyles()

  return(
    <Card>
      <CardContent>
        <Typography color={'textSecondary'} gutterBottom className={classes.title}>
          <strong>Подробно о товаре: {product.title}</strong>
        </Typography>
        <Divider/>
        <Typography className={classes.title} color={'textSecondary'} gutterBottom>
          <strong>Id: {product.id}</strong>
          <br/>
          <strong>Название: {product.title}</strong>
          <br/>
          <strong>Артикул: {product.code}</strong>
          <br/>
          <strong>Цена: {product.price}</strong>
          <br/>
          <strong>Категория: {product.category}</strong>
        </Typography>
      </CardContent>
    </Card>
  )
}

const useStyles = makeStyles({
  title: {
    fontSize: 14
  }
})