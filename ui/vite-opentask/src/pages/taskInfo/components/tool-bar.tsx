import { DataTableToolbarProps } from "@/components/date-table/data-table-toolbar";
import { DataTableViewOptions } from "@/components/date-table/data-table-view-options";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Cross2Icon } from "@radix-ui/react-icons";
import { useState } from "react";

export function DataTableToolbar<TData>({
  table,
}: DataTableToolbarProps<TData>) {
  const isFiltered = table.getState().columnFilters.length > 0;
  const [name, setName] = useState(
    (table.getColumn("name")?.getFilterValue() as string) ?? ""
  );

  const onSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    console.log('ssss');
    
    const formData = new FormData(event.currentTarget);
    table.getColumn("name")?.setFilterValue(formData.get("name"));
  };

  const reset = () => {
    table.resetColumnFilters();
    setName("");
  };
  return (
    <div className="flex items-center justify-between">
      <form onSubmit={onSubmit}>
        <div className="ml-auto flex w-full space-x-2 sm:justify-end">
          <div className=" ">
            <Input
              placeholder="任务名称"
              type="text"
              name="name"
              autoCapitalize="none"
              autoComplete="false"
              autoCorrect="off"
              onChange={(e) => {
                const val = e.target.value;
                setName(val);
              }}
              value={name}
            />
          </div>
          <Button>查 询</Button>

          {isFiltered && (
            <Button
              variant="ghost"
              onClick={reset}
              className="h-8 px-2 lg:px-3"
            >
              Reset
              <Cross2Icon className="ml-2 h-4 w-4" />
            </Button>
          )}
        </div>
      </form>

      {/* <DataTableViewOptions table={table} /> */}
    </div>
  );
}
