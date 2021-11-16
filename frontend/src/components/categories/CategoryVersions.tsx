import React from 'react'
import { Column, Action, Query } from 'material-table'
import { Container } from '@material-ui/core'
import * as client from 'client'
import GimTable from 'components/common/GimTable'
import useMergeState from 'util/useMergeState'

const initialState = {
    isLoading: false
}

const versionСolumns: Column<client.CategoryLookup>[] = [
    { title: 'Версия', field: 'version' },
    { title: 'Наименование', field: 'name' },
    { title: 'Родитель', field: 'parent' }
]

const CategoryVersions: React.FC = (props) => {

    const [{ isLoading }, setState] = useMergeState(initialState)

    const versionActions: Action<client.CategoryLookup>[] = [
        {
            icon: 'undo',
            tooltip: 'Восстановить эту версию',
            onClick: (event, rowData) => {
                rowData = rowData as client.CategoryLookup
                setState({ isLoading: true })
                client.api.categoriesRestoreVersion(rowData.version || '').then(v => {
                    setState({ isLoading: false })
                })
            }
        }
    ]

    return (
        <Container>
            <GimTable
                title="Версии"
                columns={versionСolumns}
                actions={versionActions}
                data={(query: Query<client.CategoryLookup>) => {
                    return new Promise(resolve => {
                        client.api.categoriesGetVersions({ page: query.page, pageSize: query.pageSize }, {}).then(
                            v => resolve({ data: v.data.entities, page: query.page, totalCount: v.data.count })
                        )
                    })
                }}
                isLoading={isLoading}
            />
        </Container>
    )
}
export default CategoryVersions