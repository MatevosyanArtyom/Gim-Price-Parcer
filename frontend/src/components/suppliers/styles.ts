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
        h5СontactPersons: {
            paddingTop: theme.spacing(2),
            paddingLeft: theme.spacing(2),
        },
        passwordLinkDiv: {
        },
        passwordLinkButton: {
            minWidth: '175px',
            marginBottom: theme.spacing(2)
        },
        input: {
            display: 'flex',
            padding: 0,
            height: 'auto',
            flex: 1
        },
        valueContainer: {
            display: 'flex',
            flexWrap: 'wrap',
            flex: 1,
            alignItems: 'center',
            overflow: 'hidden',
        },
        noOptionsMessage: {
            padding: theme.spacing(1, 2),
        },
        noPaddingBottom: {
            paddingBottom: 0
        },
        selectPaper: {
            position: 'absolute',
            zIndex: 1,
            marginTop: theme.spacing(1),
            left: 0,
            right: 0,
        },
        displayFlex: {
            display: 'flex'
        }
    })
)

const useStyles = makeStyles(styles)

export default useStyles