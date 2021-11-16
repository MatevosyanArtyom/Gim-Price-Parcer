import React, { useCallback, useEffect } from 'react'
import update from 'immutability-helper'
import { Table, TableHead, TableRow, TextField, MenuItem, TableBody } from '@material-ui/core'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import ItemsCell from './ItemsCell'
import { widths, Filter, initialFilter, PriceListItemStatuses } from './types'

const n = nameofFactory<Filter>()

type Props = {
    onChange: (filter: Filter) => void
    clearFilter: {}
}

type State = {
    filter: Filter
}

const initialState: State = {
    filter: initialFilter
}

const PriceListItemsHeader: React.FC<Props> = ({ onChange, clearFilter }) => {

    const [{ filter }, setState] = useMergeState(initialState)

    const onFieldChange = useCallback((field: string, value: string, filter: Filter) => {
        const newFilter = update(filter, { [field]: { $set: value } })
        setState({ filter: newFilter })
    }, [setState])

    useEffect(() => {
        if (clearFilter) {
            setState({ filter: initialFilter })
        }
    }, [clearFilter, setState])

    useEffect(() => {
        onChange(filter)
    }, [filter, onChange])

    return (
        <Table component="div">
            <TableHead component="div">
                <TableRow component="div" style={{ display: 'flex', paddingRight: 17 }}>
                    <ItemsCell width={widths.code}>Артикул</ItemsCell>
                    <ItemsCell width={widths.category}>Категория 1</ItemsCell>
                    <ItemsCell width={widths.category}>Категория 2</ItemsCell>
                    <ItemsCell width={widths.category}>Категория 3</ItemsCell>
                    <ItemsCell width={widths.category}>Категория 4</ItemsCell>
                    <ItemsCell width={widths.category}>Категория 5</ItemsCell>
                    <ItemsCell width={widths.name}>Наименование</ItemsCell>
                    <ItemsCell width={widths.price} align="right">Цена 1</ItemsCell>
                    <ItemsCell width={widths.price} align="right">Цена 2</ItemsCell>
                    <ItemsCell width={widths.price} align="right">Цена 3</ItemsCell>
                    <ItemsCell width={widths.status}>Статус</ItemsCell>
                    <ItemsCell flexGrow={0} flexShrink={0} width={widths.actions}>Действия</ItemsCell>
                </TableRow>
            </TableHead>
            <TableBody component="div">
                <TableRow component="div" style={{ display: 'flex', paddingRight: 17 }}>
                    <ItemsCell width={widths.code}>
                        <TextField
                            value={filter.code}
                            onChange={e => onFieldChange(n('code'), e.target.value, filter)}
                            placeholder="Артикул"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.category}>
                        <TextField
                            value={filter.category1Name}
                            onChange={e => onFieldChange(n('category1Name'), e.target.value, filter)}
                            placeholder="Категория 1"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.category}>
                        <TextField
                            value={filter.category2Name}
                            onChange={e => onFieldChange(n('category2Name'), e.target.value, filter)}
                            placeholder="Категория 2"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.category}>
                        <TextField
                            value={filter.category3Name}
                            onChange={e => onFieldChange(n('category3Name'), e.target.value, filter)}
                            placeholder="Категория 3"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.category}>
                        <TextField
                            value={filter.category4Name}
                            onChange={e => onFieldChange(n('category4Name'), e.target.value, filter)}
                            placeholder="Категория 4"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.category}>
                        <TextField
                            value={filter.category5Name}
                            onChange={e => onFieldChange(n('category5Name'), e.target.value, filter)}
                            placeholder="Категория 5"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.name}>
                        <TextField
                            value={filter.productNameRegEx}
                            onChange={e => onFieldChange(n('productNameRegEx'), e.target.value, filter)}
                            placeholder="Наименование"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.price}>
                        <TextField
                            value={filter.price1}
                            onChange={e => onFieldChange(n('price1'), e.target.value, filter)}
                            placeholder="Цена 1"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.price}>
                        <TextField
                            value={filter.price2}
                            onChange={e => onFieldChange(n('price2'), e.target.value, filter)}
                            placeholder="Цена 2"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.price}>
                        <TextField
                            value={filter.price3}
                            onChange={e => onFieldChange(n('price3'), e.target.value, filter)}
                            placeholder="Цена 3"
                            fullWidth
                        />
                    </ItemsCell>
                    <ItemsCell width={widths.status}>
                        <TextField
                            value={filter.status}
                            onChange={e => onFieldChange(n('status'), e.target.value, filter)}
                            SelectProps={{ displayEmpty: true }}
                            select
                            fullWidth
                        >
                            <MenuItem key="all" value="">Все</MenuItem>
                            {Object.keys(PriceListItemStatuses).map(key => <MenuItem key={key} value={key}>{PriceListItemStatuses[key]}</MenuItem>)}
                        </TextField>
                    </ItemsCell>
                    <ItemsCell flexGrow={0} flexShrink={0} width={widths.actions} />
                </TableRow>
            </TableBody>
        </Table >
    )
}

export default PriceListItemsHeader