import { Action, Localization } from 'material-table'
import { ArchivedFilter } from 'components/common/ShowArchivedToggler'
import { ParamsId } from 'util/types'

export const MaterialTableLocalization: Localization = {
    header: { actions: 'Действия', },
    body: {
        addTooltip: 'Добавить',
        deleteTooltip: 'Удалить',
        editRow: {
            cancelTooltip: 'Отмена',
            deleteText: 'Удалить запись?',
            saveTooltip: 'Подтвердить'
        },
        emptyDataSourceMessage: 'Нет данных для отображения'
    },
}

export const getActions = <T extends ParamsId>(params: {
    archivedFilter: ArchivedFilter,
    editOpenClick: (event: any, data: any) => any, // data: T | T[]
    toArchive: {
        onClick: (event: any, data: any) => any,
        disabled?: (rowData: T) => boolean
    },
    formArchive: {
        onClick: (event: any, data: any) => any,
        disabled?: (rowData: T) => boolean
    }
}) => {
    const { archivedFilter, editOpenClick, toArchive, formArchive } = params
    const actions: (Action<T> | ((rowData: T) => Action<T>))[] = [
        {
            icon: archivedFilter === 'OnlyActive' ? 'edit' : 'visibility',
            tooltip: archivedFilter === 'OnlyActive' ? 'Редактировать' : 'Посмотреть',
            onClick: editOpenClick
        }
    ]
    if (archivedFilter === 'OnlyActive') {
        actions.push((rowData) => ({
            icon: 'archive',
            tooltip: 'В архив',
            onClick: toArchive.onClick,
            disabled: toArchive.disabled && toArchive.disabled(rowData)
        }))
    }

    if (archivedFilter === 'OnlyArchived') {
        actions.push((rowData) => ({
            icon: 'unarchive',
            tooltip: 'Восстановить',
            onClick: formArchive.onClick,
            disabled: formArchive.disabled && formArchive.disabled(rowData)
        }))
    }
    return actions
}