import { useReducer, Dispatch } from 'react'

const reducer = (prevState: any, updater: any) => (
    typeof updater === 'function'
        ? { ...prevState, ...updater(prevState) }
        : { ...prevState, ...updater }
)

// useState не мержит новый стейт, а заменяет его. Этот hook мержит
// к тому же, работает синхронно (?)
const useMergeState = <T>(initialState: T): [T, Dispatch<Partial<T>>] => useReducer(reducer, initialState)

export default useMergeState