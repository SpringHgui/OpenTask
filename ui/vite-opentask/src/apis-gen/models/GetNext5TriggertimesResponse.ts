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
 * @interface GetNext5TriggertimesResponse
 */
export interface GetNext5TriggertimesResponse {
    /**
     * 
     * @type {Array<Date>}
     * @memberof GetNext5TriggertimesResponse
     */
    nextTriggers?: Array<Date>;
}

/**
 * Check if a given object implements the GetNext5TriggertimesResponse interface.
 */
export function instanceOfGetNext5TriggertimesResponse(value: object): boolean {
    return true;
}

export function GetNext5TriggertimesResponseFromJSON(json: any): GetNext5TriggertimesResponse {
    return GetNext5TriggertimesResponseFromJSONTyped(json, false);
}

export function GetNext5TriggertimesResponseFromJSONTyped(json: any, ignoreDiscriminator: boolean): GetNext5TriggertimesResponse {
    if (json == null) {
        return json;
    }
    return {
        
        'nextTriggers': json['nextTriggers'] == null ? undefined : json['nextTriggers'],
    };
}

export function GetNext5TriggertimesResponseToJSON(value?: GetNext5TriggertimesResponse | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'nextTriggers': value['nextTriggers'],
    };
}
