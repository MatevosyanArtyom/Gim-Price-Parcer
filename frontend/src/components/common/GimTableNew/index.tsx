import React, { forwardRef } from 'react'
import clsx from 'clsx'
import useStyles from './styles'
import { Table as MuiTable } from '@material-ui/core'
import { InfiniteLoader, AutoSizer, Table as RvTable, Column, Index } from 'react-virtualized'
import { GimTableProps } from './types'
import Header from './Header'
import Cell from './Cell'
import DeleteCell from './DeleteCell'

const GimTable = forwardRef<any, GimTableProps>(({
    columns,
    isRowLoaded,
    loadMoreRows,
    onDelete,
    onRowClick,
    rowGetter,
    rowCount,
    sort,
    sortBy,
    sortDirection
}, ref) => {

    const classes = useStyles()

    const rowClassName = (info: Index) => {
        return clsx(classes.flexContainer, info.index >= 0 ? classes.row : '')
    }

    return (
        <MuiTable component="div" className={classes.table} size="small">
            <InfiniteLoader
                ref={ref}
                isRowLoaded={isRowLoaded}
                loadMoreRows={loadMoreRows}
                rowCount={rowCount}
            >
                {({ onRowsRendered, registerChild }) => (
                    <AutoSizer>
                        {({ height, width }) => (
                            <RvTable
                                ref={registerChild}
                                onRowsRendered={onRowsRendered}
                                onRowClick={onRowClick}
                                height={height}
                                rowHeight={33}
                                rowCount={rowCount}
                                width={width}
                                headerHeight={78}
                                rowGetter={rowGetter}
                                rowClassName={rowClassName}
                                sort={sort}
                                sortBy={sortBy}
                                sortDirection={sortDirection}
                            >
                                {
                                    onDelete &&
                                    <Column
                                        key="actions"
                                        dataKey=""
                                        label="Действия"
                                        headerRenderer={(props) => <Header {...props} width={110} />}
                                        cellRenderer={(props) => <DeleteCell {...props} onDelete={onDelete} />}
                                        width={110}
                                        flexGrow={0}
                                        disableSort
                                    />
                                }
                                {columns.map(v => (
                                    <Column
                                        key={v.dataKey}
                                        dataKey={v.dataKey}
                                        label={v.label}
                                        headerRenderer={(props) => <Header {...props} filter={v.filter} width={v.width} />}
                                        cellRenderer={Cell}
                                        cellDataGetter={v.cellDataGetter}
                                        width={v.width}
                                        // flexGrow={v.flexGrow === undefined ? 1 : v.flexGrow}
                                        flexGrow={0}
                                        disableSort={v.disableSort}
                                    />
                                ))}
                            </RvTable>
                        )}
                    </AutoSizer>
                )}
            </InfiniteLoader>
        </MuiTable>
    )
})

export default GimTable