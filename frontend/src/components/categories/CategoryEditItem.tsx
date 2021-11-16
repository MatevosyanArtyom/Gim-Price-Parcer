import React from 'react'
import clsx from 'clsx'
import { Formik, Field, Form } from 'formik'
import { TextField } from 'formik-material-ui'
import { IconButton, ListItemText, MenuItem } from '@material-ui/core'
import { Check as CheckIcon, Clear as ClearIcon } from '@material-ui/icons'
import * as client from 'client'
import * as validations from 'util/validations'
import RowButtonPlaceholder from './RowButtonPlaceholder'
import useStyles from './styles'
import { CategoryEditItemProps } from './types'
import { EntityStatuses } from 'components/common/types'

const CategoryEditItem: React.FC<CategoryEditItemProps> = (props) => {

    const {
        category,
        depth,
        onCancelClick,
        onSaveClick
    } = props

    const classes = useStyles()

    // TODO: const ref = useRef<Formik<client.CategoryEdit>>(null)

    const onReset = () => {
        onCancelClick()
        // TODO: ref.current && ref.current.resetForm()

    }
    const onSubmit = (values: client.CategoryEdit) => {
        onSaveClick(values)
        onReset()
    }

    let staticPaddingLeft: any[] = []
    for (let index = 0; index < depth; index++) {
        staticPaddingLeft.push(<RowButtonPlaceholder key={`${index}-1`} />)
        staticPaddingLeft.push(<ListItemText key={`${index}-2`} className={classes.flex1percent} />)
    }

    let staticPaddingRight: any[] = []
    for (let index = 5 - depth - 1; index > 0; index--) {
        staticPaddingRight.push(<RowButtonPlaceholder key={`${index}-1`} />)
        staticPaddingRight.push(<ListItemText key={`${index}-2`} className={classes.flex1percent} />)
    }

    return (
        <Formik
            // TODO: ref={ref}
            initialValues={category}
            onReset={onReset}
            onSubmit={onSubmit}
            render={({ errors, handleBlur, handleChange, values }) => {
                return (
                    <Form autoComplete="off" className={classes.addCategoryForm} noValidate>
                        {staticPaddingLeft}
                        <RowButtonPlaceholder />
                        <Field
                            name="name"
                            component={TextField}
                            onBlur={handleBlur}
                            onChange={handleChange}
                            inputProps={{ className: classes.categoryItemFieldInput, placeholder: errors.name }}
                            validate={validations.required}
                            value={values.name}
                            error={errors.name}
                            className={clsx(classes.flex1percent, classes.marginRight1)}
                            required
                        />
                        {staticPaddingRight}
                        <ListItemText
                            primary={values.productsCount}
                            primaryTypographyProps={{ className: classes.categoryItemListItemText }}
                            className={classes.productsCountColumn}
                        />
                        <Field
                            name="status"
                            component={TextField}
                            onBlur={handleBlur}
                            onChange={handleChange}
                            inputProps={{ className: classes.categoryItemFieldInput, readOnly: !values.id }}
                            validate={validations.required}
                            value={values.status}
                            error={errors.status}
                            className={clsx(classes.flex02, classes.marginRight1, classes.minWidth105)}
                            required
                            select
                        >
                            {
                                Object.keys(EntityStatuses).map(key => (
                                    <MenuItem
                                        key={key}
                                        value={key}
                                        disabled={key === 'New'}
                                    >
                                        {EntityStatuses[key]}
                                    </MenuItem>
                                ))
                            }
                        </Field>
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

export default CategoryEditItem