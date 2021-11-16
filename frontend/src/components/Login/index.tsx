import React from 'react'
import { RouteComponentProps } from 'react-router'
import { Formik, FormikProps, Field, Form, FormikHelpers } from 'formik'
import { TextField } from 'formik-material-ui'
import { Container, Button, Grid, Typography } from '@material-ui/core'
import * as client from 'client'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import useStyles from './styles'

const n = nameofFactory<client.LoginModel>()

const initialValues: client.LoginModel = {
    email: '',
    password: ''
}

const Login: React.FC<RouteComponentProps> = (props) => {

    const classes = useStyles()

    const onSubmit = (values: client.LoginModel, actions: FormikHelpers<client.LoginModel>) => {
        actions.setStatus(undefined)
        client.api.accountsLogin(values)
            .then(value => {
                if (value.status === 200) {
                    props.history.push('/')
                }
            })
            .catch(reason => {
                actions.setStatus(reason.status)
            })
            .finally(() => actions.setSubmitting(false))
    }

    const renderForm = (props: FormikProps<client.LoginModel>) => (
        <Form autoComplete="off" noValidate>
            <Container maxWidth="sm">
                <div className={classes.paper}>
                    <Field
                        name={n('email')}
                        label="E-mail"
                        autoComplete="username"
                        component={TextField}
                        variant="outlined"
                        margin="normal"
                        className={classes.textField}
                        validate={validations.requiredEmail}
                        error={props.errors.email}
                        disabled={props.isSubmitting}
                        required
                        fullWidth
                    />
                    <Field
                        name={n('password')}
                        label="Пароль"
                        type="password"
                        autoComplete="current-password"
                        component={TextField}
                        variant="outlined"
                        margin="normal"
                        className={classes.textField}
                        validate={validations.password}
                        error={props.errors.password}
                        disabled={props.isSubmitting}
                        required
                        fullWidth
                    />
                    <Grid container spacing={2}>
                        <Grid xs={4} item>
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                className={classes.submit}
                                disabled={props.isSubmitting}
                                fullWidth
                            >
                                Войти
                        </Button>
                        </Grid>
                        <Grid xs={8} item>
                            <Typography variant="body2" className={classes.errorText} >
                                {props.status === 403 && 'Учетная запись заблокирована.'}
                                {props.status === 404 && 'Мы вас не узнали.\nПроверьте корректность учетных данных.'}
                            </Typography>
                        </Grid>
                    </Grid>
                </div>
            </Container >
        </Form >
    )

    return (
        <Formik
            initialValues={initialValues}
            onSubmit={onSubmit}
        >
            {renderForm}
        </Formik>
    )
}

export default Login