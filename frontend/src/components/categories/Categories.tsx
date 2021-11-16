import React, { useEffect, useCallback, useRef, useContext } from 'react'
import { withRouter, RouteComponentProps } from 'react-router'
import _ from 'lodash'
import clsx from 'clsx'
import update from 'immutability-helper'
import { List, Paper, Toolbar, Typography, Grid, Fab, Link, Button } from '@material-ui/core'
import { ShoppingCart as ShoppingCartIcon } from '@material-ui/icons'
import * as client from 'client'
import { GimDialogContext } from 'components/common/GimDialog'
import useMergeState from 'util/useMergeState'
import CategoriesTree from './CategoriesTree'
import CategoriesTreeHeader from './CategoriesTreeHeader'
import useStyles from './styles'
import { initialState, initialEditState } from './types'
import MergeCategoryPopper from './MergeCategoryPopper'
import CategoryMappingsTable from './CategoryMappingsTable'
import GimCircularProgress from 'components/common/GimCircularProgress'
import AppContext from 'context'

const Categories: React.FC<RouteComponentProps> = (props) => {

    let [{
        categories,
        expanded,
        scrollTo,
        editState,
        mergeState,
        isLoading
    }, setState] = useMergeState(initialState)

    const fileInputRef = useRef<HTMLInputElement>(null)
    const popperAnchorRef = useRef<HTMLDivElement>(null)
    const rootPaperRef = useRef<HTMLDivElement>(null)
    const gimDialogState = useContext(GimDialogContext)
    const context = useContext(AppContext)

    const readOnly = !context.user.accessRights.categories.full

    const classes = useStyles()

    const updateTree = useCallback(() => {
        setState({ isLoading: true })
        client.api.categoriesGetChildrenFlatten({ 'parents[]': _.keys(_.pickBy(expanded, v => v)), includeRoot: true }).then(v => {
            setState({ categories: v.data || initialState.categories, isLoading: false })
        })
    }, [expanded, setState])

    useEffect(() => {
        if (scrollTo) {
            let list = document.getElementById('rootList')
            let node = document.getElementById(scrollTo)
            if (list && node) {
                list.scrollTo(0, node.offsetTop)
                setState({ scrollTo: '' })
            }
        }
    })

    const onExpand = useCallback((categoryId: string) => {
        setState({
            expanded: { ...expanded, [categoryId]: !expanded[categoryId] }
        })
    }, [expanded, setState])

    const onAddClick = useCallback((parentId: string) => {
        setState({ editState: { isEditing: true, parentId: parentId } })
    }, [setState])

    const onEditClick = useCallback((id: string) => {
        setState({ editState: { isEditing: true, categoryId: id } })
    }, [setState])

    const onMapClick = useCallback((category: client.CategoryLookup) => {
        gimDialogState.setState!({
            open: true,
            title: `Маппинг для "${category.name}"`,
            content: (
                <CategoryMappingsTable
                    category={category}
                    onCancel={() => gimDialogState.setState!({ open: false })}
                    onConfirm={(mappings) => {
                        client.api.categoriesUpdateMappings(category.id, mappings).then(v => {
                            gimDialogState.setState!({ open: false })
                            updateTree()
                        })
                    }}
                />),
            disableBackdropClick: true,
            disableEscapeKeyDown: true
        })
    }, [gimDialogState.setState, updateTree])

    const onMergeClick = useCallback((category: client.CategoryLookup, depth: number) => {
        if (!mergeState.open) {
            setState({ mergeState: { open: true, category: category, depth: depth } })
        } else {
            setState({ isLoading: true })
            client.api.categoriesMergeOne({
                fromId: (mergeState.category && mergeState.category.id) || '',
                toId: category.id
            }).then(() => {
                setState({ mergeState: { open: false }, isLoading: false })
                updateTree()
            })
        }
    }, [mergeState.category, mergeState.open, setState, updateTree])

    const onDeleteClick = useCallback((id: string) => {
        gimDialogState.setState!({
            open: true,
            variant: 'Delete',
            onConfirm: () => {
                setState({ isLoading: true })
                client.api.categoriesDeleteOne({ id: id })
                    .then(v => {
                        updateTree()
                    })
                    .finally(() => gimDialogState.setState!({ open: false }))
            }
        })
    }, [gimDialogState.setState, setState, updateTree])

    const onSaveClick = (item: client.CategoryEdit) => {
        setState({ isLoading: true })
        const method = item.id ? client.api.categoriesUpdateOne : client.api.categoriesAddOne
        method(item, {}).then(v => {
            updateTree()
        })
    }

    const onCancelClick = () => {
        setState({ editState: initialEditState })
    }

    const onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        let fileReader = new FileReader()
        if (event.target.files) {
            let file = event.target.files[0]

            fileReader.onload = () => {
                let gimFile: client.GimFileAdd = {
                    name: file.name,
                    size: file.size,
                    data: fileReader.result as string
                }

                client.api.categoriesFromYandexMarket(gimFile).then(() => {
                    updateTree()
                })
            }
            fileReader.readAsDataURL(event.target.files[0])
        }
    }

    const onDragHover = (srcIndex: number, dstIndex: number) => {

        // console.log('----------------')
        // console.log(`${srcIndex}-${dstIndex}`)
        // const src = findCategory(categories, srcId)
        // const dst = findCategory(categories, dstId)
        // console.log(categories.map(x => x.item.name).join(','))
        // categories.splice(categories.indexOf(src), 1)
        // console.log(categories.map(x => x.item.name).join(','))
        // categories.splice(categories.indexOf(dst), 0, src)
        // console.log(categories.map(x => x.item.name).join(','))
        // insertCategory(categories, dstId, src)
        // console.log(categories)

        // categoriesDict
        let src = categories[srcIndex]
        src.parent = categories[dstIndex].parent
        setState({
            categories: update(categories, {
                $splice: [[srcIndex, 1], [dstIndex, 0, src]],
            })
        })
    }

    useEffect(() => {
        updateTree()
    }, [updateTree])

    return (
        <React.Fragment>
            <Grid container>
                <Grid item xs={12}>
                    <Paper ref={rootPaperRef} className={classes.paper}>
                        <Toolbar>
                            <Typography variant="h5" >Категории</Typography>
                            <div style={{ flexGrow: 1 }} />
                            <Button
                                color="secondary"
                                onClick={() => {
                                    client.api.categoriesDeleteMany().then(() => updateTree())
                                }}
                                disabled={readOnly}
                            >
                                Удалить все
                            </Button>
                        </Toolbar>
                        <CategoriesTreeHeader />
                        <Paper elevation={0} style={{ position: 'relative' }}>
                            <GimCircularProgress isLoading={isLoading} />
                            <List className={clsx(classes.list, classes.rootList)}>
                                <CategoriesTree
                                    categories={categories}
                                    categoryId=""
                                    depth={0}
                                    expanded={expanded}
                                    onAddClick={onAddClick}
                                    onCancelClick={onCancelClick}
                                    onDeleteClick={onDeleteClick}
                                    onEditClick={onEditClick}
                                    onExpand={onExpand}
                                    onMapClick={onMapClick}
                                    onMergeClick={onMergeClick}
                                    onSaveClick={onSaveClick}
                                    onDragHover={onDragHover}
                                    updateTree={updateTree}
                                    editState={editState}
                                    mergeState={mergeState}
                                    readOnly={readOnly}
                                />
                            </List>
                        </Paper>
                    </Paper>
                </Grid>
                <Grid item xs={4}>
                    <div ref={popperAnchorRef} />
                    <MergeCategoryPopper
                        open={mergeState.open}
                        category={mergeState.category}
                        anchorEl={popperAnchorRef.current}
                        onCancel={() => setState({ mergeState: { open: false } })}
                    />
                </Grid>
                <Grid item xs={4}>
                    <Paper className={classes.marketPaper}>
                        <Typography variant="body1">
                            Для заполнения категориями из Яндекс-Маркета загрузите файл по
                            <Link download href="https://download.cdn.yandex.net/support/ru/partnermarket/files/market_categories.xls"> ссылке</Link>,
                                        затем сохраните его в формате xlsx с помощью Microsoft Excel и укажите полученный файл здесь.
                        </Typography>
                        <Fab
                            variant="extended"
                            className={classes.fab}
                            onClick={() => fileInputRef.current && fileInputRef.current.click()}
                            disabled={readOnly}
                        >
                            <ShoppingCartIcon className={classes.extendedIcon} />
                            Выбрать файл
                        </Fab>
                        <input
                            ref={fileInputRef}
                            accept=".xlsx"
                            className={classes.input}
                            id="contained-button-file"
                            type="file"
                            onChange={onChange}
                        />
                    </Paper>
                </Grid>
            </Grid>
            {/* <CategoryVersions /> */}
        </React.Fragment>
    )
}

export default withRouter(Categories)