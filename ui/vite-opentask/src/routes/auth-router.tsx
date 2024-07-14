import { searchRouteV2 } from "@/routes/utils";
import { useLocation, Navigate, useMatches } from "react-router-dom";
import { allRoutes } from '@/routes';
import { useAtom } from "jotai";
import { currentUserAtom } from "@/store";
import { ReactElement, memo } from "react";
import { Breadcrumb, BreadcrumbItem, BreadcrumbLink, BreadcrumbList, BreadcrumbPage, BreadcrumbSeparator } from "@/components/ui/breadcrumb";

function Route(props: { children: ReactElement<any, any> }) {
	const { pathname } = useLocation();
	const matches = useMatches();
	const [currentUser] = useAtom(currentUserAtom);

	const route = searchRouteV2(matches, allRoutes);
	console.log("[Auth]", pathname, route, matches);

	if (route == undefined) {
		// 没有找到默认走权限
		if (pathname != '/') {
			console.error("[未匹配的路由]", pathname);
		}
	} else {
		// axiosCanceler.removeAllPending();
		if (!(route.meta?.requiresAuth ?? true)) {
			return props.children;
		}
	}

	if (currentUser) {
		// if (pathname == "/") {
		// 	return <Navigate to="/home" replace />;
		// }

		return <div>
			{route?.path != "/" && <Breadcrumb className="pb-8">
				<BreadcrumbList>
					<BreadcrumbItem>
						<BreadcrumbLink href="/">首页</BreadcrumbLink>
					</BreadcrumbItem>
					<BreadcrumbSeparator />
					<BreadcrumbItem>
						<BreadcrumbLink href={route?.path}>{route?.meta.title}</BreadcrumbLink>
					</BreadcrumbItem>
				</BreadcrumbList>
			</Breadcrumb>}
			<div id="page">
				{props.children}
			</div>
		</div>;
	} else {
		return <Navigate to="/login" replace />;
	}
}

const AuthRouter = memo(Route, () => { return true });

export default AuthRouter;