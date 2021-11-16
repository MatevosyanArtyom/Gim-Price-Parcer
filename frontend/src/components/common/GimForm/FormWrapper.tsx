import React from 'react'
import { Form } from 'formik'
import { Paper } from '@material-ui/core'
import useStyles from './styles'
import { FormWrapperProps } from './types'
import GimCircularProgress from '../GimCircularProgress'

const FormWrapper: React.FC<FormWrapperProps> = (props) => {

    const classes = useStyles()

    return (
        <Paper className={classes.paper}>
            <GimCircularProgress isLoading={props.isLoading || false} />
            <Form autoComplete="off" noValidate>
                {props.children}
            </Form>
        </Paper>
    )
}

export default FormWrapper