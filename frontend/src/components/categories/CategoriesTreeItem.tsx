import React, { useRef } from 'react'
import { useDrag, useDrop } from 'react-dnd'
import clsx from 'clsx'
import { XYCoord } from 'dnd-core'
import { List, ListItem, Collapse } from '@material-ui/core'
import * as client from 'client'
import CategoriesTree from './CategoriesTree'
import CategoryEditItem from './CategoryEditItem'
import CategoryRow from './CategoryRow'
import useStyles from './styles'
import { CategoriesTreeItemProps, DragObjectType } from './types'

const CategoriesTreeItem: React.FC<CategoriesTreeItemProps> = (props) => {

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
        index,
        readOnly
    } = props

    const classes = useStyles()

    const category = categories.find(x => x.id === categoryId)!

    const isEditing = editState.isEditing && editState.categoryId === category.id

    let [{ opacityMove }, dragMove, previewMove] = useDrag<DragObjectType, DragObjectType, any>({
        item: { type: 'CategoryItem', mode: 'move', depth: depth, beginIndex: index, endIndex: index, category },
        collect: monitor => {
            return {
                opacityMove: monitor.isDragging() ? 0 : 1,
            }
        },
        end: (item, monitor) => {
            if (!item || item.mode !== 'move') {
                return
            }
            if (item.beginIndex === item.endIndex) {
                return
            }

            const model: client.MoveCategoryModel = {
                afterId: item.endIndex! > 0 ? categories[item.endIndex! - 1].id : ''
            }

            client.api.categoriesMoveOne(item.category.id, model).then(v => {
                updateTree()
            })
        },
        canDrag: !editState.isEditing && !readOnly
    })

    let [{ opacityNewParent }, dragNewParent, previewNewParent] = useDrag<DragObjectType, DragObjectType, any>({
        item: { type: 'CategoryItem', mode: 'newParent', depth: depth, category: category },
        collect: monitor => {
            return {
                opacityNewParent: monitor.isDragging() ? 0.5 : 1,
            }
        },
        canDrag: !editState.isEditing && !readOnly
    })

    const [{ item, canDrop, isOver }, drop,] = useDrop<DragObjectType, any, any>({
        accept: 'CategoryItem',
        collect: monitor => ({
            canDrop: monitor.canDrop(),
            item: monitor.getItem(),
            isOver: monitor.isOver()
        }),
        canDrop: (item, monitor) => {
            if (item.mode === 'move') {
                return true
            }
            const result =
                // can't change parent to itself or to child category
                (item.category.id !== category.id && (category.path || '').indexOf(item.category.id) === -1) &&
                // root category cannot be changed
                (Boolean(item.category.parent)) &&
                // max depth
                (depth < 4) &&

                (item.depth > depth) &&
                // root category must be the same
                (item.category.rootParent === category.rootParent) &&
                // must have no children
                ((!item.category.hasChildren && !category.hasChildren) || item.depth === depth)
            return result
        },
        drop: (item, monitor) => {
            if (item.mode !== 'newParent') {
                return
            }
            if (item.category.id === category.id) {
                return
            }
            client.api.categoriesUpdateParent({ id: item.category.id, newParentId: category.id }).then(() => {
                updateTree()
            })
        },
        hover: (item, monitor) => {
            if (item.mode !== 'move' || item.category.parent !== category.parent) {
                return
            }
            const dragIndex = item.endIndex
            const hoverIndex = index

            // Don't replace items with themselves
            if (dragIndex === hoverIndex) {
                return
            }

            const hoverBoundingRect = ref.current!.getBoundingClientRect()

            // Get vertical middle
            const hoverMiddleY =
                (hoverBoundingRect.bottom - hoverBoundingRect.top) / 2

            // Determine mouse position
            const clientOffset = monitor.getClientOffset()

            // Get pixels to the top
            const hoverClientY = (clientOffset as XYCoord).y - hoverBoundingRect.top

            // Only perform the move when the mouse has crossed half of the items height
            // When dragging downwards, only move when the cursor is below 50%
            // When dragging upwards, only move when the cursor is above 50%

            // Dragging downwards
            if (dragIndex! < hoverIndex && hoverClientY < hoverMiddleY) {
                return
            }

            // Dragging upwards
            if (dragIndex! > hoverIndex && hoverClientY > hoverMiddleY) {
                return
            }

            onDragHover(dragIndex!, hoverIndex)

            item.endIndex = hoverIndex
        }
    })

    const ref = useRef<any>(null)
    drop(ref)

    const renderEditForm = () => (
        <CategoryEditItem
            category={category}
            depth={depth}
            onCancelClick={onCancelClick}
            onSaveClick={onSaveClick}
            editState={editState}
            mergeState={mergeState}
        />
    )
    const borderGreen = isOver && item && item.mode === 'newParent' && canDrop && classes.borderGreen
    const borderRed = isOver && item && item.mode === 'newParent' && !canDrop && classes.borderRed
    const borderBottom = !borderGreen && !borderRed && classes.borderBottom
    return (
        <React.Fragment key={category.id}>
            <div ref={previewMove}>
                <div ref={previewNewParent}>
                    <ListItem
                        ref={ref}
                        id={category.id}
                        className={clsx(classes.listItem, borderBottom, borderGreen, borderRed)}
                        style={{
                            opacity: Math.min(opacityMove, opacityNewParent)
                        }}
                    >
                        {isEditing ? renderEditForm() : (
                            <CategoryRow
                                category={category}
                                expanded={expanded}
                                onExpand={onExpand}
                                depth={depth}
                                dragMove={dragMove}
                                dragNewParent={dragNewParent}
                                onDeleteClick={onDeleteClick}
                                onEditClick={onEditClick}
                                onMapClick={onMapClick}
                                onMergeClick={onMergeClick}
                                mergeState={mergeState}
                                readonly={readOnly}
                            />
                        )}
                    </ListItem>
                </div>
            </div>
            <Collapse in={expanded[category.id]}>
                <List className={classes.list}>
                    <CategoriesTree
                        categories={categories}
                        categoryId={categoryId}
                        depth={depth + 1}
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
                        readOnly={readOnly}
                    />
                </List>
            </Collapse>
        </React.Fragment >
    )
}
export default CategoriesTreeItem