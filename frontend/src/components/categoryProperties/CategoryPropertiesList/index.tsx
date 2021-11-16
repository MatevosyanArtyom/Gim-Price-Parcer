import React, { useCallback, useEffect, useContext } from 'react'
import { List, ListItem, Collapse, Box } from '@material-ui/core'
import clsx from 'clsx'
import update from 'immutability-helper'
import * as client from 'client'
import GimCircularProgress from 'components/common/GimCircularProgress'
import { GimDialogContext } from 'components/common/GimDialog'
import useMergeState from 'util/useMergeState'
import useStyles from '../styles'
import ProductPropertyAddItem from './ProductPropertyAddItem'
import ProductPropertyItem from './ProductPropertyItem'
import PropertyValuesList from './PropertyValuesList'
import { EditState, EditMode, State, Props } from './types'

const initialEditState: EditState = {
    isEditing: false
}

const initialState: State = {
    editState: initialEditState,
    expanded: {},
    isLoading: false,
    properties: []
}

const ProductPropertiesList: React.FC<Props> = ({ categoryId, hasChildren, readOnly }) => {

    const classes = useStyles()
    const gimDialogState = useContext(GimDialogContext)

    const [{ editState, expanded, isLoading, properties }, setState] = useMergeState(initialState)

    const loadProductProperties = useCallback((categoryId: string) => {
        if (!categoryId) { return }
        setState({ isLoading: true })
        client.api.categoryPropertiesGetMany({ CategoryId: categoryId }).then(v => {
            setState({ properties: v.data.entities, isLoading: false })
        })
    }, [setState])

    const onEditClick = useCallback((id: string) => {
        setState({ editState: { isEditing: true, id: id } })
    }, [setState])

    const onDeleteClick = useCallback((id: string) => {
        gimDialogState.setState!({
            open: true,
            variant: 'Delete',
            onConfirm: () => {
                setState({ isLoading: true })
                client.api.categoryPropertiesDeleteOne({ id: id })
                    .then(v => {
                        loadProductProperties(categoryId)
                    })
                    .finally(() => gimDialogState.setState!({ open: false }))
            }
        })
    }, [categoryId, gimDialogState.setState, loadProductProperties, setState])

    const onAddClick = useCallback((mode: EditMode) => {
        setState({ editState: { isEditing: true, mode: mode } })
    }, [setState])

    const onCancelClick = useCallback(() => {
        setState({ editState: initialEditState })
    }, [setState])

    const onExpand = useCallback((id: string) => {
        if (expanded[id]) {
            setState({
                expanded: update(expanded, { [id]: { $set: false } })
            })
        } else {
            setState({
                expanded: update(expanded, { [id]: { $set: !expanded[id] } })
            })
        }

    }, [expanded, setState])

    const onSaveClick = useCallback((values: client.ProductPropertyEdit) => {
        const fn = values.id ? client.api.categoryPropertiesUpdateOne : client.api.categoryPropertiesAddOne
        fn({ ...values, category: categoryId }).then(v => {
            setState({ editState: initialEditState })
            loadProductProperties(categoryId)
        })
    }, [categoryId, loadProductProperties, setState])

    const onLoading = useCallback((isLoading: boolean) => {
        setState({ isLoading: isLoading })
    }, [setState])

    useEffect(() => {
        setState({ expanded: {}, properties: [] })
        loadProductProperties(categoryId)
    }, [categoryId, loadProductProperties, setState])

    return (
        <Box className={classes.box}>
            <GimCircularProgress isLoading={isLoading} />
            <List className={clsx(classes.list, classes.rootList)}>
                {properties.map(property => {
                    return (
                        <React.Fragment key={property.id}>
                            <ListItem
                                className={classes.listItem}
                                disableGutters
                                divider
                            >
                                <ProductPropertyItem
                                    editState={editState}
                                    expanded={expanded[property.id]}
                                    onDeleteClick={onDeleteClick}
                                    onCancelClick={onCancelClick}
                                    onEditClick={onEditClick}
                                    onExpand={onExpand}
                                    onSaveClick={onSaveClick}
                                    value={property}
                                    readOnly={readOnly}
                                />
                            </ListItem>
                            <Collapse in={expanded[property.id]}>
                                <PropertyValuesList
                                    propertyId={property.id}
                                    expanded={expanded[property.id]}
                                    onLoading={onLoading}
                                    readOnly={readOnly}
                                />
                            </Collapse>
                        </React.Fragment>
                    )
                })}
                <ListItem
                    className={classes.listItem}
                    disableGutters
                    divider
                >
                    <ProductPropertyAddItem
                        readOnly={!categoryId || readOnly || hasChildren}
                        editState={editState}
                        onAddClick={onAddClick}
                        onCancelClick={onCancelClick}
                        onSaveClick={onSaveClick}
                    />
                </ListItem>
            </List>
        </Box>
    )
}

export default ProductPropertiesList