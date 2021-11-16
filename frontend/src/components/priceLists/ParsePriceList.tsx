import React, { useRef, useCallback, useEffect } from 'react'
import Autosuggest, { SuggestionsFetchRequestedParams, ChangeEvent, SuggestionSelectedEventData } from 'react-autosuggest'
import { RouteComponentProps } from 'react-router'
import match from 'autosuggest-highlight/match'
import parse from 'autosuggest-highlight/parse'
import clsx from 'clsx'
import { Formik, Field, FormikProps, useFormikContext } from 'formik'
import { TextField } from 'formik-material-ui'
import _ from 'lodash'
import qs from 'query-string'
import { Paper, Toolbar, Typography, Grid, Button, MenuItem, InputAdornment, CircularProgress, FormControl, InputLabel, Input, FormHelperText, Box } from '@material-ui/core'
import * as client from 'client'
import FormWrapper from 'components/common/GimForm/FormWrapper'
import EmitResult from 'components/processingRules/EmitResult'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import useStyles from './styles'
import { ParsePriceListState, initialPriceList } from './types'

const n = nameofFactory<client.PriceListAdd>()

const initialEmitResult: client.EmitResultDto | null = null

const initialState: ParsePriceListState = {
    supplier: '',
    suppliers: [],
    isSuppliersLoading: false,
    processingRules: [],
    emitResult: initialEmitResult,
    isLoading: false
}

function renderInputComponent(inputProps: any) {
    const { classes, inputRef = () => { }, ref, name, label, required, error, margin, isLoading, ...other } = inputProps
    return (
        <FormControl error={Boolean(error)} required={required} margin={margin} fullWidth >
            <InputLabel htmlFor={name}>{label}</InputLabel>
            <Input
                ref={node => {
                    ref(node)
                    inputRef(node)
                }}
                {...(isLoading && { endAdornment: <InputAdornment position="end"><CircularProgress size={20} /></InputAdornment> })}
                {...other}
            />
            {error && <FormHelperText>{error}</FormHelperText>}
        </FormControl>
    )
}

function renderSuggestion(
    supplier: client.SupplierShort,
    { query, isHighlighted }: Autosuggest.RenderSuggestionParams,
) {
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

const FormContent = (props: RouteComponentProps & FormikProps<client.PriceListAdd>) => {

    const { errors, values, setFieldValue } = useFormikContext<client.PriceListAdd>()

    let [{ supplier, suppliers, isSuppliersLoading, processingRules, emitResult, isLoading }, setState] = useMergeState<ParsePriceListState>(initialState)

    const onSuggestionSelected = (event: any, data: SuggestionSelectedEventData<client.SupplierShort>) => {
        setFieldValue('supplier', data.suggestion.id)
        loadProcessingRules(data.suggestion.id)
    }
    const getSuggestionValue = (supplier: client.SupplierShort) => supplier.name

    const loadProcessingRules = useCallback((supplierId: string) => {
        if (!supplierId) {
            setState({ processingRules: [] })
        } else {
            client.api.lookupProcessingRulesGetMany({ SupplierId: supplierId }, {}).then(v => {
                setState({ processingRules: v.data.entities })
            })
        }
    }, [setState])

    const priceListInputRef = useRef<HTMLInputElement>(null)

    const classes = useStyles()


    const onPriceListChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files) {
            let fileReader = new FileReader()

            let file = event.target.files[0]

            fileReader.onload = () => {
                let gimFile: client.GimFileAdd = {
                    data: fileReader.result as string,
                    size: file.size,
                    name: file.name
                }
                setFieldValue(n('priceListFile'), gimFile)
            }
            fileReader.readAsDataURL(file)
        }
    }

    const onPriceListClick = () => {
        priceListInputRef.current && priceListInputRef.current.click()
    }

    const loadSuppliers = (request: SuggestionsFetchRequestedParams) => {
        setState({ isSuppliersLoading: true })
        client.api.lookupSuppliersGetMany({ Name: request.value, SortBy: 'name', IsArchived: false, page: 0, pageSize: 7 }, {}).then(v => {
            setState({ suppliers: v.data.entities, isSuppliersLoading: false })
        })
    }

    const clearSuppliers = () => {
        setState({ suppliers: [] })
    }

    const onCheckEmit = () => {
        setState({ isLoading: true })

        client.api.priceListsCheckEmit(props.values.processingRule).then(v => {
            setState({
                emitResult: v.data,
                isLoading: false
            })
        })
    }

    // const onCheckParse = () => {
    //     setState({
    //         isLoading: true
    //     })
    //     if (values.processingRule === 'Code') {
    //         values.priceListFile = undefined
    //     }

    //     client.api.priceListsParseOne(values)
    //         .then(v => {
    //             setState({ isLoading: false })
    //         })
    //         .catch(v => {
    //             setState({ isLoading: false })
    //         })
    // }

    const validateXslx = useCallback((value: string) => {

        const required = validations.required(value)
        if (required) {
            return required
        }

        if (!value.toLowerCase().endsWith('.xlsx')) {
            return 'Допустимы только файлы .xlsx'
        }
        return undefined
    }, [])

    useEffect(() => {
        const parsed = qs.parse(props.location.search)
        if (_.isEmpty(parsed)) { return }

        const supplierId = parsed.supplierId as string
        const processingRuleId = parsed.processingRuleId as string

        setFieldValue('supplier', supplierId)
        setFieldValue('processingRule', processingRuleId)

        setState({ isLoading: true })
        const promises: [
            Promise<client.HttpResponse<client.SupplierShort>>,
            Promise<client.HttpResponse<client.GetAllResultDtoOfProcessingRuleLookup>>,
        ] = [
                client.api.lookupSuppliersGetOne(supplierId),
                client.api.lookupProcessingRulesGetMany({ SupplierId: supplierId })
            ]

        Promise.all(promises).then(results => {
            setState({
                supplier: results[0].data.name,
                processingRules: results[1].data.entities,
                isLoading: false
            })
        })

    }, [props.location.search, setFieldValue, setState])

    return (
        <Box className={classes.root}>
            <Grid container spacing={2}>
                <Grid item md={6}>
                    <FormWrapper
                        isLoading={isLoading}
                    >
                        <Grid item xs={10}>
                            <Toolbar disableGutters>
                                <Typography variant="h5">Прайсы — загрузка прайса</Typography>
                            </Toolbar>
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
                                        if (values.supplier) {
                                            setFieldValue('supplier', '')
                                        }
                                        setFieldValue('processingRule', '')
                                        loadProcessingRules('')
                                        setState({ supplier: params.newValue })
                                    },
                                    isLoading: isSuppliersLoading,
                                    required: true,
                                    error: errors.supplier
                                } as any}
                                theme={{
                                    container: classes.autosuggestContainer,
                                    suggestionsContainerOpen: classes.suggestionsContainerOpen,
                                    suggestionsList: classes.suggestionsList,
                                    suggestion: classes.suggestion,
                                }}
                            />
                            <Field
                                name={n('processingRule')}
                                label="Правило обработки"
                                component={TextField}
                                margin="normal"
                                validate={validations.required}
                                error={errors.processingRule}
                                fullWidth
                                required
                                select
                            >
                                {processingRules.map((v: client.ProcessingRuleLookup) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                            </Field>
                            <div className={classes.fileInputDiv}>
                                <Button type="button" variant="contained" className={classes.fileInputButton} onClick={onPriceListClick} disabled={isLoading}>Выбрать</Button>
                                <Field
                                    name={`${n('priceListFile')}.name`}
                                    label="Файл прайс-листа"
                                    component={TextField}
                                    className={classes.fileInputField}
                                    onClick={onPriceListClick}
                                    margin="normal"
                                    inputProps={{
                                        readOnly: true,
                                    }}
                                    FormHelperTextProps={{ style: { display: 'none' } }}
                                    validate={validateXslx}
                                    error={errors.priceListFile}
                                    required
                                />
                            </div>
                        </Grid>
                        <Grid item xs={12}>
                            <Toolbar disableGutters>
                                <Button type="submit" variant="contained" color="primary">Произвести обработку</Button>
                                <Button color="primary" onClick={onCheckEmit} disabled={!props.values.processingRule}>Проверить компиляцию</Button>
                                {/* <Button color="primary" onClick={onCheckParse}>Проверить парсинг</Button> */}
                            </Toolbar>
                        </Grid>
                    </FormWrapper>

                    <input
                        ref={priceListInputRef}
                        accept=".xlsx"
                        className={classes.input}
                        type="file"
                        onChange={onPriceListChange}
                    />
                </Grid>
                <Grid item md={6}>
                    {emitResult && (
                        <Paper className={clsx(classes.paper, classes.noMarginLeft)}>
                            <EmitResult result={emitResult} margin={2} />
                        </Paper>
                    )}
                </Grid>
            </Grid>
        </Box >
    )
}

const ParsePriceList: React.FC<RouteComponentProps> = (props) => {
    return (
        <Formik
            initialValues={initialPriceList}
            onSubmit={(values, actions) => {
                if (values.processingRule === 'Code') {
                    values.priceListFile = undefined
                }
                // setState({ isLoading: true })
                client.api.priceListsAddOne(values).then(() => {
                    props.history.push('/parsedPriceLists')
                })
            }}
        >
            {renderProps => <FormContent {...props}{...renderProps} />}
        </Formik>
    )
}

export default ParsePriceList