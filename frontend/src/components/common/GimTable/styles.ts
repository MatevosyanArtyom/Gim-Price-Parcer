import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'

export const styles = (theme: Theme) => (
    createStyles({
        tableCell: {
            paddingTop: theme.spacing(0.5),
            paddingBottom: theme.spacing(0.5)
        },
        picker: {
            minWidth: '170px',
            maxWidth: '170px'
        },
        typographyFrom: {
            lineHeight: '2em',
            marginRight: theme.spacing(1)
        },
        typographyTo: {
            lineHeight: '2em',
            marginLeft: theme.spacing(1),
            marginRight: theme.spacing(1)
        }
    })
)

const useStyles = makeStyles(styles)

export default useStyles