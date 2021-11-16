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

export type ProductPage = 'product' | 'suppliers' | 'images';

type Props = {
    page: ProductPage,
    onChange: (page: ProductPage) => void
    isLoading: boolean
}

const PageToggler: React.FC<Props> = ({ page, onChange, isLoading }) => {

    const classes = useStyles()

    return (
        <Box>
            <Typography display="inline" className={classes.typography}>Показать</Typography>
            <ButtonGroup disabled={isLoading}>
                <Button
                    className={clsx(page === 'product' && classes.buttonActive)}
                    onClick={() => onChange('product')}
                >
                    Позиция номенклатуры
                </Button>
                <Button
                    className={clsx(page === 'suppliers' && classes.buttonActive)}
                    onClick={() => onChange('suppliers')}
                >
                    Поставщики
                </Button>
                <Button
                    className={clsx(page === 'images' && classes.buttonActive)}
                    onClick={() => onChange('images')}
                >
                    Изображения
                </Button>
            </ButtonGroup>
        </Box>
    )
}

export default PageToggler