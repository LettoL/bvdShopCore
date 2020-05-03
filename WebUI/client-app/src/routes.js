import React from "react";
import {Switch, Route, Redirect} from 'react-router-dom'
import {ImportPage} from "./pages/admin/import.page";
import {ProductsPage} from "./pages/admin/products.page";
import {ProductsHistory} from "./pages/admin/products-history.page";
import {ManagersPage} from "./pages/admin/managers.page";

export const useRoutes = () => {

  return (
    <Switch>
      <Route path='/import' exact>
        <ImportPage/>
      </Route>
      <Route path='/products' exact>
        <ProductsPage/>
      </Route>
      <Route path='/productsHistory' exact>
        <ProductsHistory/>
      </Route>
      <Route path='/managers' exact>
        <ManagersPage/>
      </Route>
      <Redirect to='/import'/>
    </Switch>
  )
}