import {
  LayoutDashboardIcon,
  ListTodo,
  CalendarDays,
  CircuitBoardIcon,
  CircleAlert,
} from "lucide-react";
import { IRouter } from "..";
import { TaskInfos } from "@/pages/taskInfos/index";
import { TaskLogs } from "@/pages/taskLogs/index";
import { Clients } from "@/pages/clients/index";
import { About } from "@/pages/about/index";

export const sideRoutes: IRouter[] = [
  {
    id: "home",
    path: "/",
    async lazy() {
      const { Home } = await import("@/pages/home/index");
      return { Component: Home };
    },
    meta: {
      title: "首页",
      Icon: LayoutDashboardIcon,
      showHeader: true,
    },
  },
  {
    id: "taskinfo",
    path: "/taskinfo",
    Component: TaskInfos,
    meta: {
      title: "计划任务",
      Icon: CalendarDays,
      showHeader: true,
    }
  },
  {
    id: "tasklog",
    path: "/tasklog",
    Component: TaskLogs,
    meta: {
      title: "执行记录",
      Icon: ListTodo,
      showHeader: true,
    },
  },
  {
    id: "clients",
    path: "/clients",
    Component: Clients,
    meta: {
      title: "工作节点",
      Icon: CircuitBoardIcon,
      showHeader: true,
    },
  },
  {
    id: "about",
    path: "/about",
    Component: About,
    meta: {
      title: "关于",
      Icon: CircleAlert,
    },
  }
];
