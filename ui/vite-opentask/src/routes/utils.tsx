import { UIMatch } from "react-router";
import { IRouter } from ".";

export const searchRoute = (path: string, routes?: IRouter[]): IRouter | undefined => {
    let result: IRouter | undefined = undefined;
    if (routes == null) {
        return undefined;
    }

    for (let item of routes) {
        if (item.path === path)
            return item;

        if (item.children) {
            const res = searchRoute(path, item.children);
            if (res != undefined) {
                if (Object.keys(res).length)
                    result = res;
            }

        }
    }

    return result;
};

export const searchRouteV2 = (matchs: UIMatch[], routes: IRouter[]): IRouter | undefined => {
    if (routes == null) {
        return undefined;
    }

    let id = "";
    if (matchs.length == 1) {
        return undefined;
    }
    else if (matchs.length == 2) {
        id = matchs[1].id;

        for (let item of routes) {
            if (item.id === id)
                return item;
        }
        return undefined;
    } else if (matchs.length == 3) {
        id = matchs[2].id;
        for (let item of routes) {
            if (item.children) {
                for (let child of item.children) {
                    if (child.id === id)
                        return item;
                }
            }
        }

        return undefined;
    } else {
        // 目前没有这种层级的目录
        return undefined;
    }
};