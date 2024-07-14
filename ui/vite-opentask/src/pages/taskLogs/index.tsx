import { DataTable } from "@/components/date-table/data-table";
import { columns } from "./components/columns";
import { useState } from "react";
import {
  ColumnFiltersState,
  GlobalFilterOptions,
  GlobalFilterTableState,
  PaginationState,
} from "@tanstack/react-table";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { ListLogsRequest, TaskLogApi } from "@/apis-gen";
import config from "@/apis/config";
import { DataTableToolbar } from "./components/tool-bar";

export function TaskLogs() {
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  const [globalFilters, setGlobalFilters] = useState<ListLogsRequest>({});

  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);

  const { status, data, error, isFetching } = useQuery({
    queryKey: ["listLogs", pagination, columnFilters, globalFilters],
    queryFn: () => {
      return new TaskLogApi(config).listLogs({
        pageNumber: pagination.pageIndex + 1,
        pageSize: pagination.pageSize,
        taskId: globalFilters.taskId,
        startTime: globalFilters.startTime,
        endTime: globalFilters.endTime,
      });
    },
    placeholderData: keepPreviousData,
  });

  return (
    <>
      <div className="pt-2">
        {status === "pending"
          ? "Loading..."
          : error instanceof Error
          ? error.message
          : data!.result!.rows! && (
              <DataTable
                globalFilters={globalFilters}
                setGlobalFilters={setGlobalFilters}
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
