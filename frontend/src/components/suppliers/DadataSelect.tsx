import React, { HTMLAttributes } from 'react'
import AsyncSelect, { Props } from 'react-select/async'
import { Typography, MenuItem, Paper, TextField } from '@material-ui/core'
import * as client from 'client'
import useStyles from './styles'
import { ControlProps } from 'react-select/src/components/Control'
import { BaseTextFieldProps } from '@material-ui/core/TextField'
import { ValueContainerProps } from 'react-select/src/components/containers'
import { NoticeProps, MenuProps } from 'react-select/src/components/Menu'
import { OptionProps } from 'react-select/src/components/Option'

type InputComponentProps = Pick<BaseTextFieldProps, 'inputRef'> & HTMLAttributes<HTMLDivElement>;
function NoOptionsMessage(props: NoticeProps<client.FiasEntity>) {
    return (
        <Typography
            color="textSecondary"
            className={props.selectProps.classes.noOptionsMessage}
            {...props.innerProps}
        >
            Ничего не найдено
      </Typography>
    )
}

function inputComponent({ inputRef, ...props }: InputComponentProps) {
    return <div ref={inputRef} {...props} />
}

const Control = (props: ControlProps<client.FiasEntity>) => {
    const {
        children,
        innerProps,
        innerRef,
        selectProps: { classes, TextFieldProps },
    } = props

    return (
        <TextField
            fullWidth
            InputProps={{
                inputComponent,
                inputProps: {
                    className: classes.input,
                    ref: innerRef,
                    children,
                    fullWidth: true,
                    ...innerProps,
                },
            }}
            {...TextFieldProps}
            margin="normal"
        />
    )
}

function ValueContainer(props: ValueContainerProps<client.FiasEntity>) {
    return <div className={props.selectProps.classes.valueContainer}>{props.children}</div>
}

function Option(props: OptionProps<client.FiasEntity>) {
    return (
        <MenuItem
            ref={props.innerRef}
            selected={props.isFocused}
            component="div"
            style={{
                fontWeight: props.isSelected ? 500 : 400,
            }}
            {...props.innerProps}
        >
            {props.children}
        </MenuItem>
    )
}

function Menu(props: MenuProps<client.FiasEntity>) {
    return (
        <Paper square className={props.selectProps.classes.selectPaper} {...props.innerProps}>
            {props.children}
        </Paper>
    )
}


const DadataSelect: React.FC<Props<client.FiasEntity>> = (props) => {

    const classes = useStyles()

    return (
        <AsyncSelect
            getOptionLabel={option => option.value || ''}
            components={{ Control, Menu, NoOptionsMessage, Option, ValueContainer }}
            classes={classes}
            styles={{
                placeholder: provided => ({ ...provided, display: 'none' })
            }}
            cacheOptions
            {...props}
        />
    )
} 
export default DadataSelect