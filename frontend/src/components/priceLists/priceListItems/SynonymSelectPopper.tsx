import React, { useEffect, useCallback } from 'react'
import * as client from 'client'
import useMergeState from 'util/useMergeState'
import { Box, List, ListItem, ListItemText, makeStyles, createStyles, DialogActions, Button, DialogContentText } from '@material-ui/core'
import GimCircularProgress from 'components/common/GimCircularProgress'
import { CheckOutlined } from '@material-ui/icons'

const useLocalStyles = makeStyles(theme =>
    createStyles({
        idColumn: {
            minWidth: 100,
            flex: '0 0 100px'
        },
        nameColumn: {
            minWidth: 400,
            flex: '1 0 400px'
        },
        scoreColumn: {
            minWidth: 100,
            flex: '0 0 100px'
        },
        actionColumn: {
            minWidth: '60px',
            flex: '0 0 60px',
            marginLeft: 'auto',
            display: 'flex',
            justifyContent: 'center'
        },
        buttons: {
            justifyContent: 'flex-start',
            paddingBottom: 0
        }
    })
)

type Props = {
    item: client.PriceListItemLookup
    onConfirm: () => void
    onCancel: () => void
}

type State = {
    synonyms: client.ProductSynonymDto[]
    selectedId: string
    isLoading: boolean
}

const initialState: State = {
    synonyms: [],
    selectedId: '',
    isLoading: true
}

const SynonymChoosePopper: React.FC<Props> = ({ item, onConfirm, onCancel }) => {

    const localClasses = useLocalStyles()

    const [{ synonyms, selectedId, isLoading }, setState] = useMergeState<State>({ ...initialState, selectedId: item.productId || '' })

    const onSubmit = useCallback(() => {
        setState({ isLoading: true })
        client.api.priceListItemsSetProductOne(item.id, selectedId)
            .then(() => {
                onConfirm()
            })
            .catch(() => setState({ isLoading: false }))
    }, [item.id, onConfirm, selectedId, setState])

    useEffect(() => {
        setState({ isLoading: true })
        client.api.priceListItemsGetSynonymsMany(item.id)
            .then(v => {
                setState({
                    synonyms: v.data,
                    isLoading: false
                })
            })
            .finally(() => setState({ isLoading: false }))
    }, [item.id, setState])

    return (
        <Box style={{ position: 'relative' }}>
            <GimCircularProgress isLoading={isLoading} />
            <DialogContentText>{item.productName}</DialogContentText>
            <List dense disablePadding>
                <ListItem key="header" divider>
                    <ListItemText primary="ID" className={localClasses.idColumn} primaryTypographyProps={{ variant: 'subtitle2' }} />
                    <ListItemText primary="Наименование" className={localClasses.nameColumn} primaryTypographyProps={{ variant: 'subtitle2' }} />
                    <ListItemText primary="Score" className={localClasses.scoreColumn} primaryTypographyProps={{ variant: 'subtitle2' }} />
                    <ListItemText primary="Выбрано" className={localClasses.actionColumn} primaryTypographyProps={{ variant: 'subtitle2' }} />
                </ListItem>
                {synonyms.map(p => (
                    <ListItem key={p.productId} button onClick={() => setState({ selectedId: p.productId })}>
                        <ListItemText primary={p.product.seqId} className={localClasses.idColumn} />
                        <ListItemText primary={p.product.name} className={localClasses.nameColumn} />
                        <ListItemText primary={p.score?.toFixed(3)} className={localClasses.scoreColumn} />
                        <div className={localClasses.actionColumn}>
                            {selectedId === p.productId && <CheckOutlined />}
                        </div>
                    </ListItem>
                ))}
            </List>
            <DialogActions className={localClasses.buttons}>
                <Button color="primary" variant="contained" onClick={onSubmit}>Сохранить</Button>
                <Button color="primary" onClick={onCancel}>Закрыть</Button>
            </DialogActions>
        </Box >
    )
}

export default SynonymChoosePopper