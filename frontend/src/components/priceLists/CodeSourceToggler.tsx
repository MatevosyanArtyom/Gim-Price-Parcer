import React from 'react'
import clsx from 'clsx'
import { FieldProps } from 'formik'
import { Box, Typography, ButtonGroup, makeStyles, createStyles, Theme, Button } from '@material-ui/core'
import * as client from 'client'

export const useStyles = makeStyles((theme: Theme) => (
    createStyles({
        box: {
            // paddingTop: theme.spacing(2),
            // paddingBottom: theme.spacing(1),
            marginTop: theme.spacing(2),
            marginBottom: theme.spacing(1)
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

const CodeSourceToggler: React.FC<FieldProps<unknown, client.PriceListAdd>> = (props) => {

    const classes = useStyles()

    return (
        <Box className={classes.box}>
            <Typography display="inline" className={classes.typography}>Источник правил</Typography>
            <ButtonGroup>
                <Button
                    className={clsx(props.field.value === 'Code' && classes.buttonActive)}
                    onClick={() => props.form.setFieldValue('rulesSource', 'Code')}
                >
                    Код
            </Button>
                <Button
                    className={clsx(props.field.value === 'File' && classes.buttonActive)}
                    onClick={() => props.form.setFieldValue('rulesSource', 'File')}
                >
                    Файл скрипта
            </Button>
            </ButtonGroup>
        </Box>
    )
}

export default CodeSourceToggler