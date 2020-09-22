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
import { $errorStore, clearError } from "../../shared/store/error-store";
import clsx from 'clsx';
import Drawer from '@material-ui/core/Drawer';
import IconButton from '@material-ui/core/IconButton';
import ChevronLeftIcon from '@material-ui/icons/ChevronLeft';
import Divider from '@material-ui/core/Divider';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { Link } from "react-router-dom";
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import MenuIcon from '@material-ui/icons/Menu';
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';

export const mainListItems = (
  <div>
    <ListItem button component={Link} to="/admin/import">
      <ListItemText primary="Импорт" />
    </ListItem>
    <ListItem button component={Link} to="/admin/products">
      <ListItemText primary="Все товары" />
    </ListItem>
    <ListItem button component={Link} to="/admin/supplyProductsList">
      <ListItemText primary="Список поставок" />
    </ListItem>
    <ListItem button component={Link} to="/admin/managers">
      <ListItemText primary="Менеджеры" />
    </ListItem>
    <ListItem button component={Link} to="/admin/archiveSales">
      <ListItemText primary="Архивные продажи" />
    </ListItem>
  
  </div>
);

export const secondListItems = (
  <div>
      <ListItem button component={Link} to="/manager/saleCreate">
        <ListItemText primary="Продажа" />
      </ListItem>
      <button><a href={'/'}>Старый интерфейс</a></button>
  </div>
);



export const AdminLayout = props => {
  const classes = useStyles();
  const errors = useStore($errorStore);

  const [openSidebar, setOpenSidebar] = React.useState(true);
  const handleDrawerOpen = () => {
    setOpenSidebar(true);
  };
  const handleDrawerClose = () => {
    setOpenSidebar(false);
  };

  const open = errors.length ? true : false;

  const handleClose = () => {
    clearError();
  }

  return (
    <div className={classes.root}>
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
      <AppBar position="absolute" className={clsx(classes.appBar, openSidebar && classes.appBarShift)}>
        <Toolbar className={classes.toolbar}>
          <IconButton
            edge="start"
            color="inherit"
            aria-label="open drawer"
            onClick={handleDrawerOpen}
            className={clsx(classes.menuButton, openSidebar && classes.menuButtonHidden)}
          >
            <MenuIcon />
          </IconButton>
          <Typography component="h1" variant="h6" color="inherit" noWrap className={classes.title}>
            BVD
          </Typography>
        </Toolbar>
      </AppBar>
      <Drawer
        variant="permanent"
        classes={{
          paper: clsx(classes.drawerPaper, !openSidebar && classes.drawerPaperClose),
        }}
        open={openSidebar}
      >
        <div className={classes.toolbarIcon}>
          <IconButton onClick={handleDrawerClose}>
            <ChevronLeftIcon />
          </IconButton>
        </div>
        <Divider />
        <List>{mainListItems}</List>
        <Divider />
        <List>{secondListItems}</List>

      </Drawer>


      <main className={classes.content}>
        <div className={classes.appBarSpacer} />
        <Container maxWidth="false" className={classes.container}>
              {props.page}   
        </Container>
      </main>
      
    </div>
  )
}

const drawerWidth = 240;

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
  },
  toolbar: {
    paddingRight: 24, 
  },
  toolbarIcon: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'flex-end',
    padding: '0 8px',
    ...theme.mixins.toolbar,
  },
  appBar: {
    zIndex: theme.zIndex.drawer + 1,
    transition: theme.transitions.create(['width', 'margin'], {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.leavingScreen,
    }),
  },
  appBarShift: {
    marginLeft: drawerWidth,
    width: `calc(100% - ${drawerWidth}px)`,
    transition: theme.transitions.create(['width', 'margin'], {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.enteringScreen,
    }),
  },
  menuButton: {
    marginRight: 36,
  },
  menuButtonHidden: {
    display: 'none',
  },
  title: {
    flexGrow: 1,
  },
  drawerPaper: {
    position: 'relative',
    whiteSpace: 'nowrap',
    width: drawerWidth,
    transition: theme.transitions.create('width', {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.enteringScreen,
    }),
  },
  drawerPaperClose: {
    overflowX: 'hidden',
    transition: theme.transitions.create('width', {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.leavingScreen,
    }),
    width: 0,
    [theme.breakpoints.up('sm')]: {
      width: 0,
    },
  },
  appBarSpacer: theme.mixins.toolbar,
  content: {
    flexGrow: 1,
    height: '100vh',
    overflow: 'auto',
  },
  container: {
    paddingTop: theme.spacing(4),
    paddingBottom: theme.spacing(4),
  },
  paper: {
    padding: theme.spacing(2),
    display: 'flex',
    overflow: 'auto',
    flexDirection: 'column',
  },
  fixedHeight: {
    height: 240,
  },
}));