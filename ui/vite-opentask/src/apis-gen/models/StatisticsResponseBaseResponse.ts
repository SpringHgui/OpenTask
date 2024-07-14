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
import type { StatisticsResponse } from './StatisticsResponse';
import {
    StatisticsResponseFromJSON,
    StatisticsResponseFromJSONTyped,
    StatisticsResponseToJSON,
} from './StatisticsResponse';

/**
 * 
 * @export
 * @interface StatisticsResponseBaseResponse
 */
export interface StatisticsResponseBaseResponse {
    /**
     * 
     * @type {number}
     * @memberof StatisticsResponseBaseResponse
     */
    code?: number;
    /**
     * 
     * @type {boolean}
     * @memberof StatisticsResponseBaseResponse
     */
    readonly success?: boolean;
    /**
     * 
     * @type {StatisticsResponse}
     * @memberof StatisticsResponseBaseResponse
     */
    result?: StatisticsResponse;
    /**
     * 
     * @type {string}
     * @memberof StatisticsResponseBaseResponse
     */
    message?: string;
    /**
     * 
     * @type {string}
     * @memberof StatisticsResponseBaseResponse
     */
    traceId?: string;
}

/**
 * Check if a given object implements the StatisticsResponseBaseResponse interface.
 */
export function instanceOfStatisticsResponseBaseResponse(value: object): boolean {
    return true;
}

export function StatisticsResponseBaseResponseFromJSON(json: any): StatisticsResponseBaseResponse {
    return StatisticsResponseBaseResponseFromJSONTyped(json, false);
}

export function StatisticsResponseBaseResponseFromJSONTyped(json: any, ignoreDiscriminator: boolean): StatisticsResponseBaseResponse {
    if (json == null) {
        return json;
    }
    return {
        
        'code': json['code'] == null ? undefined : json['code'],
        'success': json['success'] == null ? undefined : json['success'],
        'result': json['result'] == null ? undefined : StatisticsResponseFromJSON(json['result']),
        'message': json['message'] == null ? undefined : json['message'],
        'traceId': json['traceId'] == null ? undefined : json['traceId'],
    };
}

export function StatisticsResponseBaseResponseToJSON(value?: Omit<StatisticsResponseBaseResponse, 'success'> | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'code': value['code'],
        'result': StatisticsResponseToJSON(value['result']),
        'message': value['message'],
        'traceId': value['traceId'],
    };
}

