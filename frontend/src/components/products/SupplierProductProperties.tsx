import React, { useEffect, useCallback } from 'react'
import clsx from 'clsx'
import _ from 'lodash'
import { Paper, Typography, TextField } from '@material-ui/core'
import * as client from 'client'
import useMergeState from 'util/useMergeState'
import useStyles from './styles'
import GimCircularProgress from 'components/common/GimCircularProgress'
import { Dictionary } from 'lodash'

type Props = {
    supplierProduct: client.SupplierProductLookup
}

type State = {
    properties: client.ProductPropertyValueLookup[]
    allProperties: Dictionary<client.ProductPropertyValueLookup[]>
    isLoading: boolean
}

const initialState: State = {
    properties: [],
    allProperties: {},
    isLoading: false
}

const SupplierProductProperies: React.FC<Props> = ({ supplierProduct }) => {

    const classes = useStyles()

    const [{ properties, isLoading }, setState] = useMergeState(initialState)

    const loadProperties = useCallback((id: string) => {
        if (!id) { return }
        setState({ isLoading: true })
        client.api.supplierProductsGetProperties(id).then(v => {
            setState({ properties: v.data.values || [], allProperties: _.groupBy(v.data.allValues, v => v.propertyId), isLoading: false })
        })
    }, [setState])

    useEffect(() => {
        loadProperties(supplierProduct.id)
    }, [loadProperties, supplierProduct.id])

    return (
        <Paper className={clsx(classes.paper, classes.positionRelative)}>
            <GimCircularProgress isLoading={isLoading} />
            <Typography variant="h5" gutterBottom>Описание позиции у поставщика</Typography>
            <TextField
                label="Артикул"
                value={supplierProduct.code}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Наименование"
                value={supplierProduct.name}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Цена 1"
                value={supplierProduct.price1 || ''}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Цена 2"
                value={supplierProduct.price2 || ''}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Цена 3"
                value={supplierProduct.price3 || ''}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Описание"
                value={supplierProduct.description}
                InputProps={{ readOnly: true }}
                margin="dense"
                rows="5"
                multiline
                fullWidth
            />
            {properties.map(v => (
                <TextField
                    key={v.id}
                    label={v.property}
                    value={v.name}
                    margin="dense"
                    fullWidth
                />
            ))}
        </Paper>
    )
}

export default SupplierProductProperies