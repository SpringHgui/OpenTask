"use client";

import { ColumnDef } from "@tanstack/react-table";
import { DataTableColumnHeader } from "../../../components/date-table/data-table-column-header";
import { TaskLog } from "@/apis-gen/models";
import {
  CheckCircledIcon,
  CircleIcon,
  CrossCircledIcon,
  QuestionMarkCircledIcon,
  StopwatchIcon,
} from "@radix-ui/react-icons";
import { DataTableRowActions } from "./data-table-row-actions";
import { format } from "date-fns";
import { CircleX } from "lucide-react";

export const columns: ColumnDef<TaskLog>[] = [
  {
    accessorKey: "id",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="ID" />
    ),
    cell: ({ row }) => row.getValue("id"),
    enableSorting: false,
    enableHiding: false,
  },
  {
    accessorKey: "taskId",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="计划任务ID" />
    ),
    cell: ({ row }) => {
      return row.getValue<string>("taskId");
    },
    enableSorting: false,
  },
  {
    accessorKey: "handleStart",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="开始时间" />
    ),
    cell: ({ row }) => {
      const time = row.getValue("handleStart");
      return (
        <div>{time instanceof Date && format(time, "yyyy-MM-dd HH:mm:ss")}</div>
      );
    },
    enableSorting: false,
  },
  // {
  //   accessorKey: "endTime",
  //   header: ({ column }) => (
  //     <DataTableColumnHeader column={column} title="结束时间" />
  //   ),
  //   cell: ({ row }) => {
  //     const time = row.getValue("endTime");
  //     return (
  //       <div>{time instanceof Date && format(time, "yyyy-MM-dd HH:mm:ss")}</div>
  //     );
  //   },
  //   enableSorting: false,
  // },

  {
    accessorKey: "handleStatus",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="状态" />
    ),
    cell: ({ row }) => {
      const status = statuses.find(
        (status) => status.value == row.getValue("handleStatus")
      );

      if (!status) {
        return null;
      }

      return (
        <div className="flex w-[100px] items-center">
          {status.icon && (
            <status.icon className="mr-2 h-4 w-4 text-muted-foreground" />
          )}
          <span>{status.label}</span>
        </div>
      );
    },
    enableSorting: false,
  },
  {
    accessorKey: "handleResult",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="结果" />
    ),
    cell: ({ row }) => {
      return row.getValue("handleResult");
    },
    enableSorting: false,
  },
  {
    accessorKey: "handleClient",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="执行节点" />
    ),
    cell: ({ row }) => {
      return row.getValue("handleClient");
    },
    enableSorting: false,
  },
  {
    enableSorting: false,
    id: "actions",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="操作" />
    ),
    cell: ({ row }) => <DataTableRowActions row={row} />,
  },
];

export const statuses = [
  {
    value: "",
    label: "未知",
    icon: QuestionMarkCircledIcon,
  },
  {
    value: "0",
    label: "待执行",
    icon: CircleIcon,
  },
  {
    value: "1",
    label: "执行中",
    icon: StopwatchIcon,
  },
  {
    value: "2",
    label: "完成",
    icon: CheckCircledIcon,
  },
  {
    value: "3",
    label: "失败",
    icon: CircleX,
  },
  {
    value: "canceled",
    label: "取消",
    icon: CrossCircledIcon,
  },
];
