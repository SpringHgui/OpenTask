import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { BaseStatistics } from "./components/base-statistics";
import { TaskLogsDayTrend } from "./components/tasklog-daytrend";

import { Avatar, AvatarImage, AvatarFallback } from "@radix-ui/react-avatar";
import { TaskLogTop } from "./components/tasklog-top";

export function Home() {
  return (
    <div>
      <BaseStatistics></BaseStatistics>

      <div className=" pt-2 grid gap-4 md:gap-8 lg:grid-cols-2 xl:grid-cols-3">
        <div className="xl:col-span-2">
          <TaskLogsDayTrend></TaskLogsDayTrend>
        </div>
        <div>
          <TaskLogTop></TaskLogTop>
        </div>
      </div>
    </div>
  );
}
