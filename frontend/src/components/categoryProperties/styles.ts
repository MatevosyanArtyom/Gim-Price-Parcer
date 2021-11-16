import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'

export const styles = (theme: Theme) => (
    createStyles({
        box: {
            paddingLeft: theme.spacing(2),
            position: 'relative'
        },
        divButtonsContainer: {
            display: 'flex',
            marginLeft: 'auto',
            minWidth: '216px',
        },
        field: {
            flex: 1,
            marginRight: theme.spacing(2)
        },
        fieldInput: {
            fontSize: '14px',
            paddingBottom: '5px'
        },
        list: {
            paddingTop: 0,
            paddingBottom: 0
        },
        listItem: {
            paddingTop: 0,
            paddingBottom: 0,
        },
        paper: {
            margin: theme.spacing(2)
        },
        noMarginLeft: {
            marginLeft: 0
        },
        productPropertyForm: {
            display: 'flex',
            flex: 'auto',
            alignItems: 'center',
            '& p': {
                display: 'none'
            }
        },
        rootList: {
            overflow: 'auto',
            minHeight: 555,
            maxHeight: 555,
            overflowY: 'scroll'
        },
        rowButton: {
            padding: theme.spacing(1)
        },
        categoryRowListItemText: {
            fontSize: '0.875rem'
        },
    })
)

const useStyles = makeStyles(styles)

export default useStyles