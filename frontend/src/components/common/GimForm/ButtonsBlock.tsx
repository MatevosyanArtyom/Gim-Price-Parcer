import React from 'react'
import useStyles from './styles'

type Props = {
    align: 'left' | 'right'
}

const ButtonsBlock: React.FC<Props> = ({ align, children }) => {

    const classes = useStyles()

    return (
        <div className={classes.buttons} style={{ justifyContent: align === 'left' ? 'flex-start' : 'flex-end', }}>
            {children}
        </div>
    )
}

export default ButtonsBlock