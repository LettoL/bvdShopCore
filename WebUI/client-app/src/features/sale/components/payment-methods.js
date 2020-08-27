import React from "react";
import {makeStyles} from "@material-ui/core/styles";
import {CashPayment} from "./cash-payment";
import {CashlessPayment} from "./cashless-payment";

export const PaymentMethods = () => {
  const classes = useStyles()

  return (
    <>
      <h3>Информация по платежам</h3>
      <CashPayment/>
      <CashlessPayment/>
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  marginDefault: {
    margin: theme.spacing(1),
    marginLeft: 0,
    display: 'block'
  },
}))