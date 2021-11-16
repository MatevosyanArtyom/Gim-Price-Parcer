import { useRef, useEffect } from 'react'

// Returns true if component is mounted
// We have to check if component is mounted before setState()
const useIsMounted = () => {
    const isMounted = useRef(false)
    useEffect(() => {
        isMounted.current = true
        return () => {
            isMounted.current = false
        }
    }, [])
    return isMounted
}

export default useIsMounted