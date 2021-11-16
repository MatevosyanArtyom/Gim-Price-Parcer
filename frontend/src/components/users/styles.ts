import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'

export const styles = (theme: Theme) => (
    createStyles({
        box: {
            padding: theme.spacing(2),
            paddingTop: 0
        },
        paper: {
            margin: theme.spacing(2)
        },
        tableCell: {
            paddingTop: theme.spacing(0.5),
            paddingBottom: theme.spacing(0.5)
        },
        gridItem: {
            padding: theme.spacing(2)
        },
        h5: {
            marginTop: theme.spacing(3)
        },
        passwordLinkDiv: {
        },
        passwordLinkButton: {
            minWidth: '175px',
            marginBottom: theme.spacing(2)
        }
    })
)

const useStyles = makeStyles(styles)

export default useStyles