import React, { useEffect, useCallback, useRef, useContext } from 'react'
import clsx from 'clsx'
import { Paper, Typography, TextField, Button } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import GimCircularProgress from 'components/common/GimCircularProgress'
import useMergeState from 'util/useMergeState'
import useStyles from './styles'
import { initialProduct } from './types'

type Props = {
    id: string
}

type State = {
    product: client.ProductLookup
    description: string
    isLoading: boolean
}

const initialState: State = {
    product: initialProduct,
    description: '',
    isLoading: false
}

const ProductMain: React.FC<Props> = ({ id }) => {

    const classes = useStyles()

    const [{ product, description, isLoading }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)

    const readOnly = !context.user.accessRights.products.full

    const inputRef = useRef<any>(null)

    const loadProduct = useCallback((id: string) => {
        if (!id) { return }
        setState({ isLoading: true })
        client.api.productsGetLookupOne(id).then(v => {
            setState({ product: v.data, description: v.data.description || '', isLoading: false })
        })
    }, [setState])

    const onDescriptionChange = useCallback((event: React.ChangeEvent<HTMLInputElement>) => {
        setState({ description: event.target.value })
    }, [setState])

    const onDescriptionSubmit = useCallback(() => {
        setState({ isLoading: true })
        client.api.productsSetDescriptionOne(product.id, { description: description }).then(v => {
            loadProduct(id)
        })
    }, [description, id, loadProduct, product.id, setState])

    useEffect(() => {
        loadProduct(id)
    }, [id, loadProduct])

    return (
        <Paper className={clsx(classes.paper, classes.positionRelative)}>
            <GimCircularProgress isLoading={isLoading} />
            <Typography variant="h5" gutterBottom>Базовое описание</Typography>
            <TextField
                label="ID"
                value={product.seqId}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Наименование"
                value={product.name}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Категория 1"
                value={product.category1}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Категория 2"
                value={product.category2}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Категория 3"
                value={product.category3}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Категория 4"
                value={product.category4}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Категория 5"
                value={product.category5}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Цена от, Р"
                value={product.priceFrom === null ? 'Нет цен' : product.priceFrom}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                ref={inputRef}
                label="Описание"
                value={description}
                onChange={onDescriptionChange}
                margin="dense"
                rows="5"
                InputProps={{ readOnly: readOnly }}
                multiline
                fullWidth
            />
            <div className={classes.margintop1}>
                <Button
                    color="primary"
                    variant="contained"
                    onClick={onDescriptionSubmit}
                    disabled={readOnly}
                >
                    Сохранить
                </Button>
            </div>
        </Paper>
    )
}

export default ProductMain