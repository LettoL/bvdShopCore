import React, {useEffect} from "react";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import TextField from "@material-ui/core/TextField";
import {makeStyles} from "@material-ui/core/styles";
import {useStore} from "effector-react";
import {
  $filterCategoryId,
  $filterShopId,
  setCategoryFilter,
  setShopFilter,
  setTitleFilter
} from "../../models/product/product.store";
import {$categoriesFilter, $shopsFilter, fetchCategoriesFx, fetchShopsFx} from "../../models/filter/filter.store";

export const ProductTableFilters = () => {
  const classes = useStyles()

  const shopId = useStore($filterShopId)
  const categoryId = useStore($filterCategoryId)
  const shops = useStore($shopsFilter)
  const categories = useStore($categoriesFilter)

  const handleChangeShop = (event) => {
    setShopFilter(event.target.value)
  };

  const handleChangeCategory = (event) => {
    setCategoryFilter(event.target.value)
  };

  const handleChangeTitle = (event) => {
    setTitleFilter(event.target.value)
  };

  useEffect(() => {
    fetchCategoriesFx()
    fetchShopsFx()
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
        <TextField
          label="Поиск по названию"
          onChange={handleChangeTitle}
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