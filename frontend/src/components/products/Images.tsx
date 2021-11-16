import React, { useCallback, useRef, useContext } from 'react'
import { Column, Action } from 'material-table'
import { Paper, Typography, Grid, IconButton, Tooltip } from '@material-ui/core'
import { Star, StarBorder, CheckCircle, RadioButtonUnchecked } from '@material-ui/icons'
import * as client from 'client'
import AppContext from 'context'
import { GimDialogContext } from 'components/common/GimDialog'
import GimTable from 'components/common/GimTable'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import useStyles from './styles'

const n = nameofFactory<client.ImageFullDto>()

type Props = {
    id: string
}

type State = {
    image: client.ImageFullDto
    isLoading: boolean
}

const initialImage: client.ImageFullDto = {
    id: '',
    data: '',
    productId: '',
    size: 0
}

const initialState: State = {
    image: initialImage,
    isLoading: false
}

const Images: React.FC<Props> = ({ id }) => {

    const tableRef = useRef<any>(null)
    const classes = useStyles()
    const [{ image, isLoading }, setState] = useMergeState(initialState)

    const gimDialogState = useContext(GimDialogContext)

    const context = useContext(AppContext)

    const readOnly = !context.user.accessRights.products.full

    const columns: Column<client.ImageFullDto>[] = [
        {
            title: 'Изображение',
            field: n('data'),
            render: (rowData) => <img src={`data:image/png;base64,${rowData.data}`} alt={rowData.id} style={{ maxHeight: '100px', maxWidth: '180px' }} />,
            cellStyle: { padding: '4px', width: '180px', textAlign: 'center' },
        },
        { title: 'Имя файла', field: n('name') },
        {
            title: 'Главное',
            field: n('isMain'),
            render: (rowData) => (
                <Tooltip title={rowData.isMain ? '' : 'Сделать главным'}>
                    <IconButton
                        onClick={event => {
                            event.stopPropagation()

                            if (readOnly) {
                                return
                            }

                            setState({ isLoading: true })
                            client.api.imagesSetMain(rowData.id).then(v => {
                                tableRef.current && tableRef.current.onQueryChange()
                                setState({ isLoading: false })
                            })
                        }}
                    >
                        {rowData.isMain ? <Star /> : <StarBorder />}
                    </IconButton >
                </Tooltip>
            )
        },
        {
            title: 'Публикация',
            field: n('isPublished'),
            render: (rowData) => (
                <Tooltip title={rowData.isPublished ? 'Снять с публикации' : 'Опубликовать'}>
                    <IconButton
                        onClick={event => {
                            event.stopPropagation()

                            if (readOnly) {
                                return
                            }

                            setState({ isLoading: true })
                            client.api.imagesSetPublished(rowData.id, { isPublished: !rowData.isPublished }).then(v => {
                                tableRef.current && tableRef.current.onQueryChange()
                                setState({ isLoading: false })
                            })
                        }}
                    >
                        {rowData.isPublished ? <CheckCircle /> : <RadioButtonUnchecked />}
                    </IconButton >
                </Tooltip>
            )
        }
    ]

    const actions: Action<client.ImageFullDto>[] = [{
        icon: 'delete',
        onClick: (_event, data) => {
            const rowData = data as client.ImageFullDto
            gimDialogState.setState!({
                open: true,
                title: 'Удаление позиции',
                variant: 'Delete',
                onConfirm: () => {
                    client.api.imagesDeleteOne(rowData.id).then(v => {
                        gimDialogState.setState!({ open: false })
                        tableRef.current && tableRef.current.onQueryChange()
                    })
                }
            })
        }
    }]

    const onRowClick = useCallback((_event, rowData) => {
        rowData = rowData as client.ImageFullDto
        setState({ image: rowData })
    }, [setState])

    return (
        <Grid item xs={12}>
            <Paper className={classes.paper}>
                <Typography variant="h5" gutterBottom>Изображения</Typography>
                <Grid container spacing={2}>
                    <Grid item xs={4}>
                        {image.id && <img src={`data:image/png;base64,${image.data}`} alt={image.id} style={{ maxHeight: '100%', maxWidth: '100%' }} />}
                    </Grid>
                    <Grid item xs={8}>
                        <GimTable
                            ref={tableRef}
                            actions={actions}
                            columns={columns}
                            data={query => {
                                return new Promise(resolve => {
                                    id
                                        ? client.api.imagesGetMany({
                                            page: query.page,
                                            pageSize: query.pageSize,
                                            ProductId: id,
                                            Status: 'DownloadSuccess'
                                        }, {}).then(v => {
                                            resolve({ data: v.data.entities, page: query.page, totalCount: v.data.count })
                                        })
                                        : resolve({ data: [], page: 0, totalCount: 0 })
                                })
                            }}
                            onRowClick={onRowClick}
                            options={{
                                actionsColumnIndex: -1,
                                rowStyle: (rowData) => ({
                                    height: '108px',
                                    backgroundColor: image.id === rowData.id ? 'rgba(0, 0, 0, 0.07)' : undefined
                                })
                            }}
                            isLoading={isLoading}
                        />
                    </Grid>
                </Grid>
            </Paper>
        </Grid>
    )
}

export default Images