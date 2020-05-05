import React from "react";
import {Switch, Route, Redirect} from 'react-router-dom'
import {ImportPage} from "./pages/admin/import.page";
import {ProductsPage} from "./pages/admin/products.page";
import {ProductsHistory} from "./pages/admin/products-history.page";
import {ManagersPage} from "./pages/admin/managers.page";

export const useRoutes = () => {

  return (
    <Switch>
      <Route path='/admin/import' exact>
        <ImportPage/>
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
      <Redirect to='/admin/import'/>
    </Switch>
  )
}