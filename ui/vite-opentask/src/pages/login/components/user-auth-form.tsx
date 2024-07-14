import * as React from "react";
import { cn } from "@/lib/utils";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Link, useNavigate } from "react-router-dom";

import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";
import { useAtom } from "jotai";
import { tokenAtom } from "@/store";
import { Icons } from "@/components/icons";
import { UserApi } from "@/apis-gen";
import config from "@/apis/config";

interface UserAuthFormProps extends React.HTMLAttributes<HTMLDivElement> {}

export function UserAuthForm({ className, ...props }: UserAuthFormProps) {
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  let [token, setToken] = useAtom(tokenAtom);

  const navigate = useNavigate();
  const mutation = useMutation({
    mutationFn: (formData: FormData) => {
      const name =
        formData.get("name")?.valueOf().toString() ??
        (() => {
          throw new Error("Name is not defined");
        })();
      const password =
        formData.get("password")?.valueOf().toString() ??
        (() => {
          throw new Error("Password is not defined");
        })();
      setIsLoading(true);
      return new UserApi(config).login({
        loginCommand: { userName: name, password: password },
      });
    },
    onSuccess: (data) => {
      console.log("登录成功", data);
      if (!data.success) {
        toast.error(data.message);
        return;
      }

      if (!data?.result?.token) {
        toast.error("返回值异常");
        return;
      }

      setToken(data.result.token);
      toast.success("登录成功");
      navigate(`/`, { replace: true });
    },
    onError: (error) => {
      console.log(error);

      toast.error(error.message);
    },
    onSettled: () => {
      setIsLoading(false);
    },
  });

  const onSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    mutation.mutate(formData);
  };

  // const add = (e: React.FormEvent<HTMLFormElement>) => {
  //   e.preventDefault()
  //   const title = e.currentTarget.inputTitle.value
  //   e.currentTarget.inputTitle.value = ''

  //   setTodos((prev) => [...prev, atom<Todo>({ title, completed: false })])
  // }

  return (
    <div className={cn("grid gap-6", className)} {...props}>
      <form onSubmit={onSubmit}>
        <div className="grid gap-2">
          <div className="grid gap-1">
            <Label htmlFor="email">账号</Label>
            <Input
              id="email"
              placeholder="账号"
              type="text"
              name="name"
              autoCapitalize="none"
              autoComplete="账号"
              autoCorrect="off"
              disabled={isLoading}
            />
          </div>
          <div className="grid gap-1">
            <Label htmlFor="password">密码</Label>
            <Link
              to="/forgot"
              className="ml-auto inline-block text-sm underline"
            >
              忘记密码？
            </Link>

            <Input
              id="password"
              name="password"
              type="password"
              placeholder="Password"
              autoCapitalize="none"
              autoComplete="password"
              autoCorrect="off"
              disabled={isLoading}
            />
          </div>
          <Button disabled={isLoading}>
            {isLoading && (
              <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
            )}
            登 录
          </Button>
        </div>
      </form>
      {/* <div className="relative">
        <div className="absolute inset-0 flex items-center">
          <span className="w-full border-t" />
        </div>
        <div className="relative flex justify-center text-xs uppercase">
          <span className="bg-background px-2 text-muted-foreground">
            Or continue with
          </span>
        </div>
      </div>
      <Button variant="outline" type="button" disabled={isLoading}>
        {isLoading ? (
          <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
        ) : (
          <Icons.gitHub className="mr-2 h-4 w-4" />
        )}{" "}
        Github
      </Button> */}
    </div>
  );
}
