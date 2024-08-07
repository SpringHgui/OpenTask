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
import type { TaskInfo } from './TaskInfo';
import {
    TaskInfoFromJSON,
    TaskInfoFromJSONTyped,
    TaskInfoToJSON,
} from './TaskInfo';

/**
 * 
 * @export
 * @interface AddTaskRequest
 */
export interface AddTaskRequest {
    /**
     * 
     * @type {TaskInfo}
     * @memberof AddTaskRequest
     */
    task?: TaskInfo;
}

/**
 * Check if a given object implements the AddTaskRequest interface.
 */
export function instanceOfAddTaskRequest(value: object): boolean {
    return true;
}

export function AddTaskRequestFromJSON(json: any): AddTaskRequest {
    return AddTaskRequestFromJSONTyped(json, false);
}

export function AddTaskRequestFromJSONTyped(json: any, ignoreDiscriminator: boolean): AddTaskRequest {
    if (json == null) {
        return json;
    }
    return {
        
        'task': json['task'] == null ? undefined : TaskInfoFromJSON(json['task']),
    };
}

export function AddTaskRequestToJSON(value?: AddTaskRequest | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'task': TaskInfoToJSON(value['task']),
    };
}

