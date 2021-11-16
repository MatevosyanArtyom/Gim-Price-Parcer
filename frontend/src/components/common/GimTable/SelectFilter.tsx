import React from 'react'
import clsx from 'clsx'
import { TableCell } from '@material-ui/core'
import Select, { SelectProps } from '@material-ui/core/Select'
import useStyles from './styles'
import { TextFieldFilterBare } from './TextFieldFilter'

const SelectFilter: React.FC<SelectProps & { component?: 'div' | 'td' }> = ({ component, className, inputComponent, children, ...otherProps }) => {

    const classes = useStyles()
    // console.log(children)
    return (
        <TableCell className={clsx(classes.tableCell, className)} component={component}>
            <Select
                input={<TextFieldFilterBare select>{children}</TextFieldFilterBare>}
                onClick={(e: any) => {
                    e.stopPropagation()
                }}
                MenuProps={{
                    PaperProps: {
                        style: {
                            // item_height * items_count + padding_top
                            maxHeight: 36 * 6 + 8
                        }
                    }
                }}
                fullWidth
                displayEmpty
                children={children}
                {...otherProps}
            />
        </TableCell>
    )
}

export default SelectFilter