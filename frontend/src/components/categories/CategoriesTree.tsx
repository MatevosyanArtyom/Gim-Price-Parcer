import React from 'react'
import CategoriesTreeItem from './CategoriesTreeItem'
import { CategoriesTreeProps } from './types'
import { ListItem } from '@material-ui/core'
import CategoryAddItem from './CategoryAddItem'
import useStyles from './styles'

const CategoriesTree: React.FC<CategoriesTreeProps> = (props) => {
    const {
        categories,
        categoryId,
        depth,
        expanded,
        onAddClick,
        onDeleteClick,
        onEditClick,
        onCancelClick,
        onExpand,
        onMapClick,
        onMergeClick,
        onSaveClick,
        onDragHover,
        updateTree,
        editState,
        mergeState,
        readOnly
    } = props

    const classes = useStyles()

    return (
        <React.Fragment>
            {
                categories.filter(x => x.parent === categoryId).map(category => (
                    <CategoriesTreeItem
                        key={category.id}
                        categories={categories}
                        categoryId={category.id}
                        depth={depth}
                        expanded={expanded}
                        onAddClick={onAddClick}
                        onDeleteClick={onDeleteClick}
                        onEditClick={onEditClick}
                        onCancelClick={onCancelClick}
                        onExpand={onExpand}
                        onMapClick={onMapClick}
                        onMergeClick={onMergeClick}
                        onSaveClick={onSaveClick}
                        onDragHover={onDragHover}
                        updateTree={updateTree}
                        editState={editState}
                        mergeState={mergeState}
                        index={categories.findIndex(x => x.id === category.id)}
                        readOnly={readOnly}
                    />
                ))
            }
            {depth < 5 && (
                <ListItem className={classes.listItem}>
                    <CategoryAddItem
                        depth={depth}
                        editState={editState}
                        onAddClick={onAddClick}
                        onCancelClick={onCancelClick}
                        onSaveClick={onSaveClick}
                        parentId={categoryId}
                        mergeState={mergeState}
                        readOnly={readOnly}
                    />
                </ListItem>
            )}
        </React.Fragment>
    )
}

export default CategoriesTree