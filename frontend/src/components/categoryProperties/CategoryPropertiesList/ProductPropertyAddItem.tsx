import React from 'react'
import ButtonAdd from 'components/common/GimTable/ButtonAdd'
import * as client from 'client'
import ProductPropertyEditItem from './ProductPropertyEditItem'
import { ProductPropertyAddItemProps } from './types'

export const initialProductProperty: client.ProductPropertyEdit = {
    id: '',
    seqId: 0,
    category: '',
    name: '',
    key: ''
}

const ProductPropertyAddItem: React.FC<ProductPropertyAddItemProps> = (props) => {

    const {
        readOnly,
        editState,
        onAddClick,
        onCancelClick,
        onSaveClick,
    } = props

    if (!editState.isEditing || editState.id) {
        return <ButtonAdd onClick={() => onAddClick('property')} disabled={readOnly}>Добавить характеристику</ButtonAdd>
    }

    return (
        <ProductPropertyEditItem
            onCancelClick={onCancelClick}
            onSaveClick={onSaveClick}
            value={initialProductProperty}
        />
    )
}

export default ProductPropertyAddItem