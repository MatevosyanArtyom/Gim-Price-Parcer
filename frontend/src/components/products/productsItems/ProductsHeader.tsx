import React, { useCallback, useEffect, ChangeEvent } from 'react'
import update from 'immutability-helper'
import { Table, TableHead, TableRow, TextField, MenuItem, TableBody } from '@material-ui/core'
import * as client from 'client'
import { EntityStatuses } from 'components/common/types'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import ProductsTableCell from './ProductsTableCell'
import { widths, Filter, initialFilter } from './types'

const n = nameofFactory<Filter>()

type Props = {
    onChange: (filter: Filter) => void
}

type State = {
    categories1: client.CategoryLookup[]
    categories2: client.CategoryLookup[]
    categories3: client.CategoryLookup[]
    categories4: client.CategoryLookup[]
    categories5: client.CategoryLookup[]
    filter: Filter
}

const initialState: State = {
    categories1: [],
    categories2: [],
    categories3: [],
    categories4: [],
    categories5: [],
    filter: initialFilter
}

const ProductsHeader: React.FC<Props> = ({ onChange }) => {

    const [{ categories1, categories2, categories3, categories4, categories5, filter }, setState] = useMergeState(initialState)

    const onFieldChange = useCallback((field: string, value: string, filter: Filter) => {
        const newFilter = update(filter, { [field]: { $set: value } })
        setState({ filter: newFilter })
    }, [setState])

    const onCategory1Change = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        const newFilter = update(filter, {
            category1: { $set: e.target.value },
            category2: { $set: '' },
            category3: { $set: '' },
            category4: { $set: '' },
            category5: { $set: '' }
        })
        setState({
            filter: newFilter,
            categories2: [],
            categories3: [],
            categories4: [],
            categories5: []
        })
    }, [filter, setState])

    const onCategory2Change = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        let newFilter = update(filter, {
            category2: { $set: e.target.value },
            category3: { $set: '' },
            category4: { $set: '' },
            category5: { $set: '' }
        })
        setState({
            filter: newFilter,
            categories3: [],
            categories4: [],
            categories5: []
        })
    }, [filter, setState])

    const onCategory3Change = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        let newFilter = update(filter, {
            category3: { $set: e.target.value },
            category4: { $set: '' },
            category5: { $set: '' }
        })
        setState({
            filter: newFilter,
            categories4: [],
            categories5: []
        })
    }, [filter, setState])

    const onCategory4Change = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        let newFilter = update(filter, {
            category4: { $set: e.target.value },
            category5: { $set: '' }
        })
        setState({
            filter: newFilter,
            categories5: []
        })
    }, [filter, setState])

    const onCategory5Change = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        let newFilter = update(filter, {
            category5: { $set: e.target.value }
        })
        setState({
            filter: newFilter
        })
    }, [filter, setState])

    useEffect(() => {
        onChange(filter)
    }, [filter, onChange])

    const loadCategories = useCallback((level: number, categoryId: string) => {
        client.api.lookupCategoriesGetChildrenFlatten({ 'parents[]': [categoryId] }, {}).then(v => {
            setState({ [`categories${level}`]: v.data })
        })
    }, [setState])

    useEffect(() => {
        if (filter.category1) {
            loadCategories(2, filter.category1)
        }
    }, [filter.category1, loadCategories])

    useEffect(() => {
        if (filter.category2) {
            loadCategories(3, filter.category2)
        }
    }, [filter.category2, loadCategories])

    useEffect(() => {
        if (filter.category3) {
            loadCategories(4, filter.category3)
        }
    }, [filter.category3, loadCategories])

    useEffect(() => {
        if (filter.category4) {
            loadCategories(5, filter.category4)
        }
    }, [filter.category4, loadCategories])

    useEffect(() => {
        client.api.lookupCategoriesGetChildrenFlatten({ includeRoot: true }, {}).then(v => {
            setState({ categories1: v.data })
        })
    }, [setState])

    return (
        <Table component="div">
            <TableHead component="div">
                <TableRow component="div" style={{ display: 'flex', paddingRight: 17 }}>
                    <ProductsTableCell width={widths.id}>ID</ProductsTableCell>
                    <ProductsTableCell width={widths.category}>Категория 1</ProductsTableCell>
                    <ProductsTableCell width={widths.category}>Категория 2</ProductsTableCell>
                    <ProductsTableCell width={widths.category}>Категория 3</ProductsTableCell>
                    <ProductsTableCell width={widths.category}>Категория 4</ProductsTableCell>
                    <ProductsTableCell width={widths.category}>Категория 5</ProductsTableCell>
                    <ProductsTableCell width={widths.name}>Наименование</ProductsTableCell>
                    <ProductsTableCell width={widths.priceFrom} align="right">Цена, от</ProductsTableCell>
                    <ProductsTableCell width={widths.suppliersCount} align="right">Поставщиков</ProductsTableCell>
                    <ProductsTableCell width={widths.imagesCount} align="right">Фото</ProductsTableCell>
                    <ProductsTableCell width={widths.status}>Статус</ProductsTableCell>
                    <ProductsTableCell flexGrow={0} flexShrink={0} width={widths.actions}>Действия</ProductsTableCell>
                </TableRow>
            </TableHead>
            <TableBody component="div">
                <TableRow component="div" style={{ display: 'flex', paddingRight: 17 }}>
                    <ProductsTableCell width={widths.id}>
                        <TextField
                            value={filter.seqId}
                            onChange={e => onFieldChange(n('seqId'), e.target.value, filter)}
                            placeholder="ID"
                            fullWidth
                        />
                    </ProductsTableCell>
                    <ProductsTableCell width={widths.category}>
                        <TextField
                            value={filter.category1}
                            onChange={onCategory1Change}
                            select
                            SelectProps={{ autoWidth: true, displayEmpty: true }}
                            style={{ maxWidth: widths.category - 8 }}
                            fullWidth
                        >
                            <MenuItem key="all" value="">Все</MenuItem>
                            {categories1.map((v: client.CategoryLookup) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                        </TextField>
                    </ProductsTableCell>
                    <ProductsTableCell width={widths.category}>
                        <TextField
                            value={filter.category2}
                            onChange={onCategory2Change}
                            SelectProps={{ displayEmpty: true }}
                            style={{ maxWidth: widths.category - 8 }}
                            select
                            fullWidth
                        >
                            <MenuItem key="all" value="">Все</MenuItem>
                            {categories2.map((v: client.CategoryLookup) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                        </TextField>
                    </ProductsTableCell>
                    <ProductsTableCell width={widths.category}>
                        <TextField
                            value={filter.category3}
                            onChange={onCategory3Change}
                            SelectProps={{ displayEmpty: true }}
                            style={{ maxWidth: widths.category - 8 }}
                            select
                            fullWidth
                        >
                            <MenuItem key="all" value="">Все</MenuItem>
                            {categories3.map((v: client.CategoryLookup) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                        </TextField>
                    </ProductsTableCell>
                    <ProductsTableCell width={widths.category}>
                        <TextField
                            value={filter.category4}
                            onChange={onCategory4Change}
                            SelectProps={{ displayEmpty: true }}
                            style={{ maxWidth: widths.category - 8 }}
                            select
                            fullWidth
                        >
                            <MenuItem key="all" value="">Все</MenuItem>
                            {categories4.map((v: client.CategoryLookup) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                        </TextField>
                    </ProductsTableCell>
                    <ProductsTableCell width={widths.category}>
                        <TextField
                            value={filter.category5}
                            onChange={onCategory5Change}
                            SelectProps={{ displayEmpty: true }}
                            style={{ maxWidth: widths.category - 8 }}
                            select
                            fullWidth
                        >
                            <MenuItem key="all" value="">Все</MenuItem>
                            {categories5.map((v: client.CategoryLookup) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                        </TextField>
                    </ProductsTableCell>
                    <ProductsTableCell width={widths.name}>
                        <TextField
                            value={filter.name}
                            onChange={e => onFieldChange(n('name'), e.target.value, filter)}
                            placeholder="Наименование"
                            fullWidth
                        />
                    </ProductsTableCell>
                    <ProductsTableCell width={widths.priceFrom}></ProductsTableCell>
                    <ProductsTableCell width={widths.suppliersCount}></ProductsTableCell>
                    <ProductsTableCell width={widths.imagesCount}></ProductsTableCell>
                    <ProductsTableCell width={widths.status}>
                        <TextField
                            value={filter.status}
                            onChange={e => onFieldChange(n('status'), e.target.value, filter)}
                            SelectProps={{ displayEmpty: true }}
                            select
                            fullWidth
                        >
                            <MenuItem key="all" value="">Все</MenuItem>
                            {Object.keys(EntityStatuses).map(key => <MenuItem key={key} value={key}>{EntityStatuses[key]}</MenuItem>)}
                        </TextField>
                    </ProductsTableCell>
                    <ProductsTableCell flexGrow={0} flexShrink={0} width={widths.actions} />
                </TableRow>
            </TableBody>
        </Table >
    )
}

export default ProductsHeader