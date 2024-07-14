import { DashboardApi } from "@/apis-gen";
import config from "@/apis/config";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { useQuery } from "@tanstack/react-query";
import {
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

export const TaskLogsDayTrend = () => {
  const day1 = new Date();
  day1.setDate(day1.getDate() - 10);
  const { status, data, error, isFetching } = useQuery({
    queryKey: ["taskLogsDayTrend"],
    queryFn: () =>
      new DashboardApi(config).taskLogsDayTrend({
        start: day1,
        end: new Date(),
      }),
  });

  if (status === "pending") {
    return "";
  }

  return (
    <Card x-chunk="dashboard-01-chunk-5">
      <CardHeader>
        <CardTitle>近10日调度概览</CardTitle>
      </CardHeader>
      <CardContent className="grid gap-8">
        <ResponsiveContainer width="100%" height={550}>
          <LineChart data={data?.result?.data || []}>
            <Line
              type="monotone"
              dataKey="total"
              stroke="green"
              // activeDot={{
              //   r: 6,
              //   style: { fill: "var(--primary)", opacity: 0.25 },
              // }}
              // style={
              //   {
              //     stroke: "var(--theme-primary)",
              //     opacity: 0.25,
              //   } as React.CSSProperties
              // }
              strokeWidth={2}
            />
            <Line
              type="monotone"
              dataKey="success"
              stroke="green"
              strokeWidth={2}
            />
            <Tooltip />
            <Legend />
            <XAxis dataKey="day" stroke="#888888" fontSize={12} />
            <YAxis stroke="#888888" fontSize={12} />
          </LineChart>
        </ResponsiveContainer>{" "}
      </CardContent>
    </Card>
  );
};
