"use client";

import { cn } from "@/lib/utils";
import { ArrowRight, LayoutDashboardIcon, LucideIcon } from "lucide-react";

import { Link, useLocation } from "react-router-dom";
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "../ui/tooltip";
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "../ui/sheet.mine";
import { ReactNode } from "react";
import { useAtom } from "jotai";
import { isMinimized } from "@/store/useSiderBar";

export interface NavItem {
  href?: string;
  title: string;
  blank?: boolean;
  icon?: LucideIcon;
  sheet?: ReactNode;
}

export interface SidebarNavProps extends React.HTMLAttributes<HTMLElement> {
  items: NavItem[];
  onlyIcon?: boolean;
}

export function SidebarNav({
  className,
  items,
  onlyIcon,
  ...props
}: SidebarNavProps) {
  const { pathname } = useLocation();
  // console.log(pathname, "pathname");
  const [minimized, setCounter] = useAtom(isMinimized);

  return (
    <nav className="grid items-start gap-2">
      <TooltipProvider>
        {items.map((item, index) => {
          return (
            item.href && (
              <Tooltip key={index}>
                <TooltipTrigger asChild>
                  <Link
                    key={item.href}
                    to={item.href ?? ""}
                    target={item.blank ? "_blank" : "_self"}
                    className={cn(
                      "flex items-center gap-2 overflow-hidden rounded-md py-2 text-sm font-medium hover:bg-accent hover:text-accent-foreground",
                      (
                        item?.href === "/"
                          ? pathname === "/"
                          : pathname.indexOf(item?.href ?? "") >= 0
                      )
                        ? "bg-accent"
                        : "transparent"
                      // item && 'cursor-not-allowed opacity-80'
                    )}
                  >
                    {item.icon && <item.icon className={`ml-3 size-5`} />}
                    {!minimized ? (
                      <span className="mr-2 truncate">{item.title}</span>
                    ) : (
                      ""
                    )}
                  </Link>
                </TooltipTrigger>
                <TooltipContent
                  align="center"
                  side="right"
                  sideOffset={8}
                  className={!minimized ? "hidden" : "inline-block"}
                >
                  {item.title}
                </TooltipContent>
              </Tooltip>
            )
          );
        })}
      </TooltipProvider>
    </nav>
  );
}
