import React from 'react'
import { TableCellProps } from 'react-virtualized'
import { TableCell } from '@material-ui/core'
import useStyles from './styles'

export type CellProps = TableCellProps & {
    color?: string
}

const colors = {
    green: '#4caf50',
    red: '#f44336',
    grey: 'rgba(0, 0, 0, 0.26)'
}

// Какой-то хак хуков
const Wrapper: React.FC<CellProps> = (props) => {

    const classes = useStyles()

    return (
        <TableCell
            component="div"
            style={{ display: 'flex', alignItems: 'center', height: '100%', padding: '0px 4px' }}
            variant="body"
        >
            <span
                className={classes.span}
                style={{ color: colors[props.color || ''], whiteSpace: 'normal' }}
            >
                {props.cellData}
            </span>
            {props.children}
        </TableCell>
    )
}

const Cell: React.FC<CellProps> = (props) => <Wrapper {...props} />

export default Cell