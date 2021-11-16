import React, { useEffect } from 'react'
import { FormikHelpers, Form, Field, FormikProps, Formik } from 'formik'
import { TextField } from 'formik-material-ui'
import { Container, Grid, Button, Typography } from '@material-ui/core'
import * as client from 'client'
// import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import useStyles from './styles'
import { RouteComponentPropsWithToken, PasswordChangeFormProps } from './types'
import { RouteComponentProps } from 'react-router'

const n = nameofFactory<PasswordChangeFormProps>()

const initialFormValues: PasswordChangeFormProps = {
    token: '',
    email: '',
    status: 'Unknown',
    password: '',
    passwordConfirm: ''
}

// const initialState: StateType = {
//     isTokenValid: undefined,
//     email: undefined,
//     status: undefined
// }

const PasswordChange: React.FC<RouteComponentPropsWithToken & RouteComponentProps> = (props) => {

    const token = props.match.params.token

    const classes = useStyles()

    // const [{ isTokenValid, email, status }, setState] = useMergeState(initialState)

    const onSubmit = (values: PasswordChangeFormProps, actions: FormikHelpers<PasswordChangeFormProps>) => {
        client.api.accountsChangePasswordAndLogin({ token: values.token, password: values.password })
            .then(v => {
                props.history.push('/')
            })
            .finally(() => actions.setSubmitting(false))
    }

    const validatePasswordConfirm = (value: string) => {
        // TODO: if (formikRef.current) {
        // TODO:     const password = formikRef.current.state.values.password
        // TODO:     return password === value
        // TODO:         ? undefined
        // TODO:         : 'Пароли не совпадают'
        // TODO: }
    }

    const renderForm = (formProps: FormikProps<PasswordChangeFormProps>) => {

        const values = formProps.values

        const isNew = formProps.values.status === 'New'

        const changePasswordForm = () => (
            <React.Fragment>
                <Typography variant="h4" gutterBottom>
                    {
                        isNew
                            ? `Создание пароля для ${values.email}`
                            : `Изменение пароля для ${values.email}`
                    }
                </Typography>
                <Typography variant="h5" gutterBottom>
                    {
                        isNew
                            ? 'Для вас создана учетная запись, придумайте себе пароль'
                            : 'Придумайте новый пароль для вашей учетной записи'
                    }
                </Typography>
                <Grid>
                    <Grid item xs={6}>
                        <Field
                            name={n('password')}
                            label="Пароль"
                            type="password"
                            autoComplete="new-password"
                            component={TextField}
                            variant="outlined"
                            margin="normal"
                            className={classes.textField}
                            validate={validations.password}
                            error={formProps.errors.password}
                            disabled={formProps.isSubmitting}
                            required
                            fullWidth
                        />
                        <Field
                            name={n('passwordConfirm')}
                            label="Подтверждение пароля"
                            type="password"
                            autoComplete="new-password"
                            component={TextField}
                            variant="outlined"
                            margin="normal"
                            className={classes.textField}
                            validate={validatePasswordConfirm}
                            error={formProps.errors.passwordConfirm}
                            disabled={formProps.isSubmitting}
                            required
                            fullWidth
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={2}>
                    <Grid xs={4} item>
                        <Button
                            type="submit"
                            variant="contained"
                            color="primary"
                            className={classes.submit}
                            disabled={formProps.isSubmitting}
                            fullWidth
                        >
                            {isNew
                                ? 'Создать пароль и авторизоваться в системе'
                                : 'Готово'
                            }
                        </Button>
                    </Grid>
                </Grid>
            </React.Fragment>
        )

        const invalidTokenForm = () => (
            <React.Fragment >
                <Typography variant="h5">
                    Неверный токен смены пароля
                </Typography>
                <Grid container spacing={2}>
                    <Grid xs={4} item>
                        <Button
                            type="button"
                            variant="contained"
                            color="primary"
                            className={classes.submit}
                            onClick={() => props.history.push('/')}
                            fullWidth
                        >
                            На главную
                        </Button>
                    </Grid>
                </Grid>
            </React.Fragment >
        )

        if (values) {
            return (
                <Form autoComplete="off" noValidate>
                    <Container maxWidth="md">
                        <div className={classes.paper}>
                            {
                                values.isTokenValid
                                    ? changePasswordForm()
                                    : invalidTokenForm()
                            }
                        </div>
                    </Container >
                </Form >
            )
        }
    }

    useEffect(() => {
        client.api.accountsValidateToken({ token }, {}).then(v => {
            // TODO: if (formikRef.current) {
            //     if (v.data.isTokenValid) {
            //         formikRef.current.setValues({
            //             ...initialFormValues,
            //             token: v.data.token,
            //             isTokenValid: v.data.isTokenValid,
            //             email: v.data.email!,
            //             status: v.data.status!
            //         })
            //     }
            // }
        })
    }, [token])

    return (
        <Formik
            initialValues={initialFormValues}
            onSubmit={onSubmit}
        >
            {props => renderForm(props)}
        </Formik>
    )
}

export default PasswordChange