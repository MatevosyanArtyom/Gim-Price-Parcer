import React from 'react'
import { IconButton, makeStyles, Theme, createStyles, Tooltip } from '@material-ui/core'
import { SvgIconProps } from '@material-ui/core/SvgIcon'

type Props = {
    Icon: (props: SvgIconProps) => JSX.Element
    onClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void
    tooltip: string
    disabled?: boolean
}

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        rowButton: {
            padding: theme.spacing(1)
        }
    })
)


const GimIconButton: React.FC<Props> = ({ Icon, onClick, tooltip, disabled }) => {
    const classes = useStyles()

    const renderButton = () => (
        <IconButton
            onClick={onClick}
            className={classes.rowButton}
            disabled={disabled}
        >
            <Icon fontSize="small" />
        </IconButton>
    )

    return disabled
        ? renderButton()
        : <Tooltip title={tooltip}>{renderButton()}</Tooltip>
}

export default GimIconButton