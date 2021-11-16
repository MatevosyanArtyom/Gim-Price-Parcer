import React from 'react'
import clsx from 'clsx'
import { TextField, TableCell, InputAdornment } from '@material-ui/core'
import { TextFieldProps } from '@material-ui/core/TextField'
import { Search } from '@material-ui/icons'
import useStyles from './styles'

export const TextFieldFilterBare: React.FC<TextFieldProps & { inputComponent?: any }> = ({ onClick, inputComponent, children, select, ...otherProps }) => {
    return (
        <TextField
            InputProps={{
                startAdornment: (
                    <InputAdornment position="start">
                        <Search />
                    </InputAdornment>
                )
            }}
            autoComplete="off"
            onClick={e => {
                e.stopPropagation()
                onClick && onClick(e)
            }}
            select={select}
            {...otherProps}
        >
            {children}
        </TextField>

    )
}

const TextFieldFilter: React.FC<TextFieldProps & { component?: 'div' | 'td' }> = ({ component, ...otherProps }) => {

    const classes = useStyles()

    return (
        <TableCell className={clsx(classes.tableCell, otherProps.className)} component={component}>
            <TextFieldFilterBare {...otherProps} />
        </TableCell>
    )
}

export default TextFieldFilter