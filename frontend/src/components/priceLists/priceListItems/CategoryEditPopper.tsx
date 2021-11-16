import React, { useEffect, useCallback } from 'react'
import { Grid, Paper, DialogContentText, createStyles, Theme, makeStyles, Button, TextField, MenuItem, DialogActions } from '@material-ui/core'
import * as client from 'client'
import clsx from 'clsx'
import useMergeState from 'util/useMergeState'
import GimCircularProgress from 'components/common/GimCircularProgress'

export const useStyles = makeStyles((theme: Theme) => (
    createStyles({
        dialogActions: {
            justifyContent: 'flex-start',
            padding: 0
        },
        paper: {
            padding: theme.spacing(2),
            display: 'flex',
            flexDirection: 'column',
            minHeight: '150px'
        },
        marginBottomAuto: {
            marginBottom: 'auto'
        },
        marginTop3: {
            marginTop: theme.spacing(3)
        },
        marginRight2: {
            marginRight: theme.spacing(2)
        },
        padding2: {
            padding: theme.spacing(2),
        },
        positionRelative: {
            position: 'relative'
        }
    })
))

type Props = {
    rowData: client.PriceListItemLookup
    level: number
    onCancel: () => void
    onConfirm: () => void
}

type State = {
    categories1: client.CategoryLookup[]
    categories2: client.CategoryLookup[]
    categories3: client.CategoryLookup[]
    categories4: client.CategoryLookup[]
    categories5: client.CategoryLookup[]
    categoryId1: string
    categoryId2: string
    categoryId3: string
    categoryId4: string
    categoryId5: string
    categorySelectedId: string
    mappings: string[]
    mode?: 'map' | 'skip'
    isLoading: boolean
}

const initialState: State = {
    categories1: [],
    categories2: [],
    categories3: [],
    categories4: [],
    categories5: [],
    categoryId1: '',
    categoryId2: '',
    categoryId3: '',
    categoryId4: '',
    categoryId5: '',
    categorySelectedId: '',
    mappings: [],
    isLoading: false
}

const CategoryEditPopper: React.FC<Props> = ({ rowData, level, onCancel, onConfirm }) => {

    const classes = useStyles()

    const [state, setState] = useMergeState(initialState)
    const {
        categories1,
        categories2,
        categories3,
        categories4,
        categories5,
        categoryId1,
        categoryId2,
        categoryId3,
        categoryId4,
        categoryId5,
        categorySelectedId,
        mappings,
        mode,
        isLoading
    } = state

    const categoryName = rowData[`category${level}Name`]

    const onCategoryChange = useCallback((level: number, categoryId: string) => {
        const category = state[`categories${level}`].find((x: client.CategoryLookup) => x.id === categoryId) as client.CategoryLookup
        if (category.hasChildren) {
            setState({ isLoading: true })
            client.api.categoriesGetChildrenFlatten({ 'parents[]': categoryId ? [categoryId] : [] }, {}).then(v => {
                setState({
                    [`categories${level + 1}`]: v.data,
                    [`categoryId${level}`]: categoryId,
                    isLoading: false
                })
            })
        } else {
            setState({ [`categoryId${level}`]: categoryId, categorySelectedId: categoryId, mappings: category.mappings.map(m => m.name!) })
        }
    }, [setState, state])

    const onMapClick = useCallback(() => {
        setState({ isLoading: true })
        client.categoryApi.priceListItemsSetCategoryMapToMany(
            rowData.priceListId,
            categorySelectedId,
            level,
            { categoryName: categoryName }
        ).then(v => {
            setState({ isLoading: false })
            onConfirm()
        })
    }, [categoryName, categorySelectedId, level, onConfirm, rowData.priceListId, setState])

    const onSkipClick = useCallback(() => {
        setState({ isLoading: true })
        client.api.priceListItemsSkipMany({ PriceListId: rowData.priceListId, CategoryName: categoryName }, {}).then(v => {
            setState({ isLoading: false })
            onConfirm()
        })
    }, [categoryName, onConfirm, rowData.priceListId, setState])

    useEffect(() => {
        if (mode !== 'map') { return }
        setState({ isLoading: true })
        client.api.categoriesGetChildrenFlatten({ includeRoot: true }, {}).then(v => {
            setState({ categories1: v.data, isLoading: false })
        })
    }, [mode, setState])

    return (
        <div>
            <Grid container>
                {!mode && (
                    <React.Fragment>
                        <Grid item xs={6}>
                            <Paper className={clsx(classes.paper, classes.marginRight2)}>
                                <DialogContentText className={classes.marginBottomAuto} color="textPrimary">Задать меппинг на существующий уровень каталога</DialogContentText>
                                <div><Button color="primary" variant="contained" onClick={() => setState({ mode: 'map' })}>Выбрать</Button></div>
                            </Paper>
                        </Grid>
                        <Grid item xs={6}>
                            <Paper className={classes.paper}>
                                <DialogContentText className={classes.marginBottomAuto} color="textPrimary">Удалить все записи, принадлежащие уровню "{categoryName}"</DialogContentText>
                                <div><Button color="primary" variant="contained" onClick={() => setState({ mode: 'skip' })}>Выбрать</Button></div>
                            </Paper>
                        </Grid>
                    </React.Fragment>
                )}
                {mode === 'map' && (
                    <React.Fragment>
                        <Grid item xs={6}>
                            <Paper className={clsx(classes.paper, classes.marginRight2, classes.positionRelative)}>
                                <GimCircularProgress isLoading={isLoading} />
                                <DialogContentText color="textPrimary">Задаем меппинг на существующий уровень</DialogContentText>
                                <TextField
                                    label="Категория 1"
                                    value={categoryId1}
                                    onChange={e => onCategoryChange(1, e.target.value)}
                                    margin="dense"
                                    select
                                    fullWidth
                                >
                                    {categories1.map((v: any) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                                </TextField>
                                <TextField
                                    label="Категория 2"
                                    value={categoryId2}
                                    onChange={e => onCategoryChange(2, e.target.value)}
                                    disabled={!categoryId1 || (!categoryId2 && Boolean(categorySelectedId))}
                                    margin="dense"
                                    select
                                    fullWidth
                                >
                                    {categories2.map((v: any) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                                </TextField>
                                <TextField
                                    label="Категория 3"
                                    value={categoryId3}
                                    onChange={e => onCategoryChange(3, e.target.value)}
                                    disabled={!categoryId2 || (!categoryId2 && Boolean(categorySelectedId))}
                                    margin="dense"
                                    select
                                    fullWidth
                                >
                                    {categories3.map((v: any) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                                </TextField>
                                <TextField
                                    label="Категория 4"
                                    value={categoryId4}
                                    onChange={e => onCategoryChange(4, e.target.value)}
                                    disabled={!categoryId3 || (!categoryId3 && Boolean(categorySelectedId))}
                                    margin="dense"
                                    select
                                    fullWidth
                                >
                                    {categories4.map((v: any) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                                </TextField>
                                <TextField
                                    label="Категория 5"
                                    value={categoryId5}
                                    onChange={e => onCategoryChange(5, e.target.value)}
                                    disabled={!categoryId4 || (!categoryId4 && Boolean(categorySelectedId))}
                                    margin="dense"
                                    select
                                    fullWidth
                                >
                                    {categories5.map((v: any) => <MenuItem key={v.id} value={v.id}>{v.name}</MenuItem>)}
                                </TextField>
                                {categorySelectedId && mappings.length > 0 && (
                                    <DialogContentText variant="body2" className={classes.marginTop3}>
                                        Существующие маппинги: {mappings.join(', ')}
                                    </DialogContentText>
                                )}
                                <DialogContentText color="textPrimary">Создать меппинг и разместить все товары в родственной подгруппе</DialogContentText>
                                <DialogActions className={classes.dialogActions}>
                                    <Button color="primary" variant="contained" disabled={!categorySelectedId} onClick={onMapClick}>Выбрать</Button>
                                    <Button onClick={() => setState({ mode: undefined })}>Назад</Button>
                                </DialogActions>
                            </Paper>
                        </Grid>
                    </React.Fragment>
                )}
                {mode === 'skip' && (
                    <React.Fragment>
                        <Grid item xs={6}>
                        </Grid>
                        <Grid item xs={6}>
                            <Paper className={clsx(classes.paper, classes.positionRelative)}>
                                <GimCircularProgress isLoading={isLoading} />
                                <DialogContentText color="textPrimary" className={classes.marginBottomAuto}>
                                    Собираемся удалить все записи, принадлежащие уровню "{categoryName}"
                                </DialogContentText>
                                <DialogActions className={classes.dialogActions}>
                                    <Button color="secondary" onClick={onSkipClick}>Удалить записи</Button>
                                    <Button onClick={() => setState({ mode: undefined })}>Назад</Button>
                                </DialogActions>
                            </Paper>
                        </Grid>
                    </React.Fragment>
                )}
            </Grid>
        </div >
    )
}

export default CategoryEditPopper