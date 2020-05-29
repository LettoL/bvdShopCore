import React, {useEffect} from "react";
import {fetchShopsFx, shops} from "../../store/shop-store";
import {useStore} from "effector-react";
import {ShopCreate} from "./shop-create";

export const ShopsList = () => {

  useEffect(() => {
    fetchShopsFx()
  }, [])

  const shopsData = useStore(shops)

  return(
    <>
      <ShopCreate/>
      <ul>
        {shopsData.map(shop => (
          <li>{shop.id} / {shop.title}</li>
        ))}
      </ul>
    </>
  )
}