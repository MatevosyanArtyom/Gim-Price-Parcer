import * as client from 'client'
import { ArchivedFilter } from 'components/common/ShowArchivedToggler'


export const initialAccessRights: client.AccessRightsDto = {
    suppliers: {},
    priceListAdd: {},
    priceLists: {},
    commitedPriceLists: {},
    products: {},
    userRoles: {},
    users: {},
    categories: {},
    properties: {},
    processingRules: {}
}

export type StateType = {
    archivedFilter: ArchivedFilter,
    isLoading: boolean
}
