import React, {useEffect} from "react";

import {useStore} from "effector-react";
import { fetchCategoriesFx, categories } from "../../store/category-store";
import { CategoryCreate } from './category-create';

export const CategoriesList = () => {

  useEffect(() => {
    fetchCategoriesFx()
  }, [])

  const categoriesData = useStore(categories)

  return(
    <>
      <CategoryCreate />
      <ul>
        {categoriesData.map(category => (
          <li>{category.id} / {category.title}</li>
        ))}
      </ul>
    </>
  )
}