import React from "react";
import {Switch, Route, Redirect} from 'react-router-dom'
import {ImportPage} from "./pages/admin/import.page";
import {ProductsPage} from "./pages/admin/products.page";
import {ProductsHistory} from "./pages/admin/products-history.page";
import {SupplyProductForm} from "./components/supply-product/form/supply-product-form";
import {ShopsList} from "./components/shop/shops-list";
import {SuppliersList} from "./components/supplier/suppliers-list";
import { CategoriesList } from "./components/categories/categories-list";
import {SupplyProductsList} from "./components/supply-product/list/supply-products-list";
import {ManagerList} from "./components/manager/manager-list";
import {ArchiveSalesList} from "./components/archive-sale/archive-sales-list";
import {Product} from './features/product/components/product.page'
import { SaleCreate } from "./features/sale/components/sale-create";

export const useRoutes = () => {

  return (
    <Switch>
      <Route path='/admin/product' exact>
        <Product/>
      </Route>
      <Route path='/admin/import' exact>
        <ImportPage/>
      </Route>
      <Route path='/admin/supply' exact>
        <SupplyProductForm/>
      </Route>
      <Route path='/admin/products' exact>
        <ProductsPage/>
      </Route>
      <Route path='/admin/productsHistory' exact>
        <ProductsHistory/>
      </Route>
      <Route path='/admin/managers' exact>
        <ManagerList/>
      </Route>
      <Route path='/admin/shops' exact>
        <ShopsList/>
      </Route>
      <Route path='/admin/suppliers' exact>
        <SuppliersList/>
      </Route>
      <Route path='/admin/categories' exact>
        <CategoriesList/>
      </Route>
      <Route path='/admin/supplyProductsList' exact>
        <SupplyProductsList/>
      </Route>
      <Route path='/admin/archiveSales' exact>
        <ArchiveSalesList/>
      </Route>
      <Route path='/manager/saleCreate' exact>
        <SaleCreate/>
      </Route>
      <Redirect to='/admin/product'/>
    </Switch>
  )
}