import React from 'react'
import useStyles from './styles'
import { GimFormProps } from './types'
import { FormikProps } from 'formik'

type Props = {
    gimFormProps: GimFormProps<any>,
    renderProps: FormikProps<any>
}

const GimFormButtons: React.FC<Props> = ({ gimFormProps, renderProps }) => {

    const classes = useStyles()

    let buttons = []
    if (gimFormProps.buttons) {
        //buttons = gimFormProps.buttons
    } else if (gimFormProps.readonly) {
        buttons.push(
            // <Button
            //     key="edit"
            //     variant="contained"
            //     color="primary"
            //     className={classes.button}
            //     onClick={() => browserHistory.push(`${browserHistory.location.pathname}/${renderProps.values.id}`)}
            //     disabled={!(renderProps.values && renderProps.values.id)}
            // >
            //     Редактировать
            // </Button>
        )
    } else {
        buttons.push(
            // <Button
            //     key="cancel"
            //     color="primary"
            //     className={classes.button}
            //     onClick={() => browserHistory.goBack()}
            //     disabled={renderProps.isSubmitting}
            // >
            //     Отмена
            // </Button>,

        )
    }

    return (
        <div className={classes.buttons}>
            {buttons}
        </div>
    )
}

export default GimFormButtons