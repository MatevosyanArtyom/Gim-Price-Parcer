import React, { useRef, useCallback, useContext, useEffect } from 'react'
import clsx from 'clsx'
import update from 'immutability-helper'
import { Dictionary } from 'lodash'
import { InfiniteLoader, IndexRange, AutoSizer, Table, Column, TableCellProps, Index, ColumnProps } from 'react-virtualized'
import { Paper, DialogContentText, Box } from '@material-ui/core'
import { DeleteOutline, Replay, Edit as EditIcon, Search as SearchIcon } from '@material-ui/icons'
import * as client from 'client'
import { GimDialogContext } from 'components/common/GimDialog'
import GimCircularProgress from 'components/common/GimCircularProgress'
import GimIconButton from 'components/common/GimIconButton'
import Cell from 'components/common/GimTableNew/Cell'
import CellContainer from 'components/common/GimTableNew/CellContainer'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import CategoryEditPopper from './CategoryEditPopper'
import PropertiesEditTable from './PropertiesEditTable'
import CategoryCell from './CategoryCell'
import useStyles from './styles'
import { Filter, NotFoundStr, PriceListItemStatuses, widths } from './types'
import SynonymChoosePopper from './SynonymSelectPopper'

const n = nameofFactory<client.PriceListItemLookup>()

type Props = {
    priceListId: string
    disabled: boolean
    filter: Filter
    forceUpdate: {}
    setForceUpdate: () => void
}

type State = {
    rowCount: number
    isLoading: boolean
}

const initialLoadedRowsMap: Dictionary<number> = {}

const initialState: State = {
    rowCount: 999,
    isLoading: false
}

const PriceListItemsBody: React.FC<Props> = ({ disabled, filter, priceListId, forceUpdate, setForceUpdate }) => {

    const loaderRef = useRef<InfiniteLoader>(null)

    const loadedRowsMap = useRef(initialLoadedRowsMap)
    const items = useRef(new Array<client.PriceListItemLookup>(999))

    const [{ rowCount, isLoading }, setState] = useMergeState(initialState)
    const gimDialogState = useContext(GimDialogContext)

    const classes = useStyles()

    const loadMoreRows = useCallback((params: IndexRange) => {
        const { startIndex, stopIndex } = params

        for (var i = startIndex; i <= stopIndex; i++) {
            loadedRowsMap.current = update(loadedRowsMap.current, { [i]: { $set: 1 } })
        }

        return new Promise(resolve => {
            client.api.priceListItemsGetManyIndexed({ PriceListId: priceListId, ...filter, startIndex, stopIndex }, {}).then(v => {
                if (v.data.count !== rowCount) {
                    items.current = new Array<client.PriceListItemLookup>(v.data.count)
                    setState({ rowCount: v.data.count })
                }

                for (var i = startIndex; i <= stopIndex; i++) {
                    loadedRowsMap.current = update(loadedRowsMap.current, { [i]: { $set: 2 } })
                }
                items.current = update(items.current, { $splice: [[startIndex, stopIndex - startIndex + 1, ...v.data.entities]] })
                resolve()
            })
        })
    }, [filter, priceListId, rowCount, setState])

    const rowGetter = useCallback((info: Index) => {
        return items.current[info.index] || {}
    }, [])

    const isRowLoaded = useCallback((params: Index) => {
        return loadedRowsMap.current[params.index] > 0
    }, [])

    const onSkipClick = useCallback((cellProps: TableCellProps) => {
        gimDialogState.setState!({
            open: true,
            title: 'Удаление позиции',
            variant: 'Delete',
            content: (
                <DialogContentText>Элемент будет помечен на удаление и не будет импортирован в БД при утверждении загрузки</DialogContentText>
            ),
            onConfirm: () => {
                setState({ isLoading: true })
                client.api.priceListItemsSetSkipOne(cellProps.rowData.id, true).then(v => {
                    gimDialogState.setState!({ open: false })
                    loadedRowsMap.current = {}
                    loaderRef.current && loaderRef.current.resetLoadMoreRowsCache(true)
                    setState({ isLoading: false })
                    setForceUpdate()
                })
            }
        })
    }, [gimDialogState.setState, setForceUpdate, setState])

    const onRestoreClick = useCallback((cellProps: TableCellProps) => {
        setState({ isLoading: true })
        client.api.priceListItemsSetSkipOne(cellProps.rowData.id, false).then(v => {
            loadedRowsMap.current = {}
            loaderRef.current && loaderRef.current.resetLoadMoreRowsCache(true)
            setState({ isLoading: false })
            setForceUpdate()
        })
    }, [setForceUpdate, setState])

    const onEditClick = useCallback((cellProps: TableCellProps) => {
        gimDialogState.setState!({
            open: true,
            title: 'Новые значения полей описания',
            content: (
                <PropertiesEditTable
                    item={cellProps.rowData}
                    onCancel={() => {
                        gimDialogState.setState!({ open: false })
                        setForceUpdate()
                    }}
                    onConfirm={() => {
                        gimDialogState.setState!({ open: false })
                        setForceUpdate()
                    }}
                />),
            disableBackdropClick: true,
            disableEscapeKeyDown: true,
            width: 'lg'
        })
    }, [gimDialogState.setState, setForceUpdate])

    const onCategoryEditClick = useCallback((rowData: client.PriceListItemLookup, level: number) => {
        gimDialogState.setState!({
            open: true,
            title: `Определен незнакомый уровень "${rowData[`category${level}Name`]}"`,
            content: (
                <CategoryEditPopper
                    rowData={rowData}
                    level={level}
                    onCancel={() => gimDialogState.setState!({ open: false })}
                    onConfirm={() => {
                        gimDialogState.setState!({ open: false })
                        loadedRowsMap.current = {}
                        loaderRef.current && loaderRef.current.resetLoadMoreRowsCache(true)
                        setForceUpdate()
                    }}
                />
            ),
            // disableBackdropClick: true,
            // disableEscapeKeyDown: true,
            width: 'md'
        })
    }, [gimDialogState.setState, setForceUpdate])

    const categoryCellRenderer = useCallback((tableCellProps: TableCellProps, level: number) => {
        const color = tableCellProps.rowData.skip
            ? 'grey'
            : (tableCellProps.rowData[`category${level}Status`] === 'Error'
                ? (!tableCellProps.rowData[`mapTo${level}Id`] ? 'red' : 'green')
                : undefined)
        return (
            <CategoryCell
                {...tableCellProps}
                level={level}
                color={color}
                onEditClick={() => onCategoryEditClick(tableCellProps.rowData, level)}
                status={tableCellProps.rowData[`category${level}Status`]}
                disabled={disabled}
            />
        )
    }, [disabled, onCategoryEditClick])

    const mainCellRenderer = useCallback((tableCellProps: TableCellProps) => {
        const color = tableCellProps.rowData.skip ? 'grey' : undefined
        return <Cell {...tableCellProps} color={color} />
    }, [])

    const withErrorCellRenderer = useCallback((tableCellProps: TableCellProps) => {
        const color = tableCellProps.rowData.skip
            ? 'grey'
            : tableCellProps.cellData === NotFoundStr ? 'red' : undefined
        return (
            <Cell {...tableCellProps} color={color} />
        )
    }, [])

    const productNameRenderer = useCallback((tableCellProps: TableCellProps) => {
        const color = tableCellProps.rowData.skip
            ? 'grey'
            : tableCellProps.cellData === NotFoundStr ? 'red' : undefined
        return (
            <Cell {...tableCellProps} color={color} >
                {tableCellProps.rowData.hasSynonyms && (
                    <GimIconButton
                        Icon={SearchIcon}
                        onClick={() => {
                            gimDialogState.setState!({
                                open: true,
                                title: 'Найдены похожие товары',
                                content: (
                                    <SynonymChoosePopper
                                        item={tableCellProps.rowData}
                                        onCancel={() => gimDialogState.setState!({ open: false })}
                                        onConfirm={() => {
                                            gimDialogState.setState!({ open: false })
                                            loadedRowsMap.current = {}
                                            loaderRef.current && loaderRef.current.resetLoadMoreRowsCache(true)
                                            setForceUpdate()
                                        }}
                                    />
                                ),
                                width: 'md'
                            })
                        }}
                        tooltip="Выбрать из похожих"
                        disabled={disabled}
                    />
                )}
            </Cell>
        )
    }, [disabled, gimDialogState.setState, setForceUpdate])

    useEffect(() => {
        if (forceUpdate) {
            loadedRowsMap.current = {}
            loaderRef.current && loaderRef.current.resetLoadMoreRowsCache(true)
        }
    }, [forceUpdate])

    const columns: ColumnProps[] = [
        {
            dataKey: n('code'),
            label: 'Артикул',
            width: widths.code,
            cellDataGetter: ({ rowData }) => rowData.code || (rowData.id ? NotFoundStr : ''),
            cellRenderer: withErrorCellRenderer
        },
        {
            dataKey: n('category1Name'),
            label: 'Категория 1',
            width: widths.category,
            cellRenderer: (tableCellProps) => categoryCellRenderer(tableCellProps, 1)
        },
        {
            dataKey: n('category2Name'),
            label: 'Категория 2',
            width: widths.category,
            cellRenderer: (tableCellProps) => categoryCellRenderer(tableCellProps, 2)
        },
        {
            dataKey: n('category3Name'),
            label: 'Категория 3',
            width: widths.category,
            cellRenderer: (tableCellProps) => categoryCellRenderer(tableCellProps, 3)
        },
        {
            dataKey: n('category4Name'),
            label: 'Категория 4',
            width: widths.category,
            cellRenderer: (tableCellProps) => categoryCellRenderer(tableCellProps, 4)
        },
        {
            dataKey: n('category5Name'),
            label: 'Категория 5',
            width: widths.category,
            cellRenderer: (tableCellProps) => categoryCellRenderer(tableCellProps, 5)
        },
        {
            dataKey: n('productName'),
            label: 'Наименование',
            width: widths.name,
            cellDataGetter: ({ rowData }) => rowData.productName || (rowData.id ? NotFoundStr : ''),
            cellRenderer: productNameRenderer
        },
        {
            dataKey: n('price1'),
            label: 'Цена 1',
            width: widths.price,
            cellDataGetter: ({ rowData }) => rowData.price1 || (rowData.id ? NotFoundStr : ''),
            cellRenderer: withErrorCellRenderer
        },
        {
            dataKey: n('price2'),
            label: 'Цена 2',
            width: widths.price,
            cellRenderer: mainCellRenderer
        },
        {
            dataKey: n('price3'),
            label: 'Цена 3',
            width: widths.price,
            cellRenderer: mainCellRenderer
        },
        {
            dataKey: n('status'),
            label: 'Статус',
            width: widths.status,
            cellDataGetter: ({ rowData }) => PriceListItemStatuses[rowData.status],
            cellRenderer: (tableCellProps) => {
                const rowStatus = tableCellProps.rowData.status
                const color = tableCellProps.rowData.skip
                    ? 'grey'
                    : rowStatus === 'Error' || rowStatus === 'FatalError' ? 'red' : undefined
                return <Cell {...tableCellProps} color={color} />
            }
        }
    ]

    useEffect(() => {
        if (rowCount === 0) {
            items.current = new Array<client.PriceListItemLookup>(999)
            loadedRowsMap.current = {}
            setState({ rowCount: 999 })
        }

    }, [rowCount, setState, filter])

    useEffect(() => {
        loadedRowsMap.current = {}
        loaderRef.current && loaderRef.current.resetLoadMoreRowsCache(true)
    }, [filter, setState])

    return (
        <Box className={classes.positionRelative}>
            <GimCircularProgress isLoading={isLoading} />
            <Paper className={clsx(classes.minHeight)} elevation={0}>
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
                                    disableHeader={true}
                                    headerHeight={37}
                                    height={height}
                                    onRowsRendered={onRowsRendered}
                                    rowCount={rowCount}
                                    rowGetter={rowGetter}
                                    rowHeight={41}
                                    width={width}
                                    gridClassName={classes.table}
                                >
                                    {columns.map(columnProps => (
                                        <Column
                                            key={columnProps.dataKey}
                                            cellRenderer={Cell}
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
                                        className={classes.cell}
                                        flexGrow={0}
                                        flexShrink={0}
                                        cellRenderer={cellProps => (
                                            <CellContainer>
                                                {cellProps.rowData.id && (
                                                    <React.Fragment>
                                                        {!cellProps.rowData.skip && (
                                                            <GimIconButton
                                                                Icon={DeleteOutline}
                                                                onClick={() => onSkipClick(cellProps)}
                                                                tooltip="Пропустить"
                                                                disabled={disabled}
                                                            />
                                                        )}
                                                        {cellProps.rowData.skip && (
                                                            <GimIconButton
                                                                Icon={Replay}
                                                                onClick={() => onRestoreClick(cellProps)}
                                                                tooltip="Восстановить"
                                                                disabled={disabled}
                                                            />
                                                        )}
                                                        <GimIconButton
                                                            Icon={EditIcon}
                                                            onClick={() => onEditClick(cellProps)}
                                                            tooltip="Редактировать"
                                                            disabled={cellProps.rowData.status === 'Ok' || cellProps.rowData.skip || disabled}
                                                        />
                                                    </React.Fragment>
                                                )}
                                            </CellContainer>
                                        )}
                                    />
                                </Table>
                            )}
                        </AutoSizer>
                    )}
                </InfiniteLoader>
            </Paper>
        </Box>
    )
}

export default PriceListItemsBody