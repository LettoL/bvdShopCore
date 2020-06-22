import React, {useEffect, useState} from "react";
import {useStore} from "effector-react";
import {$managers, fetchManagersFx, filterManagersFx} from "../../models/manager/manager.store";
import TableContainer from "@material-ui/core/TableContainer";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import {KeyboardDatePicker, MuiPickersUtilsProvider} from "@material-ui/pickers";
import DateFnsUtils from "@date-io/date-fns";
import FormControl from "@material-ui/core/FormControl";
import {makeStyles} from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";

export const ManagerList = () => {
  const classes = useStyles()

  const [startDate, setStartDate] = useState(null)
  const [endDate, setEndDate] = useState(null)

  const managers = useStore($managers)

  const searchHandler = () => {
    console.log(startDate, endDate)
    filterManagersFx({startDate, endDate})
  }

  useEffect(() => {
    fetchManagersFx()
  }, [])

  return (
    <>
      <FormControl className={classes.formControl}>
        <MuiPickersUtilsProvider utils={DateFnsUtils}>
          <KeyboardDatePicker
            disableToolbar
            variant='inline'
            format='dd/MM/yyyy'
            margin='normal'
            label='Дата с'
            value={startDate}
            onChange={event => setStartDate(event)}
            KeyboardButtonProps={{
              'aria-label': 'change date',
            }}
          />
        </MuiPickersUtilsProvider>
      </FormControl>
      <FormControl className={classes.formControl}>
        <MuiPickersUtilsProvider utils={DateFnsUtils}>
          <KeyboardDatePicker
            disableToolbar
            variant='inline'
            format='dd/MM/yyyy'
            margin='normal'
            label='Дата по'
            value={endDate}
            onChange={event => setEndDate(event)}
            KeyboardButtonProps={{
              'aria-label': 'change date',
            }}
          />
        </MuiPickersUtilsProvider>
      </FormControl>
      <Button
        onClick={searchHandler}
      >
        Поиск
      </Button>
      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Имя</TableCell>
              <TableCell>Маржа</TableCell>
              <TableCell>Оборот</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {managers.map(row => (
              <TableRow key={row.id}>
                <TableCell>{row.name}</TableCell>
                <TableCell>{row.margin}</TableCell>
                <TableCell>{row.sum}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </>
  )
}

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  },
  selectEmpty: {
    marginTop: theme.spacing(2),
  },
}));