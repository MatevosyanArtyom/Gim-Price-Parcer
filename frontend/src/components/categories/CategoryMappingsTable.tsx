import React, { useEffect } from 'react'
import * as client from 'client'
import useMergeState from 'util/useMergeState'
import { ListItem, List, TextField, IconButton, Tooltip, ListItemText, makeStyles, createStyles, DialogActions, Button } from '@material-ui/core'
import { Add as AddIcon, DeleteOutline as DeleteIcon } from '@material-ui/icons'
import moment from 'moment'
import useStyles from './styles'
import clsx from 'clsx'

type StateType = {
    mappings: client.CategoryMappingItem[]
}

type PropsType = {
    category: client.CategoryLookup,
    onConfirm: (mappings: client.CategoryMappingItem[]) => void
    onCancel: () => void
}

const initialState: StateType = {
    mappings: []
}

const useLocalStyles = makeStyles(theme =>
    createStyles({
        nameColumn: {
            minWidth: '200px',
            marginRight: theme.spacing(1)
        },
        dateColumn: {
            minWidth: '125px',
            marginRight: theme.spacing(1),
            display: 'flex',
            justifyContent: 'flex-end'
        },
        actionsColumn: {
            minWidth: '90px',
            marginLeft: 'auto',
            display: 'flex',
            justifyContent: 'flex-end'
        },
        buttonsLeft: {
            justifyContent: 'flex-start'
        }
    })
)

const CategoryMappingsTable: React.FC<PropsType> = ({ category, onConfirm, onCancel }) => {

    const [{ mappings }, setState] = useMergeState(initialState)

    const classes = useStyles()
    const localClasses = useLocalStyles()


    const newMapping = (): client.CategoryMappingItem => ({
        name: '',
        createdDate: moment().format()
    })

    useEffect(() => {
        let mappings = [...category.mappings]
        if (!mappings.length) {
            mappings.push(newMapping())
        }
        setState({ mappings: mappings })
    }, [category.mappings, setState])

    return (
        <React.Fragment>
            <List dense>
                <ListItem className={classes.headerbackgroundColor}>
                    <ListItemText
                        primary="Аналоги"
                        primaryTypographyProps={{ className: classes.fontWeightBold }}
                        className={localClasses.nameColumn}
                    />
                    <ListItemText
                        primary="Дата добавления"
                        primaryTypographyProps={{ className: classes.fontWeightBold }}
                        className={localClasses.dateColumn}
                    />
                    <ListItemText
                        primary="Действия"
                        primaryTypographyProps={{ className: classes.fontWeightBold }}
                        className={localClasses.actionsColumn}
                    />
                </ListItem>
                {mappings.map((v, i) => {
                    return (
                        <ListItem key={i} className={clsx(classes.listItem, classes.addCategoryForm, classes.borderBottom)}>
                            <TextField
                                value={v.name}
                                onChange={(e) => {
                                    mappings[i].name = e.target.value
                                    setState({ mappings })
                                }}
                                className={clsx(classes.categoryItemFieldInput, classes.marginRight1, localClasses.nameColumn)}
                            />
                            <ListItemText
                                primary={moment(v.createdDate).format('DD.MM.YYYY')}
                                className={clsx(localClasses.dateColumn, classes.marginRight1)}
                            />
                            <div className={localClasses.actionsColumn}>
                                <Tooltip title="Добавить">
                                    <IconButton
                                        className={classes.rowButton}
                                        onClick={e => {
                                            mappings.push(newMapping())
                                            setState({ mappings })
                                        }}
                                    >
                                        <AddIcon fontSize="small" />
                                    </IconButton>
                                </Tooltip>
                                <Tooltip title="Удалить">
                                    <IconButton
                                        className={classes.rowButton}
                                        onClick={e => {
                                            mappings.splice(i, 1)
                                            setState({ mappings })
                                        }}
                                    >
                                        <DeleteIcon fontSize="small" />
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </ListItem>
                    )
                })}
            </List>
            <DialogActions className={localClasses.buttonsLeft}>
                <Button color="primary" variant="contained" onClick={() => onConfirm(mappings)}>Сохранить</Button>
                <Button color="primary" onClick={onCancel}>Закрыть</Button>
            </DialogActions>
        </React.Fragment>
    )
}

export default CategoryMappingsTable