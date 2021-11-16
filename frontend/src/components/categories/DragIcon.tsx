import React, { forwardRef } from 'react'
import clsx from 'clsx'
import { Tooltip } from '@material-ui/core'
import {
    Height as HeightIcon,
    OpenWith as OpenWithIcon
} from '@material-ui/icons'
import useStyles from './styles'

type DragIconProps = { type: 'move' | 'newParent', disabled?: boolean }

const DragIcon = forwardRef<any, DragIconProps>(({ type, disabled }, ref) => {

    const classes = useStyles()

    const className = clsx(
        classes.buttonLike,
        type === 'move' && classes.cursorNsResize,
        type === 'newParent' && classes.cursorMove
    )

    const color = disabled ? 'disabled' : 'action'

    return (
        <Tooltip
            ref={ref}
            title={type === 'move' ? 'Изменить порядок' : 'Изменить родителя'}>
            <div className={className}>
                {
                    type === 'move'
                        ? <HeightIcon fontSize="small" color={color} />
                        : <OpenWithIcon fontSize="small" color={color} />
                }
            </div>
        </Tooltip>
    )
})

export default React.memo(DragIcon)