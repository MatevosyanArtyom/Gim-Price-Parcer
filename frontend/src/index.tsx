import React from 'react'
import ReactDOM from 'react-dom'
import { Router } from 'react-router-dom'
import { Route, Switch } from 'react-router'
import browserHistory from 'browserHistory'
import Login from 'components/Login'
import PasswordChange from 'components/Login/PasswordChange'
import 'react-virtualized/styles.css'
import './index.css'
import App from './App'
import * as serviceWorker from './serviceWorker'

const Root = () => (
    <Router history={browserHistory}>
        <Switch>
            <Route exact path="/login" component={Login} />
            <Route exact path="/passwordChange/:token" component={PasswordChange} />
            <Route component={App} />
        </Switch>
    </Router>
)

ReactDOM.render(<Root />, document.getElementById('root'))

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister()
