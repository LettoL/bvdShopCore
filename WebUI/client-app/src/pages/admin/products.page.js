import React, {useCallback, useEffect, useState} from 'react'
import {useHttp} from "../../hooks/http.hook";
import {Constants} from "../../const";
import {Products} from "../../components/admin-products/products";
import {useStore} from "effector-react";
import {$filteredProducts, fetchProductsFx} from "../../models/product/product.store";

const API = Constants.API

export const ProductsPage = () => {
  const {loading, request} = useHttp()
  const products = useStore($filteredProducts)

  useEffect(() => {
    fetchProductsFx()
  }, [])

  if(loading)
    return <div>Загрузка...</div>
  else
    return <Products/>
}