'use client';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuShortcut,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu';
import { currentUserAtom, tokenAtom } from '@/store';
import { useAtom } from 'jotai';
import { Link } from 'react-router-dom';

export function UserNav() {
  let [token, setToken] = useAtom(tokenAtom);
  let [user, setUser] = useAtom(currentUserAtom);

  function handleClick() {
    setToken("");
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" className="relative h-8 w-8 rounded-full">
          <Avatar className="h-8 w-8">
            <AvatarImage
              src={user?.UserName ?? ''}
              alt={user?.UserName ?? ''}
            />
            <AvatarFallback>{user?.UserName?.[0]}</AvatarFallback>
          </Avatar>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="w-56" align="end" forceMount>
        <DropdownMenuLabel className="font-normal">
          <div className="flex flex-col space-y-1">
            <p className="text-sm font-medium leading-none">
              {user?.UserName}
            </p>
            {/* <p className="text-xs leading-none text-muted-foreground">
              {user?.UserId}
            </p> */}
          </div>
        </DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuGroup>
          {/* <Link to={"/setting"}>
            <DropdownMenuItem>
              设置中心
            </DropdownMenuItem>
          </Link> */}
          {/* <Link to={"/user"}>
            <DropdownMenuItem>
              个人中心
            </DropdownMenuItem>
          </Link> */}
        </DropdownMenuGroup>
        {/* <DropdownMenuSeparator /> */}
        <DropdownMenuItem onClick={() => handleClick()}>
          退出登录
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
