import * as client from 'client'

export type Props = {
    categoryId: string
    hasChildren: boolean
    readOnly: boolean
}

export type State = {
    editState: EditState
    expanded: { [id: string]: boolean }
    isLoading: boolean
    properties: client.ProductPropertyLookup[]
}

export type EditMode = 'property' | 'value'

export type EditState = {
    isEditing: boolean
    mode?: EditMode
    id?: string
}

export type ProductPropertyAddItemProps = {
    readOnly: boolean
    editState: EditState
    onAddClick: (mode: EditMode) => void
    onCancelClick: () => void
    onSaveClick: (item: client.ProductPropertyEdit) => void
}

export type ProductPropertyEditItemProps = {
    onCancelClick: () => void
    onSaveClick: (values: client.ProductPropertyEdit) => void
    value: client.ProductPropertyEdit
}

export type ProductPropertyItemProps = {
    editState: EditState
    expanded: boolean
    onDeleteClick: (id: string) => void
    onCancelClick: () => void
    onEditClick: (id: string) => void
    onExpand: (id: string) => void
    onSaveClick: (item: client.ProductPropertyEdit) => void
    value: client.ProductPropertyLookup
    readOnly: boolean
}

export type ProductPropertyRowProps = {
    editState: EditState
    expanded: boolean
    onDeleteClick: (id: string) => void
    onEditClick: (id: string) => void
    onExpand: (id: string) => void
    value: client.ProductPropertyLookup
    readOnly: boolean
}