import React, { useCallback } from 'react'
import clsx from 'clsx'
import { ListItemText, IconButton } from '@material-ui/core'
import { ChevronRight, ExpandMore, } from '@material-ui/icons'
import * as client from 'client'
import { EntityStatuses } from 'components/common/types'
import DragIcon from './DragIcon'
import RowButtonPlaceholder from './RowButtonPlaceholder'
import RowIconButton from './RowIconButton'
import useStyles from './styles'

const CategoryRow: React.FC<{
    category: client.CategoryLookup,
    depth: any,
    dragMove: any,
    dragNewParent: any,
    expanded: any,
    onDeleteClick: any,
    onEditClick: any,
    onMapClick: any,
    onMergeClick: any,
    onExpand: any,
    mergeState: any
    readonly: boolean
}> = ({
    category,
    depth,
    dragMove,
    dragNewParent,
    expanded,
    onDeleteClick,
    onEditClick,
    onMapClick,
    onMergeClick,
    onExpand,
    mergeState,
    readonly
}) => {

        const classes = useStyles()

        const onEdit = useCallback(() => onEditClick(category.id), [onEditClick, category.id])
        const onMap = useCallback(() => onMapClick(category), [onMapClick, category,])
        const onMerge = useCallback(() => onMergeClick(category, depth), [onMergeClick, category, depth])
        const onDelete = useCallback(() => onDeleteClick(category.id), [onDeleteClick, category.id])

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

        const deleteDisabled = readonly || category.hasChildren || Boolean(category.productsCount)

        return (
            <React.Fragment>
                {staticPaddingLeft}
                {depth < 4 ? (
                    <IconButton
                        className={classes.rowButton}
                        onClick={e => {
                            e.stopPropagation()
                            onExpand(category.id)
                        }}
                    >
                        {expanded[category.id] ? <ExpandMore fontSize="small" /> : <ChevronRight fontSize="small" />}
                    </IconButton>
                ) : <RowButtonPlaceholder />
                }
                <ListItemText
                    primary={category.name}
                    primaryTypographyProps={{ className: classes.categoryItemListItemText }}
                    className={clsx(classes.flex1percent, classes.noWrap)}
                />
                {staticPaddingRight}
                <ListItemText
                    primary={category.productsCount}
                    primaryTypographyProps={{ className: classes.categoryItemListItemText }}
                    className={classes.productsCountColumn}
                    style={{ textAlign: 'end' }}
                />
                <ListItemText
                    primary={EntityStatuses[category.status || 'Unknown']}
                    primaryTypographyProps={{ className: classes.categoryItemListItemText }}
                    className={clsx(classes.flex02, classes.marginRight1, classes.minWidth105)}
                />
                <div className={classes.divButtonsContainer}>
                    <DragIcon ref={dragMove} type="move" disabled={readonly} />
                    <DragIcon ref={dragNewParent} type="newParent" disabled={readonly} />
                    <RowIconButton
                        type="edit"
                        category={category}
                        depth={depth}
                        mergeState={mergeState}
                        onClick={onEdit}
                        disabled={readonly}
                    />
                    <RowIconButton
                        type="map"
                        category={category}
                        depth={depth}
                        mergeState={mergeState}
                        onClick={onMap}
                        disabled={readonly}
                    />
                    <RowIconButton
                        type="merge"
                        category={category}
                        depth={depth}
                        mergeState={mergeState}
                        onClick={onMerge}
                        disabled={readonly}
                    />
                    <RowIconButton
                        type="delete"
                        category={category}
                        depth={depth}
                        mergeState={mergeState}
                        onClick={onDelete}
                        disabled={deleteDisabled}
                    />
                </div>
            </React.Fragment>
        )
    }

export default React.memo(CategoryRow)