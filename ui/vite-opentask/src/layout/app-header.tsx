
import { ModeToggle } from "@/components/mode-toggle";
import { cn } from "@/lib/utils";
import { MobileSidebar } from "./mobile-sidebar";
import { Logo } from "@/components/logo";
import { UserNav } from "./user-nav";

export default function Header({ title }: { title: string | undefined }) {

    return <>
        <div className="border-b bg-background/95 backdrop-blur">
            <nav className="flex h-14 items-center justify-between px-4">
                <Logo></Logo>
                <div className={cn('block lg:!hidden')}>
                    <MobileSidebar />
                </div>
                <div className="flex items-center gap-2">
                    <UserNav />
                    <ModeToggle />
                </div>
            </nav>
        </div>

    </>
}