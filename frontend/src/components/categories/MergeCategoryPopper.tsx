import React from 'react'
import { Popper, Paper, DialogTitle, DialogActions, Button } from '@material-ui/core'
import { PopperProps } from '@material-ui/core/Popper'
import * as client from 'client'

const MergeCategoryPopper: React.FC<Partial<PopperProps> & { onCancel: () => void, category?: client.CategoryLookup }> = (props) => {

    const name = (props.category && props.category.name) || ''
    return (
        <Popper open={props.open || false} anchorEl={props.anchorEl}>
            <Paper>
                <DialogTitle>Выберите категорию для объединения с "{name}"</DialogTitle>
                <DialogActions>
                    <Button color="primary" onClick={props.onCancel}>Отмена</Button>
                </DialogActions>
            </Paper>
        </Popper>
    )
}

export default MergeCategoryPopper