import React from 'react'
import browserHistory from 'browserHistory'
import { Button } from '@material-ui/core'
import { ButtonProps } from '@material-ui/core/Button'
import useStyles from './styles'

const ButtonCancel: React.FC<ButtonProps> = (props) => {

    const classes = useStyles()

    return (
        <Button
            key="cancel"
            color="primary"
            className={classes.button}
            onClick={() => browserHistory.goBack()}
            disabled={props.disabled}
        >
            Отмена
        </Button>
    )
}

export default ButtonCancel