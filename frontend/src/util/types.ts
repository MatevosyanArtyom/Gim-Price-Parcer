import { RouteComponentProps } from 'react-router'

export type ParamsId = {
    id: string
}

export type RouteComponentPropsWithId = RouteComponentProps<ParamsId>