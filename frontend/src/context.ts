import React from 'react'
import * as api from 'api'
import { initialAccessRights } from 'components/userRoles/types'

export const initialUser: api.UserLookup = {
    id: '',
    seqId: 0,
    email: '',
    roleId: '',
    fullname: '',
    position: '',
    phoneNumber: '',
    hasSuppliersAccess: false,
    hasFullAccess: false,
    hasUsersAccess: false,
    accessRights: initialAccessRights,
    createdDate: '',
    status: 'Unknown'
}

export const initialContext = {
    user: initialUser
}

const AppContext = React.createContext(initialContext)

export default AppContext