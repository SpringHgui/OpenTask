import { NonIndexRouteObject, createBrowserRouter } from 'react-router-dom';

import { bottomRoutes } from './modules/bottom';
import hidden from './modules/hidden';
import { App } from '@/App';
import { LucideIcon } from 'lucide-react';
import { sideRoutes } from './modules/side';
import { fullscreenRoutes } from './modules/fullscreen';
import { FullPageLayout } from '@/layout/app-layout';
import { _404 } from '@/pages/error/_404';
import ErrorPage from './error-page';

export interface IRouter extends NonIndexRouteObject {
  meta: {
    title?: string,
    requiresAuth?: boolean,
    hidden?: boolean,
    Icon?: LucideIcon,
    showHeader?: boolean,
    blank?: boolean,
  },
  children?: IRouter[]
}

export const routesWithLayout = [...sideRoutes, ...bottomRoutes, ...hidden];

export const router = createBrowserRouter(
  [
    {
      path: "/",
      element: <App />,
      children: [
        ...routesWithLayout],
        // errorElement: <ErrorPage></ErrorPage>
    },
    {
      path: "/",
      element: <FullPageLayout />,
      children: fullscreenRoutes,
    },
    {
      path: "*",
      element: <_404></_404>
    }
  ],
  {
    basename: "/",
    future: {
      v7_normalizeFormMethod: true,
      v7_fetcherPersist: true,
      v7_relativeSplatPath: true
    },
  }
);

export const allRoutes = [...routesWithLayout, ...fullscreenRoutes];
