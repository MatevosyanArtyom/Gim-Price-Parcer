import { makeStyles, createStyles, Theme } from '@material-ui/core'

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
        },
        autosuggestContainer: {
            position: 'relative',
        },
        suggestionsContainerOpen: {
            position: 'absolute',
            zIndex: 1,
            marginTop: theme.spacing(1),
            left: 0,
            right: 0,
        },
        suggestionsList: {
            margin: 0,
            padding: 0,
            listStyleType: 'none',
        },
        suggestion: {
            display: 'block',
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
        fileInput: {
            display: 'none'
        },
        noMarginLeft: {
            marginLeft: 0
        },
        input: {
            display: 'none'
        },
        button: {
            marginTop: theme.spacing(3),
            marginLeft: theme.spacing(1),
        },
    })
)

const useStyles = makeStyles(styles)

export default useStyles