import { HttpResponse } from 'client'

const response = <T>(data: T): HttpResponse<T> => ({
    data,
    status: 200,
    statusText: ''
} as HttpResponse<T>)

const promiseWithHttpResponse = <T>(value: T) => Promise.resolve(response(value))

export default promiseWithHttpResponse