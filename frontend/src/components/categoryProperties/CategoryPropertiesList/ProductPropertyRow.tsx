import React from 'react'
import { ListItemText, IconButton } from '@material-ui/core'
import { ChevronRight, ExpandMore, Edit as EditIcon, DeleteOutline } from '@material-ui/icons'
import RowButtonPlaceholder from 'components/categories/RowButtonPlaceholder'
import useStyles from '../styles'
import { ProductPropertyRowProps } from './types'

const ProductPropertyRow: React.FC<ProductPropertyRowProps> = ({
    expanded,
    onDeleteClick,
    onEditClick,
    onExpand,
    value,
    readOnly
}) => {

    const classes = useStyles()

    return (
        <React.Fragment>
            <IconButton
                className={classes.rowButton}
                onClick={e => {
                    e.stopPropagation()
                    onExpand(value.id)
                }}
            >
                {expanded ? <ExpandMore fontSize="small" /> : <ChevronRight fontSize="small" />}
            </IconButton>
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
                <RowButtonPlaceholder />
            </div>
        </React.Fragment>
    )
}

export default ProductPropertyRow