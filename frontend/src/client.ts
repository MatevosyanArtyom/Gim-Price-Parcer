import browserHistory from 'browserHistory'
import * as apiModule from 'api'

var apiObj = new apiModule.Api()
var api = apiObj.api;
var categoryApi = apiObj.categoryMapToMany

export const promiseWithCatch = <T>(promise: Promise<apiModule.HttpResponse<T>>): Promise<apiModule.HttpResponse<T>> => {
    promise.catch(error => {
        const url = (error.url).toLowerCase()
        switch (error.status) {
            case 401:
                browserHistory.push('/login')
                break
            //return Promise.reject({ ...error })
            case 403:
                // переадресация со всех страниц, кроме логина
                // на странице логина 403 означает верную пару логин-пароль, но заблокированную учетную запись
                if (url.includes('login')) {
                    //return Promise.reject({ ...error })
                } else {
                    browserHistory.push({ pathname: '/accessDenied' })
                    //return Promise.reject({ ...error })
                }
                break
        }
    })
    return promise
}

export * from 'api'
export { api, categoryApi }