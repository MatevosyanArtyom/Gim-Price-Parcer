import React from 'react'
import { useDrop } from 'react-dnd'
import clsx from 'clsx'
import { Typography } from '@material-ui/core'
import * as client from 'client'
import useStyles from './styles'
import { DragObjectType, DropZoneProps } from './types'

const RootCategoryDropZone: React.FC<DropZoneProps> = props => {

    const { updateTree } = props

    const classes = useStyles()

    const [{ canDrop, isOver }, drop] = useDrop<DragObjectType, any, any>({
        accept: 'CategoryItem',
        drop: (item, monitor) => {

            client.api.categoriesUpdateParent({ id: item.category.id }).then(() => {
                updateTree(item.category.id, '')
            })

        },
        collect: monitor => ({
            canDrop: monitor.canDrop(),
            isOver: monitor.isOver()
        }),
    })

    const isActive = canDrop && isOver

    return canDrop
        ? <div ref={drop} className={clsx(classes.rootDropZone, isActive && classes.rootDropZoneHover)}>
            <Typography color="textSecondary">Для перемещения в корень перенесите категорию сюда</Typography>
        </div>
        : <div />

}

export default RootCategoryDropZone