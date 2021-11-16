import React from 'react'
import { Typography } from '@material-ui/core'

const AccessDenied: React.FC = () => {
    return (
        <div style={{ display: 'flex', height: '100vh', width: '100vw' }}>
            <div style={{ display: 'flex', height: '70%', width: '100%', alignItems: 'center', justifyContent: 'center' }}>
                <Typography variant="h4" style={{ color: 'red' }}>Доступ закрыт</Typography>
            </div>
        </div>
    )
}

export default AccessDenied