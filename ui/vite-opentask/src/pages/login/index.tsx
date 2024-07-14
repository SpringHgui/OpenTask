import { cn } from "@/lib/utils"
import { buttonVariants } from "@/components/ui/button"
import { UserAuthForm } from "./components/user-auth-form"
import { Link } from "react-router-dom"
import { delay } from "msw";
import { Logo } from "@/components/logo";
import Footer from '@/layout/app-footer'

export function Login() {

    return (
        <>
            <div className="container relative  h-full flex-col items-center justify-center md:grid lg:max-w-none lg:grid-cols-2 lg:px-0">
                {/* <Link
                    to="/examples/authentication"
                    className={cn(
                        buttonVariants({ variant: "ghost" }),
                        "absolute right-4 top-4 md:right-8 md:top-8"
                    )}
                >
                    Login
                </Link> */}
                <div className="relative hidden h-full flex-col bg-muted p-10 text-white lg:flex dark:border-r">
                    <div className="absolute inset-0 bg-zinc-900" />
                    <Logo></Logo>
                    {/* <p className="text-white relative z-20 flex items-center text-sm font-medium pt-5">分布式任务调度系统</p> */}
                    <div className="relative z-20 mt-auto">
                        <blockquote className="space-y-2">
                            <p className="text-lg">
                                古人学问无遗力，少壮工夫老始成。</p>
                            <p className="text-lg">
                                纸上得来终觉浅，绝知此事要躬行。
                            </p>
                            <footer className="text-sm">《冬夜读书示子聿》 陆游</footer>
                        </blockquote>
                    </div>
                </div>
                <div className="lg:p-8 flex flex-col justify-between">
                    <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
                        <div className="flex flex-col space-y-2 text-center">
                            <h1 className="text-2xl font-semibold tracking-tight">
                                登录
                            </h1>
                            <p className="text-sm text-muted-foreground">
                                输入你的账号进行登录
                            </p>
                        </div>
                        <UserAuthForm />
                        {/* <p className="px-8 text-center text-sm text-muted-foreground">
                            By clicking continue, you agree to our{" "}
                            <Link
                                to={"/terms"}
                                className="underline underline-offset-4 hover:text-primary"
                            >
                                Terms of Service
                            </Link>{" "}
                            and{" "}
                            <Link
                                to={"/privacy"}
                                className="underline underline-offset-4 hover:text-primary"
                            >
                                Privacy Policy
                            </Link>
                            .
                            
                        </p> */}

                    </div>
                    <Footer></Footer>
                </div>
            </div >
        </>
    )
}
