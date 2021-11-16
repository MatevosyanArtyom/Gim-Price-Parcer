import React from 'react'
import clsx from 'clsx'
import { makeStyles, Theme, createStyles, Box, Typography, ButtonGroup, Button } from '@material-ui/core'

export const useStyles = makeStyles((theme: Theme) => (
    createStyles({
        buttonActive: {
            textDecoration: 'none',
            backgroundColor: 'rgba(0, 0, 0, 0.08)'
        },
        typography: {
            marginRight: theme.spacing(1)
        }
    })
))

export type PriceListPage = 'items' | 'properties';

type Props = {
    page: PriceListPage,
    onChange: (page: PriceListPage) => void
    isLoading: boolean
}

const PriceListPageToggler: React.FC<Props> = ({ page, onChange, isLoading }) => {

    const classes = useStyles()

    return (
        <Box>
            <Typography display="inline" className={classes.typography}>Показать</Typography>
            <ButtonGroup disabled={isLoading}>
                <Button
                    className={clsx(page === 'items' && classes.buttonActive)}
                    onClick={() => onChange('items')}
                >
                    Список позиций
                </Button>
                <Button
                    className={clsx(page === 'properties' && classes.buttonActive)}
                    onClick={() => onChange('properties')}
                >
                    Характеристики товаров и их значения
                </Button>
            </ButtonGroup>
        </Box>
    )
}

export default PriceListPageToggler