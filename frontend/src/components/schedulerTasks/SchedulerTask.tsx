import React, { useEffect, } from 'react'
import { withRouter } from 'react-router'
import { Field, Formik, FormikProps } from 'formik'
import { TextField, CheckboxWithLabel } from 'formik-material-ui'
import { MenuItem } from '@material-ui/core'
import * as client from 'client'
import promiseWithHttpResponse from 'util/promiseWithHttpResponse'
import { RouteComponentPropsWithId } from 'util/types'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import { IntegrationMethods, StartBys, Statuses } from './types'
import FormWrapper from 'components/common/GimForm/FormWrapper'

const n = nameofFactory<client.SchedulerTaskFull>()

export const initialSchedulerTask: client.SchedulerTaskLookup = {
    id: '',
    integrationMethod: 'Unknown',
    requestRequired: false,
    startBy: 'Unknown',
    emails: '',
    script: '',
    version: '',
    modified: '',
    name: '',
    supplier: '',
    schedule: '',
    status: 'Unknown'
}

const initialSuppliers: client.SupplierShort[] = []

const initialEmitResult: client.EmitResultDto = {}

const initialState = {
    schedulerTask: initialSchedulerTask,
    suppliers: initialSuppliers,
    emitResult: initialEmitResult,
    isLoading: false
}

const SchedulerTask: React.FC<RouteComponentPropsWithId> = (props) => {

    const id = props.match.params.id === 'add' ? '' : props.match.params.id

    // TODO: const formRef = useRef<Formik>(null)

    let [{ schedulerTask, suppliers, isLoading }, setState] = useMergeState(initialState)

    //const classes = useStyles()

    useEffect(() => {
        setState({ isLoading: true })

        let promises: [
            Promise<client.HttpResponse<client.SchedulerTaskFull>>,
            Promise<client.HttpResponse<client.GetAllResultDtoOfSupplierShort>>
        ] = [
                id ? client.api.schedulerTasksGetOne(id) : promiseWithHttpResponse(initialSchedulerTask),
                client.api.lookupSuppliersGetMany({}, {})
            ]

        Promise.all(promises).then(results => {
            setState({
                schedulerTask: results[0].data,
                suppliers: results[1].data.entities,
                isLoading: false
            })
        })
    }, [id, setState])

    //const onCheckEmit = () => {
    // if (formRef.current) {
    //     setState({
    //         isLoading: true
    //     })
    //     let values = formRef.current.state.values
    //     client.api.SchedulerTasksCheckEmit({ payload: { script: values.script, rulesSource: 'Code' } }, {}).then(v => {
    //         setState({
    //             emitResult: v.data,
    //             isLoading: false
    //         })
    //     })
    // }
    //}
    const formContent = (renderProps: FormikProps<client.SchedulerTaskLookup>) => {
        return (
            <FormWrapper
                isLoading={isLoading}
            >
                <Field key={n('id')} name={n('id')} label="ID" component={TextField} margin="normal" fullWidth disabled />,
                <Field
                    key={n('name')}
                    name={n('name')}
                    label="Наименование"
                    component={TextField}
                    margin="normal"
                    validate={validations.required}
                    // TODO: error={props.errors.name}
                    fullWidth
                    required
                />,
                <Field
                    key={n('supplier')}
                    name={n('supplier')}
                    label="Поставщик"
                    component={TextField}
                    margin="normal"
                    validate={validations.required}
                    // TODO: error={props.errors.supplier}
                    fullWidth
                    required
                    select
                >
                    {suppliers.map((v: any) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                </Field>
                <Field key={n('integrationMethod')} name={n('integrationMethod')} label="Способ интеграции" component={TextField} margin="normal" select fullWidth>
                    {Object.keys(IntegrationMethods).map(key => <MenuItem key={key} value={key}>{IntegrationMethods[key]}</MenuItem>)}
                </Field>
                <Field key={n('requestRequired')} name={n('requestRequired')} Label={{ label: 'Требуется запрос' }} component={CheckboxWithLabel} />
                <Field key={n('startBy')} name={n('startBy')} label="Способ инициации" component={TextField} margin="normal" select fullWidth>
                    {Object.keys(StartBys).map(key => <MenuItem key={key} value={key}>{StartBys[key]}</MenuItem>)}
                </Field>
                <Field key={n('emails')} name={n('emails')} label="Адреса электронной почты" component={TextField} margin="normal" fullWidth />
                <Field key={n('schedule')} name={n('schedule')} label="Расписание" component={TextField} margin="normal" fullWidth />
                <Field
                    key={n('script')}
                    name={n('script')}
                    label="Правила обработки"
                    component={TextField}
                    margin="normal"
                    rows="5"
                    rowsMax="5"
                    multiline
                    fullWidth
                />,
                <Field key={n('status')} name={n('status')} label="Статус" component={TextField} margin="normal" select fullWidth>
                    {Object.keys(Statuses).map(key => <MenuItem key={key} value={key}>{Statuses[key]}</MenuItem>)}
                </Field>
                <Field key={n('modified')} name={n('modified')} label="Дата последнего изменения" component={TextField} margin="normal" fullWidth disabled />
            </FormWrapper>
        )
    }

    return (
        <Formik
            initialValues={schedulerTask}
            onSubmit={(values) => {
                if (id) {
                    client.api.schedulerTasksUpdateOne(values).then(() => props.history.goBack())
                } else {
                    client.api.schedulerTasksAddOne(values).then(() => props.history.goBack())
                }
            }}
            enableReinitialize={true}
        >
            {props => formContent(props)}
        </Formik>
        // <>
        //     <GimForm
        //         // TODO: ref={formRef}
        //         initialValues={schedulerTask}
        //         onSubmit={(values) => {
        //             if (id) {
        //                 // TODO: client.SchedulerTasksUpdateOne({ entity: values }, {}).then(() => props.history.goBack())
        //             } else {
        //                 // TODO: client.SchedulerTasksAddOne({ entity: values }, {}).then(() => props.history.goBack())
        //             }
        //         }}
        //         fields={(props) => ([

        //         ])}
        //         buttons={[
        //             <Button key="cancel" color="primary" className={classes.button} onClick={() => props.history.goBack()} disabled={isLoading}>Отмена</Button>,
        //             <Button key="chekEmit" color="primary" className={classes.button} onClick={onCheckEmit} disabled={isLoading}>Проверить компиляцию</Button>,
        //             <Button key="submit" type="submit" variant="contained" color="primary" className={classes.button} disabled={isLoading}>Сохранить</Button>
        //         ]}
        //         isLoading={isLoading}
        //     />
        //     <EmitResult result={emitResult} />
        // </>
    )
}

export default withRouter(SchedulerTask)