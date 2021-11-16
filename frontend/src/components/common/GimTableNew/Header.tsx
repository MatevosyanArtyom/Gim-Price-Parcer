import React from 'react'
import _ from 'lodash'
import { TableHeaderProps } from 'react-virtualized'
import clsx from 'clsx'
import { TableCell, TableSortLabel } from '@material-ui/core'
import useStyles from './styles'

type Props = TableHeaderProps & { filter?: any, width: number }

// Какой-то хак хуков
const Wrapper = (props: Props) => {
    const classes = useStyles()
    const isSorted = props.dataKey === props.sortBy
    const sortDir = _.toLower(props.sortDirection) as 'asc' | 'desc' || 'asc'
    return (
        <div style={{ maxWidth: props.width }}>
            <TableCell
                component="div"
                className={clsx(classes.tableCell, classes.flexContainer)}
                variant="head"
                sortDirection={isSorted ? sortDir : false}
            >
                {
                    props.disableSort
                        ? <span>{props.label}</span>
                        : <TableSortLabel
                            active={isSorted}
                            direction={sortDir}

                        >
                            {props.label}
                        </TableSortLabel>
                }

            </TableCell>
            {
                props.filter
                    // ? <TableCell
                    //     component="div"
                    //     className={clsx(classes.tableCell, classes.flexContainer, classes.filterCell)}
                    //     variant="body"
                    // >
                    //     {/* <TextField
                    //         name={props.dataKey}
                    //         className={classes.filterTextField}
                    //         onClick={e => e.stopPropagation()}
                    //         onChange={(event) => {
                    //             props.filter(event)
                    //         }}
                    //         // margin="dense"
                    //         // variant="outlined"
                    //         fullWidth
                    //     /> */}
                    //     {props.filter()}
                    // </TableCell>
                    ? props.filter()
                    : <TableCell component="div" variant="body" className={clsx(classes.tableCell, classes.flexContainer, classes.filterCell)} style={{ padding: 0 }}><div style={{ padding: 0, height: 40, maxHeight: 40, width: '100%' }}></div></TableCell>
            }
        </div>
    )
}

const Header: React.FC<Props> = (props) => <Wrapper {...props} />

export default Header