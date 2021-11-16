import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        flexContainer: {
            display: 'flex',
            boxSizing: 'border-box',
        },
        paper: {
            overflowX: 'auto',
            margin: theme.spacing(2),
            // marginRight: 0,
            padding: theme.spacing(2),
            // paddingRight: 0
        },
        table: {
            minHeight: 743
        },
        row: {
            '&:hover': {
                background: 'rgba(0, 0, 0, 0.07)',
            }
        },
        filterCell: {
            paddingTop: 0,
            paddingBottom: 0
        },
        filterTextField: {
            marginTop: theme.spacing(0.5),
            marginBottom: theme.spacing(0.5)
        },
        tableCell: {
            padding: 0,
            paddingRight: theme.spacing(0.5),
            width: '100%',
            height: '100%',
            display: 'table'
        },
        span: {
            textOverflow: 'ellipsis',
            whiteSpace: 'nowrap',
            overflow: 'hidden',
            display: 'table-cell',
            verticalAlign: 'middle'
        },
        error: {
            color: '#f44336'
        },
        success: {
            color: '4caf50'
        }
    })
)

export default useStyles