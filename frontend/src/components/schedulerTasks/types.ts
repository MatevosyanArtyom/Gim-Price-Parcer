export type IntegrationMethod = 'Unknown' | 'Api' | 'Email' | 'File'
export type StartBy = 'Unknown' | 'Email' | 'Schedule'
export type Status = 'Unknown' | 'Active' | 'Inactive' | 'New'

export const IntegrationMethods = {
    Api: 'API',
    Email: 'E-mail',
    File: 'Файл'
}

export const StartBys = {
    Email: 'По входящему почтовому сообщению',
    Schedule: 'По расписанию'
}

export const Statuses = {
    New: 'Новый',
    Active: 'Активен',
    Inactive: 'Не активен'
}