import React from 'react'
import ProductPropertyEditItem from './ProductPropertyEditItem'
import ProductPropertyRow from './ProductPropertyRow'
import { ProductPropertyItemProps } from './types'

const ProductPropertyItem: React.FC<ProductPropertyItemProps> = ({
    editState,
    expanded,
    onDeleteClick,
    onCancelClick,
    onEditClick,
    onExpand,
    onSaveClick,
    value,
    readOnly
}) => {

    return editState.isEditing && editState.id === value.id
        ? (
            <ProductPropertyEditItem
                onCancelClick={onCancelClick}
                onSaveClick={onSaveClick}
                value={value}
            />
        )
        : (
            <ProductPropertyRow
                editState={editState}
                expanded={expanded}
                onDeleteClick={onDeleteClick}
                onEditClick={onEditClick}
                onExpand={onExpand}
                value={value}
                readOnly={readOnly}
            />
        )
}

export default ProductPropertyItem