import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'

export const styles = (theme: Theme) => (
    createStyles({
        gridItem: {
            padding: theme.spacing(2),
            paddingTop: 0
        },
        paper: {
            margin: theme.spacing(2)
        },
        tableCell: {
            paddingTop: theme.spacing(0.5),
            paddingBottom: theme.spacing(0.5)
        }
    })
)

const useStyles = makeStyles(styles)

export default useStyles