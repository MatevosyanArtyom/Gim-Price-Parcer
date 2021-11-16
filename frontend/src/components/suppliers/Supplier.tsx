import React, { useEffect, useCallback, useMemo, useContext } from 'react'
import { withRouter } from 'react-router'
import { Field, Formik, FormikProps } from 'formik'
import { TextField, CheckboxWithLabel } from 'formik-material-ui'
import moment from 'moment'
import { Grid, Typography, MenuItem } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import FormWrapper from 'components/common/GimForm/FormWrapper'
import DatePickerField from 'components/common/GimForm/DatePickerField'
import ButtonsBlock from 'components/common/GimForm/ButtonsBlock'
import ButtonCancel from 'components/common/GimForm/ButtonCancel'
import ButtonSubmit from 'components/common/GimForm/ButtonSubmit'
import ButtonFromArchive from 'components/common/GimForm/ButtonFromArchive'
import { EntityStatuses } from 'components/common/types'
import DadataClient, { DadataSuggestion } from 'util/dadataClient'
import promiseWithHttpResponse from 'util/promiseWithHttpResponse'
import { RouteComponentPropsWithId } from 'util/types'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import ContactPersons from './ContactPersons'
import useStyles from './styles'
import { FieldsTranslations } from './types'
import DadataSelect from './DadataSelect'

const n = nameofFactory<client.SupplierEdit>()
const nB = nameofFactory<client.BankDetails>()

export const initialSupplier: client.SupplierEdit = {
    id: '',
    seqId: 0,
    createdDate: moment().format(),
    name: '',
    region: {
        fiasId: '',
        value: ''
    },
    city: {
        fiasId: '',
        value: ''
    },
    email: '',
    phoneNumber: '',
    legalAddress: '',
    officeAddress: '',
    inn: '',
    bankDetails: {
        account: '',
        correspondentAccount: '',
        rcbic: ''
    },
    largeWholesale: false,
    smallWholesale: false,
    retail: false,
    installment: false,
    credit: false,
    deposit: false,
    transferForSale: false,
    hasShowroom: false,
    workWithIndividuals: false,
    dropshipping: false,
    minimumPurchase: 0,
    paidPartnership: false,
    contactPersons: [],
    status: 'New',
    user: ''
}

const initialVersions: client.EntityVersionDtoOfSupplierEdit[] = []

const initialState = {
    supplier: initialSupplier,
    versions: initialVersions,
    isLoading: false
}

const Supplier: React.FC<RouteComponentPropsWithId> = (props) => {

    const id = props.match.params.id === 'add' ? '' : props.match.params.id

    const classes = useStyles()

    let [{ supplier, isLoading }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)

    const noEditRights = supplier.status !== 'New' && !context.user.accessRights.suppliers.full
    const readOnly = supplier.isArchived || noEditRights

    const init = useCallback(() => {
        setState({ isLoading: true })

        let promises: [
            Promise<client.HttpResponse<client.SupplierEdit>>
        ] = [
                id ? client.api.suppliersGetOne(id) : promiseWithHttpResponse(initialSupplier),
            ]

        Promise.all(promises).then(results => {
            setState({
                supplier: results[0].data,
                isLoading: false
            })
        })
    }, [id, setState])

    useEffect(init, [init])

    const dadata = useMemo(() => new DadataClient(), [])

    const mapDadataSuggestionToFiasEntity = useCallback((value: DadataSuggestion): client.FiasEntity => ({
        data: JSON.stringify(value.data),
        fiasId: value.data.fias_id,
        unrestrictedValue: value.unrestricted_value,
        value: value.value,
    }), [])

    const formContent = (renderProps: FormikProps<client.SupplierEdit>) => {
        const values = renderProps.values

        const loadRegions = (inputValue: string, callback: ((options: client.FiasEntity[]) => void)) => {
            dadata.region(inputValue).then(v => {
                callback(v.data.suggestions.map(mapDadataSuggestionToFiasEntity))
            })
        }

        const loadCities = (inputValue: string, callback: ((options: client.FiasEntity[]) => void)) => {
            dadata.city(inputValue, values.region?.fiasId || '').then(v => {
                callback(v.data.suggestions.map(mapDadataSuggestionToFiasEntity))
            })
        }

        return (
            <FormWrapper
                isLoading={isLoading}
            >
                <Grid container>
                    <Grid item xs={12} className={classes.gridItem}>
                        <Typography variant="h4" gutterBottom>{values.name}</Typography>
                    </Grid>
                    <Grid item md={5} xs={12} className={classes.gridItem}>
                        <Typography variant="h5" gutterBottom>Базовое описание</Typography>
                        <Field name={n('seqId')} label="ID" component={TextField} margin="normal" fullWidth disabled />
                        <DatePickerField
                            name={n('createdDate')}
                            label={FieldsTranslations.createdDate}
                            format="DD.MM.YYYY"
                            margin="normal"
                            value={values.createdDate}
                            fullWidth
                            disabled
                        />
                        <Field
                            name={n('name')}
                            label={FieldsTranslations.name}
                            component={TextField}
                            margin="normal"
                            error={renderProps.errors.name}
                            validate={validations.required}
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                            required
                        />
                        <DadataSelect
                            loadOptions={loadRegions}
                            TextFieldProps={{
                                label: 'Регион',
                                InputLabelProps: {
                                    ...(renderProps.values.region!.value && { shrink: true }) // hack. bug?
                                }
                            }}
                            value={renderProps.values.region}
                            onChange={value => {
                                renderProps.setFieldValue('region', value)
                            }}
                            isDisabled={readOnly}
                        />
                        <DadataSelect
                            loadOptions={loadCities}
                            TextFieldProps={{
                                label: 'Город',
                                InputLabelProps: {
                                    ...(renderProps.values.region!.value && { shrink: true }) // hack. bug?
                                }
                            }}
                            value={renderProps.values.city}
                            onChange={value => {
                                renderProps.setFieldValue('city', value)
                                const region = renderProps.values.region
                                if (!region || !region.fiasId) {
                                    const data = JSON.parse((value as client.FiasEntity).data || '')
                                    renderProps.setFieldValue('region', {
                                        ...value,
                                        fiasId: data.region_fias_id,
                                        unrestrictedValue: data.region_with_type,
                                        value: data.region_with_type,
                                    })
                                }
                            }}
                            isDisabled={readOnly}
                        />
                        <Field
                            name={n('email')}
                            label={FieldsTranslations.email}
                            component={TextField}
                            margin="normal"
                            error={renderProps.errors.email}
                            validate={validations.email}
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                        <Field
                            name={n('phoneNumber')}
                            label={FieldsTranslations.phoneNumber}
                            component={TextField}
                            margin="normal"
                            error={renderProps.errors.phoneNumber}
                            validate={validations.phoneNumber}
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                    </Grid>
                    <Grid item md={2} />
                    <Grid item md={5} xs={12} className={classes.gridItem}>
                        <Typography variant="h5" gutterBottom>Адреса и реквизиты</Typography>
                        <Field
                            name={n('legalAddress')}
                            label={FieldsTranslations.legalAddress}
                            component={TextField}
                            margin="normal"
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                        <Field
                            name={n('officeAddress')}
                            label={FieldsTranslations.officeAddress}
                            component={TextField}
                            margin="normal"
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                        <Field
                            name={n('inn')}
                            label={FieldsTranslations.inn}
                            component={TextField}
                            margin="normal"
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                        <Field
                            name={`${n('bankDetails')}.${nB('rcbic')}`}
                            label={FieldsTranslations.bankDetails.rcbic}
                            component={TextField}
                            margin="normal"
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                        <Field
                            name={`${n('bankDetails')}.${nB('correspondentAccount')}`}
                            label={FieldsTranslations.bankDetails.correspondentAccount}
                            component={TextField}
                            margin="normal"
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                        <Field
                            name={`${n('bankDetails')}.${nB('account')}`}
                            label={FieldsTranslations.bankDetails.account}
                            component={TextField}
                            margin="normal"
                            InputProps={{ readOnly: readOnly }}
                            fullWidth
                        />
                    </Grid>
                    <Grid item xs={12} lg={6} className={classes.gridItem}>
                        <Grid container item xs={12}>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('largeWholesale')} Label={{ label: FieldsTranslations.largeWholesale }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('installment')} Label={{ label: FieldsTranslations.installment }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('transferForSale')} Label={{ label: FieldsTranslations.transferForSale }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                        </Grid>
                        <Grid container item xs={12}>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('smallWholesale')} Label={{ label: FieldsTranslations.smallWholesale }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('credit')} Label={{ label: FieldsTranslations.credit }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('hasShowroom')} Label={{ label: FieldsTranslations.hasShowroom }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                        </Grid>
                        <Grid container item xs={12}>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('retail')} Label={{ label: FieldsTranslations.retail }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('deposit')} Label={{ label: FieldsTranslations.deposit }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                            <Grid item xs={12} lg={4}>
                                <Field name={n('workWithIndividuals')} Label={{ label: FieldsTranslations.workWithIndividuals }} component={CheckboxWithLabel} disabled={readOnly} fullWidth />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item xs={12}>
                        <ContactPersons renderProps={renderProps} />
                    </Grid>
                    <Grid item xs={12} className={classes.gridItem}>
                        <Typography variant="h5" className={classes.h5} gutterBottom>Статус</Typography>
                        <Field
                            name={n('status')}
                            label={FieldsTranslations.status}
                            component={TextField}
                            margin="normal"
                            InputProps={{
                                readOnly: !id || readOnly
                            }}
                            disabled={!context.user.accessRights.suppliers.full}
                            select
                            fullWidth
                        >
                            {Object.keys(EntityStatuses).map(key => (
                                <MenuItem
                                    key={key}
                                    value={key}
                                    disabled={key === 'New'}
                                >
                                    {EntityStatuses[key]}
                                </MenuItem>
                            ))}
                        </Field>
                    </Grid>
                </Grid>
                <ButtonsBlock align="right">
                    <ButtonCancel disabled={renderProps.isSubmitting} />
                    {!values.isArchived && <ButtonSubmit disabled={renderProps.isSubmitting || readOnly} />}
                    {
                        values.isArchived &&
                        <ButtonFromArchive
                            disabled={renderProps.isSubmitting || noEditRights}
                            onClick={() => {
                                client.api.suppliersFromArchiveOne(values.id).then(v => init())
                            }}
                        />
                    }
                </ButtonsBlock>
            </FormWrapper >
        )
    }

    return (
        <Formik
            initialValues={supplier}
            onSubmit={(values) => {
                if (id) {
                    client.api.suppliersUpdateOne(values).then(() => props.history.goBack())
                } else {
                    client.api.suppliersAddOne(values).then(() => props.history.goBack())
                }
            }}
            enableReinitialize={true}
        >
            {props => formContent(props)}
        </Formik >
    )
}

export default withRouter(Supplier)