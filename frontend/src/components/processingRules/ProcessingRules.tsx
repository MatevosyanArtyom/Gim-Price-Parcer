import React, { useRef, useContext } from 'react'
import { RouteComponentProps, withRouter } from 'react-router'
import _ from 'lodash'
import { Column, Query } from 'material-table'
import { Grid, TableCell, Paper, Box, Toolbar, Typography } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import GimTable from 'components/common/GimTable'
import ButtonAdd from 'components/common/GimTable/ButtonAdd'
import TextFieldFilter from 'components/common/GimTable/TextFieldFilter'
import { getActions } from 'components/common/GimTable/utils'
import ShowArchivedTogggler from 'components/common/ShowArchivedToggler'
import { RulesSources } from 'components/priceLists/types'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import useStyles from './styles'
import { State } from './types'

const n = nameofFactory<client.ProcessingRuleLookup>()

const columns: Column<client.ProcessingRuleLookup>[] = [
    { title: 'ID', field: n('seqId') },
    { title: 'Наименование', field: n('name') },
    { title: 'Поставщик', field: n('supplier') },
    { title: 'Правило обработки', field: n('rulesSource'), render: data => RulesSources[data.rulesSource] }
]

const initialState: State = {
    archivedFilter: 'OnlyActive',
    isLoading: false
}

const ProcessingRules: React.FC<RouteComponentProps> = (props) => {

    const tableRef = useRef<any>(null)

    const classes = useStyles()

    const [{ archivedFilter, isLoading }, setState] = useMergeState<State>(initialState)

    const context = useContext(AppContext)

    const actionsDisabled = () => !context.user.accessRights.processingRules.full

    const actions = getActions({
        archivedFilter: archivedFilter,
        editOpenClick: (event, data: client.ProcessingRuleLookup) => {
            props.history.push(`${props.history.location.pathname}/${data.id}`)
        },
        toArchive: {
            onClick: (_event, data: client.ProcessingRuleLookup) => {
                setState({ isLoading: true })
                client.api.processingRuleToArchiveOne(data.id).then(v => {
                    tableRef.current && tableRef.current.onQueryChange()
                    setState({ isLoading: false })
                })
            },
            disabled: actionsDisabled
        },
        formArchive: {
            onClick: (_event, data: client.ProcessingRuleLookup) => {
                setState({ isLoading: true })
                client.api.processingRuleFromArchiveOne(data.id).then(v => {
                    tableRef.current && tableRef.current.onQueryChange()
                    setState({ isLoading: false })
                })
            },
            disabled: actionsDisabled
        }
    })

    return (
        <Paper className={classes.paper}>
            <Toolbar>
                <Typography variant="h5">Правила обработки</Typography>
            </Toolbar>
            <Box className={classes.gridItem}>
                <ShowArchivedTogggler
                    value={archivedFilter}
                    onClick={(value) => {
                        setState({ archivedFilter: value })
                        tableRef.current && tableRef.current.onChangePage(null, 0)
                    }}
                    isLoading={isLoading}
                />
            </Box>
            <Box className={classes.gridItem}>
                <ButtonAdd disabled={actionsDisabled()} />
            </Box>
            <Grid container>
                <Grid item xs={12}>
                    <Paper elevation={0} className={classes.paper}>
                        <GimTable
                            ref={tableRef}
                            columns={columns}
                            data={(query: Query<client.ProcessingRuleLookup>) => {
                                const filters = {
                                    seqId: query.filters.find(x => x.column.field === n('seqId')),
                                    name: query.filters.find(x => x.column.field === n('name'))
                                }

                                return new Promise(resolve => {
                                    client.api.processingRuleGetMany({
                                        page: query.page,
                                        pageSize: query.pageSize,
                                        ArchivedFilter: archivedFilter,
                                        SeqId: filters.seqId ? filters.seqId.value : undefined,
                                        Name: filters.name ? filters.name.value : undefined,
                                        SortBy: query.orderBy ? query.orderBy.field : undefined,
                                        SortDirection: query.orderDirection ? _.capitalize(query.orderDirection) as 'Asc' | 'Desc' : undefined
                                    }, {}).then(
                                        v => resolve({ data: v.data.entities, page: query.page, totalCount: v.data.count })
                                    )
                                })
                            }}
                            actions={actions}
                            components={{
                                FilterRow: (props: any) => {
                                    var fields: any[] = []
                                    fields.push(
                                        <TextFieldFilter
                                            key={n('id')}
                                            name={n('id')}
                                            placeholder="ID"
                                            onChange={e => { props.onFilterChanged(0, e.target.value) }}

                                        />
                                    )
                                    fields.push(
                                        <TextFieldFilter
                                            key={n('name')}
                                            name={n('name')}
                                            placeholder="Наименование"
                                            onChange={e => { props.onFilterChanged(1, e.target.value) }}

                                        />
                                    )
                                    fields.push(<TableCell key="2" className={classes.tableCell} />)
                                    fields.push(<TableCell key="3" className={classes.tableCell} />)
                                    fields.push(<TableCell key="4" className={classes.tableCell} />)
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

export default withRouter(ProcessingRules)