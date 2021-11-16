import { RouteComponentProps } from 'react-router'

type ParamsToken = {
    token: string
}

export type RouteComponentPropsWithToken = RouteComponentProps<ParamsToken>

export type UserStatus = 'Unknown' | 'New' | 'Active' | 'Blocked' | 'SystemBlocked'

export type PasswordChangeFormProps = {
    token: string,
    isTokenValid?: boolean,
    email: string,
    status: UserStatus,
    password: string,
    passwordConfirm: string
}

export type StateType = {
    isTokenValid?: boolean,
    email?: string,
    status?: string
}