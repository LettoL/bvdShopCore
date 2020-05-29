import React, {useState} from "react";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import {saveShopFx} from "../../store/shop-store";

export const ShopCreate = () => {

  const [title, setTitle] = useState('')
  const [disabled, setDisabled] = useState(false)

  const saveHandler = () => {
    setDisabled(true)
    saveShopFx(title)

    setTitle('')
    setDisabled(false)
  }

  return(
    <>
      <TextField
        label='Название'
        value={title}
        onChange={event => setTitle(event.target.value)}
        name='title'
      />
      <Button
        onClick={saveHandler}
        variant='contained'
        color='primary'
        disabled={disabled}
      >
        Сохранить
      </Button>
    </>
  )
}