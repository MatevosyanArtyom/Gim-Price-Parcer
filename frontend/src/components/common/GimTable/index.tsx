import React from 'react'
import browserHistory from 'browserHistory'
import _ from 'lodash'
import MaterialTable, { MaterialTableProps, Options, Components } from 'material-table'
import { Paper } from '@material-ui/core'
import { MaterialTableLocalization } from './utils'

export type RowData = {
    id?: string
}

interface GimMaterialTableProps<T extends Record<string, unknown>> extends Omit<MaterialTableProps<T>, 'onRowClick'> {
    onRowClick?: ('push' | ((_e?: React.MouseEvent, rowData?: RowData) => void))
}

type Props<T extends Record<string, unknown>> = GimMaterialTableProps<T> & {
    disabled?: boolean
    onRowAddClick?: () => void
    addButton?: boolean // показывать ли кнопку добавления по-умолчанию
}

const GimTable = React.forwardRef<any, Props<any>>((props, ref) => {

    let actions = props.actions || []
    let icons = {}
    let tableProps = {} as any
    if (props.editable) {
        // возможно, это уже не используется
        if (props.editable.onRowAdd) {
            // Для таблицы установлено inline-добавление
            icons = { Add: 'add' } // По-умолчанию там add_box

        } else if (props.addButton) {
            // В противном случае, добавление на отдельной странице или переданный метод добавления
            // Кнопка добавления по-умолчанию

        }

        !props.editable.onRowAdd && actions.push(
            {
                icon: 'add',
                tooltip: 'Добавить',
                isFreeAction: true,
                onClick: props.onRowAddClick ? () => props.onRowAddClick!() : () => browserHistory.push(`${browserHistory.location.pathname}/add`),
                disabled: props.disabled
            }
        )

        const onRowClick = (_e?: React.MouseEvent, rowData?: RowData) => {
            if (rowData && rowData.id) {
                browserHistory.push(`${browserHistory.location.pathname}/${rowData.id}`)
            }
        }

        if (props.onRowClick) {
            if (props.onRowClick === 'push') {
                tableProps.onRowClick = onRowClick
            }
        }

        // tableProps = {
        //     onRowClick
        // }
    }

    const options: Options<any> = {
        showTitle: false,
        search: false,
        toolbar: false, // TODO: убрать, после переделывания всех страниц, когда не будет кнопки добавить здесь
        pageSize: 7,
        pageSizeOptions: [7]
    }

    if (props.options) {
        _.assign(options, props.options)
        props = _.omit(props, 'options')
    }

    const components: Components = {
        Container: props => <Paper {...props} elevation={0} />,
    }

    if (props.components) {
        _.assign(components, props.components)
        props = _.omit(props, 'components')
    }

    const style: React.CSSProperties = {
        // padding: '0px 15px'
    }

    return (
        <MaterialTable
            tableRef={ref}
            localization={MaterialTableLocalization}
            options={options}
            icons={icons}
            style={style}
            {...props}
            {...tableProps}
            actions={actions}
            components={components}
        />
    )
})

export default GimTable