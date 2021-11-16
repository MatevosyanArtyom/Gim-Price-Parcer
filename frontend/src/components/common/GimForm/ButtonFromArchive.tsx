import React from 'react'
import { Button } from '@material-ui/core'
import { ButtonProps } from '@material-ui/core/Button'
import useStyles from './styles'

const ButtonFromArchive: React.FC<ButtonProps> = (props) => {

    const classes = useStyles()

    return (
        <Button
            key="submit"
            type="button"
            variant="contained"
            color="primary"
            className={classes.button}
            disabled={props.disabled}
            onClick={props.onClick}
        >
            Восстановить
        </Button >
    )
}

export default ButtonFromArchive