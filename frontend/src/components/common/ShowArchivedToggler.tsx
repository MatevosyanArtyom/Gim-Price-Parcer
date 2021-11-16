import React from 'react'
import clsx from 'clsx'
import { makeStyles, Theme, createStyles, Box, Typography, ButtonGroup, Button } from '@material-ui/core'

export const useStyles = makeStyles((theme: Theme) => (
    createStyles({
        box: {
            padding: theme.spacing(2),
            paddingBottom: 0
        },
        buttonActive: {
            textDecoration: 'none',
            backgroundColor: 'rgba(0, 0, 0, 0.08)'
        },
        typography: {
            marginRight: theme.spacing(1)
        }
    })
))

export type ArchivedFilter = 'OnlyActive' | 'OnlyArchived';

type ShowArchivedToggglerProps = {
    value: ArchivedFilter,
    onClick: (value: ArchivedFilter) => void
    isLoading: boolean
}

const ShowArchivedTogggler: React.FC<ShowArchivedToggglerProps> = ({ value, onClick, isLoading }) => {

    const classes = useStyles()

    return (
        <Box className={classes.box}>
            <Typography display="inline" className={classes.typography}>Показать</Typography>
            <ButtonGroup disabled={isLoading}>
                <Button
                    className={clsx(value === 'OnlyActive' && classes.buttonActive)}
                    onClick={() => onClick('OnlyActive')}
                >
                    Действующие
                </Button>
                <Button
                    className={clsx(value === 'OnlyArchived' && classes.buttonActive)}
                    onClick={() => onClick('OnlyArchived')}
                >
                    Архивные
                    </Button>
            </ButtonGroup>
        </Box>
    )
}

export default ShowArchivedTogggler