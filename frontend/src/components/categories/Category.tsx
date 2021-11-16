import React, { useEffect } from 'react'
import { withRouter } from 'react-router'
import { Field } from 'formik'
import { TextField } from 'formik-material-ui'
import * as client from 'client'
import GimForm from 'components/common/GimForm'
import promiseWithHttpResponse from 'util/promiseWithHttpResponse'
import * as validations from 'util/validations'
import useMergeState from 'util/useMergeState'
import { RouteComponentPropsWithId } from 'util/types'
import { HttpResponse } from 'api'

export const initialCategory: client.CategoryEdit = {
    id: '',
    parent: '',
    name: '',
    description: '',
    status: 'New',
    mappings: [],
    productsCount: 0
}

const initialParents: client.CategoryLookup[] = []

const initialState = {
    category: initialCategory,
    parents: initialParents,
    isLoading: false
}

const Category: React.FC<RouteComponentPropsWithId> = (props) => {

    const id = props.match.params.id === 'add' ? '' : props.match.params.id

    let [{ category, isLoading }, setState] = useMergeState(initialState)

    useEffect(() => {
        setState({ isLoading: true })

        let promises: [
            Promise<HttpResponse<client.CategoryEdit>>,
        ] = [
                id ? client.api.categoriesGetOne(id) : promiseWithHttpResponse(initialCategory),
            ]

        Promise.all(promises).then(results => {
            setState({
                category: results[0].data,
                isLoading: false
            })
        })
    }, [id, setState])

    return (
        <GimForm
            initialValues={category}
            onSubmit={(values) => {
                setState({ isLoading: true })
                if (id) {
                    // TODO: client.api.CategoriesUpdateOne({ entity: values }, {}).then(() => props.history.goBack())
                } else {
                    // TODO: client.api.CategoriesAddOne({ entity: values }, {}).then(() => props.history.goBack())
                }
            }}
            fields={(props) => ([
                <Field key="id" name="id" label="ID" component={TextField} margin="normal" fullWidth disabled />,
                <Field
                    key="name"
                    name="name"
                    label="Наименование"
                    component={TextField}
                    margin="normal"
                    validate={validations.required}
                    fullWidth
                    required
                // TODO: error={props.errors.name}
                />,
                <Field key="description" name="description" label="Описание" component={TextField} margin="normal" fullWidth multiline rowsMax="5" />
            ])}
            isLoading={isLoading}
        />
    )
}

export default withRouter(Category)