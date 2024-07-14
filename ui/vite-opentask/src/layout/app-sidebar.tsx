import { memo } from "react";
import { Separator } from "@/components/ui/separator"
import { SidebarNav, SidebarNavProps } from "@/components/sidebar/sidebar";
import { sideRoutes } from "@/routes/modules/side";
import { ChevronLeft, CircleUser } from "lucide-react";
import { generatePath, useParams } from "react-router-dom";
import { bottomRoutes } from "@/routes/modules/bottom";
import { cn } from "@/lib/utils";
import { tr } from "@faker-js/faker";
import { useAtom } from "jotai";
import { isMinimized } from "@/store/useSiderBar";
import { Logo } from "@/components/logo";

const Sidear = memo(() => {
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

    const fixedNavItems: SidebarNavProps = {
        items: bottomRoutes.map((router) => {
            return {
                title: router.meta?.title ?? "",
                href: router.path && generatePath(router.path, params),
                icon: router.meta?.Icon,
                blank: router.meta?.blank,
                sheet: router.element
            }
        })
    }
    const [minimized, handleToggle] = useAtom(isMinimized);

    return (
        <aside className="border-r hidden md:block"  >
            <div className='h-full ' style={{ overflow: 'hidden auto' }}>
                <nav
                    className={cn(
                        `relative h-screen`,
                        'duration-300',
                        !minimized ? 'w-56' : 'w-[72px]',
                    )}
                >
                    <ChevronLeft
                        className={cn(
                            'absolute -right-3 top-20 cursor-pointer rounded-full border bg-background text-3xl text-foreground',
                            !isMinimized && 'rotate-180'
                        )}
                        onClick={() => handleToggle(!minimized)}
                    />
                    <div className="space-y-4 py-4">
                        <div className="px-3 py-2">
                            <div className="mt-3 space-y-1">
                                <SidebarNav className='flex-1 layout-sider-menu' style={{ overflow: 'hidden auto' }} items={sidebarNavItems.items}></SidebarNav>
                                {/* <Separator /> */}
                            </div>
                            {/* <div className="mt-3 space-y-1">
                                <SidebarNav className='flex-1 layout-sider-menu' style={{ overflow: 'hidden auto' }} items={fixedNavItems.items}></SidebarNav>
                            </div> */}
                        </div>
                    </div>
                </nav>
            </div>
        </aside >
    );
});

export default Sidear;