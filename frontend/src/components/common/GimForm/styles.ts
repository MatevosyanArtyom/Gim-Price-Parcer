import { makeStyles, Theme, createStyles } from '@material-ui/core'

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        container: {
            padding: theme.spacing(2)
        },
        paper: {
            margin: theme.spacing(2),
            padding: theme.spacing(2),
            position: 'relative'
        },
        textField: {
        },
        buttons: {
            display: 'flex'
        },
        button: {
            marginTop: theme.spacing(3),
            marginLeft: theme.spacing(1),
        },
        div1: {
            position: 'absolute',
            left: 0,
            top: 0,
            height: '100%',
            width: '100%',
            zIndex: 11
        },
        div2: {
            display: 'table',
            height: '100%',
            width: '100%',
            backgroundColor: 'rgba(255, 255, 255, 0.7)'
        },
        div3: {
            display: 'table-cell',
            verticalAlign: 'middle',
            textAlign: 'center'
        }
    })
)

export default useStyles