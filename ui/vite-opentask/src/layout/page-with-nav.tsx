import { useState } from "react";
import { LinkInfo, Nav } from "../components/nav/nav";

export interface PageWithNavProps {
    Content: JSX.Element,
    links: LinkInfo[]
}

export default function PageWithNav({ links }: PageWithNavProps) {
    let [index, setIndex] = useState(0);

    return <div className="flex flex-row h-full">
        <aside className="lg:w-52 h-full">
            <Nav isCollapsed={false}
                links={links}  />
        </aside>
        <div className="flex-1">{links[index].Component}</div>
    </div>
}