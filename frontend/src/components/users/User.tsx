import React, { useEffect, useCallback, useContext } from 'react'
import { withRouter } from 'react-router'
import { Field, Formik, FormikProps } from 'formik'
import { TextField } from 'formik-material-ui'
import moment from 'moment'
import { MenuItem, Typography, Grid, TextField as MuiTextField, Button, InputAdornment, FormHelperText } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import ButtonsBlock from 'components/common/GimForm/ButtonsBlock'
import ButtonCancel from 'components/common/GimForm/ButtonCancel'
import ButtonFromArchive from 'components/common/GimForm/ButtonFromArchive'
import ButtonSubmit from 'components/common/GimForm/ButtonSubmit'
import DatePickerField from 'components/common/GimForm/DatePickerField'
import FormWrapper from 'components/common/GimForm/FormWrapper'
import promiseWithHttpResponse from 'util/promiseWithHttpResponse'
import { RouteComponentPropsWithId } from 'util/types'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import { GimUserStatuses } from './types'
import useStyles from './styles'

const n = nameofFactory<client.UserEdit>()

export const initialUser: client.UserEdit = {
    id: '',
    seqId: 0,
    createdDate: moment().format(),
    email: '',
    roleId: '',
    fullname: '',
    position: '',
    phoneNumber: '',
    hasSuppliersAccess: false,
    hasFullAccess: false,
    hasUsersAccess: false,
    status: 'New'
}

const initialRoles: client.UserRoleLookup[] = []

const initialState = {
    user: initialUser,
    roles: initialRoles,
    changePasswordToken: '',
    isLoading: false
}

const User: React.FC<RouteComponentPropsWithId> = (props) => {

    const id = props.match.params.id === 'add' ? '' : props.match.params.id

    const classes = useStyles()

    let [{ user, roles, changePasswordToken, isLoading }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)

    const readOnly = user.isArchived || !context.user.accessRights.users.full

    const init = useCallback(() => {
        setState({ isLoading: true })

        let promises: [
            Promise<client.HttpResponse<client.UserEdit>>,
            Promise<client.HttpResponse<client.GetAllResultDtoOfUserRoleLookup>>,
        ] = [
                id ? client.api.usersGetOne(id) : promiseWithHttpResponse(initialUser),
                client.api.lookupUserRolesGetMany(),
            ]

        Promise.all(promises).then(results => {
            setState({
                user: results[0].data,
                roles: results[1].data?.entities,
                isLoading: false
            })
        })
    }, [id, setState])

    useEffect(init, [init])

    const formContent = (renderProps: FormikProps<client.UserEdit>) => {
        const values = renderProps.values
        return (
            <FormWrapper
                isLoading={isLoading}
            >
                <Grid container>
                    <Grid item xs={12} className={classes.gridItem}>
                        <Typography variant="h4" gutterBottom>{values.fullname}</Typography>
                    </Grid>
                    <Grid item md={5} xs={12} className={classes.gridItem}>
                        <Typography variant="h5" gutterBottom>Регистрационные данные</Typography>
                        <Field name={n('seqId')} label="ID" component={TextField} margin="normal" fullWidth disabled />
                        <DatePickerField
                            name={n('createdDate')}
                            label="Дата создания"
                            format="DD.MM.YYYY"
                            margin="normal"
                            fullWidth
                            disabled
                        />
                        <Field
                            name={n('fullname')}
                            label="ФИО"
                            component={TextField}
                            margin="normal"
                            error={renderProps.errors.fullname}
                            validate={validations.required}
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                            required
                        />
                        <Field
                            name={n('email')}
                            label="E-mail"
                            component={TextField}
                            margin="normal"
                            error={renderProps.errors.email}
                            validate={validations.requiredEmail}
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                            required
                        />
                        <div className={classes.passwordLinkDiv} >
                            <MuiTextField
                                name="passwordLink"
                                label="Ссылка для смены пароля"
                                margin="normal"
                                InputProps={{
                                    readOnly: true,
                                    endAdornment:
                                        <InputAdornment
                                            position="end"
                                        >
                                            <Button
                                                variant="contained"
                                                className={classes.passwordLinkButton}
                                                onClick={() => {
                                                    setState({ isLoading: true })
                                                    client.api.usersSetPasswordToken(values.id).then(v => {
                                                        setState({
                                                            changePasswordToken: v.data,
                                                            isLoading: false
                                                        })
                                                    })
                                                }}
                                                disabled={!values.id || readOnly}
                                            >
                                                Получить ссылку
                                            </Button>
                                        </InputAdornment>
                                }}
                                value={changePasswordToken ? `${window.location.origin}/passwordChange/${changePasswordToken}` : ''}
                                fullWidth
                            />
                            {changePasswordToken && <FormHelperText>Ссылка отправлена на e-mail</FormHelperText>}
                        </div>
                    </Grid>
                    <Grid item md={2} />
                    <Grid item md={5} xs={12} className={classes.gridItem}>
                        <Typography variant="h5" gutterBottom>Профиль в системе</Typography>
                        <Field
                            name={n('position')}
                            label="Должность"
                            component={TextField}
                            margin="normal"
                            error={renderProps.errors.position}
                            validate={validations.required}
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                            required
                        />
                        <Field
                            name={n('phoneNumber')}
                            label="Номер телефона"
                            component={TextField}
                            margin="normal"
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                        <Field
                            name={n('roleId')}
                            label="Роль"
                            component={TextField}
                            margin="normal"
                            validate={validations.required}
                            error={renderProps.errors.roleId}
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                            required
                            select
                        >
                            {roles.map((v: client.UserRoleLookup) => <MenuItem key={v.id} value={v.id} disabled={v.isMainAdmin}>{v.name}</MenuItem>)}
                        </Field>
                    </Grid>
                </Grid>
                <Grid item xs={12} className={classes.gridItem}>
                    <Typography variant="h5" className={classes.h5} gutterBottom>Статус учетной записи</Typography>
                    <Field
                        name={n('status')}
                        label="Статус"
                        component={TextField}
                        margin="normal"
                        InputProps={{
                            readOnly: !id || readOnly
                        }}
                        select
                        fullWidth
                    >
                        {Object.keys(GimUserStatuses).map(key => (
                            <MenuItem
                                key={key}
                                value={key}
                                disabled={key === 'New' || key === 'SystemBlocked'}
                            >
                                {GimUserStatuses[key]}
                            </MenuItem>
                        ))}
                    </Field>
                </Grid>
                <ButtonsBlock align="left">
                    {!values.isArchived && <ButtonSubmit disabled={renderProps.isSubmitting || readOnly} />}
                    {
                        values.isArchived &&
                        <ButtonFromArchive
                            disabled={renderProps.isSubmitting || readOnly}
                            onClick={() => {
                                client.api.usersFromArchiveOne(values.id).then(init)
                            }}
                        />
                    }
                    <ButtonCancel disabled={renderProps.isSubmitting} />
                </ButtonsBlock>
            </FormWrapper>
        )
    }

    return (
        <Formik
            initialValues={user}
            onSubmit={(values) => {
                if (id) {
                    client.api.usersUpdateOne(values).then(() => props.history.goBack())
                } else {
                    client.api.usersAddOne(values).then(() => props.history.goBack())
                }
            }}
            enableReinitialize={true}
        >
            {props => formContent(props)}
        </Formik >
    )
}

export default withRouter(User)