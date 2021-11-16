import React, { useEffect, useCallback } from 'react'
import { withRouter } from 'react-router'
import { Grid, Paper, Typography, Box } from '@material-ui/core'
import * as client from 'client'
import promiseWithHttpResponse from 'util/promiseWithHttpResponse'
import { RouteComponentPropsWithId } from 'util/types'
import useMergeState from 'util/useMergeState'
import Images from './Images'
import PageToggler, { ProductPage } from './PageToggler'
import ProductImages from './ProductImages'
import ProductMain from './ProductMain'
import ProductProperies from './ProductProperties'
import useStyles from './styles'
import Suppliers from './Suppliers'
import { initialProduct } from './types'

type State = {
    page: ProductPage
    product: client.ProductEdit
    productLookup: client.ProductLookup
    description: string
    isLoading: boolean
}

const initialState: State = {
    page: 'product',
    product: initialProduct,
    productLookup: initialProduct,
    description: '',
    isLoading: false
}

const Product: React.FC<RouteComponentPropsWithId> = (props) => {

    const id = props.match.params.id === 'add' ? '' : props.match.params.id

    const classes = useStyles()

    let [{ page, product, isLoading }, setState] = useMergeState(initialState)

    const onPageChange = useCallback((page: ProductPage) => {
        setState({ page: page })
    }, [setState])

    useEffect(() => {

        setState({ isLoading: true })

        let promises: [
            Promise<client.HttpResponse<client.ProductEdit>>
        ] = [
                id ? client.api.productsGetOne(id) : promiseWithHttpResponse(initialProduct)
            ]

        Promise.all(promises).then(results => {
            setState({
                product: results[0].data,
                isLoading: false
            })
        })
    }, [id, setState])

    return (
        <Box className={classes.box}>
            <Grid container spacing={2}>
                <Grid item xs={6}>
                    <Paper className={classes.paper}>
                        <Typography variant="h4" gutterBottom style={{ minHeight: '1.25em' }}>{product.name}</Typography>
                        <PageToggler page={page} onChange={onPageChange} isLoading={isLoading} />
                    </Paper>
                </Grid>
                <Grid item xs={6} />
                {page === 'product' && (
                    <React.Fragment>
                        <Grid item xs={4}>
                            <ProductImages id={id} />
                        </Grid>
                        <Grid item xs={4}>
                            <ProductMain id={id} />
                        </Grid>
                        <Grid item xs={4}>
                            <ProductProperies id={id} />
                        </Grid>
                    </React.Fragment>
                )}
                {page === 'suppliers' && <Suppliers id={id} />}
                {page === 'images' && <Images id={id} />}
            </Grid>
        </Box>
    )
}

export default withRouter(Product)