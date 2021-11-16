import React from 'react'
import PropertyValueEditItem from './PropertyValueEditItem'
import PropertyValueRow from './PropertyValueRow'
import { PropertyValueItemProps } from './types'

const PropertyValueItem: React.FC<PropertyValueItemProps> = ({
    editState,
    onDeleteClick,
    onCancelClick,
    onEditClick,
    onSaveClick,
    value,
    readOnly
}) => {

    return editState.isEditing && editState.id === value.id
        ? (
            <PropertyValueEditItem
                onCancelClick={onCancelClick}
                onSaveClick={onSaveClick}
                value={value}
            />
        )
        : (
            <PropertyValueRow
                editState={editState}
                onDeleteClick={onDeleteClick}
                onEditClick={onEditClick}
                value={value}
                readOnly={readOnly}
            />
        )
}

export default PropertyValueItem