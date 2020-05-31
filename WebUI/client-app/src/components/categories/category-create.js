import React, {useState} from "react";

import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import { createCategoryFx } from "../../store/category-store";

export const CategoryCreate = () => {

  const [title, setTitle] = useState('')
  const [disabled, setDisabled] = useState(false)

  const saveHandler = () => {
    setDisabled(true);
    createCategoryFx(title);

    setTitle('');
    setDisabled(false);
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