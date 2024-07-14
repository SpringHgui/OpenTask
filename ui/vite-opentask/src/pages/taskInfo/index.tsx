import { DataTable } from "../../components/date-table/data-table";
import { columns } from "./components/columns";
import { NewTaskForm } from "./components/new-task-form";
import { useState } from "react";
import { ColumnFiltersState, PaginationState } from "@tanstack/react-table";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { TaskInfoApi } from "@/apis-gen";
import config from "@/apis/config";
import { DataTableToolbar } from "./components/tool-bar";

export function TaskInfos() {
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
  const { status, data, error } = useQuery({
    queryKey: ["listTaskInfos", pagination, columnFilters],
    queryFn: () =>
      new TaskInfoApi(config).listTaskInfos({
        pageNumber: pagination.pageIndex + 1,
        pageSize: pagination.pageSize,
        name: columnFilters.find((x) => x.id == "name")?.value as string,
      }),
    placeholderData: keepPreviousData,
  });

  return (
    <>
      <div className="flex justify-between">
        <NewTaskForm></NewTaskForm>
      </div>

      <div className="pt-2">
        {status === "pending" ? (
          "Loading..."
        ) : error instanceof Error ? (
          error.message
        ) : (
          <DataTable
            toolbar={DataTableToolbar}
            columnFilters={columnFilters}
            setColumnFilters={setColumnFilters}
            pagination={pagination}
            onPaginationChange={setPagination}
            data={data!.result!.rows!}
            total={parseInt(data!.result!.count!)}
            columns={columns}
          />
        )}
      </div>
    </>
  );
}
