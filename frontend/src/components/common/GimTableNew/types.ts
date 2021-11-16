import { Index, IndexRange, TableCellProps, RowMouseEventHandlerParams, SortDirectionType, ColumnProps } from 'react-virtualized'

export type GimTableColumn = ColumnProps & {
    align?: 'inherit' | 'left' | 'center' | 'right' | 'justify'
    filter?: React.ComponentType<any>
}

export type SortParams = {
    sortBy?: string
    sortDirection?: SortDirectionType
}

export type GimTableProps = {
    columns: GimTableColumn[]
    isRowLoaded: (params: Index) => boolean
    loadMoreRows: (params: IndexRange) => Promise<any>
    rowGetter: (info: Index) => any
    rowCount: number
    onDelete?: (props: TableCellProps, event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void
    onRowClick: (info: RowMouseEventHandlerParams) => void
    sort: (params: SortParams) => void
    sortBy?: string
    sortDirection?: SortDirectionType
}