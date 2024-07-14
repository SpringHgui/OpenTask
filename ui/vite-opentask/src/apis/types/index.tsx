
export interface BaseResponse<T> {
    code: number,
    message: string,
    data: T | undefined
}

export type JsonBodyType = Record<string, any> | string | number | boolean | null | undefined;

export interface LoginRequest {
    userName: string | object | undefined,
    passWord: string | object | undefined
}

export interface LoginResponse {
    token: string,
}

