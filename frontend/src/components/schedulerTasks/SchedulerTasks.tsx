import React from 'react'
import { RouteComponentProps, withRouter } from 'react-router'
import { TextField, CheckboxWithLabel } from 'formik-material-ui'
import { Column } from 'material-table'
import { Grid, MenuItem, Container } from '@material-ui/core'
import * as client from 'client'
import GimForm from 'components/common/GimForm'
import GimTable, { RowData } from 'components/common/GimTable'
import { initialSchedulerTask } from './SchedulerTask'
import { Field } from 'formik'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import useStyles from './styles'
import { IntegrationMethods, StartBys, Statuses } from './types'

const n = nameofFactory<client.SchedulerTaskLookup>()

const columns: Column<client.SchedulerTaskLookup>[] = [
    { title: 'ID', field: n('id') },
    { title: 'Наименование', field: n('name') },
    { title: 'Поставщик', field: n('supplier') },
    { title: 'Расписание', field: n('schedule') },
    { title: 'Статус', field: n('status') }
]

const initialSchedulerTasks: client.SchedulerTaskLookup[] = []

const initialState = {
    schedulerTasks: initialSchedulerTasks,
    schedulerTaskId: ''
}

const Suppliers: React.FC<RouteComponentProps> = (props) => {
    
    const classes = useStyles("10")

    let [{ schedulerTasks, schedulerTaskId }, setState] = useMergeState(initialState)

    let supplier = schedulerTaskId ? schedulerTasks.find(v => v.id === schedulerTaskId) : initialSchedulerTask

    return (
        <Grid container>
            <Grid item xs={8}>
                <Container className={classes.container}>
                    <GimTable
                        title="Планировщик задач"
                        columns={columns}
                        data={query => {
                            return new Promise(resolve => {
                                client.api.schedulerTasksGetMany({ page: query.page, pageSize: query.pageSize }, {}).then(
                                    v => {
                                        setState({ schedulerTasks: v.data.entities })
                                        resolve({ data: v.data.entities, page: query.page, totalCount: v.data.count })
                                    }
                                )
                            })
                        }}
                        editable={{
                            onRowDelete: (oldData: client.SupplierLookup) => new Promise(resolve => {
                                client.api.schedulerTasksDeleteOne({ id: oldData.id }, {}).then((value:any) => resolve(value))
                            })
                        }}
                        onRowClick={(_e?: React.MouseEvent, rowData?: RowData) => {
                            if (rowData && rowData.id) {
                                setState({ schedulerTaskId: rowData.id })
                            }
                        }}
                        addButton={true}
                    />
                </Container>
            </Grid>
            <Grid item xs={4}>
                <GimForm
                    initialValues={supplier}
                    onSubmit={() => {

                    }}
                    fields={() => ([
                        <Field key={n('id')} name={n('id')} label="ID" component={TextField} margin="normal" fullWidth inputProps={{ readOnly: true }} />,
                        <Field key={n('name')} name={n('name')} label="Наименование" component={TextField} margin="normal" fullWidth inputProps={{ readOnly: true }} />,
                        <Field key={n('supplier')} name={n('supplier')} label="Поставщик" component={TextField} margin="normal" fullWidth inputProps={{ readOnly: true }} />,
                        <Field key={n('status')} name={n('integrationMethod')} label="Способ интеграции" component={TextField} margin="normal" select fullWidth>
                            {Object.keys(IntegrationMethods).map(key => <MenuItem key={key} value={key}>{IntegrationMethods[key]}</MenuItem>)}
                        </Field>,
                        <Field key={n('requestRequired')} name={n('requestRequired')} Label={{ label: 'Требуется запрос' }} component={CheckboxWithLabel} />,
                        <Field key={n('startBy')} name={n('startBy')} label="Способ инициации" component={TextField} margin="normal" select fullWidth>
                            {Object.keys(StartBys).map(key => <MenuItem key={key} value={key}>{StartBys[key]}</MenuItem>)}
                        </Field>,
                        <Field key={n('schedule')} name={n('schedule')} label="Расписание" component={TextField} margin="normal" fullWidth inputProps={{ readOnly: true }} />,
                        <Field key={n('status')} name={n('status')} label="Статус" component={TextField} margin="normal" select fullWidth>
                            {Object.keys(Statuses).map(key => <MenuItem key={key} value={key}>{Statuses[key]}</MenuItem>)}
                        </Field>,
                        <Field key={n('modified')} name={n('modified')} label="Дата последнего изменения" component={TextField} margin="normal" fullWidth inputProps={{ readOnly: true }} />
                    ])}
                    readonly
                />
            </Grid>
        </Grid>
    )
}

export default withRouter(Suppliers)