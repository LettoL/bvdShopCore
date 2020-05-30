import React, {useEffect} from "react";
import {fetchSuppliersFx, suppliers} from "../../store/supplier-store";
import {useStore} from "effector-react";
import {SupplierCreate} from "./supplier-create";

export const SuppliersList = () => {

  useEffect(() => {
    fetchSuppliersFx()
  }, [])

  const suppliersData = useStore(suppliers)

  return(
    <>
      <SupplierCreate/>
      <ul>
        {suppliersData.map(supplier => (
          <li>{supplier.id} / {supplier.name}</li>
        ))}
      </ul>
    </>
  )
}