import React from "react";
import Grid from "@material-ui/core/Grid";
import {makeStyles} from "@material-ui/core/styles";
import {Menu} from "../menu/menu";

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  }
}));

export const AdminLayout = props => {
  const classes = useStyles()

  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={1}>
          <Menu/>
        </Grid>
        <Grid container item xs={11}>
          {props.page}
        </Grid>
      </Grid>
    </div>
  )
}