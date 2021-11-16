import { makeStyles, createStyles } from '@material-ui/core'
import { Theme } from '@material-ui/core'


export const styles = (theme: Theme) => (
    createStyles(
        {
            positionRelative: {
                position: 'relative'
            },
            minHeight: {
                minHeight: 328
            },
            cell: {
                height: '100%',
                margin: '0px !important'
            },

            table: {
                overflowY: 'scroll !important' as any,
                '&:focus': {
                    outline: 'none'
                }
            }
        }
    )
)

const useStyles = makeStyles(styles)

export default useStyles