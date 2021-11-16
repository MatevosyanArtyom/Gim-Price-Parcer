import { ArchivedFilter } from 'components/common/ShowArchivedToggler'
import * as client from 'client'

export type GimUserStatus = 'New' | 'Active' | 'Blocked' | 'SystemBlocked'

export const GimUserStatuses = {
    New: 'Новый',
    Active: 'Активен',
    Blocked: 'Заблокирован',
    SystemBlocked: 'Заблокирован (системой)'
}

export type StateType = {
    archivedFilter: ArchivedFilter,
    roles: client.UserRoleLookup[]
    isLoading: boolean
}