import React from 'react'
import { List, ListItem, Collapse } from '@material-ui/core'
import CategoriesTree from './CategoriesTree'
import CategoryRow from './CategoryRow'
import useStyles from './styles'
import { CategoriesTreeItemProps } from './types'

const CategoriesTreeItem: React.FC<CategoriesTreeItemProps> = (props) => {

    const {
        categories,
        category,
        categoryId,
        depth,
        expanded,
        loadCategories,
        onExpand,
        onCategoryClick,
        parentId
    } = props

    const classes = useStyles()

    return (
        <React.Fragment key={category.id}>
            <ListItem
                id={category.id}
                className={classes.listItem}
                onClick={() => onCategoryClick(category.id)}
                selected={category.id === categoryId}
                button
                disableGutters
                divider
            >
                <CategoryRow
                    category={category}
                    expanded={expanded[category.id]}
                    onExpand={onExpand}
                    depth={depth}
                />
            </ListItem>
            <Collapse in={expanded[category.id]}>
                <List className={classes.list}>
                    <CategoriesTree
                        categories={categories}
                        categoryId={categoryId}
                        depth={depth + 1}
                        expanded={expanded}
                        loadCategories={loadCategories}
                        onCategoryClick={onCategoryClick}
                        onExpand={onExpand}
                        parentId={parentId}
                    />
                </List>
            </Collapse>
        </React.Fragment >
    )
}
export default CategoriesTreeItem