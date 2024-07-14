import { DashboardApi } from "@/apis-gen";
import config from "@/apis/config";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { useQuery } from "@tanstack/react-query";

export const TaskLogTop = () => {
  const day1 = new Date();
  day1.setDate(day1.getDate() - 10);
  const { status, data, error, isFetching } = useQuery({
    queryKey: ["topTaskLog"],
    queryFn: () =>
      new DashboardApi(config).topTaskLog({
        start: day1,
        end: new Date(),
        count: 15,
      },),
  });

  if (status === "pending") {
    return "";
  }

  return (
    <Card x-chunk="dashboard-01-chunk-5">
      <CardHeader>
        <CardTitle>调度次数排名</CardTitle>
      </CardHeader>
      <CardContent className="grid gap-8">
        {data?.result?.data?.map((x) => {
          return (
            <div className="flex items-center gap-4">
              {/* <Avatar className="hidden h-9 w-9 sm:flex">
                <AvatarImage src="/avatars/01.png" alt="Avatar" />
                <AvatarFallback>OM</AvatarFallback>
              </Avatar> */}
              <div className="grid gap-1">
                <p className="text-sm font-medium leading-none">#{x.taskId} {x.name}</p>
                <p className="text-sm text-muted-foreground">{x.description}</p>
              </div>
              <div className="ml-auto font-medium">{x.count}次</div>
            </div>
          );
        })}
      </CardContent>
    </Card>
  );
};
