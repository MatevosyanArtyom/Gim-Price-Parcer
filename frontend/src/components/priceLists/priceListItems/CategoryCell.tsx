import React from 'react'
import { TableCellProps } from 'react-virtualized'
import { Edit as EditIcon } from '@material-ui/icons'
import { TableCell } from '@material-ui/core'
import GimIconButton from 'components/common/GimIconButton'

export type CellProps = TableCellProps & {
    level: number
    color?: string
    disabled: boolean
    onEditClick: () => void
    status: 'Error' | 'Ok'
}

const colors = {
    green: '#4caf50',
    red: '#f44336',
    grey: 'rgba(0, 0, 0, 0.26)'
}

const CategoryCell: React.FC<CellProps> = (props) => {

    return (
        <TableCell
            component="div"
            style={{ display: 'flex', alignItems: 'center', height: '100%', padding: '0px 4px' }}
            variant="body"
        >
            <span
                style={{ color: colors[props.color || ''], whiteSpace: 'normal' }}
            >
                {props.cellData}
            </span>
            {props.status === 'Error' && (
                <GimIconButton
                    Icon={EditIcon}
                    onClick={props.onEditClick}
                    tooltip="Редактировать"
                    disabled={props.rowData.skip || Boolean(props.rowData[`mapTo${props.level}Id`]) || props.disabled}
                />
            )}
        </TableCell>
    )
}

export default CategoryCell