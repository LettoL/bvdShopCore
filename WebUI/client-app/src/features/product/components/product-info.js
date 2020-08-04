import React from 'react'
import Card from '@material-ui/core/Card'
import CardContent from '@material-ui/core/CardContent'
import Typography from '@material-ui/core/Typography'
import {makeStyles} from '@material-ui/core/styles'
import Divider from '@material-ui/core/Divider'

export const ProductInfo = () => {
  const classes = useStyles()

  return(
    <Card>
      <CardContent>
        <Typography color={'textSecondary'} gutterBottom className={classes.title}>
          <strong>Подробно о товаре: (Название товара)</strong>
        </Typography>
        <Divider/>
        <Typography className={classes.title} color={'textSecondary'} gutterBottom>
          <strong>Id: 1</strong>
          <br/>
          <strong>Название: Name</strong>
          <br/>
          <strong>Артикул: Code</strong>
          <br/>
          <strong>Цена: Price</strong>
          <br/>
          <strong>Категория: Category</strong>
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