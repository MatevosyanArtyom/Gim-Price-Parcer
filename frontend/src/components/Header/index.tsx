import React, { useContext } from 'react'
import { withRouter, } from 'react-router-dom'
import { RouteComponentProps } from 'react-router'
import _ from 'lodash'
import { AppBar, Toolbar, createStyles, makeStyles, Theme, IconButton, Menu, MenuItem, Typography, Box, Button } from '@material-ui/core'
import { AccountCircle } from '@material-ui/icons'
import AppContext from 'context'

const styles = (theme: Theme) =>
    createStyles({
        menuButton: {
            marginRight: theme.spacing(2)
        },
        title: {
            flexGrow: 1
        },
        logoBox: {
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            border: '5px solid',
            borderColor: theme.palette.common.white,
            padding: theme.spacing(0.5),
            margin: theme.spacing(1)
        },
        logoTitle: {
            letterSpacing: '0.2em',
            marginRight: '-0.2em',
            fontWeight: 500,
            lineHeight: 1
        },
        logoSubtitle: {
            lineHeight: 1,
            fontFamily: 'Roboto Condensed'
        },
        button: {
            color: 'inherit'
        }
    })

const useStyles = makeStyles(styles)

const Header: React.FC<RouteComponentProps> = (props) => {

    const [serviceSectionsEl, setServiceSectionsEl] = React.useState<null | HTMLElement>(null)
    const [priceListsEl, setPriceListsEl] = React.useState<null | HTMLElement>(null)

    const classes = useStyles()

    const context = useContext(AppContext)

    if (!context.user.id) {
        return <div />
    }

    const accessRights = context.user.accessRights

    const serviceSections = {
        ...(accessRights.userRoles.read && { userRoles: 'Роли' }),
        ...(accessRights.users.read && { users: 'Пользователи системы' }),
        ...(accessRights.categories.read && { categories: 'Структура каталога' }),
        ...(accessRights.properties.read && { productProperties: 'Характеристики товаров и их значения' }),
        ...(accessRights.processingRules.read && { processingRules: 'Правила обработки' })
    }

    const priceListsSections = {
        ...(accessRights.priceListAdd.full && { parsePriceList: 'Загрузка прайса' }),
        ...(accessRights.priceLists.read && { parsedPriceLists: 'Лента загрузок' }),
        ...(accessRights.commitedPriceLists.read && { commitedPriceLists: 'Утвержденные загрузки' })
    }

    const handleServiceSectionsClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setServiceSectionsEl(event.currentTarget)
    }

    const handlePriceListsClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setPriceListsEl(event.currentTarget)
    }

    const handleLoginClick = () => {
        props.history.push('/login')
    }

    const handleClose = (path?: string) => {
        setServiceSectionsEl(null)
        setPriceListsEl(null)
        if (path) {
            props.history.push(`/${path}`)
        }
    }

    return (
        <AppBar position="static">
            <Toolbar>
                <Box className={classes.logoBox}>
                    <Typography variant="h4" className={classes.logoTitle}>ГИМ</Typography>
                    <Typography variant="body2" className={classes.logoSubtitle}>ТОВАРЫ И ПОСТАВЩИКИ</Typography>
                </Box>
                {(accessRights.suppliers.read || accessRights.suppliers.readSelf) && (
                    <Button
                        className={classes.button}
                        size="large"
                        onClick={() => props.history.push('/suppliers')}
                    >
                        Поставщики
                    </Button>
                )}
                {!_.isEmpty(priceListsSections) && (
                    <React.Fragment>
                        <Button
                            className={classes.button}
                            size="large"
                            onClick={handlePriceListsClick}
                            onMouseEnter={handlePriceListsClick}
                        >
                            Прайсы
                </Button>
                        <Menu
                            anchorEl={priceListsEl}
                            open={Boolean(priceListsEl)}
                            onClose={() => handleClose()}
                            getContentAnchorEl={null}
                            anchorOrigin={{
                                vertical: 'bottom',
                                horizontal: 'left'
                            }}
                            transformOrigin={{
                                vertical: 'top',
                                horizontal: 'left',
                            }}
                            elevation={2}
                        >
                            {Object.keys(priceListsSections).map(key => <MenuItem key={key} onClick={() => handleClose(key)}>{priceListsSections[key]}</MenuItem>)}
                        </Menu>
                    </React.Fragment>
                )}
                {accessRights.products.read && (
                    <Button
                        className={classes.button}
                        size="large"
                        onClick={() => props.history.push('/products')}
                    >
                        Номенклатура
                    </Button>
                )}
                {!_.isEmpty(serviceSections) && (
                    <React.Fragment>
                        <Button
                            className={classes.button}
                            size="large"
                            onClick={handleServiceSectionsClick}
                            onMouseEnter={handleServiceSectionsClick}
                        >
                            Служебные разделы
                    </Button>
                        <Menu
                            anchorEl={serviceSectionsEl}
                            open={Boolean(serviceSectionsEl)}
                            onClose={() => handleClose()}
                            getContentAnchorEl={null}
                            anchorOrigin={{
                                vertical: 'bottom',
                                horizontal: 'left'
                            }}
                            transformOrigin={{
                                vertical: 'top',
                                horizontal: 'left',
                            }}
                            elevation={2}
                        >
                            {Object.keys(serviceSections).map(key => <MenuItem key={key} onClick={() => handleClose(key)}>{serviceSections[key]}</MenuItem>)}
                        </Menu>
                    </React.Fragment>
                )}
                <Typography variant="h6" className={classes.title}>
                    {/* {sections[section]} */}
                </Typography>
                <Typography variant="h6">
                    {context.user.email}
                </Typography>
                <IconButton
                    onClick={handleLoginClick}
                    color="inherit"
                >
                    <AccountCircle />
                </IconButton>
            </Toolbar>
        </AppBar >
    )
}

export default withRouter(Header)