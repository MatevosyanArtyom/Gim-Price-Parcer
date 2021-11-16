import * as client from 'client'

export type Props = {
    expanded: boolean,
    onLoading: (isLoading: boolean) => void,
    propertyId: string,
    readOnly: boolean
}

export type EditState = {
    isEditing: boolean,
    id?: string
}

export type State = {
    editState: EditState,
    propertyValues: client.ProductPropertyValueLookup[]
}

export type PropertyValueAddItemProps = {
    editState: EditState,
    onAddClick: () => void,
    onCancelClick: () => void,
    onSaveClick: (item: client.ProductPropertyValueEdit) => void,
    readOnly: boolean
}

export type PropertyValueEditItemProps = {
    onCancelClick: () => void,
    onSaveClick: (values: client.ProductPropertyValueEdit) => void,
    value: client.ProductPropertyValueEdit
}

export type PropertyValueItemProps = {
    editState: EditState,
    onDeleteClick: (id: string) => void,
    onCancelClick: () => void,
    onEditClick: (id: string) => void,
    onSaveClick: (item: client.ProductPropertyEdit) => void,
    value: client.ProductPropertyLookup,
    readOnly: boolean
}

export type PropertyValueRowProps = {
    editState: EditState,
    onDeleteClick: (id: string) => void,
    onEditClick: (id: string) => void,
    value: client.ProductPropertyLookup,
    readOnly: boolean
}