import React from 'react'
import { Grid, Divider } from '@material-ui/core'


const AccessRightsDivider: React.FC = () => {
    return (
        <Grid container item xs={12}>
            <Grid item xs={10}>
                <Divider component="div" />
            </Grid>
            <Grid item xs={2} />
        </Grid>
    )
}

export default AccessRightsDivider