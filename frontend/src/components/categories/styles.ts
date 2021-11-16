import { makeStyles, createStyles } from '@material-ui/core'
import { green, red } from '@material-ui/core/colors'

const useStyles = makeStyles(theme =>
    createStyles({
        addCategoryForm: {
            display: 'flex',
            flex: 'auto',
            alignItems: 'center',
            '& p': {
                display: 'none'
            }
        },
        fab: {
            margin: theme.spacing(1),
        },
        rootList: {
            overflow: 'auto',
            minHeight: 550,
            maxHeight: 550,
            overflowY: 'scroll'
        },
        list: {
            paddingTop: 0,
            paddingBottom: 0
        },
        listItem: {
            paddingTop: 0,
            paddingBottom: 0,
            // '&:hover': {
            //     backgroundColor: 'rgba(0, 0, 0, 0.07)'
            // }
        },
        noHover: {
            '&:hover': {
                backgroundColor: 'unset'
            }
        },
        cursorNsResize: {
            cursor: 'ns-resize'
        },
        cursorMove: {
            cursor: 'move'
        },
        rootDropZone: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            minHeight: 96,
            minWidth: 48,
            outline: '1px dashed rgba(0, 0, 0, 0.7)',
            marginTop: theme.spacing(2)
        },
        rootDropZoneHover: {
            backgroundColor: 'rgba(0, 0, 0, 0.07)'
        },
        rowButton: {
            padding: theme.spacing(1)
        },
        borderBottom: {
            borderBottom: '1px solid rgba(224, 224, 224, 1)'
        },
        borderRed: {
            borderBottom: '1px solid transparent',
            backgroundColor: red[100],
            outline: '1px dashed',
            outlineColor: red[500]
        },
        borderGreen: {
            borderBottom: '1px solid transparent',
            backgroundColor: green[100],
            outline: '1px dashed',
            outlineColor: green[500]
        },
        paper: {
            margin: theme.spacing(2),
            marginRight: 0,
            padding: theme.spacing(2),
            paddingTop: 0,
        },
        marketPaper: {
            margin: theme.spacing(2),
            padding: theme.spacing(2),
            marginTop: 0
        },
        input: {
            display: 'none',
        },
        extendedIcon: {
            marginRight: theme.spacing(1),
        },
        fontWeightBold: {
            fontWeight: 'bold'
        },
        categoryItemFieldInput: {
            fontSize: '14px',
            paddingBottom: '5px'
        },
        categoryItemListItemText: {
            fontSize: '0.875rem'
        },
        maxWidth550px: {
            maxWidth: '550px'
        },
        flex1percent: {
            flex: '1 1 1%',
            marginRight: theme.spacing(1)
        },
        productsCountColumn: {
            flex: '0.1 1 1%',
            minWidth: '85px',
            marginRight: theme.spacing(3),
            textAlign: 'end'
        },
        flex02: {
            flex: '0.2 1 1%'
        },
        minWidth105: {
            minWidth: '105px'
        },
        marginRight1: {
            marginRight: theme.spacing(1)
        },
        negativeMargin36: {
            marginLeft: '-36px'
        },
        noWrap: {
            whiteSpace: 'nowrap'
        },
        divButtonsContainer: {
            display: 'flex',
            flex: '0.2 1 1%',
            minWidth: '216px',
            justifyContent: 'flex-end'
        },
        headerbackgroundColor: {
            backgroundColor: 'rgba(0, 0, 0, 0.07)'
        },
        buttonLike: {
            color: 'rgba(0, 0, 0, 0.54)',
            padding: theme.spacing(1),
            display: 'flex'
        }
    })
)

export default useStyles