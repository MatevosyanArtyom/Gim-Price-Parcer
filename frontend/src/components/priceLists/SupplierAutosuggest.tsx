import React, { useCallback } from 'react'
import Autosuggest, { ChangeEvent, SuggestionsFetchRequestedParams, SuggestionSelectedEventData } from 'react-autosuggest'
import match from 'autosuggest-highlight/match'
import parse from 'autosuggest-highlight/parse'
import { Paper, MenuItem, FormControl, Input, FormHelperText, InputAdornment, CircularProgress, TableCell } from '@material-ui/core'
import * as client from 'client'
import useMergeState from 'util/useMergeState'
import useStyles from './styles'

type Props = {
    onChange: (supplierId: string) => void
}

type State = {
    supplier: string
    suppliers: client.SupplierShort[]
    isLoading: boolean
}

const initialState: State = {
    supplier: '',
    suppliers: [],
    isLoading: false
}

const renderSuggestion = (
    supplier: client.SupplierShort,
    { query, isHighlighted }: Autosuggest.RenderSuggestionParams,
) => {
    const matches = match(supplier.name, query)
    const parts = parse(supplier.name, matches)

    return (
        <MenuItem selected={isHighlighted} component="div">
            <div>
                {parts.map(part => (
                    <span key={part.text} style={{ fontWeight: part.highlight ? 500 : 400 }}>
                        {part.text}
                    </span>
                ))}
            </div>
        </MenuItem>
    )
}

const renderInputComponent = (inputProps: any) => {
    const { classes, inputRef = () => { }, ref, name, label, required, error, margin, isLoading, ...other } = inputProps
    return (
        <FormControl error={Boolean(error)} required={required} fullWidth >
            <Input
                ref={node => {
                    ref(node)
                    inputRef(node)
                }}
                placeholder="Поставщик"
                endAdornment={(<InputAdornment position="end">
                    <CircularProgress size={20} style={{ opacity: isLoading ? 1 : 0 }} />
                </InputAdornment>)}
                {...other}
            />
            {error && <FormHelperText>{error}</FormHelperText>}
        </FormControl>
    )
}

const SupplierAutosuggest: React.FC<Props> = ({ onChange }) => {

    const classes = useStyles()

    let [{ supplier, suppliers, isLoading }, setState] = useMergeState<State>(initialState)

    const loadSuppliers = useCallback((request: SuggestionsFetchRequestedParams) => {
        setState({ isLoading: true })
        client.api.lookupSuppliersGetMany({ Name: request.value, SortBy: 'name', page: 0, pageSize: 7 }, {}).then(v => {
            setState({ suppliers: v.data.entities, isLoading: false })
        })
    }, [setState])

    const clearSuppliers = () => {
        setState({ suppliers: [] })
    }

    const onSuggestionSelected = useCallback((event: any, data: SuggestionSelectedEventData<client.SupplierShort>) => {
        setState({ supplier: data.suggestion.name })
        onChange(data.suggestion.id)
    }, [onChange, setState])

    const getSuggestionValue = useCallback((supplier: client.SupplierShort) => supplier.name, [])

    return (
        <TableCell className={classes.supplierFilterCell}>
            <Autosuggest
                suggestions={suppliers}
                shouldRenderSuggestions={() => true}
                onSuggestionsFetchRequested={loadSuppliers}
                onSuggestionsClearRequested={clearSuppliers}
                onSuggestionSelected={onSuggestionSelected}
                getSuggestionValue={getSuggestionValue}
                renderSuggestionsContainer={options => (
                    <Paper {...options.containerProps} square>
                        {options.children}
                    </Paper>
                )}
                renderSuggestion={renderSuggestion}
                renderInputComponent={renderInputComponent}
                inputProps={{
                    classes: classes,
                    label: 'Поставщик',
                    margin: 'normal',
                    value: supplier,
                    onChange: (event: any, params: ChangeEvent) => {
                        setState({ supplier: params.newValue })
                        if (!params.newValue) {
                            onChange(params.newValue)
                        }
                    },
                    isLoading: isLoading,
                    required: true
                } as any}
                theme={{
                    container: classes.autosuggestContainer,
                    suggestionsContainerOpen: classes.suggestionsContainerOpen,
                    suggestionsList: classes.suggestionsList,
                    suggestion: classes.suggestion,
                }}
            />
        </TableCell>
    )
}

export default SupplierAutosuggest
