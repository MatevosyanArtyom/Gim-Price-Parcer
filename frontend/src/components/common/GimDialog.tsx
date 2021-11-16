import React, { createContext, useContext } from 'react'
import { Dialog, DialogTitle, DialogActions, Button, DialogContent, DialogContentText } from '@material-ui/core'

export type GimDialogContextType = {
    open: boolean
    variant?: 'Delete' | 'Archive'
    title?: string
    content?: string | JSX.Element
    actions?: JSX.Element
    disableBackdropClick?: boolean
    disableEscapeKeyDown?: boolean
    onConfirm?: () => void
    setState?: (state: GimDialogContextType) => void
    width?: 'xs' | 'sm' | 'md' | 'lg' | 'xl'
}

export const initialGimDialogState: GimDialogContextType = {
    open: false
}

export const GimDialogContext = createContext<GimDialogContextType>(initialGimDialogState)

const GimDialog: React.FC = (props) => {
    let { open, variant, title, content, actions, disableBackdropClick, disableEscapeKeyDown, onConfirm, setState, width } = useContext(GimDialogContext)

    const onClose = () => {
        setState && setState(initialGimDialogState)
    }

    if (!content && variant) {
        switch (variant) {
            case 'Archive':
                content = <DialogContentText>Элемент будет перемещен в архив</DialogContentText>
                break
            case 'Delete':
                content = <DialogContentText>Элемент будет удален без возможности восстановления</DialogContentText>
                break
        }
    }

    if (content && typeof content === 'string') {
        content = <DialogContentText>{content}</DialogContentText>
    }

    if (!title && variant) {
        switch (variant) {
            case 'Archive':
                title = 'Отправить элемент коллекции в архив?'
                break
            case 'Delete':
                title = 'Удалить элемент коллекции?'
                break
        }
    }

    if (!actions && variant) {
        switch (variant) {
            case 'Archive':
                actions = (
                    <React.Fragment>
                        <Button color="primary">Отмена</Button>
                        <Button color="secondary">В архив</Button>
                    </React.Fragment>
                )
                break
            case 'Delete':
                actions = (
                    <React.Fragment>
                        <Button color="primary" onClick={onClose}>Отмена</Button>
                        <Button color="secondary" onClick={onConfirm}>Удалить</Button>
                    </React.Fragment>
                )
                break
        }
    }

    return (
        <Dialog
            onClose={onClose}
            open={open}
            disableBackdropClick={disableBackdropClick}
            disableEscapeKeyDown={disableEscapeKeyDown}
            maxWidth={width}
            disableScrollLock={true}
            fullWidth
        >
            <DialogTitle>{title || ''}</DialogTitle>
            <DialogContent>
                {content}
            </DialogContent>
            <DialogActions>
                {actions}
            </DialogActions>
        </Dialog>
    )
}

export default GimDialog