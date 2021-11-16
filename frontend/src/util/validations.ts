export const required = (value: any) => value ? undefined : 'Поле обязательно для заполнения'

export const password = (value: string) => {

    if (required(value)) {
        return required(value)
    }

    if (value.length < 8) {
        return 'Пароль должен содержать минимум 8 символов'
    }

    if (value.length > 24) {
        return 'Максимальная длина пароля 24 символа'
    }

    if (!/[0-9]/g.test(value)) {
        return 'Пароль должен содержать хотя бы одну цифру'
    }

    if (!/[a-zA-Z]/g.test(value)) {
        return 'Пароль должен содержать хотя бы один символ латинского алфавита'
    }

    return undefined
}

export const email = (value: string) => {
    if (value && !/^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[\w!#$%&'*+/=?`{|}~^-]+)*@(?:[A-Z0-9-]+\.)+[A-Z]{2,6}$/i.test(value)) {
        return 'Неверный формат e-mail'
    }
    return undefined
}

export const phoneNumber = (value: string) => {
    if (value && !/^(?=.*[0-9])[+\- ()0-9]+$/i.test(value)) {
        return 'Номер телефона может содержать только цифры и знаки +-()'
    }
    return undefined
}

export const requiredEmail = (value: string) => {
    if (required(value)) {
        return required(value)
    }

    if (email(value)) {
        return email(value)
    }

    return undefined
}