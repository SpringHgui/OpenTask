import { DashboardApi } from "@/apis-gen";
import config from "@/apis/config";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { useQuery } from "@tanstack/react-query";
import { ClipboardList, Boxes, Router, Server } from "lucide-react";

export const BaseStatistics = () => {
  const { status, data, error, isFetching } = useQuery({
    queryKey: ["statistics"],
    queryFn: () => new DashboardApi(config).statistics({}),
  });

  if (status === "pending") {
    return "Loading...";
  }

  return (
    <div className="grid gap-4 md:grid-cols-2 md:gap-8 lg:grid-cols-4">
      <Card x-chunk="dashboard-01-chunk-0">
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">任务数量</CardTitle>
          <ClipboardList className="h-4 w-4 text-muted-foreground" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">
            {data?.result?.taskInfos?.count}
          </div>
          {/* <p className="text-xs text-muted-foreground">
          +20.1% from last month
        </p> */}
        </CardContent>
      </Card>
      <Card x-chunk="dashboard-01-chunk-1">
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">调度次数</CardTitle>
          <Boxes className="h-4 w-4 text-muted-foreground" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">
            {data?.result?.taskLogs?.count}
          </div>
          {/* <p className="text-xs text-muted-foreground">
          +180.1% from last month
        </p> */}
        </CardContent>
      </Card>
      <Card x-chunk="dashboard-01-chunk-2">
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">执行器数</CardTitle>
          <Router className="h-4 w-4 text-muted-foreground" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">
            {data?.result?.workers?.count}
          </div>
          {/* <p className="text-xs text-muted-foreground">
          +19% from last month
        </p> */}
        </CardContent>
      </Card>
      <Card x-chunk="dashboard-01-chunk-3">
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">节点数量</CardTitle>
          <Server className="h-4 w-4 text-muted-foreground" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">
            {" "}
            {data?.result?.servers?.count}
          </div>
          {/* <p className="text-xs text-muted-foreground">
          +201 since last hour
        </p> */}
        </CardContent>
      </Card>
    </div>
  );
};
