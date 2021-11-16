import React from 'react'
import { ListItemText, IconButton } from '@material-ui/core'
import { ChevronRight, ExpandMore, } from '@material-ui/icons'
import RowButtonPlaceholder from 'components/categories/RowButtonPlaceholder'
import useStyles from './styles'
import { CategoryRowProps } from './types'

const CategoryRow: React.FC<CategoryRowProps> = ({
    category,
    depth,
    expanded,
    onExpand
}) => {

    const classes = useStyles()

    return (
        <React.Fragment>
            <div style={{ minWidth: 36 * depth }} />
            {category.hasChildren
                ? (
                    <IconButton
                        className={classes.rowButton}
                        onClick={e => {
                            e.stopPropagation()
                            onExpand(category.id, 'category')
                        }}
                    >
                        {expanded ? <ExpandMore fontSize="small" /> : <ChevronRight fontSize="small" />}
                    </IconButton>
                )
                : <RowButtonPlaceholder />
            }
            < ListItemText
                primary={category.name}
                primaryTypographyProps={{ className: classes.categoryRowListItemText }}
            />
        </React.Fragment>
    )
}

export default CategoryRow