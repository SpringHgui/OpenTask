import { Shapes } from "lucide-react";
import { Link } from "react-router-dom";

export function Logo() {
    // const [minimized, handleToggle] = useAtom(isMinimized);
    return <>
        <Link to={"/"} replace={true} className="relative z-20 flex items-center text-lg font-medium"> <Shapes />
            {/* {!minimized && "OpenTask"} */}
            OpenTask
        </Link>
    </>
}