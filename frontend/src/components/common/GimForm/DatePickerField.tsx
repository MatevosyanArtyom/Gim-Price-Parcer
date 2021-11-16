import React from 'react'
import { Field, FieldAttributes } from 'formik'
import MomentUtils from '@date-io/moment'
import { MuiPickersUtilsProvider, DatePicker } from '@material-ui/pickers'

const DatePickerField: React.FC<FieldAttributes<any>> = (props) => {
    return (
        <MuiPickersUtilsProvider utils={MomentUtils}>
            <Field
                component={DatePicker}
                {...props}
            />
        </MuiPickersUtilsProvider>
    )
}

export default DatePickerField