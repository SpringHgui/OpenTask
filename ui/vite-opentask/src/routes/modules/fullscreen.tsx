import { IRouter } from "..";

export const fullscreenRoutes: IRouter[] = [
  {
    id: "login",
    path: "/login",
    async lazy() {
      const { Login } = await import("@/pages/login/index");
      return { Component: Login };
    },
    meta: {
      requiresAuth: false,
      hidden: true,
    }
  },
  // {
  //   id: "forgot",
  //   path: "/forgot",
  //   async lazy() {
  //     const { Forgot } = await import("@/pages/login/forgot");
  //     return { Component: Forgot };
  //   },
  //   meta: {
  //     requiresAuth: false,
  //     hidden: true,
  //   }
  // }
];
