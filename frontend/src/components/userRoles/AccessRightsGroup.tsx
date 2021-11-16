import React from 'react'
import { Grid, Typography } from '@material-ui/core'

type Props = {
    header: string
}

const AccessRightsGroup: React.FC<Props> = ({ header, children }) => {
    return (
        <Grid container item xs={12} style={{ marginBottom: 16 }}>
            <Grid item xs={2}>
                <Typography variant="h6" gutterBottom>{header}</Typography>
            </Grid>
            <Grid container item xs={10}>
                <Grid item xs={2} style={{ backgroundColor: 'rgba(0, 0, 0, 0.07)', padding: '8px' }}><Typography variant="subtitle2">Подраздел</Typography></Grid>
                <Grid item xs={8} style={{ backgroundColor: 'rgba(0, 0, 0, 0.07)', padding: '8px' }}><Typography variant="subtitle2">Уровни доступа</Typography></Grid>
                <Grid item xs={2} />
                {children}
            </Grid>
        </Grid>
    )
}

export default AccessRightsGroup