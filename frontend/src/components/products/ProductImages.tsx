import React, { useEffect } from 'react'
import clsx from 'clsx'
import { Paper, Typography, Grid } from '@material-ui/core'
import * as client from 'client'
import GimCircularProgress from 'components/common/GimCircularProgress'
import useMergeState from 'util/useMergeState'
import useStyles from './styles'

type Props = {
    id: string
}

type State = {
    images: client.ImageFullDto[]
    isLoading: boolean
}

const initialState: State = {
    images: [],
    isLoading: false
}

const ProductImages: React.FC<Props> = ({ id }) => {

    const classes = useStyles()

    const [{ images, isLoading }, setState] = useMergeState(initialState)

    useEffect(() => {
        setState({ isLoading: true })
        client.api.imagesGetMany({
            ProductId: id,
            Status: 'DownloadSuccess',
            page: 0,
            pageSize: 9
        }, {}).then(v => {
            setState({ images: v.data.entities, isLoading: false })
        })
    }, [id, setState])

    const mainImage = images && (images.find(x => x.isMain) || images[0])
    return (
        <Paper className={clsx(classes.paper, classes.positionRelative)}>
            <GimCircularProgress isLoading={isLoading} />
            <Typography variant="h5" gutterBottom>Главное изображение</Typography>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    {mainImage && mainImage.status === 'DownloadSuccess'
                        ? <img src={`data:image/png;base64,${mainImage.data}`} alt={mainImage.id} style={{ maxWidth: '100%' }} />
                        : <Typography variant="body2">Нет изображений</Typography>
                    }
                </Grid>
                {images.filter(image => image.status === 'DownloadSuccess').map(image => (
                    <Grid item key={image.id} xs={4}>
                        <img src={`data:image/png;base64,${image.data}`} alt={image.id} style={{ maxWidth: '100%' }} />
                    </Grid>
                ))}

            </Grid>
        </Paper >
    )
}

export default ProductImages