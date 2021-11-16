import React from 'react'
import { makeStyles, Theme, createStyles, CircularProgress } from '@material-ui/core'

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        div1: {
            position: 'absolute',
            left: 0,
            top: 0,
            height: '100%',
            width: '100%',
            zIndex: 11
        },
        div2: {
            display: 'table',
            height: '100%',
            width: '100%',
            backgroundColor: 'rgba(255, 255, 255, 0.7)'
        },
        div3: {
            display: 'table-cell',
            verticalAlign: 'middle',
            textAlign: 'center'
        }
    })
)

const GimCircularProgress: React.FC<{ isLoading: boolean }> = ({ isLoading }) => {
    const classes = useStyles()
    return (
        <React.Fragment>
            {
                isLoading && (
                    <div className={classes.div1}>
                        <div className={classes.div2}>
                            <div className={classes.div3}>
                                <CircularProgress />
                            </div>
                        </div>
                    </div>
                )
            }
        </React.Fragment>
    )
}

export default GimCircularProgress