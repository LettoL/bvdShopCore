import React, {useEffect} from "react";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import TextField from "@material-ui/core/TextField";
import {makeStyles} from "@material-ui/core/styles";
import {useStore} from "effector-react";
import {
  $filterCategoryId, $filterMinAmount, $filterMinBookedCount,
  $filterShopId, $filterSupplierId, fetchProductsBySupplierFx, fetchProductsFx,
  setCategoryFilter, setMinAmountBookedCount, setMinAmountFilter,
  setShopFilter, setSupplierFilter,
  setTitleFilter
} from "../../models/product-table/product.store";
import {
  $categoriesFilter,
  $shopsFilter,
  $suppliersFilter,
  fetchCategoriesFx,
  fetchShopsFx, fetchSupplierFx
} from "../../models/filter/filter.store";
import {fetchSuppliersFx} from "../../store/supplier-store";

export const ProductTableFilters = () => {
  const classes = useStyles()

  const shopId = useStore($filterShopId)
  const categoryId = useStore($filterCategoryId)
  const supplierId = useStore($filterSupplierId)
  const shops = useStore($shopsFilter)
  const categories = useStore($categoriesFilter)
  const suppliers = useStore($suppliersFilter)
  const minAmount = useStore($filterMinAmount)
  const minBookedCount = useStore($filterMinBookedCount)

  const handleChangeMinBookedCount = event => {
    setMinAmountBookedCount(event.target.value)
  }

  const handleChangeMinAmount = event => {
    setMinAmountFilter(event.target.value)
  }

  const handleChangeShop = (event) => {
    setShopFilter(event.target.value)
  };

  const handleChangeCategory = (event) => {
    setCategoryFilter(event.target.value)
  };

  const handleChangeSupplier = (event) => {
    const value = event.target.value

    if(value > 0)
      fetchProductsBySupplierFx(value)
    else
      fetchProductsFx()

    setSupplierFilter(value)
  }

  const handleChangeTitle = (event) => {
    setTitleFilter(event.target.value)
  };

  useEffect(() => {
    fetchCategoriesFx()
    fetchShopsFx()
    fetchSupplierFx()
  }, [])

  return (
    <>
      <FormControl className={classes.formControl}>
        <InputLabel id="demo-simple-select-label">Магазин</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={shopId}
          onChange={handleChangeShop}
        >
          <MenuItem value={0}>Выбрать магазин</MenuItem>
          {shops.map(shop => (
            <MenuItem key={shop.id} value={shop.id}>{shop.title}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <InputLabel id="demo-simple-select-label">Категория</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={categoryId}
          onChange={handleChangeCategory}
        >
          <MenuItem value={0}>Выбрать категорию</MenuItem>
          {categories.map(category => (
            <MenuItem key={category.id} value={category.id}>{category.title}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <InputLabel id="demo-simple-select-label">Поставщк</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={supplierId}
          onChange={handleChangeSupplier}
        >
          <MenuItem value={0}>Выбрать поставщика</MenuItem>
          {suppliers.map(supplier => (
            <MenuItem key={supplier.id} value={supplier.id}>{supplier.name}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl className={classes.formControl}>
        <TextField
          label="Поиск по названию"
          onChange={handleChangeTitle}
        />
      </FormControl>
      <FormControl className={classes.formControl}>
        <TextField
          label="Минимальное кол-во"
          onChange={handleChangeMinAmount}
          value={minAmount}
        />
      </FormControl>
      <FormControl className={classes.formControl}>
        <TextField
          label="Минимальное забронированное кол-во"
          onChange={handleChangeMinBookedCount}
          value={minBookedCount}
        />
      </FormControl>
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  }
}));