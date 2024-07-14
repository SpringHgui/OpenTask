import { queryClient } from "@/apis";
import { TaskInfo, TaskInfoApi } from "@/apis-gen";
import config from "@/apis/config";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { toast } from "sonner";

export const SwitchEnable = ({
  enabled,
  id,
}: {
  enabled: boolean;
  id: string;
}) => {
  const [open, setOpen] = useState<boolean>(enabled);

  const mutation = useMutation({
    mutationFn: (task: TaskInfo) => {
      return new TaskInfoApi(config).switchTaskStatus({
        switchTaskStatusRequest: {
          taskId: task.id,
          enabled: !task.enabled,
        },
      });
    },
    onSuccess: (data) => {
      if (!data.success) {
        toast.error(data.message);
        return;
      }

      setOpen(!open);
      queryClient.invalidateQueries({ queryKey: ["listTaskInfos"] });
    },
    onError: (error) => {
      console.log(error);
      toast.error(error.message);
    },
    onSettled: () => {},
  });

  return (
    <div className="flex items-center space-x-2">
      <Switch
        id="airplane-mode"
        checked={open}
        onCheckedChange={() => {
          mutation.mutate({ id, enabled });
        }}
      />
      <Label htmlFor="airplane-mode">{open ? "已启用" : "已禁用"}</Label>
    </div>
  );
};
