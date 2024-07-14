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
import type { TaskLogsDayTrendResponse } from './TaskLogsDayTrendResponse';
import {
    TaskLogsDayTrendResponseFromJSON,
    TaskLogsDayTrendResponseFromJSONTyped,
    TaskLogsDayTrendResponseToJSON,
} from './TaskLogsDayTrendResponse';

/**
 * 
 * @export
 * @interface TaskLogsDayTrendResponseBaseResponse
 */
export interface TaskLogsDayTrendResponseBaseResponse {
    /**
     * 
     * @type {number}
     * @memberof TaskLogsDayTrendResponseBaseResponse
     */
    code?: number;
    /**
     * 
     * @type {boolean}
     * @memberof TaskLogsDayTrendResponseBaseResponse
     */
    readonly success?: boolean;
    /**
     * 
     * @type {TaskLogsDayTrendResponse}
     * @memberof TaskLogsDayTrendResponseBaseResponse
     */
    result?: TaskLogsDayTrendResponse;
    /**
     * 
     * @type {string}
     * @memberof TaskLogsDayTrendResponseBaseResponse
     */
    message?: string;
    /**
     * 
     * @type {string}
     * @memberof TaskLogsDayTrendResponseBaseResponse
     */
    traceId?: string;
}

/**
 * Check if a given object implements the TaskLogsDayTrendResponseBaseResponse interface.
 */
export function instanceOfTaskLogsDayTrendResponseBaseResponse(value: object): boolean {
    return true;
}

export function TaskLogsDayTrendResponseBaseResponseFromJSON(json: any): TaskLogsDayTrendResponseBaseResponse {
    return TaskLogsDayTrendResponseBaseResponseFromJSONTyped(json, false);
}

export function TaskLogsDayTrendResponseBaseResponseFromJSONTyped(json: any, ignoreDiscriminator: boolean): TaskLogsDayTrendResponseBaseResponse {
    if (json == null) {
        return json;
    }
    return {
        
        'code': json['code'] == null ? undefined : json['code'],
        'success': json['success'] == null ? undefined : json['success'],
        'result': json['result'] == null ? undefined : TaskLogsDayTrendResponseFromJSON(json['result']),
        'message': json['message'] == null ? undefined : json['message'],
        'traceId': json['traceId'] == null ? undefined : json['traceId'],
    };
}

export function TaskLogsDayTrendResponseBaseResponseToJSON(value?: Omit<TaskLogsDayTrendResponseBaseResponse, 'success'> | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'code': value['code'],
        'result': TaskLogsDayTrendResponseToJSON(value['result']),
        'message': value['message'],
        'traceId': value['traceId'],
    };
}
