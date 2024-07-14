import { Separator } from "@/components/ui/separator";
import Markdown, { Components } from "react-markdown";
import { Link } from "react-router-dom";
import remarkGfm from "remark-gfm";

export const About = () => {
  const markdown = `
# 简介
OpenTask 新一代分布式任务调度框架，调度中心采用去中心化的架构设计，调度中心高可用的同时又追求最小的性能损耗。
# 特性
- crontab时间类型的定时任务
- 调度中心去中心化集群部署，支持高可用
- 多语言支持，工作节点与调度中心采用MQTT协议，常用开发语言友好对接。
- 友好的云原生部署，推荐使用k8s进行部署，方便进行扩缩容。
- 待补充...
# 仓库
[Github](https://github.com/SpringHgui/OpenTask)  
# 协议
[MIT](https://github.com/SpringHgui/OpenTask/blob/master/LICENSE)

  `;

  const components: Components = {
    h1: ({ children }) => (
      <>
        <h1 className="text-2xl font-semibold pt-8">{children}</h1>
        <Separator className="my-4" />
      </>
    ),
    h3: ({ children }) => {
      return <h3 className="text-xl font-semibold">{children}</h3>;
    },
    p: ({ children }) => {
      return <p className="text-sm leading-relaxed">{children}</p>;
    },
    a: ({ href, children }) => (
      <Link
        to={href || "#"}
        target="_blank"
        className="underline underline-offset-4 hover:text-primary"
      >
        {children}
      </Link>
    ),
  };

  return (
    <div>
      <Markdown remarkPlugins={[remarkGfm]} components={components}>
        {markdown}
      </Markdown>
    </div>
  );
};
