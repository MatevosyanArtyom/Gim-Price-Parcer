import * as client from 'client'
import { ExpandedType } from 'components/categories/types'

export type State = {
    categories: client.CategoryLookup[]
    categoryId: string // выбранная категория
    expanded: ExpandedType
    isLoading: boolean
}

export type ExpandMode = 'category' | 'property'
export type OnExpandType = (id: string, mode: ExpandMode) => void

export type CategoriesTreeProps = {
    categories: client.CategoryLookup[]
    categoryId: string // текущая "выбранная" категория
    depth: number
    expanded: ExpandedType
    loadCategories: () => void
    onCategoryClick: (categoryId: string) => void
    onExpand: OnExpandType
    parentId: string // идентификатор родительской категории
}

export type CategoriesTreeItemProps = {
    categories: client.CategoryLookup[]
    category: client.CategoryLookup // Текущая категория
    categoryId: string // выбранная категория
    depth: number
    loadCategories: () => void
    expanded: ExpandedType
    onExpand: OnExpandType
    onCategoryClick: (categoryId: string) => void
    parentId: string // идентификатор родительской категории
}

export type CategoryRowProps = {
    category: client.CategoryLookup,
    depth: number,
    expanded: boolean,
    onExpand: OnExpandType,
}




