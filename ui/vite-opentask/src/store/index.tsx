import { atom } from 'jotai'
import { atomWithStorage } from 'jotai/utils'
import { jwtDecode } from "@/lib/jwt-decode";


export const tokenAtom = atomWithStorage("token", '', undefined, { getOnInit: true })

export interface User {
    UserName: string,
    UserId: number,
}

export const currentUserAtom = atom<User | undefined>((get) => {
    const token = get(tokenAtom);
    if (token && token.length > 0) {
        const decoded = jwtDecode(token);
        const expiration = new Date(decoded.exp! * 1000);
        const fiveMinutes = 1000 * 60 * 5;
        if (expiration.getTime() - new Date().getTime() < fiveMinutes) {
            if (import.meta.env.MODE !== 'mock') {
                return undefined;
            }
        }

        const user = JSON.parse((decoded as any)["User"]);
        return user as User
    }

    return undefined;
})

export const isFullPageAtom = atomWithStorage("isFullPage", "side")