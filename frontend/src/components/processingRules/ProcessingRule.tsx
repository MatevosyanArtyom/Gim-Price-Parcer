import React, { useEffect, useCallback, useRef, useContext } from 'react'
import { withRouter } from 'react-router'
import { Field, FormikProps, Formik } from 'formik'
import { TextField } from 'formik-material-ui'
import { MenuItem, Typography, Grid, Button, InputAdornment, FormHelperText, Paper, FormControl, Input, CircularProgress, InputLabel, Toolbar } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import ButtonsBlock from 'components/common/GimForm/ButtonsBlock'
import ButtonCancel from 'components/common/GimForm/ButtonCancel'
import ButtonFromArchive from 'components/common/GimForm/ButtonFromArchive'
import ButtonSubmit from 'components/common/GimForm/ButtonSubmit'
import FormWrapper from 'components/common/GimForm/FormWrapper'
import { RouteComponentPropsWithId } from 'util/types'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import useStyles from './styles'
import Autosuggest, { SuggestionSelectedEventData, SuggestionsFetchRequestedParams, ChangeEvent } from 'react-autosuggest'
import match from 'autosuggest-highlight/match'
import parse from 'autosuggest-highlight/parse'
import CodeSourceToggler from 'components/priceLists/CodeSourceToggler'
import EmitResult from './EmitResult'
import clsx from 'clsx'

type ProcessingRuleFullType = client.ProcessingRuleFull & {
    codeFile: client.GimFileAdd
}

const initialEmitResult: client.EmitResultDto | null = null

const n = nameofFactory<ProcessingRuleFullType>()

type State = {
    processingRule: client.ProcessingRuleFull
    supplier: string
    suppliers: client.SupplierShort[]
    emitResult: client.EmitResultDto | null
    isSuppliersLoading: boolean
    isLoading: boolean
}

const initialProcessingRule: ProcessingRuleFullType = {
    id: '',
    seqId: '' as any,
    name: '',
    supplier: '',
    rulesSource: 'Code',
    code: '',
    codeFile: {
        data: '',
        name: '',
        size: 0
    }
}

const initialSuppliers: client.SupplierShort[] = []

const initialState: State = {
    processingRule: initialProcessingRule,
    supplier: '',
    suppliers: initialSuppliers,
    emitResult: initialEmitResult,
    isSuppliersLoading: false,
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

const FormContent = (props: RouteComponentPropsWithId & FormikProps<ProcessingRuleFullType>) => {

    const codeFileInputRef = useRef<HTMLInputElement>(null)

    const classes = useStyles()

    let [{ suppliers, supplier, emitResult, isSuppliersLoading, isLoading }, setState] = useMergeState<State>(initialState)

    const context = useContext(AppContext)

    const readOnly = props.values.isArchived || !context.user.accessRights.processingRules.full

    const loadSuppliers = (request: SuggestionsFetchRequestedParams) => {
        setState({ isSuppliersLoading: true })
        client.api.lookupSuppliersGetMany({
            Name: request.value,
            SortBy: 'name',
            IsArchived: false,
            page: 0,
            pageSize: 7
        }).then(v => {
            setState({ suppliers: v.data.entities, isSuppliersLoading: false })
        })
    }

    const clearSuppliers = () => {
        setState({ suppliers: [] })
    }

    const onCodeFileClick = () => {
        !readOnly && codeFileInputRef.current && codeFileInputRef.current.click()
    }

    const onCodeFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files) {
            let fileReader = new FileReader()

            let file = event.target.files[0]

            fileReader.onload = () => {
                let gimFile: client.GimFileAdd = {
                    data: fileReader.result as string,
                    size: file.size,
                    name: file.name
                }
                props.setFieldValue(n('codeFile'), gimFile)
                props.setFieldValue(n('code'), gimFile.data)
            }
            fileReader.readAsText(file)
        }
    }

    const onSuggestionSelected = (event: any, data: SuggestionSelectedEventData<client.SupplierShort>) => {
        props.setFieldValue('supplier', data.suggestion.id)
    }
    const getSuggestionValue = (supplier: client.SupplierShort) => supplier.name

    const onCheckEmit = () => {
        setState({ isLoading: true })

        client.api.processingRuleCheckEmit({ rulesSource: props.values.rulesSource, script: props.values.code }).then(v => {
            setState({
                emitResult: v.data,
                isLoading: false
            })
        })
    }

    return (
        <FormWrapper
            isLoading={isLoading}
        >
            <Grid container>
                <Grid item md={6} xs={12} className={classes.gridItem}>
                    <Typography variant="h5">Характеристики</Typography>
                    <Field name={n('seqId')} label="ID" component={TextField} margin="normal" fullWidth disabled />
                    <Field
                        name={n('name')}
                        label="Наименование"
                        component={TextField}
                        margin="normal"
                        error={props.errors.name}
                        validate={validations.required}
                        InputProps={{ readOnly: readOnly }}
                        fullWidth
                        required
                    />
                    <Autosuggest
                        suggestions={suppliers}
                        shouldRenderSuggestions={() => !readOnly}
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
                                if (props.values.supplier) {
                                    props.setFieldValue('supplier', '')
                                }
                                setState({ supplier: params.newValue })
                            },
                            isLoading: isSuppliersLoading,
                            required: true,
                            error: props.errors.supplier,
                            readOnly: readOnly
                        } as any}
                        theme={{
                            container: classes.autosuggestContainer,
                            suggestionsContainerOpen: classes.suggestionsContainerOpen,
                            suggestionsList: classes.suggestionsList,
                            suggestion: classes.suggestion,
                        }}
                    />
                    <Toolbar disableGutters>
                        <Typography variant="h5">Правила обработки</Typography>
                    </Toolbar>
                    <Field
                        name={`${n('rulesSource')}`}
                        component={CodeSourceToggler}
                    />
                    {props.values.rulesSource === 'File' &&
                        <div className={classes.fileInputDiv}>
                            <Button
                                type="button"
                                variant="contained"
                                className={classes.fileInputButton}
                                onClick={onCodeFileClick}
                                disabled={isLoading || readOnly}
                            >
                                Выбрать
                                </Button>
                            <Field
                                name={`${n('codeFile')}.name`}
                                label="Файл скрипта"
                                component={TextField}
                                className={classes.fileInputField}
                                onClick={onCodeFileClick}
                                margin="normal"
                                inputProps={{
                                    readOnly: true,
                                }}
                                error={props.errors.codeFile}
                            />
                        </div>
                    }
                    {props.values.rulesSource === 'Code' &&
                        <Grid item xs={12} className={classes.gridItem}>
                            <Field
                                name={n('code')}
                                label="Код"
                                component={TextField}
                                margin="normal"
                                validate={props.values.rulesSource === 'Code' && validations.required}
                                required={props.values.rulesSource === 'Code'}
                                error={props.errors.code}
                                rows="10"
                                InputProps={{ readOnly: readOnly }}
                                fullWidth
                                multiline
                            />
                        </Grid>
                    }
                    <input
                        ref={codeFileInputRef}
                        accept=".cs"
                        className={classes.input}
                        type="file"
                        onChange={onCodeFileChange}
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
            <ButtonsBlock align="right">
                <Button color="primary" onClick={onCheckEmit} className={clsx(classes.button)}>Проверить компиляцию</Button>
                <ButtonCancel disabled={props.isSubmitting} />
                {!props.values.isArchived && <ButtonSubmit disabled={props.isSubmitting || readOnly} />}
                {
                    props.values.isArchived &&
                    <ButtonFromArchive
                        disabled={props.isSubmitting || readOnly}
                        onClick={() => {
                            // client.ProcessingRuleFromArchiveOne({ id: props.values.id }, {}).then(v => loadProcessingRule(id))
                        }}
                    />
                }
            </ButtonsBlock>
        </FormWrapper>
    )
}

const ProcessingRule: React.FC<RouteComponentPropsWithId> = (props) => {

    const id = props.match.params.id === 'add' ? '' : props.match.params.id

    let [{ processingRule }, setState] = useMergeState<State>(initialState)

    const loadProcessingRule = useCallback((id: string) => {
        setState({ isLoading: true })
        if (id) {
            client.api.processingRuleGetOne(id).then(v => {
                setState({ processingRule: v.data, isLoading: false })
            })
        } else {
            setState({ processingRule: initialProcessingRule, isLoading: false })
        }
    }, [setState])

    useEffect(() => {
        loadProcessingRule(id)
    }, [id, loadProcessingRule])

    return (
        <Formik
            initialValues={processingRule as ProcessingRuleFullType}
            onSubmit={values => {
                if (id) {
                    client.api.processingRuleUpdateOne(values).then(() => props.history.goBack())
                } else {
                    client.api.processingRuleAddOne(values).then(() => props.history.goBack())
                }
            }}
            enableReinitialize={true}
        >
            {renderProps => <FormContent {...props} {...renderProps} />}
        </Formik>
    )
}

export default withRouter(ProcessingRule)