import { FormikConfig, FormikProps } from 'formik'

export type FormWrapperProps = {
    isLoading?: boolean
}

export type GimFormProps<T> = FormikConfig<T> & {
    buttons?: any[]
    defaultButtons?: boolean
    fields?: (props: FormikProps<T>) => any[]
    isLoading?: boolean
    readonly?: boolean
}