import { ArchivedFilter } from 'components/common/ShowArchivedToggler'

export type StateType = {
    archivedFilter: ArchivedFilter,
    isLoading: boolean
}

export const FieldsTranslations = {
    id: 'ID',
    seqId: 'ID',
    createdDate: 'Дата создания',
    name: 'Наименование',
    city: 'Город',
    region: 'Область',
    email: 'E-mail',
    phoneNumber: 'Номер телефона',
    legalAddress: 'Юридический адрес',
    officeAddress: 'Фактический адрес',
    inn: 'ИНН',
    bankDetails: {
        account: 'Номер счета',
        correspondentAccount: 'Корр. счет',
        rcbic: 'БИК'
    },
    largeWholesale: 'Крупный опт',
    smallWholesale: 'Мелкий опт',
    retail: 'Поштучно',
    installment: 'Возможна рассрочка',
    credit: 'Кредит',
    deposit: 'Возможен депозит',
    transferForSale: 'Передача на реализацию',
    hasShowroom: 'Есть шоурум',
    workWithIndividuals: 'Работает с ФЛ',
    dropshipping: '',
    minimumPurchase: '',
    paidPartnership: '',
    contactPersons: '',
    status: 'Статус',
}