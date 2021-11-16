import React, { useMemo, useCallback } from 'react'
import { Column } from 'material-table'
import * as client from 'client'
import { Paper, Grid, Typography } from '@material-ui/core'
import GimTable from 'components/common/GimTable'
import { nameofFactory } from 'util/utils'
import useStyles from './styles'
import useMergeState from 'util/useMergeState'
import SupplierProductProperies from './SupplierProductProperties'

type Props = {
    id: string
}

type State = {
    supplierProduct: client.SupplierProductLookup
}

const initialSupplierProduct: client.SupplierProductLookup = {
    id: '',
    code: '',
    name: '',
    price1: 0,
    price2: 0,
    price3: 0,
    description: '',
    supplier: '',
    supplierSeqId: 0,
    product: ''
}

const initialState: State = {
    supplierProduct: initialSupplierProduct
}

const n = nameofFactory<client.SupplierProductLookup>()

const Suppliers: React.FC<Props> = ({ id }) => {

    const classes = useStyles()

    const [{ supplierProduct }, setState] = useMergeState(initialState)

    const columns: Column<client.SupplierProductLookup>[] = useMemo(() => ([
        { title: 'ID поставщика', field: n('supplierSeqId') },
        { title: 'Поставщик', field: n('supplier') },
        { title: 'Артикул', field: n('code') },
        { title: 'Наименование', field: n('name') },
        { title: 'Количество', field: n('quantity') },
        { title: 'Цена', field: n('price1') }
    ]), [])

    const onRowClick = useCallback((_event, rowData) => {
        rowData = rowData as client.SupplierProductLookup
        setState({ supplierProduct: rowData })
    }, [setState])

    return (
        <React.Fragment>
            <Grid item xs={8}>
                <Paper className={classes.paper}>
                    <Typography variant="h5" gutterBottom>Поставщики</Typography>
                    <GimTable
                        title="Наличие у поставщиков"
                        columns={columns}
                        data={query => {
                            return new Promise(resolve => {
                                id
                                    ? client.api.supplierProductsGetMany({
                                        page: query.page,
                                        pageSize: query.pageSize,
                                        ProductId: id
                                    }, {}).then(v => {
                                        resolve({ data: v.data.entities, page: query.page, totalCount: v.data.count })
                                    })
                                    : resolve({ data: [], page: 0, totalCount: 0 })
                            })
                        }}
                        onRowClick={onRowClick}
                        options={{
                            actionsColumnIndex: -1,
                            rowStyle: (rowData) => ({
                                backgroundColor: supplierProduct.id === rowData.id ? 'rgba(0, 0, 0, 0.07)' : undefined
                            })
                        }}
                    />
                </Paper>
            </Grid>
            <Grid item xs={4}>
                <SupplierProductProperies supplierProduct={supplierProduct} />
            </Grid>
        </React.Fragment>)
}

export default Suppliers