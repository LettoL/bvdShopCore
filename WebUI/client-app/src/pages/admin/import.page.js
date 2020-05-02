import React, {useState} from "react";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import {useHttp} from "../../hooks/http.hook";
import {Constants} from "../../const";

const API = Constants.API

export const ImportPage = () => {
  const {request} = useHttp()
  const [products, setProducts] = useState([])
  
  const [form, setForm] = useState({
    products: ''
  })
  
  const changeHandler = event => {
    setForm({...form, [event.target.name]: event.target.value})
  }
  
  const productsTextHandle = async () => {
    const productsText = form.products.split('\n');
    const productsProperties = productsText.map(x => x.split(';'));
    
    const products = productsProperties.map(x => {
      return {
        number: +(x[0].replace(',', '.').replace(/\s+/g,'')),
        code: x[1],
        title: x[2],
        amount: +(x[3].replace(',', '.').replace(/\s+/g,'')),
        price: +(x[5].replace(',', '.').replace(/\s+/g,'')),
        sum: +(x[6].replace(',', '.').replace(/\s+/g,''))
      }
    });

    setProducts(products)
    
    /*const data = await request(API + 'api/supplyProducts/import', 'POST', {
      products: products
    })*/
  }

  console.log(products)
  
  return (
    <div>
      <br/>
      <TextField
        label="Внесите товары"
        multiline
        rows={10}
        variant="outlined"
        value={form.products}
        onChange={changeHandler}
        name="products"
        fullWidth
      />
      <br/>
      <Button 
        variant="contained" 
        color="primary"
        onClick={productsTextHandle}
      >
        Обработать
      </Button>
      <ul>
        {products.map(product => (
          <li key={product.number}>
            {product.number}
            {product.code}
            {product.title}
            {product.amount}
            {product.price}
            {product.sum}
          </li>
        ))}
      </ul>
    </div>
  )
}