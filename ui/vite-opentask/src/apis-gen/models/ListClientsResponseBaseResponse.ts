/* tslint:disable */
/* eslint-disable */
/**
 * OpenTask.WebApi
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { mapValues } from '../runtime';
import type { ListClientsResponse } from './ListClientsResponse';
import {
    ListClientsResponseFromJSON,
    ListClientsResponseFromJSONTyped,
    ListClientsResponseToJSON,
} from './ListClientsResponse';

/**
 * 
 * @export
 * @interface ListClientsResponseBaseResponse
 */
export interface ListClientsResponseBaseResponse {
    /**
     * 
     * @type {number}
     * @memberof ListClientsResponseBaseResponse
     */
    code?: number;
    /**
     * 
     * @type {boolean}
     * @memberof ListClientsResponseBaseResponse
     */
    readonly success?: boolean;
    /**
     * 
     * @type {ListClientsResponse}
     * @memberof ListClientsResponseBaseResponse
     */
    result?: ListClientsResponse;
    /**
     * 
     * @type {string}
     * @memberof ListClientsResponseBaseResponse
     */
    message?: string;
    /**
     * 
     * @type {string}
     * @memberof ListClientsResponseBaseResponse
     */
    traceId?: string;
}

/**
 * Check if a given object implements the ListClientsResponseBaseResponse interface.
 */
export function instanceOfListClientsResponseBaseResponse(value: object): boolean {
    return true;
}

export function ListClientsResponseBaseResponseFromJSON(json: any): ListClientsResponseBaseResponse {
    return ListClientsResponseBaseResponseFromJSONTyped(json, false);
}

export function ListClientsResponseBaseResponseFromJSONTyped(json: any, ignoreDiscriminator: boolean): ListClientsResponseBaseResponse {
    if (json == null) {
        return json;
    }
    return {
        
        'code': json['code'] == null ? undefined : json['code'],
        'success': json['success'] == null ? undefined : json['success'],
        'result': json['result'] == null ? undefined : ListClientsResponseFromJSON(json['result']),
        'message': json['message'] == null ? undefined : json['message'],
        'traceId': json['traceId'] == null ? undefined : json['traceId'],
    };
}

export function ListClientsResponseBaseResponseToJSON(value?: Omit<ListClientsResponseBaseResponse, 'success'> | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'code': value['code'],
        'result': ListClientsResponseToJSON(value['result']),
        'message': value['message'],
        'traceId': value['traceId'],
    };
}
