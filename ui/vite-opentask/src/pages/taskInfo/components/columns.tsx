"use client";

import { ColumnDef } from "@tanstack/react-table";
import { DataTableRowActions } from "./data-table-row-actions";
import { DataTableColumnHeader } from "../../../components/date-table/data-table-column-header";
import { TaskInfo } from "@/apis-gen/models";
import { SwitchEnable } from "./switch-status";

export const columns: ColumnDef<TaskInfo>[] = [
  {
    accessorKey: "id",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="id" />
    ),
    cell: ({ row }) => row.getValue("id"),
    enableSorting: false,
    enableHiding: false,
  },
  {
    accessorKey: "name",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="任务" />
    ),
    cell: ({ row }) => {
      return row.getValue("name");
    },
    enableSorting: false,
  },
  {
    accessorKey: "description",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="描述" />
    ),
    cell: ({ row }) => {
      return row.getValue("description");
    },
    enableSorting: false,
  },
  {
    accessorKey: "handler",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Handler" />
    ),
    cell: ({ row }) => {
      return row.getValue("handler");
    },
    enableSorting: false,
  },
  {
    accessorKey: "handleParams",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="参数" />
    ),
    cell: ({ row }) => {
      return row.getValue("handleParams");
    },
    enableSorting: false,
  },
  {
    accessorKey: "appid",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="应用" />
    ),
    cell: ({ row }) => {
      return row.getValue("appid");
    },
    enableSorting: false,
  },
  {
    accessorKey: "timeConf",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="时间表达式" />
    ),
    cell: ({ row }) => {
      return row.getValue("timeConf");
    },
    enableSorting: false,
  },
  {
    accessorKey: "enabled",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="启用状态" />
    ),
    cell: ({ row }) => {
      return (
        <SwitchEnable
          id={row.getValue<string>("id")}
          enabled={row.getValue<boolean>("enabled")}
        ></SwitchEnable>
      );
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
