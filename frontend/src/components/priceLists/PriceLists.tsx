import React, { useRef, useEffect, useContext } from 'react'
import { useHistory } from 'react-router'
import _ from 'lodash'
import { Column, Query, Action } from 'material-table'
import moment from 'moment'
import { Grid, Paper, Toolbar, Typography, MenuItem, TableCell, Box } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import { GimDialogContext } from 'components/common/GimDialog'
import GimTable from 'components/common/GimTable'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import useStyles from './styles'
import { DeleteOutline } from '@material-ui/icons'
import { PriceListStatuses } from './types'
import TextFieldFilter from 'components/common/GimTable/TextFieldFilter'
import FromToDateFilter from 'components/common/GimTable/FromToDateFilter'
import SelectFilter from 'components/common/GimTable/SelectFilter'
import SupplierAutosuggest from './SupplierAutosuggest'

const n = nameofFactory<client.PriceListLookup>()

type Props = {
    variant: 'parsed' | 'commited'
}

const getColumns = (suppliers: client.SupplierShort[]): Column<client.PriceListLookup>[] => ([
    { title: 'ID', field: n('seqId'), defaultSort: 'desc', headerStyle: { width: 150 } },
    {
        title: 'Дата обработки',
        field: n('parsedDate'),
        headerStyle: { width: 450 },
        render: (data: client.PriceListLookup) => data.parsedDate ? moment(data.parsedDate).format('DD.MM.YYYY') : ''
    },
    { title: 'Поставщик', field: n('supplierId'), lookup: _.mapValues(_.keyBy(suppliers, 'id'), 'name') },
    { title: 'Правило обработки', field: n('processingRule') },
    { title: 'Статус', field: n('status'), render: data => PriceListStatuses[data.status] },
])

const initialSuppliers: client.SupplierShort[] = []
const initialSchedulersTasks: client.SchedulerTaskLookup[] = []

const initialState = {
    suppliers: initialSuppliers,
    schedulersTasks: initialSchedulersTasks,
    isLoading: false
}

const PriceLists: React.FC<Props> = ({ variant }) => {

    const tableRef = useRef<any>(null)
    const classes = useStyles()
    let history = useHistory()
    let [{ suppliers, isLoading }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)
    const gimDialogState = useContext(GimDialogContext)

    const columns = getColumns(suppliers)

    const onDeleteClick = (event: any, data: client.PriceListLookup | client.PriceListLookup[]) => {
        gimDialogState.setState!({
            open: true,
            variant: 'Delete',
            onConfirm: () => {
                setState({ isLoading: true })
                client.api.priceListsDeleteOne({ id: (data as client.PriceListLookup).id }, {})
                    .then(v => {
                        tableRef.current && tableRef.current.onQueryChange()
                        setState({ isLoading: false })
                    })
                    .finally(() => gimDialogState.setState!({ open: false }))
            }
        })
    }

    useEffect(() => {

        setState({ isLoading: true })

        let promises: [
            Promise<client.HttpResponse<client.GetAllResultDtoOfSupplierShort>>,
            Promise<client.HttpResponse<client.GetAllResultDtoOfSchedulerTaskLookup>>
        ] = [
                client.api.lookupSuppliersGetMany({ IsArchived: false }),
                client.api.schedulerTasksGetMany()
            ]

        Promise.all(promises).then(results => {
            setState({
                suppliers: results[0].data.entities,
                schedulersTasks: results[1].data.entities,
                isLoading: false
            })
        })
    }, [setState])

    return (
        <Box className={classes.root}>
            <Paper className={classes.paper}>
                <Toolbar>
                    {variant === 'parsed' && <Typography variant="h5">Прайсы — Лента загрузок</Typography>}
                    {variant === 'commited' && <Typography variant="h5">Прайсы — Утвержденные загрузки</Typography>}
                </Toolbar>
                <Grid container>
                    <Grid item xs={12}>
                        <GimTable
                            ref={tableRef}
                            title="Прайс-листы"
                            columns={columns}
                            data={(query: Query<client.PriceListItemLookup>) => {
                                const filters = {
                                    parsedFrom: query.filters.find(x => x.column.field === n('parsedDate')),
                                    parsedTo: query.filters.find(x => x.column.field === n('parsedDate')),
                                    seqId: query.filters.find(x => x.column.field === n('seqId')),
                                    supplierId: query.filters.find(x => x.column.field === n('supplierId')),
                                    // rulesSource: query.filters.find(x => x.column.field === n('rulesSource')),
                                    status: query.filters.find(x => x.column.field === n('status')),
                                }
                                const status = variant === 'commited' ? 'Committed' : (filters.status ? filters.status.value : undefined)

                                return new Promise(resolve => {
                                    client.api.priceListsGetMany({
                                        page: query.page,
                                        pageSize: query.pageSize,
                                        ParsedFrom: filters.parsedFrom ? filters.parsedFrom.value.split(';')[0] : undefined,
                                        ParsedTo: filters.parsedTo ? filters.parsedTo.value.split(';')[1] : undefined,
                                        SeqId: filters.seqId ? filters.seqId.value : undefined,
                                        SupplierId: filters.supplierId ? filters.supplierId.value : undefined,
                                        // rulesSource: filters.rulesSource ? filters.rulesSource.value : undefined,
                                        Status: status,
                                        SortBy: query.orderBy ? query.orderBy.field : undefined,
                                        SortDirection: query.orderDirection ? _.capitalize(query.orderDirection) as 'Asc' | 'Desc' : undefined
                                    }, {}).then(v => {
                                        resolve({ data: v.data.entities, page: query.page, totalCount: v.data.count })
                                    })
                                })
                            }}
                            actions={[
                                {
                                    icon: 'visibility',
                                    tooltip: 'Посмотреть',
                                    onClick: (_event: any, data: client.PriceListLookup) => history.push(`${history.location.pathname}/${data.id}`)
                                },
                                ...(variant === 'parsed' ? [
                                    (rowData: client.PriceListLookup): Action<client.PriceListLookup> => ({
                                        icon: () => <DeleteOutline />,
                                        tooltip: 'Удалить',
                                        onClick: onDeleteClick,
                                        disabled: context.user.id !== rowData.authorId && !context.user.accessRights.priceLists.full
                                    })
                                ] : [])
                            ]}
                            components={{
                                FilterRow: (props: any) => {
                                    // const rulesSource = props.columns.find((x: any) => x.field === n('rulesSource')).tableData.filterValue || ''
                                    const status = props.columns.find((x: any) => x.field === n('status')).tableData.filterValue || ''
                                    var fields = [
                                        <TextFieldFilter
                                            key={n('seqId')}
                                            name={n('seqId')}
                                            placeholder="ID"
                                            onChange={e => { props.onFilterChanged(0, e.target.value) }}
                                        />,
                                        <FromToDateFilter key={n('parsedDate')} onFilterChanged={props.onFilterChanged} />,
                                        // <TextFieldFilter
                                        //     key={n('supplierId')}
                                        //     name={n('supplierId')}
                                        //     placeholder="Поставщик"
                                        //     onChange={e => { props.onFilterChanged(2, e.target.value) }}
                                        // />,
                                        <SupplierAutosuggest
                                            key={n('supplierId')}
                                            onChange={supplierId => { props.onFilterChanged(2, supplierId) }}
                                        />,
                                        <TableCell key="3" />,
                                        // <SelectFilter
                                        //     key={n('rulesSource')}
                                        //     name={n('rulesSource')}
                                        //     placeholder="Правило обработки"
                                        //     value={rulesSource}
                                        //     onChange={e => { props.onFilterChanged(3, e.target.value) }}
                                        //     component="td"
                                        // >
                                        //     <MenuItem key="all" value="">Все</MenuItem>
                                        //     {_.keys(RulesSources).map((key: string) => <MenuItem key={key} value={key}>{RulesSources[key]}</MenuItem>)}
                                        // </SelectFilter>,
                                        ...(variant === 'parsed' ? [
                                            <SelectFilter
                                                key={n('status')}
                                                name={n('status')}
                                                placeholder="Статус"
                                                value={status}
                                                onChange={e => { props.onFilterChanged(4, e.target.value) }}
                                                component="td"
                                            >
                                                <MenuItem key="all" value="">Все</MenuItem>
                                                {_.keys(PriceListStatuses).map((key: string) => <MenuItem key={key} value={key}>{PriceListStatuses[key]}</MenuItem>)}
                                            </SelectFilter>
                                        ] : [<TableCell key="4" />]),
                                        <TableCell key="5" />
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
                    </Grid>
                </Grid>
            </Paper>
        </Box>
    )
}

export default PriceLists