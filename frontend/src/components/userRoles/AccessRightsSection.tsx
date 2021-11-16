import React from 'react'
import { Grid, ListItem, ListItemText } from '@material-ui/core'

type Props = {
    header?: string
}

const AccessRightsSection: React.FC<Props> = ({ header }) => {
    return (
        <Grid item xs={2}>
            <ListItem dense disableGutters style={{ paddingLeft: 8, height: '100%' }}>
                <ListItemText primary={header || '—'} />
            </ListItem>
        </Grid>
    )
}

export default AccessRightsSection