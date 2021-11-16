import React, { useCallback, useEffect, useContext } from 'react'
import clsx from 'clsx'
import update from 'immutability-helper'
import _ from 'lodash'
import { Paper, Grid, Box, Toolbar, Typography, List } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import GimCircularProgress from 'components/common/GimCircularProgress'
import useMergeState from 'util/useMergeState'
import CategoriesTree from './CategoriesTree'
import ProductPropertiesList from './CategoryPropertiesList'
import useStyles from './styles'
import { State } from './types'

const initialState: State = {
    categories: [],
    categoryId: '',
    expanded: {},
    isLoading: false
}

const ProductProperties: React.FC = () => {

    const [{
        categories,
        categoryId,
        expanded,
        isLoading,
    }, setState] = useMergeState(initialState)

    const classes = useStyles()

    const category = categories.find(x => x.id === categoryId) || {} as client.CategoryLookup

    const context = useContext(AppContext)

    const readOnly = !context.user.accessRights.properties.full

    const loadCategories = useCallback(() => {
        setState({ isLoading: true })
        client.api.lookupCategoriesGetChildrenFlatten({ 'parents[]': _.keys(_.pickBy(expanded, v => v)), includeRoot: true }, {}).then(v => {
            setState({ categories: v.data, isLoading: false })
        })
    }, [expanded, setState])


    const onCategoryExpand = useCallback((id: string) => {
        setState({
            expanded: update(expanded, { [id]: { $set: !expanded[id] } })
        })
    }, [expanded, setState])

    const onCategoryClick = useCallback((categoryId: string) => {
        setState({ categoryId: categoryId })
    }, [setState])

    useEffect(() => {
        loadCategories()
    }, [loadCategories])

    return (
        <Grid container>
            <Grid item xs={6}>
                <Paper className={classes.paper}>
                    <Toolbar>
                        <Typography variant="h5" >Категории</Typography>
                    </Toolbar>
                    <Box className={classes.box}>
                        <GimCircularProgress isLoading={isLoading} />
                        <List className={clsx(classes.list, classes.rootList)}>
                            <CategoriesTree
                                categories={categories}
                                categoryId={categoryId}
                                depth={0}
                                expanded={expanded}
                                loadCategories={loadCategories}
                                onCategoryClick={onCategoryClick}
                                onExpand={onCategoryExpand}
                                parentId=""
                            />
                        </List>
                    </Box>
                </Paper>
            </Grid>
            <Grid item xs={6}>
                <Paper className={clsx(classes.paper, classes.noMarginLeft)}>
                    <Toolbar>
                        <Typography variant="h5">Характеристики и значения</Typography>
                    </Toolbar>
                    <ProductPropertiesList categoryId={categoryId} readOnly={readOnly} hasChildren={category.hasChildren || false} />
                </Paper>
            </Grid>
        </Grid>
    )
}

export default ProductProperties