import { RouteComponentProps } from 'react-router'
import { StyledComponentProps } from '@material-ui/core'
import * as client from 'client'

export const initialProduct: client.ProductEdit = {
    id: '',
    seqId: 0,
    name: '',
    category: '',
    category1: '',
    category2: '',
    category3: '',
    category4: '',
    category5: '',
    supplier: '',
    manufacturer: '',
    priceFrom: 0,
    description: '',
    status: 'Active'
}

export type Params = {
    supplierId: string
}

export type Props = RouteComponentProps<Params> & StyledComponentProps