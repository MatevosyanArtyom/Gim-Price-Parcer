import React, { useEffect, useCallback, useContext } from 'react'
import { withRouter } from 'react-router'
import { Field, Formik, FormikProps } from 'formik'
import { TextField } from 'formik-material-ui'
import _ from 'lodash'
import moment from 'moment'
import { Grid, Typography } from '@material-ui/core'
import * as client from 'client'
import AppContext from 'context'
import promiseWithHttpResponse from 'util/promiseWithHttpResponse'
import { RouteComponentPropsWithId } from 'util/types'
import useMergeState from 'util/useMergeState'
import { nameofFactory } from 'util/utils'
import * as validations from 'util/validations'
import ButtonsBlock from 'components/common/GimForm/ButtonsBlock'
import ButtonCancel from 'components/common/GimForm/ButtonCancel'
import ButtonFromArchive from 'components/common/GimForm/ButtonFromArchive'
import ButtonSubmit from 'components/common/GimForm/ButtonSubmit'
import FormWrapper from 'components/common/GimForm/FormWrapper'
import AccessRightsCheckBox from './AccessRightsCheckbox'
import AccessRightsDivider from './AccessRightsDivider'
import AccessRightsGroup from './AccessRightsGroup'
import AccessRightsSection from './AccessRightsSection'
import connectedFlags from './connectedFlags'
import useStyles from './styles'
import { initialAccessRights } from './types'

const n = nameofFactory<client.UserRoleEdit>()
const nA = nameofFactory<client.AccessRightsDto>()
const nM = nameofFactory<client.AccessRightMode>()

const initialValues: client.UserRoleEdit = {
    id: '',
    seqId: 0,
    createdDate: moment().format(),
    name: '',
    accessRights: initialAccessRights
}

const initialState = {
    userRole: initialValues,
    isLoading: false
}

const UserRole: React.FC<RouteComponentPropsWithId> = (props) => {

    const id = props.match.params.id === 'add' ? '' : props.match.params.id

    const classes = useStyles()

    let [{ userRole, isLoading }, setState] = useMergeState(initialState)

    const context = useContext(AppContext)

    const readOnly = userRole.isArchived || !context.user.accessRights.userRoles.full

    const init = useCallback(() => {
        setState({ isLoading: true })

        let promises: [
            Promise<client.HttpResponse<client.UserRoleEdit>>,
        ] = [
                id ? client.api.userRolesGetOne(id) : promiseWithHttpResponse(initialValues),
            ]

        Promise.all(promises).then(results => {
            setState({
                userRole: results[0].data,
                isLoading: false
            })
        })
    }, [id, setState])

    useEffect(init, [init])

    const formContent = (renderProps: FormikProps<client.UserRoleEdit>) => {
        const values = renderProps.values

        const onCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
            const newValue = !_.get(values, e.target.name)
            renderProps.setFieldValue(e.target.name, newValue)
            if (newValue) {
                const leftFlags = _.get(connectedFlags, `${e.target.name}.leftFlags`) as unknown as string[]
                leftFlags.map(v => renderProps.setFieldValue(v, true))
            } else {
                const rightFlags = _.get(connectedFlags, `${e.target.name}.rightFlags`) as unknown as string[]
                rightFlags.map(v => renderProps.setFieldValue(v, false))
            }
        }

        return (
            <FormWrapper
                isLoading={isLoading}
            >
                <Grid container>
                    <Grid item xs={12} className={classes.gridItem}>
                        <Typography variant="h4" gutterBottom>{`Роль — ${values.name}`}</Typography>
                    </Grid>
                    <Grid container item xs={12} className={classes.gridItem}>
                        <Grid item xs={5}>
                            <Typography variant="h5" gutterBottom>Регистрационные данные</Typography>
                            <Field name={n('seqId')} label="ID" component={TextField} margin="normal" fullWidth disabled />
                            <Field
                                name={n('name')}
                                label="Наименование"
                                component={TextField}
                                margin="normal"
                                error={renderProps.errors.name}
                                validate={validations.required}
                                InputProps={{ readOnly: readOnly }}
                                fullWidth
                                required
                            />
                        </Grid>
                    </Grid>
                    <Grid container item xs={12} className={classes.gridItem}>
                        <Grid item xs={12}>
                            <Typography variant="h5" style={{ margin: '16px 0px' }}>Матрица прав роли</Typography>
                        </Grid>
                        <AccessRightsGroup header="Поставщики">
                            <AccessRightsSection />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('suppliers')}.${nM('readSelf')}`}
                                checked={values.accessRights.suppliers.readSelf}
                                onChange={onCheckboxChange}
                                primary="Просмотр своих"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('suppliers')}.${nM('editSelf')}`}
                                checked={values.accessRights.suppliers.editSelf}
                                onChange={onCheckboxChange}
                                primary="Создание и архивирование поставщиков"
                                secondary={'В рамках статуса "Новый"'}
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('suppliers')}.${nM('read')}`}
                                checked={values.accessRights.suppliers.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр полный"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('suppliers')}.${nM('full')}`}
                                checked={values.accessRights.suppliers.full}
                                onChange={onCheckboxChange}
                                primary="Полное управление"
                                secondary="Работа со статусами, отправка в архив"
                                readOnly={readOnly}
                            />
                        </AccessRightsGroup>
                        <AccessRightsGroup header="Прайсы">
                            <AccessRightsSection header="Загрузка прайсов" />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('priceListAdd')}.${nM('full')}`}
                                checked={values.accessRights.priceListAdd.full}
                                onChange={onCheckboxChange}
                                primary="Доступ к загрузке прайсов"
                                readOnly={readOnly}
                            />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <AccessRightsDivider />
                            <AccessRightsSection header="Лента загрузок" />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('priceLists')}.${nM('read')}`}
                                checked={values.accessRights.priceLists.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('priceLists')}.${nM('editSelf')}`}
                                checked={values.accessRights.priceLists.editSelf}
                                onChange={onCheckboxChange}
                                primary="Редактирование своих"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('priceLists')}.${nM('full')}`}
                                checked={values.accessRights.priceLists.full}
                                onChange={onCheckboxChange}
                                primary="Полное управление"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('priceLists')}.${nM('createProperties')}`}
                                checked={values.accessRights.priceLists.createProperties}
                                onChange={onCheckboxChange}
                                primary="Создание характеристик"
                                readOnly={readOnly}
                            />
                            <Grid item xs={2} />
                            <AccessRightsDivider />
                            <AccessRightsSection header="Утвержденные загрузки" />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('commitedPriceLists')}.${nM('read')}`}
                                checked={values.accessRights.commitedPriceLists.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр"
                                readOnly={readOnly}
                            />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                        </AccessRightsGroup>
                        <AccessRightsGroup header="Номенклатура">
                            <AccessRightsSection />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('products')}.${nM('read')}`}
                                checked={values.accessRights.products.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('products')}.${nM('full')}`}
                                checked={values.accessRights.products.full}
                                onChange={onCheckboxChange}
                                primary="Полное управление"
                                readOnly={readOnly}
                            />
                        </AccessRightsGroup>
                        <AccessRightsGroup header="Служебные разделы">
                            <AccessRightsSection header="Роли" />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('userRoles')}.${nM('read')}`}
                                checked={values.accessRights.userRoles.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('userRoles')}.${nM('full')}`}
                                checked={values.accessRights.userRoles.full}
                                onChange={onCheckboxChange}
                                primary="Полное управление"
                                readOnly={readOnly}
                            />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <AccessRightsSection header="Пользователи системы" />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('users')}.${nM('read')}`}
                                checked={values.accessRights.users.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('users')}.${nM('full')}`}
                                checked={values.accessRights.users.full}
                                onChange={onCheckboxChange}
                                primary="Полное управление"
                                readOnly={readOnly}
                            />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <AccessRightsSection header="Структура каталога" />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('categories')}.${nM('read')}`}
                                checked={values.accessRights.categories.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('categories')}.${nM('full')}`}
                                checked={values.accessRights.categories.full}
                                onChange={onCheckboxChange}
                                primary="Полное управление"
                                readOnly={readOnly}
                            />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <AccessRightsSection header="Хар-ки товаров и их значения" />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('properties')}.${nM('read')}`}
                                checked={values.accessRights.properties.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('properties')}.${nM('full')}`}
                                checked={values.accessRights.properties.full}
                                onChange={onCheckboxChange}
                                primary="Полное управление"
                                readOnly={readOnly}
                            />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <Grid item xs={2} />
                            <AccessRightsSection header="Правила обработки" />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('processingRules')}.${nM('read')}`}
                                checked={values.accessRights.processingRules.read}
                                onChange={onCheckboxChange}
                                primary="Просмотр"
                                readOnly={readOnly}
                            />
                            <AccessRightsCheckBox
                                name={`${n('accessRights')}.${nA('processingRules')}.${nM('full')}`}
                                checked={values.accessRights.processingRules.full}
                                onChange={onCheckboxChange}
                                primary="Полное управление"
                                readOnly={readOnly}
                            />
                        </AccessRightsGroup>
                    </Grid>
                    <Grid item xs={12} className={classes.gridItem}>
                        <ButtonsBlock align="left">
                            {!values.isArchived && <ButtonSubmit disabled={renderProps.isSubmitting || readOnly} />}
                            {
                                values.isArchived &&
                                <ButtonFromArchive
                                    disabled={renderProps.isSubmitting || readOnly}
                                    onClick={() => {
                                        client.api.userRolesFromArchiveOne(values.id).then(init)
                                    }}
                                />
                            }
                            <ButtonCancel disabled={renderProps.isSubmitting} />
                        </ButtonsBlock>
                    </Grid>
                </Grid>
            </FormWrapper>
        )
    }

    return (
        <Formik
            initialValues={userRole}
            onSubmit={(values) => {
                if (id) {
                    client.api.userRolesUpdateOne(values).then(() => props.history.goBack())
                } else {
                    client.api.userRolesAddOne(values).then(() => props.history.goBack())
                }
            }}
            enableReinitialize={true}
        >
            {props => formContent(props)}
        </Formik>
    )
}

export default withRouter(UserRole)