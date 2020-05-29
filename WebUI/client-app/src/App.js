import React, {useCallback, useState} from 'react';
import {BrowserRouter} from 'react-router-dom'
import {useRoutes} from "./routes";
import {AdminLayout} from "./components/layouts/admin.layout";
import {AuthContext} from "./contexts/auth.context";
import {useCookies} from "react-cookie";

function App() {
  const routes = useRoutes()
  const [cookie] = useCookies()

  const userId = cookie.userId
  const userRole = 'Administrator'//cookie.userRole
  const isAuth = !!userId

  if(userRole === 'Administrator')
    return (
      <AuthContext.Provider value={{
        userId, userRole, isAuth
      }}>
        <BrowserRouter>
          <AdminLayout page={routes} />
        </BrowserRouter>
      </AuthContext.Provider>
    )

  if(userRole === 'Manager')
    return (
      <h1>Manager</h1>
    )

  return (
    <h1>Нет авторизации</h1>
  )

}

export default App;
