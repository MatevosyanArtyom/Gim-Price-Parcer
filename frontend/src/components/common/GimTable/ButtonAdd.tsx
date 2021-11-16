import React from 'react'
import { Button, Theme, createStyles, makeStyles } from '@material-ui/core'
import { Add } from '@material-ui/icons'
import { RouteComponentProps, withRouter } from 'react-router'
import { ButtonProps } from '@material-ui/core/Button'

export const useStyles = makeStyles((theme: Theme) => (
    createStyles({
        button: {
            marginLeft: theme.spacing(2),
            // paddingTop: theme.spacing(0.5),
            // paddingBottom: theme.spacing(0.5),
            marginTop: theme.spacing(0.25),
            marginBottom: '3px'
        },
        iconLeft: {
            marginRight: theme.spacing(1)
        }
    })
))

const ButtonAdd: React.FC<RouteComponentProps & ButtonProps> = (props) => {

    const classes = useStyles()

    return (
        <Button
            name="add"
            variant="text"
            className={props.className || classes.button}
            onClick={props.onClick ? props.onClick : () => props.history.push(`${props.location.pathname}/add`)}
            size={props.size || 'small'}
            disabled={props.disabled}
        >
            <Add className={classes.iconLeft} />
            {props.children || 'Добавить'}
        </Button>
    )
}

export default withRouter(ButtonAdd)