import React, { useRef, useContext } from 'react'
import { RouteComponentProps, withRouter } from 'react-router'
import _ from 'lodash'
import { Column, Query } from 'material-table'
import moment from 'moment'
import { Grid, TableCell, Paper, Box, Toolbar, Typography } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import GimTable from 'components/common/GimTable'
import { nameofFactory } from 'util/utils'
import ButtonAdd from 'components/common/GimTable/ButtonAdd'
import FromToDateFilter from 'components/common/GimTable/FromToDateFilter'
import TextFieldFilter from 'components/common/GimTable/TextFieldFilter'
import { getActions } from 'components/common/GimTable/utils'
import ShowArchivedTogggler from 'components/common/ShowArchivedToggler'
import useMergeState from 'util/useMergeState'
import useStyles from './styles'
import { StateType } from './types'

const n = nameofFactory<client.UserRoleLookup>()

const columns: Column<client.UserRoleLookup>[] = [
    { title: 'ID', field: n('seqId') },
    {
        title: 'Дата создания',
        field: n('createdDate'),
        defaultSort: 'desc',
        render: (data: client.UserRoleLookup) => moment(data.createdDate).format('DD.MM.YYYY')
    },
    { title: 'Наименование', field: n('name') },
    { title: 'Пользователей', field: n('usersCount') }
]

const initialState: StateType = {
    archivedFilter: 'OnlyActive',
    isLoading: false
}

const UserRoles: React.FC<RouteComponentProps> = (props) => {

    const tableRef = useRef<any>(null)

    const classes = useStyles()

    const [{ archivedFilter, isLoading }, setState] = useMergeState<StateType>(initialState)

    const context = useContext(AppContext)

    const actionsDisabled = () => !context.user.accessRights.userRoles.full

    const actions = getActions({
        archivedFilter: archivedFilter,
        editOpenClick: (event, data: client.UserRoleLookup) => {
            props.history.push(`${props.history.location.pathname}/${data.id}`)
        },
        toArchive: {
            onClick: (_event, data: client.UserRoleLookup) => {
                setState({ isLoading: true })
                client.api.userRolesToArchiveOne(data.id).then(v => {
                    tableRef.current && tableRef.current.onQueryChange()
                    setState({ isLoading: false })
                })
            },
            disabled: actionsDisabled
        },
        formArchive: {
            onClick: (_event, data: client.UserRoleLookup) => {
                setState({ isLoading: true })
                client.api.userRolesFromArchiveOne(data.id).then(v => {
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
                <Typography variant="h5">Роли</Typography>
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
                            data={(query: Query<client.UserRoleLookup>) => {
                                const filters = {
                                    createdFrom: query.filters.find(x => x.column.field === n('createdDate')),
                                    createdTo: query.filters.find(x => x.column.field === n('createdDate')),
                                    seqId: query.filters.find(x => x.column.field === n('seqId')),
                                    name: query.filters.find(x => x.column.field === n('name')),
                                    usersFrom: query.filters.find(x => x.column.field === n('usersCount'))
                                }

                                return new Promise(resolve => {
                                    client.api.userRolesGetMany({
                                        page: query.page,
                                        pageSize: query.pageSize,
                                        ArchivedFilter: archivedFilter,
                                        CreatedFrom: filters.createdFrom ? filters.createdFrom.value.split(';')[0] : undefined,
                                        CreatedTo: filters.createdTo ? filters.createdTo.value.split(';')[1] : undefined,
                                        SeqId: filters.seqId ? filters.seqId.value : undefined,
                                        Name: filters.name ? filters.name.value : undefined,
                                        UsersFrom: filters.usersFrom ? filters.usersFrom.value : undefined,
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
                                    fields.push(<FromToDateFilter key={n('createdDate')} onFilterChanged={props.onFilterChanged} />)
                                    fields.push(
                                        <TextFieldFilter
                                            key={n('name')}
                                            name={n('name')}
                                            placeholder="Наименование"
                                            onChange={e => { props.onFilterChanged(2, e.target.value) }}

                                        />
                                    )

                                    // TODO: фильтр по кол-ву пользователей
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

export default withRouter(UserRoles)