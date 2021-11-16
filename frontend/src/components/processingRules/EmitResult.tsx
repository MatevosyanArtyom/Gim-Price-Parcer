import React, { useCallback } from 'react'
import _ from 'lodash'
import { List, ListItemIcon, ListItem, ListItemText, Divider } from '@material-ui/core'
import { Cancel as CancelIcon, CheckCircle as CheckCircleIcon, Error as ErrorIcon, Info as InfoIcon, Warning as WarningIcon } from '@material-ui/icons'
import * as client from 'client'

const EmitResult: React.FC<{ result: client.EmitResultDto | null, margin?: number }> = ({ result, margin }) => {

    const getIcon = useCallback((severity: 'Hidden' | 'Info' | 'Warning' | 'Error') => {
        switch (severity) {
            case 'Error':
                return <ErrorIcon htmlColor="red" />
            case 'Warning':
                return <WarningIcon htmlColor="orange" />
            default:
                return <InfoIcon htmlColor="blue" />
        }
    }, [])

    if (!result || _.isEmpty(result)) {
        return <></>
    }

    return (
        <List disablePadding>
            <ListItem>
                <ListItemIcon>{
                    result.success
                        ? <CheckCircleIcon htmlColor="green" />
                        : <CancelIcon htmlColor="red" />
                }
                </ListItemIcon>
                <ListItemText primary={`${result.success ? 'Succeeded' : 'Failed'}`} />
            </ListItem>
            <Divider />
            {
                result.diagnostics &&
                result.diagnostics.map(v => (
                    <ListItem dense>
                        <ListItemIcon>
                            {getIcon(v.severity!)}
                        </ListItemIcon>
                        <ListItemText primary={v.message} />
                    </ListItem>
                ))
            }
        </List>
    )
}

export default EmitResult