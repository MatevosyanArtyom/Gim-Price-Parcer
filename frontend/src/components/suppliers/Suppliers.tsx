import React, { useRef, useCallback, useContext } from 'react'
import { RouteComponentProps, withRouter } from 'react-router'
import clsx from 'clsx'
import _ from 'lodash'
import { Column, Query } from 'material-table'
import moment from 'moment'
import { Grid, Paper, Box, MenuItem, TableCell, Toolbar, Typography, Button } from '@material-ui/core'
import { Clear as ClearIcon } from '@material-ui/icons'
import * as client from 'client'
import AppContext from 'context'
import GimTable from 'components/common/GimTable'
import { EntityStatuses } from 'components/common/types'
import useIsMounted from 'util/useIsMounted'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import ShowArchivedTogggler from 'components/common/ShowArchivedToggler'
import ButtonAdd from 'components/common/GimTable/ButtonAdd'
import TextFieldFilter from 'components/common/GimTable/TextFieldFilter'
import FromToDateFilter from 'components/common/GimTable/FromToDateFilter'
import SelectFilter from 'components/common/GimTable/SelectFilter'
import useStyles from './styles'
import { StateType } from './types'
import { getActions } from 'components/common/GimTable/utils'

const n = nameofFactory<client.SupplierLookup>()

const columns: Column<client.SupplierLookup>[] = [
    { title: 'ID', field: n('seqId') },
    {
        title: 'Дата создания',
        field: n('createdDate'),
        defaultSort: 'desc',
        render: (data: client.SupplierLookup) => moment(data.createdDate).format('DD.MM.YYYY')
    },
    { title: 'Наименование', field: n('name') },
    { title: 'Город', field: n('city') },
    { title: 'ИНН', field: n('inn') },
    { title: 'Статус', field: n('status'), lookup: EntityStatuses, sorting: false }
]

const initialState: StateType = {
    archivedFilter: 'OnlyActive',
    isLoading: false
}

const Suppliers: React.FC<RouteComponentProps> = (props) => {

    const tableRef = useRef<any>(null)

    const isMounted = useIsMounted()

    const classes = useStyles()

    let [{ archivedFilter, isLoading }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)

    const actionsDisabled = (rowData: client.SupplierLookup) => rowData.status !== 'New' && !context.user.accessRights.suppliers.full

    const actions = getActions<client.SupplierLookup>({
        archivedFilter: archivedFilter,
        editOpenClick: (_event, data) => {
            props.history.push(`${props.history.location.pathname}/${(data as client.SupplierLookup).id}`)
        },
        toArchive: {
            onClick: (_event: any, data: client.SupplierLookup) => {
                setState({ isLoading: true })
                client.api.suppliersToArchiveOne(data.id).then(v => {
                    tableRef.current && tableRef.current.onQueryChange()
                    setState({ isLoading: false })
                })
            },
            disabled: actionsDisabled
        },
        formArchive: {
            onClick: (_event: any, data: client.SupplierLookup) => {
                setState({ isLoading: true })
                client.api.suppliersFromArchiveOne(data.id).then(v => {
                    tableRef.current && tableRef.current.onQueryChange()
                    setState({ isLoading: false })
                })
            },
            disabled: actionsDisabled
        }
    })

    const onClearFilterClick = useCallback(() => {
        setState({ isLoading: true })
        if (tableRef.current) {
            tableRef.current.onFilterChange(0, '')
            tableRef.current.onFilterChange(1, '')
            tableRef.current.onFilterChange(2, '')
            tableRef.current.onFilterChange(3, '')
            tableRef.current.onFilterChange(4, '')
            tableRef.current.onFilterChange(5, '')
            tableRef.current.onChangeOrder(1, 'desc')
        }

        setState({ isLoading: false })
    }, [setState])

    return (
        <Paper className={classes.paper}>
            <Toolbar>
                <Typography variant="h5">Поставщики</Typography>
            </Toolbar>
            <Box className={classes.box}>
                <ShowArchivedTogggler
                    value={archivedFilter}
                    onClick={(value) => {
                        setState({ archivedFilter: value })
                        tableRef.current && tableRef.current.onChangePage(null, 0)
                    }}
                    isLoading={isLoading}
                />
            </Box>
            <Box className={clsx(classes.box, classes.noPaddingBottom, classes.displayFlex)}>
                <ButtonAdd disabled={!context.user.accessRights.suppliers.editSelf} />
                <div style={{ margin: 'auto' }} />
                <Button startIcon={<ClearIcon />} onClick={onClearFilterClick}>Сбросить фильтры и сортировку</Button>
            </Box>
            <Grid container>
                <Grid item xs={12}>
                    <Paper elevation={0} className={classes.paper}>
                        <GimTable
                            ref={tableRef}
                            columns={columns}
                            data={(query: Query<client.SupplierLookup>) => {
                                const filters = {
                                    seqId: query.filters.find(x => x.column.field === n('seqId')),
                                    createdFrom: query.filters.find(x => x.column.field === n('createdDate')),
                                    createdTo: query.filters.find(x => x.column.field === n('createdDate')),
                                    name: query.filters.find(x => x.column.field === n('name')),
                                    city: query.filters.find(x => x.column.field === n('city')),
                                    inn: query.filters.find(x => x.column.field === n('inn')),
                                    status: query.filters.find(x => x.column.field === n('status')),
                                }
                                const args = {

                                    IsArchived: archivedFilter === 'OnlyArchived',
                                    SeqId: filters.seqId ? filters.seqId.value : undefined,
                                    CreatedFrom: filters.createdFrom ? filters.createdFrom.value.split(';')[0] : undefined,
                                    CreatedTo: filters.createdTo ? filters.createdTo.value.split(';')[1] : undefined,
                                    Name: filters.name ? filters.name.value : undefined,
                                    City: filters.city ? filters.city.value : undefined,
                                    Inn: filters.inn ? filters.inn.value : undefined,
                                    Status: filters.status ? filters.status.value : undefined,

                                }
                                return new Promise(resolve => {
                                    client.api.suppliersCount(args, {}).then(v => {
                                        const totalPages = Math.max(Math.ceil(v.data / query.pageSize) - 1, 0)

                                        const page = Math.min(query.page, totalPages)
                                        client.api.suppliersGetMany({
                                            ...args,
                                            page: page,
                                            pageSize: query.pageSize,
                                            SortBy: query.orderBy ? query.orderBy.field : undefined,
                                            SortDirection: query.orderDirection ? _.capitalize(query.orderDirection) as 'Asc' | 'Desc' : undefined
                                        }, {}).then(v => {
                                            if (isMounted.current) {
                                                resolve({ data: v.data.entities, page: page || 0, totalCount: v.data.count })
                                            }
                                        })
                                    })
                                })
                            }}
                            actions={actions}
                            components={{
                                FilterRow: (props: any) => {
                                    const status = props.columns.find((x: any) => x.field === n('status')).tableData.filterValue || ''
                                    var fields = [
                                        <TextFieldFilter
                                            key={n('seqId')}
                                            name={n('seqId')}
                                            placeholder="ID"
                                            onChange={e => { props.onFilterChanged(0, e.target.value) }}
                                        />,
                                        <FromToDateFilter key={n('createdDate')} onFilterChanged={props.onFilterChanged} />,
                                        <TextFieldFilter
                                            key={n('name')}
                                            name={n('name')}
                                            placeholder="Наименование"
                                            onChange={e => { props.onFilterChanged(2, e.target.value) }}
                                        />,
                                        <TextFieldFilter
                                            key={n('city')}
                                            name={n('city')}
                                            placeholder="Город"
                                            onChange={e => { props.onFilterChanged(3, e.target.value) }}
                                        />,
                                        <TextFieldFilter
                                            key={n('inn')}
                                            name={n('inn')}
                                            placeholder="ИНН"
                                            onChange={e => { props.onFilterChanged(4, e.target.value) }}
                                        />,
                                        <SelectFilter
                                            key={n('status')}
                                            name={n('status')}
                                            placeholder="Статус"
                                            value={status}
                                            onChange={e => { props.onFilterChanged(5, e.target.value) }}
                                        >
                                            <MenuItem key="all" value="">Все</MenuItem>
                                            {_.keys(EntityStatuses).map((key: string) => <MenuItem key={key} value={key}>{EntityStatuses[key]}</MenuItem>)}
                                        </SelectFilter>,
                                        <TableCell key="6" className={classes.tableCell} />
                                    ]
                                    return (<tr>{fields}</tr>)
                                }
                            }}
                            options={{
                                actionsColumnIndex: -1,
                                filtering: true
                            }}
                            isLoading={isLoading}
                        />
                    </Paper>
                </Grid>
            </Grid>
        </Paper>
    )
}

export default withRouter(Suppliers)