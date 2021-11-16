import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'


const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        paper: {
            marginTop: theme.spacing(8),
            display: 'flex',
            flexDirection: 'column'
        },
        textField: {
            display: 'block'
        },
        submit: {
            margin: theme.spacing(3, 0, 2),
        },
        errorText: {
            marginTop: theme.spacing(3),
            color: 'red',
            whiteSpace: 'pre-line'
        }
    })
)

export default useStyles