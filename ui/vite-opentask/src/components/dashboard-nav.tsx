'use client';

import { Icons } from '@/components/icons';
import { cn } from '@/lib/utils';
import { Dispatch, SetStateAction } from 'react';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger
} from './ui/tooltip';
import { atom, useAtom } from 'jotai';
import { NavItem } from './sidebar/sidebar';
import { isMinimized } from '@/store/useSiderBar';
import { Link, useLocation } from 'react-router-dom';

interface DashboardNavProps {
  items: NavItem[];
  setOpen?: Dispatch<SetStateAction<boolean>>;
  isMobileNav?: boolean;
}

export function DashboardNav({
  items,
  setOpen,
  isMobileNav = false
}: DashboardNavProps) {
  // const path = usePathname();
  const { pathname } = useLocation()
  const [minimized, handleToggle] = useAtom(isMinimized);

  if (!items?.length) {
    return null;
  }

  console.log('isActive', isMobileNav, isMinimized);

  return (
    <nav className="grid items-start gap-2">
      <TooltipProvider>
        {items.map((item, index) => {
          const Icon = item.icon;
          return (
            item.href && (
              <Tooltip key={index}>
                <TooltipTrigger asChild>
                  <Link
                    to={item.href}
                    className={cn(
                      'flex items-center gap-2 overflow-hidden rounded-md py-2 text-sm font-medium hover:bg-accent hover:text-accent-foreground',
                      pathname === item.href ? 'bg-accent' : 'transparent',
                      // item.disabled && 'cursor-not-allowed opacity-80'
                    )}
                    onClick={() => {
                      if (setOpen) setOpen(false);
                    }}
                  >
                    {Icon && <Icon className={`ml-3 size-5`} />}
                    {isMobileNav || (!isMinimized && !isMobileNav) ? (
                      <span className="mr-2 truncate">{item.title}</span>
                    ) : (
                      ''
                    )}
                  </Link>
                </TooltipTrigger>
                <TooltipContent
                  align="center"
                  side="right"
                  sideOffset={8}
                  className={!isMinimized ? 'hidden' : 'inline-block'}
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
