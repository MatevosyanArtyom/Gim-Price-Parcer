import React, { useCallback, useEffect, useContext } from 'react'
import { List, ListItem } from '@material-ui/core'
import * as client from 'client'
import { GimDialogContext } from 'components/common/GimDialog'
import useMergeState from 'util/useMergeState'
import useStyles from '../../styles'
import PropertyValueAddItem from './PropertyValueAddItem'
import PropertyValueItem from './PropertyValueItem'
import { EditState, State, Props } from './types'

const initialEditState: EditState = {
    isEditing: false
}

const initialState: State = {
    editState: initialEditState,
    propertyValues: []
}

const PropertyValuesList: React.FC<Props> = ({ expanded, onLoading, propertyId, readOnly }) => {

    const classes = useStyles()
    const gimDialogState = useContext(GimDialogContext)

    const [{ editState, propertyValues }, setState] = useMergeState(initialState)

    const loadPropertyValues = useCallback((propertyId: string) => {
        onLoading(true)
        client.api.categoryPropertyValuesGetMany({ PropertyId: propertyId }, {}).then(v => {
            setState({ propertyValues: v.data })
            onLoading(false)
        })
    }, [onLoading, setState])

    const onEditClick = useCallback((id: string) => {
        setState({ editState: { isEditing: true, id: id } })
    }, [setState])

    const onDeleteClick = useCallback((id: string) => {
        gimDialogState.setState!({
            open: true,
            variant: 'Delete',
            onConfirm: () => {
                onLoading(true)
                client.api.categoryPropertyValuesDeleteOne({ id: id }, {}).then(v => {
                    gimDialogState.setState!({ open: false })
                    loadPropertyValues(propertyId)
                })
            }
        })
    }, [gimDialogState.setState, loadPropertyValues, onLoading, propertyId])

    const onAddClick = useCallback(() => {
        setState({ editState: { isEditing: true } })
    }, [setState])

    const onCancelClick = useCallback(() => {
        setState({ editState: initialEditState })
    }, [setState])

    const onSaveClick = useCallback((values: client.ProductPropertyEdit) => {
        const fn = values.id ? client.api.categoryPropertyValuesUpdateOne : client.api.categoryPropertyValuesAddOne
        fn({ ...values, property: propertyId }).then(v => {
            setState({ editState: initialEditState })
            loadPropertyValues(propertyId)
        })
    }, [loadPropertyValues, propertyId, setState])

    useEffect(() => {
        expanded && loadPropertyValues(propertyId)
    }, [expanded, loadPropertyValues, propertyId])

    return (
        <List className={classes.list}>
            {propertyValues.map(propertyValue => {
                return (
                    <ListItem
                        className={classes.listItem}
                        disableGutters
                        divider
                    >
                        <PropertyValueItem
                            editState={editState}
                            onDeleteClick={onDeleteClick}
                            onCancelClick={onCancelClick}
                            onEditClick={onEditClick}
                            onSaveClick={onSaveClick}
                            value={propertyValue}
                            readOnly={readOnly}
                        />
                    </ListItem>
                )
            })}
            <ListItem
                className={classes.listItem}
                disableGutters
                divider
            >
                <PropertyValueAddItem
                    editState={editState}
                    onAddClick={onAddClick}
                    onCancelClick={onCancelClick}
                    onSaveClick={onSaveClick}
                    readOnly={readOnly}
                />
            </ListItem>
        </List>
    )
}

export default PropertyValuesList