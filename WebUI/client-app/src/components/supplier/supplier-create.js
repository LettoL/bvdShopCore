import React, {useState} from "react";
import {createSupplierFx} from "../../store/supplier-store";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";

export const SupplierCreate = () => {

  const [name, setName] = useState('')
  const [phone, setPhone] = useState('')
  const [email, setEmail] = useState('')
  const [disabled, setDisabled] = useState(false)

  const saveHandler = () => {
    setDisabled(true)
    createSupplierFx(name, phone, email)

    setName('')
    setDisabled(false)
  }

  return(
    <>
      <TextField
        label='Имя поставщика'
        value={name}
        onChange={event => setName(event.target.value)}
        name='name'
      />
      <TextField
        label='Телефон'
        value={phone}
        onChange={event => setPhone(event.target.value)}
        name='phone'
      />
      <TextField
        label='Почта'
        value={email}
        onChange={event => setEmail(event.target.value)}
        name='email'
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