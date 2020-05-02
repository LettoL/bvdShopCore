import React, {useState} from "react";
import {useHttp} from "../../hooks/http.hook";
import {Constants} from "../../const";
import {Form} from "../../components/import-form/form";

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
  
  return (
    <Form
      products={products}
      productsText={form.products}
      productsTextChange={changeHandler}
      productsTextHandle={productsTextHandle}
    />
  )
}