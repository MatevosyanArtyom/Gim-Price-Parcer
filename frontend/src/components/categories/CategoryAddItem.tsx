import React from 'react'
import { CategoryAddItemProps } from './types'
import ButtonAdd from 'components/common/GimTable/ButtonAdd'
import CategoryEditItem from './CategoryEditItem'
import { initialCategory } from './Category'
import RowButtonPlaceholder from './RowButtonPlaceholder'
import { ListItemText } from '@material-ui/core'
import useStyles from './styles'
import clsx from 'clsx'

const CategoryAddItem: React.FC<CategoryAddItemProps> = (props) => {

    const {
        depth,
        editState,
        onAddClick,
        onCancelClick,
        onSaveClick,
        parentId,
        mergeState,
        readOnly
    } = props

    const classes = useStyles()

    if (!editState.isEditing || editState.parentId !== parentId) {
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
            <React.Fragment>
                {staticPaddingLeft}
                <RowButtonPlaceholder />
                <div className={classes.flex1percent}>
                    <ButtonAdd
                        onClick={() => onAddClick(parentId)}
                        className={classes.negativeMargin36}
                        disabled={readOnly}
                    />
                </div>
                {staticPaddingRight}
                <ListItemText className={classes.productsCountColumn} />
                <ListItemText className={clsx(classes.flex02, classes.marginRight1, classes.minWidth105)} />
                <RowButtonPlaceholder />
                <RowButtonPlaceholder />
                <RowButtonPlaceholder />
                <RowButtonPlaceholder />
                <RowButtonPlaceholder />
                <RowButtonPlaceholder />
            </React.Fragment>
        )
    }

    return (
        <CategoryEditItem
            category={{ ...initialCategory, parent: parentId }}
            depth={depth}
            editState={editState}
            onCancelClick={onCancelClick}
            onSaveClick={onSaveClick}
            mergeState={mergeState}
        />
    )
}

export default CategoryAddItem