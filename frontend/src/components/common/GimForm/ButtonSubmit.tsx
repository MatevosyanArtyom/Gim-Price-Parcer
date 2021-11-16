import React from 'react'
import { Button } from '@material-ui/core'
import { ButtonProps } from '@material-ui/core/Button'
import useStyles from './styles'

const ButtonSubmit: React.FC<ButtonProps> = (props) => {

    const classes = useStyles()

    return (
        <Button
            key="submit"
            type="submit"
            variant="contained"
            color="primary"
            className={classes.button}
            disabled={props.disabled}
        >
            Сохранить
        </Button >
    )
}

export default ButtonSubmit