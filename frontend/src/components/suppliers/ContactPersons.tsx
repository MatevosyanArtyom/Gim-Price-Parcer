import React, { useRef } from 'react'
import { FormikProps } from 'formik'
import MaterialTable, { EditComponentProps } from 'material-table'
import { Done as DoneIcon } from '@material-ui/icons'
import { Checkbox, Paper, Typography } from '@material-ui/core'
import * as client from 'client'
import { MaterialTableLocalization } from 'components/common/GimTable/utils'
import { nameofFactory } from 'util/utils'
import useStyles from './styles'
import ButtonAdd from 'components/common/GimTable/ButtonAdd'

const nC = nameofFactory<client.ContactPersonDto>()

type ContactPersonsProps = {
    renderProps: FormikProps<client.SupplierEdit>
}

const EditComponent = ({ value, onChange }: EditComponentProps<client.ContactPersonDto>) => (
    <Checkbox
        checked={value}
        onChange={() => onChange(!value)}
    />
)

const CheckboxRender = (data: client.ContactPersonDto, field: (data: client.ContactPersonDto) => boolean | undefined) => (
    <div style={{ display: 'flex', alignItems: 'center' }}>
        {field(data) ? <DoneIcon /> : <div />}
    </div>
)

const ContactPersons: React.FC<ContactPersonsProps> = ({ renderProps }) => {

    const ref = useRef<any>(null)

    const classes = useStyles()

    let { values } = renderProps
    return (
        <Paper className={classes.paper}>
            <Typography variant="h5" className={classes.h5СontactPersons} gutterBottom>Контактные лица</Typography>
            <ButtonAdd
                onClick={() => {
                    ref.current && ref.current.setState({ showAddRow: true })
                }}
                disabled={values.isArchived}
            />
            <MaterialTable
                tableRef={ref}
                title="Контактные лица"
                columns={[
                    { title: 'ФИО', field: nC('name') },
                    { title: 'E-mail', field: nC('email') },
                    { title: 'Номер телефона', field: nC('phoneNumber') },
                    { title: 'Skype', field: nC('skype') },
                    {
                        title: 'Telegram',
                        field: nC('hasTelegram'),
                        editComponent: EditComponent,
                        render: (data: client.ContactPersonDto) => CheckboxRender(data, x => x.hasTelegram)
                    },
                    {
                        title: 'Viber',
                        field: nC('hasViber'),
                        editComponent: EditComponent,
                        render: (data: client.ContactPersonDto) => CheckboxRender(data, x => x.hasViber)
                    },
                    {
                        title: 'WhatsApp',
                        field: nC('hasWhatsApp'),
                        editComponent: EditComponent,
                        render: (data: client.ContactPersonDto) => CheckboxRender(data, x => x.hasWhatsApp)
                    },
                    { title: 'Доступность', field: nC('availability') },
                ]}
                data={values.contactPersons}
                editable={{
                    onRowAdd: (newData) => {
                        return new Promise(resolve => {
                            values.contactPersons.push(newData)
                            renderProps.setValues(values)
                            resolve()
                        })
                    },
                    onRowUpdate: (newData, oldData) => {
                        return new Promise(resolve => {
                            if (oldData) {
                                const index = values.contactPersons.indexOf(oldData)
                                values.contactPersons[index] = newData
                                renderProps.setValues(values)
                            }
                            resolve()
                        })
                    },
                    onRowDelete: (oldData: client.ContactPersonDto) => {
                        return new Promise(resolve => {
                            const index = values.contactPersons.indexOf(oldData)
                            values.contactPersons.splice(index, 1)
                            renderProps.setValues(values)
                            resolve()
                        })
                    },
                    isEditable: () => !values.isArchived,
                    isDeletable: () => !values.isArchived
                }}
                components={{
                    Container: props => <Paper {...props} elevation={0} />
                }}
                localization={MaterialTableLocalization}
                options={{
                    actionsCellStyle: { minWidth: '96px' },
                    search: false,
                    toolbar: false
                }}
            />
        </Paper>
    )
}

export default ContactPersons