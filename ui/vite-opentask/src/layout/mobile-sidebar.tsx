'use client';
import { DashboardNav } from '@/components/dashboard-nav';
import { SidebarNavProps } from '@/components/sidebar/sidebar';
import { Sheet, SheetContent, SheetTrigger } from '@/components/ui/sheet';

import { sideRoutes } from '@/routes/modules/side';
import { MenuIcon } from 'lucide-react';
import { useState } from 'react';
import { generatePath, useParams } from 'react-router-dom';

// import { Playlist } from "../data/playlists";

interface SidebarProps extends React.HTMLAttributes<HTMLDivElement> {
  // playlists: Playlist[];
}

export function MobileSidebar({ className }: SidebarProps) {
  const [open, setOpen] = useState(false);
  const params = useParams();
  const sidebarNavItems: SidebarNavProps = {
    items: sideRoutes.map((router) => {
      return {
        title: router.meta?.title ?? "",
        href: router.path && generatePath(router.path, params),
        icon: router.meta?.Icon,
        blank: router.meta?.blank,
      }
    })
  }

  return (
    <>
      <Sheet open={open} onOpenChange={setOpen}>
        <SheetTrigger asChild>
          <MenuIcon />
        </SheetTrigger>
        <SheetContent side="left" className="!px-0">
          <div className="space-y-4 py-4">
            <div className="px-3 py-2">
              <h2 className="mb-2 px-4 text-lg font-semibold tracking-tight">
                Overview
              </h2>
              <div className="space-y-1">
                <DashboardNav
                  items={sidebarNavItems.items}
                  isMobileNav={true}
                  setOpen={setOpen}
                />
              </div>
            </div>
          </div>
        </SheetContent>
      </Sheet>
    </>
  );
}
