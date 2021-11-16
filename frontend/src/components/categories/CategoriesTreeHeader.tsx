import React from 'react'
import { List, ListItem, ListItemText } from '@material-ui/core'
import clsx from 'clsx'
import RowButtonPlaceholder from './RowButtonPlaceholder'
import useStyles from './styles'

const CategoriesTreeHeader: React.FC = () => {

    const classes = useStyles()

    return (
        <div style={{ display: 'flex' }}>
            <List id="header" className={clsx(classes.list, classes.headerbackgroundColor)} style={{ flex: '1' }}>
                <ListItem className={classes.listItem}>
                    <RowButtonPlaceholder />
                    <ListItemText
                        primary="Категория 1"
                        primaryTypographyProps={{ variant: 'h6', className: clsx(classes.fontWeightBold, classes.negativeMargin36) }}
                        className={classes.flex1percent}
                    />
                    <RowButtonPlaceholder />
                    <ListItemText
                        primary="Категория 2"
                        primaryTypographyProps={{ variant: 'h6', className: clsx(classes.fontWeightBold, classes.negativeMargin36) }}
                        className={classes.flex1percent}
                    />
                    <RowButtonPlaceholder />
                    <ListItemText
                        primary="Категория 3"
                        primaryTypographyProps={{ variant: 'h6', className: clsx(classes.fontWeightBold, classes.negativeMargin36) }}
                        className={classes.flex1percent}
                    />
                    <RowButtonPlaceholder />
                    <ListItemText
                        primary="Категория 4"
                        primaryTypographyProps={{ variant: 'h6', className: clsx(classes.fontWeightBold, classes.negativeMargin36) }}
                        className={classes.flex1percent}
                    />
                    <RowButtonPlaceholder />
                    <ListItemText
                        primary="Категория 5"
                        primaryTypographyProps={{ variant: 'h6', className: clsx(classes.fontWeightBold, classes.negativeMargin36) }}
                        className={classes.flex1percent}
                    />
                    <ListItemText
                        primary="Товаров"
                        primaryTypographyProps={{ variant: 'h6', className: classes.fontWeightBold }}
                        className={classes.productsCountColumn}
                    />
                    <ListItemText
                        primary="Статус"
                        primaryTypographyProps={{ variant: 'h6', className: classes.fontWeightBold }}
                        className={clsx(classes.flex02, classes.marginRight1, classes.minWidth105)}
                    />
                    <RowButtonPlaceholder />
                    <RowButtonPlaceholder />
                    <RowButtonPlaceholder />
                    <RowButtonPlaceholder />
                    <RowButtonPlaceholder />
                    <RowButtonPlaceholder />
                </ListItem>
            </List>
            <div style={{ minWidth: '17px' }}></div>
        </div>
    )
}

export default CategoriesTreeHeader