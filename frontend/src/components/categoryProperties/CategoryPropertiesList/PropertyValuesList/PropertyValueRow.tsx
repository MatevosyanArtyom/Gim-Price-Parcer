import React from 'react'
import { ListItemText, IconButton } from '@material-ui/core'
import { Edit as EditIcon, DeleteOutline } from '@material-ui/icons'
import RowButtonPlaceholder from 'components/categories/RowButtonPlaceholder'
import useStyles from '../../styles'
import { PropertyValueRowProps } from './types'

const PropertyValueRow: React.FC<PropertyValueRowProps> = ({
    onDeleteClick,
    onEditClick,
    value,
    readOnly
}) => {

    const classes = useStyles()

    return (
        <React.Fragment>
            <RowButtonPlaceholder />
            <RowButtonPlaceholder />
            <ListItemText
                primary={value.name}
                primaryTypographyProps={{ className: classes.categoryRowListItemText }}
            />
            <ListItemText
                primary={value.key}
                primaryTypographyProps={{ className: classes.categoryRowListItemText }}
            />
            <div className={classes.divButtonsContainer}>
                <IconButton
                    className={classes.rowButton}
                    onClick={e => {
                        e.stopPropagation()
                        onEditClick(value.id)
                    }}
                    disabled={readOnly}
                >
                    <EditIcon fontSize="small" />
                </IconButton>
                <IconButton
                    className={classes.rowButton}
                    onClick={e => {
                        e.stopPropagation()
                        onDeleteClick(value.id)
                    }}
                    disabled={readOnly}
                >
                    <DeleteOutline fontSize="small" />
                </IconButton>
                <RowButtonPlaceholder />
                <RowButtonPlaceholder />
                <RowButtonPlaceholder />
            </div>
        </React.Fragment>
    )
}

export default PropertyValueRow