import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'


export const styles = (theme: Theme) => (
    createStyles(
        {
            root: {
                padding: theme.spacing(2)
            },
            container: {
                padding: 0,
                paddingTop: theme.spacing(2),
                paddingLeft: theme.spacing(2)
            },
            containerLeft: {
                margin: 0,
                padding: 0
            },
            flexContainer: {
                display: 'flex',
                boxSizing: 'border-box',
            },
            paper: {
                padding: theme.spacing(2)
            },
            positionRelative: {
                position: 'relative'
            },
            minHeight: {
                minHeight: 395
            },
            box: {
                padding: theme.spacing(2),
                paddingTop: 0
            },
            noMarginLeft: {
                marginLeft: 0
            },
            cell: {
                height: '100%',
                margin: '0px !important'
            },
            gridItem: {
                paddingLeft: theme.spacing(4),
                paddingRight: theme.spacing(4)
            },
            autosuggestContainer: {
                position: 'relative',
            },
            tableCell: {
                padding: 0,
                paddingRight: theme.spacing(0.5),
                width: '100%',
                height: '100%',
                display: 'table'
            },
            supplierFilterCell: {
                paddingTop: theme.spacing(0.5),
                paddingBottom: theme.spacing(0.5)
            },
            span: {
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
                overflow: 'hidden',
                display: 'table-cell',
                verticalAlign: 'middle',
                // maxWidth: 0
            },
            suggestionsContainerOpen: {
                position: 'absolute',
                zIndex: 1,
                marginTop: theme.spacing(1),
                left: 0,
                right: 0,
            },
            suggestion: {
                display: 'block',
            },
            suggestionsList: {
                margin: 0,
                padding: 0,
                listStyleType: 'none',
            },
            fileInputDiv: {
                display: 'flex'
            },
            fileInputButton: {
                alignSelf: 'flex-end',
                marginRight: theme.spacing(2),
                marginBottom: theme.spacing(1)
            },
            fileInputField: {
                flexGrow: 1
            },
            input: {
                display: 'none'
            },
            submitButton: {
                paddingTop: theme.spacing(2),
                paddingBottom: theme.spacing(4)
            },
            error: {
                color: '#f44336'
            },
            marginRight: {
                marginRight: theme.spacing(1)
            },
            marginLeft: {
                marginLeft: theme.spacing(4)
            }
        }
    )
)

const useStyles = makeStyles(styles)

export default useStyles