import React from 'react'
import { Formik, Field, Form } from 'formik'
import { TextField } from 'formik-material-ui'
import { IconButton } from '@material-ui/core'
import { Check as CheckIcon, Clear as ClearIcon } from '@material-ui/icons'
import * as client from 'client'
import RowButtonPlaceholder from 'components/categories/RowButtonPlaceholder'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import useStyles from '../styles'
import { ProductPropertyEditItemProps } from './types'

const n = nameofFactory<client.ProductPropertyEdit>()

const ProductPropertyEditItem: React.FC<ProductPropertyEditItemProps> = (props) => {

    const {
        onCancelClick,
        onSaveClick,
        value
    } = props

    const classes = useStyles()

    // TODO: const ref = useRef<Formik<client.ProductPropertyEdit>>(null)

    const onReset = () => {
        onCancelClick()
        // TODO: ref.current && ref.current.resetForm()

    }
    const onSubmit = (values: client.ProductPropertyEdit) => {
        onSaveClick(values)
    }

    return (
        <Formik
            // TODO: ref={ref}
            initialValues={value}
            onReset={onReset}
            onSubmit={onSubmit}
            render={({ errors, handleBlur, handleChange, values }) => {
                return (
                    <Form
                        autoComplete="off"
                        className={classes.productPropertyForm}
                        noValidate
                    >
                        <RowButtonPlaceholder />
                        <Field
                            name={n('name')}
                            component={TextField}
                            onBlur={handleBlur}
                            onChange={handleChange}
                            inputProps={{ className: classes.fieldInput, placeholder: 'Наименование' }}
                            validate={validations.required}
                            value={values.name}
                            error={errors.name}
                            className={classes.field}
                            required
                        />
                        <Field
                            name={n('key')}
                            component={TextField}
                            onBlur={handleBlur}
                            onChange={handleChange}
                            inputProps={{ className: classes.fieldInput, placeholder: 'Ключ' }}
                            validate={validations.required}
                            value={values.name}
                            error={errors.name}
                            className={classes.field}
                            required
                        />
                        <div className={classes.divButtonsContainer}>
                            <IconButton className={classes.rowButton} type="reset" onClick={e => e.stopPropagation()}><ClearIcon fontSize="small" /></IconButton>
                            <IconButton className={classes.rowButton} type="submit" onClick={e => e.stopPropagation()}><CheckIcon fontSize="small" /></IconButton>
                            <RowButtonPlaceholder />
                            <RowButtonPlaceholder />
                        </div>
                    </Form>
                )
            }}
        />
    )
}

export default ProductPropertyEditItem