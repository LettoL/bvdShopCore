import React, {useState} from "react";
import TextField from "@material-ui/core/TextField";
import FormControl from "@material-ui/core/FormControl";
import {makeStyles} from "@material-ui/core/styles";
import Select from "@material-ui/core/Select";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import {SupplyProductItem} from "./supply-product-item";
import Button from "@material-ui/core/Button";

export const SupplyProductForm = () => {
  const classes = useStyles()

  const [supplyProducts, setSupplyProducts] = useState([
    {
      productId: 0,
      amount: 0,
      shopId: 0,
      supplierId: 0,
      procurementCost: 0
    }
  ])

  const changeFormHandler = (index, event) => {
    const supplyProduct = supplyProducts[index]
    const changeItemState = {...supplyProduct, [event.target.name]: event.target.value}

    setSupplyProducts([
      ...supplyProducts.slice(0, index),
      changeItemState,
      ...supplyProducts.slice(index + 1)
    ])
  }

  const addSupplyProduct = () => {
    setSupplyProducts([...supplyProducts, {
      productId: 0,
      amount: 0,
      shopId: 0,
      supplierId: 0,
      procurementCost: 0
    }])
  }

  const saveForm = () => {
    console.log(supplyProducts)
  }

  return(
    <>
      {supplyProducts.map((supplyProduct, index) => (
        <SupplyProductItem
          key={index}
          supplyProduct={supplyProduct}
          changeFormHandler={changeFormHandler}
          index={index}
        />
      ))}
      <Button
        onClick={addSupplyProduct}
      >
        Добавить товар
      </Button>
      <Button
        onClick={saveForm}
      >
        Сохранить
      </Button>
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  }
}));