"use client";

import { Cross2Icon } from "@radix-ui/react-icons";

import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Table } from "@tanstack/react-table";
import { DataTableFacetedFilter } from "./data-table-faceted-filter";

import { DataTableViewOptions } from "./data-table-view-options";

export interface DataTableToolbarProps<TData> {
  table: Table<TData>;
}
