import * as client from 'client'

export type DragObjectType = {
    type: string
    mode: 'move' | 'newParent'
    depth: number
    category: client.CategoryLookup
    beginIndex?: number
    endIndex?: number
}
export type ExpandedType = { [key: string]: boolean }
export type OnExpandType = (сategoryId: string) => void
export type DropZoneProps = {
    updateTree: (from: string, to: string) => void
}

export type CategoryEditState = {
    isEditing: boolean
    categoryId?: string // заполнено, когда категория редактируется
    parentId?: string // заполнено, когда категория добавляется
}

type CategoryItemBaseProps = {
    depth: number
    onCancelClick: () => void
    onSaveClick: (item: client.CategoryEdit) => void
    editState: CategoryEditState
    mergeState: CategoriesMergeStateType
}

export type CategoriesTreeBaseProps = {
    categoryId: string // текущая "выбранная" категория
    // parentId: string // ID родительской категории для части дерева
    expanded: ExpandedType // идентификаторы "открытых" категорий
    onAddClick: (parentId: string) => void // при нажатии на кнопку добавления категории в строке
    onDeleteClick: (id: string) => void // при нажатии на кнопку удаления категории в строке
    onEditClick: (id: string) => void // при нажатии на кнопку редактирования категории в строке
    // onMoveEnd: (id: string, afterId: string, newParentId: string) => void // при окончании перемещения категории
    onMapClick: (category: client.CategoryLookup) => void //при нажатии на кнопку редактирования маппингов в строке
    onMergeClick: (category: client.CategoryLookup, depth: number) => void // при нажатии на кнопку объединения категорий в строке
    onExpand: OnExpandType // при разворачивании категории
    // onItemClick: (c: string) => void // при клике на категорию
    onDragHover: (srcIndex: number, dstIndex: number) => void // при перетаскивании категории
    updateTree: () => void // обновить дерево с сервера
    readOnly: boolean
} & CategoryItemBaseProps
export type CategoriesTreeProps = {
    categories: client.CategoryLookup[]
    // categoriesDict: Dictionary<client.CategoryLookup>
} & CategoriesTreeBaseProps
export type CategoriesTreeItemProps = {
    // category: client.TreeItemOfCategoryLookup
    categories: client.CategoryLookup[]
    index: number
    // categoriesDict: Dictionary<client.CategoryLookup>
} & CategoriesTreeBaseProps

export type CategoryAddItemProps = {
    onAddClick: (parentId: string) => void
    parentId: string
    readOnly: boolean
} & CategoryItemBaseProps

export type CategoryEditItemProps = {
    category: client.CategoryEdit
} & CategoryItemBaseProps

export type CategoriesMergeStateType = {
    open: boolean
    category?: client.CategoryLookup // заполнено, когда категория объединяется
    depth?: number // заполнено, когда категория объединяется
}

export type StateType = {
    categories: client.CategoryLookup[] // текущий список категорий, отфильтрованный при необходимости
    // categoriesDict: Dictionary<client.CategoryLookup>
    // categoryId: string // текущая "выбранная" категория
    expanded: ExpandedType // идентификаторы "открытых" категорий
    filter: string // строка поиска
    scrollTo: string // идентификатор категории, к которой скроллить список после очистки фильтра
    editState: CategoryEditState
    mergeState: CategoriesMergeStateType
    isLoading: boolean
}

// export type CategoryLookupWithIndex = client.CategoryLookup & {
//     index: number
// }

export const initialEditState: CategoryEditState = {
    isEditing: false,
    parentId: '',
    categoryId: ''
}

export const initialMergeState: CategoriesMergeStateType = {
    open: false
}

export const initialState: StateType = {
    categories: [],
    expanded: {},
    filter: '',
    scrollTo: '',
    editState: initialEditState,
    mergeState: initialMergeState,
    isLoading: false
}