import React, { useEffect, useCallback, useContext } from 'react'
import { useHistory } from 'react-router'
import clsx from 'clsx'
import moment from 'moment'
import qs from 'query-string'
import { Paper, Typography, Grid, Box, TextField, Toolbar, Button, DialogContentText } from '@material-ui/core'
import { Clear as ClearIcon } from '@material-ui/icons'
import * as client from 'client'
import AppContext from 'context'
import GimCircularProgress from 'components/common/GimCircularProgress'
import { GimDialogContext } from 'components/common/GimDialog'
import { RouteComponentPropsWithId } from 'util/types'
import useMergeState from 'util/useMergeState'
import PriceListItems from './PriceListItems'
import PriceListPageToggler from './PriceListPageToggler'
import PriceListProperties from './PriceListProperties'
import useStyles from './styles'
import { PriceListStatuses, PriceListState } from './types'

const initialPriceList: client.PriceListFull = {
    id: '',
    seqId: 0,
    priceListFile: {
        id: '',
        name: '',
        data: '',
        size: 0
    },
    supplier: '',
    supplierId: '',
    processingRule: '',
    processingRuleId: '',
    status: 'Unknown',
    createdDate: '',
    author: '',
    authorId: ''
}

const initialState: PriceListState = {
    priceList: initialPriceList,
    page: 'items',
    clearFilter: {},
    forceUpdate: {},
    isLoading: false
}

const PriceList: React.FC<RouteComponentPropsWithId> = (props) => {

    const id = props.match.params.id

    let history = useHistory()
    const classes = useStyles()

    const [{ priceList, page, commitError, clearFilter, forceUpdate, isLoading }, setState] = useMergeState(initialState)
    const context = useContext(AppContext)
    const gimDialogState = useContext(GimDialogContext)

    // чужой прайс-лист, или уже примененный
    const disabled = (context.user.id !== priceList.authorId && !context.user.accessRights.priceLists.full) || priceList.status === 'Committed' || isLoading
    // есть ошибки в характеристиках и нет прав на их игнорирование
    const commitDisabled = disabled ||
        // утвердить прайс можно только в статусе 'готов' или 'ошибки' 
        (priceList.status !== 'Ready' && priceList.status !== 'Errors') ||
        // но только если нет ошибок в элементах прайс-листа
        (priceList.status === 'Errors' && priceList.hasUnprocessedErrors)
    // если есть ошибки в характеристиках, должны быть права на их игнорирование
    // (priceList.hasPropertiesErrors && !context.user.accessRights.priceLists.commitWithErrors)

    const propertiesActionsDisabled = disabled || !context.user.accessRights.priceLists.createProperties

    const loadPriceList = useCallback((id: string) => {
        setState({ isLoading: true })
        client.api.priceListsGetOne(id).then(v => {
            setState({ priceList: v.data, isLoading: false })
        })
    }, [setState])

    const onCommit = useCallback(() => {
        gimDialogState.setState!({
            open: true,
            title: 'Утверждение загрузки',
            content: (
                <React.Fragment>
                    <DialogContentText>Утвержденная загрузка будет применена к БД</DialogContentText>
                </React.Fragment>
            ),
            actions: (
                <React.Fragment>
                    <Button color="primary" onClick={() => gimDialogState.setState!({ open: false })}>Отмена</Button>
                    <Button
                        color="primary"
                        variant="contained"
                        onClick={() => {
                            setState({ isLoading: true, commitError: false })
                            client.api.priceListsCommitOne(id)
                                .then(v => {
                                    setState({ isLoading: false, forceUpdate: {} })
                                })
                                .catch(v => {
                                    setState({ isLoading: false, commitError: true })
                                })
                                .finally(() => gimDialogState.setState!({ open: false }))
                        }}
                    >
                        Утвердить
                    </Button>
                </React.Fragment>
            )
        })

    }, [gimDialogState.setState, id, setState])

    const onDeleteClick = () => {
        gimDialogState.setState!({
            open: true,
            variant: 'Delete',
            onConfirm: () => {
                setState({ isLoading: true })
                client.api.priceListsDeleteOne({ id: id }, {})
                    .then(v => {
                        props.history.goBack()
                    })
                    .finally(() => gimDialogState.setState!({ open: false }))
            }
        })
    }

    const onSearchProductsClick = useCallback(() => {
        gimDialogState.setState!({
            open: true,
            title: 'Поиск похожей номенклатуры',
            content: 'Будет выполнен повторный поиск похожей номенклатуры',
            actions: (
                <React.Fragment>
                    <Button color="primary" onClick={() => gimDialogState.setState!({ open: false })}>Отмена</Button>
                    <Button
                        color="primary"
                        variant="contained"
                        onClick={() => {
                            setState({ isLoading: true })
                            client.api.priceListsSearchProducts(id)
                                .then(() => {
                                    setState({ isLoading: false, forceUpdate: {} })
                                })
                                .finally(() => gimDialogState.setState!({ open: false }))
                        }}
                    >
                        Выполнить поиск
                    </Button>
                </React.Fragment>
            )
        })
    }, [gimDialogState.setState, id, setState])

    const onSkipManyClick = useCallback((mode: 'code' | 'name' | 'price') => {
        setState({ isLoading: true })
        client.api.priceListItemsSkipMany({
            PriceListId: priceList.id,
            UnprocessedCodeError: mode === 'code',
            UnprocessedNameErrors: mode === 'name',
            UnprocessedPriceError: mode === 'price',
        }).then(v => {
            setState({ forceUpdate: {}, isLoading: false })
        })
    }, [priceList.id, setState])

    const onCreatePropertiesClick = useCallback(() => {
        setState({ isLoading: true })
        client.api.priceListItemPropertiesSetActionMany({ priceListId: id, action: 'CreateNew' }).then(() => {
            setState({ forceUpdate: {}, isLoading: false })
        })
    }, [id, setState])

    const onClearFilterClick = useCallback(() => {
        setState({ clearFilter: {} })
    }, [setState])

    const setForceUpdate = useCallback(() => {
        setState({ forceUpdate: {} })
    }, [setState])

    useEffect(() => {
        loadPriceList(id)
    }, [id, loadPriceList, setState, forceUpdate])

    return (
        <Box className={classes.root}>
            <Grid container spacing={2}>
                <Grid item xs={8}>
                    <Paper className={classes.paper}>
                        <Toolbar>
                            <Typography variant="h5" gutterBottom>Загрузка "{priceList.priceListFile.name}"</Typography>
                        </Toolbar>
                        <Grid container spacing={2}>
                            <Grid item xs={3}>
                                <TextField
                                    label="Поставщик"
                                    value={priceList.supplier}
                                    InputProps={{ readOnly: true }}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label="Загрузил"
                                    value={priceList.author}
                                    InputProps={{ readOnly: true }}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label="Текущий статус"
                                    value={PriceListStatuses[priceList.status] || ''}
                                    InputProps={{ readOnly: true }}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Button
                                    color="primary"
                                    variant="contained"
                                    style={{ marginTop: 8 }}
                                    onClick={() => {
                                        history.push({
                                            pathname: '/parsePriceList',
                                            search: qs.stringify({
                                                supplierId: priceList.supplierId,
                                                processingRuleId: priceList.processingRuleId
                                            })
                                        })
                                    }}
                                >
                                    Загрузить заново
                                </Button>
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label="Правило обработки"
                                    value={priceList.processingRule}
                                    InputProps={{ readOnly: true }}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label="Обработка произведена"
                                    value={priceList.parsedDate ? moment(priceList.parsedDate).format('DD.MM.YYYY HH:mm') : 'Еще не выполнялась'}
                                    InputProps={{ readOnly: true }}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label="Дата"
                                    value={priceList.statusDate ? moment(priceList.statusDate).format('DD.MM.YYYY HH:mm') : ''}
                                    InputProps={{ readOnly: true }}
                                    fullWidth
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Button
                                    color="primary"
                                    variant="contained"
                                    style={{ marginTop: 8 }}
                                    onClick={onSearchProductsClick}
                                >
                                    Повторный поиск
                                </Button>
                            </Grid>
                        </Grid>
                    </Paper>
                </Grid>
                <Grid item xs={4}></Grid>
                <Grid item xs={12}>
                    <Paper className={clsx(classes.paper, classes.positionRelative)}>
                        <GimCircularProgress isLoading={isLoading} />
                        <Box className={classes.paper}>
                            <Typography variant="h5" gutterBottom>Результат обработки</Typography>
                            <div style={{ display: 'flex' }}>
                                <PriceListPageToggler page={page} onChange={page => setState({ page: page })} isLoading={isLoading} />
                                <div style={{ margin: 'auto' }} />
                                {page === 'items' && (
                                    <Box>
                                        {(priceList.hasUnprocessedCodeErrors || priceList.hasUnprocessedNameErrors || priceList.hasUnprocessedPriceErrors) && (
                                            <React.Fragment>
                                                <Typography display="inline" className={classes.marginRight}>Удалить позиции</Typography>
                                                {priceList.hasUnprocessedCodeErrors && (
                                                    <Button
                                                        variant="outlined"
                                                        className={classes.marginRight}
                                                        onClick={() => onSkipManyClick('code')}
                                                        disabled={disabled}
                                                    >
                                                        Без артикула
                                                    </Button>
                                                )}
                                                {priceList.hasUnprocessedNameErrors && (
                                                    <Button
                                                        variant="outlined"
                                                        className={classes.marginRight}
                                                        onClick={() => onSkipManyClick('name')}
                                                        disabled={disabled}
                                                    >
                                                        Без наименования
                                                    </Button>
                                                )}
                                                {priceList.hasUnprocessedPriceErrors && (
                                                    <Button
                                                        variant="outlined"
                                                        className={classes.marginRight}
                                                        onClick={() => onSkipManyClick('price')}
                                                        disabled={disabled}
                                                    >
                                                        Без цены
                                                    </Button>
                                                )}
                                            </React.Fragment>
                                        )}
                                        <Button startIcon={<ClearIcon />} className={classes.marginLeft} onClick={onClearFilterClick}>Сбросить фильтры</Button>
                                    </Box>
                                )}
                                {page === 'properties' && priceList.hasPropertiesErrors && (
                                    <Box>
                                        <Button
                                            variant="outlined"
                                            className={classes.marginRight}
                                            onClick={onCreatePropertiesClick}
                                            disabled={propertiesActionsDisabled}
                                        >
                                            Создать все характеристики
                                        </Button>
                                    </Box>
                                )}
                            </div>
                        </Box>
                        {page === 'items' && (
                            <PriceListItems
                                id={id}
                                disabled={disabled}
                                clearFilter={clearFilter}
                                forceUpdate={forceUpdate}
                                setForceUpdate={setForceUpdate}
                            />
                        )}
                        {page === 'properties' && (
                            <PriceListProperties
                                id={id}
                                disabled={propertiesActionsDisabled}
                                forceUpdate={forceUpdate}
                                setForceUpdate={setForceUpdate}

                            />
                        )}
                        {priceList.status !== 'Committed' && (
                            <React.Fragment>
                                <Typography variant="h6" gutterBottom>Утверждение загрузки</Typography>
                                <Button
                                    variant="contained"
                                    color="primary"
                                    className={classes.marginRight}
                                    onClick={onCommit}
                                    disabled={commitDisabled}
                                >
                                    Утвердить
                                </Button>
                                <Button color="secondary" className={classes.marginRight} onClick={onDeleteClick} disabled={disabled || isLoading}>Удалить</Button>
                                {commitError && <Typography color="secondary" component="span" style={{ paddingLeft: '8px' }}>Произошла ошибка</Typography>}
                            </React.Fragment>
                        )}
                    </Paper>
                </Grid>
            </Grid>
        </Box >
    )
}

export default PriceList