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


import * as runtime from '../runtime';
import type {
  LoginCommand,
  LoginResponseBaseResponse,
} from '../models/index';
import {
    LoginCommandFromJSON,
    LoginCommandToJSON,
    LoginResponseBaseResponseFromJSON,
    LoginResponseBaseResponseToJSON,
} from '../models/index';

export interface LoginRequest {
    loginCommand?: LoginCommand;
}

/**
 * 
 */
export class UserApi extends runtime.BaseAPI {

    /**
     */
    async loginRaw(requestParameters: LoginRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<LoginResponseBaseResponse>> {
        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        if (this.configuration && this.configuration.apiKey) {
            headerParameters["Authorization"] = await this.configuration.apiKey("Authorization"); // Bearer authentication
        }

        const response = await this.request({
            path: `/api/User/Login`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: LoginCommandToJSON(requestParameters['loginCommand']),
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => LoginResponseBaseResponseFromJSON(jsonValue));
    }

    /**
     */
    async login(requestParameters: LoginRequest = {}, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<LoginResponseBaseResponse> {
        const response = await this.loginRaw(requestParameters, initOverrides);
        return await response.value();
    }

}
