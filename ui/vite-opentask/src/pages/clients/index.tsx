import { ColumnFiltersState, PaginationState } from "@tanstack/react-table";
import { useState } from "react";
import { columns } from "./components/columns";
import { DataTable } from "@/components/date-table/data-table";
import { ClientApi } from "@/apis-gen";
import config from "@/apis/config";
import { useQuery } from "@tanstack/react-query";

export function Clients() {
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);

  const { status, data, error, isFetching } = useQuery({
    queryKey: ["listClients"],
    queryFn: () =>
      new ClientApi(config).listClients({
        pageNumber: pagination.pageIndex + 1,
        pageSize: pagination.pageSize,
      }),
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
                toolbar={(table) => {
                  return <div></div>;
                }}
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
