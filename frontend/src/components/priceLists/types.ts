import * as client from 'client'

export type RulesSource = 'Unknown' | 'Code' | 'File'

export const NotFoundStr = 'Не найдено'

export const PriceListStatuses = {
    InQueue: 'В очереди',
    Processing: 'Обрабатывается',
    Errors: 'Ошибки',
    Ready: 'Готов к загрузке',
    Committed: 'Загружен',
    Failed: 'Неудача'
}

export const RulesSources = {
    Code: 'Код скрипта',
    File: 'Файл скрипта'
}

export const initialPriceList: client.PriceListAdd = {
    supplier: '',
    priceListFile: {
        data: '',
        name: '',
        size: 0
    },
    processingRule: ''
}

export type ParsePriceListState = {
    supplier: string
    suppliers: client.SupplierShort[]
    isSuppliersLoading: boolean
    processingRules: client.ProcessingRuleLookup[]
    emitResult: client.EmitResultDto | null
    isLoading: boolean
}

export type PriceListState = {
    priceList: client.PriceListFull
    page: 'items' | 'properties'
    commitError?: boolean
    clearFilter: {}
    forceUpdate: {}
    isLoading: boolean
}