import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'


export const styles = (theme: Theme) => (
    createStyles({
        box: {
            padding: theme.spacing(2)
        },
        paper: {
            // margin: theme.spacing(2),
            padding: theme.spacing(2)
        },
        positionRelative: {
            position: 'relative'
        },
        button: {
            margin: theme.spacing(1),
        },
        margintop1: {
            marginTop: theme.spacing(1)
        },
        margintop2: {
            marginTop: theme.spacing(2)
        },
        imagesContainer: {
            padding: 0,
            paddingTop: theme.spacing(2),
            paddingRight: theme.spacing(2)
        },
        uploadButton: {
            marginTop: theme.spacing(1),
        },
        input: {
            display: 'none',
        },
        fab: {
            margin: theme.spacing(1),
        },
        headerCell: {
            display: 'flex'
        },
        tableCell: {
            padding: theme.spacing(0.5)
        }
    })
)

const useStyles = makeStyles(styles)

export default useStyles