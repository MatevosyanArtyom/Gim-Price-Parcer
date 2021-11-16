import React from 'react'
import { Theme, createStyles, makeStyles, TableCell } from '@material-ui/core'
import { TableCellProps } from '@material-ui/core/TableCell'

const useStyles = makeStyles((theme: Theme) => (
    createStyles({
        tableCell: ({ flexGrow, flexShrink, width }: any) => ({
            padding: theme.spacing(0.5),
            flex: `${flexGrow} ${flexShrink} ${width}px`,
            // maxWidth: width
        })
    })
))

type Props = {
    flexGrow?: 0 | 1,
    flexShrink?: 0 | 1,
    width: number
}

const ProductsTableCell: React.FC<Props & TableCellProps> = ({ flexGrow = 1, flexShrink = 1, children, width, ...tableCellProps }) => {

    const classes = useStyles({ flexGrow, flexShrink, width })

    return <TableCell component="div" className={classes.tableCell} {...tableCellProps}>{children}</TableCell>
}

export default ProductsTableCell