import React from 'react'
import MomentUtils from '@date-io/moment'
import { InputAdornment, Typography, TableCell } from '@material-ui/core'
import { Search } from '@material-ui/icons'
import { KeyboardDatePicker, MuiPickersUtilsProvider, KeyboardDatePickerProps } from '@material-ui/pickers'
import useMergeState from 'util/useMergeState'
import useStyles from './styles'

const initialState: { from: string | null, to: string | null } = {
    from: null,
    to: null
}

const Picker: React.FC<KeyboardDatePickerProps> = (props: KeyboardDatePickerProps) => {
    const classes = useStyles()
    return (
        <KeyboardDatePicker
            variant="inline"
            InputProps={{
                startAdornment: (
                    <InputAdornment position="start">
                        <Search />
                    </InputAdornment>
                )
            }}
            format="DD.MM.YYYY"
            autoOk={true}
            invalidDateMessage=""
            className={classes.picker}
            autoComplete="off"
            {...props}
        />
    )
}

const FromToDateFilter: React.FC<any> = ({ name, onFilterChanged }) => {

    const classes = useStyles()

    const [{ from, to }, setState] = useMergeState(initialState)

    return (
        <TableCell className={classes.tableCell}>
            <MuiPickersUtilsProvider utils={MomentUtils}>
                <Picker
                    name={`${name}From`}
                    placeholder="от"
                    value={from}
                    onChange={date => {
                        if (date) {
                            let value = date.format()
                            setState({ from: value })
                            if (date.isValid()) {
                                onFilterChanged(1, `${value};${to || ''}`)
                            }
                        } else {
                            onFilterChanged(1, `;${to || ''}`)
                        }
                    }}
                />
                <Typography display="inline" className={classes.typographyTo}>—</Typography>
                <Picker
                    name={`${name}To`}
                    placeholder="до"
                    value={to}
                    onChange={date => {
                        if (date) {
                            let value = date.format()
                            setState({ to: value })
                            if (date.isValid()) {
                                onFilterChanged(1, `${from || ''};${value}`)
                            }
                        } else {
                            onFilterChanged(1, `${from || ''};`)
                        }
                    }}
                />
            </MuiPickersUtilsProvider>
        </TableCell>
    )
}

export default FromToDateFilter