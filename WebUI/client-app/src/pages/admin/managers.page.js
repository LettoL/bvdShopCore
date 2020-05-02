import React, {useCallback, useEffect, useState} from 'react'
import TableContainer from "@material-ui/core/TableContainer";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableCell from "@material-ui/core/TableCell";
import TableBody from "@material-ui/core/TableBody";
import {useHttp} from "../../hooks/http.hook";

export const ManagersPage = () => {
  const {loading, request} = useHttp()
  const [managers, setManagers] = useState([]);
  
  const getManagers = useCallback(async () => {
    try {
      const data = await request('http://localhost:5000/api/managers')
      setManagers(data)
    } 
    catch (e) {}
  }, [request])
  
  useEffect(() => {
    getManagers()
  }, [getManagers])
  
  if(loading)
    return (
      <div>Загрузка...</div>
    )
  else
    return (
      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Имя</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {managers.map(row => (
              <TableRow key={row.id}>
                <TableCell>{row.name}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    )
}