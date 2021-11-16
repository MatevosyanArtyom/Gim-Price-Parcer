import React from 'react'
import { Formik, FormikProps } from 'formik'
import ButtonsBlock from './ButtonsBlock'
import FormWrapper from './FormWrapper'
//import useStyles from './styles'
import { GimFormProps } from './types'

const GimForm = <T extends any>(props: GimFormProps<T>, ref: any) => {

    //const classes = useStyles()

    const render = (renderProps: FormikProps<T>) => {

        //const { isSubmitting } = renderProps

        let buttons = []
        if (props.buttons) {
            //buttons = props.buttons
        } else if (props.defaultButtons) {
            if (props.readonly) {
                buttons.push(
                    //     <Button
                    //         key="edit"
                    //         variant="contained"
                    //         color="primary"
                    //         className={classes.button}
                    //         // TODO: onClick={() => browserHistory.push(`${browserHistory.location.pathname}/${renderProps.values.id}`)}
                    //         // TODO: disabled={!(renderProps.values && renderProps.values.id)}
                    //     >
                    //         Редактировать
                    // </Button>
                )
            } else {
                //buttons.push(<ButtonCancel key="cancel" disabled={isSubmitting} />)
                //buttons.push(<ButtonSubmit key="submit" disabled={isSubmitting} />)
            }
        }

        return (
            <FormWrapper
                isLoading={props.isLoading}
            >
                {props.fields && props.fields(renderProps)}
                {buttons && (
                    <ButtonsBlock align="right">
                        {buttons}
                    </ButtonsBlock>
                )}
                {props.render && props.render(renderProps)}
            </FormWrapper>
        )
    }

    return (
        <Formik
            ref={ref}
            // @ts-ignore
            render={render}
            enableReinitialize
            {...props}
        />
    )
}

const withRef = React.forwardRef(GimForm)

export default withRef