"use client"

import { cn } from "@/lib/utils"
import { Tooltip, TooltipContent, TooltipTrigger } from "../ui/tooltip"
import { Link, useLocation } from "react-router-dom"
import { buttonVariants } from "../ui/button"
import { LucideIcon } from "lucide-react"
import { MouseEventHandler } from "react"
import { Item } from "@radix-ui/react-dropdown-menu"

export interface LinkInfo {
  title: string
  label?: string
  icon: LucideIcon
  // variant: "secondary" | "ghost",
  Component?: JSX.Element,
  to?: string,
  onClick?: MouseEventHandler | undefined;
}

export interface NavProps {
  isCollapsed: boolean
  links: LinkInfo[]
}

export function Nav({ links, isCollapsed }: NavProps) {
  const { pathname } = useLocation()
  console.log("Nav", pathname);
  let actives = links.filter(x => x.to).filter(x => pathname.endsWith(x.to!))
  let active: LinkInfo | undefined = undefined;
  if (actives.length > 0) {
    active = actives[0];
  } else {
    active = links[0]
  }

  return (
    <div
      data-collapsed={isCollapsed}
      style={{ boxShadow: "rgba(9, 30, 66, 0.05) 0px 2px 8px" }}
      className="group flex flex-col gap-4 py-2 data-[collapsed=true]:py-2 h-full"
    >
      <nav className="grid gap-1 px-2 group-[[data-collapsed=true]]:justify-center group-[[data-collapsed=true]]:px-2">
        {links.map((link, index) =>
          isCollapsed ? (
            <Tooltip key={index} delayDuration={0}>
              <TooltipTrigger asChild>
                <Link
                  to={link.to ?? ""}
                  className={cn(
                    active == link ? buttonVariants({ variant: "secondary", size: "icon" }) : buttonVariants({ variant: "ghost", size: "icon" }),
                    "h-9 w-9",
                    // link.variant === "secondary" &&
                    "dark:bg-muted dark:text-muted-foreground dark:hover:bg-muted dark:hover:text-white"
                  )}
                >
                  <link.icon className="h-4 w-4" />
                  <span className="sr-only">{link.title}</span>
                </Link>
              </TooltipTrigger>
              <TooltipContent side="right" className="flex items-center gap-4">
                {link.title}
                {link.label && (
                  <span className="ml-auto text-muted-foreground">
                    {link.label}
                  </span>
                )}
              </TooltipContent>
            </Tooltip>
          ) : (
            link.to || link.to == "" ?
              <Link
                key={index}
                to={link.to ?? ""}
                className={cn(
                  active == link ? buttonVariants({ variant: "secondary", size: "sm" }) : buttonVariants({ variant: "ghost", size: "sm" }),
                  "dark:bg-muted dark:text-white dark:hover:bg-muted dark:hover:text-white",
                  "justify-start"
                )}
              >
                <link.icon className="mr-2 h-4 w-4" />
                {link.title}
                {link.label && (
                  <span
                    className={cn(
                      "ml-auto",
                      "text-background dark:text-white"
                    )}
                  >
                    {link.label}
                  </span>
                )}
              </Link> : <div style={{ cursor: "pointer" }} key={index} onClick={link.onClick} className={cn(
                buttonVariants({ variant: "ghost", size: "sm" }),
                "dark:bg-muted dark:text-white dark:hover:bg-muted dark:hover:text-white",
                "justify-start"
              )}>
                <link.icon className="mr-2 h-4 w-4" />
                {link.title}
                {link.label && (
                  <span
                    className={cn(
                      "ml-auto",
                      "text-background dark:text-white"
                    )}
                  >
                    {link.label}
                  </span>
                )}
              </div>
          )
        )}
      </nav>
    </div>
  )
}
