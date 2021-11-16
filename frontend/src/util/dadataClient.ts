import axios, { AxiosRequestConfig } from 'axios'

export type DadataSuggestion = {
    data: {
        fias_id: string
        region_fias_id: string
    }
    unrestricted_value: string
    value: string
}

export type DadataResponse = {
    suggestions: DadataSuggestion[]
}

class DadataClient {
    apiKey = '715a1d92f578319c5e44be9c2da2e3b72a5ce8f8'

    config: AxiosRequestConfig = {
        baseURL: 'https://suggestions.dadata.ru/',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': `Token ${this.apiKey}`
        }
    }

    region = (query: string) => {
        const url = '/suggestions/api/4_1/rs/suggest/address'
        const data = {
            query: query,
            from_bound: { value: 'region' },
            to_bound: { value: 'region' }
        }
        return axios.post<DadataResponse>(url, data, this.config)
    }

    city = (query: string, region_fias_id?: string) => {
        const url = '/suggestions/api/4_1/rs/suggest/address'
        const data = {
            query: query,
            locations: [
                ...(region_fias_id ? [{ 'region_fias_id': region_fias_id }] : [])
            ],
            from_bound: { value: 'city' },
            to_bound: { value: 'settlement' },
            restrict_value: true
        }
        return axios.post<DadataResponse>(url, data, this.config)
    }
}

export default DadataClient