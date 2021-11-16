import React from 'react'
import { PropertyValueAddItemProps } from './types'
import RowButtonPlaceholder from 'components/categories/RowButtonPlaceholder'
import ButtonAdd from 'components/common/GimTable/ButtonAdd'
import * as client from 'client'
import PropertyValueEditItem from './PropertyValueEditItem'

export const initialPropertyValue: client.ProductPropertyValueEdit = {
    id: '',
    seqId: 0,
    property: '',
    name: ''
}

const PropertyValueAddItem: React.FC<PropertyValueAddItemProps> = (props) => {

    const {
        editState,
        onAddClick,
        onCancelClick,
        onSaveClick,
        readOnly
    } = props

    if (!editState.isEditing || editState.id) {
        return (
            <React.Fragment>
                <RowButtonPlaceholder />
                <ButtonAdd onClick={() => onAddClick()} disabled={readOnly}>Добавить значение</ButtonAdd>
            </React.Fragment>
        )
    }

    return (
        <PropertyValueEditItem
            onCancelClick={onCancelClick}
            onSaveClick={onSaveClick}
            value={initialPropertyValue}
        />
    )
}

export default PropertyValueAddItem