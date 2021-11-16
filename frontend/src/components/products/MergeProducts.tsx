import React from 'react'
import { Paper, Toolbar, Typography, TextField, Table, TableHead, TableRow, TableCell, TableBody, Grid, Button } from '@material-ui/core'
import * as client from 'client'
import { Clear as ClearIcon } from '@material-ui/icons'
import GimIconButton from 'components/common/GimIconButton'
import useStyles from './styles'

type Props = {
    products: client.ProductLookup[]
    onMergeCancelClick: () => void
    onMergeRemoveClick: (id: string) => void
    onMergeSubmitClick: () => void
}

const MergeProducts: React.FC<Props> = ({ products, onMergeCancelClick, onMergeRemoveClick, onMergeSubmitClick }) => {

    const classes = useStyles()

    if (!products.length) {
        return <div></div>
    }

    const product = products[0]

    return (
        <Paper className={classes.paper}>
            <Toolbar disableGutters>
                <Typography variant="h5">Объединение позиций</Typography>
            </Toolbar>
            <TextField
                label="ID"
                value={product.seqId}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <TextField
                label="Наименование"
                value={product.name}
                InputProps={{ readOnly: true }}
                margin="dense"
                fullWidth
            />
            <Table size="small" padding="none" className={classes.margintop2}>
                <TableHead>
                    <TableRow>
                        <TableCell className={classes.tableCell}>ID</TableCell>
                        <TableCell className={classes.tableCell}>Наименование</TableCell>
                        <TableCell className={classes.tableCell} style={{ width: 36 }} />
                    </TableRow>
                </TableHead>
                <TableBody>
                    {products.map((v, i) => (
                        <TableRow>
                            <TableCell className={classes.tableCell}>{v.seqId}</TableCell>
                            <TableCell className={classes.tableCell}>{v.name}</TableCell>
                            <TableCell className={classes.tableCell} style={{ width: 36 }} >
                                {Boolean(i) && (
                                    <GimIconButton
                                        Icon={ClearIcon}
                                        onClick={() => onMergeRemoveClick(v.id)}
                                        tooltip="Убрать из объединяемых"
                                    />
                                )}
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
            <Grid container spacing={2} className={classes.margintop2}>
                <Grid item xs={6}>
                    <Button color="primary" variant="contained" onClick={onMergeSubmitClick} fullWidth>Сохранить</Button>
                </Grid>
                <Grid item xs={6}>
                    <Button onClick={onMergeCancelClick} fullWidth>Отмена</Button>
                </Grid>
            </Grid>
        </Paper>
    )
}

export default MergeProducts