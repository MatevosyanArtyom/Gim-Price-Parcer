import * as client from 'client'
import { nameofFactory } from 'util/utils'

const n = nameofFactory<client.UserRoleEdit>()
const nA = nameofFactory<client.AccessRightsDto>()
const nM = nameofFactory<client.AccessRightMode>()

// матрица связанных прав доступа
// путь в объекте описывает право, которое сейчас редактируется
// leftFlags описывает права, которые должны установлены при установке редактируемого права (визуально, все что слева)
// например, устанавливаем право "Просмотр полный (read)" для поставщиков (suppliers): должны быть так же установлены флаги readSelf и editSelf
// rightFlags описывает права, которые должны быть сняты при снятии редактируемого права (визуально, все что права)
// например, снимаем право "Создание и архивирование поставщиков (editSelf)": должны быть так же сняты флаги read и full

const connectedFlags = {
    [n('accessRights')]: {
        [nA('suppliers')]: {
            [nM('readSelf')]: {
                leftFlags: [],
                rightFlags: [
                    `${n('accessRights')}.${nA('suppliers')}.${nM('editSelf')}`,
                    `${n('accessRights')}.${nA('suppliers')}.${nM('read')}`,
                    `${n('accessRights')}.${nA('suppliers')}.${nM('full')}`
                ],
            },
            [nM('editSelf')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('suppliers')}.${nM('readSelf')}`,
                ],
                rightFlags: [
                    `${n('accessRights')}.${nA('suppliers')}.${nM('read')}`,
                    `${n('accessRights')}.${nA('suppliers')}.${nM('full')}`
                ]
            },
            [nM('read')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('suppliers')}.${nM('readSelf')}`,
                    `${n('accessRights')}.${nA('suppliers')}.${nM('editSelf')}`
                ],
                rightFlags: [
                    `${n('accessRights')}.${nA('suppliers')}.${nM('full')}`
                ]
            },
            [nM('full')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('suppliers')}.${nM('readSelf')}`,
                    `${n('accessRights')}.${nA('suppliers')}.${nM('editSelf')}`,
                    `${n('accessRights')}.${nA('suppliers')}.${nM('read')}`
                ],
                rightFlags: []
            }
        },
        [nA('priceListAdd')]: {
            [nM('full')]: {
                leftFlags: [],
                rightFlags: [
                    // `${n('accessRights')}.${nA('priceLists')}.${nM('read')}`,
                    // `${n('accessRights')}.${nA('priceLists')}.${nM('editSelf')}`,
                    // `${n('accessRights')}.${nA('priceLists')}.${nM('full')}`,
                    // `${n('accessRights')}.${nA('priceLists')}.${nM('createProperties')}`
                ]
            }
        },
        [nA('priceLists')]: {
            [nM('read')]: {
                leftFlags: [
                    // `${n('accessRights')}.${nA('priceListAdd')}.${nM('full')}`
                ],
                rightFlags: [
                    `${n('accessRights')}.${nA('priceLists')}.${nM('editSelf')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('full')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('createProperties')}`
                ]
            },
            [nM('editSelf')]: {
                leftFlags: [
                    // `${n('accessRights')}.${nA('priceListAdd')}.${nM('full')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('read')}`

                ],
                rightFlags: [
                    `${n('accessRights')}.${nA('priceLists')}.${nM('full')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('createProperties')}`
                ]
            },
            [nM('full')]: {
                leftFlags: [
                    // `${n('accessRights')}.${nA('priceListAdd')}.${nM('full')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('read')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('editSelf')}`

                ],
                rightFlags: [
                    `${n('accessRights')}.${nA('priceLists')}.${nM('createProperties')}`
                ]
            },
            [nM('createProperties')]: {
                leftFlags: [
                    // `${n('accessRights')}.${nA('priceListAdd')}.${nM('full')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('read')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('editSelf')}`,
                    `${n('accessRights')}.${nA('priceLists')}.${nM('full')}`
                ],
                rightFlags: []
            }
        },
        [nA('commitedPriceLists')]: {
            [nM('read')]: {
                leftFlags: [],
                rightFlags: []
            }
        },
        [nA('products')]: {
            [nM('read')]: {
                leftFlags: [],
                rightFlags: [
                    `${n('accessRights')}.${nA('products')}.${nM('full')}`
                ]
            },
            [nM('full')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('products')}.${nM('read')}`
                ],
                rightFlags: []
            }
        },
        [nA('userRoles')]: {
            [nM('read')]: {
                leftFlags: [],
                rightFlags: [
                    `${n('accessRights')}.${nA('userRoles')}.${nM('full')}`
                ]
            },
            [nM('full')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('userRoles')}.${nM('read')}`
                ],
                rightFlags: []
            }
        },
        [nA('users')]: {
            [nM('read')]: {
                leftFlags: [],
                rightFlags: [
                    `${n('accessRights')}.${nA('users')}.${nM('full')}`
                ]
            },
            [nM('full')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('users')}.${nM('read')}`
                ],
                rightFlags: []
            }
        },
        [nA('categories')]: {
            [nM('read')]: {
                leftFlags: [],
                rightFlags: [
                    `${n('accessRights')}.${nA('categories')}.${nM('full')}`
                ]
            },
            [nM('full')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('categories')}.${nM('read')}`
                ],
                rightFlags: []
            }
        },
        [nA('properties')]: {
            [nM('read')]: {
                leftFlags: [],
                rightFlags: [
                    `${n('accessRights')}.${nA('properties')}.${nM('full')}`
                ]
            },
            [nM('full')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('properties')}.${nM('read')}`
                ],
                rightFlags: []
            }
        },
        [nA('processingRules')]: {
            [nM('read')]: {
                leftFlags: [],
                rightFlags: [
                    `${n('accessRights')}.${nA('processingRules')}.${nM('full')}`
                ]
            },
            [nM('full')]: {
                leftFlags: [
                    `${n('accessRights')}.${nA('processingRules')}.${nM('read')}`
                ],
                rightFlags: []
            }
        }
    }
}

export default connectedFlags