import React, { useEffect, useCallback, useMemo } from 'react'
import { Route } from 'react-router'
import { DndProvider } from 'react-dnd'
import { HTML5Backend } from 'react-dnd-html5-backend'
import * as client from 'client'

import './App.css'
import GimDialog, { GimDialogContext, initialGimDialogState, GimDialogContextType } from 'components/common/GimDialog'
import Header from 'components/Header'
import AccessDenied from 'components/common/AccessDenied'
import Category from 'components/categories/Category'
import Categories from 'components/categories/Categories'
import CommitedPriceLists from 'components/priceLists/CommitedPriceLists'
import ParsePriceList from 'components/priceLists/ParsePriceList'
import ParsedPriceLists from 'components/priceLists/ParsedPriceLists'
import PriceList from 'components/priceLists/PriceList'
import ProcessingRule from 'components/processingRules/ProcessingRule'
import ProcessingRules from 'components/processingRules/ProcessingRules'
import Product from 'components/products/Product'
import Products from 'components/products/Products'
import ProductProperties from 'components/categoryProperties/ProductProperties'
import Supplier from 'components/suppliers/Supplier'
import Suppliers from 'components/suppliers/Suppliers'
import SchedulerTask from 'components/schedulerTasks/SchedulerTask'
import SchedulerTasks from 'components/schedulerTasks/SchedulerTasks'
import User from 'components/users/User'
import Users from 'components/users/Users'
import UserRole from 'components/userRoles/UserRole'
import UserRoles from 'components/userRoles/UserRoles'
import useMergeState from 'util/useMergeState'
import AppContext, { initialContext } from 'context'
import { CssBaseline } from '@material-ui/core'

const initialState = {
    appState: initialContext,
    gimDialogState: initialGimDialogState,
    isLoading: false
}

const App: React.FC = () => {

    const [{ appState, gimDialogState }, setState] = useMergeState(initialState)

    const getUserInfo = useCallback(() => {
        setState({ isLoading: true })
        client.promiseWithCatch(client.api.accountsUserInfo({})).then(v => {
            setState({
                appState: { user: v.data || initialContext.user },
                isLoading: false
            })
        })
    }, [setState])

    const setGimDialogState = useCallback((state: GimDialogContextType) => {
        setState({ gimDialogState: { ...state } })
    }, [setState])

    useEffect(() => {
        getUserInfo()
    }, [getUserInfo, setState])

    const gimDialogStateMemo = useMemo(() => ({
        ...gimDialogState,
        setState: setGimDialogState
    }), [gimDialogState, setGimDialogState])

    return (
        <React.Fragment>
            <CssBaseline />
            <GimDialogContext.Provider value={gimDialogStateMemo}>
                <GimDialog />
                <AppContext.Provider value={appState}>
                    <DndProvider backend={HTML5Backend}>
                        <Header />
                        <Route exact path="/accessDenied" component={AccessDenied} />
                        <Route exact path="/categories" component={Categories} />
                        <Route exact path="/categories/:id" component={Category} />
                        <Route exact path="/parsePriceList" component={ParsePriceList} />
                        <Route exact path="/parsedPriceLists" component={ParsedPriceLists} />
                        <Route exact path="/parsedPriceLists/:id" component={PriceList} />
                        <Route exact path="/commitedPriceLists" component={CommitedPriceLists} />
                        <Route exact path="/commitedPriceLists/:id" component={PriceList} />
                        <Route exact path="/processingRules" component={ProcessingRules} />
                        <Route exact path="/processingRules/:id" component={ProcessingRule} />
                        <Route exact path="/products" component={Products} />
                        <Route exact path="/products/:id" component={Product} />
                        <Route exact path="/productProperties" component={ProductProperties} />
                        <Route exact path="/schedulerTasks" component={SchedulerTasks} />
                        <Route exact path="/schedulerTasks/:id" component={SchedulerTask} />
                        <Route exact path="/suppliers" component={Suppliers} />
                        <Route exact path="/suppliers/:id" component={Supplier} />
                        <Route exact path="/users" component={Users} />
                        <Route exact path="/users/:id" component={User} />
                        <Route exact path="/userRoles" component={UserRoles} />
                        <Route exact path="/userRoles/:id" component={UserRole} />
                    </DndProvider>
                </AppContext.Provider>
            </GimDialogContext.Provider>
        </React.Fragment>
    )
}

export default App
