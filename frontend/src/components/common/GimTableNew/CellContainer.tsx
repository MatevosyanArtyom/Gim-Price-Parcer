import React from 'react'
import clsx from 'clsx'
import { TableCell } from '@material-ui/core'
import useStyles from './styles'

// Какой-то хак хуков
const CellContainer: React.FC = (props) => {

    const classes = useStyles()

    return (
        <TableCell
            component="div"
            className={clsx(classes.tableCell, classes.flexContainer)}
            variant="body"
        >
            {props.children}
        </TableCell>
    )
}

// const Cell: React.FC<CellProps> = (props) => <Wrapper {...props} />

export default CellContainer