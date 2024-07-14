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
/**
 * 
 * @export
 * @interface SwitchTaskStatusRequest
 */
export interface SwitchTaskStatusRequest {
    /**
     * 
     * @type {string}
     * @memberof SwitchTaskStatusRequest
     */
    taskId?: string;
    /**
     * 
     * @type {boolean}
     * @memberof SwitchTaskStatusRequest
     */
    enabled?: boolean;
}

/**
 * Check if a given object implements the SwitchTaskStatusRequest interface.
 */
export function instanceOfSwitchTaskStatusRequest(value: object): boolean {
    return true;
}

export function SwitchTaskStatusRequestFromJSON(json: any): SwitchTaskStatusRequest {
    return SwitchTaskStatusRequestFromJSONTyped(json, false);
}

export function SwitchTaskStatusRequestFromJSONTyped(json: any, ignoreDiscriminator: boolean): SwitchTaskStatusRequest {
    if (json == null) {
        return json;
    }
    return {
        
        'taskId': json['taskId'] == null ? undefined : json['taskId'],
        'enabled': json['enabled'] == null ? undefined : json['enabled'],
    };
}

export function SwitchTaskStatusRequestToJSON(value?: SwitchTaskStatusRequest | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'taskId': value['taskId'],
        'enabled': value['enabled'],
    };
}

