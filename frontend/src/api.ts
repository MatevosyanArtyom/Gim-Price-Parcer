export interface LoginModel {
  email: string;
  password: string;
}

export type GimUserStatus = "Unknown" | "New" | "Active" | "Blocked" | "SystemBlocked";

export interface CheckTokenValidityModel {
  token: string;
  isTokenValid?: boolean;
  email?: string;
  status?: GimUserStatus;
}

export interface ChangePasswordModel {
  token: string;
  password: string;
}

export interface AccessRightMode {
  readSelf?: boolean;
  read?: boolean;
  editSelf?: boolean;
  edit?: boolean;
  full?: boolean;
  createProperties?: boolean;
}

export interface AccessRightsDto {
  suppliers: AccessRightMode;
  priceListAdd: AccessRightMode;
  priceLists: AccessRightMode;
  commitedPriceLists: AccessRightMode;
  products: AccessRightMode;
  userRoles: AccessRightMode;
  users: AccessRightMode;
  categories: AccessRightMode;
  properties: AccessRightMode;
  processingRules: AccessRightMode;
}

export interface UserLookup {
  email: string;
  fullname: string;
  position: string;
  roleId: string;
  phoneNumber?: string;
  hasSuppliersAccess?: boolean;
  hasFullAccess?: boolean;
  hasUsersAccess?: boolean;
  createdDate?: string;
  isArchived?: boolean;
  status?: GimUserStatus;
  id: string;
  seqId: number;
  accessRights: AccessRightsDto;
}

export type EntityStatus = "Unknown" | "New" | "Active" | "Inactive";

export interface CategoryMappingItem {
  name?: string;
  createdDate?: string;
}

export interface CategoryLookup {
  productsCount?: number;
  version?: string;
  modified?: string;
  status?: EntityStatus;
  mappings: CategoryMappingItem[];
  name: string;
  id: string;
  path: string;
  parent: string;
  rootParent: string;
  position: number;
  hasChildren?: boolean;
}

export interface GetAllResultDtoOfCategoryLookup {
  count: number;
  entities: CategoryLookup[];
}

export interface CategoryAdd {
  productsCount?: number;
  version?: string;
  modified?: string;
  status?: EntityStatus;
  mappings: CategoryMappingItem[];
  name: string;
  path?: string;
  parent?: string;
  description?: string;
  position?: number;
}

export interface CategoryEdit {
  productsCount?: number;
  version?: string;
  modified?: string;
  status?: EntityStatus;
  mappings: CategoryMappingItem[];
  name: string;
  path?: string;
  parent?: string;
  description?: string;
  position?: number;
  id: string;
}

export interface TreeItemOfCategoryLookup {
  item: CategoryLookup;
  children: TreeItemOfCategoryLookup[];
}

export interface GimFileAdd {
  name: string;
  size: number;
  data: string;
}

export interface UpdateParentModel {
  id: string;
  newParentId?: string;
}

export interface MergeCategoryModel {
  fromId: string;
  toId: string;
}

export interface MoveCategoryModel {
  afterId?: string;
}

export interface ProductPropertyLookup {
  category?: string;
  key?: string;
  name?: string;
  id: string;
  seqId: number;
}

export interface GetAllResultDtoOfProductPropertyLookup {
  count: number;
  entities: ProductPropertyLookup[];
}

export interface ProductPropertyAdd {
  category?: string;
  key?: string;
  name?: string;
}

export interface ProductPropertyEdit {
  category?: string;
  key?: string;
  name?: string;
  id: string;
  seqId: number;
}

export interface ProductPropertyValueLookup {
  property?: string;
  name?: string;
  id: string;
  seqId: number;
  propertyId: string;
}

export interface ProductPropertyValueAdd {
  property?: string;
  name?: string;
}

export interface ProductPropertyValueEdit {
  property?: string;
  name?: string;
  id: string;
  seqId: number;
}

export type GimImageDownloadStatus = "Unknown" | "NotDownloaded" | "Downloading" | "DownloadSuccess" | "DownloadFail";

export interface ImageFullDto {
  isMain?: boolean;
  isPublished?: boolean;
  productId: string;
  name?: string;
  url?: string;
  size: number;
  status?: GimImageDownloadStatus;
  id: string;
  data: string;
}

export interface GetAllResultDtoOfImageFullDto {
  count: number;
  entities: ImageFullDto[];
}

export interface ImageAddDto {
  isMain?: boolean;
  isPublished?: boolean;
  productId: string;
  name?: string;
  url?: string;
  size: number;
  status?: GimImageDownloadStatus;
}

export interface ImageLookupDto {
  isMain?: boolean;
  isPublished?: boolean;
  productId: string;
  name?: string;
  url?: string;
  size: number;
  status?: GimImageDownloadStatus;
  id: string;
}

export type SortDirection = "Asc" | "Desc";

export interface SupplierShort {
  id: string;
  seqId: number;
  name: string;
}

export interface GetAllResultDtoOfSupplierShort {
  count: number;
  entities: SupplierShort[];
}

export type ArchivedFilter = "Unknown" | "OnlyActive" | "OnlyArchived";

export type RulesSource = "Unknown" | "Code" | "File";

export interface ProcessingRuleLookup {
  name: string;
  supplier: string;
  rulesSource: RulesSource;
  code: string;
  id: string;
  seqId: number;
  isArchived?: boolean;
}

export interface GetAllResultDtoOfProcessingRuleLookup {
  count: number;
  entities: ProcessingRuleLookup[];
}

export interface UserRoleLookup {
  name: string;
  isMainAdmin?: boolean;
  createdDate?: string;
  isArchived?: boolean;
  accessRights: AccessRightsDto;
  id: string;
  seqId: number;
  usersCount?: number;
}

export interface GetAllResultDtoOfUserRoleLookup {
  count: number;
  entities: UserRoleLookup[];
}

export interface ManufacturerLookup {
  name: string;
  id: string;
}

export interface GetAllResultDtoOfManufacturerLookup {
  count: number;
  entities: ManufacturerLookup[];
}

export interface ManufacturerAdd {
  name: string;
  description?: string;
}

export interface ManufacturerEdit {
  name: string;
  description?: string;
  id: string;
}

export type PriceListItemStatus = "Unknown" | "Error" | "Fixed" | "Ok";

export type PriceListItemAction = "Unknown" | "CreateNew" | "LeaveOld" | "ApplyNew";

export interface PriceListItemProductPropertyLookup {
  id: string;
  seqId: number;
  propertyKey: string;
  property?: string;
  propertyId?: string;
  propertyValue?: string;
  valueId?: string;
  value?: string;
  productValue?: string;
  action: PriceListItemAction;
  status: PriceListItemStatus;
}

export interface GetAllResultDtoOfPriceListItemProductPropertyLookup {
  count: number;
  entities: PriceListItemProductPropertyLookup[];
}

export interface PriceListItemPropertySetActionModel {
  priceListId: string;
  propertyKey?: string;
  action: PriceListItemAction;
}

export type PriceListItemCategoryAction = "Unknown" | "MapTo";

export interface PriceListItemLookup {
  id: string;
  seqId: number;
  priceListId: string;
  code?: string;
  product?: string;
  productId?: string;
  productName?: string;
  hasSynonyms?: boolean;
  category1Name?: string;
  category1Id?: string;
  category1Status?: PriceListItemStatus;
  category1Action?: PriceListItemCategoryAction;
  mapTo1Id?: string;
  category2Name?: string;
  category2Id?: string;
  category2Status?: PriceListItemStatus;
  category2Action?: PriceListItemCategoryAction;
  mapTo2Id?: string;
  category3Name?: string;
  category3Id?: string;
  category3Status?: PriceListItemStatus;
  category3Action?: PriceListItemCategoryAction;
  mapTo3Id?: string;
  category4Name?: string;
  category4Id?: string;
  category4Status?: PriceListItemStatus;
  category4Action?: PriceListItemCategoryAction;
  mapTo4Id?: string;
  category5Name?: string;
  category5Id?: string;
  category5Status?: PriceListItemStatus;
  category5Action?: PriceListItemCategoryAction;
  mapTo5Id?: string;
  price1?: number;
  price2?: number;
  price3?: number;
  quantity?: number;
  description?: string;
  nameAction: PriceListItemAction;
  nameStatus: PriceListItemStatus;
  status: PriceListItemStatus;
  skip?: boolean;
}

export interface GetAllResultDtoOfPriceListItemLookup {
  count: number;
  entities: PriceListItemLookup[];
}

export interface ProductLookup {
  name: string;
  category1?: string;
  category2?: string;
  category3?: string;
  category4?: string;
  category5?: string;
  category: string;
  supplier: string;
  manufacturer: string;
  description?: string;
  priceFrom?: number;
  status?: EntityStatus;
  version?: string;
  id: string;
  seqId: number;
  supplierCount?: number;
  imageTotalCount?: number;
  imagePublishedCount?: number;
}

export interface ProductSynonymDto {
  product: ProductLookup;
  productId: string;
  score?: number;
}

export type PriceListStatus = "Unknown" | "InQueue" | "Processing" | "Errors" | "Ready" | "Committed" | "Failed";

export interface GimFileFull {
  name: string;
  size: number;
  id: string;
  data: string;
}

export interface PriceListLookup {
  supplier: string;
  processingRule: string;
  schedulerTask?: string;
  id: string;
  seqId: number;
  supplierId: string;
  priceListFile: GimFileFull;
  hasUnprocessedCodeErrors?: boolean;
  hasUnprocessedNameErrors?: boolean;
  hasUnprocessedPriceErrors?: boolean;
  hasUnprocessedErrors?: boolean;
  hasPropertiesErrors?: boolean;
  status: PriceListStatus;
  createdDate: string;
  author: string;
  authorId: string;
  statusDate?: string;
  parsedDate?: string;
}

export interface GetAllResultDtoOfPriceListLookup {
  count: number;
  entities: PriceListLookup[];
}

export interface PriceListAdd {
  supplier: string;
  processingRule: string;
  schedulerTask?: string;
  code?: string;
  priceListFile?: GimFileAdd;
}

export interface PriceListFull {
  supplier: string;
  processingRule: string;
  schedulerTask?: string;
  id: string;
  seqId: number;
  supplierId: string;
  processingRuleId: string;
  priceListFile: GimFileFull;
  hasUnprocessedCodeErrors?: boolean;
  hasUnprocessedNameErrors?: boolean;
  hasUnprocessedPriceErrors?: boolean;
  hasUnprocessedErrors?: boolean;
  hasPropertiesErrors?: boolean;
  createProperties?: boolean;
  status: PriceListStatus;
  createdDate: string;
  author: string;
  authorId: string;
  statusDate?: string;
  parsedDate?: string;
}

export type DiagnosticSeverity = "Hidden" | "Info" | "Warning" | "Error";

export interface DiagnosticDto {
  severity?: DiagnosticSeverity;
  message?: string;
}

export interface EmitResultDto {
  success?: boolean;
  diagnostics?: DiagnosticDto[];
}

export interface ProcessingRuleAdd {
  name: string;
  supplier: string;
  rulesSource: RulesSource;
  code: string;
}

export interface ProcessingRuleFull {
  name: string;
  supplier: string;
  rulesSource: RulesSource;
  code: string;
  id: string;
  seqId: number;
  isArchived?: boolean;
}

export interface CheckEmitPayload {
  rulesSource: RulesSource;
  script: string;
}

export interface GetAllResultDtoOfProductLookup {
  count: number;
  entities: ProductLookup[];
}

export interface ProductEdit {
  name: string;
  category1?: string;
  category2?: string;
  category3?: string;
  category4?: string;
  category5?: string;
  category: string;
  supplier: string;
  manufacturer: string;
  description?: string;
  priceFrom?: number;
  status?: EntityStatus;
  version?: string;
  id: string;
  seqId: number;
}

export interface ProductPropertiesModel {
  values?: ProductPropertyValueLookup[];
  allValues?: ProductPropertyValueLookup[];
}

export interface ProductAdd {
  name: string;
  category1?: string;
  category2?: string;
  category3?: string;
  category4?: string;
  category5?: string;
  category: string;
  supplier: string;
  manufacturer: string;
  description?: string;
  priceFrom?: number;
  status?: EntityStatus;
  version?: string;
}

export type IntegrationMethod = "Unknown" | "Api" | "Email" | "File";

export type SchedulerTaskStartBy = "Unknown" | "Email" | "Schedule";

export type SchedulerTaskStatus = "Unknown" | "Active" | "Inactive" | "New";

export interface SchedulerTaskLookup {
  name: string;
  supplier?: string;
  integrationMethod?: IntegrationMethod;
  requestRequired?: boolean;
  startBy?: SchedulerTaskStartBy;
  emails?: string;
  schedule?: string;
  script?: string;
  status?: SchedulerTaskStatus;
  version?: string;
  id: string;
  modified: string;
}

export interface GetAllResultDtoOfSchedulerTaskLookup {
  count: number;
  entities: SchedulerTaskLookup[];
}

export interface SchedulerTaskAdd {
  name: string;
  supplier?: string;
  integrationMethod?: IntegrationMethod;
  requestRequired?: boolean;
  startBy?: SchedulerTaskStartBy;
  emails?: string;
  schedule?: string;
  script?: string;
  status?: SchedulerTaskStatus;
  version?: string;
}

export interface SchedulerTaskFull {
  name: string;
  supplier?: string;
  integrationMethod?: IntegrationMethod;
  requestRequired?: boolean;
  startBy?: SchedulerTaskStartBy;
  emails?: string;
  schedule?: string;
  script?: string;
  status?: SchedulerTaskStatus;
  version?: string;
  id: string;
  modified: string;
}

export interface SchedulerTaskEdit {
  name: string;
  supplier?: string;
  integrationMethod?: IntegrationMethod;
  requestRequired?: boolean;
  startBy?: SchedulerTaskStartBy;
  emails?: string;
  schedule?: string;
  script?: string;
  status?: SchedulerTaskStatus;
  version?: string;
  id: string;
}

export interface SupplierProductLookup {
  supplier: string;
  supplierSeqId: number;
  product: string;
  code?: string;
  name?: string;
  price1?: number;
  price2?: number;
  price3?: number;
  quantity?: number;
  description?: string;
  version?: string;
  id: string;
}

export interface GetAllResultDtoOfSupplierProductLookup {
  count: number;
  entities: SupplierProductLookup[];
}

export interface SupplierProductAdd {
  supplier: string;
  supplierSeqId: number;
  product: string;
  code?: string;
  name?: string;
  price1?: number;
  price2?: number;
  price3?: number;
  quantity?: number;
  description?: string;
  version?: string;
}

export interface SupplierProductEdit {
  supplier: string;
  supplierSeqId: number;
  product: string;
  code?: string;
  name?: string;
  price1?: number;
  price2?: number;
  price3?: number;
  quantity?: number;
  description?: string;
  version?: string;
  id: string;
}

export interface SupplierLookup {
  name: string;
  inn?: string;
  version?: string;
  modified?: string;
  createdDate?: string;
  isArchived?: boolean;
  status?: EntityStatus;
  id: string;
  seqId: number;
  region?: string;
  city?: string;
  productsCount?: number;
  user: string;
}

export interface GetAllResultDtoOfSupplierLookup {
  count: number;
  entities: SupplierLookup[];
}

export interface FiasEntity {
  data?: string;
  fiasId?: string;
  value?: string;
  unrestrictedValue?: string;
}

export interface BankDetails {
  rcbic?: string;
  account?: string;
  correspondentAccount?: string;
}

export interface ContactPersonDto {
  name: string;
  email?: string;
  phoneNumber?: string;
  skype?: string;
  hasTelegram?: boolean;
  hasViber?: boolean;
  hasWhatsApp?: boolean;
  availability?: string;
}

export interface SupplierAdd {
  name: string;
  inn?: string;
  version?: string;
  modified?: string;
  createdDate?: string;
  isArchived?: boolean;
  status?: EntityStatus;
  region?: FiasEntity;
  city?: FiasEntity;
  email?: string;
  phoneNumber?: string;
  legalAddress?: string;
  officeAddress?: string;
  bankDetails?: BankDetails;
  contactPersons: ContactPersonDto[];
  largeWholesale?: boolean;
  smallWholesale?: boolean;
  retail?: boolean;
  installment?: boolean;
  credit?: boolean;
  deposit?: boolean;
  transferForSale?: boolean;
  hasShowroom?: boolean;
  workWithIndividuals?: boolean;
  paidPartnership?: boolean;
  dropshipping?: boolean;
  minimumPurchase?: number;
}

export interface SupplierEdit {
  name: string;
  inn?: string;
  version?: string;
  modified?: string;
  createdDate?: string;
  isArchived?: boolean;
  status?: EntityStatus;
  region?: FiasEntity;
  city?: FiasEntity;
  email?: string;
  phoneNumber?: string;
  legalAddress?: string;
  officeAddress?: string;
  bankDetails?: BankDetails;
  contactPersons: ContactPersonDto[];
  largeWholesale?: boolean;
  smallWholesale?: boolean;
  retail?: boolean;
  installment?: boolean;
  credit?: boolean;
  deposit?: boolean;
  transferForSale?: boolean;
  hasShowroom?: boolean;
  workWithIndividuals?: boolean;
  paidPartnership?: boolean;
  dropshipping?: boolean;
  minimumPurchase?: number;
  id: string;
  seqId: number;
  user: string;
}

export interface EntityVersionDtoOfSupplierEdit {
  entity: SupplierEdit;
  createdDate: string;
  user: string;
  id: string;
  seqId: number;
}

export interface GetAllResultDtoOfEntityVersionDtoOfSupplierEdit {
  count: number;
  entities: EntityVersionDtoOfSupplierEdit[];
}

export interface UserRoleAdd {
  name: string;
  isMainAdmin?: boolean;
  createdDate?: string;
  isArchived?: boolean;
  accessRights: AccessRightsDto;
}

export interface UserRoleEdit {
  name: string;
  isMainAdmin?: boolean;
  createdDate?: string;
  isArchived?: boolean;
  accessRights: AccessRightsDto;
  id: string;
  seqId: number;
}

export interface GetAllResultDtoOfUserLookup {
  count: number;
  entities: UserLookup[];
}

export interface UserAdd {
  email: string;
  fullname: string;
  position: string;
  roleId: string;
  phoneNumber?: string;
  hasSuppliersAccess?: boolean;
  hasFullAccess?: boolean;
  hasUsersAccess?: boolean;
  createdDate?: string;
  isArchived?: boolean;
  status?: GimUserStatus;
}

export interface UserEdit {
  email: string;
  fullname: string;
  position: string;
  roleId: string;
  phoneNumber?: string;
  hasSuppliersAccess?: boolean;
  hasFullAccess?: boolean;
  hasUsersAccess?: boolean;
  createdDate?: string;
  isArchived?: boolean;
  status?: GimUserStatus;
  id: string;
  seqId: number;
}

export type RequestParams = Omit<RequestInit, "body" | "method"> & {
  secure?: boolean;
};

export type RequestQueryParamsType = Record<string | number, any>;

interface ApiConfig<SecurityDataType> {
  baseUrl?: string;
  baseApiParams?: RequestParams;
  securityWorker?: (securityData: SecurityDataType) => RequestParams;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
  data: D;
  error: E | null;
}

enum BodyType {
  Json,
  FormData,
}

class HttpClient<SecurityDataType> {
  public baseUrl: string = "";
  private securityData: SecurityDataType = null as any;
  private securityWorker: null | ApiConfig<SecurityDataType>["securityWorker"] = null;

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {
      "Content-Type": "application/json",
    },
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType) => {
    this.securityData = data;
  };

  private addQueryParam(query: RequestQueryParamsType, key: string) {
    return (
      encodeURIComponent(key) + "=" + encodeURIComponent(Array.isArray(query[key]) ? query[key].join(",") : query[key])
    );
  }

  protected addQueryParams(rawQuery?: RequestQueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter((key) => "undefined" !== typeof query[key]);
    return keys.length
      ? `?${keys
          .map((key) =>
            typeof query[key] === "object" && !Array.isArray(query[key])
              ? this.addQueryParams(query[key] as object).substring(1)
              : this.addQueryParam(query, key),
          )
          .join("&")}`
      : "";
  }

  private bodyFormatters: Record<BodyType, (input: any) => any> = {
    [BodyType.Json]: JSON.stringify,
    [BodyType.FormData]: (input: any) =>
      Object.keys(input).reduce((data, key) => {
        data.append(key, input[key]);
        return data;
      }, new FormData()),
  };

  private mergeRequestOptions(params: RequestParams, securityParams?: RequestParams): RequestParams {
    return {
      ...this.baseApiParams,
      ...params,
      ...(securityParams || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params.headers || {}),
        ...((securityParams && securityParams.headers) || {}),
      },
    };
  }

  private safeParseResponse = <T = any, E = any>(response: Response): Promise<HttpResponse<T, E>> => {
    const r = response as HttpResponse<T, E>;
    r.error = null;

    return response
      .json()
      .then((data) => {
        if (r.ok) {
          r.data = data;
        } else {
          r.error = data;
        }
        return r;
      })
      .catch((e) => {
        r.error = e;
        return r;
      });
  };

  public request = <T = any, E = any>(
    path: string,
    method: string,
    { secure, ...params }: RequestParams = {},
    body?: any,
    bodyType?: BodyType,
    secureByDefault?: boolean,
  ): Promise<HttpResponse<T>> => {
    const requestUrl = `${this.baseUrl}${path}`;
    const secureOptions =
      (secureByDefault || secure) && this.securityWorker ? this.securityWorker(this.securityData) : {};
    const requestOptions = {
      ...this.mergeRequestOptions(params, secureOptions),
      method,
      body: body ? this.bodyFormatters[bodyType || BodyType.Json](body) : null,
    };

    return fetch(requestUrl, requestOptions).then(async (response) => {
      const data = await this.safeParseResponse<T, E>(response);
      if (!response.ok) throw data;
      return data;
    });
  };
}
/**
 * @title GIM price parser API
 * @version v1
 */
export class Api<SecurityDataType = any> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * @tags Accounts
     * @name AccountsLogin
     * @request POST:/api/Accounts/login
     */
    accountsLogin: (body: LoginModel, params?: RequestParams) =>
      this.request<any, any>(`/api/Accounts/login`, "POST", params, body),

    /**
     * @tags Accounts
     * @name AccountsValidateToken
     * @request GET:/api/Accounts/token-validation
     */
    accountsValidateToken: (query?: { token?: string }, params?: RequestParams) =>
      this.request<CheckTokenValidityModel, any>(
        `/api/Accounts/token-validation${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Accounts
     * @name AccountsChangePasswordAndLogin
     * @request PATCH:/api/Accounts/new-password
     */
    accountsChangePasswordAndLogin: (body: ChangePasswordModel, params?: RequestParams) =>
      this.request<any, any>(`/api/Accounts/new-password`, "PATCH", params, body),

    /**
     * @tags Accounts
     * @name AccountsUserInfo
     * @request GET:/api/Accounts/userinfo
     */
    accountsUserInfo: (params?: RequestParams) =>
      this.request<UserLookup, any>(`/api/Accounts/userinfo`, "GET", params),

    /**
     * @tags Categories
     * @name CategoriesGetMany
     * @request GET:/api/Categories
     */
    categoriesGetMany: (params?: RequestParams) =>
      this.request<GetAllResultDtoOfCategoryLookup, any>(`/api/Categories`, "GET", params),

    /**
     * @tags Categories
     * @name CategoriesAddOne
     * @request POST:/api/Categories
     */
    categoriesAddOne: (body: CategoryAdd, params?: RequestParams) =>
      this.request<CategoryEdit, any>(`/api/Categories`, "POST", params, body),

    /**
     * @tags Categories
     * @name CategoriesUpdateOne
     * @request PUT:/api/Categories
     */
    categoriesUpdateOne: (body: CategoryEdit, params?: RequestParams) =>
      this.request<CategoryEdit, any>(`/api/Categories`, "PUT", params, body),

    /**
     * @tags Categories
     * @name CategoriesDeleteOne
     * @request DELETE:/api/Categories
     */
    categoriesDeleteOne: (query?: { id?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/Categories${this.addQueryParams(query)}`, "DELETE", params),

    /**
     * @tags Categories
     * @name CategoriesFind
     * @request GET:/api/Categories/find
     */
    categoriesFind: (query?: { filter?: string; includeChildren?: string[] }, params?: RequestParams) =>
      this.request<TreeItemOfCategoryLookup[], any>(`/api/Categories/find${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags Categories
     * @name CategoriesGetChildren
     * @request GET:/api/Categories/children
     */
    categoriesGetChildren: (query?: { parentId?: string; "includeChildren[]"?: string[] }, params?: RequestParams) =>
      this.request<TreeItemOfCategoryLookup[], any>(
        `/api/Categories/children${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Categories
     * @name CategoriesGetChildrenFlatten
     * @request GET:/api/Categories/children-flatten
     */
    categoriesGetChildrenFlatten: (
      query?: { "ids[]"?: string[]; "parents[]"?: string[]; includeRoot?: boolean },
      params?: RequestParams,
    ) =>
      this.request<CategoryLookup[], any>(
        `/api/Categories/children-flatten${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Categories
     * @name CategoriesGetOne
     * @request GET:/api/Categories/{id}
     */
    categoriesGetOne: (id: string, params?: RequestParams) =>
      this.request<CategoryEdit, any>(`/api/Categories/${id}`, "GET", params),

    /**
     * @tags Categories
     * @name CategoriesAddManyRandom
     * @request POST:/api/Categories/many
     */
    categoriesAddManyRandom: (data: { count?: number }, params?: RequestParams) =>
      this.request<CategoryEdit[], any>(`/api/Categories/many`, "POST", params, data),

    /**
     * @tags Categories
     * @name CategoriesDeleteMany
     * @request DELETE:/api/Categories/many
     */
    categoriesDeleteMany: (params?: RequestParams) => this.request<any, any>(`/api/Categories/many`, "DELETE", params),

    /**
     * @tags Categories
     * @name CategoriesFromYandexMarket
     * @request POST:/api/Categories/yandexMarket
     */
    categoriesFromYandexMarket: (body: GimFileAdd, params?: RequestParams) =>
      this.request<CategoryEdit[], any>(`/api/Categories/yandexMarket`, "POST", params, body),

    /**
     * @tags Categories
     * @name CategoriesUpdateParent
     * @request PUT:/api/Categories/newParent
     */
    categoriesUpdateParent: (body: UpdateParentModel, params?: RequestParams) =>
      this.request<CategoryEdit, any>(`/api/Categories/newParent`, "PUT", params, body),

    /**
     * @tags Categories
     * @name CategoriesMergeOne
     * @request PATCH:/api/Categories/merge
     */
    categoriesMergeOne: (body: MergeCategoryModel, params?: RequestParams) =>
      this.request<CategoryEdit, any>(`/api/Categories/merge`, "PATCH", params, body),

    /**
     * @tags Categories
     * @name CategoriesMoveOne
     * @request PATCH:/api/Categories/{id}/move
     */
    categoriesMoveOne: (id: string, body: MoveCategoryModel, params?: RequestParams) =>
      this.request<CategoryEdit, any>(`/api/Categories/${id}/move`, "PATCH", params, body),

    /**
     * @tags Categories
     * @name CategoriesUpdateMappings
     * @request PATCH:/api/Categories/{id}/mapings
     */
    categoriesUpdateMappings: (id: string, body: CategoryMappingItem[], params?: RequestParams) =>
      this.request<CategoryEdit, any>(`/api/Categories/${id}/mapings`, "PATCH", params, body),

    /**
     * @tags Categories
     * @name CategoriesGetVersions
     * @request GET:/api/Categories/versions
     */
    categoriesGetVersions: (query?: { page?: number; pageSize?: number }, params?: RequestParams) =>
      this.request<GetAllResultDtoOfCategoryLookup, any>(
        `/api/Categories/versions${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Categories
     * @name CategoriesRestoreVersion
     * @request PUT:/api/Categories/restore/{version}
     */
    categoriesRestoreVersion: (version: string, params?: RequestParams) =>
      this.request<any, any>(`/api/Categories/restore/${version}`, "PUT", params),

    /**
     * @tags CategoryProperties
     * @name CategoryPropertiesGetMany
     * @request GET:/api/CategoryProperties
     */
    categoryPropertiesGetMany: (query?: { CategoryId?: string }, params?: RequestParams) =>
      this.request<GetAllResultDtoOfProductPropertyLookup, any>(
        `/api/CategoryProperties${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags CategoryProperties
     * @name CategoryPropertiesAddOne
     * @request POST:/api/CategoryProperties
     */
    categoryPropertiesAddOne: (body: ProductPropertyAdd, params?: RequestParams) =>
      this.request<ProductPropertyEdit, any>(`/api/CategoryProperties`, "POST", params, body),

    /**
     * @tags CategoryProperties
     * @name CategoryPropertiesUpdateOne
     * @request PUT:/api/CategoryProperties
     */
    categoryPropertiesUpdateOne: (body: ProductPropertyEdit, params?: RequestParams) =>
      this.request<ProductPropertyEdit, any>(`/api/CategoryProperties`, "PUT", params, body),

    /**
     * @tags CategoryProperties
     * @name CategoryPropertiesDeleteOne
     * @request DELETE:/api/CategoryProperties
     */
    categoryPropertiesDeleteOne: (query?: { id?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/CategoryProperties${this.addQueryParams(query)}`, "DELETE", params),

    /**
     * @tags CategoryProperties
     * @name CategoryPropertiesGetOne
     * @request GET:/api/CategoryProperties/{id}
     */
    categoryPropertiesGetOne: (id: string, params?: RequestParams) =>
      this.request<ProductPropertyEdit, any>(`/api/CategoryProperties/${id}`, "GET", params),

    /**
     * @tags CategoryPropertyValues
     * @name CategoryPropertyValuesGetMany
     * @request GET:/api/CategoryPropertyValues
     */
    categoryPropertyValuesGetMany: (
      query?: { PropertiesIds?: string[]; PropertyId?: string; ValuesIds?: string[] },
      params?: RequestParams,
    ) =>
      this.request<ProductPropertyValueLookup[], any>(
        `/api/CategoryPropertyValues${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags CategoryPropertyValues
     * @name CategoryPropertyValuesAddOne
     * @request POST:/api/CategoryPropertyValues
     */
    categoryPropertyValuesAddOne: (body: ProductPropertyValueAdd, params?: RequestParams) =>
      this.request<ProductPropertyValueEdit, any>(`/api/CategoryPropertyValues`, "POST", params, body),

    /**
     * @tags CategoryPropertyValues
     * @name CategoryPropertyValuesUpdateOne
     * @request PUT:/api/CategoryPropertyValues
     */
    categoryPropertyValuesUpdateOne: (body: ProductPropertyValueEdit, params?: RequestParams) =>
      this.request<ProductPropertyValueEdit, any>(`/api/CategoryPropertyValues`, "PUT", params, body),

    /**
     * @tags CategoryPropertyValues
     * @name CategoryPropertyValuesDeleteOne
     * @request DELETE:/api/CategoryPropertyValues
     */
    categoryPropertyValuesDeleteOne: (query?: { id?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/CategoryPropertyValues${this.addQueryParams(query)}`, "DELETE", params),

    /**
     * @tags CategoryPropertyValues
     * @name CategoryPropertyValuesGetOne
     * @request GET:/api/CategoryPropertyValues/{id}
     */
    categoryPropertyValuesGetOne: (id: string, params?: RequestParams) =>
      this.request<ProductPropertyValueEdit, any>(`/api/CategoryPropertyValues/${id}`, "GET", params),

    /**
     * @tags Images
     * @name ImagesGetMany
     * @request GET:/api/Images
     */
    imagesGetMany: (
      query?: { Ids?: string[]; ProductId?: string; IsMain?: boolean; Status?: any; page?: number; pageSize?: number },
      params?: RequestParams,
    ) => this.request<GetAllResultDtoOfImageFullDto, any>(`/api/Images${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags Images
     * @name ImagesAddOne
     * @request POST:/api/Images
     */
    imagesAddOne: (body: ImageAddDto, params?: RequestParams) =>
      this.request<ImageLookupDto, any>(`/api/Images`, "POST", params, body),

    /**
     * @tags Images
     * @name ImagesGetMain
     * @request GET:/api/Images/main
     */
    imagesGetMain: (query?: { productId?: string }, params?: RequestParams) =>
      this.request<ImageFullDto, any>(`/api/Images/main${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags Images
     * @name ImagesSetMain
     * @request PATCH:/api/Images/{id}/main
     */
    imagesSetMain: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/Images/${id}/main`, "PATCH", params),

    /**
     * @tags Images
     * @name ImagesSetPublished
     * @request PATCH:/api/Images/{id}/publish
     */
    imagesSetPublished: (id: string, query?: { isPublished?: boolean }, params?: RequestParams) =>
      this.request<any, any>(`/api/Images/${id}/publish${this.addQueryParams(query)}`, "PATCH", params),

    /**
     * @tags Images
     * @name ImagesDeleteOne
     * @request DELETE:/api/Images/{id}
     */
    imagesDeleteOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/Images/${id}`, "DELETE", params),

    /**
     * @tags Lookup
     * @name LookupCategoriesGetChildrenFlatten
     * @request GET:/api/Lookup/categories
     */
    lookupCategoriesGetChildrenFlatten: (
      query?: { "ids[]"?: string[]; "parents[]"?: string[]; includeRoot?: boolean },
      params?: RequestParams,
    ) => this.request<CategoryLookup[], any>(`/api/Lookup/categories${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags Lookup
     * @name LookupSuppliersGetMany
     * @request GET:/api/Lookup/suppliers-many
     */
    lookupSuppliersGetMany: (
      query?: {
        SeqId?: number;
        CreatedFrom?: string;
        CreatedTo?: string;
        Name?: string;
        City?: string;
        Inn?: string;
        UserId?: string;
        Status?: any;
        IsArchived?: boolean;
        SortBy?: string;
        SortDirection?: any;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfSupplierShort, any>(
        `/api/Lookup/suppliers-many${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Lookup
     * @name LookupSuppliersGetOne
     * @request GET:/api/Lookup/suppliers/{id}
     */
    lookupSuppliersGetOne: (id: string, params?: RequestParams) =>
      this.request<SupplierShort, any>(`/api/Lookup/suppliers/${id}`, "GET", params),

    /**
     * @tags Lookup
     * @name LookupProcessingRulesGetMany
     * @request GET:/api/Lookup/processing-rules
     */
    lookupProcessingRulesGetMany: (
      query?: {
        SeqId?: number;
        Name?: string;
        SupplierId?: string;
        ArchivedFilter?: any;
        SortBy?: string;
        SortDirection?: any;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfProcessingRuleLookup, any>(
        `/api/Lookup/processing-rules${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Lookup
     * @name LookupUserRolesGetMany
     * @request GET:/api/Lookup/user-roles
     */
    lookupUserRolesGetMany: (
      query?: {
        SeqId?: number;
        CreatedFrom?: string;
        CreatedTo?: string;
        Name?: string;
        UsersFrom?: number;
        ArchivedFilter?: any;
        SortBy?: string;
        SortDirection?: any;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfUserRoleLookup, any>(
        `/api/Lookup/user-roles${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Manufacturers
     * @name ManufacturersGetMany
     * @request GET:/api/Manufacturers
     */
    manufacturersGetMany: (query?: { page?: number; pageSize?: number }, params?: RequestParams) =>
      this.request<GetAllResultDtoOfManufacturerLookup, any>(
        `/api/Manufacturers${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Manufacturers
     * @name ManufacturersAddOne
     * @request POST:/api/Manufacturers
     */
    manufacturersAddOne: (body: ManufacturerAdd, params?: RequestParams) =>
      this.request<ManufacturerEdit, any>(`/api/Manufacturers`, "POST", params, body),

    /**
     * @tags Manufacturers
     * @name ManufacturersUpdateOne
     * @request PUT:/api/Manufacturers
     */
    manufacturersUpdateOne: (body: ManufacturerEdit, params?: RequestParams) =>
      this.request<ManufacturerEdit, any>(`/api/Manufacturers`, "PUT", params, body),

    /**
     * @tags Manufacturers
     * @name ManufacturersDeleteOne
     * @request DELETE:/api/Manufacturers
     */
    manufacturersDeleteOne: (query?: { id?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/Manufacturers${this.addQueryParams(query)}`, "DELETE", params),

    /**
     * @tags Manufacturers
     * @name ManufacturersGetOne
     * @request GET:/api/Manufacturers/{id}
     */
    manufacturersGetOne: (id: string, params?: RequestParams) =>
      this.request<ManufacturerEdit, any>(`/api/Manufacturers/${id}`, "GET", params),

    /**
     * @tags PriceListItemProperties
     * @name PriceListItemPropertiesGetMany
     * @request GET:/api/PriceListItemProperties
     */
    priceListItemPropertiesGetMany: (
      query?: {
        PriceListItemId?: string;
        PriceListItemsIds?: string[];
        PropertyKey?: string;
        Status?: any;
        productId?: string;
      },
      params?: RequestParams,
    ) =>
      this.request<PriceListItemProductPropertyLookup[], any>(
        `/api/PriceListItemProperties${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags PriceListItemProperties
     * @name PriceListItemPropertiesGetManyIndexed
     * @request GET:/api/PriceListItemProperties/indexed
     */
    priceListItemPropertiesGetManyIndexed: (
      query?: { priceListId?: string; startIndex?: number; stopIndex?: number },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfPriceListItemProductPropertyLookup, any>(
        `/api/PriceListItemProperties/indexed${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags PriceListItemProperties
     * @name PriceListItemPropertiesSetActionOne
     * @request PATCH:/api/PriceListItemProperties/{id}/action
     */
    priceListItemPropertiesSetActionOne: (id: string, query?: { action?: any }, params?: RequestParams) =>
      this.request<any, any>(`/api/PriceListItemProperties/${id}/action${this.addQueryParams(query)}`, "PATCH", params),

    /**
     * @tags PriceListItemProperties
     * @name PriceListItemPropertiesSetActionMany
     * @request PATCH:/api/PriceListItemProperties/action-many
     */
    priceListItemPropertiesSetActionMany: (body: PriceListItemPropertySetActionModel, params?: RequestParams) =>
      this.request<any, any>(`/api/PriceListItemProperties/action-many`, "PATCH", params, body),

    /**
     * @tags PriceListItems
     * @name PriceListItemsGetManyIndexed
     * @request GET:/api/PriceListItems
     */
    priceListItemsGetManyIndexed: (
      query?: {
        PriceListId?: string;
        Code?: string;
        CategoryName?: string;
        Category1Name?: string;
        Category2Name?: string;
        Category3Name?: string;
        Category4Name?: string;
        Category5Name?: string;
        ProductNameEq?: string;
        ProductNameRegEx?: string;
        Price1?: number;
        Price2?: number;
        Price3?: number;
        Status?: any;
        Skip?: boolean;
        UnprocessedItemsOnly?: boolean;
        ProcessedItemsOnly?: boolean;
        UnprocessedCodeError?: boolean;
        UnprocessedNameErrors?: boolean;
        UnprocessedPriceError?: boolean;
        SortBy?: string;
        SortDirection?: any;
        startIndex?: number;
        stopIndex?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfPriceListItemLookup, any>(
        `/api/PriceListItems${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags PriceListItems
     * @name PriceListItemsGetSynonymsMany
     * @request GET:/api/PriceListItems/{id}/synonyms
     */
    priceListItemsGetSynonymsMany: (id: string, params?: RequestParams) =>
      this.request<ProductSynonymDto[], any>(`/api/PriceListItems/${id}/synonyms`, "GET", params),

    /**
     * @tags PriceListItems
     * @name PriceListItemsSetProductOne
     * @request PATCH:/api/PriceListItems/{id}/product/{productId}
     */
    priceListItemsSetProductOne: (id: string, productId: string, params?: RequestParams) =>
      this.request<any, any>(`/api/PriceListItems/${id}/product/${productId}`, "PATCH", params),

    /**
     * @tags PriceListItems
     * @name PriceListItemsSetSkipOne
     * @request PATCH:/api/PriceListItems/{id}/skip-one/{skip}
     */
    priceListItemsSetSkipOne: (id: string, skip: boolean, params?: RequestParams) =>
      this.request<any, any>(`/api/PriceListItems/${id}/skip-one/${skip}`, "PATCH", params),

    /**
     * @tags PriceListItems
     * @name PriceListItemsSkipMany
     * @request PATCH:/api/PriceListItems/skip-many
     */
    priceListItemsSkipMany: (
      query?: {
        PriceListId?: string;
        Code?: string;
        CategoryName?: string;
        Category1Name?: string;
        Category2Name?: string;
        Category3Name?: string;
        Category4Name?: string;
        Category5Name?: string;
        ProductNameEq?: string;
        ProductNameRegEx?: string;
        Price1?: number;
        Price2?: number;
        Price3?: number;
        Status?: any;
        Skip?: boolean;
        UnprocessedItemsOnly?: boolean;
        ProcessedItemsOnly?: boolean;
        UnprocessedCodeError?: boolean;
        UnprocessedNameErrors?: boolean;
        UnprocessedPriceError?: boolean;
      },
      params?: RequestParams,
    ) => this.request<any, any>(`/api/PriceListItems/skip-many${this.addQueryParams(query)}`, "PATCH", params),

    /**
     * @tags PriceListItems
     * @name PriceListItemsSetNameActionOne
     * @request PATCH:/api/PriceListItems/{id}/nameAction
     */
    priceListItemsSetNameActionOne: (id: string, query?: { action?: any }, params?: RequestParams) =>
      this.request<any, any>(`/api/PriceListItems/${id}/nameAction${this.addQueryParams(query)}`, "PATCH", params),

    /**
     * @tags PriceLists
     * @name PriceListsGetMany
     * @request GET:/api/PriceLists
     */
    priceListsGetMany: (
      query?: {
        SeqId?: number;
        ParsedFrom?: string;
        ParsedTo?: string;
        SupplierId?: string;
        RulesSource?: any;
        Status?: any;
        ExceptStatus?: any;
        SortBy?: string;
        SortDirection?: any;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfPriceListLookup, any>(
        `/api/PriceLists${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags PriceLists
     * @name PriceListsAddOne
     * @request POST:/api/PriceLists
     */
    priceListsAddOne: (body: PriceListAdd, params?: RequestParams) =>
      this.request<PriceListLookup, any>(`/api/PriceLists`, "POST", params, body),

    /**
     * @tags PriceLists
     * @name PriceListsDeleteOne
     * @request DELETE:/api/PriceLists
     */
    priceListsDeleteOne: (query?: { id?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/PriceLists${this.addQueryParams(query)}`, "DELETE", params),

    /**
     * @tags PriceLists
     * @name PriceListsGetOne
     * @request GET:/api/PriceLists/{id}
     */
    priceListsGetOne: (id: string, params?: RequestParams) =>
      this.request<PriceListFull, any>(`/api/PriceLists/${id}`, "GET", params),

    /**
     * @tags PriceLists
     * @name PriceListsCommitOne
     * @request POST:/api/PriceLists/{id}/commit
     */
    priceListsCommitOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/PriceLists/${id}/commit`, "POST", params),

    /**
     * @tags PriceLists
     * @name PriceListsParseOne
     * @request POST:/api/PriceLists/parse-one
     */
    priceListsParseOne: (body: PriceListAdd, params?: RequestParams) =>
      this.request<PriceListItemLookup[], any>(`/api/PriceLists/parse-one`, "POST", params, body),

    /**
     * @tags PriceLists
     * @name PriceListsSearchProducts
     * @request PATCH:/api/PriceLists/{id}/search-products
     */
    priceListsSearchProducts: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/PriceLists/${id}/search-products`, "PATCH", params),

    /**
     * @tags PriceLists
     * @name PriceListsIndex
     * @request POST:/api/PriceLists/test
     */
    priceListsIndex: (
      data: {
        ContentType?: string;
        ContentDisposition?: string;
        Headers?: string;
        Length?: number;
        Name?: string;
        FileName?: string;
      },
      params?: RequestParams,
    ) => this.request<any, any>(`/api/PriceLists/test`, "POST", params, data, BodyType.FormData),

    /**
     * @tags PriceLists
     * @name PriceListsCheckEmit
     * @request POST:/api/PriceLists/emit
     */
    priceListsCheckEmit: (body: string, params?: RequestParams) =>
      this.request<EmitResultDto, any>(`/api/PriceLists/emit`, "POST", params, body),

    /**
     * @tags ProcessingRule
     * @name ProcessingRuleGetMany
     * @request GET:/api/ProcessingRule
     */
    processingRuleGetMany: (
      query?: {
        SeqId?: number;
        Name?: string;
        SupplierId?: string;
        ArchivedFilter?: any;
        SortBy?: string;
        SortDirection?: any;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfProcessingRuleLookup, any>(
        `/api/ProcessingRule${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags ProcessingRule
     * @name ProcessingRuleAddOne
     * @request POST:/api/ProcessingRule
     */
    processingRuleAddOne: (body: ProcessingRuleAdd, params?: RequestParams) =>
      this.request<ProcessingRuleFull, any>(`/api/ProcessingRule`, "POST", params, body),

    /**
     * @tags ProcessingRule
     * @name ProcessingRuleUpdateOne
     * @request PUT:/api/ProcessingRule
     */
    processingRuleUpdateOne: (body: ProcessingRuleFull, params?: RequestParams) =>
      this.request<ProcessingRuleFull, any>(`/api/ProcessingRule`, "PUT", params, body),

    /**
     * @tags ProcessingRule
     * @name ProcessingRuleGetOne
     * @request GET:/api/ProcessingRule/{id}
     */
    processingRuleGetOne: (id: string, params?: RequestParams) =>
      this.request<ProcessingRuleFull, any>(`/api/ProcessingRule/${id}`, "GET", params),

    /**
     * @tags ProcessingRule
     * @name ProcessingRuleToArchiveOne
     * @request PATCH:/api/ProcessingRule/to-archive-one/{id}
     */
    processingRuleToArchiveOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/ProcessingRule/to-archive-one/${id}`, "PATCH", params),

    /**
     * @tags ProcessingRule
     * @name ProcessingRuleFromArchiveOne
     * @request PATCH:/api/ProcessingRule/from-archive-one/{id}
     */
    processingRuleFromArchiveOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/ProcessingRule/from-archive-one/${id}`, "PATCH", params),

    /**
     * @tags ProcessingRule
     * @name ProcessingRuleCheckEmit
     * @request POST:/api/ProcessingRule/emit
     */
    processingRuleCheckEmit: (body: CheckEmitPayload, params?: RequestParams) =>
      this.request<EmitResultDto, any>(`/api/ProcessingRule/emit`, "POST", params, body),

    /**
     * @tags Products
     * @name ProductsGetManyIndexed
     * @request GET:/api/Products/indexed
     */
    productsGetManyIndexed: (
      query?: {
        startIndex?: number;
        stopIndex?: number;
        SortBy?: string;
        SortDirection?: any;
        Ids?: string[];
        SeqId?: number;
        Category1?: string;
        Category2?: string;
        Category3?: string;
        Category4?: string;
        Category5?: string;
        Name?: string;
        Names?: string[];
        Status?: any;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfProductLookup, any>(
        `/api/Products/indexed${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Products
     * @name ProductsCountAll
     * @request GET:/api/Products/count
     */
    productsCountAll: (
      query?: {
        Ids?: string[];
        SeqId?: number;
        Category1?: string;
        Category2?: string;
        Category3?: string;
        Category4?: string;
        Category5?: string;
        Name?: string;
        Names?: string[];
        Status?: any;
      },
      params?: RequestParams,
    ) => this.request<number, any>(`/api/Products/count${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags Products
     * @name ProductsGetOne
     * @request GET:/api/Products/{id}
     */
    productsGetOne: (id: string, params?: RequestParams) =>
      this.request<ProductEdit, any>(`/api/Products/${id}`, "GET", params),

    /**
     * @tags Products
     * @name ProductsGetProperties
     * @request GET:/api/Products/{id}/properties
     */
    productsGetProperties: (id: string, params?: RequestParams) =>
      this.request<ProductPropertiesModel, any>(`/api/Products/${id}/properties`, "GET", params),

    /**
     * @tags Products
     * @name ProductsGetLookupOne
     * @request GET:/api/Products/lookup/{id}
     */
    productsGetLookupOne: (id: string, params?: RequestParams) =>
      this.request<ProductLookup, any>(`/api/Products/lookup/${id}`, "GET", params),

    /**
     * @tags Products
     * @name ProductsAddOne
     * @request POST:/api/Products
     */
    productsAddOne: (body: ProductAdd, params?: RequestParams) =>
      this.request<ProductEdit, any>(`/api/Products`, "POST", params, body),

    /**
     * @tags Products
     * @name ProductsUpdateOne
     * @request PUT:/api/Products
     */
    productsUpdateOne: (body: ProductEdit, params?: RequestParams) =>
      this.request<ProductEdit, any>(`/api/Products`, "PUT", params, body),

    /**
     * @tags Products
     * @name ProductsDeleteOne
     * @request DELETE:/api/Products
     */
    productsDeleteOne: (query?: { id?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/Products${this.addQueryParams(query)}`, "DELETE", params),

    /**
     * @tags Products
     * @name ProductsAddManyRandom
     * @request POST:/api/Products/many
     */
    productsAddManyRandom: (data: { count?: number }, params?: RequestParams) =>
      this.request<ProductEdit[], any>(`/api/Products/many`, "POST", params, data),

    /**
     * @tags Products
     * @name ProductsDeleteMany
     * @request DELETE:/api/Products/many
     */
    productsDeleteMany: (params?: RequestParams) => this.request<any, any>(`/api/Products/many`, "DELETE", params),

    /**
     * @tags Products
     * @name ProductsSetDescriptionOne
     * @request PATCH:/api/Products/{id}/description
     */
    productsSetDescriptionOne: (id: string, query?: { description?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/Products/${id}/description${this.addQueryParams(query)}`, "PATCH", params),

    /**
     * @tags Products
     * @name ProductsSetPropertyValueOne
     * @request PATCH:/api/Products/{id}/property-value/{oldId}/{newId}
     */
    productsSetPropertyValueOne: (id: string, oldId: string, newId: string, params?: RequestParams) =>
      this.request<any, any>(`/api/Products/${id}/property-value/${oldId}/${newId}`, "PATCH", params),

    /**
     * @tags Products
     * @name ProductsMergeMany
     * @request PATCH:/api/Products/merge
     */
    productsMergeMany: (body: string[], params?: RequestParams) =>
      this.request<any, any>(`/api/Products/merge`, "PATCH", params, body),

    /**
     * @tags Products
     * @name ProductsGetVersions
     * @request GET:/api/Products/{id}/versions
     */
    productsGetVersions: (id: string, query?: { page?: number; pageSize?: number }, params?: RequestParams) =>
      this.request<GetAllResultDtoOfProductLookup, any>(
        `/api/Products/${id}/versions${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Products
     * @name ProductsRestoreVersion
     * @request PUT:/api/Products/{id}/restore/{version}
     */
    productsRestoreVersion: (id: string, version: string, params?: RequestParams) =>
      this.request<ProductEdit, any>(`/api/Products/${id}/restore/${version}`, "PUT", params),

    /**
     * @tags SchedulerTasks
     * @name SchedulerTasksGetMany
     * @request GET:/api/SchedulerTasks
     */
    schedulerTasksGetMany: (query?: { page?: number; pageSize?: number }, params?: RequestParams) =>
      this.request<GetAllResultDtoOfSchedulerTaskLookup, any>(
        `/api/SchedulerTasks${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags SchedulerTasks
     * @name SchedulerTasksAddOne
     * @request POST:/api/SchedulerTasks
     */
    schedulerTasksAddOne: (body: SchedulerTaskAdd, params?: RequestParams) =>
      this.request<SchedulerTaskFull, any>(`/api/SchedulerTasks`, "POST", params, body),

    /**
     * @tags SchedulerTasks
     * @name SchedulerTasksUpdateOne
     * @request PUT:/api/SchedulerTasks
     */
    schedulerTasksUpdateOne: (body: SchedulerTaskEdit, params?: RequestParams) =>
      this.request<SchedulerTaskFull, any>(`/api/SchedulerTasks`, "PUT", params, body),

    /**
     * @tags SchedulerTasks
     * @name SchedulerTasksDeleteOne
     * @request DELETE:/api/SchedulerTasks
     */
    schedulerTasksDeleteOne: (query?: { id?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/SchedulerTasks${this.addQueryParams(query)}`, "DELETE", params),

    /**
     * @tags SchedulerTasks
     * @name SchedulerTasksGetOne
     * @request GET:/api/SchedulerTasks/{id}
     */
    schedulerTasksGetOne: (id: string, params?: RequestParams) =>
      this.request<SchedulerTaskFull, any>(`/api/SchedulerTasks/${id}`, "GET", params),

    /**
     * @tags SupplierProducts
     * @name SupplierProductsGetMany
     * @request GET:/api/SupplierProducts
     */
    supplierProductsGetMany: (
      query?: {
        ProductId?: string;
        SupplierSeqId?: number;
        SupplierName?: string;
        Code?: string;
        Name?: string;
        Price?: number;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfSupplierProductLookup, any>(
        `/api/SupplierProducts${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags SupplierProducts
     * @name SupplierProductsAddOne
     * @request POST:/api/SupplierProducts
     */
    supplierProductsAddOne: (body: SupplierProductAdd, params?: RequestParams) =>
      this.request<SupplierProductEdit, any>(`/api/SupplierProducts`, "POST", params, body),

    /**
     * @tags SupplierProducts
     * @name SupplierProductsUpdateOne
     * @request PUT:/api/SupplierProducts
     */
    supplierProductsUpdateOne: (body: SupplierProductEdit, params?: RequestParams) =>
      this.request<SupplierProductEdit, any>(`/api/SupplierProducts`, "PUT", params, body),

    /**
     * @tags SupplierProducts
     * @name SupplierProductsDeleteOne
     * @request DELETE:/api/SupplierProducts
     */
    supplierProductsDeleteOne: (query?: { id?: string }, params?: RequestParams) =>
      this.request<any, any>(`/api/SupplierProducts${this.addQueryParams(query)}`, "DELETE", params),

    /**
     * @tags SupplierProducts
     * @name SupplierProductsGetOne
     * @request GET:/api/SupplierProducts/{id}
     */
    supplierProductsGetOne: (id: string, params?: RequestParams) =>
      this.request<SupplierProductEdit, any>(`/api/SupplierProducts/${id}`, "GET", params),

    /**
     * @tags SupplierProducts
     * @name SupplierProductsGetProperties
     * @request GET:/api/SupplierProducts/{id}/properties
     */
    supplierProductsGetProperties: (id: string, params?: RequestParams) =>
      this.request<ProductPropertiesModel, any>(`/api/SupplierProducts/${id}/properties`, "GET", params),

    /**
     * @tags Suppliers
     * @name SuppliersGetMany
     * @request GET:/api/Suppliers
     */
    suppliersGetMany: (
      query?: {
        SeqId?: number;
        CreatedFrom?: string;
        CreatedTo?: string;
        Name?: string;
        City?: string;
        Inn?: string;
        UserId?: string;
        Status?: any;
        IsArchived?: boolean;
        SortBy?: string;
        SortDirection?: any;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfSupplierLookup, any>(`/api/Suppliers${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags Suppliers
     * @name SuppliersAddOne
     * @request POST:/api/Suppliers
     */
    suppliersAddOne: (body: SupplierAdd, params?: RequestParams) =>
      this.request<SupplierEdit, any>(`/api/Suppliers`, "POST", params, body),

    /**
     * @tags Suppliers
     * @name SuppliersUpdateOne
     * @request PUT:/api/Suppliers
     */
    suppliersUpdateOne: (body: SupplierEdit, params?: RequestParams) =>
      this.request<SupplierEdit, any>(`/api/Suppliers`, "PUT", params, body),

    /**
     * @tags Suppliers
     * @name SuppliersCount
     * @request GET:/api/Suppliers/count
     */
    suppliersCount: (
      query?: {
        SeqId?: number;
        CreatedFrom?: string;
        CreatedTo?: string;
        Name?: string;
        City?: string;
        Inn?: string;
        UserId?: string;
        Status?: any;
        IsArchived?: boolean;
      },
      params?: RequestParams,
    ) => this.request<number, any>(`/api/Suppliers/count${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags Suppliers
     * @name SuppliersGetOne
     * @request GET:/api/Suppliers/{id}
     */
    suppliersGetOne: (id: string, params?: RequestParams) =>
      this.request<SupplierEdit, any>(`/api/Suppliers/${id}`, "GET", params),

    /**
     * @tags Suppliers
     * @name SuppliersToArchiveOne
     * @request PATCH:/api/Suppliers/to-archive-one/{id}
     */
    suppliersToArchiveOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/Suppliers/to-archive-one/${id}`, "PATCH", params),

    /**
     * @tags Suppliers
     * @name SuppliersFromArchiveOne
     * @request PATCH:/api/Suppliers/from-archive-one/{id}
     */
    suppliersFromArchiveOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/Suppliers/from-archive-one/${id}`, "PATCH", params),

    /**
     * @tags Suppliers
     * @name SuppliersGetVersions
     * @request GET:/api/Suppliers/{id}/versions
     */
    suppliersGetVersions: (id: string, query?: { page?: number; pageSize?: number }, params?: RequestParams) =>
      this.request<GetAllResultDtoOfEntityVersionDtoOfSupplierEdit, any>(
        `/api/Suppliers/${id}/versions${this.addQueryParams(query)}`,
        "GET",
        params,
      ),

    /**
     * @tags Suppliers
     * @name SuppliersRestoreVersion
     * @request PUT:/api/Suppliers/restore/{versionId}
     */
    suppliersRestoreVersion: (versionId: string, params?: RequestParams) =>
      this.request<SupplierEdit, any>(`/api/Suppliers/restore/${versionId}`, "PUT", params),

    /**
     * @tags Test
     * @name TestTest
     * @request POST:/api/Test
     */
    testTest: (params?: RequestParams) => this.request<any, any>(`/api/Test`, "POST", params),

    /**
     * @tags UserRoles
     * @name UserRolesGetMany
     * @request GET:/api/UserRoles
     */
    userRolesGetMany: (
      query?: {
        SeqId?: number;
        CreatedFrom?: string;
        CreatedTo?: string;
        Name?: string;
        UsersFrom?: number;
        ArchivedFilter?: any;
        SortBy?: string;
        SortDirection?: any;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) =>
      this.request<GetAllResultDtoOfUserRoleLookup, any>(`/api/UserRoles${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags UserRoles
     * @name UserRolesAddOne
     * @request POST:/api/UserRoles
     */
    userRolesAddOne: (body: UserRoleAdd, params?: RequestParams) =>
      this.request<UserRoleEdit, any>(`/api/UserRoles`, "POST", params, body),

    /**
     * @tags UserRoles
     * @name UserRolesUpdateOne
     * @request PUT:/api/UserRoles
     */
    userRolesUpdateOne: (body: UserRoleEdit, params?: RequestParams) =>
      this.request<UserRoleEdit, any>(`/api/UserRoles`, "PUT", params, body),

    /**
     * @tags UserRoles
     * @name UserRolesGetOne
     * @request GET:/api/UserRoles/{id}
     */
    userRolesGetOne: (id: string, params?: RequestParams) =>
      this.request<UserRoleEdit, any>(`/api/UserRoles/${id}`, "GET", params),

    /**
     * @tags UserRoles
     * @name UserRolesToArchiveOne
     * @request PATCH:/api/UserRoles/to-archive-one/{id}
     */
    userRolesToArchiveOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/UserRoles/to-archive-one/${id}`, "PATCH", params),

    /**
     * @tags UserRoles
     * @name UserRolesFromArchiveOne
     * @request PATCH:/api/UserRoles/from-archive-one/{id}
     */
    userRolesFromArchiveOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/UserRoles/from-archive-one/${id}`, "PATCH", params),

    /**
     * @tags Users
     * @name UsersGetMany
     * @request GET:/api/Users
     */
    usersGetMany: (
      query?: {
        SeqId?: number;
        CreatedFrom?: string;
        CreatedTo?: string;
        Fullname?: string;
        Email?: string;
        RoleId?: string;
        Status?: any;
        Token?: string;
        ArchivedFilter?: any;
        SortBy?: string;
        SortDirection?: any;
        page?: number;
        pageSize?: number;
      },
      params?: RequestParams,
    ) => this.request<GetAllResultDtoOfUserLookup, any>(`/api/Users${this.addQueryParams(query)}`, "GET", params),

    /**
     * @tags Users
     * @name UsersAddOne
     * @request POST:/api/Users
     */
    usersAddOne: (body: UserAdd, params?: RequestParams) =>
      this.request<UserEdit, any>(`/api/Users`, "POST", params, body),

    /**
     * @tags Users
     * @name UsersUpdateOne
     * @request PUT:/api/Users
     */
    usersUpdateOne: (body: UserEdit, params?: RequestParams) =>
      this.request<UserEdit, any>(`/api/Users`, "PUT", params, body),

    /**
     * @tags Users
     * @name UsersGetOne
     * @request GET:/api/Users/{id}
     */
    usersGetOne: (id: string, params?: RequestParams) => this.request<UserEdit, any>(`/api/Users/${id}`, "GET", params),

    /**
     * @tags Users
     * @name UsersSetPasswordToken
     * @request PATCH:/api/Users/new-password-token/{id}
     */
    usersSetPasswordToken: (id: string, params?: RequestParams) =>
      this.request<string, any>(`/api/Users/new-password-token/${id}`, "PATCH", params),

    /**
     * @tags Users
     * @name UsersToArchiveOne
     * @request PATCH:/api/Users/to-archive-one/{id}
     */
    usersToArchiveOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/Users/to-archive-one/${id}`, "PATCH", params),

    /**
     * @tags Users
     * @name UsersFromArchiveOne
     * @request PATCH:/api/Users/from-archive-one/{id}
     */
    usersFromArchiveOne: (id: string, params?: RequestParams) =>
      this.request<any, any>(`/api/Users/from-archive-one/${id}`, "PATCH", params),
  };
  categoryMapToMany = {
    /**
     * @tags PriceListItems
     * @name PriceListItemsSetCategoryMapToMany
     * @request PATCH:/category-map-to-many/{priceListId}/{categoryId}/{level}
     */
    priceListItemsSetCategoryMapToMany: (
      priceListId: string,
      categoryId: string,
      level: number,
      query?: { categoryName?: string },
      params?: RequestParams,
    ) =>
      this.request<any, any>(
        `/category-map-to-many/${priceListId}/${categoryId}/${level}${this.addQueryParams(query)}`,
        "PATCH",
        params,
      ),
  };
}
