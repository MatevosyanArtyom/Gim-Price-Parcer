import React from 'react'
import CategoriesTreeItem from './CategoriesTreeItem'
import { CategoriesTreeProps } from './types'

const CategoriesTree: React.FC<CategoriesTreeProps> = (props) => {
    const {
        categories,
        categoryId,
        depth,
        expanded,
        loadCategories,
        onExpand,
        onCategoryClick,
        parentId
    } = props

    return (
        <React.Fragment>
            {categories.filter(x => x.parent === parentId).map(category => (
                <CategoriesTreeItem
                    key={category.id}
                    categories={categories}
                    category={category}
                    categoryId={categoryId}
                    depth={depth}
                    expanded={expanded}
                    loadCategories={loadCategories}
                    onExpand={onExpand}
                    onCategoryClick={onCategoryClick}
                    parentId={category.id}
                />
            ))}
        </React.Fragment>
    )
}

export default CategoriesTree