import React from 'react'
import Tabs from '@material-ui/core/Tabs'
import Tab from '@material-ui/core/Tab'
import Paper from '@material-ui/core/Paper'
import { ProductSupplies } from './product-supplies'
import { ProductsUnderstaffed } from './products-understaffed'


function TabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <div>
          {children}
        </div>
      )}
    </div>
  );
}

export const ProductTabs = () => {

  const [value, setValue] = React.useState(0);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  return(
    <Paper>
      <Tabs
        indicatorColor={'primary'}
        textColor={'primary'}
        variant={'fullWidth'}
        onChange={handleChange}
        centered
      >
        <Tab label={'Поставки'}/>
        <Tab label={'Некомплект'}/>
      </Tabs>
      <TabPanel value={value} index={0}>
          <ProductSupplies />
      </TabPanel>
      <TabPanel value={value} index={1}>
          <ProductsUnderstaffed />
      </TabPanel>
    </Paper>
  )
}