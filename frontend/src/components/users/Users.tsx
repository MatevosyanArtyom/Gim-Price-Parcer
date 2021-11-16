import React, { useEffect, useRef, useContext } from 'react'
import { RouteComponentProps, withRouter } from 'react-router'
import _ from 'lodash'
import { Column, Query } from 'material-table'
import moment from 'moment'
import { Grid, Paper, Box, TableCell, MenuItem, Toolbar, Typography } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import GimTable from 'components/common/GimTable'
import useIsMounted from 'util/useIsMounted'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import { GimUserStatuses, StateType } from './types'
import useStyles from './styles'
import { getActions } from 'components/common/GimTable/utils'
import ShowArchivedTogggler from 'components/common/ShowArchivedToggler'
import ButtonAdd from 'components/common/GimTable/ButtonAdd'
import FromToDateFilter from 'components/common/GimTable/FromToDateFilter'
import TextFieldFilter from 'components/common/GimTable/TextFieldFilter'
import SelectFilter from 'components/common/GimTable/SelectFilter'

const n = nameofFactory<client.UserLookup>()

const getColumns = (roles: client.UserRoleLookup[]): Column<client.UserLookup>[] => ([
    { title: 'ID', field: n('seqId') },
    {
        title: 'Дата создания',
        field: n('createdDate'),
        defaultSort: 'desc',
        render: (data: client.UserLookup) => moment(data.createdDate).format('DD.MM.YYYY')
    },
    { title: 'ФИО', field: n('fullname') },
    { title: 'E-mail', field: n('email') },
    { title: 'Роль', field: n('roleId'), lookup: _.mapValues(_.keyBy(roles, 'id'), 'name') },
    { title: 'Статус', field: n('status'), lookup: GimUserStatuses }
])

const initialRoles: client.UserRoleLookup[] = []

const initialState: StateType = {
    archivedFilter: 'OnlyActive',
    roles: initialRoles,
    isLoading: false
}

const Users: React.FC<RouteComponentProps> = (props) => {

    const tableRef = useRef<any>(null)

    // Перед resolve() нужно проверять, что компонент все еще монтирован на странице
    // Может быть случай, когда произошел редирект на страницу логина или 403
    const isMounted = useIsMounted()

    const classes = useStyles()

    let [{ archivedFilter, roles, isLoading }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)

    const actionsDisabled = () => !context.user.accessRights.users.full

    const actions = getActions({
        archivedFilter: archivedFilter,
        editOpenClick: (event, data: client.UserLookup) => {
            props.history.push(`${props.history.location.pathname}/${data.id}`)
        },
        toArchive: {
            onClick: (_event, data: client.UserLookup) => {
                setState({ isLoading: true })
                client.api.usersToArchiveOne(data.id).then(v => {
                    tableRef.current && tableRef.current.onQueryChange()
                    setState({ isLoading: false })
                })
            },
            disabled: actionsDisabled
        },
        formArchive: {
            onClick: (_event, data: client.UserLookup) => {
                setState({ isLoading: true })
                client.api.usersFromArchiveOne(data.id).then(v => {
                    tableRef.current && tableRef.current.onQueryChange()
                    setState({ isLoading: false })
                })
            },
            disabled: actionsDisabled
        }
    })

    const columns = getColumns(roles)

    useEffect(() => {
        setState({ isLoading: true })

        let promises: [
            Promise<client.HttpResponse<client.GetAllResultDtoOfUserRoleLookup>>,
        ] = [
                client.api.lookupUserRolesGetMany({ ArchivedFilter: 'OnlyActive' }, {}),
            ]

        Promise.all(promises).then(results => {
            setState({
                roles: results[0].data?.entities || initialState.roles,
                isLoading: false
            })
        })
    }, [setState])

    return (
        <Paper className={classes.paper}>
            <Toolbar>
                <Typography variant="h5">Пользователи системы</Typography>
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
            <Box className={classes.box}>
                <ButtonAdd disabled={actionsDisabled()} />
            </Box>
            <Grid container>
                <Grid item xs={12}>
                    <Paper elevation={0} className={classes.paper}>
                        <GimTable
                            ref={tableRef}
                            columns={columns}
                            data={(query: Query<client.UserLookup>) => {
                                const filters = {
                                    createdFrom: query.filters.find(x => x.column.field === n('createdDate')),
                                    createdTo: query.filters.find(x => x.column.field === n('createdDate')),
                                    seqId: query.filters.find(x => x.column.field === n('seqId')),
                                    fullname: query.filters.find(x => x.column.field === n('fullname')),
                                    email: query.filters.find(x => x.column.field === n('email')),
                                    roleId: query.filters.find(x => x.column.field === n('roleId')),
                                    status: query.filters.find(x => x.column.field === n('status')),
                                }

                                return new Promise(resolve => {
                                    client.api.usersGetMany({
                                        page: query.page,
                                        pageSize: query.pageSize,
                                        ArchivedFilter: archivedFilter,
                                        CreatedFrom: filters.createdFrom ? filters.createdFrom.value.split(';')[0] : undefined,
                                        CreatedTo: filters.createdTo ? filters.createdTo.value.split(';')[1] : undefined,
                                        SeqId: filters.seqId ? filters.seqId.value : undefined,
                                        Fullname: filters.fullname ? filters.fullname.value : undefined,
                                        Email: filters.email ? filters.email.value : undefined,
                                        RoleId: filters.roleId ? filters.roleId.value : undefined,
                                        Status: filters.status ? filters.status.value : undefined,
                                        SortBy: query.orderBy ? query.orderBy.field : undefined,
                                        SortDirection: query.orderDirection ? _.capitalize(query.orderDirection) as 'Asc' | 'Desc' : undefined
                                    }, {}).then(
                                        v => {
                                            if (isMounted.current) { // TODO: add it to all async requests
                                                resolve({ data: v.data?.entities, page: query.page, totalCount: v.data?.count })
                                            }
                                        }
                                    )
                                })
                            }}
                            actions={actions}
                            components={{
                                FilterRow: (props: any) => {
                                    const roleId = props.columns.find((x: any) => x.field === n('roleId')).tableData.filterValue || ''
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
                                            key={n('fullname')}
                                            name={n('fullname')}
                                            placeholder="ФИО"
                                            onChange={e => { props.onFilterChanged(2, e.target.value) }}
                                        />,
                                        <TextFieldFilter
                                            key={n('email')}
                                            name={n('email')}
                                            placeholder="E-mail"
                                            onChange={e => { props.onFilterChanged(3, e.target.value) }}
                                        />,
                                        <SelectFilter
                                            key={n('roleId')}
                                            name={n('roleId')}
                                            placeholder="Роль"
                                            value={roleId}
                                            onChange={e => { props.onFilterChanged(4, e.target.value) }}
                                        >
                                            <MenuItem key="all" value="">Все</MenuItem>
                                            {roles.map((v: client.UserRoleLookup) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                                        </SelectFilter>,
                                        <SelectFilter
                                            key={n('status')}
                                            name={n('status')}
                                            placeholder="Статус"
                                            value={status}
                                            onChange={e => { props.onFilterChanged(5, e.target.value) }}
                                        >
                                            <MenuItem key="all" value="">Все</MenuItem>
                                            {_.keys(GimUserStatuses).map((key: string) => <MenuItem key={key} value={key}>{GimUserStatuses[key]}</MenuItem>)}
                                        </SelectFilter>,
                                        <TableCell key="6" className={classes.tableCell} />,
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

export default withRouter(Users)