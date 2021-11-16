import React, { useContext, useCallback } from 'react'
import update from 'immutability-helper'
import { Grid, Toolbar, Typography, Paper, Button, Box } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import { SortParams } from 'components/common/GimTableNew/types'
import MergeProducts from './MergeProducts'
import ProductsBody from './productsItems/ProductsBody'
import ProductsHeader from './productsItems/ProductsHeader'
import { Filter, initialFilter } from './productsItems/types'
import useStyles from './styles'
import { Props } from './types'
import useMergeState from 'util/useMergeState'

const initialSortParams: SortParams = {}

type State = {
    sortParams: SortParams
    filter: Filter
    mergeProducts: client.ProductLookup[]
}
const initialState: State = {
    sortParams: { ...initialSortParams },
    filter: initialFilter,
    mergeProducts: []
}

const Products: React.FC<Props> = () => {

    const classes = useStyles()

    const [{ filter, mergeProducts }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)

    const readOnly = !context.user.accessRights.products.full

    const onFilterChange = useCallback((filter: Filter) => {
        setState({ filter: filter })
    }, [setState])

    const onMergeAddClick = useCallback((product: client.ProductLookup) => {
        const i = mergeProducts.indexOf(product)
        let newMergeProducts
        if (i > -1) {
            newMergeProducts = update(mergeProducts, { $splice: [[i, 1]] })
        } else {
            newMergeProducts = update(mergeProducts, { $push: [product] })
        }
        setState({ mergeProducts: newMergeProducts })
    }, [mergeProducts, setState])

    const onMergeRemoveClick = useCallback((id: string) => {
        const i = mergeProducts.findIndex(v => v.id === id)
        const newMergeProducts = update(mergeProducts, { $splice: [[i, 1]] })
        setState({ mergeProducts: newMergeProducts })
    }, [mergeProducts, setState])

    const onMergeCancelClick = useCallback(() => {
        setState({ mergeProducts: [] })
    }, [setState])

    const onMergeClick = useCallback((product: client.ProductLookup) => {
        const newMergeProducts = update(mergeProducts, { $push: [product] })
        setState({ mergeProducts: newMergeProducts })
    }, [mergeProducts, setState])

    const onMergeSubmitClick = useCallback(() => {
        client.api.productsMergeMany(mergeProducts.map(v => v.id)).then(v => {
            setState({ mergeProducts: [] })
            onFilterChange({ ...filter })
        })
    }, [filter, mergeProducts, onFilterChange, setState])

    return (
        <Box className={classes.box}>
            <Grid container spacing={2}>
                <Grid item xs={10}>
                    <Paper className={classes.paper}>
                        <Toolbar>
                            <Typography variant="h5">Номенклатура</Typography>
                            <div style={{ flexGrow: 1 }} />
                            <Button color="secondary" onClick={() => {
                                client.api.productsDeleteMany().then(() => onFilterChange({ ...filter }))
                            }}>Удалить все</Button>
                        </Toolbar>
                        <ProductsHeader onChange={onFilterChange} />
                        <ProductsBody
                            filter={filter}
                            mergeProducts={mergeProducts}
                            onMergeAddClick={onMergeAddClick}
                            onMergeCancelClick={onMergeCancelClick}
                            onMergeClick={onMergeClick}
                            readOnly={readOnly}
                        />
                    </Paper>
                </Grid>
                <Grid item xs={2}>
                    <MergeProducts
                        products={mergeProducts}
                        onMergeCancelClick={onMergeCancelClick}
                        onMergeRemoveClick={onMergeRemoveClick}
                        onMergeSubmitClick={onMergeSubmitClick}
                    />
                </Grid>
            </Grid>
        </Box>
    )
}

export default Products