import React, {useState} from "react";
import {makeStyles} from "@material-ui/core/styles";
import {SupplyProductItem} from "./supply-product-item";
import Button from "@material-ui/core/Button";
import Grid from '@material-ui/core/Grid';
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";


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
  const [age, setAge] = React.useState('');

  const handleChange = (event) => {
    setAge(event.target.value);
  };

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
    <Grid container item xs={12} spacing={1}>
    <div>
      <FormControl className={classes.formControl}>
        <InputLabel id="demo-simple-select-label">Какахи</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={age}
          onChange={handleChange}
        >
          <MenuItem value={10}>Бибиби</MenuItem>
          <MenuItem value={20}>Какака</MenuItem>
          <MenuItem value={30}>Кукуку</MenuItem>
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <InputLabel id="demo-simple-select-label">Писи</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={age}
          onChange={handleChange}
        >
          <MenuItem value={10}>Ten</MenuItem>
          <MenuItem value={20}>Twenty</MenuItem>
          <MenuItem value={30}>Thirty</MenuItem>
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <InputLabel id="demo-simple-select-label">Каки</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={age}
          onChange={handleChange}
        >
          <MenuItem value={10}>Ten</MenuItem>
          <MenuItem value={20}>Twenty</MenuItem>
          <MenuItem value={30}>Thirty</MenuItem>
        </Select>
      </FormControl>
      </div>
      </Grid>
      <div className={"margin-top-40"}>
      <Grid container item xs={12} spacing={2}>
      {supplyProducts.map((supplyProduct, index) => (
        <SupplyProductItem 
          key={index}
          supplyProduct={supplyProduct}
          changeFormHandler={changeFormHandler}
          index={index}
        />
      ))}
      
      
      <Grid container item xs={6} spacing={0}
      direction="row"
      justify="space-between"
      alignItems="baseline">
        <Button 
          variant="contained" 
          size="large" 
          color="primary" 
          className={classes.margin}
          onClick={addSupplyProduct}
        >
          Добавить товар
        </Button>
        
        <Button 
          variant="contained" 
          size="large" 
          color="primary" 
          className={classes.margin}
          onClick={saveForm}
          >
          Сохранить
        </Button>
      </Grid>
      </Grid>
      </div>
    </>
  )
} 

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  },
}));