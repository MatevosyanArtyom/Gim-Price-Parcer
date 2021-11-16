import React, { useRef, useCallback, useEffect, useMemo } from 'react'
import { withRouter, RouteComponentProps } from 'react-router'
import { InfiniteLoader, AutoSizer, Table, Column, IndexRange, Index, ColumnProps } from 'react-virtualized'
import update from 'immutability-helper'
import _, { Dictionary } from 'lodash'
import { makeStyles, Theme, createStyles, Paper } from '@material-ui/core'
import { CheckBox, CheckBoxOutlineBlank, Clear as ClearIcon, Edit as EditIcon, MergeType as MergeTypeIcon } from '@material-ui/icons'
import * as client from 'client'
import GimIconButton from 'components/common/GimIconButton'
import { EntityStatuses } from 'components/common/types'
import { nameofFactory } from 'util/utils'
import { widths, Filter } from './types'
import useMergeState from 'util/useMergeState'

const n = nameofFactory<client.ProductLookup>()

type Props = RouteComponentProps & {
    filter: Filter
    mergeProducts: client.ProductLookup[]
    onMergeAddClick: (product: client.ProductLookup) => void
    onMergeCancelClick: () => void
    onMergeClick: (product: client.ProductLookup) => void
    readOnly?: boolean
}

type State = {
    rowCount: number
}

const initialState: State = {
    rowCount: 999
}

const useStyles = makeStyles((theme: Theme) => (
    createStyles({
        paper: {
            minHeight: 600
        },
        table: {
            overflowY: 'scroll !important' as any
        },
        row: {
            borderBottom: '1px solid rgba(224, 224, 224, 1)'
        },
        cell: {
            margin: '0 !important',
            padding: theme.spacing(0.5)
        },
        actionsCell: {
            margin: '0 !important',
            padding: 0
        }
    })
))

const columns: ColumnProps[] = [
    {
        dataKey: n('seqId'),
        label: 'ID',
        width: widths.id,
        cellRenderer: ({ cellData }) => <span style={{ float: 'right' }}>{cellData}</span>
    },
    {
        dataKey: n('category1'),
        label: 'Категория 1',
        width: widths.category
    },
    {
        dataKey: n('category2'),
        label: 'Категория 2',
        width: widths.category
    },
    {
        dataKey: n('category3'),
        label: 'Категория 3',
        width: widths.category
    },
    {
        dataKey: n('category4'),
        label: 'Категория 4',
        width: widths.category
    },
    {
        dataKey: n('category5'),
        label: 'Категория 5',
        width: widths.category
    },
    {
        dataKey: n('name'),
        label: 'Наименование',
        width: widths.name
    },
    {
        dataKey: n('priceFrom'),
        label: 'Цена, от',
        width: widths.priceFrom,
        cellRenderer: ({ cellData }) => <span style={{ float: 'right' }}>{cellData}</span>
    },
    {
        dataKey: n('supplierCount'),
        label: 'Поставщиков',
        width: widths.suppliersCount,
        cellRenderer: ({ cellData }) => <span style={{ float: 'right' }}>{cellData}</span>
    },
    {
        dataKey: 'imagesCount',
        label: 'Фото',
        width: widths.imagesCount,
        cellRenderer: ({ rowData }) => <span style={{ float: 'right' }}>{rowData.id ? `${rowData.imagePublishedCount} / ${rowData.imageTotalCount}` : ''}</span>
    },
    {
        dataKey: n('status'),
        label: 'Статус',
        width: widths.status,
        cellDataGetter: ({ rowData }) => EntityStatuses[rowData.status]
    }
]

const initialLoadedRowsMap: Dictionary<number> = {}

const ProductsBody: React.FC<Props> = ({ filter, history, mergeProducts, onMergeAddClick, onMergeCancelClick, onMergeClick, readOnly }) => {

    const loaderRef = useRef<InfiniteLoader>(null)

    const loadedRowsMap = useRef(initialLoadedRowsMap)
    const items = useRef(new Array<client.ProductLookup>(999))

    const [{ rowCount }, setState] = useMergeState(initialState)
    const mergeProductsDict = useMemo(() => _.keyBy(mergeProducts, 'id'), [mergeProducts])

    const classes = useStyles()

    const loadMoreRows = useCallback((params: IndexRange) => {
        const { startIndex, stopIndex } = params

        for (var i = startIndex; i <= stopIndex; i++) {
            loadedRowsMap.current = update(loadedRowsMap.current, { [i]: { $set: 1 } })
        }

        return new Promise(resolve => {
            client.api.productsGetManyIndexed({ ...filter, startIndex, stopIndex }, {}).then(v => {
                if (v.data.count !== rowCount) {
                    items.current = new Array<client.ProductLookup>(v.data.count)
                    setState({ rowCount: v.data.count })
                }

                for (var i = startIndex; i <= stopIndex; i++) {
                    loadedRowsMap.current = update(loadedRowsMap.current, { [i]: { $set: 2 } })
                }
                items.current = update(items.current, { $splice: [[startIndex, stopIndex - startIndex + 1, ...v.data.entities]] })
                resolve()
            })
        })

    }, [filter, rowCount, setState])

    const rowGetter = useCallback((info: Index) => {
        return items.current[info.index] || {}
    }, [])

    const isRowLoaded = useCallback((params: Index) => {
        return loadedRowsMap.current[params.index] > 0
    }, [])

    useEffect(() => {
        if (rowCount === 0) {
            items.current = new Array<client.ProductLookup>(999)
            loadedRowsMap.current = {}
            setState({ rowCount: 999 })
        }

    }, [rowCount, setState, filter])

    useEffect(() => {
        loadedRowsMap.current = {}
        loaderRef.current && loaderRef.current.resetLoadMoreRowsCache(true)
    }, [filter, setState])

    return (
        <Paper className={classes.paper} elevation={0}>
            <InfiniteLoader
                ref={loaderRef}
                loadMoreRows={loadMoreRows}
                isRowLoaded={isRowLoaded}
                minimumBatchSize={25}
                rowCount={rowCount}
            >
                {({ onRowsRendered, registerChild }) => (
                    <AutoSizer>
                        {({ height, width }) => (
                            <Table
                                ref={registerChild}
                                headerHeight={0}
                                height={height}
                                onRowsRendered={onRowsRendered}
                                rowCount={rowCount}
                                rowGetter={rowGetter}
                                rowHeight={37}
                                width={width}
                                disableHeader={true}
                                rowClassName={(info) => items.current[info.index] && items.current[info.index].id ? classes.row : ''}
                                gridClassName={classes.table}
                            >
                                {columns.map(columnProps => (
                                    <Column
                                        key={columnProps.dataKey}
                                        className={classes.cell}
                                        flexGrow={1}
                                        flexShrink={1}
                                        {...columnProps}
                                    />
                                ))}
                                <Column
                                    dataKey="actions"
                                    label="Действия"
                                    width={widths.actions}
                                    className={classes.actionsCell}
                                    flexGrow={0}
                                    flexShrink={0}
                                    cellRenderer={({ rowData, rowIndex }) => (items.current[rowIndex] && items.current[rowIndex].id && (
                                        <div>
                                            <GimIconButton
                                                Icon={EditIcon}
                                                onClick={() => { history.push(`${history.location.pathname}/${rowData.id}`) }}
                                                tooltip="Редактировать"
                                            />
                                            {(!mergeProducts.length || (Boolean(mergeProducts.length) && mergeProducts[0].id !== rowData.id)) && (
                                                <GimIconButton
                                                    Icon={MergeTypeIcon}
                                                    onClick={() => onMergeClick(rowData)}
                                                    tooltip="Объединить"
                                                    disabled={(Boolean(mergeProducts.length) && mergeProducts[0].id !== rowData.id) || readOnly}
                                                />
                                            )}
                                            {Boolean(mergeProducts.length) && mergeProducts[0].id === rowData.id && (
                                                <GimIconButton
                                                    Icon={ClearIcon}
                                                    onClick={onMergeCancelClick}
                                                    tooltip="Отмена"
                                                />
                                            )}
                                            {Boolean(mergeProducts.length) && mergeProducts[0].id !== rowData.id && (
                                                <GimIconButton
                                                    Icon={mergeProductsDict[rowData.id] ? CheckBox : CheckBoxOutlineBlank}
                                                    onClick={() => onMergeAddClick(rowData)}
                                                    tooltip={mergeProductsDict[rowData.id] ? 'Убрать из объединяемых' : 'Добавить к объединяемым'}
                                                />
                                            )}
                                        </div>
                                    ))}
                                />
                            </Table>
                        )}
                    </AutoSizer>
                )}
            </InfiniteLoader>
        </Paper >
    )
}

export default withRouter(ProductsBody)