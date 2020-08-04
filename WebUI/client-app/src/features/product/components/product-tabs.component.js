import React from 'react'
import Tabs from '@material-ui/core/Tabs'
import Tab from '@material-ui/core/Tab'
import Paper from '@material-ui/core/Paper'

export const ProductTabs = () => {
  return(
    <Paper>
      <Tabs
        indicatorColor={'primary'}
        textColor={'primary'}
        variant={'fullWidth'}
        centered
      >
        <Tab label={'One'}/>
        <Tab label={'Two'}/>
      </Tabs>
    </Paper>
  )
}