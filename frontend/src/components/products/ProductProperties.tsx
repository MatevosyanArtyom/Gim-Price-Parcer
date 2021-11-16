import React, { useEffect, useCallback, useContext } from 'react'
import clsx from 'clsx'
import _, { Dictionary } from 'lodash'
import { Paper, Typography, TextField, MenuItem } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import GimCircularProgress from 'components/common/GimCircularProgress'
import useMergeState from 'util/useMergeState'
import useStyles from './styles'

type Props = {
    id: string
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

const ProductProperies: React.FC<Props> = ({ id }) => {

    const classes = useStyles()

    const [{ properties, allProperties, isLoading }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)

    const readOnly = !context.user.accessRights.products.full

    const loadProperties = useCallback((id: string) => {
        if (!id) { return }
        setState({ isLoading: true })
        client.api.productsGetProperties(id).then(v => {
            setState({ properties: v.data.values || [], allProperties: _.groupBy(v.data.allValues, v => v.propertyId), isLoading: false })
        })
    }, [setState])

    const onValueChange = useCallback((oldId: string, newId: string) => {
        if (oldId === newId) { return }
        setState({ isLoading: true })
        client.api.productsSetPropertyValueOne(id, oldId, newId).then(v => {
            loadProperties(id)
        })
    }, [id, loadProperties, setState])

    useEffect(() => {
        loadProperties(id)
    }, [id, loadProperties])

    return (
        <Paper className={clsx(classes.paper, classes.positionRelative)}>
            <GimCircularProgress isLoading={isLoading} />
            <Typography variant="h5" gutterBottom>Характеристики</Typography>
            {properties.map(v => (
                <TextField
                    key={v.id}
                    label={v.property}
                    value={v.id}
                    onChange={e => onValueChange(v.id, e.target.value)}
                    margin="dense"
                    InputProps={{ readOnly: readOnly }}
                    select
                    fullWidth
                >
                    {/* TODO: Убрать slice */}
                    {allProperties[v.propertyId].slice(0, 10).map(x => <MenuItem key={x.id} value={x.id}>{x.name}</MenuItem>)}
                </TextField>
            ))}
        </Paper>
    )
}

export default ProductProperies