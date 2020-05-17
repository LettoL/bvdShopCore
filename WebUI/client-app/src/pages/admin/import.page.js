import React, {useCallback, useEffect, useState} from "react";
import {useHttp} from "../../hooks/http.hook";
import {Constants} from "../../const";
import {Form} from "../../components/import-form/form";

const API = Constants.API

export const ImportPage = () => {
  const {request} = useHttp()

  const [shops, setShops] = useState([])
  const [categories, setCategories] = useState([])
  const [suppliers, setSuppliers] = useState([])

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
    catch (e) {}
  }, [request])

  const getSuppliers = useCallback(async () => {
    try {
      const data = await request(API + 'api/suppliers')
      setSuppliers(data)
    }
    catch (e) {}
  }, [request])

  useEffect(() => {
    getShops()
    getCategories()
    getSuppliers()
  }, [getShops, getCategories, getSuppliers])

  const saveForm = async (data) => {
    try {
      const response = await request(API + 'api/supplyProducts/import', 'POST', {...data})
      console.log({...data})
      console.log(response)
    }
    catch (e) { }
  }
  
  return (
    <Form
      shops={shops}
      suppliers={suppliers}
      categories={categories}
      saveForm={saveForm}
    />
  )
}