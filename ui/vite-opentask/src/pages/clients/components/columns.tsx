"use client";

import { ColumnDef } from "@tanstack/react-table";
import { DataTableColumnHeader } from "../../../components/date-table/data-table-column-header";
import { ExecutorClient } from "@/apis-gen/models";
import { DataTableRowActions } from "./data-table-row-actions";
import {
  HoverCard,
  HoverCardContent,
  HoverCardTrigger,
} from "@/components/ui/hover-card";
import { Button } from "@/components/ui/button";

export const columns: ColumnDef<ExecutorClient>[] = [
  {
    accessorKey: "clientId",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="clientId" />
    ),
    cell: ({ row }) => row.getValue("clientId"),
    enableSorting: false,
    enableHiding: false,
  },
  {
    accessorKey: "groupName",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="AppName" />
    ),
    cell: ({ row }) => {
      return row.getValue<string>("groupName");
    },
    enableSorting: false,
  },
  {
    accessorKey: "startTime",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="连接时间" />
    ),
    cell: ({ row }) => {
      const time = row.getValue("startTime");
      return <div>{time instanceof Date && time.toDateString()}</div>;
    },
    enableSorting: false,
  },
  {
    accessorKey: "handelrs",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="handelrs" />
    ),
    cell: ({ row }) => {
      const handelrs = row.getValue("handelrs") as string[];

      return (
        <HoverCard>
          <HoverCardTrigger asChild>
            <Button variant="link">共{handelrs?.length}个handelrs</Button>
          </HoverCardTrigger>
          <HoverCardContent className="w-80">
            <div className="">
              {handelrs?.map((y) => {
                return <div>{y}</div>;
              })}
            </div>
          </HoverCardContent>
        </HoverCard>
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
