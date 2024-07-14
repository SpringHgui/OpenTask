import { Button } from "@/components/ui/button"
import { ArrowLeft, X } from "lucide-react";
import { MouseEventHandler, ReactNode } from "react";

export function SubpageBack({ onBack, children, title, showClose, onClose }
    : { onBack?: MouseEventHandler | undefined, onClose?: MouseEventHandler | undefined, title: string, showClose?: boolean, children: ReactNode }) {
    return <div>
        <div className=" flex flex-row items-center " >
            <Button variant="secondary" size="icon" className=" rounded-full" onClick={onBack}>
                <ArrowLeft className="h-4 w-4" />
            </Button>
            <div className="flex flex-row flex-1 items-center">
                <h3 className="flex-1 pl-2 text-xl font-bold inline-block">{title}</h3>
                {showClose
                    && <Button variant="secondary" size="icon" onClick={onClose} className=" rounded-full float-right">
                        <X className="h-4 w-4" />
                    </Button>
                }
            </div>
        </div>
        {children}
    </div>
}