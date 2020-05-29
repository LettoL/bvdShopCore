import React from "react";
import {Switch, Route, Redirect} from 'react-router-dom'
import {ImportPage} from "./pages/admin/import.page";
import {ProductsPage} from "./pages/admin/products.page";
import {ProductsHistory} from "./pages/admin/products-history.page";
import {ManagersPage} from "./pages/admin/managers.page";
import {SupplyProductForm} from "./components/supply-product/form/supply-product-form";
import {ShopsList} from "./components/shop/shops-list";

export const useRoutes = () => {

  return (
    <Switch>
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
        <ManagersPage/>
      </Route>
      <Route path='/admin/shops' exact>
        <ShopsList/>
      </Route>
      <Redirect to='/admin/import'/>
    </Switch>
  )
}