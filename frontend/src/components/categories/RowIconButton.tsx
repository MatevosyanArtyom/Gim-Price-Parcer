import React from 'react'
import { IconButton, Tooltip } from '@material-ui/core'
import {
    DeleteOutline,
    Edit as EditIcon,
    MergeType as MergeTypeIcon,
    Notes as NotesIcon,
} from '@material-ui/icons'
import useStyles from './styles'

type RowIconButtonProps = {
    type: 'edit' | 'map' | 'merge' | 'delete'
    onClick: any
    category: any
    mergeState: any
    depth: number
    disabled?: boolean
}

const RowIconButton: React.FC<RowIconButtonProps> = ({ type, onClick, category, mergeState, depth, disabled }) => {
    const classes = useStyles()

    // признак того, что сейчас выполняется объединение и обе категории являются конечными в своих ветках
    const tailCategories = mergeState.open && mergeState.category && !mergeState.category.hasChildren && !category.hasChildren

    const sameRootCategory = mergeState.open && mergeState.category!.rootCategory === category.rootCategory

    disabled = disabled ||

        // во время объединения все остальные кнопки недоступны
        (mergeState.open && type !== 'merge') ||

        // исходная категория недоступна для объединения
        (mergeState.open && type === 'merge' && category.id === mergeState.category!.id) ||

        // объединения возможны только в пределах одной тематики (т.е. корневые группы объединять нельзя)
        (type === 'merge' && !depth) ||

        // объединения возможны только в пределах одной тематики (т.е. корневая группа должна быть одинакова)
        (mergeState.open && type === 'merge' && !sameRootCategory) ||

        // объединения возможны только в рамках одной глубины иерархии
        // объединения в рамках разной глубины иерархии возможны, если обе категории являются конечными в своих ветках
        (mergeState.open && type === 'merge' && !tailCategories && mergeState.depth !== depth)

    let tooltip = ''
    let Icon = DeleteOutline
    switch (type) {
        case 'delete':
            tooltip = 'Удалить'
            Icon = DeleteOutline
            break
        case 'edit':
            tooltip = 'Редактировать'
            Icon = EditIcon
            break
        case 'map':
            tooltip = 'Маппинг'
            Icon = NotesIcon
            break
        case 'merge':
            tooltip = 'Объединить'
            Icon = MergeTypeIcon
            break
    }

    const renderButton = () => (
        <IconButton
            className={classes.rowButton}
            onClick={e => {
                e.stopPropagation()
                onClick()
            }}

            disabled={disabled}
        >
            <Icon fontSize="small" />
        </IconButton>
    )

    return disabled
        ? renderButton()
        : <Tooltip title={tooltip}>{renderButton()}</Tooltip >
}

export default React.memo(RowIconButton)
