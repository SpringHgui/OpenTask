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
import type { ExecutorClient } from './ExecutorClient';
import {
    ExecutorClientFromJSON,
    ExecutorClientFromJSONTyped,
    ExecutorClientToJSON,
} from './ExecutorClient';

/**
 * 
 * @export
 * @interface ListClientsResponse
 */
export interface ListClientsResponse {
    /**
     * 
     * @type {string}
     * @memberof ListClientsResponse
     */
    count?: string;
    /**
     * 
     * @type {Array<ExecutorClient>}
     * @memberof ListClientsResponse
     */
    rows?: Array<ExecutorClient>;
}

/**
 * Check if a given object implements the ListClientsResponse interface.
 */
export function instanceOfListClientsResponse(value: object): boolean {
    return true;
}

export function ListClientsResponseFromJSON(json: any): ListClientsResponse {
    return ListClientsResponseFromJSONTyped(json, false);
}

export function ListClientsResponseFromJSONTyped(json: any, ignoreDiscriminator: boolean): ListClientsResponse {
    if (json == null) {
        return json;
    }
    return {
        
        'count': json['count'] == null ? undefined : json['count'],
        'rows': json['rows'] == null ? undefined : ((json['rows'] as Array<any>).map(ExecutorClientFromJSON)),
    };
}

export function ListClientsResponseToJSON(value?: ListClientsResponse | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'count': value['count'],
        'rows': value['rows'] == null ? undefined : ((value['rows'] as Array<any>).map(ExecutorClientToJSON)),
    };
}

