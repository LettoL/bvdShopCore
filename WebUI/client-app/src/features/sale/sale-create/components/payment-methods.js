import React from "react";
import {CashPayment} from "./cash-payment";
import {CashlessPayment} from "./cashless-payment";

export const PaymentMethods = () => {
  return (
    <>
      <h3>Информация по платежам</h3>
      <CashPayment/>
      <CashlessPayment/>
    </>
  )
}