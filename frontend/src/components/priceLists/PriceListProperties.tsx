import React, { useRef, useCallback, useEffect } from 'react'
import clsx from 'clsx'
import update from 'immutability-helper'
import { Dictionary } from 'lodash'
import { InfiniteLoader, AutoSizer, Table, Column, IndexRange, Index, ColumnProps } from 'react-virtualized'
import { Paper, Box } from '@material-ui/core'
import { Add, Remove } from '@material-ui/icons'
import * as client from 'client'
import GimCircularProgress from 'components/common/GimCircularProgress'
import GimIconButton from 'components/common/GimIconButton'
import Cell from 'components/common/GimTableNew/Cell'
import CellContainer from 'components/common/GimTableNew/CellContainer'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import { PriceListItemStatuses } from './priceListItems/types'
import useStyles from './styles'
import { NotFoundStr } from './types'

const n = nameofFactory<client.PriceListItemProductPropertyLookup>()

type Props = {
    id: string
    disabled: boolean
    forceUpdate: {}
    setForceUpdate: () => void
}
type State = {
    properties: client.PriceListItemProductPropertyLookup[]
    isLoading: boolean
}

const initialLoadedRowsMap: Dictionary<number> = {}

const initialState: State = {
    properties: [],
    isLoading: false
}

const PriceListProperties: React.FC<Props> = ({ id, disabled, forceUpdate, setForceUpdate }) => {

    const loaderRef = useRef<InfiniteLoader>(null)

    const loadedRowsMap = useRef(initialLoadedRowsMap)
    const items = useRef(new Array<client.PriceListItemProductPropertyLookup>(999))
    const rowCount = useRef(999)

    const [{ isLoading }, setState] = useMergeState(initialState)

    const classes = useStyles()

    const loadMoreRows = useCallback((params: IndexRange) => {
        const { startIndex, stopIndex } = params

        for (var i = startIndex; i <= stopIndex; i++) {
            loadedRowsMap.current = update(loadedRowsMap.current, { [i]: { $set: 1 } })
        }

        return new Promise(resolve => {
            client.api.priceListItemPropertiesGetManyIndexed({ priceListId: id, startIndex, stopIndex }, {}).then(v => {
                if (v.data.count !== rowCount.current) {
                    rowCount.current = v.data.count
                    items.current = new Array<client.PriceListItemProductPropertyLookup>(v.data.count)

                    // вынести rowCount в state?
                    setState({})
                }

                for (var i = startIndex; i <= stopIndex; i++) {
                    loadedRowsMap.current = update(loadedRowsMap.current, { [i]: { $set: 2 } })
                }
                items.current = update(items.current, { $splice: [[startIndex, stopIndex - startIndex + 1, ...v.data.entities]] })
                resolve()
            })
        })

    }, [id, setState])

    useEffect(() => {
        if (forceUpdate) {
            loadedRowsMap.current = {}
            loaderRef.current && loaderRef.current.resetLoadMoreRowsCache(true)
        }
    }, [forceUpdate])

    const rowGetter = useCallback((info: Index) => {
        return items.current[info.index] || {}
    }, [])

    const isRowLoaded = useCallback((params: Index) => {
        return loadedRowsMap.current[params.index] > 0
    }, [])

    const columns: ColumnProps[] = [
        {
            dataKey: n('property'),
            label: 'Характеристика',
            width: 300,
            cellDataGetter: ({ rowData }) => rowData.property || (rowData.id ? NotFoundStr : ''),
            cellRenderer: cellProps => {
                const color = cellProps.rowData.action === 'CreateNew'
                    ? 'green'
                    : cellProps.cellData === NotFoundStr ? 'red' : undefined
                return <Cell {...cellProps} color={color} />
            }
        },
        {
            dataKey: n('propertyKey'),
            label: 'Ключ',
            width: 150
        },
        {
            dataKey: n('propertyValue'),
            label: 'Значение',
            width: 300
        },
        {
            dataKey: n('status'),
            label: 'Статус',
            width: 90,
            cellDataGetter: ({ rowData }) => PriceListItemStatuses[rowData.status],
            cellRenderer: cellProps => {
                const color = cellProps.rowData.action === 'CreateNew'
                    ? 'green'
                    : cellProps.rowData.status === 'Error' ? 'red' : undefined
                return <Cell {...cellProps} color={color} />
            }
        },
        {
            dataKey: n('action'),
            label: 'Действие',
            width: 120,
            cellDataGetter: ({ rowData }) => rowData.id ? (rowData.action === 'CreateNew' ? 'Создать новый' : 'Пропустить') : '',
            cellRenderer: cellProps => {
                const color = cellProps.rowData.action === 'CreateNew' ? 'green' : undefined
                return (
                    <Cell {...cellProps} color={color} />
                )
            }
        },
        {
            dataKey: 'actions',
            width: 36,
            cellRenderer: cellProps => {
                const model: client.PriceListItemPropertySetActionModel = {
                    priceListId: id,
                    propertyKey: cellProps.rowData.propertyKey,
                    action: cellProps.rowData.action === 'Unknown' ? 'CreateNew' : 'Unknown'
                }

                const onActionClick = () => {
                    setState({ isLoading: true })
                    client.api.priceListItemPropertiesSetActionMany(model).then(() => {
                        setState({ isLoading: false })
                        setForceUpdate()
                    })
                }

                return (
                    <CellContainer>
                        {cellProps.rowData.id && (
                            <React.Fragment>
                                {cellProps.rowData.action === 'Unknown' && (
                                    <GimIconButton
                                        Icon={Add}
                                        onClick={onActionClick}
                                        tooltip="Создать характеристику"
                                        disabled={disabled}
                                    />
                                )}
                                {cellProps.rowData.action === 'CreateNew' && (
                                    <GimIconButton
                                        Icon={Remove}
                                        onClick={onActionClick}
                                        tooltip="Пропустить характеристику"
                                        disabled={disabled}
                                    />
                                )}
                            </React.Fragment>
                        )}
                    </CellContainer>
                )
            }
        }
    ]

    return (
        <Box className={classes.positionRelative}>
            <GimCircularProgress isLoading={isLoading} />
            <Paper className={clsx(classes.minHeight)} elevation={0}>
                <InfiniteLoader
                    ref={loaderRef}
                    loadMoreRows={loadMoreRows}
                    isRowLoaded={isRowLoaded}
                    minimumBatchSize={25}
                    rowCount={rowCount.current}
                >
                    {({ onRowsRendered, registerChild }) => (
                        <AutoSizer>
                            {({ height, width }) => (
                                <Table
                                    ref={registerChild}
                                    headerHeight={37}
                                    height={height}
                                    onRowsRendered={onRowsRendered}
                                    rowCount={rowCount.current}
                                    rowGetter={rowGetter}
                                    rowHeight={41}
                                    width={width}
                                >
                                    {columns.map(columnProps => (
                                        <Column
                                            key={columnProps.dataKey}
                                            cellRenderer={Cell}
                                            className={classes.cell}
                                            headerClassName={classes.cell}
                                            {...columnProps}
                                        />
                                    ))}
                                </Table>
                            )}
                        </AutoSizer>
                    )}
                </InfiniteLoader>
            </Paper>
        </Box>
    )
}

export default PriceListProperties