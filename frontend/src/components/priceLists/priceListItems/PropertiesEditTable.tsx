import React, { useCallback, useEffect, useRef } from 'react'
import * as client from 'client'
import { List, ListItem, ListItemText, makeStyles, createStyles, DialogActions, Button } from '@material-ui/core'
import GimIconButton from 'components/common/GimIconButton'
import { Done as DoneIcon, Clear as ClearIcon } from '@material-ui/icons'
import useMergeState from 'util/useMergeState'
import GimCircularProgress from 'components/common/GimCircularProgress'
import { PriceListItemStatuses, PriceListItemStatus, PriceListItemAction } from './types'

const useLocalStyles = makeStyles(theme =>
    createStyles({
        column: {
            minWidth: 150,
            flex: '1 0 150px'
        },
        dateColumn: {
            minWidth: '125px',
            marginRight: theme.spacing(1),
            display: 'flex',
            justifyContent: 'flex-end'
        },
        actionsColumn: {
            minWidth: '90px',
            marginLeft: 'auto',
            display: 'flex',
            justifyContent: 'flex-end'
        },
        buttonsLeft: {
            justifyContent: 'flex-start'
        }
    })
)

type Props = {
    item: client.PriceListItemLookup
    onConfirm: () => void
    onCancel: () => void
}

type PriceListItemProductPropertyLookupWithAction = client.PriceListItemProductPropertyLookup & {
    actionNew: 'Unknown' | 'CreateNew' | 'LeaveOld' | 'ApplyNew'
}

type State = {
    properties: PriceListItemProductPropertyLookupWithAction[]
    nameFixed: boolean
    isLoading: boolean
}

const initialState: State = {
    properties: [],
    nameFixed: false,
    isLoading: false
}

const PropertiesEditTable: React.FC<Props> = ({ item, onConfirm, onCancel }) => {
    const localClasses = useLocalStyles()

    const nameAction = useRef<PriceListItemAction>(item.nameAction)
    const [{ properties, nameFixed, isLoading }, setState] = useMergeState<State>(initialState)

    const loadProperties = useCallback((priceListItemId) => {
        setState({ isLoading: true })
        client.api.priceListItemPropertiesGetMany({ PriceListItemId: priceListItemId, productId: item.productId }, {}).then(v => {
            let props = v.data as PriceListItemProductPropertyLookupWithAction[]
            props.forEach(prop => prop.actionNew = prop.action)
            setState({ properties: props, isLoading: false })
        })
    }, [item.productId, setState])

    const onAction = useCallback((id: string, action: 'LeaveOld' | 'ApplyNew') => {
        setState({ isLoading: true })
        client.api.priceListItemPropertiesSetActionOne(id, { action: action }).then(v => {
            loadProperties(item.id)
        })
    }, [item.id, loadProperties, setState])

    const onNameAction = useCallback((action: 'LeaveOld' | 'ApplyNew') => {
        setState({ isLoading: true })
        client.api.priceListItemsSetNameActionOne(item.id, { action: action }).then(v => {
            nameAction.current = action
            setState({ nameFixed: true, isLoading: false })
        })
    }, [item.id, setState])

    useEffect(() => {
        loadProperties(item.id)
    }, [item.id, loadProperties])

    const GetStatus = useCallback((status: PriceListItemStatus, action: 'Unknown' | 'LeaveOld' | 'ApplyNew') => {
        return action !== 'Unknown' || nameFixed
            ? `????????????????????. ${action === 'ApplyNew' ? '?????????????????? ??????????' : ''}${action === 'LeaveOld' ? '?????????????????? ??????????????' : ''}`
            : PriceListItemStatuses[status]
    }, [nameFixed])

    return (
        <div style={{ position: 'relative' }}>
            <GimCircularProgress isLoading={isLoading} />
            <List dense>
                <ListItem key="header" divider>
                    <ListItemText primary="????????" className={localClasses.column} />
                    <ListItemText primary="?????????????? ????????????????" className={localClasses.column} />
                    <ListItemText primary="?????????? ????????????????" className={localClasses.column} />
                    <ListItemText primary="????????????" className={localClasses.column} />
                    <ListItemText primary="????????????????" className={localClasses.column} />
                </ListItem>
                <ListItem key="name" divider >
                    <ListItemText primary="????????????????????????" className={localClasses.column} />
                    <ListItemText primary={item.product || '- - - -'} className={localClasses.column} />
                    <ListItemText primary={item.productName || '- - - -'} className={localClasses.column} />
                    <ListItemText primary={GetStatus(item.nameStatus as any, nameAction.current as any)} className={localClasses.column} />
                    <div className={localClasses.column}>
                        {item.nameStatus !== 'Ok' && (
                            <GimIconButton Icon={ClearIcon} onClick={() => onNameAction('LeaveOld')} tooltip="???????????????? ??????????????" />
                        )}
                        {item.nameStatus !== 'Ok' && (
                            <GimIconButton Icon={DoneIcon} onClick={() => onNameAction('ApplyNew')} tooltip="?????????????????? ??????????" />
                        )}
                    </div>
                </ListItem>
                {properties.map(property => {
                    return (
                        <ListItem key={property.propertyKey} divider>
                            <ListItemText primary={property.property || property.propertyKey} className={localClasses.column} />
                            <ListItemText primary={property.productValue || '- - - -'} className={localClasses.column} />
                            <ListItemText primary={property.propertyValue || '- - - -'} className={localClasses.column} />
                            <ListItemText primary={GetStatus(property.status as any, property.action as any)} className={localClasses.column} />
                            <div className={localClasses.column}>
                                {/* ???????????????? ?????????????????????????????? ???????????? ?????? ???????????????? ????????????????????????, ???????? ?????? propertyId, ???????????? ?????? ?????????? ?????? ??????-???? */}
                                {property.propertyId && (
                                    <React.Fragment>
                                        {property.status !== 'Ok' && (
                                            <GimIconButton Icon={ClearIcon} onClick={() => onAction(property.id, 'LeaveOld')} tooltip="???????????????? ??????????????" />
                                        )}
                                        {property.status !== 'Ok' && (
                                            <GimIconButton Icon={DoneIcon} onClick={() => onAction(property.id, 'ApplyNew')} tooltip="?????????????????? ??????????" />
                                        )}
                                    </React.Fragment>
                                )}
                            </div>
                        </ListItem>
                    )
                })}
            </List>
            <DialogActions className={localClasses.buttonsLeft}>
                <Button color="primary" variant="contained" onClick={() => onConfirm()}>??????????????</Button>
            </DialogActions>
        </div>
    )
}

export default PropertiesEditTable