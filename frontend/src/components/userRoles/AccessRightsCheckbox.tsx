import React from 'react'
import { Grid, ListItem, ListItemIcon, Checkbox, ListItemText } from '@material-ui/core'

type Props = {
    name: string
    checked?: boolean
    onChange: (event: React.ChangeEvent<HTMLInputElement>) => void
    primary: string
    secondary?: string
    readOnly?: boolean
}

const AccessRightsCheckBox: React.FC<Props> = ({ name, checked, onChange, primary, secondary, readOnly }) => {
    onChange = readOnly ? () => { } : onChange
    return (
        <Grid item xs={2}>
            <ListItem dense disableGutters style={{ paddingRight: 8 }}>
                <ListItemIcon style={{ alignSelf: 'start', minWidth: 42 }}>
                    <Checkbox
                        name={name}
                        checked={checked || false}
                        onChange={onChange}
                    />
                </ListItemIcon>
                <ListItemText primary={primary} secondary={secondary} />
            </ListItem>
        </Grid>
    )
}

export default AccessRightsCheckBox