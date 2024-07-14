import { Label } from "@/components/ui/label";
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet.mine";

import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { toast } from "sonner";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { useState } from "react";
import { Step, StepItem, Stepper, useStepper } from "@/components/ui/stepper";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import { TaskInfo, TaskInfoApi } from "@/apis-gen";
import config from "@/apis/config";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import { Button } from "@/components/ui/button";
import { Icons } from "@/components/icons";
import { PlusCircle } from "lucide-react";
import { Link } from "react-router-dom";

const steps = [
  { label: "基本信息", description: "基本信息" },
  { label: "时间配置", description: "时间配置" },
  { label: "报警配置", description: "报警配置" },
] satisfies StepItem[];

const taskSchema = z
  .object({
    appid: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
    name: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
    handler: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
    description: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
    timeType: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
    timeConf: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
    attemptInterval: z.number(),
    attemptMax: z.number().max(10).default(2),
    handleParams: z.string(),
    scheduleMode: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
    alarmType: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
    alarmConf: z.string().min(2, {
      message: "Username must be at least 2 characters.",
    }),
  })
  .partial();

const requiredSchema = taskSchema.required({
  name: true,
  appid: true,
  handler: true,
  timeConf: true,
});

export const NewTaskForm = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [open, setOpen] = useState<boolean>(false);

  const form = useForm<z.infer<typeof taskSchema>>({
    resolver: zodResolver(requiredSchema),
    defaultValues: {
      appid: "",
      name: "",
      timeType: "cron",
      alarmType: "none",
      scheduleMode: "alone",
    },
  });

  const mutation = useMutation({
    mutationFn: (taskInfo: TaskInfo) => {
      setIsLoading(true);
      return new TaskInfoApi(config).addTaskInfo({
        addTaskRequest: {
          task: taskInfo,
        },
      });
    },
    onSuccess: (data) => {
      if (!data.success) {
        toast.error(data.message);
        return;
      }

      toast.success("创建成功");
      setOpen(false);
    },
    onError: (error) => {
      console.log(error);
      toast.error(error.message);
    },
    onSettled: () => {
      setIsLoading(false);
    },
  });

  function onSubmit(data: z.infer<typeof taskSchema>) {
    mutation.mutate(data as TaskInfo);
  }

  const alarmType = form.watch("alarmType");

  return (
    <>
      <Button
        size="sm"
        className="h-8 gap-1"
        onClick={() => {
          setOpen(true);
        }}
      >
        <PlusCircle className="h-3.5 w-3.5" />
        <span className="sr-only sm:not-sr-only sm:whitespace-nowrap">
          创建任务
        </span>
      </Button>
      <Sheet open={open} onOpenChange={setOpen}>
        <SheetContent className="w-1/3 overflow-y-auto">
          <SheetHeader>
            <SheetTitle>创建计划任务</SheetTitle>
            <SheetDescription>创建计划任务</SheetDescription>
          </SheetHeader>
          <Stepper variant="circle-alt" initialStep={0} size="sm" steps={steps}>
            {steps.map((stepProps, index) => {
              return (
                <Step key={stepProps.label} {...stepProps}>
                  <Form {...form}>
                    <form
                      onSubmit={form.handleSubmit(onSubmit)}
                      className="w-2/3 space-y-6"
                    >
                      {index === 0 && (
                        <>
                          <FormField
                            control={form.control}
                            name="name"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>任务名称</FormLabel>
                                <FormControl>
                                  <Input placeholder="任务名称" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={form.control}
                            name="appid"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>所属应用</FormLabel>
                                <FormControl>
                                  <Input placeholder="应用名" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={form.control}
                            name="handler"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Handler</FormLabel>
                                <FormControl>
                                  <Input placeholder="Handler" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={form.control}
                            name="description"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>描述</FormLabel>
                                <FormControl>
                                  <Textarea
                                    placeholder="描述你的任务"
                                    {...field}
                                  />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={form.control}
                            name="scheduleMode"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>执行模式</FormLabel>
                                <FormControl>
                                  <RadioGroup
                                    onValueChange={field.onChange}
                                    defaultValue={field.value}
                                    className="flex flex-col space-y-1"
                                  >
                                    <FormItem className="flex items-center space-x-3 space-y-0">
                                      <FormControl>
                                        <RadioGroupItem value="alone" />
                                      </FormControl>
                                      <FormLabel className="font-normal">
                                        单节点
                                      </FormLabel>
                                    </FormItem>
                                    <FormItem className="flex items-center space-x-3 space-y-0">
                                      <FormControl>
                                        <RadioGroupItem
                                          disabled
                                          value="broadcast"
                                        />
                                      </FormControl>
                                      <FormLabel className="font-normal">
                                        广播/分片
                                      </FormLabel>
                                    </FormItem>
                                  </RadioGroup>
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={form.control}
                            name="handleParams"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>运行参数</FormLabel>
                                <FormControl>
                                  <Textarea placeholder="运行参数" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                        </>
                      )}

                      {index === 1 && (
                        <>
                          <FormField
                            control={form.control}
                            name="timeType"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>时间类型</FormLabel>
                                <FormControl>
                                  <Select
                                    onValueChange={field.onChange}
                                    defaultValue={field.value}
                                  >
                                    <SelectTrigger className="w-[180px]">
                                      <SelectValue placeholder="选择时间类型" />
                                    </SelectTrigger>
                                    <SelectContent>
                                      <SelectGroup>
                                        <SelectItem value="cron">
                                          crontab
                                        </SelectItem>
                                      </SelectGroup>
                                    </SelectContent>
                                  </Select>
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={form.control}
                            name="timeConf"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>时间表达式</FormLabel>{" "}
                                <Link
                                  className="underline underline-offset-4 hover:text-primary"
                                  to={"https://cron.ciding.cc/"}
                                  target="_blank"
                                >
                                  在线生成 crontab{" "}
                                </Link>
                                <FormControl>
                                  <Input
                                    placeholder="crontab 时间表达式"
                                    {...field}
                                  />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                        </>
                      )}
                      {index === 2 && (
                        <>
                          <FormField
                            control={form.control}
                            name="alarmType"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>报警方式</FormLabel>
                                <FormControl>
                                  <Select
                                    onValueChange={field.onChange}
                                    defaultValue={field.value}
                                  >
                                    <SelectTrigger className="w-[180px]">
                                      <SelectValue placeholder="选择报警方式" />
                                    </SelectTrigger>
                                    <SelectContent>
                                      <SelectGroup>
                                        <SelectItem value="none">
                                          不报警
                                        </SelectItem>
                                        <SelectItem value="wxwork">
                                          企业微信
                                        </SelectItem>
                                      </SelectGroup>
                                    </SelectContent>
                                  </Select>
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          {alarmType === "wxwork" && (
                            <FormField
                              control={form.control}
                              name="alarmConf"
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel>webhook</FormLabel>
                                  <FormControl>
                                    <Input placeholder="webhook" {...field} />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                          )}
                        </>
                      )}

                      <StepButtons isLoading={isLoading} />
                    </form>
                  </Form>
                </Step>
              );
            })}
          </Stepper>
        </SheetContent>
      </Sheet>
    </>
  );
};

const StepButtons = ({ isLoading }: { isLoading: boolean }) => {
  const { nextStep, prevStep, isLastStep, isOptionalStep, isDisabledStep } =
    useStepper();

  return (
    <div className="w-full flex gap-2 p-3">
      <Button
        disabled={isDisabledStep}
        onClick={prevStep}
        size="sm"
        variant="secondary"
      >
        上一步
      </Button>

      {isLastStep ? (
        <Button size="sm" type="submit">
          {isLoading && <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />}
          提交
        </Button>
      ) : (
        <Button size="sm" onClick={nextStep}>
          {isOptionalStep ? "跳过" : "下一步"}
        </Button>
      )}
    </div>
  );
};
