import { User } from "@/pages/user";
import { IRouter } from "..";
import {
  CircleUser,
  Settings,
} from "lucide-react";
import { About } from "@/pages/about/index";

const hidden: IRouter[] = [
  {
    id: "setting",
    path: "/setting",
    Component: About,
    meta: {
      title: "设置中心",
      Icon: Settings,
    },
  },
  {
    id: "user",
    path: "/user",
    meta: {
      title: "个人",
      Icon: CircleUser,
    },
    element: <User></User>,
  },
];

export default hidden;
