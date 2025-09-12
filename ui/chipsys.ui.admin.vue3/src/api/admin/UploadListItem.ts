import { UploadListItemAddInput, UploadListItemUpdateInput, UploadListItemGetOutput, UploadListItemGetListOutput, UploadListItemGetListInput } from './data-contracts'
import { RequestParams } from './http-client'

export class UploadListItemApi {
  async add(input: UploadListItemAddInput, params?: RequestParams) {
    return { success: true, data: { id: 1 } }
  }

  async update(input: UploadListItemUpdateInput, params?: RequestParams) {
    return { success: true, data: input }
  }

  async delete(input: { id: number }, params?: RequestParams) {
    return { success: true }
  }

  async get(input: { id: number }, params?: RequestParams) {
    return { success: true, data: { id: input.id, rawCode: 'TEST001', matchStatus: 'Unmatched' } as UploadListItemGetOutput }
  }

  async getList(input: UploadListItemGetListInput, params?: RequestParams) {
    return { success: true, data: [] as UploadListItemGetListOutput[] }
  }
}
