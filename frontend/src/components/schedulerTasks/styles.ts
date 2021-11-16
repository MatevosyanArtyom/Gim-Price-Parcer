import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'


export const styles = (theme: Theme) => (
    createStyles({
        button: {
            marginTop: theme.spacing(3),
            marginLeft: theme.spacing(1),
        },
        container: {
            padding: 0,
            paddingTop: theme.spacing(2),
            paddingLeft: theme.spacing(2)
        },
        margin: (props: any) => ({
            margin: theme.spacing(props.margin)
        })
    })
)

const useStyles = makeStyles(styles)

export default useStyles