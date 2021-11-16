import React, { useCallback, useRef, useEffect } from 'react'
import _ from 'lodash'
import { Box } from '@material-ui/core'
import PriceListItemsHeader from './priceListItems/PriceListItemsHeader'
import PriceListItemsBody from './priceListItems/PriceListItemsBody'
import { Filter, initialFilter } from './priceListItems/types'
import useMergeState from 'util/useMergeState'

type Props = {
    id: string
    disabled: boolean
    clearFilter: {}
    forceUpdate: {}
    setForceUpdate: () => void
}

type State = {
    filter: Filter
}

const initialState: State = {
    filter: initialFilter
}

const PriceListItems: React.FC<Props> = ({ id, disabled, clearFilter, forceUpdate, setForceUpdate }) => {

    const [{ filter }, setState] = useMergeState(initialState)

    const debounced = useRef(_.debounce((filter: Filter) => {
        setState({ filter: filter })
    }, 300))

    const onFilterChange = useCallback((filter: Filter) => {
        debounced.current(filter)
    }, [])

    useEffect(() => {
        setState({ filter: filter })
    }, [filter, setState])

    return (
        <Box>
            <PriceListItemsHeader onChange={onFilterChange} clearFilter={clearFilter} />
            <PriceListItemsBody priceListId={id} disabled={disabled} filter={filter} forceUpdate={forceUpdate} setForceUpdate={setForceUpdate} />
        </Box>
    )
}

export default PriceListItems