import React from "react";
import Grid from "@material-ui/core/Grid";
import { makeStyles } from "@material-ui/core/styles";
import { Menu } from "../menu/menu";
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import Button from '@material-ui/core/Button';
import { useStore } from "effector-react";
import { setError, $errorStore, clearError } from "../../shared/store/error-store";
import { DialogContentText } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  }
}));

export const AdminLayout = props => {
  const classes = useStyles();
  const errors = useStore($errorStore);

  const open = errors.length ? true : false;

  const handleClose = () => {
    clearError();
  }

  return (
    <>
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle id="form-dialog-title">Ошибки</DialogTitle>
        <DialogContent>
          <ol>
            {errors.map(error => (
              <li>{error}</li>
            ))}
          </ol>     
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="primary">
            Ок
          </Button>
        </DialogActions>
      </Dialog>
      <div className={classes.root}>
        <Grid container spacing={3}>
          <Grid item xs={1}>
            <Menu />
          </Grid>
          <Grid container item xs={11}>
            {props.page}
          </Grid>
        </Grid>
      </div>
    </>
  )
}