import React from "react";
import ListItemText from "@material-ui/core/ListItemText";
import ListItem from "@material-ui/core/ListItem";

export const ProductListItem = props => {

  const {title} = props

  return (
    <ListItem button>
      <ListItemText primary={title}/>
    </ListItem>
  )
}