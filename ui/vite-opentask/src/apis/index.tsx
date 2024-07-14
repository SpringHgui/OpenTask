import { QueryClient, keepPreviousData } from "@tanstack/react-query";

import { useQuery } from "@tanstack/react-query";
import config from "./config";
import {
  ClientApi,
  ListClientsRequest,
  ListLogsRequest,
  ListTaskInfosRequest,
  TaskInfoApi,
  TaskLogApi,
} from "@/apis-gen";
import { PaginationState } from "@tanstack/react-table";

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      gcTime: 1000 * 60 * 60 * 24, // 24 hours
    },
  },
});

export function useTaskInfos(request: ListTaskInfosRequest) {
  return useQuery({
    queryKey: ["listTaskInfos", request.pageNumber, request.pageSize],
    queryFn: () => new TaskInfoApi(config).listTaskInfos(request),
  });
}
 