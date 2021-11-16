import React from 'react'
import { TableCellProps } from 'react-virtualized'
import clsx from 'clsx'
import { TableCell } from '@material-ui/core'
import { DeleteOutline } from '@material-ui/icons'
import GimIconButton from 'components/common/GimIconButton'
import useStyles from './styles'

type Props = TableCellProps & { onDelete: (props: TableCellProps, event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void }
// Какой-то хак хуков
const Wrapper = (props: Props) => {

    const classes = useStyles()

    return (
        <TableCell
            component="div"
            className={clsx(classes.tableCell, classes.flexContainer)}
            variant="body"
            style={{ paddingTop: 1, paddingBottom: 1 }}
        >
            <GimIconButton Icon={DeleteOutline} onClick={e => props.onDelete(props, e)} tooltip="Удалить" />
        </TableCell>
    )
}

const DeleteCell: React.FC<Props> = (props) => <Wrapper {...props} />

export default DeleteCell
