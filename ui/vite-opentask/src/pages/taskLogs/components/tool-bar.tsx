import { DataTableToolbarProps } from "@/components/date-table/data-table-toolbar";
import { DataTableViewOptions } from "@/components/date-table/data-table-view-options";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import { Input } from "@/components/ui/input";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { cn } from "@/lib/utils";
import { CalendarIcon, Cross2Icon } from "@radix-ui/react-icons";
import { addDays, format } from "date-fns";
import { useState } from "react";
import { DateRange } from "react-day-picker";
import { da, zhCN } from "date-fns/locale";
import { ListLogsRequest } from "@/apis-gen";
import { queryClient } from "@/apis";

export function DataTableToolbar<TData>({
  table,
}: DataTableToolbarProps<TData>) {
  const isFiltered = table.getState().globalFilter?.taskId;
  const [taskID, setTaskId] = useState(
    (table.getState().globalFilter?.taskId as string) ?? ""
  );

  const [date, setDate] = useState<DateRange | undefined>({
    from: addDays(new Date(), -1),
    to: addDays(new Date(), 0),
  });

  const onSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    queryClient.invalidateQueries({ queryKey: ["listLogs"] });
    table.setPageIndex(0);

    const filters: ListLogsRequest = {
      taskId: taskID,
      startTime: date?.from,
      endTime: date?.to,
    };

    table.setGlobalFilter(filters);
  };

  const reset = () => {
    table.resetGlobalFilter();
    setTaskId("");
  };
  return (
    <div className="flex items-center justify-between">
      <form onSubmit={onSubmit}>
        <div className="ml-auto flex w-full space-x-2 sm:justify-end">
          <div className=" ">
            <Input
              placeholder="计划任务ID"
              // type="number"
              name="taskId"
              autoCapitalize="none"
              autoComplete="false"
              autoCorrect="off"
              onChange={(e) => {
                const val = e.target.value;
                setTaskId(val);
              }}
              value={taskID}
            />
          </div>

          <div>
            <div className={cn("grid gap-2")}>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    id="date"
                    variant={"outline"}
                    className={cn(
                      "w-[300px] justify-start text-left font-normal",
                      !date && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {date?.from ? (
                      date.to ? (
                        <>
                          {format(date.from, "yyyy-MM-dd")} ~{" "}
                          {format(date.to, "yyyy-MM-dd")}
                        </>
                      ) : (
                        format(date.from, "yyyy-MM-dd")
                      )
                    ) : (
                      <span>调度时间</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <Calendar
                    initialFocus
                    mode="range"
                    locale={zhCN}
                    selected={date}
                    onSelect={setDate}
                    numberOfMonths={2}
                  />
                </PopoverContent>
              </Popover>
            </div>
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
