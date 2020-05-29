import React, {useCallback, useEffect, useState} from 'react'
import {useHttp} from "../../hooks/http.hook";
import {Constants} from "../../const";
import {Products} from "../../components/admin-products/products";

const API = Constants.API

export const ProductsPage = () => {
  const {loading, request} = useHttp()
  const [products, setProducts] = useState([])
  const [shops, setShops] = useState([])
  const [categories, setCategories] = useState([])

  const getProducts = useCallback(async () => {
    try {
      const data = await request(API + 'api/products')
      setProducts(data)
    }
    catch (e) {}
  }, [request])

  const getShops = useCallback(async () => {
    try {
      const data = await request(API + 'api/shops')
      setShops(data)
    }
    catch (e) {}
  }, [request])

  const getCategories = useCallback(async () => {
    try {
      const data = await request(API + 'api/categories')
      setCategories(data)
    }
    catch (e) {
    }
  }, [request])

  useEffect(() => {
    getProducts()
    getShops()
    getCategories()
  }, [getProducts, getShops, getCategories])

  if(loading)
    return (
      <div>Загрузка...</div>
    )
  else
    return (
      <Products
        shops={shops}
        categories={categories}
        products={products}
      />
    )
}