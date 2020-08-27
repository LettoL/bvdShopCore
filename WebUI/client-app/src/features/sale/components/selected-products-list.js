import React from "react";
import {useStore} from "effector-react";
import {$saleProducts} from "../models/sale-create";
import {SelectedProductItem} from "./selected-product-item";

export const SelectedProductsList = () => {

  const saleProducts = useStore($saleProducts)

  return (
    <>
      {saleProducts.map(product => (
        <SelectedProductItem key={product.id} product={product}/>
      ))}
    </>
  )
}