
export type PriceListItemAction = 'Unknown' | 'CreateNew' | 'LeaveOld' | 'ApplyNew'
export type PriceListItemStatus = 'Error' | 'Ok'

export type Filter = {
    code: string
    category1Name: string
    category2Name: string
    category3Name: string
    category4Name: string
    category5Name: string
    productNameRegEx: string
    price1: number
    price2: number
    price3: number
    status: any
}

export const NotFoundStr = 'Не найдено'

export const PriceListItemStatuses = {
    Error: 'Ошибки',
    Fixed: 'Исправлено',
    Ok: 'OK'
}

// 1839
export const widths = {
    code: 130,
    category: 180, // x5
    name: 310,
    price: 100, // x3
    status: 120,
    actions: 79,
}

export const initialFilter: Filter = {
    code: '',
    category1Name: '',
    category2Name: '',
    category3Name: '',
    category4Name: '',
    category5Name: '',
    productNameRegEx: '',
    price1: '' as any,
    price2: '' as any,
    price3: '' as any,
    status: ''
}