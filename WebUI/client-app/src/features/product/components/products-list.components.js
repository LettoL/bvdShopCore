import React from 'react'
import TextField from '@material-ui/core/TextField'
import Button from '@material-ui/core/Button'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'

export const ProductsList = () => {
  return (
    <>
      <TextField label='Поиск товара' variant='outlined'/>
      <List>
        <ListItem>
          <Button>Product</Button>
        </ListItem>
        <ListItem>
          <Button>Product</Button>
        </ListItem>
        <ListItem>
          <Button>Product</Button>
        </ListItem>
        <ListItem>
          <Button>Product</Button>
        </ListItem>
      </List>
    </>
  )
}