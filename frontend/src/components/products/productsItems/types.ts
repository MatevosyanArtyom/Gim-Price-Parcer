
export type Filter = {
    seqId?: number
    category1: string
    category2: string
    category3: string
    category4: string
    category5: string
    name: string
    status: any
}

// 1521
export const widths = {
    id: 80,
    category: 145, // x5
    name: 215,
    priceFrom: 90,
    suppliersCount: 105,
    imagesCount: 70,
    status: 120,
    actions: 116,
}

export const initialFilter: Filter = {
    seqId: '' as any, // 0 будет отображаться в инпуте
    category1: '',
    category2: '',
    category3: '',
    category4: '',
    category5: '',
    name: '',
    status: ''
}