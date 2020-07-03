import React from "react";
import ListItem from "@material-ui/core/ListItem";
import {NavLink} from "react-router-dom";
import ListItemText from "@material-ui/core/ListItemText";
import List from "@material-ui/core/List";

export const Menu = () => {

  return (
    <List>
      <ListItem>
        <a href='/'>
          Старый интерфейс
        </a>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/import'}>
          <ListItemText primary={'Импорт'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/supplyProductsList'}>
          <ListItemText primary={'Список поставок'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/managers'}>
          <ListItemText primary={'Менеджеры'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/archiveSales'}>
          <ListItemText primary={'Архивные продажи'}/>
        </NavLink>
      </ListItem>
    </List>
  )

  /*return (
    <List>
      <ListItem>
        <a href='/'>
          Старый интерфейс
        </a>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/import'}>
          <ListItemText primary={'Импорт'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/supply'}>
          <ListItemText primary={'Поставка'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/supplyProductsList'}>
          <ListItemText primary={'Список поставок'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/managers'}>
          <ListItemText primary={'Менеджеры'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/products'}>
          <ListItemText primary={'Все товары'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/productsHistory'}>
          <ListItemText primary={'История товаров'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/shops'}>
          <ListItemText primary={'Магазины'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/suppliers'}>
          <ListItemText primary={'Поставщики'}/>
        </NavLink>
      </ListItem>
      <ListItem button>
        <NavLink to={'/admin/categories'}>
          <ListItemText primary={'Категории'}/>
        </NavLink>
      </ListItem>
    </List>
  )*/
}