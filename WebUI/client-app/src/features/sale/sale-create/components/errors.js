import React from "react";
import {useStore} from "effector-react";
import {$errors} from "../model/sale-create";

export const Errors = () => {

  const errors = useStore($errors)

  return (
    <>
      {errors.map(error => (
        <p>{error}</p>
      ))}
    </>
  )
}